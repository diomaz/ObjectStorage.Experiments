using System;
using System.IO;
using System.Threading.Tasks;

using Amazon.S3;
using Amazon.S3.Model;

using Microsoft.AspNetCore.StaticFiles;

namespace ObjectStorage.Experiments
{
	public static class AmazonS3Service
	{
		private static string accessKey = "key";
		private static string accessSecret = "secret";
		private static string bucket = "bucket";

		public static async Task<GetObjectModel> GetObject(string id)
		{
			var client = new AmazonS3Client(accessKey, accessSecret, Amazon.RegionEndpoint.EUCentral1);

			GetObjectRequest request = new GetObjectRequest
			{
				BucketName = bucket,
				Key = id
			};

			var responseObject = await client.GetObjectAsync(request);

			return new GetObjectModel
			{
				ContentType = responseObject.Headers.ContentType,
				Content = responseObject.ResponseStream
			};
		}

		public static async Task<UploadObjectModel> UploadObject(FileStream file)
		{
			// connecting to thec client
			var client = new AmazonS3Client(accessKey, accessSecret, Amazon.RegionEndpoint.EUCentral1);

			byte[] fileBytes = new Byte[file.Length];
			file.Read(fileBytes, 0, Int32.Parse(file.Length.ToString()));

			var fileName = Guid.NewGuid().ToString();
			var mimeProvider = new FileExtensionContentTypeProvider();

			if (!mimeProvider.TryGetContentType(file.Name, out string contentType))
			{
				contentType = "application/octet-stream";
			}

			PutObjectResponse response = null;

			using (var stream = new MemoryStream(fileBytes))
			{
				var request = new PutObjectRequest
				{
					BucketName = bucket,
					Key = fileName,
					InputStream = stream,
					ContentType = contentType,
					CannedACL = S3CannedACL.PublicRead
				};

				try
				{
					response = await client.PutObjectAsync(request);
				}
				catch (Exception ex)
				{
				}
				finally
				{

				}
			};

			return new UploadObjectModel
			{
				Success = response.HttpStatusCode == System.Net.HttpStatusCode.OK,
				FileName = fileName
			};
		}

		public static async Task<UploadObjectModel> RemoveObject(String fileName)
		{
			var client = new AmazonS3Client(accessKey, accessSecret, Amazon.RegionEndpoint.EUCentral1);

			var request = new DeleteObjectRequest
			{
				BucketName = bucket,
				Key = fileName
			};

			var response = await client.DeleteObjectAsync(request);

			if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
			{
				return new UploadObjectModel
				{
					Success = true,
					FileName = fileName
				};
			}
			else
			{
				return new UploadObjectModel
				{
					Success = false,
					FileName = fileName
				};
			}
		}
	}
}
