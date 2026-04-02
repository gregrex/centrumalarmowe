-- v4 seed: lobby and dictionary extensions
-- This seed is intentionally lightweight and starter-safe.

insert into gameplay_settings(key, value)
values
('lobby.bot_fill_enabled', 'true'),
('lobby.reconnect_seconds', '60'),
('hud.icon_catalog_version', '1.0.0'),
('reference.bundle.version', '1.1.0');
