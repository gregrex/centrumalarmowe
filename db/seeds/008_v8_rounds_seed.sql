insert into round_runtime_state(session_id, round_id, tick, elapsed_seconds, phase, active_human_players, active_bot_players, open_incidents, shared_actions_pending)
values ('DEMO112', 'round.demo.001', 12, 44, 'round.phase.dispatch-window', 2, 2, 3, 1);

insert into unit_runtime(session_id, unit_id, call_sign, unit_type, status, current_node_id, cooldown_seconds, eta_seconds, available, is_bot_backfilled)
values
('DEMO112', 'unit.ambulance.01', 'A-01', 'medical', 'returning', 'zone.central', 35, 90, 1, 0),
('DEMO112', 'unit.ambulance.02', 'A-02', 'medical', 'engaged', 'zone.industrial.east', 120, 210, 0, 0),
('DEMO112', 'unit.fire.01', 'F-01', 'fire', 'available', 'station.fire.1', 0, 160, 1, 0),
('DEMO112', 'unit.police.01', 'P-01', 'police', 'bot-assigned', 'district.stadium', 20, 110, 1, 1);

insert into incident_delta_log(session_id, delta_id, incident_id, change_type, severity, status, timer_delta_seconds, needs_attention, created_utc)
values
('DEMO112', 'delta.001', 'inc.fire.002', 'delta.type.escalated', 'critical', 'active', -30, 1, sysutcdatetime()),
('DEMO112', 'delta.002', 'inc.medical.001', 'delta.type.timeout-risk', 'high', 'active', -20, 1, sysutcdatetime());
