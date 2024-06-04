using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using REstate1.Data.Entities;
using REstate1.Data;
using System.Text.RegularExpressions;

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
            if (string.IsNullOrEmpty(SurnameTextBox.Text) || string.IsNullOrEmpty(NameTextBox.Text) || string.IsNullOrEmpty(PatronymicTextBox.Text))
            {
                MessageBox.Show("Необходимо заполнить ФИО полностью");
                return;
            }

            string name = NameTextBox.Text;
            string surname = SurnameTextBox.Text;
            string patronymic = PatronymicTextBox.Text;

            Regex regex = new Regex(@"^[А-ЯЁ][а-яё]+$");

            if (!regex.IsMatch(name))
            {
                MessageBox.Show("Проверьте корректность имени");
                return;
            }

            Regex regex2 = new Regex(@"^[А-ЯЁ][а-яё]{2,}$");

            if (!regex2.IsMatch(surname))
            {
                MessageBox.Show("Проверьте корректность фамилии");
                return;
            }

            Regex regex3 = new Regex(@"^[А-ЯЁ][а-яё]{3,}$");

            if (!regex3.IsMatch(patronymic))
            {
                MessageBox.Show("Проверьте корректность отчества");
                return;
            }
            if (!string.IsNullOrEmpty(DealShareTextBox.Text))
            {
                string dealShare = DealShareTextBox.Text;

                if (!int.TryParse(dealShare, out int dealShareValue))
                {
                    MessageBox.Show("Доля от комиссии должна быть числом");
                    return;
                }

                if (dealShareValue < 0 || dealShareValue > 100)
                {
                    MessageBox.Show("Доля от комиссии должна быть числом от 0 до 100");
                    return;
                }
            }
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
            this.Close();
        }
    }
}
