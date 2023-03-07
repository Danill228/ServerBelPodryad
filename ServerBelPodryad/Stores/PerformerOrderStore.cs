using MySql.Data.MySqlClient;
using ServerBelPodryad.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerBelPodryad.Stores
{
    public static class PerformerOrderStore
    {
        public static bool IsRespondOrderExists(int idPerformer, int idOrder)
        {
            try
            {
                Database.OpenConnectionOrReturnNow();

                string sql = "select * from users_orders where id_performer = @id_performer and id_order = @id_order";
                MySqlCommand command = new MySqlCommand(sql, Database.GetConnection());
                command.Parameters.Add("@id_performer", MySqlDbType.Int32).Value = idPerformer;
                command.Parameters.Add("@id_order", MySqlDbType.Int32).Value = idOrder;
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка проверки отклика на заявку: {ex.Message}");
            }
            finally
            {
                Database.CloseConnection();
            }
        }

        public static void SaveRespond(int idPerformer, int idOrder)
        {
            try
            {
                Database.OpenConnectionOrReturnNow();

                string sql = "insert into users_orders (id_performer, id_order) values (@id_performer, @id_order)";
                MySqlCommand command = new MySqlCommand(sql, Database.GetConnection());
                command.Parameters.Add("@id_performer", MySqlDbType.Int32).Value = idPerformer;
                command.Parameters.Add("@id_order", MySqlDbType.Int32).Value = idOrder;
                command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка сохранения ответа на заявку: {ex.Message}");
            }
            finally
            {
                Database.CloseConnection();
            }
        }
    }
}
