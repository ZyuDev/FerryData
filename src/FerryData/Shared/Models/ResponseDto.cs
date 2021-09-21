namespace FerryData.Shared.Models
{
    public class ResponseDto<T>
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
