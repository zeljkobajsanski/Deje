var bingMapsCredentials = 'AoAspjo-51Af3fRuiPmgnwZ6969gydoWrZ26rmvfE9tk10e7Q5JYNvs4rXLQ8VKn';
var defaultZoom = 13;
var defaultLatitude = 45.25;
var defaultLongitude = 19.85;

function callBingMapsSearch(searchTerm, callback) {
    var geocodeRequest = "http://dev.virtualearth.net/REST/v1/Locations?query=" + encodeURI(searchTerm) + "&output=json&jsonp=" + callback + "&key=" + bingMapsCredentials;
    var script = document.createElement("script");
    script.setAttribute("type", "text/javascript");
    script.setAttribute("src", geocodeRequest);
    document.body.appendChild(script);
}