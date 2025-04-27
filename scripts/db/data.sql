-- Clear existing data (if any)
DELETE FROM Smartwatch;
DELETE FROM PersonalComputer;
DELETE FROM Embedded;
DELETE FROM Device;

-- Insert base devices
INSERT INTO Device (Id, Name, IsEnabled)
VALUES
-- Embedded devices
('ED-001', 'Factory Controller 3000', 1),
('ED-002', 'Warehouse Sensor Array', 0),
('ED-003', 'Production Line Monitor', 1),
('ED-004', 'HVAC Controller', 1),
('ED-005', 'Security Camera #42', 0),

-- Personal computers
('PC-001', 'Developer Workstation', 1),
('PC-002', 'Reception Computer', 1),
('PC-003', 'Accounting Desktop', 0),
('PC-004', 'Server Backup', 1),
('PC-005', 'Marketing Laptop', 0),

-- Smartwatches
('SW-001', 'Employee Fitness Tracker', 1),
('SW-002', 'Executive Smartwatch', 0),
('SW-003', 'Visitor Pass #1', 1),
('SW-004', 'Warehouse Supervisor', 1),
('SW-005', 'Security Patrol', 0);

-- Insert embedded devices data
SET IDENTITY_INSERT Embedded ON;
INSERT INTO Embedded (Id, DeviceId, IpAddress, NetworkName)
VALUES
    (1, 'ED-001', '192.168.1.10', 'MD Ltd. Factory Network'),
    (2, 'ED-002', '192.168.1.11', 'MD Ltd. Warehouse LAN'),
    (3, 'ED-003', '192.168.1.12', 'MD Ltd. Production VLAN'),
    (4, 'ED-004', '192.168.2.10', 'MD Ltd. Building Automation'),
    (5, 'ED-005', '192.168.3.42', 'MD Ltd. Security Network');
SET IDENTITY_INSERT Embedded OFF;

-- Insert personal computers data
SET IDENTITY_INSERT PersonalComputer ON;
INSERT INTO PersonalComputer (Id, DeviceId, OperationSystem)
VALUES
    (1, 'PC-001', 'Windows 11 Pro'),
    (2, 'PC-002', 'Windows 10 Enterprise'),
    (3, 'PC-003', 'Ubuntu 22.04 LTS'),
    (4, 'PC-004', 'Windows Server 2022'),
    (5, 'PC-005', 'macOS Ventura');
SET IDENTITY_INSERT PersonalComputer OFF;

-- Insert smartwatches data
SET IDENTITY_INSERT Smartwatch ON;
INSERT INTO Smartwatch (Id, DeviceId, BatteryPercentage)
VALUES
    (1, 'SW-001', 85),
    (2, 'SW-002', 15),  -- Low battery
    (3, 'SW-003', 72),
    (4, 'SW-004', 92),
    (5, 'SW-005', 5);   -- Very low battery
SET IDENTITY_INSERT Smartwatch OFF;