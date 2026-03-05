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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestTaskLKDS.Models;

namespace TestTaskLKDS.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddRedactEmployeePage.xaml
    /// </summary>
    public partial class AddRedactEmployeePage : Page
    {
        private MainWindow _mainWindow = Application.Current.MainWindow as MainWindow;
        private Employee _employee;
        public AddRedactEmployeePage(Employee employee)
        {
            InitializeComponent();

            _employee = employee;

            _mainWindow.BackBtn.Visibility = Visibility.Visible;
        }
    }
}
