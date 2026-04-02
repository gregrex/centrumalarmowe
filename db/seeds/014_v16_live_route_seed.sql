insert into demo_mission_runtime_polish (mission_id, theme, scene_pack_id, audio_pack_id, continuity_tags_json)
values ('mission.demo.16', 'rainy_evening', 'scene_pack.demo.01', 'audio_pack.demo.01', '["menu_city_night","runtime_rain","report_room_afterglow"]')
on conflict (mission_id) do nothing;

insert into route_runtime_segments (route_id, mission_id, from_node_id, to_node_id, state, eta_seconds, visual_style)
values
('route.demo.01', 'mission.demo.16', 'hub.ems', 'incident.bridge', 'clear', 42, 'pulse_blue'),
('route.demo.02', 'mission.demo.16', 'hub.fire', 'incident.mall', 'delayed', 67, 'pulse_amber'),
('route.demo.03', 'mission.demo.16', 'hub.police', 'incident.bridge', 'rerouted', 58, 'pulse_red')
on conflict (route_id) do nothing;
