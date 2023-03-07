using ServerBelPodryad.Shared;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerBelPodryad
{
    public class Server
    {
        private static Socket handler;
        private static Socket socket;
        public static void CreateServer()
        {
            try
            {
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(Constant.SERVER_HOST), Constant.PORT);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                socket.Bind(ipPoint);
                socket.Listen(10);

                Console.WriteLine($"Сервер создан и запущен на порту {Constant.PORT}");

            }

            catch (Exception ex)
            {
                throw new Exception("Ошибка создания сервера: " + ex.Message);
            }

        }

        public static void StartConnecting()
        {
            try
            {
                Console.WriteLine("Ожидание подключений...");
                handler = socket.Accept();
                Console.WriteLine($"Подключился пользователь: {handler.RemoteEndPoint}");
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка подключения сервера: " + ex.Message);
            }

        }



        public static Socket GetSocket()
        {
            return handler;
        }

        public static void SendMessage(string data)
        {
            try
            {
                var message = Encoding.UTF8.GetBytes(data);
                int bytesSent = GetSocket().Send(message);

            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка отпраки сообщения: " + ex.Message);
            }

        }

        public static string ReceiveMessage()
        {
            try
            {
                byte[] bytes = new byte[25000];
                int bytesRec = GetSocket().Receive(bytes);
                string reply = Encoding.UTF8.GetString(bytes, 0, bytesRec);

                return reply;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка приема сообщения: " + ex.Message);
            }
        }

    }
}
