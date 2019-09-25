using System;
using ELMA.BirthdayTerminal.Models;
using ELMA.BirthdayTerminal.Services;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace ELMA.BirthdayTerminal
{
    /// <summary>
    /// Попап с настройками подключения
    /// </summary>
    public partial class SettingsPopupPage : PopupPage
    {
        private ILocalStorageService localStorageService;
        private ISecurityService securityService;

        private GlobalSettings settings;

        /// <summary>
        /// Ctor
        /// </summary>
        public SettingsPopupPage()
		{
			InitializeComponent();

            localStorageService = DependencyService.Get<ILocalStorageService>();
            securityService = DependencyService.Get<ISecurityService>();
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public SettingsPopupPage(GlobalSettings settings): this()
        {
            this.settings = settings;

            BindInput(settingsHost, Entry.TextProperty, nameof(GlobalSettings.Host), settings);
            BindInput(settingsLogin, Entry.TextProperty, nameof(GlobalSettings.Login), settings);
            BindInput(settingsPassword, Entry.TextProperty, nameof(GlobalSettings.Password), settings);
        }

        /// <summary>
        /// Биндинг
        /// </summary>
        private void BindInput(InputView input, BindableProperty property, string path, GlobalSettings settings)
        {
            input.SetBinding(property, path);
            input.BindingContext = settings;
        }

        /// <summary>
        /// При нажатии кнопки сохранения настроек
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnOk(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                SettingsMenuPage.OnNewGlobalSettings(settings);
            });

            CloseAllPopup();
        }

        /// <summary>
        /// При отмене редактирования настроек
        /// </summary>
        /// <returns></returns>
        protected override bool OnBackgroundClicked()
        {
            CloseAllPopup();

            return false;
        }

        /// <summary>
        /// Закрыть попап
        /// </summary>
        private async void CloseAllPopup()
        {
            await PopupNavigation.Instance.PopAllAsync();
        }
    }
}