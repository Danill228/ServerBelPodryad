using MySql.Data.MySqlClient;
using ServerBelPodryad.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ServerBelPodryad.Services
{
    public static class Database
    {
        private static readonly MySqlConnection connection = new MySqlConnection($"" +
            $"server={Constant.DATABASE_HOST}; " +
            $"username={Constant.USERNAME}; " +
            $"password={Constant.PASSWORD}; " +
            $"database={Constant.DATABASE}");
        public static bool OpenConnectionOrReturnNow()
        {
            try 
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                return true;
            }
            catch(Exception ex)
            {
                throw new Exception("Ошибка подключения к базе данных: " + ex.Message);
            }
        }

        public static void CloseConnection()
        {
            if (connection.State == ConnectionState.Open)
                connection.Close();
        }

        public static MySqlConnection GetConnection()
        {
            return connection;
        }
    }
}
