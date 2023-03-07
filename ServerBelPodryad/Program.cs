using Newtonsoft.Json;
using ServerBelPodryad.Models;
using ServerBelPodryad.Services;
using ServerBelPodryad.Shared;
using ServerBelPodryad.Stores;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerBelPodryad
{
    class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var enc1251 = Encoding.GetEncoding(1251);

            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = enc1251;

            Server.CreateServer();

            while (true)
            {
                try
                {
                    Server.StartConnecting();

                    while (true)
                    {
                        try
                        {
                            string reply = "";
                            string codeMessage = Server.ReceiveMessage();
                            Console.WriteLine("Принято сообщение: " + codeMessage);

                            switch (codeMessage)
                            {
                                case CodeCommand.AUTHORIZATION:
                                    {
                                        try
                                        {
                                            Console.WriteLine($"Выполняется запрос авторизации {CodeCommand.AUTHORIZATION}");
                                            Server.SendMessage(CodeCommand.OK);
                                            string[] loginAndPassword = Server.ReceiveMessage().Split(" ");
                                            string login = loginAndPassword[0];
                                            string password = loginAndPassword[1];

                                            User user = UserStore.GetUserByLoginAndPassword(login, password);
                                            if (user.Id == 0)
                                            {
                                                Console.WriteLine($"Пользователя не найден, логин - {user.Login}");
                                                reply = CodeCommand.AUTHORIZATION_FAIL;
                                                break;
                                            }

                                            Server.SendMessage(CodeCommand.OK);
                                            string userJson = JsonConvert.SerializeObject(user);
                                            reply = userJson;
                                            Console.WriteLine($"Пользователь может быть авторизован, логин - {user.Login}");
                                            break;
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine($"Ошибка авторизации пользователя: {ex.Message}");
                                            reply = CodeCommand.ERROR;
                                            break;
                                        }

                                    }

                                case CodeCommand.GET_ROLE:
                                    {
                                        try
                                        {
                                            Console.WriteLine($"Выполняется запрос поиска роли {CodeCommand.GET_ROLE}");
                                            Server.SendMessage(CodeCommand.OK);
                                            int idRole = Convert.ToInt32(Server.ReceiveMessage());
                                            string role = UserStore.GetRoleById(idRole);

                                            reply = role;
                                            Console.WriteLine($"Роль пользователя - {role}");
                                            break;
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine($"Ошибка поиска роли: {ex.Message}");
                                            reply = CodeCommand.ERROR;
                                            break;
                                        }

                                    }

                                case CodeCommand.REGISTRATION:
                                    {
                                        try
                                        {
                                            Console.WriteLine($"Выполняется запрос регистрации {CodeCommand.REGISTRATION}");
                                            Server.SendMessage(CodeCommand.OK);
                                            string[] loginAndPassword = Server.ReceiveMessage().Split(" ");
                                            string login = loginAndPassword[0];
                                            string password = loginAndPassword[1];

                                            bool isUserExists = UserStore.IsUserExists(login);
                                            if (isUserExists)
                                            {
                                                Console.WriteLine($"ПОлльзователь с таким логином уже существует - {login}");
                                                reply = CodeCommand.REGISTRATION_USER_EXISTS;
                                                break;
                                            }

                                            Server.SendMessage(CodeCommand.OK);
                                            UserStore.CreateUser(login, password, Constant.USER_ID);
                                            reply = CodeCommand.OK;
                                            Console.WriteLine($"Пользователь успешно зарегестрирован - {login}");
                                            break;
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine($"Ошибка регистрации: {ex.Message}");
                                            reply = CodeCommand.ERROR;
                                            break;
                                        }
                                    }

                                case CodeCommand.GET_ORGANIZATION:
                                    {
                                        try
                                        {
                                            Console.WriteLine($"Выполняется запрос поиска организации {CodeCommand.GET_ORGANIZATION}");
                                            Server.SendMessage(CodeCommand.OK);
                                            int idUser = Convert.ToInt32(Server.ReceiveMessage());

                                            Organization organization = OrganizationStore.GetOrganizationByUserId(idUser);
                                            if (organization.Id == 0)
                                            {
                                                Console.WriteLine($"Организация не найдена");
                                                reply = CodeCommand.NONE;
                                                break;
                                            }

                                            organization.Region = RegionStore.GetRegionById(organization.IdRegion);
                                            organization.UserType = UserTypeStore.GetUserTypeById(organization.UserTypeId);

                                            Server.SendMessage(CodeCommand.OK);
                                            reply = JsonConvert.SerializeObject(organization);
                                            Console.WriteLine($"Организация успешно получена - {organization.Title}");
                                            break;
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine($"Ошибка поиска организации: {ex.Message}");
                                            reply = CodeCommand.ERROR;
                                            break;
                                        }
                                    }

                                case CodeCommand.GET_ALL_REGIONS:
                                    {
                                        try
                                        {
                                            Console.WriteLine($"Выполняется запрос поиска списка регионов {CodeCommand.GET_ALL_REGIONS}");
                                            Server.SendMessage(CodeCommand.OK);

                                            List<Region> listRegions = RegionStore.GetAllRegions();

                                            Server.SendMessage(CodeCommand.OK);
                                            reply = JsonConvert.SerializeObject(listRegions);
                                            Console.WriteLine($"Список регионов успешно получен - {listRegions.Count} элементов");
                                            break;
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine($"Ошибка поиска списка регионов: {ex.Message}");
                                            reply = CodeCommand.ERROR;
                                            break;
                                        }
                                    }

                                case CodeCommand.GET_ALL_JOB_TYPES:
                                    {
                                        try
                                        {
                                            Console.WriteLine($"Выполняется запрос поиска списка типа работы {CodeCommand.GET_ALL_JOB_TYPES}");
                                            Server.SendMessage(CodeCommand.OK);

                                            List<JobType> listJobTypes = JobTypeStore.GetAllJobTypes();

                                            Server.SendMessage(CodeCommand.OK);
                                            reply = JsonConvert.SerializeObject(listJobTypes);
                                            Console.WriteLine($"Список типа работы успешно получен - {listJobTypes.Count} элементов");
                                            break;
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine($"Ошибка поиска списка типа работы: {ex.Message}");
                                            reply = CodeCommand.ERROR;
                                            break;
                                        }
                                    }

                                case CodeCommand.GET_ALL_CURRENCIES:
                                    {
                                        try
                                        {
                                            Console.WriteLine($"Выполняется запрос поиска списка валют {CodeCommand.GET_ALL_CURRENCIES}");
                                            Server.SendMessage(CodeCommand.OK);

                                            List<Currency> listCurrencies = CurrencyStore.GetAllCurrencies();

                                            Server.SendMessage(CodeCommand.OK);
                                            reply = JsonConvert.SerializeObject(listCurrencies);
                                            Console.WriteLine($"Список валют успешно получен - {listCurrencies.Count} элементов");
                                            break;
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine($"Ошибка получения списка валют: {ex.Message}");
                                            reply = CodeCommand.ERROR;
                                            break;
                                        }

                                    }

                                case CodeCommand.GET_ALL_USER_TYPES:
                                    {
                                        try
                                        {
                                            Console.WriteLine($"Выполняется запрос поиска списка типа пользователя {CodeCommand.GET_ALL_USER_TYPES}");
                                            Server.SendMessage(CodeCommand.OK);

                                            List<UserType> listUserType = UserTypeStore.GetAllUserTypes();

                                            Server.SendMessage(CodeCommand.OK);
                                            reply = JsonConvert.SerializeObject(listUserType);
                                            Console.WriteLine($"Список типа пользователей успешно получен - {listUserType.Count} элементов");
                                            break;
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine($"Ошибка поиска списка типа пользователя: {ex.Message}");
                                            reply = CodeCommand.ERROR;
                                            break;
                                        }
                                    }

                                case CodeCommand.SAVE_USER:
                                    {
                                        try
                                        {
                                            Console.WriteLine($"Выполняется запрос сохранения пользователя {CodeCommand.SAVE_USER}");
                                            Server.SendMessage(CodeCommand.OK);
                                            string userJson = Server.ReceiveMessage();
                                            User user = JsonConvert.DeserializeObject<User>(userJson);

                                            UserStore.UpdateUser(user);

                                            reply = CodeCommand.OK;
                                            Console.WriteLine($"Пользователь успешно сохранен");
                                            break;
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine($"Ошибка сохранения пользователя: {ex.Message}");
                                            reply = CodeCommand.ERROR;
                                            break;
                                        }
                                    }

                                case CodeCommand.SAVE_ORGANIZATION:
                                    {
                                        try
                                        {
                                            Console.WriteLine($"Выполняется запрос сохранения организации {CodeCommand.SAVE_USER}");
                                            Server.SendMessage(CodeCommand.OK);
                                            string organizationJson = Server.ReceiveMessage();
                                            Organization organization = JsonConvert.DeserializeObject<Organization>(organizationJson);
                                            if (organization.Id == 0)
                                            {
                                                OrganizationStore.CreateOrganization(organization);
                                            }
                                            else
                                            {
                                                OrganizationStore.UpdateOrganization(organization);
                                            }

                                            reply = CodeCommand.OK;
                                            Console.WriteLine($"Организация успешно сохранена");
                                            break;
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine($"Ошибка сохранения организации: {ex.Message}");
                                            reply = CodeCommand.ERROR;
                                            break;
                                        }
                                    }

                                case CodeCommand.DELETE_OLD_ORGANIZATION:
                                    {
                                        try
                                        {
                                            Console.WriteLine($"Выполняется запрос удаления старой организации {CodeCommand.DELETE_OLD_ORGANIZATION}");
                                            Server.SendMessage(CodeCommand.OK);
                                            int organizationId = int.Parse(Server.ReceiveMessage());

                                            OrganizationStore.DeleteOrganizationById(organizationId);

                                            reply = CodeCommand.OK;
                                            Console.WriteLine($"Старая организация успешно удалена");
                                            break;
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine($"Ошибка удаления старой организации: {ex.Message}");
                                            reply = CodeCommand.ERROR;
                                            break;
                                        }
                                    }

                                case CodeCommand.SAVE_ORDER:
                                    {
                                        try
                                        {
                                            Console.WriteLine($"Выполняется запрос сохранения заявки {CodeCommand.SAVE_ORDER}");
                                            Server.SendMessage(CodeCommand.OK);
                                            string orderJson = Server.ReceiveMessage();
                                            Order order = JsonConvert.DeserializeObject<Order>(orderJson);

                                            OrderStore.CreateOrder(order);

                                            reply = CodeCommand.OK;
                                            Console.WriteLine($"Заявка успешно сохранена");
                                            break;
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine($"Ошибка сохранения заявки: {ex.Message}");
                                            reply = CodeCommand.ERROR;
                                            break;
                                        }
                                    }

                                case CodeCommand.GET_ALL_ORDERS:
                                    {
                                        try
                                        {
                                            Console.WriteLine($"Выполняется запрос поиска списка заявок {CodeCommand.GET_ALL_ORDERS}");
                                            Server.SendMessage(CodeCommand.OK);

                                            List<Order> listOrders = OrderStore.GetAllOrders();
                                            if (listOrders.Count == 0)
                                            {
                                                Console.WriteLine("Список заявок пустой");
                                                reply = CodeCommand.NONE;
                                                break;
                                            }

                                            foreach (Order order in listOrders)
                                            {
                                                order.Region = RegionStore.GetRegionById(order.IdRegion);
                                                order.Currency = CurrencyStore.GetCurrencyById(order.IdCurrency);
                                                order.JobType = JobTypeStore.GetJobTypeById(order.JobTypeId);
                                            }

                                            Server.SendMessage(CodeCommand.OK);
                                            reply = JsonConvert.SerializeObject(listOrders);
                                            Console.WriteLine($"Список заявок успешно получен - {listOrders.Count} элементов");
                                            break;
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine($"Ошибка получения списка заявок: {ex.Message}");
                                            reply = CodeCommand.ERROR;
                                            break;
                                        }
                                    }

                                case CodeCommand.GET_USER_BY_ORDER:
                                    {
                                        try
                                        {
                                            Console.WriteLine($"Выполняется запрос поиска заказчика по заказу {CodeCommand.GET_USER_BY_ORDER}");
                                            Server.SendMessage(CodeCommand.OK);
                                            int idCustomer = Convert.ToInt32(Server.ReceiveMessage());

                                            User user = UserStore.GetUserById(idCustomer);

                                            Server.SendMessage(CodeCommand.OK);
                                            reply = JsonConvert.SerializeObject(user);
                                            Console.WriteLine($"Заказчик успешно получен - {user.Login}");
                                            break;
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine($"Ошибка поиска заказчика по заказу: {ex.Message}");
                                            reply = CodeCommand.ERROR;
                                            break;
                                        }
                                    }

                                case CodeCommand.GET_ORDER_CUSTOMER:
                                    {
                                        try
                                        {
                                            Console.WriteLine($"Выполняется запрос поиска заказов пользователя {CodeCommand.GET_ORDER_CUSTOMER}");
                                            Server.SendMessage(CodeCommand.OK);
                                            int idCustomer = Convert.ToInt32(Server.ReceiveMessage());

                                            List<Order> orders = OrderStore.GetAllOrdersByCustomerId(idCustomer);
                                            if (orders.Count == 0)
                                            {
                                                reply = CodeCommand.NONE;
                                                break;
                                            }

                                            foreach (Order order in orders)
                                            {
                                                order.Region = RegionStore.GetRegionById(order.IdRegion);
                                                order.Currency = CurrencyStore.GetCurrencyById(order.IdCurrency);
                                                order.JobType = JobTypeStore.GetJobTypeById(order.JobTypeId);
                                            }

                                            Server.SendMessage(CodeCommand.OK);
                                            reply = JsonConvert.SerializeObject(orders);
                                            Console.WriteLine($"Заказы успешно получены - {orders.Count}");
                                            break;
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine($"Ошибка поиска заказов пользователя: {ex.Message}");
                                            reply = CodeCommand.ERROR;
                                            break;
                                        }
                                    }

                                case CodeCommand.RESPOND_ORDER:
                                    {
                                        try
                                        {
                                            Console.WriteLine($"Выполняется запрос отклика на заявку {CodeCommand.RESPOND_ORDER}");
                                            Server.SendMessage(CodeCommand.OK);
                                            string[] idPerformerAndIdOrder = Server.ReceiveMessage().Split(" ");
                                            int idPerformer = int.Parse(idPerformerAndIdOrder[0]);
                                            int idOrder = int.Parse(idPerformerAndIdOrder[1]);

                                            if (PerformerOrderStore.IsRespondOrderExists(idPerformer, idOrder))
                                            {
                                                reply = CodeCommand.RESPOND_ORDER_EXISTS;
                                                break;
                                            }

                                            PerformerOrderStore.SaveRespond(idPerformer, idOrder);

                                            reply = CodeCommand.OK;
                                            Console.WriteLine($"Отклик на заявку успешно сохранен");
                                            break;
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine($"Ошибка отклика на заявку: {ex.Message}");
                                            reply = CodeCommand.ERROR;
                                            break;
                                        }
                                    }

                                case CodeCommand.DELETE_ORDER:
                                    {
                                        try
                                        {
                                            Console.WriteLine($"Выполняется запрос удаления заявки {CodeCommand.DELETE_ORDER}");
                                            Server.SendMessage(CodeCommand.OK);
                                            int idOrder = int.Parse(Server.ReceiveMessage());

                                            OrderStore.DeleteOrderById(idOrder);

                                            reply = CodeCommand.OK;
                                            Console.WriteLine($"Заявка успешно удалена");
                                            break;
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine($"Ошибка удаления заявки: {ex.Message}");
                                            reply = CodeCommand.ERROR;
                                            break;
                                        }
                                    }

                                case CodeCommand.UPDATE_ORDER:
                                    {
                                        try
                                        {
                                            Console.WriteLine($"Выполняется запрос обновления заявки {CodeCommand.UPDATE_ORDER}");
                                            Server.SendMessage(CodeCommand.OK);
                                            string orderJson = Server.ReceiveMessage();
                                            Order order = JsonConvert.DeserializeObject<Order>(orderJson);

                                            OrderStore.UpdateOrder(order);

                                            reply = CodeCommand.OK;
                                            Console.WriteLine($"Заявка успешно обновлена");
                                            break;
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine($"Ошибка обновления заявки: {ex.Message}");
                                            reply = CodeCommand.ERROR;
                                            break;
                                        }
                                    }

                                case CodeCommand.GET_PERFORMERS_BY_ORDER:
                                    {
                                        try
                                        {
                                            Console.WriteLine($"Выполняется получения списка подрядчиков по заявки {CodeCommand.GET_PERFORMERS_BY_ORDER}");
                                            Server.SendMessage(CodeCommand.OK);
                                            int idOrder = int.Parse(Server.ReceiveMessage());
                                            List<User> performers = UserStore.GetPerformersByOrderId(idOrder);
                                            if (performers.Count == 0)
                                            {
                                                reply = CodeCommand.NONE;
                                                break;
                                            }

                                            foreach (User performer in performers)
                                            {
                                                performer.organization = OrganizationStore.GetOrganizationByUserId(performer.Id);
                                                performer.organization.Region = RegionStore.GetRegionById(performer.organization.IdRegion);
                                                performer.organization.UserType = UserTypeStore.GetUserTypeById(performer.organization.UserTypeId);
                                            }

                                            Server.SendMessage(CodeCommand.OK);
                                            string performersJson = JsonConvert.SerializeObject(performers);
                                            reply = performersJson;
                                            Console.WriteLine($"Список подрядчиков по заявки успешно получен");
                                            break;
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine($"Ошибка получения списка подрядчиков по заявки: {ex.Message}");
                                            reply = CodeCommand.ERROR;
                                            break;
                                        }
                                    }

                                case CodeCommand.CHANGE_PASSWORD:
                                    {
                                        try
                                        {
                                            Console.WriteLine($"Выполняется запрос смены пароля {CodeCommand.CHANGE_PASSWORD}");
                                            Server.SendMessage(CodeCommand.OK);
                                            string[] oldPasswordNewPasswordID = Server.ReceiveMessage().Split(" ");
                                            string oldPassword = oldPasswordNewPasswordID[0];
                                            string newPassword = oldPasswordNewPasswordID[1];
                                            int userId = int.Parse(oldPasswordNewPasswordID[2]);

                                            if (!UserStore.CheckOldPassword(userId, oldPassword))
                                            {
                                                Console.WriteLine("Неверный старый пароль, отмена операции");
                                                reply = CodeCommand.CHANGE_PASSWORD_OLD_FAIL;
                                                break;
                                            }

                                            UserStore.UpdatePasswordUserById(newPassword, userId);

                                            reply = CodeCommand.OK;
                                            Console.WriteLine($"Пароль успешно изменен");
                                            break;
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine($"Ошибка смены пароля: {ex.Message}");
                                            reply = CodeCommand.ERROR;
                                            break;
                                        }
                                    }

                                case CodeCommand.GET_ALL_INFO:
                                    {
                                        try
                                        {
                                            Console.WriteLine($"Выполняется запрос поиска списка новостей {CodeCommand.GET_ALL_INFO}");
                                            Server.SendMessage(CodeCommand.OK);

                                            List<Info> listInfo = InfoStore.GetAllInfo();

                                            Server.SendMessage(CodeCommand.OK);
                                            reply = JsonConvert.SerializeObject(listInfo);
                                            Console.WriteLine($"Список новостей успешно получен - {listInfo.Count} элементов");
                                            break;
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine($"Ошибка получения списка новостей: {ex.Message}");
                                            reply = CodeCommand.ERROR;
                                            break;
                                        }
                                    }

                                case CodeCommand.ADMIN_GET_ALL_USERS:
                                    {
                                        try
                                        {
                                            Console.WriteLine($"Выполняется запрос поиска списка пользователей {CodeCommand.ADMIN_GET_ALL_USERS}");
                                            Server.SendMessage(CodeCommand.OK);

                                            List<User> users = UserStore.GetAllUsers();
                                            if (users.Count == 0)
                                            {
                                                Console.WriteLine("Нет пользователей");
                                                reply = CodeCommand.NONE;
                                                break;
                                            }

                                            Server.SendMessage(CodeCommand.OK);
                                            reply = JsonConvert.SerializeObject(users);
                                            Console.WriteLine($"Список пользователей успешно получен - {users.Count} элементов");
                                            break;
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine($"Ошибка получения списка пользователей: {ex.Message}");
                                            reply = CodeCommand.ERROR;
                                            break;
                                        }
                                    }

                                case CodeCommand.ADMIN_DELETE_INFO:
                                    {
                                        try
                                        {
                                            Console.WriteLine($"Выполняется запрос удаления новости {CodeCommand.ADMIN_DELETE_INFO}");
                                            Server.SendMessage(CodeCommand.OK);
                                            int infoId = int.Parse(Server.ReceiveMessage());

                                            InfoStore.DeleteInfoById(infoId);

                                            reply = CodeCommand.OK;
                                            Console.WriteLine($"Новость успешно удалена");
                                            break;
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine($"Ошибка удаления новости: {ex.Message}");
                                            reply = CodeCommand.ERROR;
                                            break;
                                        }
                                    }

                                case CodeCommand.ADMIN_SAVE_INFO:
                                    {
                                        try
                                        {
                                            Console.WriteLine($"Выполняется запрос сохранения новости {CodeCommand.ADMIN_SAVE_INFO}");
                                            Server.SendMessage(CodeCommand.OK);
                                            string infoJson = Server.ReceiveMessage();
                                            Info info = JsonConvert.DeserializeObject<Info>(infoJson);

                                            if (info.Id == 0)
                                            {
                                                InfoStore.CreateInfo(info);
                                            }
                                            else
                                            {
                                                InfoStore.UpdateInfo(info);
                                            }

                                            reply = CodeCommand.OK;
                                            Console.WriteLine($"Новость успешно сохранена");
                                            break;
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine($"Ошибка сохранения новости: {ex.Message}");
                                            reply = CodeCommand.ERROR;
                                            break;
                                        }
                                    }

                                case CodeCommand.ADMIN_DELETE_USER:
                                    {
                                        try
                                        {
                                            Console.WriteLine($"Выполняется запрос удаления пользователя {CodeCommand.ADMIN_DELETE_USER}");
                                            Server.SendMessage(CodeCommand.OK);
                                            int userId = int.Parse(Server.ReceiveMessage());

                                            UserStore.DeleteUserById(userId);

                                            reply = CodeCommand.OK;
                                            Console.WriteLine($"Пользователь успешно удален");
                                            break;
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine($"Ошибка удаления пользователя: {ex.Message}");
                                            reply = CodeCommand.ERROR;
                                            break;
                                        }
                                    }

                                case CodeCommand.EXIT:
                                    {
                                        Console.WriteLine($"Выполняется запрос отключения {CodeCommand.EXIT}");
                                        throw new Exception("Пользователь завершил сеанс");
                                    }

                                default:
                                    {
                                        Console.WriteLine($"Сообщение не распознано - {codeMessage}");
                                        reply = CodeCommand.ERROR;
                                        break;
                                    }
                            }

                            Server.SendMessage(reply);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Console.WriteLine($"Отключение пользователя: {Server.GetSocket().RemoteEndPoint} \n\n\n");
                            Server.GetSocket().Shutdown(SocketShutdown.Both);
                            break;
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
        }
    }
}
