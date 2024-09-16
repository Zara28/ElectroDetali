namespace ElectroDetali.Models.HelperModels
{
    public class ResponseModel<T>
    {
        public T Value{ get; set; }
        public string ErrorMessage { get; set; }
    }
}
