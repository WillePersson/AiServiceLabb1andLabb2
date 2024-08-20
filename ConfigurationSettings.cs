using Microsoft.Extensions.Configuration;

namespace AiServiceLabb1
{
	public class ConfigurationSettings
	{
		// values of configuration settings
		public string CognitiveServicesEndpoint { get; }
		public string CognitiveServicesKey { get; }
		public string CognitiveServicesRegion { get; }
		public string QnAEndpoint { get; }
		public string QnAKey { get; }

		public ConfigurationSettings(IConfiguration config)
		{
			// Load configurations
			CognitiveServicesEndpoint = config["AzureCognitiveServices:CognitiveServicesEndpoint"];
			CognitiveServicesKey = config["AzureCognitiveServices:CognitiveServicesKey"];
			CognitiveServicesRegion = config["AzureCognitiveServices:CognetiveServiceRegion"];
			QnAEndpoint = config["AzureCognitiveServices:QnAEndpoint"];
			QnAKey = config["AzureCognitiveServices:QnAKey"];
		}
	}
}
