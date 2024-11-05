using CSnakes.Runtime;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diffusers
{
    public static class DiffusersPyEnvSetup
    {
        public static IServiceCollection ConfigureDiffusersPythonEnvironment(this IServiceCollection services)
        {
            // Set up the Python environment
            var home = Path.Join(Environment.CurrentDirectory, "diffusers"); // Path to your Python modules

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
