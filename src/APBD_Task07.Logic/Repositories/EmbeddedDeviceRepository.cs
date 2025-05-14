using System.Data;
using APBD_Task07.Logic.Repositories.interfaces;
using APBD_Task07.Model.Devices;
using Microsoft.Data.SqlClient;

namespace APBD_Task07.Logic.Repositories;

public class EmbeddedDeviceRepository(string connectionString) : IEmbeddedDeviceRepository
{
    public void AddDevice(Device device)
    {
        using var conn = new SqlConnection(connectionString);
        conn.Open();
        var embedded = (EmbeddedDevice)device;
        using var cmd = new SqlCommand("AddEmbedded", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@DeviceId", embedded.Id);
        cmd.Parameters.AddWithValue("@Name", embedded.Name);
        cmd.Parameters.AddWithValue("@IsEnabled", embedded.TurnedOn);
        cmd.Parameters.AddWithValue("@IpAddress", embedded.IpAddress);
        cmd.Parameters.AddWithValue("@NetworkName", embedded.NetworkName);

        cmd.ExecuteNonQuery();
    }

    public Device? GetDevice(string deviceId)
    {
        using var conn = new SqlConnection(connectionString);
        const string sql = @"SELECT * FROM Embedded ed 
                        JOIN Device d ON ed.DeviceId = d.Id
                        WHERE DeviceId = @DeviceId";
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@DeviceId", deviceId);
        conn.Open();
        using var reader = cmd.ExecuteReader();

        if (reader.HasRows)
        {
            reader.Read();
            var device = new EmbeddedDevice();
            device.Id = (string) reader["DeviceId"];
            device.Name = (string) reader["Name"];
            device.TurnedOn = (bool) reader["IsEnabled"];
            device.IpAddress = (string) reader["IpAddress"];
            device.NetworkName = (string) reader["NetworkName"];
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
            string sql = @"DELETE FROM Embedded
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
        var embedded = (EmbeddedDevice)device;

        {
            var rowVersion = GetRowVersion(deviceId, conn, transaction);
            string sql = @"
                UPDATE Embedded
                SET IpAddress = @IpAddress, NetworkName = @NetworkName
                WHERE DeviceId = @DeviceId AND RowVersion = @RowVersion";

            using var cmd = new SqlCommand(sql, conn, transaction);
            cmd.Parameters.AddWithValue("@DeviceId", deviceId);
            cmd.Parameters.AddWithValue("@RowVersion", rowVersion);
            cmd.Parameters.AddWithValue("@IpAddress", embedded.IpAddress);
            cmd.Parameters.AddWithValue("@NetworkName", embedded.NetworkName);
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
            cmd.Parameters.AddWithValue("@Name", device.Name);
            cmd.Parameters.AddWithValue("@IsEnabled", device.TurnedOn);
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

    public byte[]? GetRowVersion(string id, SqlConnection conn, SqlTransaction transaction)
    {
        var sql = "SELECT RowVersion FROM Embedded WHERE DeviceId = @Id";
        using var cmd = new SqlCommand(sql, conn, transaction);
        cmd.Parameters.AddWithValue("@Id", id);
        byte[]? result = (byte[]) cmd.ExecuteScalar();
        return result;
    }
}