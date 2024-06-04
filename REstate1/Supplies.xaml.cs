using Microsoft.Extensions.Logging;
using REstate1.Data.Entities;
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
    /// Логика взаимодействия для Supplies.xaml
    /// </summary>
    public partial class Supplies : Window
    {
        public Supplies()
        {
            InitializeComponent();
            Loaded += LoadSupplies;
            LoadClients();
            LoadAgents();
            LoadEstates();
        }

        private void LoadSupplies(object sender, RoutedEventArgs e)
        {
            using (var context = new RealEstateContext())
            {
                var supplies = context.Supply
                    .Include("Client")
                    .Include("Agent")
                    .ToList();

                SuppliesListBox.ItemsSource = supplies;
            }
        }


        private void LoadClients()
        {
            using (var context = new RealEstateContext())
            {
                var types = context.Client.ToList();
                ClientComboBox.ItemsSource = types;
            }
        }

        private void LoadAgents()
        {
            using (var context = new RealEstateContext())
            {
                var types = context.Agent.ToList();
                AgentComboBox.ItemsSource = types;
            }
        }

        private void LoadEstates()
        {
            using (var context = new RealEstateContext())
            {
                var types = context.RealEstate
                                   .Select(r => new
                                   {
                                       r.Id,
                                       Address = r.Address_City + ", " + r.Address_Street + " " + r.Address_House
                                   })
                                   .ToList();

                RealEstateComboBox.ItemsSource = types;
                RealEstateComboBox.DisplayMemberPath = "Address";
                RealEstateComboBox.SelectedValuePath = "Id";
            }
        }


        private void CreateSupply(object sender, RoutedEventArgs e)
        {
            var selectedClient = ClientComboBox.SelectedItem as Client;
            var selectedAgent = AgentComboBox.SelectedItem as Agent;
            var selectedEstateId = (int?)RealEstateComboBox.SelectedValue;

            if (selectedClient == null || selectedAgent == null || selectedEstateId == null)
            {
                MessageBox.Show("Выберите все поля");
                return;
            }

            using (var context = new RealEstateContext())
            {
                var supply = new Supply
                {
                    ClientId = selectedClient.Id,
                    AgentId = selectedAgent.Id,
                    RealEstateId = selectedEstateId.Value,
                    Price = long.Parse(PriceTextBox.Text)
                };

                context.Supply.Add(supply);
                context.SaveChanges();
            }

            ClearForm();
            LoadSupplies(null, null);
        }

        private void DeleteSupply(object sender, RoutedEventArgs e)
        {

            var selectedSup = SuppliesListBox.SelectedItem as Supply;
            if (selectedSup == null)
            {
                MessageBox.Show("Выберите запись для удаления");
                return;
            }
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить данное предложение?", "Подтверждение удаления", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                using (var context = new RealEstateContext())
                {
                    var supply = context.Supply.Find(selectedSup.Id);
                    if (supply != null)
                    {
                        context.Supply.Remove(supply);
                        context.SaveChanges();
                    }
                }
            }
            LoadSupplies(null, null);
        }
        private void EditSupply(object sender, RoutedEventArgs e)
        {
            var selectedSupply = SuppliesListBox.SelectedItem as Supply;

            if (selectedSupply == null)
            {
                MessageBox.Show("Выберите запись для редактирования");
                return;
            }

            var editWindow = new EditSupply(selectedSupply);
            if (editWindow.ShowDialog() == true)
            {
                LoadSupplies(null, null);
            }
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

        private void Demands_Click(object sender, RoutedEventArgs e)
        {
            Demands demands = new Demands();
            this.Close();
            demands.Show();
        }
        private void RefreshSupplies_Click(object sender, RoutedEventArgs e)
        {
            LoadSupplies(sender, e);
        }

        private void ClearForm()
        {
            ClientComboBox.SelectedItem = null;
            AgentComboBox.SelectedItem = null;
            RealEstateComboBox.SelectedItem = null;
            PriceTextBox.Clear();
        }


        private void Deals_Click(object sender, RoutedEventArgs e)
        {
            Deals deals = new Deals();
            this.Close();
            deals.Show();
        }
    }
}
