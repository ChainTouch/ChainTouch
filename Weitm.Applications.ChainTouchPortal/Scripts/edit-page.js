﻿// 百度地图API功能
function G(id) {
    return document.getElementById(id);
}

var map = new BMap.Map("Map");
map.centerAndZoom("北京", 12);                   // 初始化地图,设置城市和地图级别。

var ac = new BMap.Autocomplete(//建立一个自动完成的对象
        {
            "input": "suggestId"
            , "location": map
        });

ac.addEventListener("onhighlight", function (e) {  //鼠标放在下拉列表上的事件
    var str = "";
    var _value = e.fromitem.value;
    var value = "";
    if (e.fromitem.index > -1) {
        value = _value.province + _value.city + _value.district + _value.street + _value.business;
    }
    str = "FromItem<br />index = " + e.fromitem.index + "<br />value = " + value;

    value = "";
    if (e.toitem.index > -1) {
        _value = e.toitem.value;
        value = _value.province + _value.city + _value.district + _value.street + _value.business;
    }
    str += "<br />ToItem<br />index = " + e.toitem.index + "<br />value = " + value;
    G("searchResultPanel").innerHTML = str;
});

var myValue;
ac.addEventListener("onconfirm", function (e) {    //鼠标点击下拉列表后的事件
    var _value = e.item.value;
    myValue = _value.province + _value.city + _value.district + _value.street + _value.business;
    G("searchResultPanel").innerHTML = "onconfirm<br />index = " + e.item.index + "<br />myValue = " + myValue;

    setPlace();
});

function setPlace() {
    map.clearOverlays();    //清除地图上所有覆盖物
    function myFun() {
        var pp = local.getResults().getPoi(0).point;    //获取第一个智能搜索的结果
        map.centerAndZoom(pp, 18);
        G("EastLongitude").value = pp.lng;
        G("NorthLatitude").value = pp.lat;
        map.addOverlay(new BMap.Marker(pp));    //添加标注
    }
    var local = new BMap.LocalSearch(map, {//智能搜索
        onSearchComplete: myFun
    });
    local.search(myValue);
}
map.addEventListener("click", function (e) {
    var pointMarker = new BMap.Point(e.point.lng, e.point.lat); // 创建标注的坐标 
    map.clearOverlays();
    map.addOverlay(new BMap.Marker(pointMarker));
    G("EastLongitude").value = pointMarker.lng;
    G("NorthLatitude").value = pointMarker.lat;
});