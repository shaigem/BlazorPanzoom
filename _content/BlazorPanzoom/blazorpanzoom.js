class BlazorPanzoomInterop {

    constructor() {
        this.boundWheelListener = null;
    }

    createPanzoomForReference(element, options) {
        return Panzoom(element, options)
    }

    registerDefaultWheelZoom(element, panzoom) {
        element.parentElement.addEventListener('wheel', panzoom.zoomWithWheel)
    }

    registerWheelListener(dotnetReference, element) {
        element.parentElement.addEventListener('wheel', this.boundWheelListener = this.wheelHandler.bind(this, dotnetReference))
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

    dispose(element, panzoom) {
        element.parentElement.removeEventListener('wheel', panzoom.zoomWithWheel);
        if (this.boundWheelListener) {
            element.parentElement.removeEventListener('wheel', this.boundWheelListener);
        }
    }

}

window.blazorPanzoom = new BlazorPanzoomInterop()