insert into Scenario(Id, Code, Name, Difficulty, IsActive)
values (100, 'vs-001', 'Wieczorny korek', 'Normal', 1);

insert into GameSetting([Key], [Value]) values
('VerticalSlice.Enabled', 'true'),
('Bot.Takeover.GraceSeconds', '12'),
('Session.Tick.Milliseconds', '1000');
