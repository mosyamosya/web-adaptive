using System.Reflection;
using adaptweb4;
using Microsoft.VisualBasic.FileIO;

class Program
{
    static void Main(string[] args)
    {
        Book Harry_Potter_book = new Book(1, "Harry Potter", 399, 9.8, true);
        Book Day_Of_Dragon_book = new Book(2, "Day of Dragon", 448, 7.8, false);

        Type booktype = typeof (Book);
        TypeInfo booktypeinfo = booktype.GetTypeInfo();

        Console.WriteLine($"Назва типу: {booktype.Name}");
        Console.WriteLine("\n учасники:");
        foreach (MemberInfo memb in booktypeinfo.GetMembers())
        {
            Console.WriteLine($"Ім'я: {memb.Name}({memb.ReflectedType})");
        }

        Console.WriteLine("\nПоля:");
        foreach (FieldInfo fieldInfo in booktypeinfo.DeclaredFields)
        {
            Console.WriteLine($"Ім'я поля: {fieldInfo.Name} ({fieldInfo.Attributes}), FieldType: {fieldInfo.FieldType}");
        }
        Console.WriteLine("\nMетоди:");
        foreach (MethodInfo methodInfo in booktypeinfo.DeclaredMethods)
        {
            Console.WriteLine($"Ім'я метода: {methodInfo.Name}({methodInfo.Attributes}), MethodType: {methodInfo.ReturnType}");
        }

        Console.WriteLine("\nReflection:");
        MethodInfo reflectionMethod = booktype.GetMethod("WriteInfo");
        if (reflectionMethod != null)
        {
            reflectionMethod.Invoke(Harry_Potter_book, new object[] { Day_Of_Dragon_book });
        }
    }
}