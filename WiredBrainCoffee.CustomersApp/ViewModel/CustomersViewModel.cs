using Repository;
using Repository.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using WiredBrainCoffee.CustomersApp.Command;
using WiredBrainCoffee.CustomersApp.Common;
using WiredBrainCoffee.CustomersApp.Enum;

namespace WiredBrainCoffee.CustomersApp.ViewModel
{
    public class CustomersViewModel : ViewModelBase
    {
        private readonly IRepository<Customer> _customerRepository;

        public DelegateCommand AddCommand { get; }
        public DelegateCommand MoveNavigationCommand { get; }
        public GenericDelegateCommand<CustomerItemViewModel> DeleteCommand { get; }

        private CustomerItemViewModel? _selectedCustomer;
        private NavigationSideEnum _navigationSide;

        public CustomerItemViewModel? SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                _selectedCustomer = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsCustomerSelected");
                DeleteCommand.RaiseCanExecuteChanged();
            }
        }
        public NavigationSideEnum NavigationSide
        {
            get => _navigationSide;
            set
            {
                _navigationSide = value;
                RaisePropertyChanged();
            }
        }
    
        public ObservableCollection<CustomerItemViewModel> Customers { get; } = new();
        public bool IsCustomerSelected => SelectedCustomer is not null;
         

        public CustomersViewModel(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
            AddCommand = new DelegateCommand(Add);
            MoveNavigationCommand = new DelegateCommand(MoveNavigation);
            DeleteCommand = new GenericDelegateCommand<CustomerItemViewModel>(Delete, CanDelete);
        }

        public override void LoadData()
        {
            Customers.Clear();
            var customers = _customerRepository.All();
            if (customers is not null)
            {
                foreach (var customer in customers)
                {
                    Customers.Add(new CustomerItemViewModel(customer));
                }
            }
        }

        private void Add(object? parameter)
        {
            var customer = new Customer { FirstName = "New" };
            _customerRepository.Add(customer);
            var customerItemViewModel = new CustomerItemViewModel(customer);
            Customers.Insert(0, customerItemViewModel);
            SelectedCustomer = customerItemViewModel;
        }

        private void MoveNavigation(object? parameter)
        {
            NavigationSide = NavigationSide == NavigationSideEnum.Left ? NavigationSideEnum.Right : NavigationSideEnum.Left;
        }

        private bool CanDelete(CustomerItemViewModel? parameter) => parameter is not null;

        private void Delete(CustomerItemViewModel? parameter)
        {
            ArgumentNullException.ThrowIfNull(parameter);
            var isDeleted = _customerRepository.Delete(parameter.Id);
            if (isDeleted) { Customers.Remove(parameter); }
        }
    }
}
