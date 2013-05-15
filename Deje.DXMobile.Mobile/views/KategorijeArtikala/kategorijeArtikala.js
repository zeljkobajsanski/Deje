Deje_DXMobile.KategorijeArtikala = function (params) {
    var kategorije = ko.observableArray();
    var busy = ko.observable();
    var ucitano = false;
    var ucitaj = function () {
        if (ucitano) return;
        $.ajax({
            url: Deje_DXMobile.SERVICE_URL + '/Pronadji',
            data: {
                latituda: Deje_DXMobile.model.lat(),
                longituda: Deje_DXMobile.model.lon(),
                udaljenost: Deje_DXMobile.model.dist(),
                delatnost: params.id
            },
            beforeSend: function () { busy(true); }
        }).success(function (result) {
            kategorije(result.grupeArtikala);
            ucitano = true;
        }).complete(function () {
            busy(false);
        });
    };

    return {
        busy: busy,
        kategorije: kategorije,
        viewShown: ucitaj
    };
}