INSERT INTO RcBuildPackage (BuildId, Version, MissionId, ChannelCsv, GateCsv, ArtifactCsv)
VALUES ('Alarm112-RC-001', '0.1.0-rc1', 'showcase.mission.01', 'internal_review,android_rc', 'flow_complete,bugbash_ready,media_pack_ready,audio_lock', 'Alarm112-RC.apk,Alarm112-RC.aab,release_notes_draft.md,promo_media_pack.zip');

INSERT INTO RcBugBashChecklist (Id, Label, Severity, IsBlocking, IsPassed) VALUES
('bb.home.cta', 'Home CTA działa', 'P1', true, true),
('bb.runtime.hud', 'Runtime HUD jest czytelny', 'P1', true, true),
('bb.report.room', 'Report room ma poprawne CTA', 'P1', true, true);
