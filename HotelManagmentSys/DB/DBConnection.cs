using System;
using System.Data.SqlClient;

namespace HotelManagmentSys.DB
{
    internal class DBConnection
        
    {
        private string connectionString;
        private SqlConnection connection;

        public DBConnection()
        {
        
            connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\User\Desktop\RAD hotel\HotelManagmentSys\HotelManagmentSys\MyDB.mdf"";Integrated Security=True";
        }

        public SqlConnection OpenConnection()
        {
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
            }
            catch (Exception dbEx)
            {
                MessageBox.Show(dbEx.Message, "Database Error in DBConnection", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
               
            }
            return connection;
        }

        public void CloseConnection()
        {
            if (connection != null && connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }
}
