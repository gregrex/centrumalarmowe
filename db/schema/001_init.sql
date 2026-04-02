-- Starter schema for 112: Centrum Alarmowe

create table if not exists players (
    id uuid primary key,
    display_name varchar(120) not null,
    created_at timestamp not null default current_timestamp
);

create table if not exists sessions (
    id uuid primary key,
    mode varchar(50) not null,
    scenario_id varchar(100) not null,
    status varchar(50) not null,
    created_at timestamp not null default current_timestamp
);

create table if not exists session_members (
    id uuid primary key,
    session_id uuid not null,
    player_id uuid null,
    role_id varchar(50) not null,
    is_bot boolean not null default false,
    connection_status varchar(50) not null,
    foreign key (session_id) references sessions(id)
);

create table if not exists incident_templates (
    id varchar(100) primary key,
    name varchar(200) not null,
    category varchar(50) not null,
    base_priority int not null
);

create table if not exists telemetry_events (
    id uuid primary key,
    session_id uuid not null,
    event_name varchar(120) not null,
    role_id varchar(50) null,
    payload json null,
    created_at timestamp not null default current_timestamp
);
