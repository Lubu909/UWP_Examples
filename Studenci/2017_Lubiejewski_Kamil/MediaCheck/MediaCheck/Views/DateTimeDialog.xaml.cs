using MediaCheck.Services.Helpers;
using System;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MediaCheck.Views {
    public sealed partial class DateTimeDialog : ContentDialog {
        private string name = "tytuł";
        private string imageURL;
        public DateTimeDialog(string name, string imageURL) {
            this.InitializeComponent();
            this.name = name;
            this.imageURL = imageURL;
            calendar.Date = DateTime.Today;
        }

        private void remindMeClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) {
            DateTimeOffset notifyDate = calendar.Date ?? default(DateTimeOffset);
            TimeSpan notifyTime = time.Time;

            DateTime dateTime = notifyDate.Date + notifyTime;

            NotificationService.scheduleToast("Przypomnienie o emisji", name, imageURL, dateTime);
            NotificationService.setTile(name, imageURL, dateTime);
        }

        /*
        private void cancelRemindClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) {
            var notifier = ToastNotificationManager.CreateToastNotifier();
            ScheduledToastNotification toast = notifier.GetScheduledToastNotifications().FirstOrDefault(x => x.Tag == name);
            if(toast!=null) notifier.RemoveFromSchedule(toast);
        }
        */

        private void cancelClick(ContentDialog sender, ContentDialogButtonClickEventArgs args) {

        }
    }
}
