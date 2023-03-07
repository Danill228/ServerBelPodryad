using MySql.Data.MySqlClient;
using ServerBelPodryad.Models;
using ServerBelPodryad.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerBelPodryad.Stores
{
    public static class CurrencyStore
    {
        public static List<Currency> GetAllCurrencies()
        {
            List<Currency> currencies = new List<Currency>();
            try
            {
                Database.OpenConnectionOrReturnNow();

                string sql = "select * from currencies";
                MySqlCommand command = new MySqlCommand(sql, Database.GetConnection());
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Currency currecny = new Currency();
                    currecny.Id = (reader["id"] as int?).GetValueOrDefault();
                    currecny.CurrencyName = reader["currency"].ToString();

                    currencies.Add(currecny);
                }

                return currencies;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка получения списка валют: {ex.Message}");
            }
            finally
            {
                Database.CloseConnection();
            }

        }

        public static Currency GetCurrencyById(int id)
        {
            Currency currency = new Currency();
            try
            {
                Database.OpenConnectionOrReturnNow();

                string sql = "select * from currencies where id = @id";
                MySqlCommand command = new MySqlCommand(sql, Database.GetConnection());
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    currency.Id = (reader["id"] as int?).GetValueOrDefault();
                    currency.CurrencyName = reader["currency"].ToString();
                }

                return currency;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка получения валюты по id: {ex.Message}");
            }
            finally
            {
                Database.CloseConnection();
            }

        }

        public static Currency GetCurrencyByName(string currencyName)
        {
            Currency currency = new Currency();
            try
            {
                Database.OpenConnectionOrReturnNow();

                string sql = "select * from currencies where currency = @currency";
                MySqlCommand command = new MySqlCommand(sql, Database.GetConnection());
                command.Parameters.Add("@currency", MySqlDbType.VarChar).Value = currencyName;
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    currency.Id = (reader["id"] as int?).GetValueOrDefault();
                    currency.CurrencyName = reader["currency"].ToString();
                }

                return currency;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка получения валюты по currency: {ex.Message}");
            }
            finally
            {
                Database.CloseConnection();
            }

        }
    }
}
