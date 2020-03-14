using MySql.Data.MySqlClient;
using System.Data;

namespace MyFinance.Util
{
    public class DAL
    {
        private static string server = "localhost";
        private static string database = "financeiro";
        private static string user = "root";
        private static string password = "";
        private static string porta = "3308";
        private string connectionString = $"Server={server};Database={database};Port={porta};Uid={user};Pwd={password}";
        private MySqlConnection connection;
        public DAL()
        {
            connection = new MySqlConnection(connectionString);
            connection.Open();

        }

        //Excuta Selects
        public DataTable RetDataTable (string sql)
        {
            DataTable dataTable = new DataTable();
            MySqlCommand command = new MySqlCommand(sql, connection);
            MySqlDataAdapter da = new MySqlDataAdapter(command);
            da.Fill(dataTable);
            return dataTable;
        }
        //Executa Inserts, Updates, Deletes
        public void ExcutaComandoSQL(string sql)
        {
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.ExecuteNonQuery();
        }

    }
}
