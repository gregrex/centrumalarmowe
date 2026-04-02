-- v20 showcase demo schema additions
create table if not exists ShowcaseMission (
    Id nvarchar(64) not null primary key,
    Title nvarchar(200) not null,
    RecommendedRole nvarchar(50) not null,
    EstimatedDurationSeconds int not null
);

create table if not exists OnboardingStep (
    Id nvarchar(64) not null primary key,
    FlowId nvarchar(64) not null,
    SortOrder int not null,
    Title nvarchar(120) not null
);
