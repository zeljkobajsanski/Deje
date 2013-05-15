/* VARIABLES */

//var baseUri = 'http://localhost:36862/';
var baseUri = 'http://localhost/Deje.Web.Mobile/';
var nePretrazuj = false;
var map;
var mapsKey = 'AoAspjo-51Af3fRuiPmgnwZ6969gydoWrZ26rmvfE9tk10e7Q5JYNvs4rXLQ8VKn';
var defaultZoomLevel = 15;
var inicijalizovanaPonuda = false;
var myData;
var routeMap;

function storage() {
    this.latitude = 0;
    this.longitude = 0;
    this.distance = 0;
    this.idDelatnosti = 0;
    this.idGrupeArtikala = null;
    this.idArtikla = 0;

    this.getLatitude = function () {
        if (Modernizr.localstorage) {
            return localStorage['latitude'];
        }
        return this.latitude;
    };
    this.setLatitude = function (value) {
        this.latitude = value;
        if (Modernizr.localstorage) {
            localStorage['latitude'] = value;
        }
    };
    this.getLongitude = function () {
        if (Modernizr.localstorage) {
            return localStorage['longitude'];
        }
        return this.longitude;
    };
    this.setLongitude = function(value) {
        this.longitude = value;
        if (Modernizr.localstorage) {
            localStorage['longitude'] = value;
        }
    };
    this.getDistance = function() {
        if (Modernizr.localstorage) {
            return localStorage['distance'];
        }
        return this.distance;
    };
    this.setDistance = function(value) {
        this.distance = value;
        if (Modernizr.localstorage) {
            localStorage['distance'] = value;
        }
    };
    this.setIdGrupeArtikala = function(value) {
        this.idGrupeArtikala = value;
        if (Modernizr.localstorage) {
            localStorage['idGrupeArtikala'] = value;
        }
    };
    this.getIdGrupeArtikala = function() {
        if (Modernizr.localstorage) {
            return localStorage['idGrupeArtikala'];
        }
        return this.idGrupeArtikala;
    };
    this.setIdDelatnosti = function(value) {
        this.idDelatnosti = value;
        if (Modernizr.localstorage) {
            localStorage['idDelatnosti'] = value;
        }
    };
    this.getIdDelatnosti = function() {
        if (Modernizr.localstorage) {
            return localStorage['idDelatnosti'];
        }
        return this.idDelatnosti;
    };
    this.setIdArtikla = function(value) {
        this.idArtikla = value;
        if (Modernizr.localstorage) {
            localStorage['idArtikla'] = value;
        }
    };
    this.getIdArtikla = function() {
        if (Modernizr.localstorage) {
            return localStorage['idArtikla'];
        }
        return this.idArtikla;
    };
};


/* FUNCTIONS */

function pronadjiAdresu() {
    prikaziCekalicu();
    $.get(baseUri + 'Home/PronadjiAdresu', { 'adresa': $("#adresa").val() }, function (result) {
        if (result.status == '200') {
            if (result.count == 0) {
                prikaziInfoDialog("Adresa nije pronađena");
            } else if (result.count == 1) {
                updatePosition(result.result[0].Latitude, result.result[0].Longitude, result.result[0].Naziv);
                $.mobile.changePage("#Home");
                return false;
            } else {
                $.mobile.changePage("#pronadjeneAdreseDialog");
                var $list = $("#pronadjeneAdreseDialog article>ul");
                $list.empty();
                $.each(result.result, function (i, el) {
                    var $li = $("<li></li>");
                    var $a = $("<a href='#Home'>" + el.Naziv + "</a>");
                    $a.data("latitude", el.Latitude);
                    $a.data("longitude", el.Longitude);
                    $a.click(function () {
                        updatePosition($(this).data("latitude"), $(this).data("longitude"), $(this).text());
                    });
                    $li.append($a);
                    $list.append($li);
                });
                $list.listview('refresh');
            }
        }
        skloniCekalicu();
    });
}

