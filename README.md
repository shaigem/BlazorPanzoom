# BlazorPanzoom

BlazorPanzoom is a library for Blazor that wraps around timmywil's JavaScript
library, [panzoom](https://github.com/timmywil/panzoom). It provides an easy way to enable panning and zooming of web
components/elements through CSS transformations.

**Currently targeting:** [panzoom v4.4.1](https://github.com/timmywil/panzoom/releases/tag/4.4.1)

## Demo Showcase
**[View Demo App](https://shaigem.github.io/BlazorPanzoom/)**

<img src="https://github.com/shaigem/BlazorPanzoom/blob/e8ed0b3240efa32902e1f425d7615792e3abb05d/docs/demo_app_main.png" alt="main demo app" width="70%"/>

The demo app aims to implement all of the same examples from timmywil's panzoom'
s [demo page](https://timmywil.com/panzoom/demo/).

The list below shows which examples have been implemented.

#### Demo Example List (4/10):

- [x] [Panning and zooming](https://shaigem.github.io/BlazorPanzoom/)
- [x] [Panning and focal-point zooming (shift + mousewheel)]
- [ ] SVG support: move all the things!
- [x] [Containment within the parent: contain = inside](https://shaigem.github.io/BlazorPanzoom/inside)
- [x] [Containment within the parent: contain = outside](https://shaigem.github.io/BlazorPanzoom/within)
- [ ] Exclude elements within the Panzoom element from event handling
- [ ] Disabling one axis
- [ ] Adding on matrix functions to the transform
- [ ] Adding a transform to a child
- [ ] A Panzoom instance within another Panzoom instance

## Installation

Will be published soon on Nuget...

## API Documentation

Please see panzoom's [README #doc] for more documentation on the API.

## Usage

### Simple Example

Wrap the element that you want to enable panning and zooming for with `<Panzoom>`. For example, I want
to make an image panzoomable:

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

This example enables panning for the `<img>` component via mouse input. Zooming is not enabled by default.

#### Zoom via Mouse Wheel

To enable zooming of an element through the mouse wheel, pass `WheelHandler="@WheelHandler.ZoomOnScroll"` to
the `<Panzoom>`
component.

```html
<div class="my-main" style="border-style: solid;">
    <Panzoom WheelHandler="WheelHandler.ZoomOnScroll">
        <img @ref="@context.ElementReference" src="https://homepages.cae.wisc.edu/~ece533/images/pool.png" alt="image"/>
    </Panzoom>
</div>
```
#### Custom Zooming: Zoom on Shift Key + Mouse Scroll

The default `WheelHandler.ZoomOnScroll` is great for quickly getting an element to be zoom-able. However, what if you
want to implement custom behavior? For example, I only want to zoom when holding the `Shift` key.

Steps to implementing a custom scroll listener:
1. Set `WheelHandler` of a `<Panzoom>` component to `Custom` 
2. Create a Task function (example: `private async Task OnWheel(PanzoomWheelEventArgs args)`)
3. Set `OnWheel` of a `<Panzoom>` component to the new Task function

```c#
<div class="my-main" style="border-style: solid;">
    <Panzoom @ref="_panzoom" WheelHandler="WheelHandler.Custom" OnWheel="@OnWheel">
        <img @ref="@context.ElementReference" src="https://homepages.cae.wisc.edu/~ece533/images/pool.png" alt="image"/>
    </Panzoom>
</div>

@code {

    private Panzoom _panzoom;
        
    private async Task OnWheel(PanzoomWheelEventArgs args)
    {
        if (!args.ShiftKey)
        {
            return;
        }
        await _panzoom.ZoomWithWheel(args);
    }
}
```
The `<panzoom>.ZoomWithWheel(PanzoomWheelEventArgs)` function handles zooming via mouse wheel scroll.

See [FocalDemo.razor] and its corresponding demo [Panning and focal-point zooming (shift + mousewheel)] for a similar example to this one.

Our simple example image is now pannable and zoomable:

![A simple example image](https://github.com/shaigem/BlazorPanzoom/blob/f653196e926a8d661bb3a2507e331353c25028b2/docs/example_simple.png)
* * *
### Passing Options via `PanzoomOptions`

The `<Panzoom>` component allows you to set its options with the `PanzoomOptions` object.

#### Example: Disable panning with the `DisablePan` option

Through the component:
```html
<Panzoom PanzoomOptions=@(new PanzoomOptions{DisablePan = true})"></Panzoom>
```

Through the object (see [StandardDemo.razor]):
```c#
private Panzoom _panzoom;
// ...
_panzoom.SetOptionsAsync(new () {DisablePan = true});
```

**Note:** not all options are available. 
Please see issue [#3](https://github.com/shaigem/BlazorPanzoom/issues/3#issue-941365085) for the list of available options.

View panzoom's [README #doc] for the list of all options and their usage.

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License

[MIT](https://choosealicense.com/licenses/mit/)

[README #doc]: https://github.com/timmywil/panzoom/blob/39524b1ec721e5f7cabcabc4d7e467968dffe778/README.md#documentation
[Panning and focal-point zooming (shift + mousewheel)]: https://shaigem.github.io/BlazorPanzoom/focal/
[FocalDemo.razor]: https://github.com/shaigem/BlazorPanzoom/blob/49e7f72bb4fe3bc247dc80117858232ceef82b4e/src/BlazorPanzoom.Demo/Pages/Demos/FocalDemo.razor
[StandardDemo.razor]: https://github.com/shaigem/BlazorPanzoom/blob/49e7f72bb4fe3bc247dc80117858232ceef82b4e/src/BlazorPanzoom.Demo/Pages/Demos/StandardDemo.razor