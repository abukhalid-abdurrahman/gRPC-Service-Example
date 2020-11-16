using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gRPC_Database.Models;
using gRPC_Database.Interfaces;

namespace gRPC_Database.Contexts
{
    public class UserContext : IContext<UserModel>
    {
        private List<UserModel> Users = new List<UserModel>() { new UserModel {Age = 1, Id = 1, FullName = " ", Name = " ", Surname = " " }, new UserModel { Age = 1, Id = 2, FullName = " ", Name = " ", Surname = " " } };
        private int currentUserId = 0;
        public async Task<int> CreateAsync(UserModel user)
        {
            currentUserId++;
            user.Id = currentUserId;

            await Task.Run(() =>
            {
                Users.Add(user);
            });

            return user.Id;
        }
        public async Task<List<UserModel>> ReadAsync()
        {
            return await Task.Run(() =>
            {
                return Users;
            });
        }
        public async Task<UserModel> UpdateAsync(UserModel user)
        {
            return await Task.Run(() =>
            {
                var userEntity = Users.FirstOrDefault(x => x.Id == user.Id);

                userEntity.Age = user.Age;
                userEntity.Name = user.Name;
                userEntity.Surname = user.Surname;
                userEntity.FullName = user.FullName;

                return userEntity;
            });
        }
        public async Task<int> DeleteAsync(int userId)
        {
            int removedUserId = 0;

            await Task.Run(() =>
            {
                removedUserId = Users.RemoveAll(x => x.Id == userId);
            });

            return removedUserId > 0 ? userId : -1;
        }
    }
}
