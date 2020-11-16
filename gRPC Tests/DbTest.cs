using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using gRPC_Database.Contexts;
using gRPC_Database.Models;

namespace gRPC_Tests
{
    [TestClass]
    public class DbTest
    {
        [TestMethod]
        public void CreateUserTestMethod()
        {
            UserContext userContext = new UserContext();
            UserModel user = new UserModel() { Age = 18, Name = "Faridun", Surname = "Berdiev", FullName = "Faridun Berdiev"};
            UserModel user1 = new UserModel() { Age = 19, Name = "Ramz", Surname = "Nazarov", FullName = "Ramz Nazarov" };
            UserModel user2 = new UserModel() { Age = 20, Name = "Sorbon", Surname = "Rashidov", FullName = "Sorbon Rashidov" }; 

            userContext.CreateAsync(user).GetAwaiter().GetResult();
            userContext.CreateAsync(user1).GetAwaiter().GetResult();
            userContext.CreateAsync(user2).GetAwaiter().GetResult();

            Assert.AreEqual(user, userContext.ReadAsync().GetAwaiter().GetResult().FirstOrDefault(x => x.Id == 1));
            Assert.AreEqual(user1, userContext.ReadAsync().GetAwaiter().GetResult().FirstOrDefault(x => x.Id == 2));
            Assert.AreEqual(user2, userContext.ReadAsync().GetAwaiter().GetResult().FirstOrDefault(x => x.Id == 3));
        }

        [TestMethod]
        public void UpdateUserTestMethod()
        {
            UserContext userContext = new UserContext();
            UserModel userEntity = new UserModel() { Age = 18, Name = "Faridun", Surname = "Berdiev", FullName = "Faridun Berdiev" };
            UserModel userNewVersion = new UserModel() { Id = 1, Age = 22, Name = "Ramzier", Surname = "Nazarov", FullName = "Ramzier Nazarov" };

            int newUserId = userContext.CreateAsync(userEntity).GetAwaiter().GetResult();
            var userExist = userContext.ReadAsync().GetAwaiter().GetResult().FirstOrDefault(x => x.Id == newUserId);
            UserModel userEdited = userContext.UpdateAsync(newUserId, userNewVersion).GetAwaiter().GetResult();
            
            Assert.AreEqual(userNewVersion.FullName, userEdited.FullName);
        }

        [TestMethod]
        public void DeleteUserTestMethod()
        {
            UserContext userContext = new UserContext();
            UserModel user = new UserModel() { Age = 20, Name = "Sorbon", Surname = "Rashidov", FullName = "Sorbon Rashidov" };

            int newUserId = userContext.CreateAsync(user).GetAwaiter().GetResult();
            int userDeletedId = userContext.DeleteAsync(newUserId).GetAwaiter().GetResult();

            Assert.AreEqual(newUserId, userDeletedId);
        }
    }
}
