$(function() {
    var map, lat, lon, razdaljina,  tip, id;
    var defaultZoomLevel = 13;

    var pretrazi = function() {
        prikaziCekalicu();
        map.entities.clear();
        var myPushpin = new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(lat, lon), {
            draggable: false,
            icon: baseUri + 'Content/profile.png'
        });
        map.entities.push(myPushpin);
        var criteria = {
            latitude: lat,
            longitude: lon,
            distance: razdaljina,
            idArtikla: id,
            tip: tip
        };
        $.get(baseUri + "Home/PretraziDobavljace", criteria, function (result) {
            $.each(result, function (i, e) {
                var pushpin = new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(e.Latitude, e.Longitude));
                map.entities.push(pushpin);

                Microsoft.Maps.Events.addHandler(pushpin, 'click', function () {
                    pushpin.visibleInfobox = !pushpin.visibleInfobox;
                    if (pushpin.infobox == null) {
                        var infoBox = new Microsoft.Maps.Infobox(pushpin.getLocation(), {
                            htmlContent: getInfoboxContent(e),
                            offset: new Microsoft.Maps.Point(0, 37),
                        });
                        pushpin.infobox = infoBox;
                        map.entities.push(infoBox);
                    } else {
                        map.entities.remove(pushpin.infobox);
                        pushpin.infobox = null;
                    }
                });
            });
            skloniCekalicu();
        });
    };
    
    function getInfoboxContent(dobavljac) {
        var navigateTo = "window.location='#Dobavljac?id=" + dobavljac.Id +  "&myLat=" + lat + "&myLon=" + lon + "'";
        var onClick = "onclick=" + navigateTo;
        return "<div class='pushpin' " + onClick + ">" +
                    "<p class='naziv'>" + dobavljac.Naziv + "</p>" +
                    "<p>" + dobavljac.Vrsta + "</p>" +
                    "<p>Udaljenost: " + dobavljac.Udaljenost + " m</p>" +
               "</div>";
    }

    var pageInit = function() {
        map = new Microsoft.Maps.Map(document.getElementById("map"), {
            credentials: mapsKey,
            mapTypeId: Microsoft.Maps.MapTypeId.road,
            zoom: defaultZoomLevel,
            showBreadcrumb: false,
            showDashboard: false,
            enableSearchLogo: false,
        });
        $("#btnZoomIn").click(function () {
            zoomIn(map);
        });
        $("#btnZoomOut").click(function () {
            zoomOut(map);
        });
    };

    var pageShow = function() {
        if ($.mobile.pageData 
            && $.mobile.pageData.lat 
            && $.mobile.pageData.lon
            && $.mobile.pageData.razdaljina
            && $.mobile.pageData.tip
            && $.mobile.pageData.id)
        {
            lat = $.mobile.pageData.lat;
            lon = $.mobile.pageData.lon;
            razdaljina = $.mobile.pageData.razdaljina;
            tip = $.mobile.pageData.tip;
            id = $.mobile.pageData.id;

            map.setView({ center: new Microsoft.Maps.Location(lat, lon) });
            pretrazi();
        }
    };

    $("#Mapa").live('pageinit', pageInit)
              .live('pageshow', pageShow);
});