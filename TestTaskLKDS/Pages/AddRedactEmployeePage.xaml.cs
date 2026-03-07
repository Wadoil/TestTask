using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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
using System.IO;

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

            _mainWindow.PageLabel.Text = "Редактирование сотрудника";
            _mainWindow.BackBtn.Visibility = Visibility.Visible;

            NameEntry.Text = _employee.Name;
            SurnameEntry.Text = _employee.Surname;
            PositionEntry.Text = _employee.Position;
            List<string> _organizations = new List<string>();
            for (int i = 0; i != _mainWindow.Data.Organizations.Count; i++) 
                _organizations.Add(_mainWindow.Data.Organizations[i].Name);
            OrganizationBox.ItemsSource = _organizations;
            OrganizationBox.SelectedIndex = _employee.OrganizationID;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var _emp = _mainWindow.Data.Employees.FirstOrDefault(x => x.ID == _employee.ID);
                _emp.Name = NameEntry.Text;
                _emp.Surname = SurnameEntry.Text;
                _emp.Position = PositionEntry.Text;
                _emp.OrganizationID = OrganizationBox.SelectedIndex;

                var TestData = new DataBase
                {
                    Organizations = _mainWindow.Data.Organizations,
                    Employees = _mainWindow.Data.Employees
                };

                var options = new JsonSerializerOptions { WriteIndented = true }; // Запись по столбцам
                string jsonString = JsonSerializer.Serialize(TestData, options);
                File.WriteAllText(_mainWindow.FileName, jsonString);
            }
            catch
            {
                MessageBox.Show("Произошла ошибка при сохранении данных");
            }
        }
    }
}
