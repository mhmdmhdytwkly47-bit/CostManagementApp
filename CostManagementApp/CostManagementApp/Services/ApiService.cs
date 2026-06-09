using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using CostManagementApp.Models;

namespace CostManagementApp.Services;

public class ApiService
{
    private readonly HttpClient _client;

    public ApiService()
    {
        _client = new HttpClient();

        _client.BaseAddress =
            new Uri("http://localhost:3000/");
    }

    // دریافت تراکنش های یک کاربر
    public async Task<List<Transaction>> GetTransactions(int userId)
    {
        var response =
            await _client.GetAsync(
                $"transactions/{userId}");

        response.EnsureSuccessStatusCode();

        var json =
            await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<List<Transaction>>(json)
               ?? new List<Transaction>();
    }

    // ثبت تراکنش جدید
    public async Task AddTransaction(Transaction transaction)
    {
        var json =
            JsonConvert.SerializeObject(transaction);

        var content =
            new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

        var response =
            await _client.PostAsync(
                "transactions",
                content);

        response.EnsureSuccessStatusCode();
    }

    // حذف تراکنش
    public async Task DeleteTransaction(int id)
    {
        var response =
            await _client.DeleteAsync(
                $"transactions/{id}");

        response.EnsureSuccessStatusCode();
    }
    // ویرایش تراکنش
    public async Task UpdateTransaction(
        Transaction transaction)
    {
        var json =
            JsonConvert.SerializeObject(
                transaction);

        var content =
            new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

        var response =
            await _client.PutAsync(
                $"transactions/{transaction.id}",
                content);

        response.EnsureSuccessStatusCode();
    }
    public async Task<List<Category>>
        GetCategories(int userId)
    {
        var response =
            await _client.GetAsync(
                $"categories/{userId}");

        response.EnsureSuccessStatusCode();

        var json =
            await response.Content.ReadAsStringAsync();

        return JsonConvert
                   .DeserializeObject<List<Category>>(json)
               ?? new List<Category>();
    }
    // ثبت کاربر جدید
    public async Task<List<User>> GetUsers()
    {
        var response =
            await _client.GetAsync("users");

        response.EnsureSuccessStatusCode();

        var json =
            await response.Content
                .ReadAsStringAsync();

        return JsonConvert
            .DeserializeObject<List<User>>(json);
    }
    public async Task AddUser(User user)
    {
        var json =
            JsonConvert.SerializeObject(user);

        var content =
            new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );

        var response =
            await _client.PostAsync(
                "users",
                content
            );

        response.EnsureSuccessStatusCode();
    }
    

}
