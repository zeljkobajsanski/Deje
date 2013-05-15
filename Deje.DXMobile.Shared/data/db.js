
(function() {
    var endpointSelector = new DevExpress.EndpointSelector(Deje_DXMobile.config.endpoints);

    var serviceConfig = $.extend(true, {}, Deje_DXMobile.config.services, {
        db: {
            url: endpointSelector.urlFor("db"),
			// To enable JSONP support, uncomment the following line
            //jsonp: !window.WinJS,

            errorHandler: handleServiceError
        }
    });

    function handleServiceError(error) {
        if(window.WinJS) {
            try {
                new Windows.UI.Popups.MessageDialog(error.message).showAsync();
            } catch(e) {
                // Another dialog is shown
            }
        } else {
            alert(error.message);
        }
    }

    // Enable partial CORS support for IE < 10
    if($.browser.msie)
        $.support.cors = true;
    
    Deje_DXMobile.db = new DevExpress.data.ODataContext(serviceConfig.db);

}());
