insert into mission_briefings (mission_id, title_key, difficulty, estimated_minutes, weather_preset, time_of_day)
values ('mission.01.03', 'mission.01.03.title', 'high', 14, 'storm_night', 'night')
on conflict (mission_id) do update
set title_key = excluded.title_key,
    difficulty = excluded.difficulty,
    estimated_minutes = excluded.estimated_minutes,
    weather_preset = excluded.weather_preset,
    time_of_day = excluded.time_of_day;

insert into post_round_reports (mission_id, grade_id, score, stars)
values ('mission.01.03', 'B', 82, 2);
