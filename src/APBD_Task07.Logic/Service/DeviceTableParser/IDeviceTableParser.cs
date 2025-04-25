using System.Data.Common;
using System.Transactions;
using APBD_Task07.Model.Devices;
using Microsoft.Data.SqlClient;

namespace APBD_Task07.Logic.Service.DeviceTableParser;

public interface IDeviceTableParser
{
    bool CanParse(string deviceId);
    string GetJoinCommand();
    Device ParseFromReader(DbDataReader reader);
    bool InsertToOwnTable(Device device, SqlConnection connection);
    bool UpdateInOwnTable(string id, Device device, SqlConnection connection);
}