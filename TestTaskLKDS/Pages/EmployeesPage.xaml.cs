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

            List<string> _organizations = new List<string>();
            _organizations.Add("Без фильтра");
            for (int i = 0; i != _mainWindow.Data.Organizations.Count; i++)
                _organizations.Add(_mainWindow.Data.Organizations[i].Name);
            OrganizationBox.ItemsSource = _organizations;
            OrganizationBox.SelectedIndex = 0;

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
                        Position = _employee.Position,
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

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            EmployeesListview.ItemsSource = null;
            EmployeesListview.ItemsSource = _employeesData;
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            string _searchText = SearchBar.Text?.ToLower().Trim();

            if ((string.IsNullOrWhiteSpace(_searchText) || _searchText == "поиск") && OrganizationBox.SelectedIndex == 0)
            {
                EmployeesListview.ItemsSource = _employeesData;
            }
            else if ((string.IsNullOrWhiteSpace(_searchText) || _searchText == "поиск") && OrganizationBox.SelectedIndex != 0)
            {
                var _filteredEmployees = _employeesData.Where(x => x.Organization == _mainWindow.Data.Organizations.FirstOrDefault(y => y.Name == OrganizationBox.SelectedItem.ToString()).Name);
                EmployeesListview.ItemsSource = null;
                EmployeesListview.ItemsSource = _filteredEmployees;
            }
            else if (!(string.IsNullOrWhiteSpace(_searchText) || _searchText == "поиск") && OrganizationBox.SelectedIndex == 0)
            {
                var _filteredEmployees = _employeesData.Where(x => x.Surname.ToLower().Contains(_searchText));
                EmployeesListview.ItemsSource = null;
                EmployeesListview.ItemsSource = _filteredEmployees;
            }
            else
            {
                var _filteredEmployees = _employeesData.Where(x => x.Surname.ToLower().Contains(_searchText) && x.Organization == _mainWindow.Data.Organizations.FirstOrDefault(y => y.Name == OrganizationBox.SelectedItem.ToString()).Name);
                EmployeesListview.ItemsSource = null;
                EmployeesListview.ItemsSource = _filteredEmployees;
            }
        }
    }
}
