using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Sales.Common.Models;
using Sales.Helpers;
using Sales.Services;
using Sales.ViewModels.Sales.ViewModels;
using Xamarin.Forms;

namespace Sales.ViewModels
{
    public class ProductsViewModel : BaseViewModel
    {
        #region Atributos
        private ApiService apiService;
        private bool isRefreshing;
        private ObservableCollection<ProductItemViewModel> products;
        #endregion

        #region Propiedades
        public List<Product> MyProdct { get; set; }
        public ObservableCollection<ProductItemViewModel> Products
        {
            get { return this.products; }
            set { this.SetValue(ref this.products, value); }
        }
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }
        #endregion

        #region Constructores
        public ProductsViewModel()
        {
            instance = this;
            this.apiService = new ApiService();
            this.LoadProducts();
        }
        #endregion

        #region Singleton
        private static ProductsViewModel instance;
        public static ProductsViewModel GetInstance()
        {
            if (instance == null)
            {
                return new ProductsViewModel();
            }
            return instance;
        }
        #endregion

        #region Metodos
        private async void LoadProducts()
        {
            this.IsRefreshing = true;

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.Accept);
                return;
            }
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlProductsController"].ToString();
            var response = await
                this.apiService.GetList<Product>
                (url, prefix, controller);
            if (!response.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            this.MyProdct = (List<Product>)response.Result;
            this.RefreshList();
            this.IsRefreshing = false;
        }

        public void RefreshList()
        {
            var mylistProductItemViewModel = MyProdct.Select(p => new ProductItemViewModel
            {
                Description = p.Description,
                ImageArray = p.ImageArray,
                ImagePath = p.ImagePath,
                IsAvailable = p.IsAvailable,
                Price = p.Price,
                ProductId = p.ProductId,
                PublishOn = p.PublishOn,
                Remarks = p.Remarks,
            });

            this.Products = new ObservableCollection<ProductItemViewModel>
                (mylistProductItemViewModel.OrderBy(p => p.Description));
        }
        #endregion

        #region Comandos
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadProducts);
            }
        } 
        #endregion
    }
}