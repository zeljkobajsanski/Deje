$(function () {
    var id, myLat, myLon, initialized;

    var pageInit = function() {
        $(".btnDobavljac").click(function () {
            $.mobile.changePage("#Dobavljac", { changeHash: false });
            $(".btnPonuda").removeClass("ui-btn-active");
            $(".btnKontakt").removeClass("ui-btn-active");
        });
        $(".btnPonuda").click(function () {
            $.mobile.changePage("#Ponuda", { changeHash: false });
            $(".btnDobavljac").removeClass("ui-btn-active");
            $(".btnKontakt").removeClass("ui-btn-active");
        });
        $(".btnKontakt").click(function () {
            $.mobile.changePage("#Kontakt", { changeHash: false });
            $(".btnDobavljac").removeClass("ui-btn-active");
            $(".btnPonuda").removeClass("ui-btn-active");
        });
    };

    var pageShow = function() {
        if ($.mobile.pageData && $.mobile.pageData.id && $.mobile.pageData.myLat && $.mobile.pageData.myLon) {
            id = $.mobile.pageData.id;
            myLat = $.mobile.pageData.myLat;
            myLon = $.mobile.pageData.myLon;
            $(".btnPonuda").removeClass("ui-btn-active");
            $(".btnKontakt").removeClass("ui-btn-active");
            prikazi();
        }
    };

    var prikazi = function() {
        prikaziCekalicu();
        $.getJSON(baseUri + "Home/VratiDobavljaca", { id: id }, function (dobavljac) {
            $("#Dobavljac #slikaDobavljaca").attr("src", dobavljac.Slika);
            $("#Dobavljac #nazivDobavljaca").text(dobavljac.Naziv);
            $("#Dobavljac #vrstaDobavljaca").text(dobavljac.VrstaDobavljaca);
            $("#Dobavljac #opisDobavljaca").text('').text(dobavljac.Opis);

            prikaziPonudu(dobavljac);
            prikaziKontaktPodatke(dobavljac);
            var href = "#Ruta?lat=" + dobavljac.Latitude + "&lon=" + dobavljac.Longitude + "&myLat=" + myLat + "&myLon=" + myLon;
            $("#kakoStici").attr("href", href);
            skloniCekalicu();
        });
    };
    
    function prikaziPonudu(dobavljac) {
        var $list = $("#meniLista");
        $list.empty();
        $.each(dobavljac.Ponuda, function (i, e) {
            $list.append("<li data-role='list-divider'>" + e.Kategorija + "</li>");
            $.each(e.Artikli, function (i1, e1) {
                $list.append("<li>" +
                    "<img src='" + e1.Slika + "' alt='' />" +
                        "<h3>" + e1.Naziv + "</h3>" +
                        "<p>" + e1.Opis + "</p>" +
                        "<p>" + e1.Cena + "</p>" +
                    "</li>");
            });
        });
        if (initialized) {
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

    $("#Dobavljac").live('pageinit', pageInit)
                   .live('pageshow', pageShow);

    $("#Ponuda").live('pageinit', function() {
        initialized = true;
    });
});