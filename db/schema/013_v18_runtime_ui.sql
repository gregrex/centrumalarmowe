-- V18 runtime UI, recovery and retry schema placeholder
create table if not exists runtime_ui_profile (
    id text primary key,
    mission_id text not null,
    result_state text not null,
    active_recovery_slot text null,
    created_utc text not null
);

create table if not exists recovery_trigger_profile (
    id text primary key,
    mission_id text not null,
    trigger_key text not null,
    severity text not null,
    role_key text not null,
    requires_immediate_attention integer not null default 0
);
