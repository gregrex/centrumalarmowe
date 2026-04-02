insert into DemoFlowState (DemoFlowStateId, MissionId, ActiveStepId, BranchState, UpdatedAtUtc)
values ('demo.flow.17', 'mission.demo.17', 'flow.05', 'runtime_active', '2026-03-20T12:00:00Z');

insert into RecoveryDecisionAudit (RecoveryDecisionAuditId, MissionId, CardId, OptionId, OutcomeCode, CreatedAtUtc)
values ('recovery.audit.17.01', 'mission.demo.17', 'recovery.card.traffic_reroute', 'opt.reassign_police', 'route_stabilized', '2026-03-20T12:02:00Z');

insert into ReportRoomState (ReportRoomStateId, MissionId, VariantId, RewardRevealId, CreatedAtUtc)
values ('report.room.17', 'mission.demo.17', 'report.success', 'badge_responder_bronze', '2026-03-20T12:09:00Z');
