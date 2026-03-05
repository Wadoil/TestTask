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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Text.Json;
using TestTaskLKDS.Models;

namespace TestTaskLKDS.Pages
{
    /// <summary>
    /// Логика взаимодействия для OrganizationsPage.xaml
    /// </summary>
    public partial class OrganizationsPage : Page
    {
        private DataBase _data;
        private MainWindow _mainWindow = Application.Current.MainWindow as MainWindow;
        public OrganizationsPage(string DataFile)
        {
            InitializeComponent();

            _mainWindow.PageLabel.Text = "Организации";

            LoadData(DataFile);
        }
        public void LoadData(string DataFile)
        {
            try
            {
                _data = JsonSerializer.Deserialize<DataBase>(DataFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            if (_data != null)
                OrganizationsListview.ItemsSource = _data.Organizations;
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            int _id = _data.Organizations.Count;
            _data.Organizations.Add(new Organization() {ID=_id, Name="Added org", Address="Added address" });

            SaveData();

            OrganizationsListview.ItemsSource = null;
            OrganizationsListview.ItemsSource = _data.Organizations;       
        }
        private void SaveData()
        {
            try
            {
                var TestData = new DataBase
                {
                    Organizations = _data.Organizations,
                    Employees = _data.Employees,
                    Positions = _data.Positions
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
