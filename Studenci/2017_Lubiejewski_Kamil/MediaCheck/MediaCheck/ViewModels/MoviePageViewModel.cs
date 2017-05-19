using MediaCheck.Models;
using MediaCheck.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Mvvm;
using Template10.Utils;
using Windows.UI.Xaml.Navigation;

namespace MediaCheck.ViewModels {
    public class MoviePageViewModel : ViewModelBase {
        readonly IDateTimeDialogService _dialog;
        Services.APIServices.TMDbService _moviesService;

        public MoviePageViewModel() {
            _dialog = new DateTimeDialogService();
            Movies = new ObservableCollection<Movie>();
            if(!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                _moviesService = new Services.APIServices.TMDbService();
        }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state) {
            RefreshCommand.Execute();
            return Task.CompletedTask;
        }

        ObservableCollection<Movie> _movies = default(ObservableCollection<Movie>);
        public ObservableCollection<Movie> Movies {
            get { return _movies; }
            private set { Set(ref _movies, value); }
        }

        ObservableCollection<Movie> _movies2 = default(ObservableCollection<Movie>);
        public ObservableCollection<Movie> MoviesBackup {
            get { return _movies2; }
            private set { Set(ref _movies2, value); }
        }

        Movie _selected = default(Movie);
        public object Selected {
            get { return _selected; }
            set {
                var movie = value as Movie;
                Set(ref _selected, movie);
                if(movie == null) return;
                IsDetailsLoading = true;
                WindowWrapper.Current().Dispatcher.Dispatch(() => {
                    IsDetailsLoading = false;
                }, 1000);
            }
        }

        public DelegateCommand RefreshCommand => new DelegateCommand(() => {
            IsMasterLoading = true;
            Movies.Clear();
            Selected = null;
            WindowWrapper.Current().Dispatcher.Dispatch(async () => {
                Movies.AddRange(await _moviesService.GetMovies());
                Selected = Movies?.FirstOrDefault();
                Favorites = false;
                IsMasterLoading = false;
            }, 2000);
        });

        public DelegateCommand showDialog => new DelegateCommand(async () => {
            var movie = Selected as Movie;
            await _dialog.ShowAsync(movie.Name, movie.Image);     
        });

        public DelegateCommand addFavorite => new DelegateCommand(async () => {
            using(var db = new Favorites()) {
                var movie = Selected as Movie;
                if(db.Movies.Any(x => x.Id == movie.Id)) {
                    db.Movies.Remove(db.Movies.First(x => x.Id == movie.Id));
                    if(Favorites == true)
                        Movies.Remove(movie);
                    NotificationService.sendNotificationToast("Usunięto z ulubionych", movie.Name, movie.Image);
                } else {
                    db.Movies.Add(movie);
                    NotificationService.sendNotificationToast("Dodano do ulubionych", movie.Name, movie.Image);
                }
                await db.SaveChangesAsync();
                System.Diagnostics.Debug.WriteLine("Liczba ulubionych : " + db.Movies.Count());
            }  
        });

        public DelegateCommand showFavorites => new DelegateCommand(() => {
            using(var db = new Favorites()) {
                if(Favorites == true) {
                    Movies = MoviesBackup;
                    Favorites = false;
                } else {
                    MoviesBackup = Movies;
                    Movies = db.Movies.OrderByDescending(x => x.ReleaseDate).ToObservableCollection();
                    Favorites = true;
                }
            }
        });

        public bool? Favorites = false;

        private bool _isDetailsLoading;
        public bool IsDetailsLoading {
            get { return _isDetailsLoading; }
            set { Set(ref _isDetailsLoading, value); }
        }

        private bool _isMasterLoading;
        public bool IsMasterLoading {
            get { return _isMasterLoading; }
            set { Set(ref _isMasterLoading, value); }
        }
    }
}
