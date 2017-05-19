using MediaCheck.Views;
using System;
using System.Threading.Tasks;

namespace MediaCheck.Services.Helpers {
    public class DateTimeDialogService : IDateTimeDialogService {
        public async Task ShowAsync(string title, string imageURL) {
            var contentDialog = new DateTimeDialog(title, imageURL);
            await contentDialog.ShowAsync();
        }
    }
}
