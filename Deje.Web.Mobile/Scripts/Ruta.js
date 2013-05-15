var routeMap;

$(function () {

    var myLat, myLon, lat, lon;
    var defaultZoomLevel = 13;
    
    function routeCallback(result) {
        if (result &&
              result.resourceSets &&
              result.resourceSets.length > 0 &&
              result.resourceSets[0].resources &&
              result.resourceSets[0].resources.length > 0) {

            // Draw the route
            var routeline = result.resourceSets[0].resources[0].routePath.line;
            var routepoints = new Array();

            for (var i = 0; i < routeline.coordinates.length; i++) {

                routepoints[i] = new Microsoft.Maps.Location(routeline.coordinates[i][0], routeline.coordinates[i][1]);
            }

            // Draw the route on the map
            var routeshape = new Microsoft.Maps.Polyline(routepoints, { strokeColor: new Microsoft.Maps.Color(255, 255, 125, 000) });
            routeMap.entities.push(routeshape);
            skloniCekalicu();
        }
    }

    function prikaziNaRouteMap() {
        prikaziCekalicu();
        var myPushpin = new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(myLat, myLon), {
            draggable: false,
            icon: baseUri + 'Content/profile.png'
        });
        routeMap.entities.clear();
        routeMap.entities.push(myPushpin);
        var destinationPushpin = new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(lat, lon));
        routeMap.entities.push(destinationPushpin);
        var request = "http://dev.virtualearth.net/REST/v1/Routes?wp.0=" + myPushpin.getLocation().latitude + "," + myPushpin.getLocation().longitude +
            "&wp.1=" + destinationPushpin.getLocation().latitude + "," + destinationPushpin.getLocation().longitude +
            "&routePathOutput=Points&optimize=distance&output=json&jsonp=routeCallback&key=" + mapsKey;
        var script = document.createElement("script");
        script.setAttribute("type", "text/javascript");
        script.setAttribute("src", request);
        document.body.appendChild(script);
    }

    var pageInit = function() {
        routeMap = new Microsoft.Maps.Map(document.getElementById("routeMap"), {
            credentials: mapsKey,
            
            mapTypeId: Microsoft.Maps.MapTypeId.road,
            zoom: defaultZoomLevel,
            showBreadcrumb: false,
            showDashboard: false,
            enableSearchLogo: false,
        });
        $("#btnRouteZoomIn").live('click', function () {
            zoomIn(routeMap);
        });
        $("#btnRouteZoomOut").live('click', function () {
            zoomOut(routeMap);
        });
    };

    var pageShow = function() {
        if ($.mobile.pageData
            && $.mobile.pageData.myLat && $.mobile.pageData.myLon
            && $.mobile.pageData.lat && $.mobile.pageData.lon) {
            myLat = $.mobile.pageData.myLat;
            myLon = $.mobile.pageData.myLon;
            lat = $.mobile.pageData.lat;
            lon = $.mobile.pageData.lon;
            var center = new Microsoft.Maps.Location(myLat, myLon);
            routeMap.setView({ 'center': center });
            prikaziNaRouteMap();
        }
    };

    $("#Ruta").live('pageinit', pageInit)
              .live('pageshow', pageShow);
});

function routeCallback(result) {
    if (result &&
          result.resourceSets &&
          result.resourceSets.length > 0 &&
          result.resourceSets[0].resources &&
          result.resourceSets[0].resources.length > 0) {

        // Draw the route
        var routeline = result.resourceSets[0].resources[0].routePath.line;
        var routepoints = new Array();

        for (var i = 0; i < routeline.coordinates.length; i++) {

            routepoints[i] = new Microsoft.Maps.Location(routeline.coordinates[i][0], routeline.coordinates[i][1]);
        }

        // Draw the route on the map
        var routeshape = new Microsoft.Maps.Polyline(routepoints, { strokeColor: new Microsoft.Maps.Color(255, 255, 125, 000) });
        routeMap.entities.push(routeshape);
        skloniCekalicu();
    }
}