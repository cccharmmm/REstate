using System;
using System.Linq;
using System.Windows;
using System.Data.Entity;
using REstate1.Data.Entities;
using System.Globalization;
using System.Windows.Controls;

namespace REstate1
{
    public partial class EditDemand : Window
    {
        private Demand demand;

        public EditDemand(Demand demand)
        {
            InitializeComponent();
            this.demand = demand;
            InitializeFields(); 
            LoadClients();
            LoadAgents();
            LoadTypes();
        }

        private void InitializeFields()
        {
            ClientComboBox.SelectedItem = demand.Client;
            AgentComboBox.SelectedItem = demand.Agent;
            TypeComboBox.SelectedItem = demand.TypeRealEstate;
            CityTextBox.Text = demand.Address_City;
            StreetTextBox.Text = demand.Address_Street;
            HouseTextBox.Text = demand.Address_House;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var selectedClient = ClientComboBox.SelectedItem as Client;
            var selectedAgent = AgentComboBox.SelectedItem as Agent;
            var selectedType = TypeComboBox.SelectedItem as TypeRealEstate;

            if (selectedClient == null || selectedAgent == null || selectedType == null ||
                string.IsNullOrEmpty(CityTextBox.Text) || string.IsNullOrEmpty(StreetTextBox.Text) ||
                string.IsNullOrEmpty(HouseTextBox.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            try
            {
                using (var context = new RealEstateContext())
                {
                    if (demand != null)
                    {
                        // Обновляем существующую запись
                        demand.ClientId = selectedClient.Id;
                        demand.AgentId = selectedAgent.Id;
                        demand.Id_type = selectedType.Id_type;
                        demand.Address_City = CityTextBox.Text;
                        demand.Address_Street = StreetTextBox.Text;
                        demand.Address_House = HouseTextBox.Text;

                        context.Entry(demand).State = EntityState.Modified;
                    }
                    else
                    {
                        // Создаем новую запись
                        demand = new Demand
                        {
                            ClientId = selectedClient.Id,
                            AgentId = selectedAgent.Id,
                            Id_type = selectedType.Id_type,
                            Address_City = CityTextBox.Text,
                            Address_Street = StreetTextBox.Text,
                            Address_House = HouseTextBox.Text
                        };

                        context.Demands.Add(demand);
                    }

                    context.SaveChanges();
                }

                MessageBox.Show("Данные успешно сохранены.");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("О как!");
                this.Close();

                Console.WriteLine(ex.Message);
            }
        }



        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LoadClients()
        {
            using (var context = new RealEstateContext())
            {
                var clients = context.Client.ToList();
                ClientComboBox.ItemsSource = clients;
            }
        }

        private void LoadAgents()
        {
            using (var context = new RealEstateContext())
            {
                var agents = context.Agent.ToList();
                AgentComboBox.ItemsSource = agents;
            }
        }

        private void LoadTypes()
        {
            using (var context = new RealEstateContext())
            {
                var types = context.TypeRealEstate.ToList();
                TypeComboBox.ItemsSource = types;
            }
        }
    }
}
