using System;
using System.Threading.Tasks;

namespace AiServiceLabb1AndLabb2.Labb1
{
	public class Labb1Processor
	{
		private readonly LanguageService _languageService;
		private readonly QnAService _qnaService;

		public Labb1Processor(LanguageService languageService, QnAService qnaService)
		{
			_languageService = languageService;
			_qnaService = qnaService;
		}

		public async Task RunAsync()
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

				var (translatedQuestion, detectedLanguage) = await _languageService.DetectAndTranslateAsync(question);

				// Display the detected language and translated question
				Console.WriteLine($"Detected Language: {detectedLanguage}");
				Console.WriteLine($"Translated Question: {translatedQuestion}");

				// Query the QnA Maker service with the translated question and display the results
				await _qnaService.QueryQnAMakerAsync(question, translatedQuestion);

				Console.WriteLine("\nDo you want to ask another question? (y/n)");
				var continueResponse = Console.ReadLine();
				continueAsking = continueResponse?.Trim().ToLower() == "y";
			}
		}

		private void DisplayLanguageProcessingWelcomeMessage()
		{
			Console.Clear();
			Console.WriteLine("Welcome to the Apollo Space Program QnA Service!");
			Console.WriteLine("You can ask questions in any language, and the answers will be provided in English.");
		}
	}
}

