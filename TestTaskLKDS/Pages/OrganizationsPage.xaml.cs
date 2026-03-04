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
using TestTaskLKDS.Models;

namespace TestTaskLKDS.Pages
{
    /// <summary>
    /// Логика взаимодействия для OrganizationsPage.xaml
    /// </summary>
    public partial class OrganizationsPage : Page
    {
        private DataBase _data;
        public OrganizationsPage(string DataFile)
        {
            InitializeComponent();

            var mainWindow = Application.Current.MainWindow as MainWindow;

            mainWindow.PageLabel.Text = "Организации";

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
    }
}
