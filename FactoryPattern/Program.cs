using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactoryPattern
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.ReadKey(true);
        }

        public interface Client
        {
            string UserName { get; set; }
            string UserAuthString { get; set; }
            bool HasAccess { get; set; }

            void BuildAuthString();
        }

        public class User : Client
        {
            public string UserName { get; set; }
            public string UserAuthString { get; set; }
            public bool HasAccess { get; set; } = false;
            public User(string name)
            {
                UserName = name;
            }

            public void BuildAuthString()
            {
                UserAuthString = UserName;
            }
        }

        public class Manager : Client
        {
            public string UserName { get; set; }
            public string UserAuthString { get; set; }
            public bool HasAccess { get; set; } = true;
            public Manager(string name)
            {
                UserName = name;
            }

            public void BuildAuthString()
            {
                UserAuthString = UserName += "MAN";
            }
        }

        public class Admin : Client
        {
            public string UserName { get; set; }
            public string UserAuthString { get; set; }
            public bool HasAccess { get; set; } = true;
            public Admin(string name)
            {
                UserName = name;
            }

            public void BuildAuthString()
            {
                UserAuthString = UserName += "ADMIN";
            }
        }

        public interface AccessBehaviour
        {
            Client Client { get; set; }

            bool HandleAccess();
        }

        public class CheckString : AccessBehaviour
        {
            public Client Client { get; set; }
            public CheckString(Client client)
            {
                Client = client;
            }

            public bool HandleAccess()
            {
                if (Client.UserAuthString.Contains("ADMIN"))
                    return true;

                return false;
            }
        }

        public class SwitchAuth : AccessBehaviour
        {
            public Client Client { get; set; }
            public SwitchAuth(Client client)
            {
                Client = client;
            }

            public bool HandleAccess()
            {
                return Client.HasAccess = !Client.HasAccess;
            }
        }

        public class ClientFactory
        {
            public Client CreateClient(string clientType, string userName)
            {
                Client client = null;

                switch (clientType)
                {
                    case "User":
                        client = new User(userName);
                        break;
                    case "Manager":
                        client = new Manager(userName);
                        break;
                    case "Admin":
                        client = new Admin(userName);
                        break;
                }

                if (client != null)
                    client.BuildAuthString();

                return client;
            }
        }

        public abstract class ClientHandler
        {
            public ClientFactory Factory { get; set; }

            public abstract AccessBehaviour CreateClient(string clientType, string userName);
        }

        public class RetailClientHandler : ClientHandler
        {
            public override AccessBehaviour CreateClient(string clientType, string userName)
            {
                Client newClient = Factory.CreateClient(clientType, userName);
                AccessBehaviour access = null;

                switch (clientType)
                {
                    case "User":
                        access = new SwitchAuth(newClient);
                        break;
                    case "Manager":
                        access = new SwitchAuth(newClient);
                        break;
                    case "Admin":
                        access = new CheckString(newClient);
                        break;
                }

                return access;
            }
        }

        public class EnterpriseClientHandler : ClientHandler
        {
            public override AccessBehaviour CreateClient(string clientType, string userName)
            {
                Client newClient = Factory.CreateClient(clientType, userName);
                AccessBehaviour access = null;

                switch (clientType)
                {
                    case "User":
                        access = new SwitchAuth(newClient);
                        break;
                    case "Manager":
                        access = new CheckString(newClient);
                        break;
                    case "Admin":
                        access = new CheckString(newClient);
                        break;
                }

                return access;
            }
        }
    }
}
