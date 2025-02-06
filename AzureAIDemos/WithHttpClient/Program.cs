using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using SharedConfig.Config;

namespace WithHttpClient
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            JsonSerializerOptions serializerOptions = new JsonSerializerOptions { WriteIndented = true };

            var builder = new ConfigurationBuilder()
                .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true);
            var configuration = builder.Build();

            var services = new ServiceCollection()
                .AddLogging(builder =>
                {
                    builder.AddConfiguration(configuration.GetSection("Logging"));
                    builder.AddConsole();
                })
                .Configure<OpenAiServiceConfig>(options => configuration.GetSection("ServiceSettings").Bind(options))
                .BuildServiceProvider();

            var logger = (services.GetService<ILoggerFactory>() ?? throw new InvalidOperationException())
                .CreateLogger<Program>();

            // Load config from appsettings.json
            logger.LogInformation("Loading config from appsettings.json");
            var openAiOptions = services.GetRequiredService<IOptions<OpenAiServiceConfig>>();

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("api-key", openAiOptions.Value.Key);
            var requestUri = $"{openAiOptions.Value.Endpoint}/openai/deployments/{openAiOptions.Value.Deployment}/chat/completions?api-version={openAiOptions.Value.ApiVersion}";

            var messages = new[]
            {
                new { role = "system", content = "You are a helpful AI assistant that responds to every question like an 80's Valley Girl." }
            };

            logger.LogInformation("Starting chat");
            Console.WriteLine("Azure OpenAI Chat - Type 'exit' to quit.");
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("\nYour prompt: ");
                Console.ResetColor();
                string userInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(userInput)) continue;
                if (userInput.Equals("exit", StringComparison.CurrentCultureIgnoreCase)) break;

                messages = [.. messages, new { role = "user", content = userInput }];

                var content = new StringContent(JsonSerializer.Serialize(new { messages, max_tokens = openAiOptions.Value.DeploymentMaxTokens }), Encoding.UTF8, "application/json");

                // stream the AI response and add to chat history
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nAI response...");
                Console.ResetColor();

                var response = await httpClient.PostAsync(requestUri, content);
                var responseText = await response.Content.ReadAsStringAsync();

                var jsonResponse = JsonNode.Parse(responseText);
                var assistantResponse = jsonResponse?["choices"]?[0]?["message"]?["content"]?.ToString();

                if (!string.IsNullOrEmpty(assistantResponse))
                {
                    Console.WriteLine(assistantResponse);
                    messages = [.. messages, new { role = "assistant", content = assistantResponse }];

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nChat history:");
                    Console.ResetColor();
                    Console.Write(JsonSerializer.Serialize(messages, serializerOptions));
                }
                else
                {
                    Console.WriteLine("No response from AI.");
                }

                Console.WriteLine("\n--------------------------------------");
            }
        }
    }
}
