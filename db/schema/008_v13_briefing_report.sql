-- V13 briefing + report
create table if not exists mission_briefings (
    mission_id text primary key,
    title_key text not null,
    difficulty text not null,
    estimated_minutes integer not null,
    weather_preset text not null,
    time_of_day text not null
);

create table if not exists post_round_reports (
    report_id integer generated always as identity primary key,
    mission_id text not null,
    grade_id text not null,
    score integer not null,
    stars integer not null,
    created_at text not null default CURRENT_TIMESTAMP
);
