using REstate1.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace REstate1
{
    public partial class Agents : Window
    {
        public Agents()
        {
            InitializeComponent();
            Loaded += Agent_Loaded;
        }

        private void Clients_Click(object sender, RoutedEventArgs e)
        {
            Clients clients = new Clients();
            this.Close();
            clients.Show();
        }

        private void RealEstates_Click(object sender, RoutedEventArgs e)
        {
            RealEstates realEstates = new RealEstates();
            this.Close();
            realEstates.Show();
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

        private void UpdateAgentList()
        {
            using (var context = new RealEstateContext())
            {
                AgentListBox.ItemsSource = context.Agent.ToList();
            }
        }

        private void Agent_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateAgentList();
        }

        private void CreateAgent_Click(object sender, RoutedEventArgs e)
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


            using (var context = new RealEstateContext())
            {
                var agent = new Agent
                {
                    LastName = SurnameTextBox.Text,
                    FirstName = NameTextBox.Text,
                    MiddleName = PatronymicTextBox.Text,
                    DealShare = Convert.ToInt32(DealShareTextBox.Text)
                };

                context.Agent.Add(agent);
                context.SaveChanges();
                MessageBox.Show("Риэлтор успешно создан!");
                UpdateAgentList();
            }
        }

        private void DeleteAgent_Click(object sender, RoutedEventArgs e)
        {
            var selectedAgent = AgentListBox.SelectedItem as Agent;
            if (selectedAgent != null)
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить данного риэлтора?", "Подтверждение удаления", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    using (var context = new RealEstateContext())
                    {
                        context.Agent.Attach(selectedAgent);
                        context.Agent.Remove(selectedAgent);
                        context.SaveChanges();
                    }

                    MessageBox.Show("Агент успешно удален.");
                    UpdateAgentList();
                }
            }
            else
            {
                MessageBox.Show("Выберите агента для удаления.");
            }
        }

        private void UpdateAgent_Click(object sender, RoutedEventArgs e)
        {
            var selectedAgent = AgentListBox.SelectedItem as Agent;
            if (selectedAgent != null)
            {
                EditAgents editAgent = new EditAgents(selectedAgent);
                editAgent.ShowDialog();
                UpdateAgentList();
            }
            else
            {
                MessageBox.Show("Выберите риэлтора для редактирования.");
            }
        }

        private void searchFIO_Click(object sender, RoutedEventArgs e)
        {
            List<string> nameParts = new List<string>();

            if (!string.IsNullOrEmpty(SurnameTextBox.Text))
                nameParts.Add(SurnameTextBox.Text);

            if (!string.IsNullOrEmpty(NameTextBox.Text))
                nameParts.Add(NameTextBox.Text);

            if (!string.IsNullOrEmpty(PatronymicTextBox.Text))
                nameParts.Add(PatronymicTextBox.Text);

            if (nameParts.Count == 0)
            {
                MessageBox.Show("Пожалуйста, заполните хотя бы одно из полей ФИО перед выполнением поиска.");
                return;
            }

            string targetName = string.Join(" ", nameParts);

            using (var context = new RealEstateContext())
            {
                var agents = context.Agent.ToList();

                var matchingAgents = agents.Where(agent =>
                    fuzzy_search($"{agent.LastName} {agent.FirstName} {agent.MiddleName}", targetName) ||
                    fuzzy_search(agent.LastName, targetName) ||
                    fuzzy_search(agent.FirstName, targetName) ||
                    fuzzy_search(agent.MiddleName, targetName));

                AgentListBox.ItemsSource = null;
                AgentListBox.Items.Clear();
                AgentListBox.ItemsSource = matchingAgents;
            }
        }
        private bool fuzzy_search(string name, string target, int threshold = 3)
        {
            int distance = LevenshteinDistance(name.ToLower(), target.ToLower());
            return distance <= threshold;
        }
        private int LevenshteinDistance(string s1, string s2)
        {
            if (string.IsNullOrEmpty(s1))
            {
                return string.IsNullOrEmpty(s2) ? 0 : s2.Length;
            }

            if (string.IsNullOrEmpty(s2))
            {
                return s1.Length;
            }

            int[] v0 = new int[s2.Length + 1];
            int[] v1 = new int[s2.Length + 1];

            for (int i = 0; i < v0.Length; i++)
            {
                v0[i] = i;
            }

            for (int i = 0; i < s1.Length; i++)
            {
                v1[0] = i + 1;

                for (int j = 0; j < s2.Length; j++)
                {
                    int cost = (s1[i] == s2[j]) ? 0 : 1;
                    v1[j + 1] = Math.Min(Math.Min(v1[j] + 1, v0[j + 1] + 1), v0[j] + cost);
                }

                for (int j = 0; j < v0.Length; j++)
                {
                    v0[j] = v1[j];
                }
            }

            return v1[s2.Length];
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            UpdateAgentList();
        }
    }
}
