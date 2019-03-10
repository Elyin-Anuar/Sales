namespace Sales.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Views;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class MainViewModel
    {
        public ProductsViewModel Products { get; set; }
        public AddProductViewModel AddProduct { get; set; }

        public MainViewModel()
        { 
            this.Products = new ProductsViewModel();
        }
        public ICommand AddProductCommand
        {
            get
            {
                return new RelayCommand(GotoAddProduct);
            }
        }

        private async void GotoAddProduct()
        {
            this.AddProduct = new AddProductViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new AddProductPage());
        }
    }
}
