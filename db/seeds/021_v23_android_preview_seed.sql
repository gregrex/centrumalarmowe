insert into AndroidPreviewBuild (BuildId, Version, MissionId, Status)
values ('Alarm112-Preview-001', '0.1.0-preview1', 'showcase.mission.01', 'ready_for_internal_preview');

insert into ReleaseReadinessChecklist (Id, Label, State, Severity)
values
('rr.preview.smoke', 'Smoke preview build', 'done', 'P1'),
('rr.capture.pack', 'Capture pack complete', 'in_progress', 'P2'),
('rr.telemetry.mock', 'Telemetry dashboard ready', 'done', 'P2'),
('rr.blockers.none', 'No blocker bugs open', 'pending', 'P1');
