using System;
using System.Linq;
using System.Windows;
using REstate1.Data.Entities;
using System.Data.Entity;

namespace REstate1
{
    public partial class Deals : Window
    {
        public Deals()
        {
            InitializeComponent();
            Loaded += LoadData;
            LoadComboBoxData();
        }

        private void LoadData(object sender, RoutedEventArgs e)
        {
            using (var context = new RealEstateContext())
            {
                var deals = context.Deals
                    .Include(d => d.Demand)
                    .Include(d => d.Supply)
                    .ToList();

                DealsListBox.ItemsSource = deals;
            }
        }

        private void LoadComboBoxData()
        {
            using (var context = new RealEstateContext())
            {
                var supplyIds = context.Supply.Select(s => s.Id).Distinct().ToList();
                var demandIds = context.Demands.Select(d => d.Id).Distinct().ToList();

                SupplyComboBox.ItemsSource = supplyIds;
                DemandComboBox.ItemsSource = demandIds;
            }
        }

        private void SaveDeal_Click(object sender, RoutedEventArgs e)
        {
            int selectedSupplyId = (int)SupplyComboBox.SelectedItem;
            int selectedDemandId = (int)DemandComboBox.SelectedItem;

            var newDeal = new Deal
            {
                Supply_Id = selectedSupplyId,
                Demand_Id = selectedDemandId
            };

            using (var context = new RealEstateContext())
            {
                context.Deals.Add(newDeal);
                context.SaveChanges();
            }

            LoadData(null, null);
        }

        

        private void DeleteDeal_Click(object sender, RoutedEventArgs e)
        {
            if (DealsListBox.SelectedItem is Deal selectedDeal)
            {
                using (var context = new RealEstateContext())
                {
                    var deal = context.Deals
                        .FirstOrDefault(d => d.Supply_Id == selectedDeal.Supply_Id && d.Demand_Id == selectedDeal.Demand_Id);

                    if (deal != null)
                    {
                        context.Deals.Remove(deal);
                        context.SaveChanges();
                        MessageBox.Show("Сделка успешно удалена");
                    }
                    else
                    {
                        MessageBox.Show("Selected deal not found in the database.");
                    }
                }

                LoadData(null, null);
            }
            else
            {
                MessageBox.Show("Осуществите выбор сделки");
            }
        }

        private void Clients_Click(object sender, RoutedEventArgs e)
        {
            Clients clients = new Clients();
            Close();
            clients.Show();
        }

        private void Agents_Click(object sender, RoutedEventArgs e)
        {
            Agents agents = new Agents();
            Close();
            agents.Show();
        }

        private void RealEstates_Click(object sender, RoutedEventArgs e)
        {
            RealEstates realEstates = new RealEstates();
            Close();
            realEstates.Show();
        }

        private void Supplies_Click(object sender, RoutedEventArgs e)
        {
            Supplies supplies = new Supplies();
            Close();
            supplies.Show();
        }

        private void Demands_Click(object sender, RoutedEventArgs e)
        {
            Demands demands = new Demands();
            Close();
            demands.Show();
        }
    }
}
