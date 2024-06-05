using Microsoft.Extensions.Logging;
using REstate1.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for Supplies.xaml
    /// </summary>
    public partial class Supplies : Window
    {
        public Supplies()
        {
            InitializeComponent();
            Loaded += LoadSupplies;
            
        }

        private void LoadSupplies(object sender, RoutedEventArgs e)
        {
            using (var context = new RealEstateContext())
            {
                // Загрузка предложений и связанных с ними данных
                var supplies = context.Supply
                    .Include("Client")
                    .Include("Agent")
                    .ToList();

                SuppliesListBox.ItemsSource = supplies;

                // Загрузка клиентов, агентов и недвижимости
                LoadClients(context);
                LoadAgents(context);
                LoadEstates(context);
            }
        }

        private void LoadClients(RealEstateContext context)
        {
            var clients = context.Client.ToList();
            ClientComboBox.ItemsSource = clients;
        }

        private void LoadAgents(RealEstateContext context)
        {
            var agents = context.Agent.ToList();
            AgentComboBox.ItemsSource = agents;
        }

        private void LoadEstates(RealEstateContext context)
        {
            var estates = context.RealEstate
                .Select(r => new
                {
                    r.Id,
                    Address = r.Address_City + ", " + r.Address_Street + " " + r.Address_House
                })
                .ToList();

            RealEstateComboBox.ItemsSource = estates;
            RealEstateComboBox.DisplayMemberPath = "Address";
            RealEstateComboBox.SelectedValuePath = "Id";
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

            // Проверка на корректность введенных данных в текстовых полях
            if (!ValidatePrice(PriceTextBox.Text))
            {
                MessageBox.Show("Введите корректную цену");
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

            // Проверка, участвует ли выбранное предложение в какой-либо сделке
            bool isSupplyInDeal = IsSupplyInDeal(selectedSup);
            if (isSupplyInDeal)
            {
                MessageBox.Show("Невозможно удалить предложение, которое участвует в сделке.");
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

        private bool IsSupplyInDeal(Supply supply)
        {
            using (var context = new RealEstateContext())
            {
                return context.Deals.Any(deal => deal.Supply_Id == supply.Id);
            }
        }

        private void EditSupply(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedSupply = SuppliesListBox.SelectedItem as Supply;
                if (selectedSupply != null)
                {
                    EditSupply editSupplyWindow = new EditSupply(selectedSupply);
                    editSupplyWindow.ShowDialog();
                    LoadSupplies(null, null);
                }
                else
                {
                    MessageBox.Show("Выберите предложение для редактирования.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message);
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

        private void PriceTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        private void ClearForm()
        {
            ClientComboBox.SelectedItem = null;
            AgentComboBox.SelectedItem = null;
            RealEstateComboBox.SelectedItem = null;
            PriceTextBox.Clear();
        }

        private bool ValidatePrice(string price)
        {
            if (string.IsNullOrWhiteSpace(price))
                return false;

            return long.TryParse(price, out _);
        }

        private void Deals_Click(object sender, RoutedEventArgs e)
        {
            Deals deals = new Deals();
            this.Close();
            deals.Show();
        }
    }
}
