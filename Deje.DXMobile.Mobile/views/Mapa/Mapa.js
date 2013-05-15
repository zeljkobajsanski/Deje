

Deje_DXMobile.Mapa = function (params) {
    var mapa, mojaPozicija;
    var loading = ko.observable();

    var prikaziInfo = function(pushpin) {
        if (!pushpin.Infobox) {
            var ib = new Microsoft.Maps.Infobox(pushpin.getLocation(), {
                title: pushpin.Naziv,
                description: pushpin.Vrsta,
                visble: true,
                titleClickHandler: function (e) {
                    e.preventDefault();
                    Deje_DXMobile.app.navigate("Dobavljac/" + pushpin.Id);
                },
                offset: new Microsoft.Maps.Point(5, 25),
            });
            pushpin.Infobox = ib;
            mapa.entities.push(ib);
        } else {
            pushpin.Infobox.setOptions({visible : !pushpin.Infobox.getVisible()});
        }
    };
    
    var prikaziDobavljace = function (results) {
        if (!mapa) return;
        $.each(results, function (ix, el) {
            var pp = new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(el.Latitude, el.Longitude));
            pp.Id = el.Id;
            pp.Naziv = el.Naziv;
            pp.Vrsta = el.Vrsta + "<br/><span style='font-style: italic'>Udaljenost: " + el.Udaljenost + " m</span>";
            mapa.entities.push(pp);
            Microsoft.Maps.Events.addHandler(pp, 'click', function () {
                prikaziInfo(pp);
            });
        });
    };

    var ucitajDobavljace = function() {
        $.ajax({
            url: Deje_DXMobile.SERVICE_URL + "/PretraziDobavljace",
            data: {
                latitude: Deje_DXMobile.model.lat(),
                longitude: Deje_DXMobile.model.lon(),
                distance: Deje_DXMobile.model.dist(),
                idArtikla: params.id,
                tip: params.tip
            },
            beforeSend: function () { loading(true); }
        }).success(prikaziDobavljace).complete(function () {
            loading(false);
        });
    };

    var ucitajIprikaziMe = function () {
        if (mapa) return;
        mapa = new Microsoft.Maps.Map(document.getElementById("mapa"), {
            credentials: 'AoAspjo-51Af3fRuiPmgnwZ6969gydoWrZ26rmvfE9tk10e7Q5JYNvs4rXLQ8VKn',
            zoom: 13,
            enableSearchLogo: false,
            enableClickableLogo: false
        });
        var mojaLokacija = vratiMojuLokaciju();
        if (!mojaPozicija) {
            mojaPozicija = new Microsoft.Maps.Pushpin(mojaLokacija, { icon: '/profile.png' });
            mapa.entities.push(mojaPozicija);
            mapa.setView({ center: mojaLokacija });
        } else {
            mojaPozicija.setLocation(mojaLokacija);
        }
        ucitajDobavljace();
    };

    var vratiMojuLokaciju = function() {
        return new Microsoft.Maps.Location(Deje_DXMobile.model.lat(), Deje_DXMobile.model.lon());
    };

    return {
        loading: loading,
        viewShown: ucitajIprikaziMe
    };
}