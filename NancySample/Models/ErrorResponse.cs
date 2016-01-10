using Nancy;

namespace NancySample.Models
{
    public class ErrorResponse
    {
        public string Title;
        public string Summary;
        public string Details;
        public bool ShowDetails;

        public NancyContext Context;
    }
}