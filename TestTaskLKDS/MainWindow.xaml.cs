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
using TestTaskLKDS.Pages;

namespace TestTaskLKDS
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string FileName = "TestData.json";
        public DataBase Data;
        public MainWindow()
        {
            Logger.Info("Инициализация приложения");
            InitializeComponent();
            try
            {
                LoadData(File.ReadAllText(FileName));
                FrmMain.Navigate(new OrganizationsPage());
            }
            catch (Exception ex) 
            {
                Logger.Error($"При инициализации приложения произошла ошибка: {ex.Message}");
            }
        }

        private void GenerateBtn_Click(object sender, RoutedEventArgs e)
        {
            Logger.Info("Начата генерация тестового набора данных");
            try
            {
                List<Organization> Organizations = new List<Organization>();
                List<Employee> Employees = new List<Employee>();
                int j = 0, f = 10;
                for (int i = 0; i != 3; i++)
                {
                    Organizations.Add(new Organization() { ID = i, Name = $"Test organization {i}", Address = "Test address" });
                    for (; j != f; j++)
                        Employees.Add(new Employee() { ID = j, Name = "Test name", Surname = "Test surname", OrganizationID = i, Position = "Test position" });
                    f += 10;
                }
                var TestData = new DataBase
                {
                    Organizations = Organizations,
                    Employees = Employees
                };

                try
                {
                    var options = new JsonSerializerOptions { WriteIndented = true }; // Запись по столбцам
                    string jsonString = JsonSerializer.Serialize(TestData, options);
                    File.WriteAllText(FileName, jsonString);

                    Logger.Info("Тестовый набор данных создан");
                }
                catch (Exception ex)
                {
                    Logger.Error($"При записи данных произошла ошибка: {ex.Message}");
                    MessageBox.Show("При записи данных произошла ошибка");
                }

                LoadData(File.ReadAllText(FileName));
                FrmMain.Navigate(new OrganizationsPage());
                }
            catch (Exception ex) 
            {
                Logger.Error($"При создании тестовых данных произошлао ошибка: {ex.Message}");
                MessageBox.Show("При создании тестовых данных произошлао ошибка");
            }
        }
        public void LoadData(string DataFile)
        {
            try
            {
                Data = JsonSerializer.Deserialize<DataBase>(DataFile);
                GenerateBtn.Visibility = Visibility.Collapsed;
                FrmMain.Visibility = Visibility.Visible;
                OrganizationsBtn.Visibility = Visibility.Visible;
                EmployeesBtn.Visibility = Visibility.Visible;

                Logger.Info("Данные успешно загружены");
            }
            catch (Exception ex)
            {
                Logger.Error($"При загрузке данных произошла ошибка: {ex.Message}");
                MessageBox.Show("При загрузке данных произошла ошибка");
            }
        }
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            FrmMain.GoBack();
        }

        private void OrganizationsBtn_Click(Object sender, RoutedEventArgs e)
        {
            FrmMain.Navigate(new OrganizationsPage());
        }
        private void EmployeesBtn_Click(Object sender, RoutedEventArgs e)
        {
            FrmMain.Navigate(new EmployeesPage());
        }
    }
}
