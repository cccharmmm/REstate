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
    /// <summary>
    /// Логика взаимодействия для AgentSupply.xaml
    /// </summary>
    public partial class AgentSupply : Window
    {
        private int agentId;
        public AgentSupply(int agentId)
        {
            InitializeComponent();
            this.agentId = agentId;
            LoadSupplies();
        }
        private void LoadSupplies()
        {
            using (var context = new RealEstateContext())
            {
                var supplies = context.Supply
                    .Where(d => d.AgentId == agentId)
                    .ToList();

                DemandDataGrid.ItemsSource = supplies;
            }
        }


        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
