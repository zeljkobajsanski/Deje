var map, marker, $lat, $lng;
$(document).ready(function () {
    var bingMapsCredentials = 'AoAspjo-51Af3fRuiPmgnwZ6969gydoWrZ26rmvfE9tk10e7Q5JYNvs4rXLQ8VKn';
    var defaultZoom = 13;
    var defaultLatitude = 45.25;
    var defaultLongitude = 19.85;
    var mapOptions = {
        center: new google.maps.LatLng(defaultLatitude, defaultLongitude),
        zoom: defaultZoom,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    map = new google.maps.Map(document.getElementById("mapa"), mapOptions);
    $lat = $("#GpsLatitude");
    $lng = $("#GpsLongitude");
    var lat = $lat.val();
    var lng = $lng.val();
    var position = new google.maps.LatLng(lat, lng);
    marker = new google.maps.Marker({
        position: position,
        map: map,
        draggable: true
    });

    google.maps.event.addListener(marker, 'dragend', function(e) {
        updatePosition(e.latLng);
    });
});

function pronadjiLokaciju(adresa) {
    var gc = new google.maps.Geocoder();
    gc.geocode({ address: adresa }, function (result, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            var latLng = result[0].geometry.location;
            updatePosition(latLng);
        }
    });
}

function updatePosition(latLng) {
    $lat.val(latLng.lat());
    $lng.val(latLng.lng());
    map.setCenter(latLng);
    marker.setPosition(latLng);
}