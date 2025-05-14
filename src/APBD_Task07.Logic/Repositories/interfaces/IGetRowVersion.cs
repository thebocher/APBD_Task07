using Microsoft.Data.SqlClient;

namespace APBD_Task07.Logic.Repositories.interfaces;

public interface IGetRowVersion
{
    byte[]? GetRowVersion(string id, SqlConnection conn, SqlTransaction transaction);
}