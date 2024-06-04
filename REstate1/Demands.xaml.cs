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
using System.Windows.Shapes;
using System.Data.Entity;

namespace REstate1
{
    /// <summary>
    /// Логика взаимодействия для Demands.xaml
    /// </summary>
    public partial class Demands : Window
    {
        public Demands()
        {
            InitializeComponent();
            Loaded += LoadDemands;
        }
        private void Clients_Click(object sender, RoutedEventArgs e)
        {
            Clients clients = new Clients();
            this.Close();
            clients.Show();
        }

        private void Agents_Click(object sender, RoutedEventArgs e)
        {
            Agents agents = new Agents();
            this.Close();
            agents.Show();
        }

        private void RealEstates_Click(object sender, RoutedEventArgs e)
        {
            RealEstates realEstates = new RealEstates();
            this.Close();
            realEstates.Show();
        }

        private void Supplies_Click(object sender, RoutedEventArgs e)
        {
            Supplies supplies = new Supplies();
            this.Close();
            supplies.Show();
        }

        private void Deals_Click(object sender, RoutedEventArgs e)
        {
            Deals deals = new Deals();
            this.Close();
            deals.Show();
        }

        private void LoadDemands(object sender, RoutedEventArgs e)
        {
            using (var context = new RealEstateContext())
            {
                DemandsListBox.ItemsSource = context.Demands.ToList();
            }
        }

        private void TypeComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var selectedType = (TypeComboBox.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content.ToString();

            if (selectedType == "Квартира" || selectedType == "Дом")
            {
                MinAreaTextBox.Visibility = Visibility.Visible;
                MaxAreaTextBox.Visibility = Visibility.Visible;
                MinRoomsTextBox.Visibility = Visibility.Visible;
                MaxRoomsTextBox.Visibility = Visibility.Visible;
                MinFloorTextBox.Visibility = Visibility.Visible;
                MaxFloorTextBox.Visibility = Visibility.Visible;
            }
            else if (selectedType == "Земля")
            {
                MinAreaTextBox.Visibility = Visibility.Visible;
                MaxAreaTextBox.Visibility = Visibility.Visible;
                MinRoomsTextBox.Visibility = Visibility.Collapsed;
                MaxRoomsTextBox.Visibility = Visibility.Collapsed;
                MinFloorTextBox.Visibility = Visibility.Collapsed;
                MaxFloorTextBox.Visibility = Visibility.Collapsed;
            }
            else
            {
                MinAreaTextBox.Visibility = Visibility.Collapsed;
                MaxAreaTextBox.Visibility = Visibility.Collapsed;
                MinRoomsTextBox.Visibility = Visibility.Collapsed;
                MaxRoomsTextBox.Visibility = Visibility.Collapsed;
                MinFloorTextBox.Visibility = Visibility.Collapsed;
                MaxFloorTextBox.Visibility = Visibility.Collapsed;
            }

        }
    }
}
