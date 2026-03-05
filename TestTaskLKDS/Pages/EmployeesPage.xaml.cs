using System;
using System.Collections.Generic;
using System.IO;
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
using static TestTaskLKDS.Pages.EmployeesPage;

namespace TestTaskLKDS.Pages
{
    /// <summary>
    /// Логика взаимодействия для EmployeesPage.xaml
    /// </summary>
    public partial class EmployeesPage : Page
    {
        private MainWindow _mainWindow = Application.Current.MainWindow as MainWindow;
        private List<EmployeeData> _employeesData = new List<EmployeeData>();
        public class EmployeeData
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Position { get; set; }
            public string Organization { get; set; }
        }
        public EmployeesPage()
        {
            InitializeComponent();

            _mainWindow.PageLabel.Text = "Сотрудники";

            LoadData();

            if (_employeesData != null)
                EmployeesListview.ItemsSource = _employeesData;
        }
        private void LoadData()
        {
            try
            {
                foreach (Employee _employee in _mainWindow.Data.Employees)
                {
                    EmployeeData _employeeData = new EmployeeData
                    {
                        ID = _employee.ID,
                        Name = _employee.Name,
                        Surname = _employee.Surname,
                        Position = _mainWindow.Data.Positions.FirstOrDefault(x => x.ID == _employee.PositionID).Name,
                        Organization = _mainWindow.Data.Organizations.FirstOrDefault(x => x.ID == _employee.OrganizationID).Name
                    };
                    _employeesData.Add(_employeeData);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            int _id = _employeesData.Count;
            EmployeeData _employeeData = new EmployeeData() { ID = _id, Name = "Новый сотрудник"};
            _employeesData.Add(_employeeData);
            Employee _employee = _mainWindow.Data.Employees.FirstOrDefault(x => x.ID == _employeeData.ID);

            SaveData();

            _mainWindow.FrmMain.Navigate(new AddRedactEmployeePage(_employee));
        }

        private void OpenEmployee(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получение сотрудника из объекта listview
                var _border = sender as Border;
                var _grid = _border.Child as Grid;
                var _employee = _grid.DataContext as Employee;

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
                    Employees = _mainWindow.Data.Employees,
                    Positions = _mainWindow.Data.Positions
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

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            EmployeesListview.ItemsSource = null;
            EmployeesListview.ItemsSource = _employeesData;
        }
    }
}
