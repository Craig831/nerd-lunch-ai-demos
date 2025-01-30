from dotenv import load_dotenv
from anthropic import Anthropic
from colors import Colors
import os

conversation_history = []

load_dotenv()

MODEL=os.getenv("MODEL")
MAX_TOKENS=int(os.getenv("MAX_TOKENS"))
SYSTEM_PROMPT="You are a helpful AI assistant that responds to every question like a Marine Corps drill sergeant."

client = Anthropic()  # as long as my env variable is named ANTHROPIC_API_KEY, I don't need to pass it here

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
        print ("End of conversation")
        break

    conversation_history.append(create_message("user", user_input))

    response = client.messages.create(
        model=MODEL,
        system=SYSTEM_PROMPT,
        messages=conversation_history,
        max_tokens=MAX_TOKENS
    )

    assistant_response = response.content[0].text

    print (Colors.prompt("\nAssistant response:"))
    print(Colors.response(assistant_response))
    
    conversation_history.append(create_message("assistant", assistant_response))

print("end")