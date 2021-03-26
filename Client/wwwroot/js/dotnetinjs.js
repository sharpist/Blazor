window.dotNetInJS = {
	dotNetObjRef: null,
	setDotNetReference: function (dotNetObjRef) {
		this.dotNetObjRef = dotNetObjRef;
	},
	calculateSquare: function () {
		const number = prompt("Enter your number");
		// DotNet is an object for calling static C# methods from JavaScript
		document.getElementById("string-result").textContent = DotNet
			// assembly name, method name, parameter expected in C# method
			.invokeMethod("Blazor.Client", "CalculateSquareRoot", parseInt(number));
	},
	calculateSquareUnformatted: function () {
		const number = prompt("Enter your number");
		document.getElementById("result").textContent = DotNet
			.invokeMethod("Blazor.Client", "CalculateSquareUnformattedRoot", parseInt(number), true);
	},
	getPersonObject: function (dotNetObjRef) {
		var person = dotNetObjRef.invokeMethod("GetPersonRoot");
		document.getElementById("person").textContent = JSON.stringify(person);
	},
	getWeatherData: function () {
		dotNetInJS.dotNetObjRef.invokeMethodAsync("GetWeatherDataRootAsync")
			.then(weatherData => {
				document.getElementById("weather").textContent = JSON.stringify(weatherData);
			});
	},
	showCoordinates: function () {
		function coordinatesHandler() {
			dotNetInJS.dotNetObjRef.invokeMethod("ShowCoordinatesRoot", {
				x: window.event.screenX,
				y: window.event.screenY
			});
		};
		var el = document.getElementById("coordinates");
		el.onmousemove = coordinatesHandler;
		el.childNodes[0].className = "";
		coordinatesHandler();
	}
};
