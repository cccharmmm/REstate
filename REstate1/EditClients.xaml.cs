using REstate1.Data.Entities;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace REstate1
{
    /// <summary>
    /// Логика взаимодействия для EditClients.xaml
    /// </summary>
    public partial class EditClients : Window
    {
        public Client Client { get; set; }
        public EditClients(Client client)
        {
            InitializeComponent();
            Client = client;
            DataContext = Client;
        }
        public void SaveChanges_Click(object sender, RoutedEventArgs e)
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

            var editedClient = DataContext as Client;

            if (editedClient != null)
            {
                using (var context = new RealEstateContext())
                {
                    var original = context.Client.FirstOrDefault(c => c.Id == Client.Id);

                    if (original != null)
                    {
                        original.LastName = editedClient.LastName;
                        original.FirstName = editedClient.FirstName;
                        original.MiddleName = editedClient.MiddleName;
                        original.Phone = editedClient.Phone;
                        original.Email = editedClient.Email;

                        context.SaveChanges();
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
            DataContext = new Client { Id = Client.Id, LastName = Client.LastName, FirstName = Client.FirstName, MiddleName = Client.MiddleName, Phone = Client.Phone, Email = Client.Email };
            this.Close();
        }
    }
    
}
