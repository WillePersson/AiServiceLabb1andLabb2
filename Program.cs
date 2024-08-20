using AiServiceLabb1.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AiServiceLabb1
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			// Load configuration settings from appsettings.json
			var config = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.Build();

			var settings = new ConfigurationSettings(config);
			var languageService = new LanguageService(settings);
			var qnaService = new QnAService(settings);
			var imageService = new ImageService(settings);

			bool continueUsingApp = true;

			while (continueUsingApp)
			{
				Console.Clear();
				Console.WriteLine("Welcome to the Multi-Function AI Service!");
				Console.WriteLine("Please choose an option:");
				Console.WriteLine("1. Language Processing (Labb 1)");
				Console.WriteLine("2. Image Analysis (Labb 2)");
				Console.WriteLine("3. Exit");

				var choice = Console.ReadLine();

				switch (choice)
				{
					case "1":
						await RunLanguageProcessingAsync(languageService, qnaService);
						break;
					case "2":
						await RunImageAnalysisAsync(imageService);
						break;
					case "3":
						continueUsingApp = false;
						break;
					default:
						Console.WriteLine("Invalid choice. Please select 1, 2, or 3.");
						break;
				}
			}

			Console.WriteLine("Thank you for using the Multi-Function AI Service. Goodbye!");
		}

		private static async Task RunLanguageProcessingAsync(LanguageService languageService, QnAService qnaService)
		{
			bool continueAsking = true;

			while (continueAsking)
			{
				
				DisplayLanguageProcessingWelcomeMessage();

				
				Console.WriteLine("\nPlease enter your question (# to exit):");
				var question = Console.ReadLine();

				if (question == "#")
				{
					break; 
				}

				var (translatedQuestion, detectedLanguage) = await languageService.DetectAndTranslateAsync(question);

				Console.WriteLine($"Detected Language: {detectedLanguage}");
				Console.WriteLine($"Translated Question: {translatedQuestion}");

				await qnaService.QueryQnAMakerAsync(question, translatedQuestion);

				Console.WriteLine("\nDo you want to ask another question? (y/n)");
				var continueResponse = Console.ReadLine();
				continueAsking = continueResponse?.Trim().ToLower() == "y";
			}
		}

		private static async Task RunImageAnalysisAsync(ImageService imageService)
		{
			DisplayImageAnalysisWelcomeMessage();

			// Prompt the user for an image file path or URL
			Console.WriteLine("\nPlease enter the image file path or URL:");
			var imagePathOrUrl = Console.ReadLine();

			Console.WriteLine("Enter thumbnail width:");
			int thumbnailWidth = int.Parse(Console.ReadLine());
			Console.WriteLine("Enter thumbnail height:");
			int thumbnailHeight = int.Parse(Console.ReadLine());

			await imageService.AnalyzeImageAsync(imagePathOrUrl);

			await imageService.GenerateThumbnailAsync(imagePathOrUrl, thumbnailWidth, thumbnailHeight);

			Console.WriteLine("Do you want to generate an image with bounding boxes? (y/n)");
			var boundingBoxResponse = Console.ReadLine();

			if (boundingBoxResponse?.Trim().ToLower() == "y")
			{
				await imageService.GenerateBoundingBoxesAsync(imagePathOrUrl);
			}

			Console.WriteLine("Processing complete. Check your output folder for the results.");
		}

		private static void DisplayLanguageProcessingWelcomeMessage()
		{
			Console.Clear();
			Console.WriteLine("Welcome to the Apollo Space Program QnA Service!");
			Console.WriteLine("You can ask questions in any language, and the answers will be provided in English.");
		}

		private static void DisplayImageAnalysisWelcomeMessage()
		{
			Console.Clear();
			Console.WriteLine("Welcome to the Image Analyzer Service!");
			Console.WriteLine("You can upload an image file or provide a URL to analyze its content.");
		}
	}
}