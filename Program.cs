using AiServiceLabb1;
using AiServiceLabb1AndLabb2.Labb1;
using AiServiceLabb1AndLabb2.Labb2;
using Microsoft.Extensions.Configuration;

namespace AiServiceLabb1AndLabb2
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			var config = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.Build();

			var settings = new ConfigurationSettings(config);

			bool continueUsingApp = true;

			while (continueUsingApp)
			{
				Console.Clear();
				Console.WriteLine("Welcome to the Multi-Function AI Service!");
				Console.WriteLine("Please choose an option:");
				Console.WriteLine("1. Natural Language Processing och frågetjänster i Azure AI (Labb 1)");
				Console.WriteLine("2. Bildtjänster i Azure AI (Labb 2)");
				Console.WriteLine("3. Exit");

				var choice = Console.ReadLine();

				switch (choice)
				{
					case "1":
						var languageService = new LanguageService(settings);
						var qnaService = new QnAService(settings);
						var labb1Processor = new Labb1Processor(languageService, qnaService);
						await labb1Processor.RunAsync();
						break;
					case "2":
						var imageService = new ImageService(settings);
						var labb2Processor = new Labb2Processor(imageService);
						await labb2Processor.RunAsync();
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
	}
}
