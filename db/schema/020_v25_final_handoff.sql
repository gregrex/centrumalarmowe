CREATE TABLE IF NOT EXISTS FinalHandoffPackages (
    Id varchar(64) NOT NULL PRIMARY KEY,
    Version varchar(32) NOT NULL,
    BuildTarget varchar(32) NOT NULL,
    Status varchar(32) NOT NULL,
    RecommendedNextAction varchar(256) NOT NULL
);
