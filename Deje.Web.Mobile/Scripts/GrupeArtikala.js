$(function () {
    var lat, lon, razdaljina, delatnost;

    var pretrazi = function() {
        if ($("#nePretrazuj").val() == 1) return;
        var criteria = {
            'latituda': lat,
            'longituda': lon,
            'udaljenost': razdaljina,
            'delatnost': delatnost
        };
        prikaziCekalicu();
        $.get(baseUri + 'Home/Pronadji', criteria,
            function(result) {
                var $list = $("#GrupeArtikala ul");
                $list.empty();
                if (result.count == 0) {
                    $("#nePretrazuj").val(1);
                    prikaziInfoDialog("Nema rezultata za izabrane kriterijume");
                } else {

                    $.each(result.grupeArtikala, function(i, e) {
                        var href = "#Artikli?lat=" + lat + "&lon=" + lon + "&razdaljina=" + razdaljina + "&grupa=" + e.id;
                        var $a = $("<a href='" + href + "' data-role=''>" + e.naziv + "<span class='ui-li-count'>" + e.count + "</span></a>");
                        $("<li></li>").append($a).appendTo($list);
                    });
                    $list.listview('refresh');
                }
            }).error(function () {
                alert("Greška prilikom pretrage. Pokušajte ponovo.");
            }).complete(function () {
                skloniCekalicu();
            });
    };
    
    var initPage = function () {
        $("#btnOsveziGrupeArtikala").click(pretrazi);
        $("#nePretrazuj").val("0");
        
    };

    var pageBeforeShow = function() {
        var list = $("#GrupeArtikala ul");
        clearList(list);
    };

    var pageShow = function () {
        if ($.mobile.pageData && $.mobile.pageData.lat && $.mobile.pageData.razdaljina) {
            lat = $.mobile.pageData.lat;
            lon = $.mobile.pageData.lon;
            razdaljina = $.mobile.pageData.razdaljina;
            delatnost = 1;
            pretrazi();
        }
    };

    $("#GrupeArtikala").live('pageinit', initPage)
                       .live('pageshow', pageShow)
                       .live('pagebeforeshow', pageBeforeShow);
    
});