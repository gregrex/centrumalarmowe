-- v16 live route / timer / chain runtime
create table if not exists demo_mission_runtime_polish (
    mission_id text primary key,
    theme text not null,
    scene_pack_id text not null,
    audio_pack_id text not null,
    continuity_tags_json text not null
);

create table if not exists route_runtime_segments (
    route_id text primary key,
    mission_id text not null,
    from_node_id text not null,
    to_node_id text not null,
    state text not null,
    eta_seconds integer not null,
    visual_style text not null
);
