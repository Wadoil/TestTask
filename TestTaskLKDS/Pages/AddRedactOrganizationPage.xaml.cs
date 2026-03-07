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
    /// Логика взаимодействия для AddRedactOrganizationPage.xaml
    /// </summary>
    public partial class AddRedactOrganizationPage : Page
    {
        public class EmployeeData
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Position { get; set; }
        }

        private MainWindow _mainWindow = Application.Current.MainWindow as MainWindow;
        private Organization _organisation;
        private List<EmployeeData> _employeesData = new List<EmployeeData>();
        public AddRedactOrganizationPage(Organization organization)
        {
            InitializeComponent();

            _organisation = organization;

            _mainWindow.PageLabel.Text = "Редактирование организации";

            _mainWindow.BackBtn.Visibility = Visibility.Visible;

            NameEntry.Text = _organisation.Name;
            AddressEntry.Text = _organisation.Address;

            LoadEmployeesData();

            if (_employeesData != null)
                EmployeesListview.ItemsSource = _employeesData;
        }
        private void LoadEmployeesData()
        {
            try
            {
                foreach (Employee _employee in _mainWindow.Data.Employees.Where(x=> x.OrganizationID == _organisation.ID))
                {
                    EmployeeData _employeeData = new EmployeeData
                    {
                        ID = _employee.ID,
                        Name = _employee.Name,
                        Surname = _employee.Surname,
                        Position = _employee.Position
                    };
                    _employeesData.Add(_employeeData);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void OpenEmployee(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получение сотрудника из объекта listview
                var _border = sender as Border;
                var _grid = _border.Child as Grid;
                var _employeeData = _grid.DataContext as EmployeeData;
                Employee _employee = _mainWindow.Data.Employees.FirstOrDefault(x => x.ID == _employeeData.ID);

                _mainWindow.FrmMain.Navigate(new AddRedactEmployeePage(_employee));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void DeleteBtn_Click(Object sender, RoutedEventArgs e)
        {
            try
            {
                // Получение сотрудника из объекта listview
                var _border = sender as Border;
                var _grid = _border.Child as Grid;
                var _employeeData = _grid.DataContext as EmployeeData;
                var _employee = _mainWindow.Data.Employees.FirstOrDefault(x => x.ID == _employeeData.ID);

                _mainWindow.Data.Employees.Remove(_employee).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            SaveData();
        }
        private void SaveData()
        {
            try
            {
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
            EmployeesListview.ItemsSource = null;
            EmployeesListview.ItemsSource = _employeesData;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var _org = _mainWindow.Data.Organizations.FirstOrDefault(x => x.ID == _organisation.ID);
                _org.Name = NameEntry.Text;
                _org.Address = AddressEntry.Text;

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
