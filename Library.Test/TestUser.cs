using Library.Domain.Interfaces.Repositories;
using Library.Domain.Models.Implementataions;
using Library.Test.Implenetations;

namespace Library.Test
{
    public class TestUser
    {
        private IUnitOfWork db;
        [SetUp]
        public void Setup()
        {
            var userRepo = new UserRepositoryMock();
            db = new UnitOfWorkMock
            {
                UserRepository = userRepo
            };
        }

        [Test]
        public void TestPassword()
        {
            string password = "123456";

            var user = new UserModel(/*db*/);

            user.Password = user.HashedPassword(password);

            Console.WriteLine(user.Password);

            Assert.IsTrue(user.VerifyPassword(password));
        }

        [Test]
        public void TestEmail()
        {
            string email = "denis@mail.ru";
            string email1 = "denimail.ru";
            string email2 = "denim@ailu";
            var user = new UserModel(/*db*/);

            user.Email = email;

            Assert.AreEqual(email, user.Email);
            user.Email = email1;
            Assert.AreNotEqual(email1, user.Email);
            user.Email = email2;
            Assert.AreEqual(email2, user.Email);


        }
    }
}