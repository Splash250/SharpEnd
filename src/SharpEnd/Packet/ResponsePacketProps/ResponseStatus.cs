namespace SharpEnd.Packet
{

    public class ResponseStatus
    {
        public ResponseCode StatusObject { get; private set; }
        public int Code { get; private set; }
        public string Message { get; private set; }

        public ResponseStatus(ResponseCode code)
        {
            StatusObject = code;
            Code = (int)code;
            Message = code.ToString();
        }
    }
}
