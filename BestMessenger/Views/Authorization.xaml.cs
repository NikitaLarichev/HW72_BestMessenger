using BestMessenger.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BestMessenger.Views
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        MainWindow mainWindow;
        public Authorization()
        {
            InitializeComponent();
        }

        private async void enter_button_Click(object sender, RoutedEventArgs e)
        {
            await authorizationViewModel.CreateUserAndFindItInDB();
            if (authorizationViewModel.MainUser != null)
            {
                mainWindow = new MainWindow(authorizationViewModel.MainUser);
                mainWindow.ShowDialog();
            }
        }
    }
}
