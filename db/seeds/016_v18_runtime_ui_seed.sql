insert into runtime_ui_profile (id, mission_id, result_state, active_recovery_slot, created_utc)
values ('runtime.ui.18.demo', 'mission.demo.18', 'partial', 'recovery.slot.primary', '2026-03-20T00:00:00Z');

insert into recovery_trigger_profile (id, mission_id, trigger_key, severity, role_key, requires_immediate_attention)
values
('trigger.route_blocked', 'mission.demo.18', 'route.blocked', 'critical', 'dispatcher', 1),
('trigger.unit_delayed', 'mission.demo.18', 'unit.delayed', 'high', 'coordinator', 0),
('trigger.objective_risk', 'mission.demo.18', 'objective.at_risk', 'critical', 'operator', 1);
