using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


// To add offline sync support, add the NuGet package Microsoft.Azure.Mobile.Client.SQLiteStore
// to your project. Then, uncomment the lines marked // offline sync
// For more information, see: 
//https://azure.microsoft.com/en-us/documentation/articles/app-service-mobile-windows-store-dotnet-get-started-offline-data/
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;  // offline sync
using Microsoft.WindowsAzure.MobileServices.Sync;         // offline sync

namespace colorwebsitemobile
{
    sealed partial class MainPage: Page
    {
        private MobileServiceCollection<DemoColor, DemoColor> items;
        //private IMobileServiceTable<DemoColor> demoTable = App.MobileService.GetTable<DemoColor>();
        private IMobileServiceSyncTable<DemoColor> demoTable = App.MobileService.GetSyncTable<DemoColor>(); // offline sync

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async Task InsertDemoColor(DemoColor demoColor)
        {
            if (!string.IsNullOrWhiteSpace(demoColor.Name))
            {

                // This code inserts a new DemoColor into the database. When the operation completes
                // and Mobile App backend has assigned an Id, the item is added to the CollectionView.
                await demoTable.InsertAsync(demoColor);
                items.Add(demoColor);               
            }
        }

        private async Task RefreshDemoColorItems()
        {
            MobileServiceInvalidOperationException exception = null;
            try
            {
                // This code refreshes the entries in the list view by querying the DemoColor table.
                // The query excludes completed DemoColors.
                items = await demoTable                    
                    .ToCollectionAsync();
            }
            catch (MobileServiceInvalidOperationException e)
            {
                exception = e;
            }

            if (exception != null)
            {
                await new MessageDialog(exception.Message, "Error loading items").ShowAsync();
            }
            else
            {
                ListItems.ItemsSource = items;
                this.ButtonSave.IsEnabled = true;
            }
        }

        private async void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            ButtonRefresh.IsEnabled = false;

            await SyncAsync(); // offline sync
            await RefreshDemoColorItems();

            ButtonRefresh.IsEnabled = true;
        }

        private async void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            var demoColor = new DemoColor { Name = TextInput.Text };
            await InsertDemoColor(demoColor);
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await InitLocalStoreAsync(); // offline sync
            await RefreshDemoColorItems();
        }

        private async Task UpdateCheckedDemoColorItem(DemoColor item)
        {
            // This code removes the DemoColor from the database. When the service 
            // responds, the item is removed from the list.
            await demoTable.DeleteAsync(item);
            items.Remove(item);
            ListItems.Focus(FocusState.Unfocused);
        }

        private async void CheckBoxRemove_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            DemoColor item = cb.DataContext as DemoColor;
            await UpdateCheckedDemoColorItem(item);
        }

        #region Offline sync

        private async Task InitLocalStoreAsync()
        {
            if (!App.MobileService.SyncContext.IsInitialized)
            {
                var store = new MobileServiceSQLiteStore("localstore.db");
                store.DefineTable<DemoColor>();
                await App.MobileService.SyncContext.InitializeAsync(store);
            }

            await SyncAsync();
        }

        private async Task SyncAsync()
        {
            try
            {

                await App.MobileService.SyncContext.PushAsync();
                await demoTable.PullAsync("DemoColors", demoTable.CreateQuery());
            }
            catch (MobileServicePushFailedException)
            {
                // Push failed because of sync errors. You may be offine

            }
            catch (Exception)
            {

            }
        }

        #endregion 
    }
}
