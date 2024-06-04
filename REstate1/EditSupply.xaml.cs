using REstate1.Data.Entities;
using System.Linq;
using System.Windows;

namespace REstate1
{
    /// <summary>
    /// Логика взаимодействия для EditSupply.xaml
    /// </summary>
    public partial class EditSupply : Window
    {
        public EditSupply()
        {
            InitializeComponent();
            LoadClients();
            LoadAgents();
            LoadEstates();
        }

        private Supply _supply; 

        public EditSupply(Supply supply) // для передачи supply
        {
            InitializeComponent();
            _supply = supply;

            LoadClients();
            LoadAgents();
            LoadEstates();
            LoadSupplyDetails(); 
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


        private void LoadEstates()
        {
            using (var context = new RealEstateContext())
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
        }

        private void LoadSupplyDetails()
        {
            ClientComboBox.SelectedValue = _supply.ClientId;
            AgentComboBox.SelectedValue = _supply.AgentId;
            RealEstateComboBox.SelectedValue = _supply.RealEstateId;
            PriceTextBox.Text = _supply.Price.ToString();
        }

        private void SaveSupply(object sender, RoutedEventArgs e)
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
                var supply = context.Supply.Find(_supply.Id);
                if (supply != null)
                {
                    supply.ClientId = selectedClient.Id;
                    supply.AgentId = selectedAgent.Id;
                    supply.RealEstateId = selectedEstateId.Value;
                    supply.Price = long.Parse(PriceTextBox.Text);

                    context.SaveChanges();
                }
            }

            DialogResult = true;
            Close();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
