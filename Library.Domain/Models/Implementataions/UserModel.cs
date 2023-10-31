using Library.Domain.Models.Interfaces;
using Library.Common.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Security.Cryptography;

namespace Library.Domain.Models.Implementataions
{
    public class UserModel : IUser
    {
        private Guid _id;
        private string _name;
        private string _surname;
        private string _email;
        private int _age;
        private string _phoneNumber;
        private string _password;
        private DateTime _registerDate = default(DateTime);

        public UserModel()
        {
        }

        public Guid Id 
        {
            get 
            {
                if(Guid.Empty == _id || _id == null)
                {
                    _id = Guid.NewGuid();
                }
                return _id;
            }
            set { _id = value; }
        }
        public string Name { get { return _name; } set { _name = value; } }
        public string Surname { get { return _surname; } set { _surname = value; } }
        public string Email
        {
            get { return _email; }
            set
            {
                if(new EmailAddressAttribute().IsValid(value))
                {
                    _email = new MailAddress(value).Address;
                }
                else
                {
                    throw new ResponseResultException(System.Net.HttpStatusCode.BadRequest, $"Not valid Email {value}", "Email");
                }
            }
        }
        public int Age
        {
            get { return _age; }
            set
            {
                if (value < 0 || value > 120) throw new ResponseResultException(System.Net.HttpStatusCode.BadRequest, $"Age out of range. Got: {value}. Expected Age > 0 and Age < 120", "Age"); 
                else
                {
                    _age = value;
                }
            }
        }
        public string PhoneNumber { get { return _phoneNumber; } set { _phoneNumber = value; } }
        public string Password { 
            get { return _password; } 
            set
            {
                byte[] salt;
                new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
                var pbkdf2 = new Rfc2898DeriveBytes(value, salt, 10000);
                byte[] hash = pbkdf2.GetBytes(20);
                byte[] hashBytes = new byte[36];
                Array.Copy(salt, 0, hashBytes, 0, 16);
                Array.Copy(hash, 0, hashBytes, 16, 20);
                _password = Convert.ToBase64String(hashBytes);
            }
        }
        public string Description { get; set; }
        public DateTime RegisterDate { get 
            {
                if (_registerDate == default(DateTime))
                {
                    _registerDate = DateTime.Now;
                    return DateTime.Now;
                }
                return _registerDate; 
            }
            set
            {
                _registerDate = value;
            }
        }

        public List<IBook> BooksViewHistory { get; set; }

        public bool VerifyPassword(string password)
        {
            /* Fetch the stored value */
            string savedPasswordHash = _password;
            /* Extract the bytes */
            byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);
            /* Get the salt */
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            /* Compute the hash on the password the user entered */
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            /* Compare the results */
            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    return false;
            return true;
        }
    }
}
