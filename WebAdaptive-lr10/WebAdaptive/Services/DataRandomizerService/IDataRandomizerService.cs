namespace WebAdaptive.Services.DataRandomizerService
{
    public interface IDataRandomizerService
    {
        int GetIntegerValue(); // version 1

        string GetTextValue(); // version 2

        byte[] GenerateExcelFile(); //version 3
    }
}
