class BlazorPanzoomInterop {

    constructor() {
    }

    createPanzoomForReference(element, options) {
        return Panzoom(element, options)
    }

    createPanzoomForSelector(selector, options) {
        try {
            const elements = document.querySelectorAll(selector)
            const array = []
            elements.forEach((element) => array.push(DotNet.createJSObjectReference(Panzoom(element, options))))
            return array
        } catch {
            throw new Error(`Cannot create a Panzoom object from selectors!`);
        }
    }

    performForAllPanzoom(functionName, panzoomList, args) {
        if (!panzoomList) {
            return
        }

        for (let i = 0; i < panzoomList.length; i++) {
            const ref = panzoomList[i].jsPanzoomReference
            ref[functionName](args)
        }
    }

    registerDefaultWheelZoom(element, panzoom) {
        element.parentElement.addEventListener('wheel', panzoom.zoomWithWheel)
    }

    registerWheelListener(dotnetReference, element, panzoom) {
        panzoom.dotNetWheelListenerReference = dotnetReference
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
            panzoom.dotNetWheelListenerReference.dispose()
        }
    }
}

window.blazorPanzoom = new BlazorPanzoomInterop()