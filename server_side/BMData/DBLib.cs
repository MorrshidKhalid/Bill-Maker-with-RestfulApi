using System.Data;
using Microsoft.Data.SqlClient;

public class DBLib
{
    public static void LoadDataToDataTable(ref DataTable dt, SqlCommand command)
    {

        SqlDataReader reader = command.ExecuteReader();
        if (reader.HasRows)
            dt.Load(reader);

        reader.Close();
    }

    public static void ExecAndGetInsertedID(ref int insID, SqlCommand command)
    {
        object result = command.ExecuteScalar();
        if (result != null && int.TryParse(result.ToString(), out int insertedID))
            insID = insertedID;

    }

    public static void ExecAndGetIntClmValue(ref int clmValue, string clm, SqlCommand command)
    {
        SqlDataReader reader = command.ExecuteReader();

        if (reader.Read())
            clmValue = (int)reader[clm];
        else clmValue = -1;

        reader.Close();
    }

    public static void ExecAndGetDecimalClmValue(ref decimal clmValue, string clm, SqlCommand command)
    {
        SqlDataReader reader = command.ExecuteReader();

        if (reader.Read())
            clmValue = (decimal)reader[clm];
        else clmValue = 0;

        reader.Close();
    }

    public static void ExecAndGetStrClmValue(ref string clmValue, string clm, SqlCommand command, ref bool isFound)
    {

        using SqlDataReader reader = command.ExecuteReader();

        if (reader.Read())
        {
            isFound = true;
            clmValue = (string)reader[clm];
        }
        else isFound = false;
    }

    public static void HasRow(ref bool isFound, SqlCommand command)
    {
        using SqlDataReader reader = command.ExecuteReader();
        isFound = reader.HasRows;
    }

    public static void HandleDBNull(SqlDataReader reader, string clm, ref string valueToHandel)
            => valueToHandel = reader[clm] != DBNull.Value ? (string)reader[clm] : "";

    public static void HandleInsertDBNull(SqlCommand command, string valueToAdd, string value)
    {
        if (value != "")
            command.Parameters.AddWithValue(valueToAdd, value);
        else
            command.Parameters.AddWithValue(valueToAdd, DBNull.Value);
    }

    public static string JoinForSqlIn(List<string> stringList)
    {
        if (stringList == null || !stringList.Any())
            return ""; // Empty list results in an empty string

        return string.Join(",", stringList.Select(s => "'" + s + "'"));
    }

    public static string JoinForSqlIn(List<int> stringList)
    {
        if (stringList == null || !stringList.Any())
            return ""; // Empty list results in an empty string

        return string.Join(",", stringList.Select(s => "'" + s + "'"));
    }

}
