using MySql.Data.MySqlClient;
using ServerBelPodryad.Models;
using ServerBelPodryad.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerBelPodryad.Stores
{
    public static class OrganizationStore
    {
        public static Organization GetOrganizationByUserId(int idUser)
        {
            Organization organization = new Organization();
            try
            {
                Database.OpenConnectionOrReturnNow();

                string sql = "select * from organizations where id_user = @id_user";
                MySqlCommand command = new MySqlCommand(sql, Database.GetConnection());
                command.Parameters.Add("@id_user", MySqlDbType.Int32).Value = idUser;
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    organization.Id = (reader["id"] as int?).GetValueOrDefault();
                    organization.idUser = (reader["id_user"] as int?).GetValueOrDefault();
                    organization.UserTypeId = (reader["user_type_id"] as int?).GetValueOrDefault();
                    organization.Title = reader["title"].ToString();
                    organization.IdRegion = (reader["region_id"] as int?).GetValueOrDefault();
                    organization.LegalAddress = reader["legal_address"].ToString();
                    organization.Unp = reader["unp"].ToString();
                    organization.IsConstructionTeam = (reader["is_construction_team"] as bool?).GetValueOrDefault();
                    organization.ConstructionTeamPeoples = (reader["construction_team_peoples"] as int?).GetValueOrDefault();
                    organization.IsMultipleProfiles = (reader["is_multiple_profiles"] as bool?).GetValueOrDefault();
                    organization.BasicInformation = reader["basic_information"].ToString();
                    organization.EquipmentInformation = reader["equipment_information"].ToString();
                }

                return organization;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка получения организации по id пользователя: {ex.Message}");
            }
            finally
            {
                Database.CloseConnection();
            }
        }

        public static void CreateOrganization(Organization organization)
        {
            try
            {
                Database.OpenConnectionOrReturnNow();

                string sql = "insert into organizations (id_user, user_type_id, title, " +
                    "region_id, legal_address, unp, is_construction_team, construction_team_peoples, " +
                    "is_multiple_profiles, basic_information, equipment_information) " +
                    "values " +
                    "(@id_user, @user_type_id, @title, " +
                    "@region_id, @legal_address, @unp, @is_construction_team, @construction_team_peoples," +
                    "@is_multiple_profiles, @basic_information, @equipment_information)";
                MySqlCommand command = new MySqlCommand(sql, Database.GetConnection());
                command.Parameters.Add("@id_user", MySqlDbType.Int32).Value = organization.idUser;
                command.Parameters.Add("@user_type_id", MySqlDbType.Int32).Value = organization.UserType.Id;
                command.Parameters.Add("@title", MySqlDbType.Text).Value = organization.Title;
                command.Parameters.Add("@region_id", MySqlDbType.Int32).Value = organization.Region.Id;
                command.Parameters.Add("@legal_address", MySqlDbType.Text).Value = organization.LegalAddress;
                command.Parameters.Add("@unp", MySqlDbType.VarChar).Value = organization.Unp;
                command.Parameters.Add("@is_construction_team", MySqlDbType.Int32).Value = organization.IsConstructionTeam;
                command.Parameters.Add("@construction_team_peoples", MySqlDbType.Int32).Value = organization.ConstructionTeamPeoples;
                command.Parameters.Add("@is_multiple_profiles", MySqlDbType.Int32).Value = organization.IsMultipleProfiles;
                command.Parameters.Add("@basic_information", MySqlDbType.MediumText).Value = organization.BasicInformation;
                command.Parameters.Add("@equipment_information", MySqlDbType.MediumText).Value = organization.EquipmentInformation;

                command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка добавления организации: {ex.Message}");
            }
            finally
            {
                Database.CloseConnection();
            }
        }

        public static void UpdateOrganization(Organization organization)
        {
            try
            {
                Database.OpenConnectionOrReturnNow();

                string sql = "update organizations set " +
                    "user_type_id = @user_type_id, title = @title, region_id = @region_id, legal_address = @legal_address, " +
                    "unp = @unp, is_construction_team = @is_construction_team, construction_team_peoples = @construction_team_peoples, " +
                    "is_multiple_profiles = @is_multiple_profiles, basic_information = @basic_information, equipment_information = @equipment_information " +
                    "where id = @id";
                MySqlCommand command = new MySqlCommand(sql, Database.GetConnection());
                command.Parameters.Add("@user_type_id", MySqlDbType.Int32).Value = organization.UserType.Id;
                command.Parameters.Add("@title", MySqlDbType.Text).Value = organization.Title;
                command.Parameters.Add("@region_id", MySqlDbType.Int32).Value = organization.Region.Id;
                command.Parameters.Add("@legal_address", MySqlDbType.Text).Value = organization.LegalAddress;
                command.Parameters.Add("@unp", MySqlDbType.VarChar).Value = organization.Unp;
                command.Parameters.Add("@is_construction_team", MySqlDbType.Int32).Value = organization.IsConstructionTeam;
                command.Parameters.Add("@construction_team_peoples", MySqlDbType.Int32).Value = organization.ConstructionTeamPeoples;
                command.Parameters.Add("@is_multiple_profiles", MySqlDbType.Int32).Value = organization.IsMultipleProfiles;
                command.Parameters.Add("@basic_information", MySqlDbType.MediumText).Value = organization.BasicInformation;
                command.Parameters.Add("@equipment_information", MySqlDbType.MediumText).Value = organization.EquipmentInformation;
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = organization.Id;

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

        public static void DeleteOrganizationById(int id)
        {
            try
            {
                Database.OpenConnectionOrReturnNow();

                string sql = "delete from organizations where id = @id";
                MySqlCommand command = new MySqlCommand(sql, Database.GetConnection());
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;

                command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка удаления организации по id: {ex.Message}");
            }
            finally
            {
                Database.CloseConnection();
            }
        }


    }
}
