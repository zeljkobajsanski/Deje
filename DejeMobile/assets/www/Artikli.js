$(function () {
    var lat, lon, razdaljina, grupa, naziv;

    var pageBeforeShow = function () {
        var list = $("#Artikli ul");
        clearList(list);
    };

    var pretrazi = function() {
        prikaziCekalicu();
        if (grupa) {
            $.get(baseUri + "Home/PretraziArtikle", {
                'idGrupeArtikala': grupa,
                'latituda': lat,
                'longituda': lon,
                'razdaljina': razdaljina
            }, function (data) {
                prikaziRezultate(data);
                skloniCekalicu();
            });
        }
        if (naziv) {
            $.get(baseUri + "Home/PretraziArtiklePoNazivu", {
                'latituda': lat,
                'longituda': lon,
                'razdaljina': razdaljina,
                'naziv': naziv,
            }, function (data) {
                prikaziRezultate(data);
                skloniCekalicu();
            });
        }
        
    };
    
    function prikaziRezultate(data) {
        var $listaArtikala = $("#listaArtikala");
        $listaArtikala.empty();
        $.each(data, function (i, e) {
            var $li = $("<li></li>");
            var href = "#Mapa?lat=" + lat + "&lon=" + lon + "&razdaljina=" + razdaljina + "&tip=" + e.Tip + "&id=" + e.Id;
            var $a = $("<a href='" + href + "'>" + "<span class='ui-li-count'>" + e.Broj + "</span>" + "</a>");
            $a.append("<img src='" + e.Slika + "' alt='slika' />")
              .append("<h1>" + e.Naziv + "</h1>")
              .append("<p>" + e.Opis + "</p>");
            $li.append($a);
            $listaArtikala.append($li);
        });
        $listaArtikala.listview('refresh');
    }

    var initPage = function() {
        $("#btnOsveziArtikle").click(pretrazi);
    };

    var pageShow = function() {
        if ($.mobile.pageData
            && $.mobile.pageData.lat
            && $.mobile.pageData.lon
            && $.mobile.pageData.razdaljina) {
            lat = $.mobile.pageData.lat;
            lon = $.mobile.pageData.lon;
            razdaljina = $.mobile.pageData.razdaljina;
            
            if ($.mobile.pageData.grupa) {
                grupa = $.mobile.pageData.grupa;
                pretrazi();
            }
            if ($.mobile.pageData.naziv) {
                naziv = $.mobile.pageData.naziv;
                pretrazi();
            }
        }
        
    };

    $("#Artikli").live('pageinit', initPage)
                 .live('pageshow', pageShow)
                 .live('pagebeforeshow', pageBeforeShow);
});