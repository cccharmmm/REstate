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
    /// Логика взаимодействия для AgentDemand.xaml
    /// </summary>
    public partial class AgentDemand : Window
    {
        private int agentId;

        public AgentDemand(int agentId)
        {
            InitializeComponent();
            this.agentId = agentId;
            LoadDemands();
        }
        private void LoadDemands()
        {
            using (var context = new RealEstateContext())
            {
                var demands = context.Demands
                    .Where(d => d.AgentId == agentId)
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
