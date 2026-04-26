-- V22 release candidate schema
CREATE TABLE IF NOT EXISTS RcBuildPackage (
    BuildId TEXT PRIMARY KEY,
    Version TEXT NOT NULL,
    MissionId TEXT NOT NULL,
    ChannelCsv TEXT NOT NULL,
    GateCsv TEXT NOT NULL,
    ArtifactCsv TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS RcBugBashChecklist (
    Id TEXT PRIMARY KEY,
    Label TEXT NOT NULL,
    Severity TEXT NOT NULL,
    IsBlocking BOOLEAN NOT NULL,
    IsPassed BOOLEAN NOT NULL
);
