create table if not exists RealAndroidBuildAudit (
    Id integer primary key,
    BuildId text not null,
    MissionId text not null,
    BuildTarget text not null,
    FreezeLabel text not null,
    Status text not null,
    CreatedUtc text not null
);
