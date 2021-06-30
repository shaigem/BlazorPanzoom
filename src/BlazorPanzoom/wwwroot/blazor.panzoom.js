window.blazorPanzoom = {
    registerPanZoom: function (element) {
        const panzoom = Panzoom(element, {
            contain: "outside",
            maxScale: 4
        })
        element.parentElement.addEventListener('wheel', panzoom.zoomWithWheel)
    },
}


