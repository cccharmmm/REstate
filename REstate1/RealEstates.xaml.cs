using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using REstate1.Data;
using REstate1.Data.Entities;
using System.Text.RegularExpressions;
using System.Data.Entity.Core;
using System;

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

            if (selectedType == null)
            {
                MessageBox.Show("Выберите тип объекта недвижимости.");
                return;
            }

            string city1 = CityTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(city1))
            {
                if (!Regex.IsMatch(city1, @"^[А-Я][а-я]{3,}$"))
                {
                    MessageBox.Show("Введите корректный город");
                    return;
                }
                for (int i = 0; i < city1.Length - 1; i++)
                {
                    if (char.ToUpper(city1[i]) == char.ToUpper(city1[i + 1]))
                    {
                        MessageBox.Show("Город не может содержать повторяющиеся символы");
                        return;
                    }
                }
            }

            string street1 = StreetTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(street1))
            {
                if (!Regex.IsMatch(street1, @"^[А-Я][а-я]{3,}$"))
                {
                    MessageBox.Show("Введите корректное название улицы");
                    return;
                }
                for (int i = 0; i < street1.Length - 1; i++)
                {
                    if (char.ToUpper(street1[i]) == char.ToUpper(street1[i + 1]))
                    {
                        MessageBox.Show("Улица не может содержать повторяющиеся символы");
                        return;
                    }
                }
            }

            string house1 = HouseTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(house1))
            {
                if (!Regex.IsMatch(house1, @"^\d+$"))
                {
                    MessageBox.Show("Введите корректный номер дома (только цифры)");
                    return;
                }
            }

            string apartment1 = NumberTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(apartment1))
            {
                if (!Regex.IsMatch(apartment1, @"^\d+$"))
                {
                    MessageBox.Show("Введите корректный номер квартиры (только цифры).");
                    return;
                }
            }

            using (var context = new RealEstateContext())
            {
                var estate = new RealEstate
                {
                    Address_City = !string.IsNullOrEmpty(city1) ? city1 : null,
                    Address_Street = street1,
                    Address_House = house1,
                    Address_Number = apartment1,
                    District_Id = selectedDistrict?.ID,
                    Id_type = selectedType.Id_type
                };

                context.RealEstate.Add(estate);
                context.SaveChanges();

                if (selectedType.Name == "Дом")
                {
                    int? rooms = null, totalFloors = null;
                    if (!string.IsNullOrEmpty(RoomsTextBox.Text) && !string.IsNullOrEmpty(FloorTextBox.Text))
                    {
                        if (!int.TryParse(RoomsTextBox.Text, out int roomsValue) || !int.TryParse(FloorTextBox.Text, out int totalFloorsValue))
                        {
                            MessageBox.Show("Введите корректные значения для количества комнат и этажей дома.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        rooms = roomsValue;
                        totalFloors = totalFloorsValue;
                    }

                    float? totalArea = null;
                    if (!string.IsNullOrEmpty(AreaTextBox.Text))
                    {
                        if (!float.TryParse(AreaTextBox.Text, out float areaValue))
                        {
                            MessageBox.Show("Введите корректное значение для площади дома.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        totalArea = areaValue;
                    }

                    var house = new House
                    {
                        Id = estate.Id,
                        Rooms = rooms,
                        TotalFloors = totalFloors,
                        TotalArea = totalArea
                    };
                    context.House.Add(house);
                }
                else if (selectedType.Name == "Квартира")
                {
                    int? floor = null, rooms = null;
                    if (!string.IsNullOrEmpty(FloorTextBox.Text) && !string.IsNullOrEmpty(RoomsTextBox.Text))
                    {
                        if (!int.TryParse(FloorTextBox.Text, out int floorValue) || !int.TryParse(RoomsTextBox.Text, out int roomsValue))
                        {
                            MessageBox.Show("Введите корректные значения для этажа и количества комнат квартиры.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        floor = floorValue;
                        rooms = roomsValue;
                    }

                    float? totalArea = null;
                    if (!string.IsNullOrEmpty(AreaTextBox.Text))
                    {
                        if (!float.TryParse(AreaTextBox.Text, out float areaValue))
                        {
                            MessageBox.Show("Введите корректное значение для площади квартиры.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        totalArea = areaValue;
                    }

                    var apartment = new Apartment
                    {
                        Id = estate.Id,
                        Floor = floor,
                        Rooms = rooms,
                        TotalArea = totalArea
                    };
                    context.Apartment.Add(apartment);
                }
                else if (selectedType.Name == "Земля")
                {
                    float? totalArea = null;
                    if (!string.IsNullOrEmpty(AreaTextBox.Text))
                    {
                        if (!float.TryParse(AreaTextBox.Text, out float areaValue))
                        {
                            MessageBox.Show("Введите корректное значение для площади земли.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        totalArea = areaValue;
                    }

                    var land = new Land
                    {
                        Id = estate.Id,
                        TotalArea = totalArea
                    };
                    context.Land.Add(land);
                }

                context.SaveChanges();
            }

            UpdateRealEstatesList();
            MessageBox.Show("Объект недвижимости успешно создан");
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
                        try
                        {
                            // Удаляем записи из смежных таблиц
                            var houseToRemove = context.House.FirstOrDefault(h => h.Id == selectedEstate.Id);
                            if (houseToRemove != null)
                            {
                                context.House.Remove(houseToRemove);
                            }

                            var apartmentToRemove = context.Apartment.FirstOrDefault(a => a.Id == selectedEstate.Id);
                            if (apartmentToRemove != null)
                            {
                                context.Apartment.Remove(apartmentToRemove);
                            }

                            var landToRemove = context.Land.FirstOrDefault(l => l.Id == selectedEstate.Id);
                            if (landToRemove != null)
                            {
                                context.Land.Remove(landToRemove);
                            }

                            // Удаляем запись из таблицы RealEstate
                            context.RealEstate.Remove(selectedEstate);

                            context.SaveChanges(); // Сохраняем все изменения в базе данных

                            MessageBox.Show("Недвижимость успешно удалена.");
                            UpdateEstateList(); // Обновляем список недвижимости после удаления
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Произошла ошибка при удалении недвижимости: {ex.Message}");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите недвижимость для удаления.");
            }

        }



    }
}
