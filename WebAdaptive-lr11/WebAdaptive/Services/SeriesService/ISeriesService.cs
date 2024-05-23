using WebAdaptive.Models;

namespace WebAdaptive.Services.SeriesService
{
    public interface ISeriesService
    {
        Task<List<SeriesModel>> GetAllSeries();
        Task<SeriesModel> GetSeriesById(int id); 
        Task AddSeries(SeriesModel series);
        Task UpdateSeries(int id, SeriesModel series);
        Task DeleteSeries(int id);
    }
}
