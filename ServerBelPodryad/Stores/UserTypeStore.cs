using MySql.Data.MySqlClient;
using ServerBelPodryad.Models;
using ServerBelPodryad.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerBelPodryad.Stores
{
    public static class UserTypeStore
    {
        public static UserType GetUserTypeById(int id)
        {
            UserType userType = new UserType();
            try
            {
                Database.OpenConnectionOrReturnNow();

                string sql = "select * from user_types where id = @id";
                MySqlCommand command = new MySqlCommand(sql, Database.GetConnection());
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    userType.Id = (reader["id"] as int?).GetValueOrDefault();
                    userType.UserTypeName = reader["user_type"].ToString();
                }

                return userType;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                Database.CloseConnection();
            }

        }

        public static List<UserType> GetAllUserTypes()
        {
            List<UserType> userTypes = new List<UserType>();
            try
            {
                Database.OpenConnectionOrReturnNow();

                string sql = "select * from user_types";
                MySqlCommand command = new MySqlCommand(sql, Database.GetConnection());
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    UserType userType = new UserType();
                    userType.Id = (reader["id"] as int?).GetValueOrDefault();
                    userType.UserTypeName = reader["user_type"].ToString();

                    userTypes.Add(userType);
                }

                return userTypes;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                Database.CloseConnection();
            }

        }

        public static UserType GetUserTypeByName(string nameUserType)
        {
            UserType userType = new UserType();
            try
            {
                Database.OpenConnectionOrReturnNow();

                string sql = "select * from user_types where user_type = @user_type";
                MySqlCommand command = new MySqlCommand(sql, Database.GetConnection());
                command.Parameters.Add("@user_type", MySqlDbType.VarChar).Value = nameUserType;
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    userType.Id = (reader["id"] as int?).GetValueOrDefault();
                    userType.UserTypeName = reader["user_type"].ToString();
                }

                return userType;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                Database.CloseConnection();
            }

        }
    }
}
