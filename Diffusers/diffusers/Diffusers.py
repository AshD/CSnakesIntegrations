import torch
from diffusers import DiffusionPipeline, FluxPipeline, FluxTransformer2DModel
import io
from PIL import Image
from transformers import T5EncoderModel, CLIPTextModel
from optimum.quanto import freeze, qfloat8, quantize

model_name =  "black-forest-labs/FLUX.1-schnell"

def InitInference(hf_model_name: str):
    model_name = hf_model_name

def generate_image_flux1(prompt: str, guidance_scale: float = 3.5, steps: int = 8) -> bytes:
    """
    Generates an image using the FLUX.1 [schnell] model.

    Args:
        prompt (str): Text prompt for image generation.
        guidance_scale (float): Controls prompt adherence.
        steps (int): Number of inference steps.

    Returns:
        PIL.Image.Image: Generated image.
    """

    pipe = DiffusionPipeline.from_pretrained(
        model_name, 
        torch_dtype=torch.bfloat16
    )
    pipe.enable_sequential_cpu_offload()
    pipe.vae.enable_slicing()
    pipe.vae.enable_tiling()

    print("Starting Diffusion")

    # Generate the image
    image = pipe(
        prompt, 
        guidance_scale=guidance_scale, 
        num_inference_steps=steps
    ).images[0]

     # Convert the image to a byte array
    with io.BytesIO() as output:
        image.save(output, format="PNG")
        return output.getvalue()

    return image_bytes

# Example usage
if __name__ == "__main__":
    prompt = "A serene forest landscape at sunrise"
    generated_image = generate_image_flux1(prompt)
    generated_image.show()