function pretraziGrupeArtikala() {
    if (nePretrazuj) return;
    var criteria = {
        'latituda': myData.getLatitude(),
        'longituda': myData.getLongitude(),
        'udaljenost': myData.getDistance(),
        'delatnost': myData.getIdDelatnosti()
    };
    prikaziCekalicu();
    $.get(baseUri + 'Home/Pronadji', criteria,
        function (result) {
            var $list = $("#GrupeArtikala ul");
            $list.empty();
            if (result.count == 0) {
                nePretrazuj = true;
                prikaziInfoDialog("Nema rezultata za izabrane kriterijume");
            } else {
                
                $.each(result.grupeArtikala, function (i, e) {
                    var $a = $("<a href='#Artikli' data-role=''>" + e.naziv + "<span class='ui-li-count'>" + e.count + "</span></a>");
                    $a.click(function () {
                        myData.setIdGrupeArtikala(e.id);
                    });
                    $("<li></li>").append($a).appendTo($list);
                });
                $list.listview('refresh');
            }
            skloniCekalicu();
        });
}

function pretraziArtikle() {
    prikaziCekalicu();
    $.get(baseUri + "Home/PretraziArtikle", {
        'idGrupeArtikala': myData.getIdGrupeArtikala(),
        'latituda': myData.getLatitude(),
        'longituda': myData.getLongitude(),
        'razdaljina': myData.getDistance()
    }, prikaziArtikle);
}

function prikaziArtikle(data) {
    var $listaArtikala = $("#listaArtikala");
    $listaArtikala.empty();
    $.each(data, function(i, e) {
        var $li = $("<li></li>");
        var $a = $("<a href='#Mapa'></a>");
        $a.append("<img src='" + baseUri + e.Slika + "' alt='slika' />")
          .append("<h1>" + e.Naziv + "</h1>")
          .append("<p>" + e.Opis + "</p>");
        $a.click(function() {
            myData.setIdArtikla(e.Id);
        });
        $li.append($a);
        $listaArtikala.append($li);
    });
    $listaArtikala.listview('refresh');
    skloniCekalicu();
}

function prikaziNaMapi() {
    prikaziCekalicu();
    var myPushpin = getMyPushpin();
    map.entities.clear();
    map.entities.push(myPushpin);
    var criteria = {
        latitude: myData.getLatitude(),
        longitude: myData.getLongitude(),
        distance: myData.getDistance(),
        idArtikla: myData.getIdArtikla()
    };
    $.get(baseUri + "Home/PretraziDobavljace", criteria, function(result) {
        $.each(result, function(i, e) {
            var pushpin = new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(e.Latitude, e.Longitude));
            map.entities.push(pushpin);
            Microsoft.Maps.Events.addHandler(pushpin, 'click', function() {
                var infoBox = new Microsoft.Maps.Infobox(pushpin.getLocation(), {
                    htmlContent: getInfoboxContent(e),
                    offset : new Microsoft.Maps.Point(0, 37)
                });
                Microsoft.Maps.Events.addHandler(infoBox, 'click', function () {
                    closeInfobox(infoBox);
                });
                map.entities.push(infoBox);
            });
        });
        skloniCekalicu();
    });
}

function ucitajDelatnosti() {
    prikaziCekalicu();
    $.getJSON(baseUri + 'Home/UcitajDelatnosti', function(data) {
        var $select = $("<select name='delatnosti' id='delatnosti'></select>");
        $.each(data, function(i, e) {
            $select.append("<option value='" + e.Id +"'>" + e.Naziv + "</option>");
        });
        $select.insertBefore("#btnPretraziGrupeArtikala").selectmenu();
        skloniCekalicu();
    });
}

function ucitajDobavljaca(id) {
    prikaziCekalicu();
    $.getJSON(baseUri + "Home/VratiDobavljaca", { id: id }, function(dobavljac) {
        $("#Dobavljac #slikaDobavljaca").attr("src", baseUri + dobavljac.Slika);
        $("#Dobavljac #nazivDobavljaca").text(dobavljac.Naziv);
        $("#Dobavljac #vrstaDobavljaca").text(dobavljac.VrstaDobavljaca);
        $("#Dobavljac #opisDobavljaca").text('').text(dobavljac.Opis);

        prikaziPonudu(dobavljac);
        prikaziKontaktPodatke(dobavljac);
        skloniCekalicu();
    });
}

