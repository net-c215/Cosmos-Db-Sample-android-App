﻿using System;
using CosmosDbSampleApp.Shared;
using Xamarin.UITest;
using Xamarin.UITest.Android;
using Xamarin.UITest.iOS;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace CosmosDbSampleApp.UITests
{
    public class AddPersonPage : BasePage
    {
        readonly Query _saveButton, _cancelButton, _ageEntry, _nameEntry, _activityIndicator;

        public AddPersonPage(IApp app, string pageTitle) : base(app, pageTitle)
        {
            _saveButton = x => x.Marked(AutomationIdConstants.AddPersonPage_SaveButton);
            _cancelButton = x => x.Marked(AutomationIdConstants.AddPersonPage_CancelButton);
            _ageEntry = x => x.Marked(AutomationIdConstants.AddPersonPage_AgeEntry);
            _nameEntry = x => x.Marked(AutomationIdConstants.AddPersonPage_NameEntry);
            _activityIndicator = x => x.Marked(AutomationIdConstants.AddPersonPage_ActivityIndicator);
        }

        public void TapSaveButton()
        {
            switch (App)
            {
                case iOSApp iosApp:
                    iosApp.Tap(_saveButton);
                    break;
                case AndroidApp androidApp:
                    androidApp.Tap(x => x.Marked("Save"));
                    break;
                default:
                    throw new NotSupportedException();
            }

            App.Screenshot("Save Button Tapped");
        }

        public void TapCancelButton()
        {
            switch (App)
            {
                case iOSApp iosApp:
                    iosApp.Tap(_cancelButton);
                    break;
                case AndroidApp androidApp:
                    androidApp.Tap(x => x.Marked("Cancel"));
                    break;
                default:
                    throw new NotSupportedException();
            }

            App.Screenshot("Cancel Button Tapped");
        }

        public void EnterAge(int age)
        {
            App.EnterText(_ageEntry, age.ToString());
            App.DismissKeyboard();
            App.Screenshot($"Entered Age, {age}");
        }

        public void EnterName(string name)
        {
            App.EnterText(_nameEntry, name);
            App.DismissKeyboard();
            App.Screenshot($"Entered Name, {name}");
        }

        public void WaitForActivityIndicator()
        {
            App.WaitForElement(_activityIndicator);
            App.Screenshot("Activity Indicator Appeared");
        }

        public void WaitForNoActivityIndicator()
        {
            App.WaitForNoElement(_activityIndicator);
            App.Screenshot("Activity Indicator Disappeared");
        }
    }
}
