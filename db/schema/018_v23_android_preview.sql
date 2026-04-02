-- v23 android preview / telemetry / review
create table if not exists AndroidPreviewBuild (
    BuildId nvarchar(64) not null primary key,
    Version nvarchar(32) not null,
    MissionId nvarchar(64) not null,
    Status nvarchar(32) not null,
    CreatedUtc datetime2 not null default sysutcdatetime()
);

create table if not exists ReleaseReadinessChecklist (
    Id nvarchar(64) not null primary key,
    Label nvarchar(256) not null,
    State nvarchar(32) not null,
    Severity nvarchar(16) not null
);
