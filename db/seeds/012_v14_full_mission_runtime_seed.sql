insert into MissionScripts (MissionId, StepOrder, AtSeconds, EventName) values
('mission.01.04', 1, 0, 'briefing_confirmed'),
('mission.01.04', 2, 20, 'runtime_open'),
('mission.01.04', 3, 35, 'incident.medical.spawn'),
('mission.01.04', 4, 80, 'incident.fire.spawn'),
('mission.01.04', 5, 130, 'incident.police.spawn'),
('mission.01.04', 6, 420, 'mission.complete.gate');

insert into MissionOutcomeDefinitions (OutcomeId, LabelKey, ScoreDelta, CityStabilityDelta) values
('dispatch.success', 'dispatch.success', 15, 2),
('dispatch.delayed', 'dispatch.delayed', -8, -5),
('dispatch.blocked', 'dispatch.blocked', -12, -8),
('dispatch.rerouted', 'dispatch.rerouted', 4, 0),
('dispatch.failed_no_unit', 'dispatch.failed_no_unit', -18, -10);
