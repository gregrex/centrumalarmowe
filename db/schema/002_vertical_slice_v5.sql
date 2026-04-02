
-- v5 additions for quick play and reporting

create table if not exists dispatch_units (
    id varchar(40) primary key,
    unit_type varchar(60) not null,
    status varchar(40) not null,
    zone varchar(80) not null,
    session_id uuid null
);

create table if not exists incidents (
    id varchar(80) primary key,
    session_id uuid not null,
    category varchar(40) not null,
    severity varchar(40) not null,
    status varchar(40) not null,
    zone varchar(80) not null,
    title varchar(200) not null
);

create table if not exists session_reports (
    id uuid primary key,
    session_id uuid not null,
    score_grade varchar(4) not null,
    pressure_peak int not null,
    created_at timestamp not null default current_timestamp
);
