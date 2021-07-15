class BlazorPanzoomInterop {

    constructor() {
    }

    createPanzoomForReference(element, options) {
        return Panzoom(element, options)
    }

    createPanzoomForSelector(selector, options) {
        const elements = document.querySelectorAll(selector)
        const array = []
        elements.forEach((element) => array.push(Panzoom(element, options)))
        return array
    }

    registerDefaultWheelZoom(element, panzoom) {
        element.parentElement.addEventListener('wheel', panzoom.zoomWithWheel)
    }

    registerWheelListener(dotnetReference, element, panzoom) {
        element.parentElement.addEventListener('wheel', panzoom.boundWheelListener = this.wheelHandler.bind(this, dotnetReference))
    }

    wheelHandler(dotnetReference, event) {
        dotnetReference.invokeMethodAsync('OnCustomWheelEvent', {
            deltaX: event.deltaX,
            deltaY: event.deltaY,
            clientX: event.clientX,
            clientY: event.clientY,
            shiftKey: event.shiftKey,
            altKey: event.altKey
        })
    }

    removeWheelListeners(element, panzoom) {
        element.parentElement.removeEventListener('wheel', panzoom.zoomWithWheel);
        if (panzoom.boundWheelListener) {
            element.parentElement.removeEventListener('wheel', panzoom.boundWheelListener);
        }
    }
}

window.blazorPanzoom = new BlazorPanzoomInterop()