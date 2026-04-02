-- v15 playable runtime scaffold
create table if not exists playable_mission_runtime (
    mission_id text primary key,
    pressure integer not null,
    pressure_band text not null,
    traffic_state text not null,
    weather_state text not null,
    media_state text not null
);

create table if not exists objective_state_transition (
    objective_id text not null,
    from_state text not null,
    to_state text not null,
    trigger_key text not null,
    progress_delta integer not null
);
