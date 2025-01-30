using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using SharedConfig.Config;
using System.Text.Json;

namespace WithSDK21
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

            IChatClient chatClient = 
                new AzureOpenAIClient(
                    new Uri(openAiOptions.Value.Endpoint), 
                    new AzureKeyCredential(openAiOptions.Value.Key))
                .AsChatClient(openAiOptions.Value.Deployment);

            List<ChatMessage> chatHistory = new()
            {
                new ChatMessage(ChatRole.System, "You are a helpful AI assistant that responds to every question like a pirate.")
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

                chatHistory.Add(new ChatMessage(ChatRole.User, userInput));

                // stream the AI response and add to chat history
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nAI response...");
                Console.ResetColor();
                var response = "";

                await foreach (var item in chatClient.CompleteStreamingAsync(chatHistory))
                {
                    response += item.Text;
                    Console.Write(item.Text);
                }

                chatHistory.Add(new ChatMessage(ChatRole.Assistant, response));

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nChat history:");
                Console.ResetColor();
                Console.Write(JsonSerializer.Serialize(chatHistory, serializerOptions));

                Console.WriteLine();
            }
        }
    }
}
