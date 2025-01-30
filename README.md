# nerd-lunch-ai-demos

## Anthropic (/AnthropicAIDemos)
**REQUIRED:** to use these demo scripts, you need to sign up for an [Anthropic](https://www.anthropic.com/) account to get an API KEY. Once you have an account, you can copy the .env.example file to .env and change settings to match your own account to run locally.  

Folder contains python scripts that provide a Chatbot with conversation history.

- ```.env.example```: contains configuration settings to connect to anthropic endpoints
- ```color.py```: contains definitions and static methods to colorize console messages
- ```chatbot_http.py```: chatbot that uses the python requests library to handle request and responses
- ```chatbot_sdk.py```: chatbot that uses the Anthropic SDK to handle request and responses

## Azure (/AzureAIDemos)
**REQUIRED:** to use this demo solution, you need to provision an Azure OpenAI resource in your own [Azure](https://portal.azure.com) tenant and deploy a model for your resource to use. Once your resource and model are deployed, copy the appSettings.example.json file to appSettings.json and change settings to match your own resources to run locally.  

Folder contains a .Net 9 solution that contains projects that provide a Chatbot with conversation history

- ```SharedConfig```: contains an OpenAiServiceConfig class that both solutions leverage for loading application settings from appSettings.json
- ```WithHttpClient```: project that communicates with deployed Azure Open AI resources using WithHttpClient
- ```WithSDK21```: project that communicates with deployed Azure Open AI resources using the Azure.AI.OpenAI library