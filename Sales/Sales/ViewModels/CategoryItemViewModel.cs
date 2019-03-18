namespace Sales.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Common.Models;
    using Views;
    using System.Windows.Input;

    public class CategoryItemViewModel : Category
    {
        #region Comandos
        public ICommand GotoCategoryCommand
        {
            get
            {
                return new RelayCommand(GotoCategory);
            }
        }

        private async void GotoCategory()
        {
            MainViewModel.GetInstance().Products = new ProductsViewModel(this);
            await App.Navigator.PushAsync(new ProductsPage());
        }

        #endregion
    }
}
