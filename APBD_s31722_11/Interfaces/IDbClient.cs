using System.Data;
using APBD_s31722_11.DataLayer;
using Microsoft.Data.SqlClient;

namespace APBD_s31722_11.Interfaces;

public interface IDbClient
{
    IAsyncEnumerable<T> ReadDataAsync<T>(
        string query,
        Func<SqlDataReader, T> map,
        Dictionary<string, object> parameters = null);

    Task<T> ReadScalarAsync<T>(
        string query,
        Dictionary<string, object> parameters = null,
        CommandType commandType = CommandType.Text);

    Task<int?> ReadScalarAsync(
        string query,
        Dictionary<string, object> parameters = null);

    Task<int> ExecuteNonQueriesAsTransactionAsync(List<CommandConfig> commands);
}