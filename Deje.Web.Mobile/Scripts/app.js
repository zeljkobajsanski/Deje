var baseUri = 'http://njamnjam.azurewebsites.net/';
//var baseUri = 'http://localhost:36861/';
var nePretrazuj = false;
var map;
var mapsKey = 'AoAspjo-51Af3fRuiPmgnwZ6969gydoWrZ26rmvfE9tk10e7Q5JYNvs4rXLQ8VKn';
var defaultZoomLevel = 15;
var inicijalizovanaPonuda = false;
var myData;
var routeMap;
var infoboxEntities;

Microsoft.Maps.Pushpin.prototype.infobox = null;

var PRETRAGA_ARTIKALA_KEY = 'art';

function prikaziCekalicu() {
    $.mobile.showPageLoadingMsg();
}

function skloniCekalicu() {
    $.mobile.hidePageLoadingMsg();
}

function prikaziInfoDialog(poruka) {
    $("#infoMessage").text(poruka);
    $.mobile.changePage("#infoDialog");
}

function closeDialog() {
    $(".ui-dialog").dialog('close');
}

function clearList(list) {
    list.empty();
    list.listview("refresh");
}

function zoomIn(targetMap) {
    var zoomRange = targetMap.getZoomRange();
    var zoom = targetMap.getZoom();
    zoom += 1;
    if (zoom > zoomRange.max) {
        zoom = zoomRange.max;
    }
    targetMap.setView({ 'zoom': zoom });
}

function zoomOut(targetMap) {
    var zoomRange = targetMap.getZoomRange();
    var zoom = targetMap.getZoom();
    zoom -= 1;
    if (zoom < zoomRange.min) {
        zoom = zoomRange.min;
    }
    targetMap.setView({ 'zoom': zoom });
}

$(function() {
    $(document).bind("pagebeforechange", function (event, data) {
        $.mobile.pageData = (data && data.options && data.options.pageData)
            ? data.options.pageData
            : null;
    }).bind('mobileinit', function () {
        $.support.cors = true;
        $.mobile.allowCrossDomainPages = true;
    });
});
