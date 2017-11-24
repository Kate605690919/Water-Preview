function initMap(idMap){
// 百度地图API功能
	var map = new BMap.Map(idMap);
	var point = new BMap.Point(114.09, 22.63);
	map.centerAndZoom(point, 15);            //地图初始化（设置point为中心点坐标，15为地图级别）
	map.setCurrentCity("深圳");

	/*添加文本信息框部分*/
    var data_info = [[114.09, 22.63, "<h4>" + "00T0238" + " " +"<small>" + "旺塘泵站" + "</small>" + "</h4>"  + "<br>" + "<p>" + "流量正常 信号正常" + "</p>" + "<br>" + "<a  class=\"btn btn-primary\">详情</a>"],
					 [114.08, 22.66, "<h4>" + "00T0620" + " " + "<small>" + "佳兆业-城市广场" + "</small>" + "</h4>" + "<br>" + "<p>" + "流量正常 信号正常" + "</p>" + "<br>" + "<a  class=\"btn btn-primary\">详情</a>"],
					 [114.14, 22.63, "<h4>" + "00T0640" + " " + "<small>" + "慢城金积嘉" + "</small>" + "</h4>" + "<br>" + "<p>" + "流量正常 信号正常" + "</p>" + "<br>" + "<a  class=\"btn btn-primary\">详情</a>"]
					];
	var opts = {
				width : 250,     // 信息窗口宽度
				height: 140,     // 信息窗口高度
				//title : "信息窗口" , // 信息窗口标题
				enableMessage:true//设置允许信息窗发送短息
			   };
	for(var i=0;i<data_info.length;i++){
		var marker = new BMap.Marker(new BMap.Point(data_info[i][0],data_info[i][1]));  // 创建标注
		var content = data_info[i][2];
		map.addOverlay(marker);               // 将标注添加到地图中
		addClickHandler(content,marker);
	}
	function addClickHandler(content,marker){
		marker.addEventListener("click",function(e){
			openInfo(content,e)}
		);
	}
	function openInfo(content,e){
		var p = e.target;
		var point = new BMap.Point(p.getPosition().lng, p.getPosition().lat);
		var infoWindow = new BMap.InfoWindow(content,opts);  // 创建信息窗口对象 
		map.openInfoWindow(infoWindow,point); //开启信息窗口
	}

	// map.addControl(new BMap.NavigationControl());       //添加平移缩放控件
	map.addControl(new BMap.ScaleControl());               // 添加比例尺控件
	//map.addControl(new BMap.OverviewMapControl());         //添加缩略地图控件
	map.enableScrollWheelZoom();                           //启用滚轮放大缩小
	//map.addControl(new BMap.MapTypeControl());             //添加地图类型控件
	//map.disable3DBuilding();	 
	map.setMapStyle({style:'grayscale'});                  //设置地图样式

}
