insert into ReviewBuildPackage (Id, BuildName, MissionId, CaptureModeEnabled)
values ('review-build.v21', 'Alarm112 Showcase Review Build', 'showcase.mission.01', 1);

insert into ReviewBuildChecklistItem (BuildId, [Key], Title, Severity, Required, Passed)
values
('review-build.v21', 'home.flow', 'Home flow dziala', 'blocker', 1, 1),
('review-build.v21', 'capture.mode', 'Capture mode czysci overlaye', 'major', 1, 1),
('review-build.v21', 'runtime.showcase', 'Showcase mission konczy sie raportem', 'blocker', 1, 1);
