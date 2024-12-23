using System.Security.Cryptography;
using System.Text;

namespace Gadelshin_Lab1.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Role { get; set; } // admin, reader
        public List<Book> BorrowedBooks { get; set; }

        private byte[] password;
        public string Password
        {
            get
            {
                var sb = new StringBuilder();
                foreach (var b in MD5.Create().ComputeHash(password))
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
            set { password = Encoding.UTF8.GetBytes(value); }
        }

        public User()
        {
            BorrowedBooks = new List<Book>();
        }

        // Methods
        public bool IsAdmin => Role == "admin";
        public bool CheckPassword(string password) => password == Password;

        public bool CanBorrowBooks()
        {
            return Role == "reader";
        }

        public void BorrowBook(Book book)
        {
            if (CanBorrowBooks())
            {
                book.UserId = this.Id;
                BorrowedBooks.Add(book);
            }
        }
    }
}
