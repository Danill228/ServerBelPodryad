using System;
using System.Collections.Generic;
using System.Text;

namespace ServerBelPodryad.Models
{
    public class Organization
    {
        public int Id { get; set; }
        public int idUser { get; set; }
        public int UserTypeId { get; set; }
        public string Title { get; set; }
        public int IdRegion { get; set; }
        public string LegalAddress { get; set; }
        public string Unp { get; set; }
        public bool IsConstructionTeam { get; set; }
        public int ConstructionTeamPeoples { get; set; }
        public bool IsMultipleProfiles { get; set; }
        public string BasicInformation { get; set; }
        public string EquipmentInformation { get; set; }
        public UserType UserType { get; set; }
        public Region Region { get; set; }

        public Organization()
        {
        }
    }
}
