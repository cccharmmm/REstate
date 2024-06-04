using REstate1.Data.Entities;
using System.Linq;
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
