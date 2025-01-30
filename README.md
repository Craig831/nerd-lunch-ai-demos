# nerd-lunch-ai-demos

## AnthropicAIdemos
Folder contains two python scripts that provide a Chatbot with conversation history.

- .env: contains configuration settings to connect to anthropic endpoints
- color.py: contains definitions and static methods to colorize console messages
- chatbot_http.py: chatbot that uses the python requests library to handle request and responses
- chatbot_sdk.py: chatbot that uses the Anthropic SDK to handle request and responses

## AzureAIDemos
Folder contains a .Net 9 solution that contains 3 projects

- SharedConfig: contains an OpenAiServiceConfig class that both solutions leverage for loading application settings from appSettings.json
- WithHttpClient: project that communicates with deployed Azure Open AI resources using WithHttpClient
- WithSDK21: project that communicates with deployed Azure Open AI resources using the Azure.AI.OpenAI library