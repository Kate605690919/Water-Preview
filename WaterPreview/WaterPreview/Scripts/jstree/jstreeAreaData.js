    $.get("/area/areatree", {}, function (data) {
        console.log(data);
        jstreeArea(data);
        deviceTree(data,"#devicetree")
    });

function jstreeArea(data) {
    $("#jstreeArea").jstree({
            "core": {                //所有的默认设置
                "animation": 0,          //打开或关闭动画持续的时间，默认为200
                "check_callback": false,  //允许所有的都交互操作（creat,rename,delete,move,copy）
                "themes": { "stripes": false },  //stripes为一个布尔值
                'data': data
            },
            "types": {  //存储所有类型为键值对（key value paris）的对象
                "#": {
                    "max_children": 4, //可以拥有的最大的直接子节点数，-1表示无限制（最好不要设为-1）
                    "max_depth": 5,    //可以拥有的最大嵌套数 ，1表示只有children而不能有grandchildren
                    "valid_children": ["root"],  //可以作为子节点的字符串数组
                },
                "root": {
                    "icon": "glyphicon glyphicon-paperclip",
                    "valid_children": ["default"]
                },
                "default": {
                    "icon": false
                }
            },
            "plugins": [
               "types", "state"    //wholerow扩大点击范围， type用于定义嵌套关系和图标
            ]
        })
}

function deviceTree(data, id) {
    $(id).jstree({
        "core": {                //所有的默认设置
            "animation": 0,          //打开或关闭动画持续的时间，默认为200
            "check_callback": true,  //允许所有的都交互操作（creat,rename,delete,move,copy）
            "themes": { "stripes": false },  //stripes为一个布尔值
            'data': data
        },
        "types": {  //存储所有类型为键值对（key value paris）的对象
            "#": {
                "max_children": 4, //可以拥有的最大的直接子节点数，-1表示无限制（最好不要设为-1）
                "max_depth": 5,    //可以拥有的最大嵌套数 ，1表示只有children而不能有grandchildren
                "valid_children": ["root"],  //可以作为子节点的字符串数组
            },
            "root": {
                "icon": "glyphicon glyphicon-paperclip",
                "valid_children": ["default"]
            },
            "default": {
                "icon": false
            }
        },
        "plugins": [
           "types", "state", "search"    //wholerow扩大点击范围， type用于定义嵌套关系和图标
        ]
    })
    $(id).on("changed.jstree", function (e, data) {
        console.log(data.selected);
        //var subTree = document.createElement("div");
        //subTree.id = 'subtree';
        //$("#" + data.selected).append("<div id='subtree'></div>");
        //addTree();
        //$("#" + data.selected).on("load",'#subtree', function () {
        data.selected = data.selected.length == 0 ? '6f6b8db5-1202-4644-b1b2-a52284d73e07' : data.selected;
        var anchor = $("#" + data.selected + "_anchor");
        var rowT = $('.breadcrumb+.row').offset().top;
        var widthAnchor = anchor.width();
        $("#subtree").jstree({
            "core": {                //所有的默认设置
                "animation": 0,          //打开或关闭动画持续的时间，默认为200
                "check_callback": true,  //允许所有的都交互操作（creat,rename,delete,move,copy）
                "themes": { "stripes": false },  //stripes为一个布尔值
                'data': [{ text: "流量计1", description: "flowmeter", id: "fm" }, { text: "流量计2", description: "flowmeter2", id: "fm2" }]
            },
            "types": {  //存储所有类型为键值对（key value paris）的对象
                "#": {
                    "max_children": 4, //可以拥有的最大的直接子节点数，-1表示无限制（最好不要设为-1）
                    "max_depth": 5,    //可以拥有的最大嵌套数 ，1表示只有children而不能有grandchildren
                    "valid_children": ["root"],  //可以作为子节点的字符串数组
                },
                "root": {
                    "icon": "glyphicon glyphicon-paperclip",
                    "valid_children": ["default"]
                },
                "default": {
                    "icon": false
                }
            },
            "plugins": [
               "types", "state"    //wholerow扩大点击范围， type用于定义嵌套关系和图标
            ]
        });
        var top = anchor.offset().top - rowT + 0.16 * widthAnchor;
        var left = anchor.offset().left + widthAnchor;
        $('#subtree').css({ left: left, top: top });
        //$('.jstree-node').css({ filter: alpha(opacity=45), opacity: 0.45});
     });
}



