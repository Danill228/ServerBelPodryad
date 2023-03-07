using MySql.Data.MySqlClient;
using ServerBelPodryad.Models;
using ServerBelPodryad.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerBelPodryad.Stores
{
    public static class RegionStore
    {
        public static Region GetRegionById(int id)
        {
            Region region = new Region();
            try
            {
                Database.OpenConnectionOrReturnNow();

                string sql = "select * from regions where id = @id";
                MySqlCommand command = new MySqlCommand(sql, Database.GetConnection());
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    region.Id = (reader["id"] as int?).GetValueOrDefault();
                    region.RegionName = reader["region"].ToString();
                }

                return region;
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

        public static List<Region> GetAllRegions()
        {
            List<Region> regions = new List<Region>();
            try
            {
                Database.OpenConnectionOrReturnNow();

                string sql = "select * from regions";
                MySqlCommand command = new MySqlCommand(sql, Database.GetConnection());
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Region region = new Region();
                    region.Id = (reader["id"] as int?).GetValueOrDefault();
                    region.RegionName = reader["region"].ToString();

                    regions.Add(region);
                }

                return regions;
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

        public static Region GetRegionByName(string nameRegion)
        {
            Region region = new Region();
            try
            {
                Database.OpenConnectionOrReturnNow();

                string sql = "select * from regions where region = @region";
                MySqlCommand command = new MySqlCommand(sql, Database.GetConnection());
                command.Parameters.Add("@region", MySqlDbType.VarChar).Value = nameRegion;
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    region.Id = (reader["id"] as int?).GetValueOrDefault();
                    region.RegionName = reader["region"].ToString();
                }

                return region;
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
