using Bogus;
using WebAdaptive.Models;

namespace WebAdaptive.Services.SeriesService
{
    public class SeriesService : ISeriesService
    {
        private readonly List<SeriesModel> _series = new List<SeriesModel>();
        private static int id = 1;

        public SeriesService()
        {
            try
            {
                _series.AddRange(new List<SeriesModel>
                {
                    new SeriesModel
                    {
                        Id = id++,
                        Title = "Better Call Saul",
                        Description = "A spin-off of the highly successful series \"Breaking Bad\" with already 6 seasons.",
                        NumberOfEpisodes = 60,
                        SeasonCount = 6,
                        IsOngoing = false
                    },
                    new SeriesModel
                    {
                        Id = id++,
                        Title = "The Crown",
                        Description = "The fifth season released in 2022, depicting the 90s.",
                        NumberOfEpisodes = 50,
                        SeasonCount = 5,
                        IsOngoing = false
                    },
                    new SeriesModel
                    {
                        Id = id++,
                        Title = "House of the Dragon",
                        Description = "A prequel to the mega-successful series \"Game of Thrones\".",
                        NumberOfEpisodes = 10,
                        SeasonCount = 1,
                        IsOngoing = true
                    },
                    new SeriesModel
                    {
                        Id = id++,
                        Title = "Ozark",
                        Description = "The fourth season, nominated for a Golden Globe, aired in 2022 and concluded the series.",
                        NumberOfEpisodes = 40,
                        SeasonCount = 4,
                        IsOngoing = false
                    },
                    new SeriesModel
                    {
                        Id = id++,
                        Title = "The Split",
                        Description = "One of the directors of the series is the famous actor Ben Stiller.",
                        NumberOfEpisodes = 10,
                        SeasonCount = 1,
                        IsOngoing = true
                    },
                    new SeriesModel
                    {
                        Id = id++,
                        Title = "The Tourist",
                        Description = "Jamie Dornan returns for the second season of this comedic thriller.",
                        NumberOfEpisodes = 20,
                        SeasonCount = 2,
                        IsOngoing = true
                    },
                    new SeriesModel
                    {
                        Id = id++,
                        Title = "Sunny Brothers",
                        Description = "Michelle Yeoh stars in this comedic action-packed series as Eileen Sun, the matriarch of a criminal family.",
                        NumberOfEpisodes = 10,
                        SeasonCount = 1,
                        IsOngoing = true
                    },
                    new SeriesModel
                    {
                        Id = id++,
                        Title = "Moon",
                        Description = "The latest Marvel series, a spin-off of \"Hawkeye\", but entirely different.",
                        NumberOfEpisodes = 10,
                        SeasonCount = 1,
                        IsOngoing = true
                    },
                    new SeriesModel
                    {
                        Id = id++,
                        Title = "Conviction",
                        Description = "In this gripping thriller, Peter Capaldi plays London detective Daniel Geggarty.",
                        NumberOfEpisodes = 10,
                        SeasonCount = 1,
                        IsOngoing = true
                    },
                    new SeriesModel
                    {
                        Id = id++,
                        Title = "Lords of the Air",
                        Description = "An innovative Marvel series.",
                        NumberOfEpisodes = 10,
                        SeasonCount = 1,
                        IsOngoing = true
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SeriesService constructor: {ex.Message}");
            }
        }

        public async Task<List<SeriesModel>> GetAllSeries()
        {
            try
            {
                return _series;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllSeries method: {ex.Message}");
                return null;
            }
        }

        public async Task<SeriesModel> GetSeriesById(int id)
        {
            try
            {
                return _series.FirstOrDefault(s => s.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetSeriesById method: {ex.Message}");
                return null;
            }
        }

        public async Task AddSeries(SeriesModel series)
        {
            try
            {
                if (series == null)
                    throw new ArgumentNullException(nameof(series));
                series.Id = id++;
                _series.Add(series);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddSeries method: {ex.Message}");
            }
        }

        public async Task UpdateSeries(int id, SeriesModel series)
        {
            try
            {
                if (series == null)
                    throw new ArgumentNullException(nameof(series));

                var existingSeries = _series.FirstOrDefault(s => s.Id == id);
                if (existingSeries != null)
                {
                    existingSeries.Title = series.Title;
                    existingSeries.Description = series.Description;
                    existingSeries.NumberOfEpisodes = series.NumberOfEpisodes;
                    existingSeries.SeasonCount = series.SeasonCount;
                    existingSeries.IsOngoing = series.IsOngoing;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateSeries method: {ex.Message}");
            }
        }

        public async Task DeleteSeries(int id)
        {
            try
            {
                var series = _series.FirstOrDefault(s => s.Id == id);
                if (series != null)
                {
                    _series.Remove(series);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteSeries method: {ex.Message}");
            }
        }
    }
}
