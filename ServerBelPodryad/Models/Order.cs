using System;
using System.Collections.Generic;
using System.Text;

namespace ServerBelPodryad.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int IdCustomer { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int IdRegion { get; set; }
        public string Address { get; set; }
        public int Price { get; set; }
        public int IdCurrency { get; set; }
        public bool IsCommunicationTelephone { get; set; }
        public bool IsCommunicationEmail { get; set; }
        public string DatePublication { get; set; }
        public int JobTypeId { get; set; }
        public Region Region { get; set; }
        public Currency Currency { get; set; }
        public JobType JobType { get; set; }

        public Order()
        {
        }
    }
}
