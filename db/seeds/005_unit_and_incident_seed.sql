
insert into incident_templates (id, name, category, base_priority) values
('incident.medical.busdriver_unconscious', 'Bus driver unconscious', 'Medical', 100),
('incident.fire.kitchen_apartment', 'Kitchen fire in apartment', 'Fire', 70),
('incident.police.domestic_violence', 'Domestic violence call', 'Police', 80)
on conflict do nothing;

insert into dispatch_units (id, unit_type, status, zone) values
('AMB-01', 'unit.ambulance.basic', 'Available', 'Hospital1'),
('AMB-02', 'unit.ambulance.advanced', 'Available', 'Hospital1'),
('FIRE-01', 'unit.fire.engine', 'Available', 'NorthStation'),
('POL-01', 'unit.police.patrol', 'Available', 'Central')
on conflict do nothing;
