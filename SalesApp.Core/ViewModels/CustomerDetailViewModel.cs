using MvvmCross.Core.ViewModels;
using SalesApp.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApp.Core.ViewModels
{
    public class CustomerDetailViewModel: MvxViewModel
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; RaisePropertyChanged(() => Name); }
        }
        private string address;
        public string Address
        {
            get { return address; }
            set { address = value; RaisePropertyChanged(() => Address); }
        }
        private string phone;
        public string Phone
        {
            get { return phone; }
            set { phone = value; RaisePropertyChanged(() => Phone); }
        }
        public IMvxCommand Back
        {
            get { return new MvxCommand(() => Close(this)); }
        }
        public CustomerDetailViewModel()
        {
            Name = GlobalVars.selectedCustomer.Name;
            Address = GlobalVars.selectedCustomer.Address;
            Phone = GlobalVars.selectedCustomer.Phone;
        }
    }
}
