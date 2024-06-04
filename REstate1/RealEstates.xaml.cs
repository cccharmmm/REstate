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
            }

            UpdateRealEstatesList();
        }




    }
}
