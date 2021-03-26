window.jsInDotNet = {
	setText: function (message) {
		document.getElementById("message").textContent = message;
	},
	getValue: function () {
		return document.getElementById("text").value;
	},
	// for .NET 5 with IJSObjectReference
	createCarousel: function () {
		return {
			dotNetObjRef: null,
			carouselNode: null,
			init: function (dotNetObjRef, carouselNode) {
				this.dotNetObjRef = dotNetObjRef;
				this.carouselNode = jQuery(carouselNode);
				// carousel activation
				this.carouselNode.carousel({
					interval: 2000,
					keyboard: false,
					pause:    'hover',
					ride:     'carousel',
					wrap:     true
				});
				// the event before a slide change begins
				this.carouselNode.on('slide.bs.carousel', (event) => {
					this.dotNetObjRef.invokeMethod("OnSlideRoot", event.direction, event.from, event.to);
				});
				// the event after a slide change is completed
				this.carouselNode.on('slid.bs.carousel', (event) => {
					this.dotNetObjRef.invokeMethod("OnSlidRoot", event.direction, event.from, event.to);
				});
			},
			dispose: function () {
				this.carouselNode.carousel('dispose');
			}
		};
	}
};
