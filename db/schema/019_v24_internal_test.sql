-- v24 internal test and liveops scaffold
create table if not exists InternalTestBuilds (
  Id int identity(1,1) primary key,
  BuildId nvarchar(100) not null,
  Status nvarchar(50) not null,
  CreatedUtc datetime2 not null default sysutcdatetime()
);
