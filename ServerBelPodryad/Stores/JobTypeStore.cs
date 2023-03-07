using MySql.Data.MySqlClient;
using ServerBelPodryad.Models;
using ServerBelPodryad.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerBelPodryad.Stores
{
    public static class JobTypeStore
    {
        public static JobType GetJobTypeById(int id)
        {
            JobType jobType = new JobType(); 
            try
            {
                Database.OpenConnectionOrReturnNow();

                string sql = "select * from job_types where id = @id";
                MySqlCommand command = new MySqlCommand(sql, Database.GetConnection());
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    jobType.Id = (reader["id"] as int?).GetValueOrDefault();
                    jobType.JobTypeName = reader["job_title"].ToString();
                }

                return jobType;
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

        public static List<JobType> GetAllJobTypes()
        {
            List<JobType> jobTypes = new List<JobType>();
            try
            {
                Database.OpenConnectionOrReturnNow();

                string sql = "select * from job_types";
                MySqlCommand command = new MySqlCommand(sql, Database.GetConnection());
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    JobType jobType = new JobType();
                    jobType.Id = (reader["id"] as int?).GetValueOrDefault();
                    jobType.JobTypeName = reader["job_title"].ToString();

                    jobTypes.Add(jobType);
                }

                return jobTypes;
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
