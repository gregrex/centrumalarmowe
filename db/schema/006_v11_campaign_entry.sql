-- v11 campaign entry, profile cosmetics and audio transitions
create table if not exists campaign_chapter (
    chapter_id text primary key,
    title_key text not null,
    theme_id text not null,
    progress real not null
);

create table if not exists campaign_mission_node (
    mission_id text primary key,
    chapter_id text not null,
    node_kind text not null,
    state text not null,
    x real not null,
    y real not null,
    title_key text not null
);

create table if not exists profile_cosmetic (
    cosmetic_id text primary key,
    category text not null,
    rarity text not null,
    unlock_source text not null
);
