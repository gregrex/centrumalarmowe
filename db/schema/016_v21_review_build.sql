-- v21 review build schema starter
create table if not exists ReviewBuildPackage
(
    Id varchar(64) not null primary key,
    BuildName varchar(200) not null,
    MissionId varchar(100) not null,
    CaptureModeEnabled boolean not null
);

create table if not exists ReviewBuildChecklistItem
(
    Id integer generated always as identity primary key,
    BuildId varchar(64) not null,
    KeyName varchar(100) not null,
    Title varchar(200) not null,
    Severity varchar(50) not null,
    Required boolean not null,
    Passed boolean not null
);
