namespace ICOCore.Utils.Http
{
    public class HttRequestData
    {
        public string Method { get; set; }
        public string Endpoint { set; get; }
        public string ContentType { set; get; }
        public string AcceptType { set; get; }

        public string PostData { set; get; }
        public string BasicAuthenUser { set; get; }
        public string BasicAuthenPassword { set; get; }
        public string NetworkCredentialUsername { set; get; }
        public string NetworkCredentialPassword { set; get; }

        public bool IsEnableSSL { set; get; }


    }
}