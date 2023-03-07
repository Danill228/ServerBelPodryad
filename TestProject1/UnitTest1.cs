using NUnit.Framework;
using ServerBelPodryad.Models;
using ServerBelPodryad.Stores;
using System.Collections.Generic;

namespace TestProject1
{
    public class Tests
    {

        [Test]
        public void SaveOrder_Passed()
        {
            Order order = new Order
            {
                IdCustomer = 16,
                Title = "eee",
                Description = "eee",
                Address = "eee",
                Price = 1000,
                IsCommunicationTelephone = true,
                IsCommunicationEmail = true,
                DatePublication = "2023-02-27 16:55:09",
                Region = new Region
                {
                    Id = 3,
                    RegionName = "Гомельская область"
                },
                Currency = new Currency
                {
                    Id = 1,
                    CurrencyName = "BLR"
                },
                JobType = new JobType
                {
                    Id = 4,
                    JobTypeName = "Гидроизоляционные работы"
                }
            };


            OrderStore.CreateOrder(order);
        }

        [Test]
        public void AllOrders_Passed()
        {
            List<Order> listOrders = OrderStore.GetAllOrders();

            foreach (Order order in listOrders)
            {
                order.Region = RegionStore.GetRegionById(order.IdRegion);
                order.Currency = CurrencyStore.GetCurrencyById(order.IdCurrency);
                order.JobType = JobTypeStore.GetJobTypeById(order.JobTypeId);
            }
        }

        [Test]
        public void RespondOrder_Passed()
        {
            int idPerformer = 16;
            int idOrder = 20;

            PerformerOrderStore.SaveRespond(idPerformer, idOrder);
        }

        [Test]
        public void SaveUser_Passed()
        {
            User user = new User
            {
                FirstName = "aaa",
                LastName = "aaa",
                ThirdName = "aaa",
                Email = "aaa@gmail.com",
                Phone = "375294586593"
            };

            UserStore.UpdateUser(user);
        }

        [Test]
        public void AuthorizationUser_Passed()
        {
            string login = "123";
            string password = "123";

            User user = UserStore.GetUserByLoginAndPassword(login, password);
            if (user.Id == 0)
            {
                Assert.Fail();
            }

            if (user.IdRole != 2)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void AuthorizationAdmin_Passed()
        {
            string login = "a";
            string password = "a";

            User user = UserStore.GetUserByLoginAndPassword(login, password);
            if (user.Id == 0)
            {
                Assert.Fail();
            }

            if (user.IdRole != 1)
            {
                Assert.Fail();
            }
        }

    }
}