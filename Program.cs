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

			bool continueAsking = true;

			while (continueAsking)
			{
				DisplayWelcomeMessage();

				Console.WriteLine("\nPlease enter your question (# to exit):");
				var question = Console.ReadLine();

				if (question == "#")
				{
					break; 
				}

				// Detect language and translate the question if necessary
				var (translatedQuestion, detectedLanguage) = await languageService.DetectAndTranslateAsync(question);

				// Display the detected language and translated question
				Console.WriteLine($"Detected Language: {detectedLanguage}");
				Console.WriteLine($"Translated Question: {translatedQuestion}");

				// Query the QnA Maker service with the translated question and display the results
				await qnaService.QueryQnAMakerAsync(question, translatedQuestion);

				Console.WriteLine("\nDo you want to ask another question? (y/n)");
				var continueResponse = Console.ReadLine();
				continueAsking = continueResponse?.Trim().ToLower() == "y";
			}

			Console.WriteLine("Thank you for using the Apollo Space Program QnA Service. Goodbye!");
		}

	
	    // Clears the console and displays the welcome message and instructions.
		private static void DisplayWelcomeMessage()
		{
			Console.Clear();

			Console.WriteLine("Welcome to the Apollo Space Program QnA Service!");
			Console.WriteLine("You can ask questions in any language, and the answers will be provided in English.");
		}
	}
}
