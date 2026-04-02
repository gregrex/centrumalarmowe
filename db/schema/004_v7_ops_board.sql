-- v7 scaffold: active incident board, route preview cache, shared actions
create table if not exists active_incidents (
    incident_id text primary key,
    incident_type_id text not null,
    severity text not null,
    node_id text not null,
    status text not null,
    pressure integer not null default 0,
    shared_action_required integer not null default 0
);

create table if not exists shared_actions (
    shared_action_id text primary key,
    incident_id text not null,
    action_type text not null,
    requested_by_role text not null,
    timeout_seconds integer not null,
    allow_bot_assist integer not null default 0
);
