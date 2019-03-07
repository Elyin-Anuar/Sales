using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Sales.Common.Models;
using Sales.Services;
using Sales.ViewModels.Sales.ViewModels;
using Xamarin.Forms;

namespace Sales.ViewModels
{
    public class ProductsViewModel : BaseViewModel
    {
        private ApiService apiService;
        private bool isRefreshing;
        private ObservableCollection<Product> products;

        public ObservableCollection<Product> Products
        {
            get { return this.products; }
            set { this.SetValue(ref this.products, value); }
        }
        public bool IsRefreshing
        { 
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }
        public ProductsViewModel()
        {
            this.apiService = new ApiService();
            this.LoadProducts();
        }

        private async void LoadProducts()
        {
            this.IsRefreshing = true;
            var response = await
                this.apiService.GetList<Product>
                ("https://salesapi20190306034232.azurewebsites.net", "/api", "/Products/");
            if (!response.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error",response.Message,"Accept");
                return;
            }
            var list = (List<Product>)response.Result;
            this.Products = new ObservableCollection<Product>(list);
            this.IsRefreshing = false;
        }
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadProducts);
            }
        }
    }
}
