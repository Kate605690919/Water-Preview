    //$.get("/area/areatree", {}, function (data) {
    //    //console.log(data);
    //    jstreeArea(data);
    //});
function initTree(url, id, inputid) {
        $.get(url, {}, function (data) {
            //console.log(data);
            jstreeArea(data, id, inputid);
        });
    }

function jstreeArea(data, id, inputid) {
    
    $(id)//#jstreeArea
        //get_node
        .on('changed.jstree', function (e, data) {
            if (data.node) {
                $(inputid).val(data.node.original.text);
                AreaTreeId = data.node.original.id;
            }
        })
        //configure
        .jstree({
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
               "types", "state"   //wholerow扩大点击范围， type用于定义嵌套关系和图标
            ]
        })
}


