from dotenv import load_dotenv
from colors import Colors
import requests
import os
import json

conversation_history = []

load_dotenv()
MODEL=os.getenv("MODEL")
API_KEY=os.getenv("ANTHROPIC_API_KEY")
API_ENDPOINT="https://api.anthropic.com/v1/messages"
MAX_TOKENS=int(os.getenv("MAX_TOKENS"))
SYSTEM_PROMPT="You are a helpful AI assistant that responds to every question like a politician."

data = {
    "model": MODEL,
    "max_tokens": MAX_TOKENS,
    "system": SYSTEM_PROMPT,
    "messages": conversation_history
}

headers = {
    "x-api-key": API_KEY,
    "anthropic-version": "2023-06-01",
    "Content-Type": "application/json"
}

def clear_screen():
    if os.name == "nt":
        os.system("cls")
    else:
        os.system("clear")

def create_message(role: str, content: str):
    return {"role": role, "content": content}

clear_screen()
while True:
    user_input = input(Colors.prompt("\nUser prompt: "))
    
    if user_input.lower() == "exit":
        print (Colors.prompt("End of conversation"))
        break

    conversation_history.append(create_message("user", user_input))

    response = requests.post(API_ENDPOINT, json=data, headers=headers)

    if response.status_code == 200:
        response_json = json.loads(response.content.decode("utf-8"))
        assistant_response = response_json["content"][0]["text"]

        print (Colors.prompt("\nAssistant response:"))
        print(Colors.response(assistant_response))

        conversation_history.append(create_message("assistant", assistant_response))
    else:
        print (Colors.error(f"\nError code: {response.status_code}, text: {response.text} ", ))

print("end")