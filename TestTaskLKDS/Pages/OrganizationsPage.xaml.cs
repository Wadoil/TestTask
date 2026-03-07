using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
using TestTaskLKDS.Models;

namespace TestTaskLKDS.Pages
{
    /// <summary>
    /// Логика взаимодействия для OrganizationsPage.xaml
    /// </summary>
    public partial class OrganizationsPage : Page
    {
        private MainWindow _mainWindow = Application.Current.MainWindow as MainWindow;
        private List<Organization> _organizations;
        public OrganizationsPage()
        {
            InitializeComponent();

            _mainWindow.PageLabel.Text = "Организации";

            _organizations = _mainWindow.Data.Organizations;

            if (_organizations != null)
                OrganizationsListview.ItemsSource = _organizations;
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            int _id = _organizations.Count;
            Organization _organization = new Organization() { ID = _id, Name = "Новая орагнизация", Address = "" };
            _organizations.Add(_organization);

            SaveData();

            _mainWindow.FrmMain.Navigate(new AddRedactOrganizationPage(_organization));
        }
        private void OpenOrganization(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получение организации из объекта listview
                var _border = sender as Border;
                var _grid = _border.Child as Grid;
                var _organization = _grid.DataContext as Organization;

                _mainWindow.FrmMain.Navigate(new AddRedactOrganizationPage(_organization));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получение организации из объекта listview
                var _button = sender as Button;
                var _grid = _button.Parent as Grid;
                var _organization = _grid?.DataContext as Organization;

                _organizations.Remove(_organization).ToString();
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
                    Organizations = _organizations,
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
            OrganizationsListview.ItemsSource = null;
            OrganizationsListview.ItemsSource = _organizations;
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            OrganizationsListview.ItemsSource = null;
            OrganizationsListview.ItemsSource = _organizations;
        }
    }
}