function prikaziPonudu(dobavljac) {
    var $list = $("#meniLista");
    $.each(dobavljac.Ponuda, function(i, e) {
        $list.append("<li data-role='list-divider'>" + e.Kategorija + "</li>");
        $.each(e.Artikli, function(i1, e1) {
            $list.append("<li>" +
                "<img src='" + baseUri + e1.Slika + "' alt='' />" +
                    "<h3>" + e1.Naziv + "</h3>" +
                    "<p>" + e1.Opis + "</p>" +
                    "<p>" + e1.Cena + "</p>" +
                "</li>");
        });
    });
    if (inicijalizovanaPonuda) {
        $list.listview("refresh");
    }
}

function prikaziKontaktPodatke(dobavljac) {
    $("#nazivKontakta").text(dobavljac.Naziv);
    $("#mestoKontakta").text(dobavljac.Mesto);
    if (dobavljac.Www != null) {
        $("#wwwKontakta").text(dobavljac.Www).attr("href", dobavljac.Www).show();
    } else {
        $("#wwwKontakta").hide();
    }
    if (dobavljac.Telefon != null) {
        $("#telefonKontakta").attr("href", "tel:" + dobavljac.Telefon).show();
        if ($("#telefonKontakta span.ui-btn-text").length != 0) {
            $("#telefonKontakta span.ui-btn-text").text(dobavljac.Telefon);
        } else {
            $("#telefonKontakta").text(dobavljac.Telefon);
        }
    } else {
        $("#telefonKontakta").hide();
    }
    $("#kakoStici").attr("href", "#Ruta?latitude=" + dobavljac.Latitude + "&longitude=" + dobavljac.Longitude);
}

function prikaziNaRouteMap() {
    prikaziCekalicu();
    var myPushpin = getMyPushpin();
    routeMap.entities.clear();
    routeMap.entities.push(myPushpin);
    if ($.mobile.pageData && $.mobile.pageData.latitude && $.mobile.pageData.longitude) {
        var destinationPushpin = new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location($.mobile.pageData.latitude, $.mobile.pageData.longitude));
        routeMap.entities.push(destinationPushpin);
        var request = "http://dev.virtualearth.net/REST/v1/Routes?wp.0=" + myPushpin.getLocation().latitude + "," + myPushpin.getLocation().longitude +
            "&wp.1=" + destinationPushpin.getLocation().latitude + "," + destinationPushpin.getLocation().longitude +
            "&routePathOutput=Points&optimize=distance&output=json&jsonp=routeCallback&key=" + mapsKey;
        var script = document.createElement("script");
        script.setAttribute("type", "text/javascript");
        script.setAttribute("src", request);
        document.body.appendChild(script);
    }
}

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


/* HELPER FUNCTIONS */

function getMyPushpin() {
    return new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(myData.getLatitude(), myData.getLongitude()), {
        draggable: false,
        icon: baseUri + 'Content/profile.png'
    });
}

function updatePosition(latitude, longitude, address) {
    myData.setLatitude(latitude);
    myData.setLongitude(longitude);
    var $button = $("#btnPronadjiMe span.ui-btn-text");
    $button.text(address != undefined ? address : "Vaša pozicija je ažurirana").removeClass("red");

    if (myData.getLatitude() === 'null' || myData.getLongitude() === 'null') {
        $button.addClass("red").text("Vaša pozicija nije pronađena");
    }
}

function locirajMe() {
    if (Modernizr.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            updatePosition(position.coords.latitude, position.coords.longitude);
        }, function () {
            updatePosition(null, null);
        });
    } else {
        updatePosition(null, null);
    }
}

function prikaziInfoDialog(poruka) {
    $("#infoMessage").text(poruka);
    $.mobile.changePage("#infoDialog");
}

function closeDialog() {
    $(".ui-dialog").dialog('close');
}

