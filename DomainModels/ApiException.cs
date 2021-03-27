using System;

namespace DomainModels
{
    public class ApiException
    {
		public int Id { get; set; }
		public string Type { get; set; }
		public string Message { get; set; }
		public string StackTrace { get; set; }
		public string Namespace { get; set; }
		public string Classname { get; set; }
		public string Method { get; set; }
		public DateTime CreatedOn { get; set; }
	}
}
