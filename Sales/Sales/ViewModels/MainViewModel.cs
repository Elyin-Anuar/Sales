namespace Sales.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Views;
    using System.Windows.Input;
    using Xamarin.Forms;
    using System.Collections.ObjectModel;
    using System;
    using global::Sales.Helpers;
    using global::Sales.Common.Models;

    public class MainViewModel
    {
        #region Propiedades
        public MyUserASP UserASP { get; set; }
        public LoginViewModel Login { get; set; }
        public RegisterViewModel Register { get; set; }
        public EditProductViewModel EditProduct { get; set; }
        public ProductsViewModel Products { get; set; }
        public AddProductViewModel AddProduct { get; set; }
        public ObservableCollection<MenuItemViewModel> Menu { get; set; }
        public string UserFullName
        {
            get
            {
                if (this.UserASP != null && this.UserASP.Claims != null && this.UserASP.Claims.Count > 1)
                {
                    return $"{this.UserASP.Claims[0].ClaimValue} {this.UserASP.Claims[1].ClaimValue}";
                }

                return null;
            }
        }
        public string UserImageFullPath
        {
            get
            {
                if (this.UserASP != null && this.UserASP.Claims != null && this.UserASP.Claims.Count > 3)
                {
                    return $"https://salesapi20190306034232.azurewebsites.net{this.UserASP.Claims[3].ClaimValue.Substring(1)}";
                }

                return null;
            }
        }
        #endregion

        #region Contrustores
        public MainViewModel()
        {
            instance = this;
            this.LoadMenu();
        }
        #endregion

        #region Metodos
        private void LoadMenu()
        {
            this.Menu = new ObservableCollection<MenuItemViewModel>();

            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_perm_device_information",
                PageName = "AbautPage",
                Title = Languages.About,
            });

            this.Menu.Add(new MenuItemViewModel
            {

                Icon = "ic_phonelink_setup",
                PageName = "SetupPage",
                Title = Languages.Setup,
            });

            this.Menu.Add(new MenuItemViewModel
            {

                Icon = "ic_exit_to_app",
                PageName = "LoginPage",
                Title = Languages.Exit,
            });
        }
        #endregion   #endregion
        
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
            await App.Navigator.PushAsync(new AddProductPage());

        }
        #endregion
    }
}