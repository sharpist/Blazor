window.onlineStatusIndicator = {
	registerOnlineStatusHandler: function (dotNetObjRef) {
		function onlineStatusHandler() {
			dotNetObjRef.invokeMethodAsync("SetOnlineStatusColorRoot", navigator.onLine);
		};
		window.addEventListener('online', onlineStatusHandler);
		window.addEventListener('offline', onlineStatusHandler);
		onlineStatusHandler();
	}
};
