using System.Data;
using APBD_Task07.Logic.Repositories.interfaces;
using APBD_Task07.Model.Devices;
using Microsoft.Data.SqlClient;

namespace APBD_Task07.Logic.Repositories;

public class PersonalComputerRepository(string connectionString) : IPersonalComputerRepository
{
    public void AddDevice(Device device)
    {
        using var conn = new SqlConnection(connectionString);
        conn.Open();
        var personalComputer = (PersonalComputer)device;
        using var cmd = new SqlCommand("AddPersonalComputer", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@DeviceId", personalComputer.Id);
        cmd.Parameters.AddWithValue("@Name", personalComputer.Name);
        cmd.Parameters.AddWithValue("@IsEnabled", personalComputer.TurnedOn);
        cmd.Parameters.AddWithValue("@OperationSystem", personalComputer.OperationalSystem);

        cmd.ExecuteNonQuery();
    }

    public Device? GetDevice(string deviceId)
    {
        using var conn = new SqlConnection(connectionString);
        conn.Open();
        const string sql = @"SELECT * FROM PersonalComputer pc 
                        JOIN Device d ON pc.DeviceId = d.Id
                        WHERE DeviceId = @DeviceId";
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@DeviceId", deviceId);
        using var reader = cmd.ExecuteReader();

        if (reader.HasRows)
        {
            reader.Read();
            var device = new PersonalComputer();
            device.Id = (string) reader["DeviceId"];
            device.Name = (string) reader["Name"];
            device.TurnedOn = (bool) reader["IsEnabled"];
            device.OperationalSystem = (string) reader["OperationSystem"];
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
            string sql = @"DELETE FROM PersonalComputer
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
        var personalComputer = (PersonalComputer)device;

        {
            var deviceRepository = new DeviceRepository(connectionString);
            var rowVersion = deviceRepository.GetRowVersion(deviceId, conn, transaction);
            string sql = @"
                UPDATE Device
                SET Name = @Name, TurnedOn = @IsEnabled
                WHERE Id = @DeviceId AND RowVersion = @RowVersion";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@DeviceId", deviceId);
            cmd.Parameters.AddWithValue("@RowVersion", rowVersion);
            cmd.Parameters.AddWithValue("@Name", personalComputer.Name);
            cmd.Parameters.AddWithValue("@IsEnabled", personalComputer.TurnedOn);
            var result = cmd.ExecuteNonQuery();
            if (result == 0)
            {
                transaction.Rollback();
                return false;
            }
        }

        {
            var rowVersion = GetRowVersion(deviceId, conn, transaction);
            string sql = @"
                UPDATE PersonalComputer 
                SET OperationSystem = @OperationSystem,
                WHERE DeviceId = @DeviceId AND RowVersion = @RowVersion";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@DeviceId", deviceId);
            cmd.Parameters.AddWithValue("@RowVersion", rowVersion);
            cmd.Parameters.AddWithValue("@OperationSystem", personalComputer.OperationalSystem);
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
        var sql = "SELECT RowVersion FROM PersonalComputer WHERE DeviceId = @Id";
        using var cmd = new SqlCommand(sql, conn, transaction);
        cmd.Parameters.AddWithValue("@Id", id);
        byte[]? result = (byte[]) cmd.ExecuteScalar();
        return result;
    }
}