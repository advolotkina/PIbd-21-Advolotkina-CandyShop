using System.Runtime.Serialization;

namespace CandyShopView
{
    [DataContract]
    class HttpErrorMessage
    {
        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public string ExceptionMessage { get; set; }

        [DataMember]
        public string MessageDetail { get; set; }
    }
}
