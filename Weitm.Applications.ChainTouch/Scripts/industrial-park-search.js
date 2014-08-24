﻿$(document).ready(function () {
    var points = [];
    //initialize();
    // 百度地图API功能
    var map = new BMap.Map("property_map");                        // 创建Map实例
    map.centerAndZoom(new BMap.Point(116.404, 39.915), 11);     // 初始化地图,设置中心点坐标和地图级别
    map.addControl(new BMap.NavigationControl());               // 添加平移缩放控件
    map.addControl(new BMap.ScaleControl());                    // 添加比例尺控件
    map.addControl(new BMap.OverviewMapControl());              //添加缩略地图控件
    map.enableScrollWheelZoom();                            //启用滚轮放大缩小
    map.addControl(new BMap.MapTypeControl());          //添加地图类型控件
    map.setCurrentCity("北京");          // 设置地图显示的城市 此项是必须设置的

    //var customLayer = new BMap.CustomLayer({
    //    geotableId: 76158,
    //    q: '',
    //    tags: '', 
    //    filter: ''
    //});
    //map.addTileLayer(customLayer);
    //customLayer.addEventListener('hotspotclick', callback);

    function callback(e) {
        var customPoi = e.customPoi;
        var contentPoi = e.content;
        var content = '<p style="width:280px;margin:0;line-height:20px;">Name:' + customPoi.title  +  '<br/>Address:' + customPoi.address  + '<br/><a href="' + baseUrl + "IndustrialPark/Detail/"+ contentPoi.industrial_park_id + '">Detail</a></p>';
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

    $("#Search").click(function () {
        $('#industrial-park-search').ajaxSubmit({
            url: baseUrl + 'ApiData/IndustrialParkSearch/',
            method: "POST",
            success: function (data) {
                map.clearOverlays();
                points.length = 0;
                $.each(eval(data), function (index, element) {
                    addMarker(element.lon, element.lat, element.name, element.address, element.year, element.count, element.id);
                });
                map.setViewport(points);
            }
        });
        event.preventDefault();
    });

    function addMarker(lon, lat, name, address, year, count, id) {
        var point = new BMap.Point(lon, lat);
        points.push(point);
        var marker = new BMap.Marker(point, name);
        var sContent =
        "<div style='width:400px;height:180px'><h4 style='margin:0 0 5px 0;padding:0.2em 0'>" + name + "</h4>" +
        "<img style='float:right;margin:4px' id='imgDemo' src='http://app.baidu.com/map/images/tiananmen.jpg' width='139' height='104' title='" + name + "'/>" +
        "<p style='margin:0;line-height:1.5;font-size:13px;'>Address:" + address + "</p>" +
        "<p style='margin:0;line-height:1.5;font-size:13px;'>Established Year:" + year + "</p>" +
        "<p style='margin:0;line-height:1.5;font-size:13px;'>Properties:" + count + "</p>" +
        "<p style='margin:0;line-height:1.5;font-size:13px;'><a href='" + baseUrl + "IndustrialPark/Detail/" + id + "'>View Detail</a></p>" +
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

});