function getInfoboxContent(dobavljac) {
    return "<div class='pushpin'>" +
                "<p class='naziv'>" + dobavljac.Naziv + "</p>" +
                "<p>" + dobavljac.Vrsta + "</p>" +
                "<p>Udaljenost: " + dobavljac.Udaljenost + " m</p>" +
                "<a href='#Dobavljac?id=" + dobavljac.Id + "'>Detaljnije</a>" +
           "</div>";
}


function closeInfobox(infobox) {
    infobox.setOptions({ visible: false });
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

function prikaziCekalicu() {
    $.mobile.showPageLoadingMsg();
}

function skloniCekalicu() {
    $.mobile.hidePageLoadingMsg();
}

/* End of FUNCTIONS */

/* DOCUMENT READY */

$(function () {

    myData = new storage();
    myData.setDistance(2000);
    myData.setLatitude(45.92586898803711);
    myData.setLongitude(20.077590942382812);
    
    //locirajMe();

    myData.setIdDelatnosti(1);
    
    $(document).bind("pagebeforechange", function (event, data) {
        $.mobile.pageData = (data && data.options && data.options.pageData)
            ? data.options.pageData
            : null;
    }).bind('mobileinit', function () {
        $.support.cors = true;
        $.mobile.allowCrossDomainPages = true;
    });

    $("header h1").click(function() {
        $.mobile.changePage("#Home");
    });

    $("#distance").change(function() {
        myData.setDistance($(this).val());
    }).val(myData.getDistance());
    
    

    $("#delatnost").change(function() {
        myData.setIdDelatnosti($(this).val());
    }).load(function () {
        myData.setIdDelatnosti($(this).val());
    });

    ucitajDelatnosti();

    

    $("#PretragaLokacije").live('pageinit', function() {
        $("#frmPretragaLokacije").submit(function () {
            pronadjiAdresu();
            return false;
        });
    });
    
    $("#PretragaLokacije").live('pageinit', function() {
        $("#btnLocirajMe").live('click', locirajMe);
    });

    $("#GrupeArtikala").live('pageinit', function () {
        $("#GrupeArtikala .btnRefresh").click(pretraziGrupeArtikala);
    }).live('pageshow', pretraziGrupeArtikala);

    $("#Artikli").live('pageinit', function () {
        $("#Artikli .btnRefresh").click(pretraziArtikle);
    }).live("pageshow", pretraziArtikle);

    $("#btnPretraziGrupeArtikala").click(function() {
        nePretrazuj = false;
    });
    $("#Mapa").live('pageinit', function () {
        $("#btnZoomIn").click(function () {
            zoomIn(map);
        });
        $("#btnZoomOut").click(function () {
            zoomOut(map);
        });
        map = new Microsoft.Maps.Map(document.getElementById("map"), {
            credentials: mapsKey,
            center: new Microsoft.Maps.Location(myData.getLatitude(), myData.getLongitude()),
            mapTypeId: Microsoft.Maps.MapTypeId.road,
            zoom: defaultZoomLevel,
            showBreadcrumb : false,
            showDashboard: false,
            enableSearchLogo : false,
        });
    });
    $("#Mapa").live('pageshow', prikaziNaMapi);

    $("#Dobavljac").live('pageshow', function() {
        if ($.mobile.pageData && $.mobile.pageData.id) {
            ucitajDobavljaca($.mobile.pageData.id);
        }
    });
    $("#Ponuda").live('pageinit', function() {
        inicijalizovanaPonuda = true;
    });
    $("#Ruta").live('pageinit', function() {
        routeMap = new Microsoft.Maps.Map(document.getElementById("routeMap"), {
            credentials: mapsKey,
            center: new Microsoft.Maps.Location(myData.getLatitude(), myData.getLongitude()),
            mapTypeId: Microsoft.Maps.MapTypeId.road,
            zoom: defaultZoomLevel,
            showBreadcrumb: false,
            showDashboard: false,
            enableSearchLogo: false,
        });
        $("#btnRouteZoomIn").live('click', function() {
            zoomIn(routeMap);
        });
        $("#btnRouteZoomOut").live('click', function () {
            zoomOut(routeMap);
        });
    }).live('pageshow', prikaziNaRouteMap);
})