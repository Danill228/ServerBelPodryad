using MySql.Data.MySqlClient;
using ServerBelPodryad.Models;
using ServerBelPodryad.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ServerBelPodryad.Stores
{
    public static class UserStore
    {
        public static User GetUserByLoginAndPassword(string login, string password)
        {
            User user = new User();
            try
            {
                Database.OpenConnectionOrReturnNow();

                string sql = "select * from users where login = @login and password = @password";
                MySqlCommand command = new MySqlCommand(sql, Database.GetConnection());
                command.Parameters.Add("@login", MySqlDbType.VarChar).Value = login;
                command.Parameters.Add("@password", MySqlDbType.VarChar).Value = password;
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    user.Id = (reader["id"] as int?).GetValueOrDefault();
                    user.IdRole = (reader["id_role"] as int?).GetValueOrDefault();
                    user.Login = login;
                    user.Password = password;
                    user.FirstName = reader["first_name"].ToString();
                    user.LastName = reader["last_name"].ToString();
                    user.ThirdName = reader["third_name"].ToString();
                    user.Email = reader["email"].ToString();
                    user.Phone = reader["phone"].ToString();
                    user.Site = reader["site"].ToString();
                }

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка получения пользователя по логину и паролю: {ex.Message}");
            }
            finally
            {
                Database.CloseConnection();
            }

        }

        public static User GetUserById(int id)
        {
            User user = new User();
            try
            {
                Database.OpenConnectionOrReturnNow();

                string sql = "select * from users where id = @id";
                MySqlCommand command = new MySqlCommand(sql, Database.GetConnection());
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    user.Id = id;
                    user.IdRole = (reader["id_role"] as int?).GetValueOrDefault();
                    user.Login = reader["login"].ToString();
                    user.Password = reader["password"].ToString();
                    user.FirstName = reader["first_name"].ToString();
                    user.LastName = reader["last_name"].ToString();
                    user.ThirdName = reader["third_name"].ToString();
                    user.Email = reader["email"].ToString();
                    user.Phone = reader["phone"].ToString();
                    user.Site = reader["site"].ToString();
                }

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка получения пользователя по id: {ex.Message}");
            }
            finally
            {
                Database.CloseConnection();
            }

        }

        public static string GetRoleById(int id)
        {
            string role = "";
            try
            {
                Database.OpenConnectionOrReturnNow();

                string sql = "select * from roles where id = @id";
                MySqlCommand command = new MySqlCommand(sql, Database.GetConnection());
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    role = (reader["role"]).ToString();
                }

                return role;
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

        public static bool IsUserExists(string login)
        {
            try
            {
                Database.OpenConnectionOrReturnNow();

                string sql = "select * from users where login = @login";
                MySqlCommand command = new MySqlCommand(sql, Database.GetConnection());
                command.Parameters.Add("@login", MySqlDbType.VarChar).Value = login;
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return true;
                }

                return false;
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

        public static void CreateUser(string login, string password, int idRole)
        {
            try
            {
                Database.OpenConnectionOrReturnNow();

                string sql = "insert into users (id_role, login, password) values (@idRole, @login, @password)";
                MySqlCommand command = new MySqlCommand(sql, Database.GetConnection());
                command.Parameters.Add("@idRole", MySqlDbType.Int32).Value = idRole;
                command.Parameters.Add("@login", MySqlDbType.VarChar).Value = login;
                command.Parameters.Add("@password", MySqlDbType.VarChar).Value = password;

                command.ExecuteNonQuery();

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

        public static void UpdateUser(User user)
        {
            try
            {
                Database.OpenConnectionOrReturnNow();

                string sql = "update users set " +
                    "first_name = @first_name, last_name = @last_name, third_name = @third_name, " +
                    "email = @email, phone = @phone, site = @site " +
                    "where id = @id;";
                MySqlCommand command = new MySqlCommand(sql, Database.GetConnection());
                command.Parameters.Add("@first_name", MySqlDbType.VarChar).Value = user.FirstName;
                command.Parameters.Add("@last_name", MySqlDbType.VarChar).Value = user.LastName;
                command.Parameters.Add("@third_name", MySqlDbType.VarChar).Value = user.ThirdName;
                command.Parameters.Add("@email", MySqlDbType.VarChar).Value = user.Email;
                command.Parameters.Add("@phone", MySqlDbType.VarChar).Value = user.Phone;
                command.Parameters.Add("@site", MySqlDbType.VarChar).Value = user.Site;
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = user.Id;

                command.ExecuteNonQuery();

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

        public static List<User> GetPerformersByOrderId(int idOrder)
        {
            List<User> performers = new List<User>();
            try
            {
                Database.OpenConnectionOrReturnNow();

                string sql = "select u.* from orders o " +
                    "join users_orders uo on o.id = uo.id_order " +
                    "join users u on u.id = uo.id_performer " +
                    "where o.id = @id_order";
                MySqlCommand command = new MySqlCommand(sql, Database.GetConnection());
                command.Parameters.Add("@id_order", MySqlDbType.Int32).Value = idOrder;
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    User performer = new User();

                    performer.Id = (reader["id"] as int?).GetValueOrDefault();
                    performer.Login = reader["login"].ToString();
                    performer.FirstName = reader["first_name"].ToString();
                    performer.LastName = reader["last_name"].ToString();
                    performer.ThirdName = reader["third_name"].ToString();
                    performer.Email = reader["email"].ToString();
                    performer.Phone = reader["phone"].ToString();
                    performer.Site = reader["site"].ToString();

                    performers.Add(performer);
                }

                return performers;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка получения подрядчиков по id заявки: {ex.Message}");
            }
            finally
            {
                Database.CloseConnection();
            }

        }

        public static bool CheckOldPassword(int userId, string oldPassword)
        {
            try
            {
                Database.OpenConnectionOrReturnNow();

                string sql = "select * from users where id = @id and password = @password";
                MySqlCommand command = new MySqlCommand(sql, Database.GetConnection());
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = userId;
                command.Parameters.Add("@password", MySqlDbType.VarChar).Value = oldPassword;

                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return true;
                }

                return false;

            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка обновления заявки: {ex.Message}");
            }
            finally
            {
                Database.CloseConnection();
            }
        }

        public static void UpdatePasswordUserById(string newPassword, int userId)
        {
            try
            {
                Database.OpenConnectionOrReturnNow();

                string sql = "update users set password = @password where id = @id";
                MySqlCommand command = new MySqlCommand(sql, Database.GetConnection());
                command.Parameters.Add("@password", MySqlDbType.VarChar).Value = newPassword;
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = userId;

                command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка обновления пароля пользователя: {ex.Message}");
            }
            finally
            {
                Database.CloseConnection();
            }
        }

        public static List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            try
            {
                Database.OpenConnectionOrReturnNow();

                string sql = "select * from users";
                MySqlCommand command = new MySqlCommand(sql, Database.GetConnection());
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    User user = new User();

                    user.Id = (reader["id"] as int?).GetValueOrDefault();
                    user.IdRole = (reader["id_role"] as int?).GetValueOrDefault();
                    user.Login = reader["login"].ToString();
                    user.Password = reader["password"].ToString();
                    user.FirstName = reader["first_name"].ToString();
                    user.LastName = reader["last_name"].ToString();
                    user.ThirdName = reader["third_name"].ToString();
                    user.Email = reader["email"].ToString();
                    user.Phone = reader["phone"].ToString();
                    user.Site = reader["site"].ToString();

                    users.Add(user);
                }

                return users;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка получения списка пользователей: {ex.Message}");
            }
            finally
            {
                Database.CloseConnection();
            }

        }

        public static void DeleteUserById(int id)
        {
            try
            {
                Database.OpenConnectionOrReturnNow();

                string sql = "delete from users where id = @id";
                MySqlCommand command = new MySqlCommand(sql, Database.GetConnection());
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;

                command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка удаления пользователя по id: {ex.Message}");
            }
            finally
            {
                Database.CloseConnection();
            }
        }

    }
}
