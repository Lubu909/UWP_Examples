using System;
using Template10.Mvvm;
using Windows.UI.Xaml.Media.Imaging;

namespace MediaCheck.Models {
    public class Anime : BindableBase {
        int _Id = default(int);
        public int Id {
            get { return _Id; }
            set { Set(ref _Id, value); }
        }

        string _Name = default(string);
        public string Name {
            get { return _Name; }
            set { Set(ref _Name, value); }
        }

        string _engName = default(string);
        public string EnglishName {
            get { return _engName; }
            set { Set(ref _engName, value); }
        }

        string _orgName = default(string);
        public string OriginalName {
            get { return _orgName; }
            set { Set(ref _orgName, value); }
        }

        string _Description = default(string);
        public string Description {
            get { return _Description; }
            set { Set(ref _Description, value); }
        }

        DateTime _AiringDate = default(DateTime);
        public DateTime AiringDate {
            get { return _AiringDate; }
            set { Set(ref _AiringDate, value); }
        }

        string _Image = default(string);
        public string Image {
            get { return _Image; }
            set { Set(ref _Image, value); }
        }

        double _Ratings = default(double);
        public double Ratings {
            get { return _Ratings; }
            set { Set(ref _Ratings, value); }
        }

        string _Genres = default(string);
        public string Genres {
            get { return _Genres; }
            set { Set(ref _Genres, value); }
        }

        int _nextEP = default(int);
        public int NextEpisode {
            get { return _nextEP; }
            set { Set(ref _nextEP, value); }
        }

        int _totalEP = default(int);
        public int TotalEpisodes {
            get { return _totalEP; }
            set { Set(ref _totalEP, value); }
        }
    }
}
