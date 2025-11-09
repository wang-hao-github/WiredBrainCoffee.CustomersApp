using Repository;
using Repository.Model;
using System.Collections.ObjectModel;
using System.Linq;

namespace WiredBrainCoffee.CustomersApp.ViewModel
{
    public class ProductsViewModel:ViewModelBase
    {
        private readonly IRepository<Product> _repository;

        public ProductsViewModel(IRepository<Product> repository)
        {
            _repository = repository;
        }
        public ObservableCollection<Product> Products { get; } = new();
        public override void LoadData()
        {
            if (Products.Any()) { return; }

            var products = _repository.All("id,desc");
            foreach (var item in products)
            {
                Products.Add(item);
            }
        }
    }
}
