using System.Collections.Concurrent;
using System.Text.Json;
using Alarm112.Application.Interfaces;
using Alarm112.Contracts;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Alarm112.Infrastructure.Persistence;

/// <summary>
/// PostgreSQL-backed session store.
/// Requires table: sessions (session_id TEXT PK, snapshot JSONB, updated_at TIMESTAMPTZ).
/// See db/schema/001_init.sql for schema.
/// Register via: builder.Services.AddSingleton&lt;ISessionStore&gt;(sp => new PostgresSessionStore(connStr, sp.GetRequiredService&lt;ILogger&lt;PostgresSessionStore&gt;&gt;()));
/// </summary>
public sealed class PostgresSessionStore : ISessionStore, IDisposable
{
    private readonly NpgsqlDataSource _dataSource;
    private readonly ILogger<PostgresSessionStore> _logger;
    // Local write-through cache: reduces DB round-trips for hot sessions
    private readonly ConcurrentDictionary<string, SessionSnapshotDto> _cache = new();

    private static readonly JsonSerializerOptions _json = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    public PostgresSessionStore(string connectionString, ILogger<PostgresSessionStore> logger)
    {
        _logger = logger;
        _dataSource = NpgsqlDataSource.Create(connectionString);
        EnsureTableExists();
    }

    public SessionSnapshotDto GetOrAdd(string sessionId, Func<string, SessionSnapshotDto> factory)
    {
        if (_cache.TryGetValue(sessionId, out var cached)) return cached;

        try
        {
            using var cmd = _dataSource.CreateCommand(
                "SELECT snapshot FROM sessions WHERE session_id = @id LIMIT 1");
            cmd.Parameters.AddWithValue("id", sessionId);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                var json = reader.GetString(0);
                var snapshot = JsonSerializer.Deserialize<SessionSnapshotDto>(json, _json)!;
                _cache[sessionId] = snapshot;
                return snapshot;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "PostgresSessionStore.GetOrAdd: DB read failed for session {SessionId}", sessionId);
        }

        // Not in DB — create via factory and persist
        var newSnapshot = factory(sessionId);
        Save(newSnapshot);
        return newSnapshot;
    }

    public void Save(SessionSnapshotDto snapshot)
    {
        _cache[snapshot.SessionId] = snapshot;

        try
        {
            var json = JsonSerializer.Serialize(snapshot, _json);
            using var cmd = _dataSource.CreateCommand(
                """
                INSERT INTO sessions (session_id, snapshot, updated_at)
                VALUES (@id, @snap::jsonb, now())
                ON CONFLICT (session_id) DO UPDATE
                  SET snapshot = EXCLUDED.snapshot,
                      updated_at = EXCLUDED.updated_at
                """);
            cmd.Parameters.AddWithValue("id", snapshot.SessionId);
            cmd.Parameters.AddWithValue("snap", json);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "PostgresSessionStore.Save: DB write failed for session {SessionId}", snapshot.SessionId);
        }
    }

    public IReadOnlyList<string> GetActiveSessionIds()
    {
        try
        {
            var ids = new List<string>();
            using var cmd = _dataSource.CreateCommand(
                "SELECT session_id FROM sessions WHERE updated_at > now() - INTERVAL '2 hours'");
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) ids.Add(reader.GetString(0));
            return ids;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "PostgresSessionStore.GetActiveSessionIds: DB read failed, falling back to cache");
            return _cache.Keys.ToList();
        }
    }

    private void EnsureTableExists()
    {
        try
        {
            using var cmd = _dataSource.CreateCommand(
                """
                CREATE TABLE IF NOT EXISTS sessions (
                    session_id  TEXT PRIMARY KEY,
                    snapshot    JSONB NOT NULL,
                    updated_at  TIMESTAMPTZ NOT NULL DEFAULT now()
                );
                CREATE INDEX IF NOT EXISTS idx_sessions_updated ON sessions(updated_at);
                """);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "PostgresSessionStore: could not ensure table exists (may already exist or no permissions)");
        }
    }

    public void Dispose() => _dataSource.Dispose();
}
