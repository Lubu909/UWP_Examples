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
    public class SeriesPageViewModel : ViewModelBase {
        readonly IDateTimeDialogService _dialog;
        Services.APIServices.TMDbService _seriesService;

        public SeriesPageViewModel() {
            _dialog = new DateTimeDialogService();
            Series = new ObservableCollection<Series>();
            if(!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                _seriesService = new Services.APIServices.TMDbService();
        }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state) {
            RefreshCommand.Execute();
            return Task.CompletedTask;
        }

        ObservableCollection<Series> _series = default(ObservableCollection<Series>);
        public ObservableCollection<Series> Series {
            get { return _series; }
            private set { Set(ref _series, value); }
        }

        ObservableCollection<Series> _series2 = default(ObservableCollection<Series>);
        public ObservableCollection<Series> SeriesBackup {
            get { return _series2; }
            private set { Set(ref _series2, value); }
        }

        Series _selected = default(Series);
        public object Selected {
            get { return _selected; }
            set {
                var series = value as Series;
                Set(ref _selected, series);
                if(series == null)
                    return;
                IsDetailsLoading = true;
                WindowWrapper.Current().Dispatcher.Dispatch(() => {
                    IsDetailsLoading = false;
                }, 1000);
            }
        }

        public DelegateCommand RefreshCommand => new DelegateCommand(() => {
            IsMasterLoading = true;
            Series.Clear();
            Selected = null;
            WindowWrapper.Current().Dispatcher.Dispatch(async () => {
                Series.AddRange(await _seriesService.GetSeries());
                Selected = Series?.FirstOrDefault();
                Favorites = false;
                IsMasterLoading = false;
            }, 2000);
        });

        public DelegateCommand addFavorite => new DelegateCommand(async () => {
            using(var db = new Favorites()) {
                var series = Selected as Series;
                if(db.Series.Any(x => x.Id == series.Id)) {
                    db.Series.Remove(db.Series.First(x => x.Id == series.Id));
                    if(Favorites == true)
                        Series.Remove(series);
                    NotificationService.sendNotificationToast("Usunięto z ulubionych", series.Name, series.Image);
                } else {
                    db.Series.Add(series);
                    NotificationService.sendNotificationToast("Dodano do ulubionych", series.Name, series.Image);
                }
                await db.SaveChangesAsync();
                System.Diagnostics.Debug.WriteLine("Liczba ulubionych : " + db.Series.Count());
            }
        });

        public DelegateCommand showFavorites => new DelegateCommand(() => {
            using(var db = new Favorites()) {
                if(Favorites == true) {
                    Series = SeriesBackup;
                    Favorites = false;
                } else {
                    SeriesBackup = Series;
                    Series = db.Series.OrderByDescending(x => x.ReleaseDate).ToObservableCollection();
                    Favorites = true;
                }
            }
        });

        public bool? Favorites = false;

        public DelegateCommand showDialog => new DelegateCommand(async () => {
            var series = Selected as Series;
            await _dialog.ShowAsync(series.Name, series.Image);
        });

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
