using CSnakes.Runtime;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Formats.Png;
using System.IO;
//using Diffusers;
using Transformers;
using Diffusers;

namespace CSnakesIntegrations
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Testing Hugging Face Diffusers and Transformers in .NET using CSnakes!");

            var builder = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    // TODO - Handle multiple python envs
                    services.ConfigureDiffusersPythonEnvironment();
                    //services.ConfigureTransformersPythonEnvironment();
                });

            var app = builder.Build();

            //BUG - This returns only the last python env in all the elements of the array
            // var envs = app.Services.GetServices<IPythonEnvironment>().ToList();

            var env = app.Services.GetRequiredService<IPythonEnvironment>();

            try
            {
                var Diffusers = env.Diffusers();

                var byteArray = Diffusers.GenerateImageFlux1("Joy Amidst the downpour, digital animation, anime style, young male character, heavy rain, side profile, open mouth, upward gaze, smiling, dark hair, casual clothing, raindrops, cloudy skies, sense of wonder, refreshment, atmospheric mood, cool tones, overcast lighting, natural element, emotional expression, character close-up");

                using (MemoryStream ms = new MemoryStream(byteArray))
                {
                    var img = Image.Load<Rgba32>(ms);
                    img.Save("test.png");

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            //try
            //{
            //    ITransformers Transformers = env.Transformers();

            //    foreach (var str in Transformers.Inference("What is capital of France?"))
            //    {
            //        Console.Write($"{str}");
            //    }
            //}
            //catch (Exception ex) 
            //{
            //    Console.WriteLine(ex);
            //}

        }
    }
}
