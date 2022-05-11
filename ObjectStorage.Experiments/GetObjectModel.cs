using System.IO;

namespace ObjectStorage.Experiments
{
	public class GetObjectModel
    {
        public string ContentType { get; set; }
        public Stream Content { get; set; }
    }
}
