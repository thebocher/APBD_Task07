CREATE TABLE Device
(
    Id   VARCHAR(50) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    IsEnabled BIT NOT NULL,
);

CREATE TABLE Embedded(
    Id INT PRIMARY KEY IDENTITY,
    IpAddress VARCHAR(15),
    NetworkName VARCHAR(100),
    DeviceId VARCHAR(50)
    CONSTRAINT fk_embedded_device FOREIGN KEY (DeviceId)
        REFERENCES Device(Id) ON DELETE CASCADE
);

CREATE TABLE PersonalComputer(
    id INT PRIMARY KEY IDENTITY,
    OperationSystem VARCHAR(50),
    DeviceId VARCHAR(50)
    CONSTRAINT fk_personal_computer_device FOREIGN KEY (DeviceId) 
        REFERENCES Device(Id) ON DELETE CASCADE 
);

CREATE TABLE Smartwatch(
    Id INT PRIMARY KEY IDENTITY,
    BatteryPercentage FLOAT NOT NULL,
    DeviceId VARCHAR(50)
    CONSTRAINT fk_smartwatch_device FOREIGN KEY (DeviceId) 
        REFERENCES Device(Id) ON DELETE CASCADE 
);