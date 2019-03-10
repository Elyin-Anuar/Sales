﻿namespace Sales.ViewModels
{
    using Common.Models;
    using GalaSoft.MvvmLight.Command;
    using global::Sales.Helpers;
    using global::Sales.Services;
    using global::Sales.ViewModels.Sales.ViewModels;
    using Plugin.Media;
    using Plugin.Media.Abstractions;
    using System.Linq;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class EditProductViewModel : BaseViewModel
    {
        #region Atributos
        private Product product;
        private MediaFile file;
        private ImageSource imageSource;
        private ApiService apiServices;
        private bool isRunning;
        private bool isEnabled;
        #endregion

        #region Propiedades
        public Product Product
        {
            get { return this.product; }
            set { this.SetValue(ref this.product, value); }
        }
        public bool IsRunning
        {
            get { return this.isRunning; }
            set { this.SetValue(ref this.isRunning, value); }
        }
        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { this.SetValue(ref this.isEnabled, value); }
        }

        public ImageSource ImageSource
        {
            get { return this.imageSource; }
            set { this.SetValue(ref this.imageSource, value); }
        }
        #endregion

        #region Constructores
        public EditProductViewModel(Product product)
        {
            this.product = product;
            this.apiServices = new ApiService();
            this.isEnabled = true;
            this.ImageSource = product.ImageFullPath ;
        }

        #endregion
        #region Commands
        public ICommand ChangeImageCommand
        {
            get
            {
                return new RelayCommand(ChangeImage);
            }
        }

        private async void ChangeImage()
        {
            await CrossMedia.Current.Initialize();

            var source = await Application.Current.MainPage.DisplayActionSheet(
                Languages.ImageSource,
                Languages.Cancel,
                null,
                Languages.FromGallery,
                Languages.NewPicture);

            if (source == Languages.Cancel)
            {
                this.file = null;
                return;
            }

            if (source == Languages.NewPicture)
            {
                this.file = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        Name = "test.jpg",
                        PhotoSize = PhotoSize.Small,
                    }
                );
            }
            else
            {
                this.file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (this.file != null)
            {
                this.ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = this.file.GetStream();
                    return stream;
                });
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand(Save);
            }
        }

        private async void Save()
        {
            if (string.IsNullOrEmpty(this.Product.Description))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.DescriptionError,
                    Languages.Accept);
                return;
            }

            if (this.Product.Price < 0)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PriceError,
                    Languages.Accept);
                return;
            }
            this.IsRunning = true;
            this.IsEnabled = false;

            var connection = await this.apiServices.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Accept);
                return;
            }

            byte[] imageArray = null;
            if (this.file != null)
            {
                imageArray = FilesHelper.ReadFully(this.file.GetStream());
                this.Product.ImageArray = imageArray;
            }

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlProductsController"].ToString();
            var response = await
                this.apiServices.Put
                (url, prefix, controller, this.Product, this.Product.ProductId);

            if (!response.IsSuccess)
            {
                this.isRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Accept);
                return;
            }
            var newProduct = (Product)response.Result;
            var productsViewModel = ProductsViewModel.GetInstance();

            var oldProduct = productsViewModel.MyProdct.Where
                (p => p.ProductId == this.Product.ProductId).FirstOrDefault();
            if (oldProduct !=null)
            {
                productsViewModel.MyProdct.Remove(oldProduct);
            }

            productsViewModel.MyProdct.Add(newProduct);
            productsViewModel.RefreshList();

            this.isRunning = false;
            this.IsEnabled = true;

            await Application.Current.MainPage.Navigation.PopAsync();
        }
        #endregion

    }
}