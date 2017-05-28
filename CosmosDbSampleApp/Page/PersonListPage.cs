﻿using System;
using Xamarin.Forms;

namespace CosmosDbSampleApp
{
    public class PersonListPage : BaseContentPage<PersonListViewModel>
    {
        readonly ListView _personList;
        readonly ToolbarItem _addButtonToolBarItem;

        public PersonListPage()
        {
            _addButtonToolBarItem = new ToolbarItem { Icon = "Add" };
            ToolbarItems.Add(_addButtonToolBarItem);

            _personList = new ListView(ListViewCachingStrategy.RecycleElement)
            {
                ItemTemplate = new DataTemplate(typeof(PersonListViewCell)),
                IsPullToRefreshEnabled = true,
                BackgroundColor = ColorConstants.PageBackgroundColor,
                HasUnevenRows = true
            };
            _personList.SetBinding(ListView.ItemsSourceProperty, nameof(ViewModel.PersonList));
            _personList.SetBinding(ListView.RefreshCommandProperty, nameof(ViewModel.PullToRefreshCommand));

            Content = _personList;

            Title = "Person List";
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Device.BeginInvokeOnMainThread(_personList.BeginRefresh);
        }

        protected override void SubscribeEventHandlers()
        {
			ViewModel.Error += HandleError;
            _personList.ItemTapped += HandleItemTapped;
            _addButtonToolBarItem.Clicked += HandleAddButtonClicked;
            ViewModel.PullToRefreshCompleted += HandlePullToRefreshCommand;
        }

        protected override void UnsubscribeEventHandlers()
        {
            ViewModel.Error -= HandleError;
			_personList.ItemTapped -= HandleItemTapped;
			_addButtonToolBarItem.Clicked -= HandleAddButtonClicked;
            ViewModel.PullToRefreshCompleted -= HandlePullToRefreshCommand;
        }

        void HandleError(object sender, string e) =>
            Device.BeginInvokeOnMainThread(async ()=> await DisplayAlert("Error", e, "OK "));

        void HandleItemTapped(object sender, ItemTappedEventArgs e)
        {
            var listView = sender as ListView;
            listView.SelectedItem = null;
        }

        void HandleAddButtonClicked(object sender, EventArgs e) =>
            Device.BeginInvokeOnMainThread(async () =>
                await Navigation.PushModalAsync(new NavigationPage(new AddPersonPage())
        {
            BarTextColor = ColorConstants.BarTextColor,
            BarBackgroundColor = ColorConstants.BarBackgroundColor
        }));

        void HandlePullToRefreshCommand(object sender, EventArgs e) =>
            Device.BeginInvokeOnMainThread(_personList.EndRefresh);
    }
}
