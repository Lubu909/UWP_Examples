using MediaCheck.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Template10.Utils;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

namespace MediaCheck.Services.APIServices {
    public partial class TMDbService {
        private static ObservableCollection<Movie> _movies;
        private static ObservableCollection<Series> _series;
        private static TMDbClient client = new TMDbClient(SettingsServices.APIKeys.TMDB);
        private static List<Genre> genresMovies = new List<Genre>();
        private static List<Genre> genresTV = new List<Genre>();

        public async Task<ObservableCollection<Movie>> GetMovies() {
            if(_movies != null)
                return _movies;
            return _movies = _movies
                ?? await getMoviesDataAsync();
        }

        public async Task<ObservableCollection<Series>> GetSeries() {
            if(_series != null)
                return _series;
            return _series = _series
                ?? await getDataAsync();
        }

        private async Task<string> FetchAsync(string url) {
            string jsonString;

            using(var httpClient = new System.Net.Http.HttpClient()) {
                var stream = await httpClient.GetStreamAsync(url);
                StreamReader reader = new StreamReader(stream);
                jsonString = reader.ReadToEnd();
            }

            return jsonString;
        }

        private async Task<ObservableCollection<Movie>> getMoviesDataAsync() {
            List<Movie> data = new List<Movie>();

            client.GetConfig();
            client.DefaultLanguage = "pl";

            genresMovies = await client.GetMovieGenresAsync("pl");

            int pageNumber = 1;
            int totalPages;
            do {
                SearchContainerWithDates<SearchMovie> response = await client.GetMovieNowPlayingListAsync("pl", pageNumber);

                foreach(SearchMovie movie in response.Results) {
                    if(movie.Overview.Length > 0 && data.Find(x => x.Id == movie.Id) == null) {
                        Movie item = new Movie();
                        item.Id = movie.Id;
                        item.Name = movie.Title;
                        item.OriginalName = movie.OriginalTitle;
                        item.Description = movie.Overview;
                        item.ReleaseDate = movie.ReleaseDate ?? default(DateTime);
                        item.Ratings = movie.VoteAverage;
                        if(movie.GenreIds.Count > 0)
                            item.Genres = parseGenres(movie.GenreIds, genresMovies);
                        else
                            item.Genres = "Nieznane";
                        item.Image = SettingsServices.TMDBSettings.ImagePath + SettingsServices.TMDBSettings.logoSize + movie.PosterPath;

                        data.Add(item);
                    }
                }

                totalPages = response.TotalPages;
            } while(pageNumber++ < totalPages);

            data = data.OrderByDescending(x => x.ReleaseDate).ToList();

            return data.ToObservableCollection();
        }

        private async Task<ObservableCollection<Series>> getDataAsync() {
            List<Series> data = new List<Series>();

            client.GetConfig();
            client.DefaultLanguage = "pl";

            genresTV = await client.GetTvGenresAsync("pl");

            int pageNumber = 1;
            int totalPages;
            do {
                SearchContainer<SearchTv> response = await client.GetTvShowListAsync(TMDbLib.Objects.TvShows.TvShowListType.OnTheAir, pageNumber, "pl");
                foreach(SearchTv series in response.Results) {
                    if(series.Overview.Length > 0 && data.Find(x => x.Id == series.Id) == null) {
                        Series item = new Series();
                        item.Id = series.Id;
                        item.Name = series.Name;
                        item.OriginalName = series.OriginalName;
                        item.Description = series.Overview;
                        item.ReleaseDate = series.FirstAirDate ?? default(DateTime);
                        item.Ratings = series.VoteAverage;
                        if(series.GenreIds.Count > 0)
                            item.Genres = parseGenres(series.GenreIds, genresTV);
                        else
                            item.Genres = "Nieznane";
                        item.Image = SettingsServices.TMDBSettings.ImagePath + SettingsServices.TMDBSettings.logoSize + series.PosterPath;

                        data.Add(item);
                    }
                }
                totalPages = response.TotalPages;
            } while(pageNumber++ < totalPages);

            data = data.OrderByDescending(x => x.ReleaseDate).ToList();

            return data.ToObservableCollection();
        }

        private string parseGenres(List<int> ids, List<Genre> genres) {
            string result = "";

            for(int i = 0; i < ids.Count; i++) {
                var found = genres.Find(x => x.Id == ids.ElementAt(i));
                if(found != null) {
                    result += found.Name;
                    if(i < ids.Count - 1)
                        result += ", ";
                }
            }

            return result;
        }
    }
}
