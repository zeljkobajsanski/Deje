Deje_DXMobile.Home = function (params) {

    var delatnosti = ko.observableArray();
    var izabranaDelatnost = ko.observable();
    var osvezi = function () {
        Deje_DXMobile.ucitajKoordinate();
        $.ajax({
        url: Deje_DXMobile.SERVICE_URL + '/UcitajDelatnosti'
        }).success(function (result) {
            delatnosti(result);
            if (result.length == 1) {
                izabranaDelatnost(result[0].Id);
            }

        }).error(function () {
            alert('Greška prilikom učitavanja delatnosti');
        });
    };
    var pretrazi = function () {
        var uri = Deje_DXMobile.app.router.format({
            view: 'KategorijeArtikala',
            id: izabranaDelatnost()
        });
        Deje_DXMobile.app.navigate(uri);
    }

    osvezi();

    var viewModel = {
        adress: Deje_DXMobile.model.adress,
        distance: Deje_DXMobile.model.dist,
        max: 10000,
        step: 100,
        delatnosti: delatnosti,
        izabranaDelatnost: izabranaDelatnost,
        osvezi: osvezi,
        pretrazi: pretrazi
    };

    return viewModel;
};