using System;
using System.IO;
using System.Threading.Tasks;

namespace AiServiceLabb1AndLabb2.Labb2
{
	public class Labb2Processor
	{
		private readonly ImageService _imageService;

		public Labb2Processor(ImageService imageService)
		{
			_imageService = imageService;
		}

		public async Task RunAsync()
		{
			DisplayImageAnalysisWelcomeMessage();

			// Prompt the user for an image file path or URL
			Console.WriteLine("\nPlease enter the image file path or URL:");
			var imagePathOrUrl = Console.ReadLine();


			Console.WriteLine("Enter thumbnail width:");
			int thumbnailWidth = int.Parse(Console.ReadLine());
			Console.WriteLine("Enter thumbnail height:");
			int thumbnailHeight = int.Parse(Console.ReadLine());

			// Analyze the image
			await _imageService.AnalyzeImageAsync(imagePathOrUrl);


			string thumbnailPath = await _imageService.GenerateThumbnailAsync(imagePathOrUrl, thumbnailWidth, thumbnailHeight);
			Console.WriteLine($"Thumbnail generated successfully at: {thumbnailPath}");


			Console.WriteLine("Do you want to generate an image with bounding boxes? (y/n)");
			var boundingBoxResponse = Console.ReadLine();

			if (boundingBoxResponse?.Trim().ToLower() == "y")
			{
				string boundingBoxPath = await _imageService.GenerateBoundingBoxesAsync(imagePathOrUrl);
				Console.WriteLine($"Image with bounding boxes saved at: {boundingBoxPath}");
			}

			Console.WriteLine("Processing complete. Check the paths above for the results.");
		}

		private void DisplayImageAnalysisWelcomeMessage()
		{
			Console.Clear();
			Console.WriteLine("Welcome to the Image Analyzer Service!");
			Console.WriteLine("You can upload an image file or provide a URL to analyze its content.");
		}
	}
}
