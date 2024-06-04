using REstate1.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для Clients.xaml
    /// </summary>
    public partial class Clients : Window
    {
        public Clients()
        {
            InitializeComponent();
            Loaded += Clients_Loaded;
        }

        private void Agents_Click(object sender, RoutedEventArgs e)
        {
            Agents agents = new Agents();
            this.Close();
            agents.Show();
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

        private void UpdateClientList()
        {
            using (var context = new RealEstateContext())
            {
                ClientListBox.ItemsSource = context.Client.ToList();
            }
        }

        private void Clients_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateClientList();
        }

        public void CreateClient_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SurnameTextBox.Text) && string.IsNullOrEmpty(NameTextBox.Text) && string.IsNullOrEmpty(PatronymicTextBox.Text))
            {
                MessageBox.Show("Необходимо заполнить хотя бы одно из полей: Фамилия, Имя или Отчество.");
                return;
            }

            if (string.IsNullOrEmpty(PhoneTextBox.Text) && string.IsNullOrEmpty(EmailTextBox.Text))
            {
                MessageBox.Show("Необходимо указать хотя бы одно из полей: Номер телефона или Электронная почта.");
                return;
            }

            if (!string.IsNullOrEmpty(NameTextBox.Text))
            {
                string name = NameTextBox.Text;

                Regex regex = new Regex(@"^[А-ЯЁ][а-яё]+$");

                if (!regex.IsMatch(name))
                {
                    MessageBox.Show("Проверьте корректность имени");
                    return;
                }
            }
            if (!string.IsNullOrEmpty(SurnameTextBox.Text))
            {
                string surname = SurnameTextBox.Text;

                Regex regex = new Regex(@"^[А-ЯЁ][а-яё]{2,}$");

                if (!regex.IsMatch(surname))
                {
                    MessageBox.Show("Проверьте корректность фамилии");
                    return;
                }
            }
            if (!string.IsNullOrEmpty(PatronymicTextBox.Text))
            {
                string patronymic = PatronymicTextBox.Text;

                Regex regex = new Regex(@"^[А-ЯЁ][а-яё]{3,}$");

                if (!regex.IsMatch(patronymic))
                {
                    MessageBox.Show("Проверьте корректность отчества");
                    return;
                }
            }
            if (!string.IsNullOrEmpty(EmailTextBox.Text))
            {
                string email = EmailTextBox.Text;

                Regex emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

                if (!emailRegex.IsMatch(email))
                {
                    MessageBox.Show("Некорректный формат email");
                    return;
                }
            }
            if (!string.IsNullOrEmpty(PhoneTextBox.Text))
            {
                string phoneNumber = PhoneTextBox.Text;

                Regex phoneRegex = new Regex(@"^\+7\d{10}$");

                if (!phoneRegex.IsMatch(phoneNumber))
                {
                    MessageBox.Show("Некорректный формат номера телефона. Введите номер в формате +7XXXXXXXXXX.");
                    return;
                }
            }


            using (var context = new RealEstateContext())
            {
                var client = new Client
                {
                    LastName = SurnameTextBox.Text,
                    FirstName = NameTextBox.Text,
                    MiddleName = PatronymicTextBox.Text,
                    Phone = PhoneTextBox.Text,
                    Email = EmailTextBox.Text
                };

                context.Client.Add(client);
                context.SaveChanges();
                MessageBox.Show("Клиент успешно создан!");
            }
        }

        public void Update_Click(object sender, RoutedEventArgs e)
        {
            UpdateClientList();
        }

        public void DeleteClient_Click(Object sender, RoutedEventArgs e)
        {
            var selectedClient = ClientListBox.SelectedItem as Client;
            if (selectedClient != null)
            {
                MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить данного клиента?", "Подтверждение удаления", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    using (var context = new RealEstateContext())
                    {
                        context.Client.Attach(selectedClient);
                        context.Client.Remove(selectedClient);
                        context.SaveChanges();
                    }

                    MessageBox.Show("Клиент успешно удален.");
                    UpdateClientList();
                }
            }
            else
            {
                MessageBox.Show("Выберите клиента для удаления.");
            }
        }

        public void UpdateClient_Click(Object sender, RoutedEventArgs e)
        {
            var selectedClient = ClientListBox.SelectedItem as Client;
            if(selectedClient != null)
            {
                EditClients editClients = new EditClients(selectedClient);
                editClients.ShowDialog();
                UpdateClientList();
            }
            else
            {
                MessageBox.Show("Выберите клиента для редактирования.");
            }
        }
    }
}
