using System.Collections.ObjectModel;
using Sales.Common.Models;
using Sales.ViewModels.Sales.ViewModels;

namespace Sales.ViewModels
{

 
    public class ProductsViewModel : BaseViewModel
    {
        private ObservableCollection<Product> products { get; set; }

        public ObservableCollection<Product> Products
        {
            get { return this.products; }
            set { this.SetValue(ref this.products, value); }
        }
    }
}
