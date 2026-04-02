create table if not exists DemoFlowState (
    DemoFlowStateId text primary key,
    MissionId text not null,
    ActiveStepId text not null,
    BranchState text not null,
    UpdatedAtUtc text not null
);

create table if not exists RecoveryDecisionAudit (
    RecoveryDecisionAuditId text primary key,
    MissionId text not null,
    CardId text not null,
    OptionId text not null,
    OutcomeCode text not null,
    CreatedAtUtc text not null
);

create table if not exists FailBranchAudit (
    FailBranchAuditId text primary key,
    MissionId text not null,
    BranchId text not null,
    ReasonCode text not null,
    CreatedAtUtc text not null
);

create table if not exists ReportRoomState (
    ReportRoomStateId text primary key,
    MissionId text not null,
    VariantId text not null,
    RewardRevealId text not null,
    CreatedAtUtc text not null
);
