namespace Sales.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Views;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class MainViewModel
    {
        #region Propiedades
        public LoginViewModel Login { get; set; }
        public EditProductViewModel EditProduct { get; set; }
        public ProductsViewModel Products { get; set; }
        public AddProductViewModel AddProduct { get; set; }
        #endregion

        #region Contrustores
        public MainViewModel()
        {
            instance = this;
        }
        #endregion

        #region Singleton
        private static MainViewModel instance;
        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }
            return instance;
        }
        #endregion


        #region Comandos
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
        #endregion
    }
}
