// See https://aka.ms/new-console-template for more information
using ObjectStorage.Experiments;

Console.WriteLine("Hello, World!");

using (var stream = new FileStream(@"D:\temp\ddd.csv", FileMode.Open))
{
	var result = AmazonS3Service.UploadObject(stream);
}

