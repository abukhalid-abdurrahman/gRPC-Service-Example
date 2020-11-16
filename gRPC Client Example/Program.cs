using Grpc.Net.Client;
using gRPC_Database.Models;
using gRPC_Service_Example;
using System;
using System.Threading.Tasks;

namespace gRPC_Client_Example
{
    class Program
    {
        private static string gRPCHostName = "https://localhost:5001";

        private static void SystemOutput(string _text)
        {
            Console.WriteLine(_text);
        }

        private static string GetClientString(string _caption)
        {
            SystemOutput($"Input {_caption}: ");
            return Console.ReadLine();
        }

        private static int GetClientInt(string _caption)
        {
            string _clientString = GetClientString(_caption);
            int.TryParse(_clientString, out int _value);
            return _value;
        }


        private static async Task ExecuteCRUD(string command)
        {
            using var channel = GrpcChannel.ForAddress(gRPCHostName);
            var client = new User.UserClient(channel);

            switch (command)
            {
                case "create":
                    {
                        UserModel _newUser = new UserModel();
                        _newUser.Name = GetClientString(nameof(_newUser.Name));
                        _newUser.Surname = GetClientString(nameof(_newUser.Surname));
                        _newUser.Age = GetClientInt(nameof(_newUser.Age));
                        _newUser.FullName = $"{_newUser.Name} {_newUser.Surname}";

                        var response = await client.CreateAsync(new UserData
                        {
                            Age = _newUser.Age,
                            Name = _newUser.Name,
                            Surname = _newUser.Surname,
                            FullName = _newUser.FullName,
                        });
                        SystemOutput($"Reply Message: User Id: {response.Id}");
                    }
                    break;
                case "read":
                    {
                        var list = await client.ReadAsync(new Google.Protobuf.WellKnownTypes.Empty(), null);
                        if (list.UsersList_ == null)
                            return;

                        foreach (var item in list.UsersList_)
                        {
                            SystemOutput($"User Name: {item.FullName}, Age: {item.Age}\n");
                        }
                    }
                    break;
                case "update":
                    {
                        UserModel _editedUser = new UserModel();
                        _editedUser.Id = GetClientInt(nameof(_editedUser.Id));
                        _editedUser.Name = GetClientString(nameof(_editedUser.Name));
                        _editedUser.Surname = GetClientString(nameof(_editedUser.Surname));
                        _editedUser.Age = GetClientInt(nameof(_editedUser.Age));
                        _editedUser.FullName = $"{_editedUser.Name} {_editedUser.Surname}";

                        var response = await client.UpdateAsync(new UserData
                        {
                            Id = _editedUser.Id,
                            Age = _editedUser.Age,
                            Name = _editedUser.Name,
                            Surname = _editedUser.Surname,
                            FullName = _editedUser.FullName,
                        });
                        SystemOutput($"Reply Message: User Edited Id: {response.Id}, Full Name: {response.FullName}");
                    }
                    break;
                case "delete":
                    {
                        int _userId = GetClientInt("Id");
                        var response = await client.DeleteAsync(new UserData() { Id = _userId });
                        SystemOutput($"Deleted User Id: {response.Id}");
                    }
                    break;
                default:
                    SystemOutput("Incorrect command!");
                    break;
            }

            channel.Dispose();
        }

        static async Task Main(string[] args)
        {
            string cmd = string.Empty;
            while(cmd != "terminate")
            {
                cmd = GetClientString("command");

                await ExecuteCRUD(cmd);
            }

            Console.ReadKey();
        }
    }
}
