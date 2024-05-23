using System.Text.RegularExpressions;
using System.Data;
using System.Linq.Expressions;
class Program
{
    static string fileName = "LoremIpsum.txt";
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        do
        {
            Console.WriteLine("Меню:");
            Console.WriteLine("1. Порахувати кількість слів");
            Console.WriteLine("2. Вирішити рівняння");
            Console.WriteLine("3. Вихід");

            Console.Write("Input:");
            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                switch (choice)
                {
                    case 1:
                        GetWordCount();
                        break;

                    case 2:
                        CalculateExpression();
                        break;

                    case 3:
                        Console.WriteLine("Приходьте ще!");
                        return;

                    default:
                        Console.WriteLine("Щось пішло не так.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Неправильний ввід.");
            }

            Console.WriteLine();
        } while (true);
    }

    //1
    static void GetWordCount()
    {
        Console.Write("Введіть текст (якщо тексту не буде, то буде використано текст за замовчуванням):");
        string? text = Console.ReadLine();

        MatchCollection matches = Regex.Matches(text, "[a-zA-Z0-9]");
        string[] words;

        if (matches.Count() > 0)
        {
            words = text.Split(new char[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            Console.WriteLine($"кількість слів:{words.Length}");
            return;
        }

        string currentDirectory = Directory.GetCurrentDirectory();
        string filePath = Path.Combine(currentDirectory, "FileName.txt");
        string content = File.ReadAllText(filePath);
        words = content.Split(new char[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        Console.WriteLine($"текст за замовчуванням: {content}");
        Console.WriteLine($"кількість слів:{words.Length}");
        return;
    }
    //2
    static void CalculateExpression()
    {
        DataTable table = new DataTable();
        Console.Write("введіть рівняння:");

        string expr = Console.ReadLine();

        try
        {
            table.Columns.Add("expression", typeof(string), expr);
            DataRow row = table.NewRow();
            table.Rows.Add(row);
            Console.WriteLine($"Результат: {double.Parse((string)row["expression"])}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"щось пішло не так: {ex.Message}");
        }
    }
}