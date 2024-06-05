using System;
using System.Linq;
using System.Windows;
using System.Data.Entity;
using REstate1.Data.Entities;
using System.Globalization;
using System.Windows.Controls;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace REstate1
{
    public partial class EditDemand : Window
    {
        private Demand demand;
        private int agentId;
        private int clientId;
        private int typeRealEstateId;

        public Demand Demand
        {
            get { return demand; }
            set { demand = value; }
        }

        public EditDemand(Demand demand, int agentId, int clientId, int typeRealEstateId)
        {
            InitializeComponent();
            this.demand = demand;
            this.agentId = agentId;
            this.clientId = clientId;
            this.typeRealEstateId = typeRealEstateId;
            LoadData();
        }

        private void LoadData()
        {
            LoadClients();
            LoadAgents();
            LoadTypes();

            using (var context = new RealEstateContext())
            {
                var agent = context.Agent.FirstOrDefault(a => a.Id == agentId);
                var client = context.Client.FirstOrDefault(c => c.Id == clientId);
                var typeRealEstate = context.TypeRealEstate.FirstOrDefault(t => t.Id_type == typeRealEstateId);

                if (agent != null)
                    AgentComboBox.SelectedValue = agent.Id;
                if (client != null)
                    ClientComboBox.SelectedValue = client.Id;
                if (typeRealEstate != null)
                    TypeComboBox.SelectedValue = typeRealEstate.Id_type;

                if (demand != null)
                {
                    CityTextBox.Text = demand.Address_City;
                    StreetTextBox.Text = demand.Address_Street;
                    HouseTextBox.Text = demand.Address_House;
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var selectedAgentId = (int)AgentComboBox.SelectedValue;
            var selectedClientId = (int)ClientComboBox.SelectedValue;
            var selectedTypeId = (int)TypeComboBox.SelectedValue;

            if (selectedAgentId == 0 || selectedClientId == 0 || selectedTypeId == 0)
            {
                MessageBox.Show("Пожалуйста, выберите риэлтора, агента и тип недвижимости.");
                return;
            }

            // Проверка на корректность введенных данных в текстовых полях
            string city = CityTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(city))
            {
                if (!Regex.IsMatch(city, @"^[А-Я][а-я]{3,}$"))
                {
                    MessageBox.Show("Введите корректный город");
                    return;
                }
                for (int i = 0; i < city.Length - 1; i++)
                {
                    if (char.ToUpper(city[i]) == char.ToUpper(city[i + 1]))
                    {
                        MessageBox.Show("Город не может содержать повторяющиеся символы");
                        return;
                    }
                }
            }

            string street = StreetTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(street))
            {
                if (!Regex.IsMatch(street, @"^[А-Я][а-я]{3,}$"))
                {
                    MessageBox.Show("Введите корректное название улицы");
                    return;
                }
                for (int i = 0; i < street.Length - 1; i++)
                {
                    if (char.ToUpper(street[i]) == char.ToUpper(street[i + 1]))
                    {
                        MessageBox.Show("Улица не может содержать повторяющиеся символы");
                        return;
                    }
                }
            }

            string house = HouseTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(house))
            {
                if (!Regex.IsMatch(house, @"^\d+$"))
                {
                    MessageBox.Show("Введите корректный номер дома (только цифры)");
                    return;
                }
            }

            try
            {
                using (var context = new RealEstateContext())
                {
                    if (demand != null)
                    {
                        demand.ClientId = selectedClientId;
                        demand.AgentId = selectedAgentId;
                        demand.Id_type = selectedTypeId;
                        demand.Address_City = city;
                        demand.Address_Street = street;
                        demand.Address_House = house;
                        context.Entry(demand).State = EntityState.Modified;
                    }
                    else
                    {
                        demand = new Demand
                        {
                            ClientId = selectedClientId,
                            AgentId = selectedAgentId,
                            Id_type = selectedTypeId,
                            Address_City = city,
                            Address_Street = street,
                            Address_House = house
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
                MessageBox.Show("Произошла ошибка: " + ex.Message);
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
                ClientComboBox.DisplayMemberPath = "LastName";
                ClientComboBox.SelectedValuePath = "Id";
            }
        }

        private void LoadAgents()
        {
            using (var context = new RealEstateContext())
            {
                var agents = context.Agent.ToList();
                AgentComboBox.ItemsSource = agents;
                AgentComboBox.DisplayMemberPath = "LastName";
                AgentComboBox.SelectedValuePath = "Id";
            }
        }

        private void LoadTypes()
        {
            using (var context = new RealEstateContext())
            {
                var types = context.TypeRealEstate.ToList();
                TypeComboBox.ItemsSource = types;
                TypeComboBox.DisplayMemberPath = "Name";
                TypeComboBox.SelectedValuePath = "Id_type";
            }
        }
    }
}
