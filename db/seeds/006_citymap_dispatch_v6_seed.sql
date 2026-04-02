insert into city_nodes (node_id, type_id, label_key, x, y) values
('zone.central', 'district.center', 'zone.central', 0.50, 0.36),
('station.medical.1', 'station.medical', 'station.medical.1', 0.44, 0.52),
('station.fire.1', 'station.fire', 'station.fire.1', 0.67, 0.56),
('station.police.1', 'station.police', 'station.police.1', 0.58, 0.22);

insert into city_connections (connection_id, from_node_id, to_node_id, type_id) values
('c1', 'station.medical.1', 'zone.central', 'road.primary'),
('c2', 'station.fire.1', 'zone.central', 'road.primary'),
('c3', 'station.police.1', 'zone.central', 'road.secondary');
