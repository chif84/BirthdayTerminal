using ELMA.BirthdayTerminal.Models;
using ELMA.BirthdayTerminal.Services;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms;

namespace ELMA.BirthdayTerminal
{
    /// <summary>
    /// Страница меню запуска терминала
    /// </summary>
    public partial class SettingsMenuPage : ContentPage
    {
        private ISecurityService securityService;
        private ILocalStorageService localStorageService;
        private IBirthdayEventService birthdayEventService;

        private GlobalSettings settings;

        /// <summary>
        /// Ctor
        /// </summary>
        public SettingsMenuPage()
        {
            InitializeComponent();

            securityService = DependencyService.Get<ISecurityService>();
            localStorageService = DependencyService.Get<ILocalStorageService>();
            birthdayEventService = DependencyService.Get<IBirthdayEventService>();

            settings = localStorageService.Get<GlobalSettings>(typeof(GlobalSettings).Name);
            if (settings == null)
            {
                settings = new GlobalSettings() { Host = "localhost", Login = "admin" };
            }

            CheckConnection();
        }

        /// <summary>
        /// При нажатии кнопки Настройки
        /// </summary>
        private async void OnButtonSettingsClicked(object sender, EventArgs e)
        {
            var settingsPopup = new SettingsPopupPage((GlobalSettings)settings.Clone());

            NewGlobalSettingsEvent += (eventSender, settings) =>
            {
                this.settings = settings;

                securityService.ResetConnection();
                localStorageService.Set(typeof(GlobalSettings).Name, settings);
            };

            await PopupNavigation.Instance.PushAsync(settingsPopup);
        }

        private void BindInput(InputView input, BindableProperty property, string path, GlobalSettings settings)
        {
            input.SetBinding(property, path);
            input.BindingContext = settings;
        }

        /// <summary>
        /// При нажатии кнопки Подключиться
        /// </summary>
        private void OnButtonBeginClicked(object sender, EventArgs e)
        {
            var chekConnection = CheckConnection();
            if (string.IsNullOrWhiteSpace(chekConnection))
            {
                birthdayEventService.SyncronizeEvents(settings);
                App.Current.MainPage = new TerminalMenuPage(settings);
            }
            else
            {
                DependencyService.Get<IMessageService>().ShortAlert(chekConnection);
            }
        }

        /// <summary>
        /// Проверка соединения
        /// </summary>
        /// <returns></returns>
        private string CheckConnection()
        {
            return securityService.CheckConnection(settings);
        }

        #region События

        public static event EventHandler<GlobalSettings> NewGlobalSettingsEvent;
        public static void OnNewGlobalSettings(GlobalSettings settings)
        {
            NewGlobalSettingsEvent?.Invoke(null, settings);
        }

        #endregion
    }
}
