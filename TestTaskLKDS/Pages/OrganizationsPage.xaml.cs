using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
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
    /// TODO: Добавить кнопку "обновить"/обновлять данные после перехода со страницы редактирования (желательно)
    public partial class OrganizationsPage : Page
    {
        private MainWindow _mainWindow = Application.Current.MainWindow as MainWindow;
        private List<Organization> organizations;
        public OrganizationsPage()
        {
            InitializeComponent();

            _mainWindow.PageLabel.Text = "Организации";

            organizations = _mainWindow.Data.Organizations;

            if (organizations != null)
                OrganizationsListview.ItemsSource = organizations;
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            // TODO: переход на страницу добавления/редактирования, остальное вырезать
            int _id = organizations.Count;
            organizations.Add(new Organization() { ID = _id, Name = "Added org", Address = "Added address" });

            SaveData();
        }
        private void OpenOrganization(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получение организации из объекта listview
                var border = sender as Border;
                var grid = border.Child as Grid;
                var organization = grid?.DataContext as Organization;

                _mainWindow.FrmMain.Navigate(new AddRedactPage(organization));
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
                var button = sender as Button;
                var grid = button.Parent as Grid;
                var organization = grid?.DataContext as Organization;

                organizations.Remove(organization).ToString();
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
                    Organizations = organizations,
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
            OrganizationsListview.ItemsSource = null;
            OrganizationsListview.ItemsSource = organizations;
        }
    }
}
