using Microsoft.Extensions.Options;
using Module25.PLL.Views;

namespace Module25
{
    public class Program
    {
        public static string connectionString;

        static void Main()
        {
           //Чтобы было удобнее - у меня СУБД развернута на VM вне домена, поэтому вход по SQL login
           connectionString = @"Server=WIN-ACL07JRD9H1\SQLEXPRESS;TrustServerCertificate=True;Trusted_Connection=false;Database=HomeWork;User ID=sa;Password=1q2W3e4R";
           //connectionString = @"Data Source=.\SQLEXPRESS;Database=HomeWork;TrustServerCertificate=True;Trusted_Connection=True;Integrated Security = true";
           
            var mainView = new MainView();
            mainView.Show();
        }
    }
}
