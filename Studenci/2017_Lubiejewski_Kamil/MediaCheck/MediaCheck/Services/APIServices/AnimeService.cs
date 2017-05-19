using MediaCheck.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Template10.Utils;
using Windows.Data.Json;

namespace MediaCheck.Services.APIServices {
    public partial class AnimeService {
        private static ObservableCollection<Anime> _animes;
        private static HttpClientHandler handler = new HttpClientHandler { UseDefaultCredentials = true };
        private static HttpClient client = new HttpClient(handler);
        private static string airingURL = "https://anilist.co/api/browse/anime?status=currently airing&full_page=true&airing_data=true&type=TV&genres_exclude=Ecchi";
        private static string tokenURL = "https://anilist.co/api/auth/access_token";
        private static string token;

        public async Task<ObservableCollection<Anime>> GetAnime() {
            if(_animes != null)
                return _animes;
            return _animes = _animes
                ?? await getAnimeDataAsync();
        }

        private async Task<ObservableCollection<Anime>> getAnimeDataAsync() {
            List<Anime> data = new List<Anime>();
            token = await getToken();

            System.Diagnostics.Debug.WriteLine("Token to: " + token);

            HttpClientHandler handler = new HttpClientHandler { UseDefaultCredentials = true };
            client = new HttpClient(handler);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);

            HttpResponseMessage response = await client.GetAsync(airingURL);

            System.Diagnostics.Debug.WriteLine("Data response: " + response.StatusCode.ToString());

            if(response.IsSuccessStatusCode) {
                string responseString = await response.Content.ReadAsStringAsync();
                JsonArray array = JsonArray.Parse(responseString);
                for(uint i=0; i<array.Count; i++) {
                    JsonObject anime = array.GetObjectAt(i);
                    Anime item = new Anime();
                    item.Id = Convert.ToInt32(anime.GetNamedNumber("id"));
                    item.Name = anime.GetNamedString("title_romaji");
                    item.EnglishName = anime.GetNamedString("title_english");
                    item.OriginalName = anime.GetNamedString("title_japanese");
                    item.Image = anime.GetNamedString("image_url_lge");
                    if(anime.GetNamedValue("airing").ValueType != JsonValueType.Null) {
                        item.NextEpisode = Convert.ToInt32(anime.GetNamedObject("airing").GetNamedNumber("next_episode"));
                        item.TotalEpisodes = Convert.ToInt32(anime.GetNamedNumber("total_episodes"));
                        item.AiringDate = DateTime.Parse(anime.GetNamedObject("airing").GetNamedString("time"));
                    }
                    item.Ratings = anime.GetNamedNumber("average_score");
                    item.Genres = parseGenres(anime.GetNamedArray("genres"));
                    //item.Description
                    data.Add(item);
                }

            }
            data = data.OrderBy(x => x.AiringDate).ToList();
            return data.ToObservableCollection();
        }

        private async Task<string> getToken() {
            HttpClientHandler handler = new HttpClientHandler { UseDefaultCredentials = true };
            client = new HttpClient(handler);

            var request = new TokenRequest();
            request.grant_type = "client_credentials";
            request.client_id = SettingsServices.APIKeys.AniListID;
            request.client_secret = SettingsServices.APIKeys.AniListSecret;

            string jsonString = JsonConvert.SerializeObject(request);
            //string jsonString = "{\n\t\"grant_type\":\"client_credentials\",\n\t\"client_id\":\"lubu909-wy6kn\",\n\t\"client_secret\":\"fw4Mw4Ra9MLB6UaoG9Q\"\n}";

            HttpResponseMessage response = await client.PostAsync(tokenURL, new StringContent(jsonString, Encoding.UTF8, "application/json"));

            System.Diagnostics.Debug.WriteLine("Token response: " + response.StatusCode.ToString());

            if(response.IsSuccessStatusCode) {
                System.Diagnostics.Debug.WriteLine("Pobrało token");
                string responseString = await response.Content.ReadAsStringAsync();
                JsonObject responseObject = JsonObject.Parse(responseString);
                return responseObject.GetNamedString("access_token");
            } else {
                string responseString = await response.Content.ReadAsStringAsync();
                JsonObject responseObject = JsonObject.Parse(responseString);
                System.Diagnostics.Debug.WriteLine(responseObject.GetNamedString("error_description"));
                throw new HttpRequestException();
            }
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

        private string parseGenres(JsonArray array) {
            string result = "";
            for(uint i=0; i< array.Count; i++) {
                result += array.GetStringAt(i);
                if(i < array.Count - 1)
                    result += ", ";
            }
            return result;
        }
    }

    class TokenRequest {
        public string grant_type;
        public string client_id;
        public string client_secret;
    }
}
