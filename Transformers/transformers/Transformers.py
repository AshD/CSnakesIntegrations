## Requires manual, platform specific setup of pytorch, see https://pytorch.org/get-started/locally/
from typing import Generator
from transformers import AutoModelForCausalLM, AutoTokenizer, pipeline, TextStreamer,TextIteratorStreamer
import torch
from transformers_stream_generator import init_stream_support
from threading import Thread

model_name = "microsoft/Phi-3.5-mini-instruct"

def InitInference(hf_model_name: str):
    model_name = model_name

def Inference(user_message: str, system_message: str = "You are a helpful AI assistant.", temperature: float = 0.0) -> Generator[str, None, None]:
    model = AutoModelForCausalLM.from_pretrained( 
        model_name,  
        device_map="auto",  
        torch_dtype="auto",  
        trust_remote_code=True,  
    ) 

    tokenizer = AutoTokenizer.from_pretrained(model_name) 

    messages = [ 
        {"role": "system", "content": system_message}, 
        {"role": "user", "content": user_message}, 
    ] 

    pipe = pipeline( 
        "text-generation", 
        model=model, 
        tokenizer=tokenizer, stream=True
    ) 

    generation_args = { 
        "max_new_tokens": 2500, 
        "return_full_text": False, 
        "temperature": temperature, 
        "do_sample": False 
    } 

    formatted_prompt = tokenizer.apply_chat_template(messages, tokenize=False, add_generation_prompt=True)

    inputs = tokenizer([formatted_prompt], return_tensors="pt")

    streamer = TextIteratorStreamer(tokenizer, skip_prompt=True,skip_special_tokens=True)

    generation_kwargs = dict(inputs, streamer=streamer, max_new_tokens=1500)

    thread = Thread(target=model.generate, kwargs=generation_kwargs)

    thread.start()
    for new_text in streamer:
        yield new_text

if __name__ == "__main__":
    print("Running Inference")
    for token in Inference("Tell me a story about a brave knight."):
        print(token, end='', flush=True)
