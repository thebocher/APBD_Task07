using System.Data;
using APBD_Task07.Logic.Repositories.interfaces;
using APBD_Task07.Model.Devices;
using Microsoft.Data.SqlClient;

namespace APBD_Task07.Logic.Repositories;

public class SmartwatchRepository(string connectionString) : ISmartwatchRepository
{
    public byte[]? GetRowVersion(string deviceId, SqlConnection conn, SqlTransaction transaction)
    {
        var sql = "SELECT RowVersion FROM Smartwatch WHERE DeviceId = @deviceId";
        using var cmd = new SqlCommand(sql, conn, transaction);
        cmd.Parameters.AddWithValue("@deviceId", deviceId);
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return (byte[])reader["RowVersion"];
        }

        return null;
    }
    
    public void AddDevice(Device device)
    {
        using var conn = new SqlConnection(connectionString);
        conn.Open();
        var smartwatch = (SmartWatch)device;
        using var cmd = new SqlCommand("AddSmartwatch", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@DeviceId", smartwatch.Id);
        cmd.Parameters.AddWithValue("@Name", smartwatch.Name);
        cmd.Parameters.AddWithValue("@IsEnabled", smartwatch.TurnedOn);
        cmd.Parameters.AddWithValue("@BatteryPercentage", smartwatch.BatteryPercentage);

        cmd.ExecuteNonQuery();
    }

    public Device? GetDevice(string deviceId)
    {
        using var conn = new SqlConnection(connectionString);
        conn.Open();
        const string sql = @"SELECT * FROM Smartwatch sw 
                        JOIN Device d ON sw.DeviceId = d.Id
                        WHERE DeviceId = @DeviceId";
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@DeviceId", deviceId);
        using var reader = cmd.ExecuteReader();

        if (reader.HasRows)
        {
            reader.Read();
            var device = new SmartWatch();
            device.Id = (string) reader["DeviceId"];
            device.Name = (string) reader["Name"];
            device.TurnedOn = (bool) reader["IsEnabled"];
            device.BatteryPercentage = (int) reader["BatteryPercentage"];
            return device;
        }

        return null;
    }

    public bool DeleteDevice(string deviceId)
    {
        using var conn = new SqlConnection(connectionString);
        conn.Open();
        var transaction = conn.BeginTransaction();

        {
            string sql = @"DELETE FROM Smartwatch
                                WHERE DeviceId = @DeviceId";
            using var cmd = new SqlCommand(sql, conn, transaction);
            cmd.Parameters.AddWithValue("@DeviceId", deviceId);
            var result = cmd.ExecuteNonQuery();
    
            if (result == 0)
            {
                transaction.Rollback();
                return false;
            }    
        }

        {
            string sql = @"DELETE FROM Device WHERE Id = @DeviceId";
            using var cmd = new SqlCommand(sql, conn, transaction);
            cmd.Parameters.AddWithValue("@DeviceId", deviceId);
            var result = cmd.ExecuteNonQuery();
            if (result == 0)
            {
                transaction.Rollback();
                return false;
            }
        }
        
        transaction.Commit();
        
        return true;
    }

    public bool UpdateDevice(string deviceId, Device device)
    {
        using var conn = new SqlConnection(connectionString);
        conn.Open();
        var transaction = conn.BeginTransaction();
        var smartwatch = (SmartWatch)device;

        {
            var rowVersion = GetRowVersion(deviceId, conn, transaction);
            string sql = @"
                UPDATE Smartwatch 
                SET BatteryPercentage = @BatteryPercentage
                WHERE DeviceId = @DeviceId AND RowVersion = @RowVersion";

            using var cmd = new SqlCommand(sql, conn, transaction);
            cmd.Parameters.AddWithValue("@DeviceId", deviceId);
            cmd.Parameters.AddWithValue("@RowVersion", rowVersion);
            cmd.Parameters.AddWithValue("@BatteryPercentage", smartwatch.BatteryPercentage);
            var result = cmd.ExecuteNonQuery();
    
            if (result == 0)
            {
                transaction.Rollback();
                return false;
            }    
        }

        {
            var deviceRepository = new DeviceRepository(connectionString);
            var rowVersion = deviceRepository.GetRowVersion(deviceId, conn, transaction);
            string sql = @"
                UPDATE Device
                SET Name = @Name, IsEnabled = @IsEnabled
                WHERE Id = @DeviceId AND RowVersion = @RowVersion";
            var cmd = new SqlCommand(sql, conn, transaction);
            cmd.Parameters.AddWithValue("@DeviceId", deviceId);
            cmd.Parameters.AddWithValue("@RowVersion", rowVersion);
            cmd.Parameters.AddWithValue("@Name", smartwatch.Name);
            cmd.Parameters.AddWithValue("@IsEnabled", smartwatch.TurnedOn);
            var result = cmd.ExecuteNonQuery();
            if (result == 0)
            {
                transaction.Rollback();
                return false;
            }
        }
        
        transaction.Commit();
        return true;
    }
}