$(function() {

    var pageInit = function() {
        $("#btnObrisiHistory").click(function () {
            window.localStorage.clear();
            window.location = "#Home";
        });
    };

    var beforePageShow = function() {
        var list = $("#history ul");
        list.empty();
        var str = window.localStorage.getItem(PRETRAGA_ARTIKALA_KEY);
        if (str) {
            var history = JSON.parse(str);
            $.each(history, function (ix, item) {
                list.append("<li>" + "<a href='#Home?artikal=" + item + "'>" + item + "</a></li>");
            });
            list.listview("refresh");
        }
    };

    var pageShow = function () {
        //var list = $("#history ul");
        //var str = window.localStorage.getItem(PRETRAGA_ARTIKALA_KEY);
        //if (str) {
        //    var history = JSON.parse(str);
        //    $.each(history, function (ix, item) {
        //        list.append("<li>" + "<a href='#Home?artikal=" + item +  "'>" +  item + "</a></li>");
        //    });
        //    list.listview("refresh");
        //}
    };

    $("#history").live('pageinit', pageInit)
                 .live('pagebeforeshow', beforePageShow)
                 .live('pageshow', pageShow);
});