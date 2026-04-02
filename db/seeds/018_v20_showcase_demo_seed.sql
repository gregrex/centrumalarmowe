insert into ShowcaseMission (Id, Title, RecommendedRole, EstimatedDurationSeconds)
values ('showcase.mission.01', 'Awaria na trasie miejskiej', 'dispatcher', 420);

insert into OnboardingStep (Id, FlowId, SortOrder, Title) values
('intro', 'ftue.v20', 1, 'Witaj w 112'),
('roles', 'ftue.v20', 2, 'Poznaj role'),
('map', 'ftue.v20', 3, 'Mapa miasta'),
('dispatch', 'ftue.v20', 4, 'Wyślij jednostkę');
