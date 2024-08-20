using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AiServiceLabb1.Services
{
	public class QnAService
	{
		private readonly string _qnaEndpoint;
		private readonly string _qnaKey;

		public QnAService(ConfigurationSettings settings)
		{
			_qnaEndpoint = settings.QnAEndpoint;
			_qnaKey = settings.QnAKey;
		}

		public async Task QueryQnAMakerAsync(string originalQuestion, string translatedQuestion)
		{
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _qnaKey);

				// Prepare the request body for QnA Maker
				var jsonRequest = new
				{
					top = 1,
					question = translatedQuestion,
					includeUnstructuredSources = true,
					confidenceScoreThreshold = 0.0
				};

				var content = new StringContent(
					Newtonsoft.Json.JsonConvert.SerializeObject(jsonRequest),
					Encoding.UTF8,
					"application/json");

				var response = await client.PostAsync(_qnaEndpoint, content);
				var jsonResponse = await response.Content.ReadAsStringAsync();

				// Parse the response to check if a confident answer was found
				var answerResult = JObject.Parse(jsonResponse);
				var answers = answerResult["answers"];
				if (answers != null && answers.HasValues)
				{
					var topAnswer = answers[0];
					double confidenceScore = (double)topAnswer["confidenceScore"];
					string matchedQuestion = topAnswer["questions"]?[0]?.ToString();

					Console.WriteLine($"Best Matching Question: {matchedQuestion}");
					Console.WriteLine($"Answer: {topAnswer["answer"]}");
					Console.WriteLine($"Confidence Score: {confidenceScore}");
				}
				else
				{
					Console.WriteLine("I can't understand the question, please rephrase it.");
				}
			}
		}
	}
}