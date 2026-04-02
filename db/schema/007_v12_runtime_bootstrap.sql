-- v12 runtime bootstrap schema draft
create table if not exists CampaignNodeRuntime
(
    MissionId text primary key,
    State text not null,
    Stars integer not null default 0,
    IsFocused integer not null default 0
);

create table if not exists RoleSelectionSlot
(
    MissionId text not null,
    RoleId text not null,
    State text not null,
    OccupantId text null,
    Difficulty text not null,
    IsRecommended integer not null default 0,
    IsLocked integer not null default 0,
    primary key (MissionId, RoleId)
);
