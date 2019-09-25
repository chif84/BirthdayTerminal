using System.Runtime.Serialization;

namespace ELMA.BirthdayTerminal.Models.Elma
{
    /// <summary>
    /// Модель ответа аутентификации по логину-паролю в ELMA
    /// </summary>
    [DataContract]
    public class LoginWithResponse
    {
        [DataMember]
        public string AuthToken { get; set; }

        [DataMember]
        public long CurrentUserId { get; set; }

        [DataMember]
        public string SessionToken { get; set; }
    }
}
