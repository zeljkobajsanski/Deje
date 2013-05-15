$(function() {
    
    Deje_DXMobile.app = new DevExpress.framework.html.HtmlApplication({
        ns: Deje_DXMobile,
        viewPortNode: document.getElementById("viewPort"),
        defaultLayout: Deje_DXMobile.config.defaultLayout,
        navigation: Deje_DXMobile.config.navigation
    });

    Deje_DXMobile.app.router.register("Mapa/:tip/:id", { view: "Mapa"});
    Deje_DXMobile.app.router.register(":view/:id", { view: "Home", id: undefined });

    Deje_DXMobile.SERVICE_URL = 'http://mdeje.azurewebsites.net/Home/';
    
    Microsoft.Maps.Pushpin.prototype.Infobox = null;
    Microsoft.Maps.Pushpin.prototype.Id = null;
    Microsoft.Maps.Pushpin.prototype.Naziv = null;
    Microsoft.Maps.Pushpin.prototype.Vrsta = null;

    Deje_DXMobile.ucitajKoordinate = function () {
        navigator.geolocation.getCurrentPosition(function (position) {
            Deje_DXMobile.model.lat(position.coords.latitude);
            Deje_DXMobile.model.lon(position.coords.longitude);
            Deje_DXMobile.model.adress("Vaša pozicija je ažurirana");
        });
    };
});
