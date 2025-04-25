#!/bin/bash

# Wait for SQL Server to start
echo "Waiting for SQL Server to start..."
for i in {1..30};
do
  /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$MSSQL_SA_PASSWORD" -C -Q "SELECT 1" &> /dev/null
  if [ $? -eq 0 ]
  then
    echo "SQL Server is ready"
    break
  else
    echo "Not ready yet..."
    sleep 1
  fi
done

# Run initialization scripts
echo "Running initialization scripts..."

# Create database
/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$MSSQL_SA_PASSWORD" -C -Q "CREATE DATABASE devices"

# Create schema and tables
/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$MSSQL_SA_PASSWORD" -C -d devices -i /usr/config/init.sql

# Insert data if needed
/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$MSSQL_SA_PASSWORD" -C -d devices -i /usr/config/data.sql

echo "Initialization complete"