namespace SharedConfig.Config
{
    public class OpenAiServiceConfig
    {
        public required string Deployment { get; init; }
        public required int DeploymentMaxTokens { get; init; }
        public required string Endpoint { get; init; }
        public required string Key { get; init; }
        public required string ApiVersion { get; init; }
    }
}
