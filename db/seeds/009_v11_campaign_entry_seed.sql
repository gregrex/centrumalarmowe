insert into campaign_chapter (chapter_id, title_key, theme_id, progress) values
('chapter.01', 'chapter.01.title', 'theme.city.day', 0.75),
('chapter.02', 'chapter.02.title', 'theme.city.storm', 0.20);

insert into campaign_mission_node (mission_id, chapter_id, node_kind, state, x, y, title_key) values
('mission.01.01', 'chapter.01', 'tutorial', 'completed', 0.10, 0.62, 'mission.01.01.title'),
('mission.01.02', 'chapter.01', 'standard', 'completed', 0.28, 0.56, 'mission.01.02.title'),
('mission.01.03', 'chapter.01', 'critical', 'active', 0.44, 0.49, 'mission.01.03.title');

insert into profile_cosmetic (cosmetic_id, category, rarity, unlock_source) values
('portrait.dispatcher.steel', 'portrait', 'common', 'starter'),
('frame.blackout.pulse', 'frame', 'rare', 'chapter.03.complete'),
('badge.fast_filter', 'badge', 'uncommon', 'daily.challenge');
