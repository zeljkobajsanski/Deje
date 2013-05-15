Deje_DXMobile.Artikli = function (params) {

    var artikli = ko.observableArray();
    var busy = ko.observable();
    var ucitano = false;
    var ucitaj = function () {
        if (ucitano) return;
        $.ajax({
            url: Deje_DXMobile.SERVICE_URL + "/PretraziArtikle",
            data: {
                idGrupeArtikala: params.id,
                latituda: Deje_DXMobile.model.lat(),
                longituda: Deje_DXMobile.model.lon(),
                razdaljina: Deje_DXMobile.model.dist()
            },
            beforeSend: function () { busy(true); }
        }).success(function (result) {
            artikli(result);
            ucitano = true;
        }).complete(function () {
            busy(false);
        });
    };
    

    return {
        artikli: artikli,
        busy: busy,
        viewShown: ucitaj
    };
}