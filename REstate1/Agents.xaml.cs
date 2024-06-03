using REstate1.Data.Entities;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace REstate1
{
    public partial class Agents : Window
    {
        public Agents()
        {
            InitializeComponent();
            Loaded += Agent_Loaded;
        }

        private void Clients_Click(object sender, RoutedEventArgs e)
        {
            Clients clients = new Clients();
            this.Close();
            clients.Show();
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

        private void UpdateAgentList()
        {
            using (var context = new RealEstateContext())
            {
                AgentListBox.ItemsSource = context.Agent.ToList();
            }
        }

        private void Agent_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateAgentList();
        }

        private void CreateAgent_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new RealEstateContext())
            {
                var agent = new Agent
                {
                    LastName = SurnameTextBox.Text,
                    FirstName = NameTextBox.Text,
                    MiddleName = PatronymicTextBox.Text,
                    DealShare = DealShareTextBox.Text
                };

                context.Agent.Add(agent);
                context.SaveChanges();
                MessageBox.Show("Агент успешно создан!");
                UpdateAgentList();
            }
        }

        private void DeleteAgent_Click(object sender, RoutedEventArgs e)
        {
            var selectedAgent = AgentListBox.SelectedItem as Agent;
            if (selectedAgent != null)
            {
                using (var context = new RealEstateContext())
                {
                    context.Agent.Attach(selectedAgent);
                    context.Agent.Remove(selectedAgent);
                    context.SaveChanges();
                }

                MessageBox.Show("Агент успешно удален.");
                UpdateAgentList();
            }
            else
            {
                MessageBox.Show("Выберите агента для удаления.");
            }
        }

        private void UpdateAgent_Click(object sender, RoutedEventArgs e)
        {
            var selectedAgent = AgentListBox.SelectedItem as Agent;
            if (selectedAgent != null)
            {
                EditAgents editAgent = new EditAgents(selectedAgent);
                editAgent.ShowDialog();
                UpdateAgentList();
            }
            else
            {
                MessageBox.Show("Выберите агента для редактирования.");
            }
        }
    }
}
