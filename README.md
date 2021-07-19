# BlazorPanzoom
[![Nuget version](https://img.shields.io/nuget/v/BlazorPanzoom)](https://www.nuget.org/packages/BlazorPanzoom/)

BlazorPanzoom is a library for Blazor that wraps around timmywil's JavaScript
library, [panzoom](https://github.com/timmywil/panzoom). It provides an easy way to enable panning and zooming of web
components/elements through CSS transformations.

**Currently targeting:** [panzoom v4.4.1](https://github.com/timmywil/panzoom/releases/tag/4.4.1)

## Demo Showcase
**[View Demo App](https://shaigem.github.io/BlazorPanzoom/)**

The demo app aims to implement all of the same examples from timmywil's panzoom'
s [demo page](https://timmywil.com/panzoom/demo/).

The list below shows which examples have been implemented.

#### Demo List

- [x] [Panning and zooming](https://shaigem.github.io/BlazorPanzoom/)
- [x] [Panning and focal-point zooming (shift + mousewheel)]
- [x] [SVG support: move all the things!](https://shaigem.github.io/BlazorPanzoom/svg)
- [x] [Containment within the parent: contain = inside](https://shaigem.github.io/BlazorPanzoom/inside)
- [x] [Containment within the parent: contain = outside](https://shaigem.github.io/BlazorPanzoom/outside)
- [x] [Exclude elements within the Panzoom element from event handling](https://shaigem.github.io/BlazorPanzoom/exclude)
- [x] [Disabling one axis](https://shaigem.github.io/BlazorPanzoom/oneaxis)
- [x] [Adding on matrix functions to the transform](https://shaigem.github.io/BlazorPanzoom/transform)
- [x] [Adding a transform to a child](https://shaigem.github.io/BlazorPanzoom/transform)
- [x] [A Panzoom instance within another Panzoom instance](https://shaigem.github.io/BlazorPanzoom/inception)

## Prerequisites
- [.NET 5.0](https://dotnet.microsoft.com/download/dotnet/5.0)

## Installation

Install the NuGet package:

```
dotnet add package BlazorPanzoom
```

Add the following to `_Imports.razor`
```razor
@using BlazorPanzoom
```
Add the following to `index.html` (client-side) or `_Host.cshtml` (server-side) in `body`
```razor
<script src="_content/BlazorPanzoom/panzoom.min.js"></script>
<script src="_content/BlazorPanzoom/blazorpanzoom.js"></script>
```
#### Client-Side Config (WebAssembly)
In `Program.cs`...

Import the following
```c#
using BlazorPanzoom;
```
And then add the following to where you register your services
```c#
builder.Services.AddBlazorPanzoomServices();
```
#### Server-Side Config
Import the following
```c#
using BlazorPanzoom;
```
In `Startup.cs`...

Import the following
```c#
using BlazorPanzoom;
```
And then add the following to where you register your services
```c#
builder.Services.AddBlazorPanzoomServices();
```
## Usage

### Simple Example

Wrap the element that you want to enable panning and zooming for with `<Panzoom>`:

```html
<!-- Using solid border-style to highlight the image's panning & zooming boundary -->
<div class="my-main" style="border-style: solid;">
    <Panzoom>
        <!-- Must set the element's reference (@ref)! -->
        <img @ref="@context.ElementReference" src="https://homepages.cae.wisc.edu/~ece533/images/pool.png" alt="image"/>
    </Panzoom>
</div>
```
**Note:** you must use `@ref="@context.ElementReference"` on the element that you want to enable panning and zooming
for!

This example enables panning for the `<img>` component via mouse input. Zooming is not enabled by default on Desktop.

#### Zoom via Mouse Wheel

To enable zooming of an element through the mouse wheel, pass `WheelMode="@WheelMode.ZoomWithWheel"` to
the `<Panzoom>`
component.

```html
<div class="my-main" style="border-style: solid;">
    <Panzoom WheelMode="WheelMode.ZoomWithWheel">
        <img @ref="@context.ElementReference" src="https://homepages.cae.wisc.edu/~ece533/images/pool.png" alt="image"/>
    </Panzoom>
</div>
```
***
### Full Example
[FocalDemo.razor] ([Live Demo][Panning and focal-point zooming (shift + mousewheel)])

Demonstrates:
- Registering an element to use `Panzoom`
  - Setting options via `PanzoomOptions`
    
See issue [#3](https://github.com/shaigem/BlazorPanzoom/issues/3#issue-941365085) for a list of supported options. For documentation
    on what each option does, see [panzoom #doc][README #doc].

- Calling `Panzoom` functions from code (`ResetAsync` & `ZoomWithWheelAsync`)
- Custom zoom wheel handling (zoom only while holding the `Shift` key)
```razor
    <div class="buttons">
        <button @onclick="OnResetClick">Reset</button>
    </div>

    <div class="panzoom-parent" style="border-width: 10px;">
        <!-- We wrap the <img> element with the Panzoom control -->
        <Panzoom @ref="_panzoom" PanzoomOptions="@(new PanzoomOptions {Canvas = true})" WheelMode="@WheelMode.Custom" OnWheel="@OnWheel">
            <!-- MUST set @ref="@context.ElementReference" to the <Panzoom>'s context -->
            <div @ref="@context.ElementReference" class="panzoom" style="width: 400px; height: 400px; margin: 0 auto;">
                <img style="width: 100%; height: 100%;" src="target.png" alt="image"/>
            </div>
        </Panzoom>
    </div>
    
 @code {

    private Panzoom _panzoom;

    private async Task OnResetClick(MouseEventArgs args) => await _panzoom.ResetAsync();

    private async Task OnWheel(CustomWheelEventArgs args)
    {
        if (!args.ShiftKey)
        {
            return;
        }
        await _panzoom.ZoomWithWheelAsync(args);
    }

}
```

For more examples, see the [Demos](https://github.com/shaigem/BlazorPanzoom/tree/master/src/BlazorPanzoom.Demo/Pages/Demos) folder.

## Documentation

Please see the [wiki](https://github.com/shaigem/BlazorPanzoom/wiki) for documentation and help.

For documentation on the panzoom JavaScript API, click [here][README #doc].


## License

[MIT](https://choosealicense.com/licenses/mit/)

[FocalDemo.razor]: src/BlazorPanzoom.Demo/Pages/Demos/FocalDemo.razor
[README #doc]: https://github.com/timmywil/panzoom/blob/39524b1ec721e5f7cabcabc4d7e467968dffe778/README.md#documentation
[Panning and focal-point zooming (shift + mousewheel)]: https://shaigem.github.io/BlazorPanzoom/focal/