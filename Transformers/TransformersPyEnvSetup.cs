using Microsoft.Extensions.DependencyInjection;
using CSnakes.Runtime;

namespace Transformers
{
    public static class TransformersPyEnvSetup
    {
        public static IServiceCollection ConfigureTransformersPythonEnvironment(this IServiceCollection services)
        {
            // Set up the Python environment
            var home = Path.Join(Environment.CurrentDirectory, "transformers"); // Path to your Python modules

            var pythonBuilder = services
                .WithPython() // Returns a PythonEnvironmentBuilder
                .WithVirtualEnvironment(Path.Join(home, ".venv"))
                .WithHome(home)
                .FromNuGet("3.11.9");

            // Use the PipInstaller to configure the Python environment
            pythonBuilder.WithPipInstaller();

            // Return the original IServiceCollection for further chaining
            return services;
        }
    }
}
