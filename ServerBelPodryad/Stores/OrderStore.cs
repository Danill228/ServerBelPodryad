using MySql.Data.MySqlClient;
using ServerBelPodryad.Models;
using ServerBelPodryad.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerBelPodryad.Stores
{
    public static class OrderStore
    {
        public static void CreateOrder(Order order)
        {
            try
            {
                Database.OpenConnectionOrReturnNow();

                string sql = "insert into orders (id_customer, title, description, id_region, " +
                    "address, price, id_currency, is_communication_telephone, " +
                    "is_communication_email, date_publication, job_type_id) " +
                    "values " +
                    "(@id_customer, @title, @description, @id_region, " +
                    "@address, @price, @id_currency, @is_communication_telephone, " +
                    "@is_communication_email, @date_publication, @job_type_id)";
                MySqlCommand command = new MySqlCommand(sql, Database.GetConnection());
                command.Parameters.Add("@id_customer", MySqlDbType.Int32).Value = order.IdCustomer;
                command.Parameters.Add("@title", MySqlDbType.VarChar).Value = order.Title;
                command.Parameters.Add("@description", MySqlDbType.MediumText).Value = order.Description;
                command.Parameters.Add("@id_region", MySqlDbType.Int32).Value = order.Region.Id;
                command.Parameters.Add("@address", MySqlDbType.Text).Value = order.Address;
                command.Parameters.Add("@price", MySqlDbType.Int32).Value = order.Price;
                command.Parameters.Add("@id_currency", MySqlDbType.Int32).Value = order.Currency.Id;
                command.Parameters.Add("@is_communication_telephone", MySqlDbType.Int32).Value = order.IsCommunicationTelephone;
                command.Parameters.Add("@is_communication_email", MySqlDbType.Int32).Value = order.IsCommunicationEmail;
                command.Parameters.Add("@date_publication", MySqlDbType.DateTime).Value = order.DatePublication;
                command.Parameters.Add("@job_type_id", MySqlDbType.Int32).Value = order.JobType.Id;

                command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка добавления заявки: {ex.Message}");
            }
            finally
            {
                Database.CloseConnection();
            }
        }

        public static List<Order> GetAllOrders()
        {
            List<Order> orders = new List<Order>();
            try
            {
                Database.OpenConnectionOrReturnNow();

                string sql = "select * from orders";
                MySqlCommand command = new MySqlCommand(sql, Database.GetConnection());
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Order order = new Order();
                    order.Id = (reader["id"] as int?).GetValueOrDefault();
                    order.IdCustomer = (reader["id_customer"] as int?).GetValueOrDefault();
                    order.Title = reader["title"].ToString();
                    order.Description = reader["description"].ToString();
                    order.IdRegion = (reader["id_region"] as int?).GetValueOrDefault();
                    order.Address = reader["address"].ToString();
                    order.Price = (reader["price"] as int?).GetValueOrDefault();
                    order.IdCurrency = (reader["id_currency"] as int?).GetValueOrDefault();
                    order.IsCommunicationTelephone = (reader["is_communication_telephone"] as bool?).GetValueOrDefault();
                    order.IsCommunicationEmail = (reader["is_communication_email"] as bool?).GetValueOrDefault();
                    order.DatePublication = reader["date_publication"].ToString();
                    order.JobTypeId = (reader["job_type_id"] as int?).GetValueOrDefault();

                    orders.Add(order);
                }

                return orders;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка получения списка заказов: {ex.Message}");
            }
            finally
            {
                Database.CloseConnection();
            }

        }

        public static List<Order> GetAllOrdersByCustomerId(int idCustomer)
        {
            List<Order> orders = new List<Order>();
            try
            {
                Database.OpenConnectionOrReturnNow();

                string sql = "select * from orders where id_customer = @id_customer";
                MySqlCommand command = new MySqlCommand(sql, Database.GetConnection());
                command.Parameters.Add("@id_customer", MySqlDbType.Int32).Value = idCustomer;
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Order order = new Order();
                    order.Id = (reader["id"] as int?).GetValueOrDefault();
                    order.IdCustomer = (reader["id_customer"] as int?).GetValueOrDefault();
                    order.Title = reader["title"].ToString();
                    order.Description = reader["description"].ToString();
                    order.IdRegion = (reader["id_region"] as int?).GetValueOrDefault();
                    order.Address = reader["address"].ToString();
                    order.Price = (reader["price"] as int?).GetValueOrDefault();
                    order.IdCurrency = (reader["id_currency"] as int?).GetValueOrDefault();
                    order.IsCommunicationTelephone = (reader["is_communication_telephone"] as bool?).GetValueOrDefault();
                    order.IsCommunicationEmail = (reader["is_communication_email"] as bool?).GetValueOrDefault();
                    order.DatePublication = reader["date_publication"].ToString();
                    order.JobTypeId = (reader["job_type_id"] as int?).GetValueOrDefault();

                    orders.Add(order);
                }

                return orders;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка получения списка заказов по id_customer: {ex.Message}");
            }
            finally
            {
                Database.CloseConnection();
            }

        }

        public static void DeleteOrderById(int id)
        {
            try
            {
                Database.OpenConnectionOrReturnNow();

                string sql = "delete from orders where id = @id";
                MySqlCommand command = new MySqlCommand(sql, Database.GetConnection());
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;

                command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка удаления заявки: {ex.Message}");
            }
            finally
            {
                Database.CloseConnection();
            }
        }

        public static void UpdateOrder(Order order)
        {
            try
            {
                Database.OpenConnectionOrReturnNow();

                string sql = "update orders set title = @title, description = @description, " +
                    "id_region = @id_region, address = @address, price = @price, id_currency = @id_currency, " +
                    "is_communication_telephone = @is_communication_telephone, is_communication_email = @is_communication_email, " +
                    "date_publication = @date_publication, job_type_id = @job_type_id where id = @id";
                MySqlCommand command = new MySqlCommand(sql, Database.GetConnection());
                command.Parameters.Add("@title", MySqlDbType.VarChar).Value = order.Title;
                command.Parameters.Add("@description", MySqlDbType.MediumText).Value = order.Description;
                command.Parameters.Add("@id_region", MySqlDbType.Int32).Value = order.IdRegion;
                command.Parameters.Add("@address", MySqlDbType.Text).Value = order.Address;
                command.Parameters.Add("@price", MySqlDbType.Int32).Value = order.Price;
                command.Parameters.Add("@id_currency", MySqlDbType.Int32).Value = order.IdCurrency;
                command.Parameters.Add("@is_communication_telephone", MySqlDbType.Int32).Value = order.IsCommunicationTelephone;
                command.Parameters.Add("@is_communication_email", MySqlDbType.Int32).Value = order.IsCommunicationEmail;
                command.Parameters.Add("@date_publication", MySqlDbType.DateTime).Value = order.DatePublication;
                command.Parameters.Add("@job_type_id", MySqlDbType.Int32).Value = order.JobTypeId;
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = order.Id;

                command.ExecuteNonQuery();

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

    }
}
