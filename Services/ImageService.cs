using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Azure.CognitiveServices.Knowledge.QnAMaker;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace AiServiceLabb1.Services
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
			try
			{
				var client = new ComputerVisionClient(new Microsoft.Azure.CognitiveServices.Vision.ComputerVision.ApiKeyServiceClientCredentials(_key))
				{
					Endpoint = _endpoint
				};

				var features = new List<VisualFeatureTypes?> { VisualFeatureTypes.Tags, VisualFeatureTypes.Objects };

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

				Console.WriteLine("Image Analysis:");
				foreach (var tag in analysis.Tags)
				{
					Console.WriteLine($"Tag: {tag.Name}, Confidence: {tag.Confidence}");
				}
			}
			catch (ComputerVisionErrorResponseException ex)
			{
				Console.WriteLine($"Error analyzing image: {ex.Message}");
				Console.WriteLine($"Error details: {ex.Response.Content}");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An unexpected error occurred: {ex.Message}");
			}
		}


		public async Task GenerateThumbnailAsync(string imagePathOrUrl, int width, int height)
		{
			var client = new ComputerVisionClient(new Microsoft.Azure.CognitiveServices.Vision.ComputerVision.ApiKeyServiceClientCredentials(_key))
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

			using (var fileStream = new FileStream("thumbnail.jpg", FileMode.Create, FileAccess.Write))
			{
				thumbnailStream.CopyTo(fileStream);
			}

			Console.WriteLine("Thumbnail generated successfully.");
		}

		public async Task GenerateBoundingBoxesAsync(string imagePathOrUrl)
		{
			var client = new ComputerVisionClient(new Microsoft.Azure.CognitiveServices.Vision.ComputerVision.ApiKeyServiceClientCredentials(_key))
			{
				Endpoint = _endpoint
			};

			ImageAnalysis analysis;
			if (Uri.IsWellFormedUriString(imagePathOrUrl, UriKind.Absolute))
			{
				analysis = await client.AnalyzeImageAsync(imagePathOrUrl, new List<VisualFeatureTypes?> { VisualFeatureTypes.Objects });
			}
			else
			{
				using (var imageStream = File.OpenRead(imagePathOrUrl))
				{
					analysis = await client.AnalyzeImageInStreamAsync(imageStream, new List<VisualFeatureTypes?> { VisualFeatureTypes.Objects });
				}
			}

			// Download image if it's a URL
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

				bitmap.Save("bounding_boxes.jpg");
			}

			// Clean up the temporary file if it was downloaded
			if (Uri.IsWellFormedUriString(imagePathOrUrl, UriKind.Absolute) && File.Exists(localImagePath))
			{
				File.Delete(localImagePath);
			}

			Console.WriteLine("Image with bounding boxes saved as bounding_boxes.jpg.");
		}
	}
}
