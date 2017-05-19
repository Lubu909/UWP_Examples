using System;
using Template10.Mvvm;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace MediaCheck.Models {
    public class Movie : BindableBase {
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

        string _OriginalName = default(string);
        public string OriginalName {
            get { return _OriginalName; }
            set { Set(ref _OriginalName, value); }
        }

        string _Description = default(string);
        public string Description {
            get { return _Description; }
            set { Set(ref _Description, value); }
        }

        DateTime _ReleaseDate = default(DateTime);
        public DateTime ReleaseDate {
            get { return _ReleaseDate; }
            set { Set(ref _ReleaseDate, value); }
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
    }
}
