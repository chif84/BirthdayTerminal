namespace ELMA.BirthdayTerminal.Services
{
    /// <summary>
    /// Сервис соединения с ELMA
    /// </summary>
    public interface IConnectionService
    {
        /// <summary>
        /// Отправить Post-запрос
        /// </summary>
        string SendPost(string path, string content, string authToken = null);

        /// <summary>
        /// Отправить Get-запрос
        /// </summary>
        string SendGet(string path, string authToken);

        /// <summary>
        /// Отправить Get-запрос
        /// </summary>
        byte[] SendGet2(string path, string authToken);
    }
}
