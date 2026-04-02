-- v21 review build schema starter
create table if not exists ReviewBuildPackage
(
    Id nvarchar(64) not null primary key,
    BuildName nvarchar(200) not null,
    MissionId nvarchar(100) not null,
    CaptureModeEnabled bit not null
);

create table if not exists ReviewBuildChecklistItem
(
    Id int identity(1,1) not null primary key,
    BuildId nvarchar(64) not null,
    [Key] nvarchar(100) not null,
    Title nvarchar(200) not null,
    Severity nvarchar(50) not null,
    Required bit not null,
    Passed bit not null
);
