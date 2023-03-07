using MySql.Data.MySqlClient;
using ServerBelPodryad.Models;
using ServerBelPodryad.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerBelPodryad.Stores
{
    public static class InfoStore
    {
        public static Info GetInfoById(int id)
        {
            Info info = new Info();
            try
            {
                Database.OpenConnectionOrReturnNow();

                string sql = "select * from news where id = @id";
                MySqlCommand command = new MySqlCommand(sql, Database.GetConnection());
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    info.Id = (reader["id"] as int?).GetValueOrDefault();
                    info.Title = reader["job_title"].ToString();
                    info.Data = reader["data"].ToString();
                    info.DatePublication = reader["date_publication"].ToString();
                }

                return info;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка получения новости по id: {ex.Message}");
            }
            finally
            {
                Database.CloseConnection();
            }

        }

        public static List<Info> GetAllInfo()
        {
            List<Info> infoList = new List<Info>();
            try
            {
                Database.OpenConnectionOrReturnNow();

                string sql = "select * from news";
                MySqlCommand command = new MySqlCommand(sql, Database.GetConnection());
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Info info = new Info();

                    info.Id = (reader["id"] as int?).GetValueOrDefault();
                    info.Title = reader["title"].ToString();
                    info.Data = reader["data"].ToString();
                    info.DatePublication = reader["date_publication"].ToString();

                    infoList.Add(info);
                }

                return infoList;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка получения списка новостей: {ex.Message}");
            }
            finally
            {
                Database.CloseConnection();
            }
        }

        public static void DeleteInfoById(int id)
        {
            try
            {
                Database.OpenConnectionOrReturnNow();

                string sql = "delete from news where id = @id";
                MySqlCommand command = new MySqlCommand(sql, Database.GetConnection());
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка удаления новости по id: {ex.Message}");
            }
            finally
            {
                Database.CloseConnection();
            }
        }

        public static void CreateInfo(Info info)
        {
            try
            {
                Database.OpenConnectionOrReturnNow();

                string sql = "insert into news (title, data, date_publication) VALUES (@title, @data, @date_publication)";
                MySqlCommand command = new MySqlCommand(sql, Database.GetConnection());
                command.Parameters.Add("@title", MySqlDbType.VarChar).Value = info.Title;
                command.Parameters.Add("@data", MySqlDbType.VarChar).Value = info.Data;
                command.Parameters.Add("@date_publication", MySqlDbType.DateTime).Value = info.DatePublication;

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка сохранения новости: {ex.Message}");
            }
            finally
            {
                Database.CloseConnection();
            }
        }

        public static void UpdateInfo(Info info)
        {
            try
            {
                Database.OpenConnectionOrReturnNow();

                string sql = "update news set title = @title, data = @data, date_publication = @date_publication where id = @id";
                MySqlCommand command = new MySqlCommand(sql, Database.GetConnection());
                command.Parameters.Add("@title", MySqlDbType.VarChar).Value = info.Title;
                command.Parameters.Add("@data", MySqlDbType.VarChar).Value = info.Data;
                command.Parameters.Add("@date_publication", MySqlDbType.DateTime).Value = info.DatePublication;
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = info.Id;

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка обновления новости: {ex.Message}");
            }
            finally
            {
                Database.CloseConnection();
            }
        }
    }
}
