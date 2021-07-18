using System;
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
    }
}