$(function() {
    var lat, lon;
    var pageInit = function () {
        $("#frmPretragaLokacije").submit(function () {
            pronadjiAdresu();
            return false;
        });
        
        $("#btnLocirajMe").click(function() {
            if (Modernizr.geolocation) {
                navigator.geolocation.getCurrentPosition(function (position) {
                    lat = position.coords.latitude;
                    lon = position.coords.longitude;
                    updatePosition(lat, lon, "Vaša pozicija je ažurirana");
                }, function () {

                });
            }
            return false;
        });
    };

    var pronadjiAdresu = function() {
        prikaziCekalicu();
        $.get(baseUri + 'Home/PronadjiAdresu', { 'adresa': $("#adresa").val() }, function(result) {
            if (result.status == '200') {
                if (result.count == 0) {
                    prikaziInfoDialog("Adresa nije pronađena");
                } else if (result.count == 1) {
                    updatePosition(result.result[0].Latitude, result.result[0].Longitude, result.result[0].Naziv);
                    return false;
                } else {
                    $.mobile.changePage("#pronadjeneAdreseDialog");
                    var $list = $("#pronadjeneAdreseDialog article>ul");
                    $list.empty();
                    $.each(result.result, function(i, el) {
                        var $li = $("<li></li>");
                        var $a = $("<a href='#Home'>" + el.Naziv + "</a>");
                        $a.data("latitude", el.Latitude);
                        $a.data("longitude", el.Longitude);
                        $a.click(function() {
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
    };
    
    function updatePosition(latitude, longitude, caption) {
        $("#btnPronadjiMe .ui-btn-text").text(caption).removeClass("red");
        window.location = "#Home?lat=" + latitude + "&lon=" + longitude;
    }

    var pageShow = function() {
    };

    $("#PretragaLokacije").live('pageinit', pageInit).live('pageshow', pageShow);
});