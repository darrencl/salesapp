using MvvmCross.Core.ViewModels;
using SalesApp.Core.Interfaces;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApp.Core.ViewModels
{
    public class CustomerAddViewModel: MvxViewModel
    {
        private readonly IDialogService dialog;
        private ICustomerDatabase customerDb;
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

        public CustomerAddViewModel(IDialogService ids, ICustomerDatabase icd)
        {
            dialog = ids;
            customerDb = icd;
        }

        /*Function: AddCustomer
         *Adding customer to sqlite based on the filled textbox
         */
        public IMvxCommand AddCustomer
        {
            get
            { return new MvxCommand(async () =>
                {
                    if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Address) && !string.IsNullOrEmpty(Phone))
                    {
                        if (CrossConnectivity.Current.IsConnected)
                        {
                            if (await CrossConnectivity.Current.IsRemoteReachable(Services.ServerDatabaseApi.ipAddress, int.Parse(Services.ServerDatabaseApi.port)))
                            {
                                if (Name != "" && Address != "" && Phone != "")
                                    funcAddCustomer();
                                else
                                {
                                    await dialog.Show("Please fill all the textboxes in order to add customer", "Customer Insert Fail");
                                }
                            }
                            else
                            {
                                await dialog.Show("Server is not reachable", "Connection Error");
                            }
                        }
                        else
                        {
                            await dialog.Show("No internet connection", "Connection Error");
                        }
                    }
                    else
                    {
                        await dialog.Show("Please fill all the textboxes needed", "Insert Failed");
                    }
                });
            }
        }
        /*Function: Back
         *Close the view model and going back to the previous
         */
        public IMvxCommand Back
        {
            get
            {
                return new MvxCommand(() => Close(this));
            }
        }
        private void funcAddCustomer()
        {
            //?? : CustomerID
            //Insert straight to the server and then sync to local is better option

            Models.Customer aCustomer = new Models.Customer();
            aCustomer.Name = Name;
            aCustomer.Address = Address;
            aCustomer.Phone = Phone;
            customerDb.InsertCustomer(aCustomer);

            Close(this);
        }
    }
}
