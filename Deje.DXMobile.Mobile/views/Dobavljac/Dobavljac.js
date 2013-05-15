Deje_DXMobile.Dobavljac = function (params) {

    var slika = ko.observable();
    var page = ko.observable(0);
    var ponuda = ko.observableArray();
    var loading = ko.observable();

    var ucitaj = function() {
        $.ajax({
            url: Deje_DXMobile.SERVICE_URL + "/VratiDobavljaca",
            data: {
                id : params.id    
            },
            beforeSend: function () { loading(true); }
        }).success(function (dobavljac) {
            slika(dobavljac.Slika);
            ponuda(dobavljac.Ponuda.map(function (item) {
                return {
                    key: item.Kategorija,
                    items: item.Artikli
                };
            }));
        }).complete(function () { loading(false); });
    };

    return {
      page: page,
      navbar: [{text: 'Info', icon: 'info'}, {text: 'Ponuda', icon: 'food'}, {text: 'Kontakt', icon: 'tel'}],
      slika: slika,
      ponuda: ponuda,
      viewShown: ucitaj,
      loading: loading
    };
}