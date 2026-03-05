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
        public bool isDataLoaded = false;
        public string DataFile;
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                DataFile = File.ReadAllText(FileName);
                isDataLoaded = true;
                GenerateBtn.Visibility = Visibility.Collapsed;
                FrmMain.Visibility = Visibility.Visible;
                FrmMain.Navigate(new OrganizationsPage(DataFile));
            }
            catch { }
        }

        private void GenerateBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Organization> Organizations = new List<Organization>();
                List<Employee> Employees = new List<Employee>();
                List<Position> Positions = new List<Position>();
                for (int i = 0; i != 3; i++)
                {
                    Organizations.Add(new Organization() { ID = i, Name = "Test organization", Address = "Test address" });
                    for (int j = 0; j != 10; j++)
                        Employees.Add(new Employee() { ID = j, Name = "Test name", Surname = "Test surname", OrganizationID = i, PositionID = j });
                }
                for (int i = 0; i != 10; i ++)
                    Positions.Add(new Position() { ID = i, Name = "Test position" });
                var TestData = new DataBase
                {
                    Organizations = Organizations,
                    Employees = Employees,
                    Positions = Positions
                };
                try
                {
                    var options = new JsonSerializerOptions { WriteIndented = true }; // Запись по столбцам
                    string jsonString = JsonSerializer.Serialize(TestData, options);
                    File.WriteAllText(FileName, jsonString);

                    MessageBox.Show("Тестовый набор данных создан");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                GenerateBtn.Visibility = Visibility.Collapsed;
                FrmMain.Visibility = Visibility.Visible;
                DataFile = File.ReadAllText(FileName);
                FrmMain.Navigate(new OrganizationsPage(DataFile));
                }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
