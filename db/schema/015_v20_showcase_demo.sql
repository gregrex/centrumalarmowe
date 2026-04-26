-- v20 showcase demo schema additions
create table if not exists ShowcaseMission (
    Id varchar(64) not null primary key,
    Title varchar(200) not null,
    RecommendedRole varchar(50) not null,
    EstimatedDurationSeconds int not null
);

create table if not exists OnboardingStep (
    Id varchar(64) not null primary key,
    FlowId varchar(64) not null,
    SortOrder int not null,
    Title varchar(120) not null
);
