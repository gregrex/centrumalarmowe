insert into active_incidents (incident_id, incident_type_id, severity, node_id, status, pressure, shared_action_required)
values
('inc.medical.001', 'incident.medical.chest-pain', 'high', 'zone.central', 'active', 7, 0),
('inc.fire.002', 'incident.fire.apartment', 'critical', 'zone.residential.north', 'active', 9, 1),
('inc.medical.004', 'incident.medical.unconscious', 'critical', 'zone.industrial.east', 'active', 10, 1);

insert into shared_actions (shared_action_id, incident_id, action_type, requested_by_role, timeout_seconds, allow_bot_assist)
values
('shared.demo.001', 'inc.fire.002', 'shared.escalate.major-incident', 'role.coordinator', 15, 1),
('shared.demo.002', 'inc.medical.004', 'shared.reallocate.cross-district', 'role.dispatcher', 10, 1);
