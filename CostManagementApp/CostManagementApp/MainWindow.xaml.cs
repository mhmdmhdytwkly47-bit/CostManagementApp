using System.Windows;
using System.Windows.Controls;
using CostManagementApp.Models;
using CostManagementApp.Services;


namespace CostManagementApp;

public partial class MainWindow : Window
{
    private readonly ApiService _apiService;

    public MainWindow()
    {
        InitializeComponent();

        _apiService = new ApiService();

        LoadUsers();
    }

    private async void LoadUsers()
    {
        try
        {
            var users =
                await _apiService.GetUsers();

            UserComboBox.ItemsSource =
                users;

            UserComboBox.DisplayMemberPath =
                "full_name";

            UserComboBox.SelectedValuePath =
                "id";
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private async void LoadTransactions(int userId)
    {
        try
        {
            var data =
                await _apiService.GetTransactions(userId);

            TransactionsGrid.ItemsSource = data;

            decimal total =
                data.Sum(x => x.amount);

            TotalText.Text =
                $"جمع کل هزینه ها: {total:N0} تومان";
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private async void AddButton_Click(
        object sender,
        RoutedEventArgs e)
    {
        try
        {
            if (UserComboBox.SelectedValue == null)
                return;

            var transaction =
                new Transaction
                {
                    user_id =
                        (int)UserComboBox.SelectedValue,

                    category_id =
                        (int)CategoryComboBox.SelectedValue,
                    amount =
                        decimal.Parse(
                            AmountBox.Text),

                    description =
                        DescriptionBox.Text,

                    type = "expense"
                };

            await _apiService.AddTransaction(
                transaction);

            LoadTransactions(
                (int)UserComboBox.SelectedValue);

            AmountBox.Clear();
            DescriptionBox.Clear();

            MessageBox.Show("هزینه ثبت شد");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private async void DeleteButton_Click(
        object sender,
        RoutedEventArgs e)
    {
        try
        {
            if (TransactionsGrid.SelectedItem
                is Transaction transaction)
            {
                await _apiService.DeleteTransaction(
                    transaction.id);

                LoadTransactions(
                    (int)UserComboBox.SelectedValue);

                MessageBox.Show("هزینه حذف شد");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void UserComboBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e)
    {
        if (UserComboBox.SelectedValue == null)
            return;

        LoadCategories(
            (int)UserComboBox.SelectedValue);
    }

    private void TransactionsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (TransactionsGrid.SelectedItem is Transaction transaction)
        {
            AmountBox.Text =
                transaction.amount.ToString();

            DescriptionBox.Text =
                transaction.description;
        }
    }
    private async void UpdateButton_Click(
        object sender,
        RoutedEventArgs e)
    {
        try
        {
            if (TransactionsGrid.SelectedItem
                is Transaction transaction)
            {
                transaction.amount =
                    decimal.Parse(
                        AmountBox.Text);

                transaction.description =
                    DescriptionBox.Text;

                await _apiService
                    .UpdateTransaction(
                        transaction);

                LoadTransactions(
                    (int)UserComboBox.SelectedValue);

                MessageBox.Show(
                    "هزینه ویرایش شد");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message);
        }
    }
    private async void LoadCategories(
        int userId)
    {
        try
        {
            var categories =
                await _apiService
                    .GetCategories(userId);

            CategoryComboBox.ItemsSource =
                categories;

            CategoryComboBox.DisplayMemberPath =
                "name";

            CategoryComboBox.SelectedValuePath =
                "id";
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
    private async void AddUserButton_Click(
        object sender,
        RoutedEventArgs e)
    {
        try
        {
            var user =
                new User
                {
                    full_name =
                        UserNameBox.Text,

                    email =
                        EmailBox.Text
                };

            await _apiService
                .AddUser(user);

            MessageBox.Show(
                "کاربر ثبت شد");

            LoadUsers();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message);
        }
    }
}