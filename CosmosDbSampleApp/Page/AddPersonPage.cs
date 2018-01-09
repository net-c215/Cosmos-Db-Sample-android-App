﻿using System;

using Xamarin.Forms;

using EntryCustomReturn.Forms.Plugin.Abstractions;

namespace CosmosDbSampleApp
{
    public class AddPersonPage : BaseContentPage<AddPersonViewModel>
    {
        #region Constant Fields
        const string _saveButtonToolBarItemText = "Save";
        const string _cancelButtonToolBarItemText = "Cancel";
        readonly AddPersonPageEntry _nameEntry;
        readonly ToolbarItem _cancelButtonToolbarItem;
        readonly ActivityIndicator _activityIndicator;
        #endregion  

        public AddPersonPage()
        {
            var saveButtonToolBar = new ToolbarItem
            {
                Text = _saveButtonToolBarItemText,
                Priority = 0,
            };
            saveButtonToolBar.SetBinding(ToolbarItem.CommandProperty, nameof(ViewModel.SaveButtonCommand));
            ToolbarItems.Add(saveButtonToolBar);

            _cancelButtonToolbarItem = new ToolbarItem
            {
                Text = _cancelButtonToolBarItemText,
                Priority = 1
            };
            ToolbarItems.Add(_cancelButtonToolbarItem);

            var ageLabel = new Label { Text = "Age" };

            var ageEntry = new AddPersonPageEntry
            {
                Placeholder = "Age",
                Keyboard = Keyboard.Numeric
            };
            ageEntry.SetBinding(Entry.TextProperty, nameof(ViewModel.AgeEntryText));
            ageEntry.SetBinding(CustomReturnEffect.ReturnCommandProperty, nameof(ViewModel.SaveButtonCommand));
            ageEntry.SetBinding(IsEnabledProperty, new Binding(nameof(ViewModel.IsActivityIndicatorActive), BindingMode.Default, new InverseBooleanConverter(), ViewModel.IsActivityIndicatorActive));
            CustomReturnEffect.SetReturnType(ageEntry, ReturnType.Go);

            var nameLabel = new Label { Text = "Name" };

            _nameEntry = new AddPersonPageEntry { Placeholder = "Name" };
            _nameEntry.SetBinding(Entry.TextProperty, nameof(ViewModel.NameEntryText));
            _nameEntry.SetBinding(IsEnabledProperty, new Binding(nameof(ViewModel.IsActivityIndicatorActive), BindingMode.Default, new InverseBooleanConverter(), ViewModel.IsActivityIndicatorActive));
            CustomReturnEffect.SetReturnCommand(_nameEntry, new Command(() => ageEntry.Focus()));
            CustomReturnEffect.SetReturnType(_nameEntry, ReturnType.Next);

            _activityIndicator = new ActivityIndicator();
            _activityIndicator.SetBinding(IsVisibleProperty, nameof(ViewModel.IsActivityIndicatorActive));
            _activityIndicator.SetBinding(ActivityIndicator.IsRunningProperty, nameof(ViewModel.IsActivityIndicatorActive));

            Padding = GetPageThickness();

            var stackLayout = new StackLayout
            {
                Children ={
                    nameLabel,
                    _nameEntry,
                    ageLabel,
                    ageEntry,
                    _activityIndicator
                }
            };

            Title = "Add Person";

            Content = new ScrollView { Content = stackLayout };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _nameEntry?.Focus();
        }

        protected override void SubscribeEventHandlers()
        {
			ViewModel.SaveErrorred += HandleError;
            ViewModel.SaveCompleted += HandleSaveCompleted;
			_cancelButtonToolbarItem.Clicked += HandleCancelButtonToolbarItemClicked;
        }

        protected override void UnsubscribeEventHandlers()
        {
            ViewModel.SaveErrorred -= HandleError;
			ViewModel.SaveCompleted += HandleSaveCompleted;
            _cancelButtonToolbarItem.Clicked -= HandleCancelButtonToolbarItemClicked;
        }

        void HandleCancelButtonToolbarItemClicked(object sender, EventArgs e)
        {
            if (_activityIndicator.IsRunning)
                return;

            PopPageFromNavigationStack();
        }

        void HandleError(object sender, string message)
        {
            Device.BeginInvokeOnMainThread(async () =>
                                           await DisplayAlert("Error", message, "ok"));
        }

        void HandleSaveCompleted(object sender, EventArgs e) => PopPageFromNavigationStack();

        void PopPageFromNavigationStack() =>
            Device.BeginInvokeOnMainThread(async () =>
                                           await Navigation.PopModalAsync());

        Thickness GetPageThickness() => new Thickness(20, 20, 20, 0);
    }
}
