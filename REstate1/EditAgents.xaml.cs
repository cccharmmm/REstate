using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using REstate1.Data.Entities;
using REstate1.Data;

namespace REstate1
{
    public partial class EditAgents : Window
    {
        public Agent Agent { get; set; }

        public EditAgents(Agent agent)
        {
            InitializeComponent();
            Agent = agent;
            DataContext = Agent;
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            var editedAgent = DataContext as Agent;

            if (editedAgent != null)
            {
                using (var context = new RealEstateContext())
                {
                    var original = context.Agent.FirstOrDefault(a => a.Id == Agent.Id);

                    if (original != null)
                    {
                        original.LastName = editedAgent.LastName;
                        original.FirstName = editedAgent.FirstName;
                        original.MiddleName = editedAgent.MiddleName;
                        original.DealShare = editedAgent.DealShare;

                        context.SaveChanges();

                        MessageBox.Show("Изменения успешно сохранены.");
                        Close(); 
                    }
                    else
                    {
                        MessageBox.Show("Не удалось найти агента для редактирования.");
                    }
                }
            }
        }

        private void CancelChanges_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new Agent { Id = Agent.Id, LastName = Agent.LastName, FirstName = Agent.FirstName, MiddleName = Agent.MiddleName, DealShare = Agent.DealShare };
        }
    }
}
