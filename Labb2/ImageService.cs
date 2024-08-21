using AiServiceLabb1;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace AiServiceLabb1AndLabb2.Labb2
{
	public class ImageService
	{
		private readonly string _endpoint;
		private readonly string _key;

		public ImageService(ConfigurationSettings settings)
		{
			_endpoint = settings.CognitiveServicesEndpoint;
			_key = settings.CognitiveServicesKey;
		}

		public async Task AnalyzeImageAsync(string imagePathOrUrl)
		{
			var client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(_key))
			{
				Endpoint = _endpoint
			};

			var features = new List<VisualFeatureTypes?> { VisualFeatureTypes.Tags, VisualFeatureTypes.Objects };

			ImageAnalysis analysis;

			// Analyze the image based on whether it's a URL or a local file
			if (Uri.IsWellFormedUriString(imagePathOrUrl, UriKind.Absolute))
			{
				analysis = await client.AnalyzeImageAsync(imagePathOrUrl, features);
			}
			else
			{
				using (var imageStream = File.OpenRead(imagePathOrUrl))
				{
					analysis = await client.AnalyzeImageInStreamAsync(imageStream, features);
				}
			}

			Console.WriteLine("Image Analysis:");
			foreach (var tag in analysis.Tags)
			{
				Console.WriteLine($"Tag: {tag.Name}, Confidence: {tag.Confidence}");
			}
		}

		// Method to generate a thumbnail of the specified image
		public async Task<string> GenerateThumbnailAsync(string imagePathOrUrl, int width, int height)
		{
			var client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(_key))
			{
				Endpoint = _endpoint
			};

			Stream thumbnailStream;
			if (Uri.IsWellFormedUriString(imagePathOrUrl, UriKind.Absolute))
			{
				thumbnailStream = await client.GenerateThumbnailAsync(width, height, imagePathOrUrl);
			}
			else
			{
				using (var imageStream = File.OpenRead(imagePathOrUrl))
				{
					thumbnailStream = await client.GenerateThumbnailInStreamAsync(width, height, imageStream);
				}
			}

			string thumbnailPath = Path.Combine(Directory.GetCurrentDirectory(), "thumbnail.jpg");
			using (var fileStream = new FileStream(thumbnailPath, FileMode.Create, FileAccess.Write))
			{
				thumbnailStream.CopyTo(fileStream);
			}

			return thumbnailPath;
		}

		// Method to generate an image with bounding boxes around detected objects

		public async Task<string> GenerateBoundingBoxesAsync(string imagePathOrUrl)
		{
			var client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(_key))
			{
				Endpoint = _endpoint
			};

			var features = new List<VisualFeatureTypes?> { VisualFeatureTypes.Objects };

			ImageAnalysis analysis;
			if (Uri.IsWellFormedUriString(imagePathOrUrl, UriKind.Absolute))
			{
				analysis = await client.AnalyzeImageAsync(imagePathOrUrl, features);
			}
			else
			{
				using (var imageStream = File.OpenRead(imagePathOrUrl))
				{
					analysis = await client.AnalyzeImageInStreamAsync(imageStream, features);
				}
			}

			string localImagePath = imagePathOrUrl;
			if (Uri.IsWellFormedUriString(imagePathOrUrl, UriKind.Absolute))
			{
				using (var httpClient = new HttpClient())
				{
					var imageBytes = await httpClient.GetByteArrayAsync(imagePathOrUrl);
					localImagePath = Path.Combine(Path.GetTempPath(), "temp_image.jpg");
					await File.WriteAllBytesAsync(localImagePath, imageBytes);
				}
			}

			// Define the path where the image with bounding boxes will be saved
			string boundingBoxPath = Path.Combine(Directory.GetCurrentDirectory(), "bounding_boxes.jpg");

			using (var bitmap = new Bitmap(localImagePath))
			{
				using (var graphics = Graphics.FromImage(bitmap))
				{
					foreach (var detectedObject in analysis.Objects)
					{
						var rect = new Rectangle(detectedObject.Rectangle.X, detectedObject.Rectangle.Y, detectedObject.Rectangle.W, detectedObject.Rectangle.H);
						graphics.DrawRectangle(Pens.Red, rect);
						graphics.DrawString(detectedObject.ObjectProperty, new Font("Arial", 12), Brushes.Red, rect.Location);
					}
				}

				bitmap.Save(boundingBoxPath);
			}

			if (Uri.IsWellFormedUriString(imagePathOrUrl, UriKind.Absolute) && File.Exists(localImagePath))
			{
				File.Delete(localImagePath);
			}

			return boundingBoxPath;
		}
	}
}

