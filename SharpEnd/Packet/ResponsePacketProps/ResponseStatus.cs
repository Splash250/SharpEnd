namespace SharpEnd.Packet
{

    public class ResponseStatus
    {
        public ResponseCode StatusObject { get; private set; }
        public int Code { get; private set; }
        public string Message { get; private set; }

        public ResponseStatus(ResponseCode Code)
        {
            this.StatusObject = Code;
            this.Code = (int)Code;
            Message = Code.ToString();
        }
    }
}
