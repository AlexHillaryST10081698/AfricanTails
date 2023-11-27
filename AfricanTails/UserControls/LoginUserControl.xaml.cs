using AfricanTails.Classes;
using AfricanTails.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace AfricanTails.UserControls
{
    /// <summary>
    /// Interaction logic for LoginUserControl.xaml
    /// </summary>
    public partial class LoginUserControl : UserControl
    {
        //Variables
        private String Username;
        private String Password;

        OverviewWindow OW = new OverviewWindow();
        
        public LoginUserControl()
        {
            InitializeComponent();
            OW.WindowState = WindowState.Maximized;
            
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            Username = UsernameTxtBox.Text;
            // PasswordBox stores the password as a SecureString, so you should convert it to a plain string.
            Password = ConvertToUnsecureString(PasswordTxtBox.SecurePassword);
            DatabaseHandler db = new DatabaseHandler();
            bool LoginSucess = db.VerifyLogin(Username, Password);
            //---------------------------Displays a meesage if username and/or password does not match-------------------------------------------------------------//
            if (LoginSucess)
            {
                MessageBox.Show("Login Succeeded", "Valid Credentials", MessageBoxButton.OK, MessageBoxImage.Information);
                OW.Show();
                MainWindow MW = new MainWindow();
                MW.Close();

            }
            //---------------------------Displays a meesage if username and/or password does not match-------------------------------------------------------------//
            else
            {
                MessageBox.Show("You have entered an invalid username or password", "Invalid User", MessageBoxButton.OK, MessageBoxImage.Warning);
                PasswordTxtBox.Clear();
            }
            

        }
        // Helper method to convert SecureString to plain string
        private string ConvertToUnsecureString(SecureString securePassword)
        {
            if (securePassword == null)
            {
                return string.Empty;
            }

            IntPtr unmanagedPassword = IntPtr.Zero;
            try
            {
                unmanagedPassword = System.Runtime.InteropServices.Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return System.Runtime.InteropServices.Marshal.PtrToStringUni(unmanagedPassword);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeGlobalAllocUnicode(unmanagedPassword);
            }
        }

        private void SignUpBtn_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow RW = new RegisterWindow();
            RW.WindowState = WindowState.Maximized;
            RW.Show();
        }
    }
}
