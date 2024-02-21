using Module25.PLL.Views;

namespace Module25
{
    public class Program
    {
        public static string connectionString;

        static void Main()
        {
           connectionString = @"Server=WIN-ACL07JRD9H1\SQLEXPRESS;TrustServerCertificate=True;Trusted_Connection=false;Database=HomeWork;User ID=sa;Password=1q2W3e4R";
            
            var mainView = new MainView();
            mainView.Show();
        }
    }
}
