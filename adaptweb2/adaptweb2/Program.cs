using System.Collections;
using System.Text;
using System.Data;
using System.Diagnostics;
using System.Linq;

class Program
{
    static void Main()
    {
        //system.text
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Привіт,");
        sb.AppendLine("як справи?");
        Console.WriteLine(sb.ToString());

        //system.collections
        List<string> myList = new List<string>();
        myList.Add("яблуко");
        myList.Add("банан");
        myList.Add("апельсин");
        Console.WriteLine("Елементи списку:");
        foreach (var item in myList)
        {Console.WriteLine(item);}
        myList.Remove("банан");
        Console.WriteLine("\nПісля видалення:");
        foreach (var item in myList)
        {Console.WriteLine(item);}

        //system.data
        DataTable table = new DataTable();
        table.Columns.Add("ID", typeof(int));
        table.Columns.Add("Name", typeof(string));
        table.Rows.Add(1, "John");
        table.Rows.Add(2, "Alice");
        table.Rows.Add(3, "Bob");
        Console.WriteLine("Вміст таблиці:");
        foreach (DataRow row in table.Rows)
        {Console.WriteLine($"ID: {row["ID"]}, Name: {row["Name"]}");}

        //system.diagnostics
        string path = "C:\\Windows\\System32\\notepad.exe";
        try
        {
            Process process = new Process();
            process.StartInfo.FileName = path;
            process.Start();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"помилка при запуску процесу: {ex.Message}");
        }

        //system.linq
        List<int> numbers = new List<int> { 5, 2, 8, 1, 10, 3 };
        var filteredNumbers = numbers.Where(x => x > 3);
        var sortedNumbers = numbers.OrderByDescending(x => x);
        Console.WriteLine("Відфільтровані числа (більше 3):");
        foreach (var num in filteredNumbers)
        {
            Console.WriteLine(num);
        }
        Console.WriteLine("\nВідсортовані числа (за спаданням):");
        foreach (var num in sortedNumbers)
        {
            Console.WriteLine(num);
        }
    }
}
