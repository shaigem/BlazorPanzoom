window.blazorPanzoom = {
    createPanzoomForReference: function (element, options) {
        const newOptions = options
        if(newOptions.origin === 'undefined') {
            delete newOptions.origin
        }
        if(newOptions.contain === 'none') {
            delete newOptions.contain
        }
        if(newOptions.focal.x === 0 && newOptions.focal.y === 0) {
            delete newOptions.focal
        }
        return Panzoom(element, newOptions)
    },
    createPanzoomForId: function (id) {
        const element = document.getElementById(id)
        return Panzoom(element)
    },
    createPanzoomForSelector: function (selector) {
        const element = document.querySelector(selector)
        return Panzoom(element)
    }
}