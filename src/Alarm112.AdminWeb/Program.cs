using System.Text;

var builder = WebApplication.CreateBuilder(args);
var apiBase = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5080";

// Basic Auth credentials — read from config/env, never hardcode
var adminUser = builder.Configuration["AdminAuth:Username"] ?? "admin";
var adminPass = builder.Configuration["AdminAuth:Password"] ?? "admin112";

var app = builder.Build();

// Basic Auth middleware for all non-health routes
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value ?? string.Empty;

    // Health check is always public
    if (path.StartsWith("/health", StringComparison.OrdinalIgnoreCase))
    {
        await next();
        return;
    }

    // Check Authorization header
    if (!context.Request.Headers.TryGetValue("Authorization", out var authHeader) ||
        !authHeader.ToString().StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
    {
        context.Response.Headers["WWW-Authenticate"] = "Basic realm=\"112 Admin Panel\", charset=\"UTF-8\"";
        context.Response.StatusCode = 401;
        await context.Response.WriteAsync("Authentication required.");
        return;
    }

    try
    {
        var encoded = authHeader.ToString().Substring("Basic ".Length).Trim();
        var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(encoded));
        var parts = decoded.Split(':', 2);
        if (parts.Length != 2 || parts[0] != adminUser || parts[1] != adminPass)
        {
            context.Response.Headers["WWW-Authenticate"] = "Basic realm=\"112 Admin Panel\", charset=\"UTF-8\"";
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Invalid credentials.");
            return;
        }
    }
    catch
    {
        context.Response.StatusCode = 400;
        return;
    }

    await next();
});

app.MapGet("/health", () => Results.Ok(new { ok = true, service = "Alarm112.AdminWeb", version = "v26" }));

