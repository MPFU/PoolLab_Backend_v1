namespace PoolLab.WebAPI.ResponseModel
{
    public class FailResponse
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public object Errors { get; set; }
    }
}
