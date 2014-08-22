
var map = new BMap.Map("map_canvas");
map.centerAndZoom(new BMap.Point(121.5155850000, 31.3095330000), 16);
map.addControl(new BMap.NavigationControl()); 
map.addControl(new BMap.ScaleControl());
map.addControl(new BMap.OverviewMapControl());
map.enableScrollWheelZoom(); 
map.addControl(new BMap.MapTypeControl());
map.setCurrentCity("上海"); 
////显示全部麻点会降低速度，先备注，看性能再定夺。
//var customLayer = new BMap.CustomLayer({
//    geotableId: 75115,
//    q: '',
//    tags: '', 
//    filter: ''
//});
//map.addTileLayer(customLayer);
//customLayer.addEventListener('hotspotclick', callback);
map.addEventListener('movestart', update)

function update(e) {
    var overlays = map.getOverlays();
    //console.log(overlays);
}

function callback(e)//单击热点图层
{
    var customPoi = e.customPoi;
    var contentPoi = e.content;
    $("#info-property").html(contentPoi.property_name);
    $("#info-sponsor").html(contentPoi.sponsor_name);
    var content = '<p style="width:280px;margin:0;line-height:20px;">Sponsor：' + contentPoi.sponsor_name + '<br/>Property:' + contentPoi.property_name + '<br/><a href="@Url.Content("~/Property/Detail/")' + contentPoi.property_id + '">Detail</a></p>';
    var searchInfoWindow = new BMapLib.SearchInfoWindow(map, content, {
        title: customPoi.title,
        width: 290,
        height: 40,
        panel: "panel",
        enableAutoPan: true,
        enableSendToPhone: true,
        searchTypes: [
            BMAPLIB_TAB_SEARCH, 
            BMAPLIB_TAB_TO_HERE, 
            BMAPLIB_TAB_FROM_HERE 
        ]
    });
    var point = new BMap.Point(customPoi.point.lng, customPoi.point.lat);
    searchInfoWindow.open(point);
}

function buildCity(province, target) {
    if (province == 'any') {
        target.html('');
        target.append("<option value='any'>City - Any</option>");
    }
    else{
        target.html('');
        target.append("<option value='loading...'>loading...</option>");
        $.ajax({
            type: "GET",
            url: baseUrl + "ApiData/Cities/" + province,
            success: function (data) {
                target.html('');
                target.append("<option value='any'>City - Any</option>");
                $.each(eval(data), function (index, element) {
                    target.append("<option value='" + element.value + "'>" + element.text + "</option>");
                });
            }
        });
    }
}

function buildArea(city, target) {
    if (city == 'any') {
        target.html('');
        target.append("<option value='any'>District - Any</option>");
    }
    else {
        target.html('');
        target.append("<option value='loading...'>loading...</option>");
        $.ajax({
            type: "GET",
            url: baseUrl + "ApiData/Areas/" + city,
            success: function (data) {
                target.html('');
                target.append("<option value='any'>District - Any</option>");
                $.each(eval(data), function (index, element) {
                    target.append("<option value='" + element.value + "'>" + element.text + "</option>");
                });
            }
        });
    }
}

