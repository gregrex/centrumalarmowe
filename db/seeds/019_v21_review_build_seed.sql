insert into ReviewBuildPackage (Id, BuildName, MissionId, CaptureModeEnabled)
values ('review-build.v21', 'Alarm112 Showcase Review Build', 'showcase.mission.01', true);

insert into ReviewBuildChecklistItem (BuildId, KeyName, Title, Severity, Required, Passed)
values
('review-build.v21', 'home.flow', 'Home flow dziala', 'blocker', true, true),
('review-build.v21', 'capture.mode', 'Capture mode czysci overlaye', 'major', true, true),
('review-build.v21', 'runtime.showcase', 'Showcase mission konczy sie raportem', 'blocker', true, true);
