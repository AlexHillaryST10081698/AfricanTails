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

namespace AfricanTails.UserControls
{
    /// <summary>
    /// Interaction logic for RegisterUserControl.xaml
    /// </summary>
    public partial class RegisterUserControl : UserControl
    {
        //variables 
        private String RegisterFirstname;
        private String RegisterLastname;
        private String RegisterUsername;
        private String RegisterPassword;
        private String RegisterConfirmPassword;

        public RegisterUserControl()
        {
            InitializeComponent();
        }

        //Button click to Sign Up
        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            RegisterFirstname = First_NameTxt.Text;
            RegisterLastname = Last_NameTxt.Text;
            RegisterUsername = UsernameTxt.Text;
            RegisterPassword =ConvertToUnsecureString(PasswordTxt.SecurePassword);
            PasswordEncryption PE = new PasswordEncryption();
            string EncryptedPassword = PE.HashedPassword(RegisterPassword);
            RegisterConfirmPassword = ConvertToUnsecureString(Confirm_PasswordTxt.SecurePassword);
            if (RegisterPassword.Equals(RegisterConfirmPassword))
            {
                DatabaseHandler DB = new DatabaseHandler();
                DB.RegisterUserToDatabase(RegisterUsername, RegisterFirstname, RegisterLastname, EncryptedPassword);
                MessageBox.Show("New User Captured");
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                RegisterWindow registerWindow = new RegisterWindow();
                registerWindow.Close();
                
            }
            else
            {
                MessageBox.Show("Please Make Sure Your Password Are The Same");
            }

        }
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
    }
}
