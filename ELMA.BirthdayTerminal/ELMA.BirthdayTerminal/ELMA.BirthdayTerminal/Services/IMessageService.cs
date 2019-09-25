namespace ELMA.BirthdayTerminal.Services
{
    /// <summary>
    /// Сервис вывода сообщений
    /// </summary>
    public interface IMessageService
    {
        /// <summary>
        /// Долгое сообщение
        /// </summary>
        void LongAlert(string message);

        /// <summary>
        /// Короткое сообщение
        /// </summary>
        void ShortAlert(string message);
    }
}
