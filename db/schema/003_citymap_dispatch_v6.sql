-- V6 schema additions (draft / scaffold)

create table if not exists city_nodes (
    node_id text primary key,
    type_id text not null,
    label_key text not null,
    x real not null,
    y real not null
);

create table if not exists city_connections (
    connection_id text primary key,
    from_node_id text not null,
    to_node_id text not null,
    type_id text not null
);

create table if not exists session_timeline (
    timeline_item_id text primary key,
    session_id text not null,
    actor_role text not null,
    severity text not null,
    message text not null,
    is_bot integer not null default 0,
    created_utc text not null
);
