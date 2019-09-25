namespace ELMA.BirthdayTerminal.Services
{
    /// <summary>
    /// Сервис работы с изображениями
    /// </summary>
    public interface IBitmapService
    {
        /// <summary>
        /// Вывести текст на изображение
        /// </summary>
        byte[] TextOut(byte[] image, string text);
    }
}
