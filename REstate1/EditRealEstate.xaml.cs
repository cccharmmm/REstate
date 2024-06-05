using REstate1.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
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
    public partial class EditRealEstate : Window
    {
        private RealEstate _realEstate;
        private List<District> _districts;
        private List<TypeRealEstate> _types;

        public EditRealEstate(RealEstate realEstate)
        {
            InitializeComponent();
            _realEstate = realEstate;
            LoadData();
            LoadRealEstateDetails();
        }

        private void LoadData()
        {
            using (var context = new RealEstateContext())
            {
                _districts = context.District.ToList();
                _types = context.TypeRealEstate.ToList();
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

        private void LoadRealEstateDetails()
        {
            CityTextBox.Text = _realEstate.Address_City;
            StreetTextBox.Text = _realEstate.Address_Street;
            HouseTextBox.Text = _realEstate.Address_House;
            NumberTextBox.Text = _realEstate.Address_Number;

            using (var context = new RealEstateContext())
            {
                var realEstateWithIncludes = context.RealEstate
                    .Include(r => r.Apartment)
                    .Include(r => r.House)
                    .Include(r => r.Land)
                    .FirstOrDefault(r => r.Id == _realEstate.Id);

                if (realEstateWithIncludes != null)
                {
                    DistrictComboBox.ItemsSource = _districts;
                    DistrictComboBox.SelectedValue = realEstateWithIncludes.District_Id;

                    TypeComboBox.ItemsSource = _types.Select(t => t.Name);
                    TypeComboBox.SelectedValue = _types.FirstOrDefault(t => t.Id_type == realEstateWithIncludes.Id_type)?.Name;

                    if (realEstateWithIncludes.Id_type == 1) // Если тип - квартира
                    {
                        AreaTextBox.Visibility = Visibility.Visible;
                        FloorTextBox.Visibility = Visibility.Visible;
                        RoomsTextBox.Visibility = Visibility.Visible;

                        AreaTextBox.Text = realEstateWithIncludes.Apartment?.TotalArea.ToString() ?? "";
                        FloorTextBox.Text = realEstateWithIncludes.Apartment?.Floor.ToString() ?? "";
                        RoomsTextBox.Text = realEstateWithIncludes.Apartment?.Rooms.ToString() ?? "";
                    }
                    else if (realEstateWithIncludes.Id_type == 2) // Если тип - дом
                    {
                        AreaTextBox.Visibility = Visibility.Visible;
                        FloorTextBox.Visibility = Visibility.Collapsed; // Не используется для домов
                        RoomsTextBox.Visibility = Visibility.Visible;

                        AreaTextBox.Text = realEstateWithIncludes.House?.TotalArea.ToString() ?? "";
                        RoomsTextBox.Text = realEstateWithIncludes.House?.Rooms.ToString() ?? "";
                    }
                    else if (realEstateWithIncludes.Id_type == 3) // Если тип - земля
                    {
                        AreaTextBox.Visibility = Visibility.Visible;
                        FloorTextBox.Visibility = Visibility.Collapsed; // Не используется для земли
                        RoomsTextBox.Visibility = Visibility.Collapsed; // Не используется для земли

                        AreaTextBox.Text = realEstateWithIncludes.Land?.TotalArea.ToString() ?? "";
                    }
                }
            }
        }

        private void SaveRealEstate(object sender, RoutedEventArgs e)
        {
            using (var context = new RealEstateContext())
            {
                var selectedDistrictName = DistrictComboBox.SelectedItem as string;
                var selectedType = _types.FirstOrDefault(t => t.Name == TypeComboBox.SelectedValue.ToString());
                var selectedDistrict = _districts.FirstOrDefault(d => d.Name == selectedDistrictName);

                // Обновляем информацию о недвижимости на основе данных из полей ввода
                _realEstate.Address_City = CityTextBox.Text;
                _realEstate.Address_Street = StreetTextBox.Text;
                _realEstate.Address_House = HouseTextBox.Text;
                _realEstate.Address_Number = NumberTextBox.Text;
                _realEstate.District_Id = selectedDistrict?.ID ?? 0; // Заменяем на ID
                _realEstate.Id_type = selectedType?.Id_type ?? 0; // Заменяем на Id_type

                // Обновляем данные в зависимости от типа недвижимости
                if (_realEstate.Id_type == 1) // Если тип - квартира
                {
                    _realEstate.Apartment.TotalArea = float.Parse(AreaTextBox.Text);
                    _realEstate.Apartment.Floor = int.Parse(FloorTextBox.Text);
                    _realEstate.Apartment.Rooms = int.Parse(RoomsTextBox.Text);
                }
                else if (_realEstate.Id_type == 2) // Если тип - дом
                {
                    _realEstate.House.TotalArea = float.Parse(AreaTextBox.Text);
                    _realEstate.House.Rooms = int.Parse(RoomsTextBox.Text);
                }
                else if (_realEstate.Id_type == 3) // Если тип - земля
                {
                    _realEstate.Land.TotalArea = float.Parse(AreaTextBox.Text);
                }

                context.SaveChanges();
            }

            Close();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}