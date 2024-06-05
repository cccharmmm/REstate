using System;
using System.Linq;
using System.Windows;
using REstate1.Data.Entities;

namespace REstate1
{
    public partial class EditSupply : Window
    {
        private Supply _supply;

        public EditSupply(Supply supply = null)
        {
            InitializeComponent();
            _supply = supply;
            LoadData();
        }

        private void LoadData()
        {
            using (var context = new RealEstateContext())
            {
                // Загрузка клиентов, агентов и типов недвижимости
                LoadClients(context);
                LoadAgents(context);
                LoadRealEstates(context);

                // Заполнение полей, если есть предложение
                if (_supply != null)
                {
                    ClientComboBox.SelectedValue = _supply.ClientId;
                    AgentComboBox.SelectedValue = _supply.AgentId;
                    RealEstateComboBox.SelectedValue = _supply.RealEstateId;
                    PriceTextBox.Text = _supply.Price.ToString();
                }
            }
        }

        private void LoadClients(RealEstateContext context)
        {
            var clients = context.Client.ToList();
            ClientComboBox.ItemsSource = clients;
            ClientComboBox.DisplayMemberPath = "LastName";
            ClientComboBox.SelectedValuePath = "Id";
        }

        private void LoadAgents(RealEstateContext context)
        {
            var agents = context.Agent.ToList();
            AgentComboBox.ItemsSource = agents;
            AgentComboBox.DisplayMemberPath = "LastName";
            AgentComboBox.SelectedValuePath = "Id";
        }

        private void LoadRealEstates(RealEstateContext context)
        {
            var realEstates = context.RealEstate
                .Select(r => new
                {
                    r.Id,
                    Address = r.Address_City + ", " + r.Address_Street + " " + r.Address_House
                })
                .ToList();

            RealEstateComboBox.ItemsSource = realEstates;
            RealEstateComboBox.DisplayMemberPath = "Address";
            RealEstateComboBox.SelectedValuePath = "Id";
        }

        private void SaveSupply(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedClientId = (int)ClientComboBox.SelectedValue;
                var selectedAgentId = (int)AgentComboBox.SelectedValue;
                var selectedRealEstateId = (int)RealEstateComboBox.SelectedValue;

                if (selectedClientId == 0 || selectedAgentId == 0 || selectedRealEstateId == 0)
                {
                    MessageBox.Show("Выберите все поля");
                    return;
                }

                if (!ValidatePrice(PriceTextBox.Text))
                {
                    MessageBox.Show("Введите корректную цену");
                    return;
                }

                using (var context = new RealEstateContext())
                {
                    if (_supply != null)
                    {
                        var supply = context.Supply.Find(_supply.Id);
                        if (supply != null)
                        {
                            supply.ClientId = selectedClientId;
                            supply.AgentId = selectedAgentId;
                            supply.RealEstateId = selectedRealEstateId;
                            supply.Price = long.Parse(PriceTextBox.Text);
                        }
                    }
                    else
                    {
                        var newSupply = new Supply
                        {
                            ClientId = selectedClientId,
                            AgentId = selectedAgentId,
                            RealEstateId = selectedRealEstateId,
                            Price = long.Parse(PriceTextBox.Text)
                        };
                        context.Supply.Add(newSupply);
                    }

                    context.SaveChanges();
                }

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private bool ValidatePrice(string price)
        {
            if (string.IsNullOrWhiteSpace(price))
                return false;

            return long.TryParse(price, out _);
        }

        private void PriceTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }
    }
}
