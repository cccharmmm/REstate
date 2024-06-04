using System.Linq;
using System.Windows;
using System.Windows.Controls;
using REstate1.Data;
using REstate1.Data.Entities;

namespace REstate1
{
    public partial class RealEstates : Window
    {
        public RealEstates()
        {
            InitializeComponent();
            Loaded += LoadEstates;
            LoadRealEstateTypes();
            LoadDistricts();
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

        private void LoadEstates(object sender, RoutedEventArgs e)
        {
            UpdateRealEstatesList();
        }

        private void UpdateRealEstatesList()
        {
            using (var context = new RealEstateContext())
            {
                EstatesListBox.ItemsSource = context.RealEstate.ToList();
            }
        }

        private void TypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedType = TypeComboBox.SelectedItem as TypeRealEstate;

            if (selectedType?.Name == "Квартира" || selectedType?.Name == "Дом")
            {
                AreaTextBox.Visibility = Visibility.Visible;
                FloorTextBox.Visibility = Visibility.Visible;
                RoomsTextBox.Visibility = Visibility.Visible;
            }
            else if (selectedType?.Name == "Земля")
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

        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FilterComboBox.SelectedItem != null)
            {
                var selectedFilter = (ComboBoxItem)FilterComboBox.SelectedItem;

                if (selectedFilter.Content.ToString() == "по адресу")
                {
                    Label1.Visibility= Visibility.Collapsed;
                    AddressComboBox.Visibility = Visibility.Visible;
                    Type2ComboBox.Visibility = Visibility.Collapsed;
                    ApplyButton.Visibility = Visibility.Visible;
                }
                else if (selectedFilter.Content.ToString() == "по типу")
                {
                    Label1.Visibility = Visibility.Collapsed;
                    AddressComboBox.Visibility = Visibility.Collapsed;
                    Type2ComboBox.Visibility = Visibility.Visible;
                    ApplyButton.Visibility = Visibility.Visible;
                }
                else
                {
                    AddressComboBox.Visibility = Visibility.Collapsed;
                    Type2ComboBox.Visibility = Visibility.Collapsed;
                    ApplyButton.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void LoadRealEstateTypes()
        {
            using (var context = new RealEstateContext())
            {
                var types = context.TypeRealEstate.ToList();
                TypeComboBox.ItemsSource = types;
            }
        }

        private void LoadDistricts()
        {
            using (var context = new RealEstateContext())
            {
                var types = context.District.ToList();
                DistrictComboBox.ItemsSource = types;
            }
        }

        private void CreateEstate(object sender, RoutedEventArgs e)
        {
            var selectedDistrict = DistrictComboBox.SelectedItem as District;
            var selectedType = TypeComboBox.SelectedItem as TypeRealEstate;

            if (selectedDistrict == null || selectedType == null)
            {
                MessageBox.Show("Выберите район и тип объекта недвижимости.");
                return;
            }

            using (var context = new RealEstateContext())
            {
                var estate = new RealEstate
                {
                    Address_City = CityTextBox.Text,
                    Address_Street = StreetTextBox.Text,
                    Address_House = HouseTextBox.Text,
                    Address_Number = NumberTextBox.Text,
                    District_Id = selectedDistrict.ID,
                    Id_type = selectedType.Id_type
                };

                context.RealEstate.Add(estate);
                context.SaveChanges();

                if (selectedType.Name == "Дом")
                {
                    var house = new House
                    {
                        Id = estate.Id,
                        Rooms = int.Parse(RoomsTextBox.Text),
                        TotalFloors = int.Parse(FloorTextBox.Text),
                        TotalArea = float.Parse(AreaTextBox.Text)
                    };
                    context.House.Add(house);
                }
                else if (selectedType.Name == "Квартира")
                {
                    var apartment = new Apartment
                    {
                        Id = estate.Id,
                        TotalArea = float.Parse(AreaTextBox.Text),
                        Floor = int.Parse(FloorTextBox.Text),
                        Rooms = int.Parse(RoomsTextBox.Text)
                    };
                    context.Apartment.Add(apartment);
                }
                else if (selectedType.Name == "Земля")
                {
                    var land = new Land
                    {
                        Id = estate.Id,
                        TotalArea = float.Parse(AreaTextBox.Text)
                    };
                    context.Land.Add(land);
                }

                context.SaveChanges();
            }

            UpdateRealEstatesList();
        }
        private void UpdateEstateList()
        {
            using (var context = new RealEstateContext())
            {
                EstatesListBox.ItemsSource = context.RealEstate.ToList();
            }
        }

        public void Update_Click(object sender, RoutedEventArgs e)
        {
            UpdateEstateList();
        }
        private void DeleteEstate(object sender, RoutedEventArgs e)
        {
            var selectedEstate = EstatesListBox.SelectedItem as RealEstate;
            if (selectedEstate != null)
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить данную недвижимость?", "Подтверждение удаления", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    using (var context = new RealEstateContext())
                    {

                        var dealsToRemove = context.Deals.Where(d => d.Supply.RealEstate.Id == selectedEstate.Id).ToList();
                        context.Deals.RemoveRange(dealsToRemove);
                        // Находим и удаляем связанные записи из таблиц House, Apartment и Land
                        var houseToRemove = context.House.FirstOrDefault(h => h.Id == selectedEstate.Id);
                        if (houseToRemove != null)
                            context.House.Remove(houseToRemove);

                        var apartmentToRemove = context.Apartment.FirstOrDefault(a => a.Id == selectedEstate.Id);
                        if (apartmentToRemove != null)
                            context.Apartment.Remove(apartmentToRemove);

                        var landToRemove = context.Land.FirstOrDefault(l => l.Id == selectedEstate.Id);
                        if (landToRemove != null)
                            context.Land.Remove(landToRemove);


                        // Удаляем недвижимость
                        context.RealEstate.Attach(selectedEstate);
                        context.RealEstate.Remove(selectedEstate);
                        context.SaveChanges();
                    }
                    MessageBox.Show("Недвижимость успешно удалена.");
                    UpdateEstateList();
                }
            }
            else
            {
                MessageBox.Show("Выберите недвижимость для удаления.");
            }
        }


    }
}
