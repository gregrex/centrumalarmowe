-- Reference dictionary seed for content-driven architecture
create table if not exists ReferenceDictionary (
    Id text primary key,
    Category text not null,
    Value text not null,
    Version integer not null default 1
);

insert into ReferenceDictionary (Id, Category, Value, Version) values
('role.operator', 'role', 'Operator zgłoszeń', 1),
('role.dispatcher', 'role', 'Dyspozytor jednostek', 1),
('role.coordinator', 'role', 'Koordynator operacyjny', 1),
('role.crisis_officer', 'role', 'Oficer kryzysowy', 1),
('severity.low', 'severity', 'Niski', 1),
('severity.medium', 'severity', 'Średni', 1),
('severity.high', 'severity', 'Wysoki', 1),
('severity.critical', 'severity', 'Krytyczny', 1)
on conflict(Id) do nothing;
