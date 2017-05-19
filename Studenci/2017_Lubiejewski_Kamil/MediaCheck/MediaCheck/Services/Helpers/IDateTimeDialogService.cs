using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaCheck.Services.Helpers {
    public interface IDateTimeDialogService {
        Task ShowAsync(string title, string imageURL);
    }
}
