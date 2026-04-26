-- v23 android preview / telemetry / review
create table if not exists AndroidPreviewBuild (
    BuildId varchar(64) not null primary key,
    Version varchar(32) not null,
    MissionId varchar(64) not null,
    Status varchar(32) not null,
    CreatedUtc timestamptz not null default now()
);

create table if not exists ReleaseReadinessChecklist (
    Id varchar(64) not null primary key,
    Label varchar(256) not null,
    State varchar(32) not null,
    Severity varchar(16) not null
);
