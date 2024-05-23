using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        ThreadDemo();
        AsyncAwaitDemo().Wait();
        GetWeatherAsync().Wait(); 
    }

    static void ThreadDemo()
    {
        Console.WriteLine("ThreadDemo знаходиться в стеку");

        Thread thread1 = new Thread(() =>
        {
            for (int i = 0; i < 20; i++)
            {
                Console.WriteLine("Tread1 ітерація");
                Console.WriteLine(i);
            }
        });

        Thread thread2 = new Thread(() =>
        {
            for (int i = 0; i < 20; i++)
            {
                Console.WriteLine("Tread2 ітерація");
                Console.WriteLine(i);
            }
        });

        thread1.Start();
        thread2.Start();

        Console.WriteLine("ThreadDemo поза стеком");
    }

    static async Task AsyncAwaitDemo()
    {
        Console.WriteLine("AsyncAwaitDemo знаходиться в стеку");

        await Task.Run(async () =>
        {
            Console.WriteLine("Початок першої асинхронної операції (чекайте 2000 мс)");
            await Task.Delay(2000);
            Console.WriteLine("Кінець першої асинхронної операції");
        });
        await Task.Run(async () =>
        {
            Console.WriteLine("Початок другої асинхронної операції (чекайте 1000 мс)");
            await Task.Delay(1000);
            Console.WriteLine("Кінець другої асинхронної операції");
        });
        Console.WriteLine("AsyncAwaitDemo поза стеком");
    }

    static async Task GetWeatherAsync()
    {
        Console.WriteLine("GetWeatherAsync знаходиться в стеку");

        HttpClient client = new HttpClient();
        string city = "Миколаїв";
        string apiKey = "b281fdd26c696cbccf7d09f3889dcc39";
        string apiUrl = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}";
        HttpResponseMessage response = await client.GetAsync(apiUrl);

        if (response.IsSuccessStatusCode)
        {
            string data = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Погодні дані: {data}");
        }
        else
        {
            Console.WriteLine("Помилка отримання погодних даних.");
        }
        Console.WriteLine("GetWeatherAsync поза стеком");
    }
}
