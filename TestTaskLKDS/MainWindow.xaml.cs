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
using System.IO;
using System.Text.Json;
using TestTaskLKDS.Models;

namespace TestTaskLKDS
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GenerateBtn_Click(object sender, RoutedEventArgs e)
        {
            // testing serialization
            // subjects are about to change DRASTICALLY
            var Organization = new Organization
            {
                ID = 0,
                Name = "Test organization",
                Address = "Test address"
            };
            var position = new Position
            {
                ID = 0,
                Name = "Test position"
            };
            var Employee = new Employee
            {
                ID = 0,
                Name = "Test name",
                Surname = "Test surname",
                PositionID = 0,
                OrganizationID = 0
            };
            var TestData = new TestData
            {
                Organizations = {Organization},
                Employees = {Employee},
                Positions = {position}
            };

            string fileName = "TestData.json";
            FileStream createStream = File.Create(fileName);
            JsonSerializer.SerializeAsync(createStream, position);

            Console.WriteLine(File.ReadAllText(fileName));
        }
        public class TestData
        {
            public List<Organization> Organizations { get; set; }
            public List<Employee> Employees { get; set; }  
            public List<Position> Positions { get; set; }
        }
    }
}
