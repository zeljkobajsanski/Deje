$(function () {
    
    var lat, lon;
    $("#distance").val(2000);
    $("#btnPronadjiMe span.ui-btn-text").addClass("red");
    if (Modernizr.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            lat = position.coords.latitude;
            lon = position.coords.longitude;
            $("#btnPronadjiMe .ui-btn-text").text("Vaša pozicija je ažurirana").removeClass("red");
        }, function () {
            
        });
    }

    $("#pretragaPoNazivu").submit(function () {
        var nazivArtikla = $("#nazivArtikla").val();
        var razdaljina = $("#distance").val();
        if (nazivArtikla && lat && lon && razdaljina) {
            dodajNazivUStorage(nazivArtikla);
            $.mobile.changePage("#Artikli?lat=" + lat + "&lon=" + lon + "&razdaljina=" + razdaljina + "&naziv=" + nazivArtikla);
        }
        return false;
    });

    var pretraziGrupeArtikala = function() {
        var razdaljina = $("#distance").val();
        if (lat && lon) {
            $("#nePretrazuj").val(0);
            var nazivArtikla = $("#nazivArtikla").val();
            if (nazivArtikla) {
                dodajNazivUStorage(nazivArtikla);
                window.location = "#Artikli?lat=" + lat + "&lon=" + lon + "&razdaljina=" + razdaljina + "&naziv=" + nazivArtikla;
                return false;
            } 
            $.mobile.changePage("#GrupeArtikala?lat=" + lat + "&lon=" + lon + "&razdaljina=" + razdaljina);
        }
        return false;
    };
    
    function dodajNazivUStorage(naziv) {
        var historyStr = window.localStorage.getItem(PRETRAGA_ARTIKALA_KEY);
        var history;
        if (historyStr) {
            history = JSON.parse(historyStr);
        } else {
            history = [];
        }
        history.unshift(naziv);
        historyStr = JSON.stringify(history);
        window.localStorage.setItem(PRETRAGA_ARTIKALA_KEY, historyStr);
    }

    var pageShow = function() {
        if ($.mobile.pageData && $.mobile.pageData.lat && $.mobile.pageData.lon) {
            lat = $.mobile.pageData.lat;
            lon = $.mobile.pageData.lon;
        }
        if ($.mobile.pageData && $.mobile.pageData.artikal) {
            $("#nazivArtikla").val($.mobile.pageData.artikal);
        }
    };
    
    $("#btnPretraziGrupeArtikala").click(pretraziGrupeArtikala);

    $("#Home").live('pageshow', pageShow);
});