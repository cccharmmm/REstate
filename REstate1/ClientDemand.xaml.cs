using System;
using System.Collections.Generic;
using System.Linq;
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
    public partial class ClientDemand : Window
    {
        /// <summary>
        /// Логика взаимодействия для ClientDemand.xaml
        /// </summary>
        private int clientId;
        public ClientDemand(int clientId)
        {
            InitializeComponent();
            this.clientId = clientId;
            LoadDemands();
        }
        private void LoadDemands()
        {
            using (var context = new RealEstateContext())
            {
                var demands = context.Demands
                    .Where(d => d.ClientId == clientId)
                    .ToList();

                DemandDataGrid.ItemsSource = demands;
            }
        }


        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
