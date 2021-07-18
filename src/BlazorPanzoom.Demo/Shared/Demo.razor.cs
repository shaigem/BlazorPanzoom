using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace BlazorPanzoom.Demo.Shared
{
    public partial class Demo
    {
        private const string GitHubPrefix =
            "https://github.com/shaigem/BlazorPanzoom/blob/master/src/BlazorPanzoom.Demo/Pages/Demos/";

        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public string Title { get; set; }

        [Parameter] public Type DemoType { get; set; }

        [Parameter] public string SubTitle { get; set; }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            var hasDemoType = parameters.TryGetValue<Type>(nameof(DemoType), out _);
            if (!hasDemoType)
            {
                throw new ArgumentNullException(nameof(DemoType), "Must specify DemoType!");
            }

            await base.SetParametersAsync(parameters);
        }
    }
}