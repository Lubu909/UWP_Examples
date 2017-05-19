using MediaCheck.Models;
using MediaCheck.Services.Helpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Mvvm;
using Template10.Utils;
using Windows.UI.Xaml.Navigation;

namespace MediaCheck.ViewModels {
    public class AnimePageViewModel : ViewModelBase {
        readonly IDateTimeDialogService _dialog;
        Services.APIServices.AnimeService _animeService;

        public AnimePageViewModel() {
            _dialog = new DateTimeDialogService();
            Anime = new ObservableCollection<Anime>();
            if(!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                _animeService = new Services.APIServices.AnimeService();
        }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state) {
            RefreshCommand.Execute();
            return Task.CompletedTask;
        }

        ObservableCollection<Anime> _anime = default(ObservableCollection<Anime>);
        public ObservableCollection<Anime> Anime {
            get { return _anime; }
            private set { Set(ref _anime, value); }
        }

        ObservableCollection<Anime> _anime2 = default(ObservableCollection<Anime>);
        public ObservableCollection<Anime> AnimeBackup {
            get { return _anime2; }
            private set { Set(ref _anime2, value); }
        }

        Anime _selected = default(Anime);
        public object Selected {
            get { return _selected; }
            set {
                var anime = value as Anime;
                Set(ref _selected, anime);
                if(anime == null)
                    return;
                IsDetailsLoading = true;
                WindowWrapper.Current().Dispatcher.Dispatch(() => {
                    IsDetailsLoading = false;
                }, 1000);
            }
        }

        public DelegateCommand RefreshCommand => new DelegateCommand(() => {
            IsMasterLoading = true;
            Anime.Clear();
            Selected = null;
            WindowWrapper.Current().Dispatcher.Dispatch(async () => {
                Anime.AddRange(await _animeService.GetAnime());
                Selected = Anime?.FirstOrDefault();
                Favorites = false;
                IsMasterLoading = false;
            }, 2000);
        });

        public DelegateCommand showDialog => new DelegateCommand(async () => {
            var anime = Selected as Anime;
            await _dialog.ShowAsync(anime.Name, anime.Image);
        });

        public DelegateCommand addFavorite => new DelegateCommand(async () => {
            using(var db = new Favorites()) {
                var anime = Selected as Anime;
                if(db.Anime.Any(x => x.Id == anime.Id)) {
                    db.Anime.Remove(db.Anime.First(x => x.Id == anime.Id));
                    if(Favorites == true)
                        Anime.Remove(anime);
                    NotificationService.sendNotificationToast("Usunięto z ulubionych", anime.Name, anime.Image);
                } else {
                    db.Anime.Add(anime);
                    NotificationService.sendNotificationToast("Dodano do ulubionych", anime.Name, anime.Image);
                }
                await db.SaveChangesAsync();
                System.Diagnostics.Debug.WriteLine("Liczba ulubionych : " + db.Anime.Count());
            }
        });

        public DelegateCommand showFavorites => new DelegateCommand(() => {
            using(var db = new Favorites()) {
                if(Favorites == true) {
                    Anime = AnimeBackup;
                    Favorites = false;
                } else {
                    AnimeBackup = Anime;
                    Anime = db.Anime.OrderBy(x => x.AiringDate).ToObservableCollection();
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
