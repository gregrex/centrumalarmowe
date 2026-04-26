-- v24 internal test and liveops scaffold
create table if not exists InternalTestBuilds (
  Id integer generated always as identity primary key,
  BuildId varchar(100) not null,
  Status varchar(50) not null,
  CreatedUtc timestamptz not null default now()
);
