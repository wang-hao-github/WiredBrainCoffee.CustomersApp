using WiredBrainCoffee.CustomersApp.Command;

namespace WiredBrainCoffee.CustomersApp.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private ViewModelBase? _selectedViewModel;
        public CustomersViewModel CustomersViewModel { get; }
        public ProductsViewModel ProductsViewModel { get; }

        public DelegateCommand SelecteViewModelCommand { get; }

        public ViewModelBase? SelectedViewModel {
            get => _selectedViewModel;
            set {
                _selectedViewModel = value;
                RaisePropertyChanged();
            }
        }
        public MainViewModel(CustomersViewModel customersViewModel, ProductsViewModel productsViewModel)
        {
            CustomersViewModel = customersViewModel;
            ProductsViewModel = productsViewModel;
            SelecteViewModelCommand = new DelegateCommand(SelecteViewModel);
            _selectedViewModel = CustomersViewModel;
        }
        public override void LoadData()
        {
            if (_selectedViewModel != null) {
                _selectedViewModel.LoadData();
            }
        }
        public void SelecteViewModel(object? parameter)
        {
            if (parameter != null) {
                SelectedViewModel = parameter as ViewModelBase;
                LoadData();
            }
        }
    }
}
