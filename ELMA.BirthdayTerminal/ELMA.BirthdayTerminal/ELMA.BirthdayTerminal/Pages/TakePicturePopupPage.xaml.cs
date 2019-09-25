using ELMA.BirthdayTerminal.Services;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms;

namespace ELMA.BirthdayTerminal
{
    /// <summary>
    /// Попап выбора текста для поздравления
    /// </summary>
    public partial class TakePicturePopupPage : PopupPage
    {
        private ILocalStorageService localStorageService;
        private ISecurityService securityService;

        /// <summary>
        /// Ctor
        /// </summary>
        public TakePicturePopupPage()
		{
			InitializeComponent();

            localStorageService = DependencyService.Get<ILocalStorageService>();
            securityService = DependencyService.Get<ISecurityService>();
        }

        /// <summary>
        /// Ctor
        /// </summary>
        public TakePicturePopupPage(string text): this()
        {
            outputText.Text = text;
        }

        /// <summary>
        /// При выборе шаблонного текста
        /// </summary>
        private async void OnSetText(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                outputText.Text = btn.Text;
            }
        }

        /// <summary>
        /// Подтверждение выбранного текста
        /// </summary>
        private async void OnOk(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                TakePicturePage.OnSetText(outputText.Text);
            });

            CloseAllPopup();
        }

        /// <summary>
        /// Отмена выбора текста
        /// </summary>
        protected override bool OnBackgroundClicked()
        {
            CloseAllPopup();

            return false;
        }

        /// <summary>
        /// Закрытие попапа
        /// </summary>
        private async void CloseAllPopup()
        {
            await PopupNavigation.Instance.PopAllAsync();
        }
    }
}