app.MapGet("/", () => Results.Content($$"""
<!DOCTYPE html>
<html lang="pl">
<head>
  <meta charset="utf-8"/>
  <meta name="viewport" content="width=device-width,initial-scale=1"/>
  <title>112 Centrum Alarmowe — Panel Admina</title>
  <style>
    /* ── Reset & base ───────────────────────────────────────── */
    *, *::before, *::after { box-sizing: border-box; margin: 0; padding: 0; }
    :root {
      --bg-base:      #080c12;
      --bg-surface:   #0f1420;
      --bg-elevated:  #161c2a;
      --bg-sidebar:   #0b0f1a;
      --accent:       #ff4b1f;
      --accent-dim:   #cc3c19;
      --accent-glow:  rgba(255,75,31,0.35);
      --role-op:      #38bdf8;
      --role-dis:     #fb923c;
      --role-coord:   #4ade80;
      --role-crisis:  #f87171;
      --ok:           #22c55e;
      --warn:         #f59e0b;
      --err:          #ef4444;
      --ok-bg:        rgba(34,197,94,0.12);
      --warn-bg:      rgba(245,158,11,0.12);
      --err-bg:       rgba(239,68,68,0.12);
      --border:       rgba(255,255,255,0.07);
      --text:         #d8e0ec;
      --text-dim:     #6b7a90;
      --sidebar-w:    220px;
      --header-h:     56px;
    }

    html, body { height: 100%; }
    body {
      font-family: 'Segoe UI', system-ui, Arial, sans-serif;
      background: var(--bg-base);
      color: var(--text);
      display: flex;
      flex-direction: column;
      min-height: 100vh;
      font-size: 14px;
      line-height: 1.5;
    }

    /* ── Header ─────────────────────────────────────────────── */
    .header {
      position: fixed; top: 0; left: 0; right: 0; z-index: 100;
      height: var(--header-h);
      background: rgba(8,12,18,0.96);
      border-bottom: 1px solid var(--border);
      backdrop-filter: blur(12px);
      display: flex; align-items: center; gap: 12px;
      padding: 0 20px;
    }
    .header-logo {
      display: flex; align-items: center; gap: 10px;
      text-decoration: none;
    }
    .logo-badge {
      background: var(--accent);
      color: #fff;
      font-size: 17px; font-weight: 800; letter-spacing: 0.5px;
      padding: 4px 10px;
      border-radius: 6px;
      box-shadow: 0 0 16px var(--accent-glow);
      animation: glow-flicker 4s ease-in-out infinite;
    }
    .logo-name {
      font-size: 15px; font-weight: 600; color: var(--text);
      letter-spacing: 0.2px;
    }
    .logo-name span { color: var(--text-dim); font-weight: 400; font-size: 12px; margin-left: 6px; }
    .header-spacer { flex: 1; }
    .header-status {
      display: flex; align-items: center; gap: 6px;
      font-size: 11px; color: var(--text-dim);
    }
    .status-dot {
      width: 8px; height: 8px; border-radius: 50%;
      background: var(--ok);
      box-shadow: 0 0 6px var(--ok);
      animation: pulse-dot 2s ease-in-out infinite;
    }
    .status-dot.warn  { background: var(--warn); box-shadow: 0 0 6px var(--warn); }
    .status-dot.err   { background: var(--err);  box-shadow: 0 0 6px var(--err); animation: pulse-dot 0.8s ease-in-out infinite; }
    #api-status-text { color: var(--text); font-weight: 500; }
    .header-ver {
      background: var(--bg-elevated); border: 1px solid var(--border);
      border-radius: 4px; padding: 2px 8px;
      font-size: 11px; color: var(--text-dim); font-family: monospace;
    }

    /* ── Layout ──────────────────────────────────────────────── */
    .layout {
      display: flex;
      padding-top: var(--header-h);
      min-height: 100vh;
    }

    /* ── Sidebar ─────────────────────────────────────────────── */
    .sidebar {
      width: var(--sidebar-w);
      min-height: calc(100vh - var(--header-h));
      background: var(--bg-sidebar);
      border-right: 1px solid var(--border);
      position: fixed; top: var(--header-h); left: 0; bottom: 0;
      overflow-y: auto;
      padding: 16px 0;
      display: flex; flex-direction: column; gap: 0;
    }
    .nav-section { padding: 8px 16px 4px; }
    .nav-label {
      font-size: 10px; font-weight: 700; letter-spacing: 1.2px;
      text-transform: uppercase; color: var(--text-dim);
    }
    .nav-item {
      display: flex; align-items: center; gap: 10px;
      padding: 8px 16px;
      font-size: 13px; color: var(--text-dim);
      text-decoration: none;
      border-left: 2px solid transparent;
      transition: all 0.15s;
      cursor: pointer;
    }
    .nav-item:hover { background: var(--bg-elevated); color: var(--text); border-left-color: var(--accent); }
    .nav-item.active { background: var(--bg-elevated); color: var(--text); border-left-color: var(--accent); }
    .nav-item .nav-icon { font-size: 15px; width: 20px; text-align: center; }
    .nav-item .nav-badge {
      margin-left: auto; font-size: 10px; background: var(--err); color: #fff;
      border-radius: 10px; padding: 1px 6px; font-weight: 700;
    }
    .nav-divider { border: none; border-top: 1px solid var(--border); margin: 8px 0; }
    .sidebar-footer {
      margin-top: auto; padding: 12px 16px;
      border-top: 1px solid var(--border);
      font-size: 11px; color: var(--text-dim);
    }
    .sidebar-footer a { color: var(--role-op); text-decoration: none; }

    /* ── Main content ────────────────────────────────────────── */
    .main {
      margin-left: var(--sidebar-w);
      flex: 1;
      padding: 24px;
      min-width: 0;
    }

    /* ── Page header ─────────────────────────────────────────── */
    .page-header {
      margin-bottom: 24px;
      animation: fade-in-up 0.3s ease-out;
    }
    .page-title { font-size: 22px; font-weight: 700; color: var(--text); }
    .page-subtitle { font-size: 12px; color: var(--text-dim); margin-top: 2px; }

    /* ── Metric strip ────────────────────────────────────────── */
    .metrics {
      display: grid;
      grid-template-columns: repeat(auto-fill, minmax(160px, 1fr));
      gap: 12px;
      margin-bottom: 28px;
    }
    .metric {
      background: var(--bg-surface);
      border: 1px solid var(--border);
      border-radius: 10px;
      padding: 14px 16px;
      position: relative;
      overflow: hidden;
      animation: fade-in-up 0.3s ease-out;
    }
    .metric::before {
      content: '';
      position: absolute; top: 0; left: 0; right: 0; height: 2px;
      background: var(--m-color, var(--accent));
    }
    .metric-label { font-size: 10px; text-transform: uppercase; letter-spacing: 1px; color: var(--text-dim); }
    .metric-value { font-size: 26px; font-weight: 700; color: var(--m-color, var(--text)); margin: 4px 0 2px; }
    .metric-sub   { font-size: 11px; color: var(--text-dim); }
    .metric-icon  { position: absolute; right: 14px; top: 50%; transform: translateY(-50%); font-size: 28px; opacity: 0.18; }

    /* ── Section label ───────────────────────────────────────── */
    .section-label {
      font-size: 11px; font-weight: 700; letter-spacing: 1.2px;
      text-transform: uppercase; color: var(--text-dim);
      margin-bottom: 12px; margin-top: 8px;
      display: flex; align-items: center; gap: 8px;
    }
    .section-label::after {
      content: ''; flex: 1; height: 1px; background: var(--border);
    }

    /* ── Cards grid ─────────────────────────────────────────── */
    .grid {
      display: grid;
      grid-template-columns: repeat(auto-fill, minmax(292px, 1fr));
      gap: 14px;
      margin-bottom: 32px;
    }
    .card {
      background: var(--bg-surface);
      border: 1px solid var(--border);
      border-radius: 12px;
      padding: 20px;
      position: relative;
      overflow: hidden;
      transition: border-color 0.2s, transform 0.15s, box-shadow 0.2s;
      animation: fade-in-up 0.35s ease-out;
    }
    .card:hover {
      border-color: rgba(255,255,255,0.14);
      transform: translateY(-1px);
      box-shadow: 0 8px 28px rgba(0,0,0,0.45);
    }
    .card-accent-line {
      position: absolute; top: 0; left: 0; right: 0; height: 2px;
      background: var(--c-color, var(--accent));
      box-shadow: 0 0 10px var(--c-color, var(--accent));
    }
    .card-header {
      display: flex; align-items: center; gap: 12px;
      margin-bottom: 10px;
    }
    .card-icon {
      width: 38px; height: 38px;
      background: var(--bg-elevated);
      border: 1px solid var(--border);
      border-radius: 9px;
      display: flex; align-items: center; justify-content: center;
      font-size: 18px;
      flex-shrink: 0;
    }
    .card-title { font-size: 14px; font-weight: 600; color: var(--text); }
    .card-tag {
      margin-left: auto; font-size: 10px;
      background: var(--bg-elevated); border: 1px solid var(--border);
      border-radius: 4px; padding: 2px 7px; color: var(--text-dim);
      font-family: monospace;
    }
    .card-desc { font-size: 12px; color: var(--text-dim); margin-bottom: 14px; line-height: 1.55; }
    .card-actions { display: flex; gap: 8px; flex-wrap: wrap; align-items: center; }

    /* ── Buttons ─────────────────────────────────────────────── */
    .btn {
      display: inline-flex; align-items: center; gap: 6px;
      padding: 7px 14px;
      border-radius: 7px;
      font-size: 12px; font-weight: 600;
      text-decoration: none;
      cursor: pointer;
      border: none;
      transition: all 0.15s;
      white-space: nowrap;
    }
    .btn-primary {
      background: var(--accent); color: #fff;
      box-shadow: 0 0 10px var(--accent-glow);
    }
    .btn-primary:hover { background: #ff6240; box-shadow: 0 0 18px var(--accent-glow); }
    .btn-ghost {
      background: var(--bg-elevated); color: var(--text);
      border: 1px solid var(--border);
    }
    .btn-ghost:hover { background: var(--bg-overlay); border-color: rgba(255,255,255,0.14); }
    .btn-link { background: none; color: var(--role-op); font-size: 11px; padding: 0; font-weight: 500; }
    .btn-link:hover { color: #7dd3fc; }

    /* ── Role cards ──────────────────────────────────────────── */
    .roles-grid {
      display: grid;
      grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
      gap: 12px;
      margin-bottom: 28px;
    }
    .role-card {
      background: var(--bg-surface);
      border: 1px solid var(--border);
      border-radius: 10px;
      padding: 16px;
      border-left: 3px solid var(--r-color);
      transition: box-shadow 0.2s;
    }
    .role-card:hover { box-shadow: 0 0 18px color-mix(in srgb, var(--r-color) 25%, transparent); }
    .role-name { font-size: 13px; font-weight: 700; color: var(--r-color); margin-bottom: 4px; }
    .role-status { font-size: 11px; color: var(--text-dim); display: flex; align-items: center; gap: 6px; }
    .role-dot { width: 7px; height: 7px; border-radius: 50%; background: var(--r-color); flex-shrink: 0; }

    /* ── Log feed ────────────────────────────────────────────── */
    .log-feed {
      background: var(--bg-surface);
      border: 1px solid var(--border);
      border-radius: 12px;
      overflow: hidden;
      margin-bottom: 28px;
    }
    .log-header {
      padding: 12px 16px;
      border-bottom: 1px solid var(--border);
      font-size: 12px; font-weight: 600; color: var(--text);
      display: flex; align-items: center; gap: 8px;
    }
    .log-header-dot { width: 7px; height: 7px; border-radius: 50%; background: var(--ok); animation: pulse-dot 2s infinite; }
    .log-body { padding: 0; max-height: 180px; overflow-y: auto; font-family: monospace; font-size: 11.5px; }
    .log-row {
      display: flex; gap: 10px;
      padding: 7px 16px;
      border-bottom: 1px solid rgba(255,255,255,0.03);
      align-items: flex-start;
    }
    .log-row:last-child { border-bottom: none; }
    .log-time { color: var(--text-dim); flex-shrink: 0; }
    .log-tag  { width: 60px; flex-shrink: 0; }
    .log-tag.ok   { color: var(--ok); }
    .log-tag.warn { color: var(--warn); }
    .log-tag.err  { color: var(--err); }
    .log-tag.info { color: var(--role-op); }
    .log-msg { color: var(--text); }

    /* ── Scan-line overlay ───────────────────────────────────── */
    .scanline-overlay {
      position: fixed; inset: 0; pointer-events: none; z-index: 0;
      background: repeating-linear-gradient(
        0deg,
        rgba(0,0,0,0) 0px, rgba(0,0,0,0) 3px,
        rgba(0,0,0,0.04) 3px, rgba(0,0,0,0.04) 4px
      );
    }

    /* ── Animations ──────────────────────────────────────────── */
    @keyframes pulse-dot {
      0%,100% { opacity:1; transform:scale(1); }
      50%      { opacity:.45; transform:scale(.7); }
    }
    @keyframes fade-in-up {
      from { opacity:0; transform:translateY(7px); }
      to   { opacity:1; transform:translateY(0); }
    }
    @keyframes glow-flicker {
      0%,100% { box-shadow:0 0 16px var(--accent-glow); }
      45%      { box-shadow:0 0 8px var(--accent-glow); }
      50%      { box-shadow:0 0 4px var(--accent-glow); }
      55%      { box-shadow:0 0 12px var(--accent-glow); }
    }

    /* ── Scrollbar ───────────────────────────────────────────── */
    ::-webkit-scrollbar { width: 5px; }
    ::-webkit-scrollbar-track { background: transparent; }
    ::-webkit-scrollbar-thumb { background: rgba(255,255,255,0.1); border-radius: 3px; }
    ::-webkit-scrollbar-thumb:hover { background: rgba(255,255,255,0.2); }

    /* ── Responsive ─────────────────────────────────────────── */
    @media(max-width:768px) {
      :root { --sidebar-w: 0px; }
      .sidebar { display: none; }
      .main { margin-left: 0; padding: 16px; }
    }
  </style>
</head>
<body>
<div class="scanline-overlay"></div>

<!-- ── HEADER ─────────────────────────────────────────────── -->
<header class="header">
  <a class="header-logo" href="/">
    <span class="logo-badge">112</span>
    <span class="logo-name">Centrum Alarmowe <span>Panel Admina</span></span>
  </a>
  <div class="header-spacer"></div>
  <div class="header-status">
    <span class="status-dot" id="api-dot"></span>
    <span id="api-status-text">sprawdzam…</span>
  </div>
  <span class="header-ver">v26</span>
</header>

<!-- ── LAYOUT ─────────────────────────────────────────────── -->
<div class="layout">

  <!-- ── SIDEBAR ─────────────────────────────────────────── -->
  <nav class="sidebar">
    <div class="nav-section"><span class="nav-label">System</span></div>
    <a class="nav-item active" href="/">
      <span class="nav-icon">🏠</span> Dashboard
    </a>
    <a class="nav-item" href="{{apiBase}}/swagger" target="_blank">
      <span class="nav-icon">📘</span> Swagger UI
    </a>
    <a class="nav-item" href="{{apiBase}}/health" target="_blank">
      <span class="nav-icon">❤️</span> Health Check
    </a>
    <hr class="nav-divider"/>

    <div class="nav-section"><span class="nav-label">Content</span></div>
    <a class="nav-item" href="{{apiBase}}/api/reference-data" target="_blank">
      <span class="nav-icon">📦</span> Reference Data
    </a>
    <a class="nav-item" href="{{apiBase}}/api/content/validate" target="_blank">
      <span class="nav-icon">✅</span> Walidacja
    </a>
    <hr class="nav-divider"/>

    <div class="nav-section"><span class="nav-label">Sesje</span></div>
    <a class="nav-item" href="{{apiBase}}/api/home-hub" target="_blank">
      <span class="nav-icon">🏘️</span> Home Hub
    </a>
    <a class="nav-item" href="{{apiBase}}/api/city-map" target="_blank">
      <span class="nav-icon">🗺️</span> City Map
    </a>
    <a class="nav-item" href="{{apiBase}}/api/mission-briefing/demo" target="_blank">
      <span class="nav-icon">📋</span> Mission Briefing
    </a>
    <a class="nav-item" href="{{apiBase}}/api/quickplay/bootstrap" target="_blank">
      <span class="nav-icon">⚡</span> Quickplay
    </a>
    <hr class="nav-divider"/>

    <div class="nav-section"><span class="nav-label">Demo</span></div>
    <a class="nav-item" href="{{apiBase}}/api/showcase-demo/demo" target="_blank">
      <span class="nav-icon">🎬</span> Showcase Demo
    </a>
    <a class="nav-item" href="{{apiBase}}/api/real-android-build/demo" target="_blank">
      <span class="nav-icon">📱</span> Android Build
    </a>

    <div class="sidebar-footer">
      API: <a href="{{apiBase}}" target="_blank">{{apiBase}}</a>
    </div>
  </nav>

  <!-- ── MAIN ─────────────────────────────────────────────── -->
  <main class="main">
    <div class="page-header">
      <div class="page-title">Dashboard operacyjny</div>
      <div class="page-subtitle">Centrum Alarmowe v26 · backend scaffold · InMemory store</div>
    </div>

    <!-- METRYKI -->
    <div class="metrics">
      <div class="metric" style="--m-color:var(--ok)">
        <div class="metric-label">Status API</div>
        <div class="metric-value" id="metric-api-status">—</div>
        <div class="metric-sub" id="metric-api-sub">oczekiwanie…</div>
        <div class="metric-icon">🛰️</div>
      </div>
      <div class="metric" style="--m-color:var(--role-op)">
        <div class="metric-label">Sesje aktywne</div>
        <div class="metric-value">0</div>
        <div class="metric-sub">InMemory store</div>
        <div class="metric-icon">🎮</div>
      </div>
      <div class="metric" style="--m-color:var(--role-dis)">
        <div class="metric-label">Content bundle</div>
        <div class="metric-value" id="metric-bundle">—</div>
        <div class="metric-sub">reference-data v26</div>
        <div class="metric-icon">📦</div>
      </div>
      <div class="metric" style="--m-color:var(--role-coord)">
        <div class="metric-label">SignalR hub</div>
        <div class="metric-value">LIVE</div>
        <div class="metric-sub">/hubs/session</div>
        <div class="metric-icon">📡</div>
      </div>
      <div class="metric" style="--m-color:var(--role-crisis)">
        <div class="metric-label">Bot Director</div>
        <div class="metric-value">4 role</div>
        <div class="metric-sub">AI fallback aktywny</div>
        <div class="metric-icon">🤖</div>
      </div>
    </div>

    <!-- ROLE STATUS -->
    <div class="section-label">Role w systemie</div>
    <div class="roles-grid">
      <div class="role-card" style="--r-color:var(--role-op)">
        <div class="role-name">Call Operator</div>
        <div class="role-status"><span class="role-dot"></span>AI fallback gotowy</div>
      </div>
      <div class="role-card" style="--r-color:var(--role-dis)">
        <div class="role-name">Dispatcher</div>
        <div class="role-status"><span class="role-dot"></span>AI fallback gotowy</div>
      </div>
      <div class="role-card" style="--r-color:var(--role-coord)">
        <div class="role-name">Ops Coordinator</div>
        <div class="role-status"><span class="role-dot"></span>AI fallback gotowy</div>
      </div>
      <div class="role-card" style="--r-color:var(--role-crisis)">
        <div class="role-name">Crisis Officer</div>
        <div class="role-status"><span class="role-dot"></span>AI fallback gotowy</div>
      </div>
    </div>

    <!-- ENDPOINTS -->
    <div class="section-label">Endpointy API</div>
    <div class="grid">

      <div class="card">
        <div class="card-accent-line" style="--c-color:var(--ok)"></div>
        <div class="card-header">
          <div class="card-icon">❤️</div>
          <div class="card-title">Health Check</div>
          <span class="card-tag">GET</span>
        </div>
        <div class="card-desc">Status serwera backend. Zwraca JSON z polem <code>ok</code> i wersją.</div>
        <div class="card-actions">
          <a class="btn btn-primary" href="{{apiBase}}/health" target="_blank">GET /health</a>
          <a class="btn btn-link" href="{{apiBase}}/swagger" target="_blank">→ Swagger UI</a>
        </div>
      </div>

      <div class="card">
        <div class="card-accent-line" style="--c-color:var(--role-dis)"></div>
        <div class="card-header">
          <div class="card-icon">✅</div>
          <div class="card-title">Walidacja contentu</div>
          <span class="card-tag">GET</span>
        </div>
        <div class="card-desc">Sprawdza poprawność wszystkich JSON bundli content pipeline.</div>
        <div class="card-actions">
          <a class="btn btn-primary" href="{{apiBase}}/api/content/validate" target="_blank">Waliduj</a>
        </div>
      </div>

      <div class="card">
        <div class="card-accent-line" style="--c-color:var(--role-crisis)"></div>
        <div class="card-header">
          <div class="card-icon">🎮</div>
          <div class="card-title">Sesje demo</div>
          <span class="card-tag">POST</span>
        </div>
        <div class="card-desc">Utwórz lub przeglądaj sesje demo. Pełne POST dostępne w Swagger.</div>
        <div class="card-actions">
          <a class="btn btn-ghost" href="{{apiBase}}/api/sessions/demo" target="_blank">GET /sessions/demo</a>
          <a class="btn btn-link" href="{{apiBase}}/swagger#/Sessions" target="_blank">→ Swagger</a>
        </div>
      </div>

      <div class="card">
        <div class="card-accent-line" style="--c-color:var(--role-op)"></div>
        <div class="card-header">
          <div class="card-icon">📦</div>
          <div class="card-title">Reference Data</div>
          <span class="card-tag">GET</span>
        </div>
        <div class="card-desc">Pełny bundle danych referencyjnych v26: role, incydenty, jednostki, mapy.</div>
        <div class="card-actions">
          <a class="btn btn-primary" href="{{apiBase}}/api/reference-data" target="_blank">GET /reference-data</a>
        </div>
      </div>

      <div class="card">
        <div class="card-accent-line" style="--c-color:var(--role-coord)"></div>
        <div class="card-header">
          <div class="card-icon">🏘️</div>
          <div class="card-title">Home Hub</div>
          <span class="card-tag">GET</span>
        </div>
        <div class="card-desc">Dane ekranu głównego gracza: profil, kampania, quickplay, powiadomienia.</div>
        <div class="card-actions">
          <a class="btn btn-primary" href="{{apiBase}}/api/home-hub" target="_blank">GET /home-hub</a>
        </div>
      </div>

      <div class="card">
        <div class="card-accent-line" style="--c-color:var(--role-op)"></div>
        <div class="card-header">
          <div class="card-icon">🗺️</div>
          <div class="card-title">City Map</div>
          <span class="card-tag">GET</span>
        </div>
        <div class="card-desc">Mapa miasta z węzłami, połączeniami i aktualnymi incydentami.</div>
        <div class="card-actions">
          <a class="btn btn-primary" href="{{apiBase}}/api/city-map" target="_blank">GET /city-map</a>
        </div>
      </div>

      <div class="card">
        <div class="card-accent-line" style="--c-color:var(--warn)"></div>
        <div class="card-header">
          <div class="card-icon">📋</div>
          <div class="card-title">Mission Briefing</div>
          <span class="card-tag">GET</span>
        </div>
        <div class="card-desc">Briefing misji showcase — pełny pakiet danych briefingu misji demo.</div>
        <div class="card-actions">
          <a class="btn btn-primary" href="{{apiBase}}/api/mission-briefing/demo" target="_blank">GET /mission-briefing</a>
        </div>
      </div>

      <div class="card">
        <div class="card-accent-line" style="--c-color:var(--role-coord)"></div>
        <div class="card-header">
          <div class="card-icon">⚡</div>
          <div class="card-title">Quickplay Lobby</div>
          <span class="card-tag">GET</span>
        </div>
        <div class="card-desc">Bootstrap quickplay session — natychmiastowy start z AI botami.</div>
        <div class="card-actions">
          <a class="btn btn-primary" href="{{apiBase}}/api/quickplay/bootstrap" target="_blank">Bootstrap</a>
        </div>
      </div>

      <div class="card">
        <div class="card-accent-line" style="--c-color:var(--text-dim)"></div>
        <div class="card-header">
          <div class="card-icon">📱</div>
          <div class="card-title">Android Build Status</div>
          <span class="card-tag">GET</span>
        </div>
        <div class="card-desc">Status buildu Android preview v26 — RC build metadata.</div>
        <div class="card-actions">
          <a class="btn btn-ghost" href="{{apiBase}}/api/real-android-build/demo" target="_blank">GET status</a>
        </div>
      </div>

      <div class="card">
        <div class="card-accent-line" style="--c-color:var(--accent)"></div>
        <div class="card-header">
          <div class="card-icon">🎬</div>
          <div class="card-title">Showcase Demo Flow</div>
          <span class="card-tag">GET</span>
        </div>
        <div class="card-desc">Kompletny flow showcase — misja demo operator + dispatcher end-to-end.</div>
        <div class="card-actions">
          <a class="btn btn-primary" href="{{apiBase}}/api/showcase-demo/demo" target="_blank">GET showcase</a>
        </div>
      </div>

    </div><!-- /grid -->

    <!-- LOG FEED -->
    <div class="section-label">Log systemowy</div>
    <div class="log-feed">
      <div class="log-header">
        <span class="log-header-dot"></span>
        Live feed — aktywność panelu
      </div>
      <div class="log-body" id="log-body">
        <div class="log-row">
          <span class="log-time" id="ts-boot"></span>
          <span class="log-tag info">[BOOT]</span>
          <span class="log-msg">AdminWeb v26 załadowany</span>
        </div>
        <div class="log-row">
          <span class="log-time"></span>
          <span class="log-tag info">[SYS]</span>
          <span class="log-msg">Sprawdzanie statusu API: {{apiBase}}/health</span>
        </div>
      </div>
    </div>

  </main>
</div><!-- /layout -->

<script>
  const API = '{{apiBase}}';
  const dot = document.getElementById('api-dot');
  const statusText = document.getElementById('api-status-text');
  const metricStatus = document.getElementById('metric-api-status');
  const metricSub    = document.getElementById('metric-api-sub');
  const metricBundle = document.getElementById('metric-bundle');
  const logBody = document.getElementById('log-body');
  document.getElementById('ts-boot').textContent = new Date().toLocaleTimeString('pl-PL');

  function addLog(level, tag, msg) {
    const row = document.createElement('div');
    row.className = 'log-row';
    row.innerHTML = `<span class="log-time">${new Date().toLocaleTimeString('pl-PL')}</span>
      <span class="log-tag ${level}">[${tag}]</span>
      <span class="log-msg">${msg}</span>`;
    logBody.appendChild(row);
    logBody.scrollTop = logBody.scrollHeight;
  }

  async function checkApi() {
    try {
      const r = await fetch(API + '/health', { signal: AbortSignal.timeout(4000) });
      const j = await r.json();
      dot.className = 'status-dot';
      statusText.textContent = 'API online';
      metricStatus.textContent = 'OK';
      metricStatus.style.color = 'var(--ok)';
      metricSub.textContent = j.version ?? 'v26';
      addLog('ok', 'HEALTH', `OK · ${j.service ?? 'Alarm112.Api'} · ${j.version ?? ''}`);
    } catch(e) {
      dot.className = 'status-dot err';
      statusText.textContent = 'API offline';
      metricStatus.textContent = 'ERR';
      metricStatus.style.color = 'var(--err)';
      metricSub.textContent = 'brak połączenia';
      addLog('err', 'HEALTH', `Brak odpowiedzi: ${e.message}`);
    }
  }

  async function checkBundle() {
    try {
      const r = await fetch(API + '/api/reference-data', { signal: AbortSignal.timeout(5000) });
      if (r.ok) {
        metricBundle.textContent = 'OK';
        metricBundle.style.color = 'var(--ok)';
        addLog('ok', 'BUNDLE', 'reference-data v26 dostępny');
      } else {
        metricBundle.textContent = r.status;
        metricBundle.style.color = 'var(--warn)';
        addLog('warn', 'BUNDLE', `HTTP ${r.status}`);
      }
    } catch(e) {
      metricBundle.textContent = '—';
      addLog('warn', 'BUNDLE', `Niedostępny: ${e.message}`);
    }
  }

  checkApi();
  checkBundle();
  setInterval(checkApi, 30000);
</script>
</body>
</html>
""", "text/html"));

app.Run();
