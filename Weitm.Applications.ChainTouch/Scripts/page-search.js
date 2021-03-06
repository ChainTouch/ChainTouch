﻿var points = [];
var myValue;
var map = new BMap.Map("map_canvas");

var ac = new BMap.Autocomplete(
{
    "input": "bydistance-district",
    "location": map
});

function move(lat, lon) {
    map.panTo(new BMap.Point(lon, lat));
}

function update(e) {
    var overlays = map.getOverlays();
    var pp = map.getCenter();
    $(".current-lat").val(pp.lat);
    $(".current-lon").val(pp.lng);
}

function callback(e) {
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
    else {
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

function favorite(propertyId) {
    $.get(baseUrl + "ApiData/AddFavorite/" + propertyId + "/");
}

function addMarker(lon, lat, name, rent, area, size, cost, id) {
    var point = new BMap.Point(lon, lat);
    points.push(point);
    var marker = new BMap.Marker(point, name);
    var sContent =
    "<div style='width:400px;height:180px'><h4 style='margin:0 0 5px 0;padding:0.2em 0'>" + name + "</h4>" +
    "<img style='float:right;margin:4px' id='imgDemo' src='http://app.baidu.com/map/images/tiananmen.jpg' width='139' height='104' title='" + name + "'/>" +
    "<p style='margin:0;line-height:1.5;font-size:13px;'>Rent:" + rent + "</p>" +
    "<p style='margin:0;line-height:1.5;font-size:13px;'>Leasable Area:" + area + "</p>" +
    "<p style='margin:0;line-height:1.5;font-size:13px;'>Total Land Size:" + size + "</p>" +
    "<p style='margin:0;line-height:1.5;font-size:13px;'>Management Cost:" + cost + "</p>" +
    "<p style='margin:0;line-height:1.5;font-size:13px;'><a href='" + baseUrl + "Property/Detail/" + id + "'>View Detail</a></p>" +
    "</div>";

    var infoWindow = new BMap.InfoWindow(sContent);

    marker.openInfoWindow(infoWindow);

    $("#item-" + id).hover(function () {
        marker.openInfoWindow(infoWindow);
    });

    marker.addEventListener("click", function () {
        marker.openInfoWindow(infoWindow);
    });

    marker.addEventListener("mouseover", function () {
        marker.openInfoWindow(infoWindow);
    });

    map.addOverlay(marker);

    //map.centerAndZoom(point, 13);
}

function unfavorite(propertyId) {
    $.get(baseUrl + "ApiData/RemoveFavorite/" + propertyId + "/");
}


function districtSearch() {
    $('#bydistrict-form').ajaxSubmit({
        url: baseUrl + 'ApiData/Local/',
        method: "POST",
        success: function (data) {
            map.clearOverlays();
            points.length = 0;
            $.each(eval(data), function (index, element) {
                addMarker(element.lon, element.lat, element.name, element.rent, element.area, element.size, element.cost, element.id);
            });
            map.setViewport(points);
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
    myGeo.getPoint(province, function (point) {
        if (point) {
            map.centerAndZoom(point, 13);
            map.addOverlay(new BMap.Marker(point));
        }
    }, province);
}

function distanceSearch() {
    var searchResult = $("#searchResultPanel").html();
    var searchDistrict = $("#bydistance-district").val();

    if (searchResult != searchDistrict) {
        //地点不一致，触发地点搜索先
        var local = new BMap.LocalSearch(map, {
            renderOptions: {
                map: map,
                autoViewport: true
            },
            onSearchComplete: function() {
                var pp = local.getResults().getPoi(0).point;
                $(".current-lat").val(pp.lat);
                $(".current-lon").val(pp.lng);

                $('#bydistance-form').ajaxSubmit({
                    url: baseUrl + 'ApiData/Nearby/',
                    method: "POST",
                    success: function (data) {
                        map.clearOverlays();
                        points.length = 0;
                        $.each(eval(data), function (index, element) {
                            addMarker(element.lon, element.lat, element.name, element.rent, element.area, element.size, element.cost, element.id);
                            
                        });
                        map.setViewport(points);
                    }
                });

                $('#bydistance-form').ajaxSubmit({
                    url: baseUrl + 'ApiData/NearbyHtml/',
                    method: "POST",
                    success: function (html) {
                        $("#search-result-panel").html(html);
                    }
                });
            }
        });
        local.search($("#bydistance-district").val());
    }
    else {
        $('#bydistance-form').ajaxSubmit({
            url: baseUrl + 'ApiData/Nearby/',
            method: "POST",
            success: function (data) {
                map.clearOverlays();
                $.each(eval(data), function (index, element) {
                    addMarker(element.lon, element.lat, element.name, element.rent, element.area, element.size, element.cost, element.id);
                    if (index == 0) {
                        map.setZoom(13)
                        map.panTo(new BMap.Point(element.lon, element.lat));
                    }
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
    }

    //$("#search-result-panel").load(baseUrl + "ApiData/LocalHtml/" + province, null, null);

    //var province = $("#bydistance-province").find("option:selected").text();
    //var district = $("#bydistance-district").val();
    //var myGeo = new BMap.Geocoder();
    //// 将地址解析结果显示在地图上，并调整地图视野  
    //myGeo.getPoint(district, function (point) {
    //    if (point) {
    //        map.centerAndZoom(point, 13);
    //        map.addOverlay(new BMap.Marker(point));
    //    }
    //}, province);
}

function doSearch() {
    if ($("#bydistrict").hasClass("active")) {
        districtSearch();
    }
    else if ($("#bydistance").hasClass("active")) {
        distanceSearch();
    }
}

function clearSortRent() {
    $("#sort-rent").text("Rent");
    $("#sort-rent").data("type", "");
    $(".sort-rent").val("");
}
function clearSortArea() {
    $("#sort-area").text("Area");
    $("#sort-area").data("type", "");
    $(".sort-area").val("");
}
function clearSortSize() {
    $("#sort-size").text("Size");
    $("#sort-size").data("type", "");
    $(".sort-size").val("");
}
function clearSortCost() {
    $("#sort-cost").text("Management Cost");
    $("#sort-cost").data("type", "");
    $(".sort-cost").val("");
}

function G(id) {
    return document.getElementById(id);
}


function setPlace() {
    map.clearOverlays();    //清除地图上所有覆盖物
    function myFun() {
        var pp = local.getResults().getPoi(0).point;    //获取第一个智能搜索的结果
        $(".current-lat").val(pp.lat);
        $(".current-lon").val(pp.lng);
        map.centerAndZoom(pp, 18);
        map.addOverlay(new BMap.Marker(pp));    //添加标注

        //distanceSearch();
    }
    var local = new BMap.LocalSearch(map, { //智能搜索
        onSearchComplete: myFun
    });
    local.search(myValue);
}

$(document).ready(function () {

    map.centerAndZoom(new BMap.Point(121.5155850000, 31.3095330000), 13);
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
    update();

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
    });

    $(".unfavorite-button").click(function () {
        $(this).addClass('favorite-button');
        $(this).removeClass('unfavorite-button');
        unfavorite($(this).data("property-id"));
    });


    $(".property-result-item").on("mouseover",function () {
        var propertyId = $(this).data("property-id");
        var lat = $(this).data("lat");
        var lon = $(this).data("lon");
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
        clearSortArea();
        clearSortCost();
        clearSortSize();
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
        clearSortRent();
        clearSortCost();
        clearSortSize();
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
        clearSortArea();
        clearSortCost();
        clearSortRent();
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
        clearSortArea();
        clearSortRent();
        clearSortSize();
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

    ac.addEventListener("onhighlight", function (e) {
        var str = "";
        var _value = e.fromitem.value;
        var value = "";
        if (e.fromitem.index > -1) {
            value = _value.province + _value.city + _value.district + _value.street + _value.business;
        }
        str = value;

        value = "";
        if (e.toitem.index > -1) {
            _value = e.toitem.value;
            value = _value.province + _value.city + _value.district + _value.street + _value.business;
        }
        str += value;
        G("searchResultPanel").innerHTML = str;
    });

    ac.addEventListener("onconfirm", function (e) {    //鼠标点击下拉列表后的事件
        var _value = e.item.value;
        myValue = _value.province + _value.city + _value.district + _value.street + _value.business;
        G("searchResultPanel").innerHTML = myValue;

        setPlace();
    });

    if ($("#bydistance-district").val() != "") {
        distanceSearch();
    }

    ac.setInputValue(initValue);
});