$(document).ready(function () {
    $("#bydistrict-province").change(function () {
        buildCity($("#bydistrict-province").val(), $("#bydistrict-city"))
    });

    $("#bydistrict-city").change(function () {
        buildArea($("#bydistrict-city").val(), $("#bydistrict-district"))
    });

    $(".favorite-button").click(function () {
        $(this).addClass('unfavorite-button');
        $(this).removeClass('favorite-button');
        favorite($(this).data("property-id"));
        //console.log($(this).data("property-id"));
    });

    $(".unfavorite-button").click(function () {
        $(this).addClass('favorite-button');
        $(this).removeClass('unfavorite-button');
        unfavorite($(this).data("property-id"));
        //console.log($(this).data("property-id"));
    });

    function favorite(propertyId)
    {
        $.get(baseUrl + "ApiData/AddFavorite/" + propertyId + "/");
    }

    function addMarker(lon, lat, name) {
        //console.log(lon + ',' + ',' + lat + ',' + name);
        var point = new BMap.Point(lon, lat);
        var marker = new BMap.Marker(point, name);
        var infoWindow = new BMap.InfoWindow(name);
        marker.addEventListener("click", function () { this.openInfoWindow(infoWindow); });
        map.addOverlay(marker);

        map.centerAndZoom(point, 16);
    }

    function unfavorite(propertyId)
    {
        $.get(baseUrl + "ApiData/RemoveFavorite/" + propertyId + "/");
    }


    function districtSearch() {
        //map.removeTileLayer(customLayer);//隐藏麻点图

        $('#bydistrict-form').ajaxSubmit({
            url: baseUrl + 'ApiData/Local/',
            method: "POST",
            success: function (data) {
                map.clearOverlays();
                $.each(eval(data), function (index, element) {
                    addMarker(element.lon, element.lat, element.name);
                });
            }
        });

        $('#bydistrict-form').ajaxSubmit({
            url: baseUrl + 'ApiData/LocalHtml/',
            method: "POST",
            success: function (html) {
                $("#search-result-panel").html(html);
            }
        });

        var province = $("#bydistrict-province").find("option:selected").text();
        map.setCurrentCity(province);
        var myGeo = new BMap.Geocoder();
        // 将地址解析结果显示在地图上，并调整地图视野  
        myGeo.getPoint(province, function (point) {
            if (point) {
                map.centerAndZoom(point, 15);
                map.addOverlay(new BMap.Marker(point));
            }
        }, province);
    }


    function distanceSearch() {
        //map.removeTileLayer(customLayer);//隐藏麻点图
        $('#bydistance-form').ajaxSubmit({
            url: baseUrl + 'ApiData/Nearby/',
            method: "POST",
            success: function (data) {
                map.clearOverlays();
                $.each(eval(data), function (index, element) {
                    addMarker(element.lon, element.lat, element.name);
                });
            }
        });

        $('#bydistance-form').ajaxSubmit({
            url: baseUrl + 'ApiData/NearbyHtml/',
            method: "POST",
            success: function (html) {
                $("#search-result-panel").html(html);
            }
        });

        //$("#search-result-panel").load(baseUrl + "ApiData/LocalHtml/" + province, null, null);

        var province = $("#bydistance-province").find("option:selected").text();
        var district = $("#bydistance-district").val();
        var myGeo = new BMap.Geocoder();
        // 将地址解析结果显示在地图上，并调整地图视野  
        myGeo.getPoint(district, function (point) {
            if (point) {
                map.centerAndZoom(point, 16);
                map.addOverlay(new BMap.Marker(point));
            }
        }, province);
    }

    function doSearch() {
        if ($("#bydistrict").hasClass("active")) {
            districtSearch();
        }
        else if ($("#bydistance").hasClass("active")) {
            distanceSearch();
        }
    }

    $(".property-result-item").click(function () {
        var propertyId = $(this).data("property-id");
        var lat = $(this).data("lat");
        var lon = $(this).data("lon");
        map.panTo(new BMap.Point(lon, lat)); 
    });

    $("#sort-rent").click(function () {
        var type = $("#sort-rent").data("type");
        if (type == "")        {
            $("#sort-rent").text("Rent ↑");
            $("#sort-rent").data("type", "↑");
            $(".sort-rent").val("asc");
        }
        else if (type == "↑")        {
            $("#sort-rent").text("Rent ↓");
            $("#sort-rent").data("type", "↓");
            $(".sort-rent").val("desc");
        }
        else if (type == "↓")        {
            $("#sort-rent").text("Rent");
            $("#sort-rent").data("type", "");
            $(".sort-rent").val("");
        }
        doSearch();
    });
    $("#sort-area").click(function () {
        var type = $("#sort-area").data("type");
        if (type == ""){
            $("#sort-area").text("Area ↑");
            $("#sort-area").data("type", "↑");
            $(".sort-area").val("asc");
        }
        else if (type == "↑")        {
            $("#sort-area").text("Area ↓");
            $("#sort-area").data("type", "↓");
            $(".sort-area").val("desc");
        }
        else if (type == "↓") {
            $("#sort-area").text("Area");
            $("#sort-area").data("type", "");
            $(".sort-area").val("");
        }
        doSearch();
    });
    $("#sort-size").click(function () {
        var type = $("#sort-size").data("type");
        if (type == "") {
            $("#sort-size").text("Size ↑");
            $("#sort-size").data("type", "↑");
            $(".sort-size").val("asc");
        }
        else if (type == "↑") {
            $("#sort-size").text("Size ↓");
            $("#sort-size").data("type", "↓");
            $(".sort-size").val("desc");
        }
        else if (type == "↓") {
            $("#sort-size").text("Size");
            $("#sort-size").data("type", "");
            $(".sort-size").val("");
        }
        doSearch();
    });
    $("#sort-cost").click(function () {
        var type = $("#sort-cost").data("type");
        if (type == "") {
            $("#sort-cost").text("Managment Cost ↑");
            $("#sort-cost").data("type", "↑");
            $(".sort-cost").val("asc");
        }
        else if (type == "↑") {
            $("#sort-cost").text("Managment Cost ↓");
            $("#sort-cost").data("type", "↓");
            $(".sort-cost").val("desc");
        }
        else if (type == "↓") {
            $("#sort-cost").text("Managment Cost");
            $("#sort-cost").data("type", "");
            $(".sort-cost").val("");
        }
        doSearch();
    });


    $("#bydistrict-button").click(function () {
        districtSearch();
        event.preventDefault();
    });

    $("#bydistance-button").click(function () {
        distanceSearch();
        event.preventDefault();
    });
});