insert into playable_mission_runtime (mission_id, pressure, pressure_band, traffic_state, weather_state, media_state)
values ('mission.01.05', 57, 'critical', 'traffic_heavy', 'weather_rain', 'media_spike');

insert into objective_state_transition (objective_id, from_state, to_state, trigger_key, progress_delta)
values
('obj.primary.01', 'active', 'progress', 'dispatch.medical.success', 25),
('obj.primary.01', 'progress', 'completed', 'incident.resolved', 75),
('obj.secondary.01', 'active', 'at_risk', 'time.exceeded.critical', -20);
