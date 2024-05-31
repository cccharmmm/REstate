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

namespace REstate1
{
    /// <summary>
    /// Логика взаимодействия для RealEstates.xaml
    /// </summary>
    public partial class RealEstates : Window
    {
        public RealEstates()
        {
            InitializeComponent();
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

       
        private void Supplies_Click(object sender, RoutedEventArgs e)
        {
            Supplies supplies = new Supplies();
            this.Close();
            supplies.Show();
        }

        private void Demands_Click(object sender, RoutedEventArgs e)
        {
            Demands demands = new Demands();
            this.Close();
            demands.Show();
        }

        private void Deals_Click(object sender, RoutedEventArgs e)
        {
            Deals deals = new Deals();
            this.Close();
            deals.Show();
        }
        private void TypeComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var selectedType = (TypeComboBox.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content.ToString();

            if (selectedType == "Квартира" || selectedType == "Дом")
            {
                AreaTextBox.Visibility = Visibility.Visible;
                FloorTextBox.Visibility = Visibility.Visible;
                RoomsTextBox.Visibility = Visibility.Visible;
            }
            else if (selectedType == "Земля")
            {
                AreaTextBox.Visibility = Visibility.Visible;
                FloorTextBox.Visibility = Visibility.Collapsed;
                RoomsTextBox.Visibility = Visibility.Collapsed;
            }
            else
            {
                AreaTextBox.Visibility = Visibility.Collapsed;
                FloorTextBox.Visibility = Visibility.Collapsed;
                RoomsTextBox.Visibility = Visibility.Collapsed;
            }

        }
        private void FilterComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var selectedType = (FilterComboBox.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content.ToString();

            if (selectedType == "адресу")
            {
              
                AddressComboBox.Visibility = Visibility.Visible;
                Type2ComboBox.Visibility = Visibility.Collapsed;
                ApplyButton.Visibility = Visibility.Visible;
                Label1.Visibility = Visibility.Collapsed;
               
            }
            else if (selectedType == "типу")
            {
              
                AddressComboBox.Visibility = Visibility.Collapsed;
                Type2ComboBox.Visibility = Visibility.Visible;
                ApplyButton.Visibility = Visibility.Visible;
                Label1.Visibility = Visibility.Collapsed;
            }
            else
            {
               
                AddressComboBox.Visibility = Visibility.Collapsed;
                Type2ComboBox.Visibility = Visibility.Collapsed;
                ApplyButton.Visibility = Visibility.Collapsed;
            }

        }
    }
}
