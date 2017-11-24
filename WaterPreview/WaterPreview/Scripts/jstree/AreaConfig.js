$.get("/area/areatree", {}, function (data) {
    jstreeAreaConfig(data);
});
function jstreeAreaConfig(data) {
    $('#jstreeAreaConfig')
        //get_node
        .on("changed.jstree", function (e, data) {
            if (data.node) {
                document.querySelector('#areaSub1').innerHTML = data.node.original.text;
                document.querySelector('#areaSub2').innerHTML = data.node.original.code;
                document.querySelector('#areaSub3').innerHTML = data.node.original.description;
                document.querySelector('#areaSub4').innerHTML = data.node.original.isleaf;
            }
        })
        //configure
        .jstree({
        "core": {                //所有的默认设置
            "animation": 0,          //打开或关闭动画持续的时间，默认为200
            "check_callback": true,  //允许所有的都交互操作（creat,rename,delete,move,copy）
            "themes": { "stripes": true },  //stripes为一个布尔值
            'data': data
        },
        "types": {  //存储所有类型为键值对（key value paris）的对象
            "#": {
                "max_children": 4, //可以拥有的最大的直接子节点数，-1表示无限制（最好不要设为-1）
                "max_depth": 5,    //可以拥有的最大嵌套数 ，1表示只有children而不能有grandchildren
                "valid_children": ["root", "meter"],  //可以作为子节点的字符串数组
                "icon": "glyphicon glyphicon-flag"
            },
            "root": {
                "icon": "glyphicon glyphicon-paperclip",
                "valid_children": ["default"]
            },
            "default": {
                "icon": false
            },
            "meter": {
                "icon": "glyphicon glyphicon-dashboard",
                "valid_children": []
            }
        },
        "plugins": [
          "contextmenu", "dnd", "search",  //contextmenu表示可以展开收起，dnd表示可以拖动改变层次关系，search表示筛选搜索框
          "state", "types"     //wholerow扩大点击范围， type用于定义嵌套关系和图标
        ]
    });
}
