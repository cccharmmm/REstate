using System.Linq;
using System.Windows;
using System.Data.Entity;
using REstate1.Data.Entities;
using System.Globalization;
using System.Windows.Controls;
using static System.Windows.Forms.AxHost;
using System.Runtime.Remoting.Contexts;

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
            LoadClients();
            LoadTypes();
            Loaded += LoadDemands;
            LoadAgents();
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
                DemandsListBox.ItemsSource = context.Demands
                    .Include(d => d.Agent)
                    .Include(d => d.Client)
                    .Include(d => d.TypeRealEstate)
                    .ToList();
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

        private void LoadTypes()
        {
            using (var context = new RealEstateContext())
            {
                var types = context.TypeRealEstate.ToList();
                TypeComboBox.ItemsSource = types;
            }
        }

        private void CreateDemand(object sender, RoutedEventArgs e)
        {
            Demand demand = null;
            var selectedClient = ClientComboBox.SelectedItem as Client;
            var selectedAgent = AgentComboBox.SelectedItem as Agent;
            var selectedType = TypeComboBox.SelectedItem as TypeRealEstate;
            if (selectedClient == null || selectedAgent == null || selectedType == null)
            {
                MessageBox.Show("Выберите все поля");
                return;
            }

            using (var context = new RealEstateContext())
            {
                if (context.Agent.Any(agent => agent.Id == selectedAgent.Id))
                {
                    var demandd = new Demand
                    {
                        ClientId = selectedClient.Id,
                        AgentId = selectedAgent.Id,
                        Id_type = selectedType.Id_type,
                        Address_City = CityTextBox.Text,
                        Address_Street = StreetTextBox.Text,
                        Address_House = HouseTextBox.Text,
                        Address_Number = NumberTextBox.Text,
                        MinPrice = int.Parse(MinPriceTextBox.Text),
                        MaxPrice = int.Parse(MaxPriceTextBox.Text)
                    };
                    context.Demands.Add(demandd);
                    context.SaveChanges();
                }
                if (selectedType.Name == "Дом")
                {
                    var house = new HouseDemand
                    {
                        Id = demand.Id,
                        MinArea = float.Parse(MinAreaTextBox.Text),
                        MaxArea = float.Parse(MaxAreaTextBox.Text),
                        MinRooms = int.Parse(MinRoomsTextBox.Text),
                        MaxRooms = int.Parse(MaxRoomsTextBox.Text),
                        MinFloors = int.Parse(MinFloorTextBox.Text),
                        MaxFloors = int.Parse(MaxFloorTextBox.Text)
                    };
                    context.Houses.Add(house);
                }
                else if (selectedType.Name == "Квартира")
                {
                    var apartment = new ApartmentDemand
                    {
                        Id = demand.Id,
                        MinArea = float.Parse(MinAreaTextBox.Text),
                        MaxArea = float.Parse(MaxAreaTextBox.Text),
                        MinRooms = int.Parse(MinRoomsTextBox.Text),
                        MaxRooms = int.Parse(MaxRoomsTextBox.Text),
                        MinFloor = int.Parse(MinFloorTextBox.Text),
                        MaxFloor = int.Parse(MaxFloorTextBox.Text)
                    };
                    context.ApartmentDemands.Add(apartment);
                }
                else if (selectedType.Name == "Земля")
                {
                    var land = new LandDemand
                    {
                        Id = demand.Id,
                        MinArea = float.Parse(MinAreaTextBox.Text),
                        MaxArea = int.Parse(MaxAreaTextBox.Text),
                    };
                    context.LandDemands.Add(land);
                }
                context.SaveChanges();
            }
        }
        private void TypeComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var selectedType = (TypeComboBox.SelectedItem as TypeRealEstate)?.Name;

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

        private void EditDemand(object sender, RoutedEventArgs e)
        {
            Demand selectedDemand = DemandsListBox.SelectedItem as Demand;
            if (selectedDemand != null)
            {
                EditDemand editDemandWindow = new EditDemand(selectedDemand);
                editDemandWindow.ShowDialog();
                LoadDemands(null, null);
            }
            else
            {
                MessageBox.Show("Выберите запись для редактирования.");
            }
        }


        private void DeleteDemand(object sender, RoutedEventArgs e)
        {
            var selectedDemand = DemandsListBox.SelectedItem as Demand;

            if (selectedDemand == null)
            {
                MessageBox.Show("Выберите запись для удаления.");
                return;
            }

            using (var context = new RealEstateContext())
            {
                var existingDemand = context.Demands.Find(selectedDemand.Id); // Проверяем существование объекта в контексте
                if (existingDemand != null)
                {
                    bool isLinkedToDeal = IsDemandLinkedToDeal(selectedDemand);

                    if (!isLinkedToDeal)
                    {
                        context.Demands.Remove(existingDemand); // Удаляем объект из контекста
                        context.SaveChanges();
                        LoadDemands(null, null);
                    }
                    else
                    {
                        MessageBox.Show("Невозможно удалить запись потребности, так как она связана с сделкой.");
                    }
                }
                else
                {
                    MessageBox.Show("Запись не найдена в базе данных.");
                }
            }
        }


        private bool IsDemandLinkedToDeal(Demand demand)
        {
            // Проверяем, есть ли связанные записи в сделках
            using (var context = new RealEstateContext())
            {
                var linkedDeal = context.Deals.FirstOrDefault(d => d.Demand_Id == demand.Id);
                return linkedDeal != null;
            }
        }
    }

    public class RequiredFieldValidationRule : ValidationRule
        {
            public override ValidationResult Validate(object value, CultureInfo cultureInfo)
            {
                if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                {
                    return new ValidationResult(false, "Поле обязательно для заполнения.");
                }
                return ValidationResult.ValidResult;
            }
    }

        public class PositiveIntegerValidationRule : ValidationRule
        {
            public override ValidationResult Validate(object value, CultureInfo cultureInfo)
            {
                int intValue;

                if (int.TryParse(value.ToString(), out intValue))
                {
                    if (intValue >= 0)
                    {
                        return ValidationResult.ValidResult;
                    }
                    else
                    {
                        return new ValidationResult(false, "Значение должно быть положительным числом.");
                    }
                }
                else
                {
                    return new ValidationResult(false, "Введите целое положительное число.");
                }
            }
        }
}

