-- v19 near-final slice
create table if not exists runtime_scoreboards (
    id text primary key,
    mission_id text not null,
    result_state text not null,
    total_score integer not null,
    stars integer not null
);

create table if not exists retry_preparations (
    id text primary key,
    mission_id text not null,
    suggested_role text not null
);
