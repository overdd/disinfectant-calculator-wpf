using System;
using System.ComponentModel;
using System.Windows;

namespace WpfApplication1
{
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            new MainWindow().Show();
        }
        
        private void RegButton_OnClick(object sender, RoutedEventArgs e)
        {
            string login = LoginInput.Text;
            string password = PasswordInput.Text;
            int is_admin = IsAdminCheckBox.IsChecked.Value ? 1 : 0;
            try
            {
                ValidationMethods.ValidateUserCredentials(login, password);
                DataBase.InsertUser(login, password, is_admin);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            
            MessageBox.Show("Регистрация успешна!", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            
        }
    }
}