using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;

namespace MediaCheck.ViewModels {
    class DateTimeViewModel : ViewModelBase {
        public DelegateCommand remindMeClick => new DelegateCommand(() => {
            
        });

        public DelegateCommand cancelRemindClick => new DelegateCommand(() => {

        });

        public DelegateCommand cancelClick => new DelegateCommand(() => {
            
        });
        
    }
}
