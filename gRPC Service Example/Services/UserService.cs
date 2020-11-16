using System.Threading.Tasks;
using System.Linq;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using gRPC_Database.Interfaces;
using gRPC_Database.Models;
using Google.Protobuf.WellKnownTypes;

namespace gRPC_Service_Example
{
    public class UserService : User.UserBase
    {
        private readonly ILogger<UserService> _logger;
        private static IContext<UserModel> _userContext;
        public UserService(ILogger<UserService> logger, IContext<UserModel> userContext)
        {
            _logger = logger;
            _userContext = userContext;
        }

        public override Task<UserResponse> Create(UserData request, ServerCallContext context)
        {
            bool _userCreatedSuccessfully = false;

            if (request != null)
                _userCreatedSuccessfully = false;

            UserModel _newUser = new UserModel() 
            { 
                Age = request.Age,
                Name = request.Name,
                Surname = request.Surname,
                FullName = request.FullName,
            };

            int _newUserId = _userContext.CreateAsync(_newUser).GetAwaiter().GetResult();
            _userCreatedSuccessfully = true;

            return Task.FromResult(new UserResponse
            {
                Id = _userCreatedSuccessfully ? _newUserId : -1
            });
        }

        public override Task<UsersList> Read(Empty request, ServerCallContext context)
        {
            var _users = _userContext
                .ReadAsync()
                .GetAwaiter()
                .GetResult()
                .Select(x => new UserData 
                { 
                    Id = x.Id,
                    Age = x.Age,
                    Name = x.Name,
                    Surname = x.Surname,
                    FullName = x.FullName
                })
                .ToList();


            UsersList _usersList = new UsersList();
            _usersList.UsersList_.AddRange(_users);

            return Task.FromResult(_usersList);
        }

        public override Task<UserData> Update(UserData request, ServerCallContext context)
        {
            UserModel _newUser = new UserModel()
            {
                Id = request.Id,
                Age = request.Age,
                Name = request.Name,
                Surname = request.Surname,
                FullName = request.FullName,
            };

            var _userUpdated = _userContext.UpdateAsync(_newUser).GetAwaiter().GetResult();

            return Task.FromResult(new UserData
            {
                Id = _userUpdated.Id,
                Age = _userUpdated.Age,
                Name = _userUpdated.Name,
                Surname = _userUpdated.Surname,
                FullName = _userUpdated.FullName
            });
        }

        public override Task<UserResponse> Delete(UserData request, ServerCallContext context)
        {
            int _deletedUserId = _userContext.DeleteAsync(request.Id).GetAwaiter().GetResult();

            return Task.FromResult(new UserResponse
            {
                Id = _deletedUserId
            });
        }
    }
}
