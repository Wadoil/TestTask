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
        private MainWindow _mainWindow = Application.Current.MainWindow as MainWindow;
        private Organization _organisation;
        public AddRedactOrganizationPage(Organization organization)
        {
            InitializeComponent();

            _organisation = organization;

            _mainWindow.PageLabel.Text = "Редактирование организации";

            _mainWindow.BackBtn.Visibility = Visibility.Visible;

            NameEntry.Text = _organisation.Name;
            AddressEntry.Text = _organisation.Address;
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
        }
    }
}
