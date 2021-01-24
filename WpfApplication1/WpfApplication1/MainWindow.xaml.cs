using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                FileLogger.WriteToFile(DateTime.Now + ": Программа запущена");
                DataBase.CreateDBFile();
                DataBase.CreateUserTable();
                DataBase.CreateRecordMetaTable();
                DataBase.CreateRecordTable();
            }
            catch (Exception err)
            {
                MessageBox.Show("Ошибка при запуске приложения: " + err.Message);
                FileLogger.WriteToFile("Ошибка: " + err.Message);
                Close();
            }
        }

        private void RegistrationButtonClick(object sender, RoutedEventArgs e)
        {
            var regWindow = new Registration();
            regWindow.Show();
            Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            FileLogger.WriteToFile(DateTime.Now + ": Программа закрыта");
        }


        private void LoginButton_OnClick(object sender, RoutedEventArgs e)
        {
            string login = LoginInput.Text;
            string password = PasswordInput.Password;

            User user = DataBase.GetUser(login, password);

            if (user == null)
            {
                MessageBox.Show("Ошибка при вводе логина или пароля!", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                AppState.CurrentUser = user;
                var rw = new ReportWindow();
                rw.Show();
                AppState.ReportWindow = rw;
                Close();
            }
        }
        
        public void RemoveText(object sender, EventArgs e)
        {
            if ((sender as TextBox).Text == "Текст...") 
            {
                (sender as TextBox).Text = "";
            }
        }

        public void AddText(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace((sender as TextBox).Text))
                (sender as TextBox).Text = "Текст...";
        }
        
        public void PasswordRemoveText(object sender, EventArgs e)
        {
            if ((sender as PasswordBox).Password == "Текст...") 
            {
                (sender as PasswordBox).Password = "";
            }
        }

        public void PasswordAddText(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace((sender as PasswordBox).Password))
                (sender as PasswordBox).Password = "Текст...";
        }
    }
}