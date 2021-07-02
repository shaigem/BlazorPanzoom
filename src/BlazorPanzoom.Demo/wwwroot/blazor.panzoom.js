window.blazorPanzoom = {
    createPanzoomForReference: function (element, options) {
        return Panzoom(element, options)
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