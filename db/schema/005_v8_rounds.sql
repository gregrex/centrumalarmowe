-- v8 round runtime and overlay seed-friendly schema placeholder

create table if not exists round_runtime_state (
    session_id nvarchar(64) not null,
    round_id nvarchar(64) not null,
    tick int not null,
    elapsed_seconds int not null,
    phase nvarchar(64) not null,
    active_human_players int not null,
    active_bot_players int not null,
    open_incidents int not null,
    shared_actions_pending int not null
);

create table if not exists unit_runtime (
    session_id nvarchar(64) not null,
    unit_id nvarchar(64) not null,
    call_sign nvarchar(64) not null,
    unit_type nvarchar(64) not null,
    status nvarchar(64) not null,
    current_node_id nvarchar(64) not null,
    cooldown_seconds int not null,
    eta_seconds int not null,
    available bit not null,
    is_bot_backfilled bit not null
);

create table if not exists incident_delta_log (
    session_id nvarchar(64) not null,
    delta_id nvarchar(64) not null,
    incident_id nvarchar(64) not null,
    change_type nvarchar(64) not null,
    severity nvarchar(32) not null,
    status nvarchar(32) not null,
    timer_delta_seconds int not null,
    needs_attention bit not null,
    created_utc datetime2 not null
);
