-- V14 full mission runtime scaffold
create table if not exists MissionScripts (
    MissionId text not null,
    StepOrder integer not null,
    AtSeconds integer not null,
    EventName text not null,
    primary key (MissionId, StepOrder)
);

create table if not exists MissionOutcomeDefinitions (
    OutcomeId text primary key,
    LabelKey text not null,
    ScoreDelta integer not null default 0,
    CityStabilityDelta integer not null default 0
);
