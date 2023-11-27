using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using Microsoft.Extensions.Configuration;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using System.IO;


namespace AfricanTails.UserControls
{
    /// <summary>
    /// Interaction logic for ReminderUserControl.xaml
    /// </summary>
    public partial class ReminderUserControl : UserControl
    {
        private bool reminderSent = false;
        private System.Timers.Timer reminderTimer; // Timer for scheduling reminders  
        private IConfiguration Configuration;
        public ReminderUserControl()
        {
            InitializeComponent();
            LoadConfiguration();
        }

        private void LoadConfiguration()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }

        private void SendReminderButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleTextBox.Text;
            string description = DescriptionTextBox.Text;
            DateTime selectedDate = ReminderDatePicker.SelectedDate ?? DateTime.Now;

            SendReminderEmail(title, description, selectedDate);
            MessageBox.Show("Email reminder sent successfully!");
        }

        private void SendReminderEmail(string title, string description, DateTime selectedDate)
        {
            string toEmail = Configuration["EmailSettings:SMTPUsername"];
            string subject = "Reminder: " + title;
            string body = $"Title: {title}\nDescriptions: {description}\nReminder Date: {selectedDate}\n\n";

            string smtpServer = Configuration["EmailSettings:SMTPServer"];
            int smtpPort = Convert.ToInt32(Configuration["EmailSettings:SMTPPort"]);
            string smtpUsername = Configuration["EmailSettings:SMTPUsername"];
            string smtpPassword = Configuration["EmailSettings:SMTPPassword"];

            SmtpClient smtpClient = new SmtpClient(smtpServer)
            {
                Port = smtpPort,
                Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                EnableSsl = true,
            };

            try
            {
                MailMessage mailMessage = new MailMessage(smtpUsername, toEmail, subject, body);
                smtpClient.Send(mailMessage);

                // Append the new reminder details to the existing content in EmailDetailsTextBox
                EmailDetailsTextBox.AppendText(body);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending email reminder: {ex.Message}");
            }
        }

       
    }
}
