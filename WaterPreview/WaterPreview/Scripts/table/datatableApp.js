//initTable();
function initTable(idTable) {
    $(idTable).dataTable({
        pageLength: 25,
        responsive: true,
        dom: '<"html5buttons"B>flipt',
        buttons: [
            { extend: 'copy' },
            { extend: 'excel', title: 'ExampleFile' },
            { extend: 'pdf', title: 'ExampleFile' },

            {
                extend: 'print',
                customize: function (win) {
                    $(win.document.body).addClass('white-bg');
                    $(win.document.body).css('font-size', '10px');
                    $(win.document.body).find('table')
                        .addClass('compact')
                        .css('font-size', 'inherit');
                }
            }
        ],
        "language": {
            "lengthMenu": "每页 _MENU_ 条记录",
            "zeroRecords": "没有找到记录",
            "info": "第 _PAGE_ 页 ( 总共 _PAGES_ 页 )",
            "infoEmpty": "无记录",
            "infoFiltered": "(从 _MAX_ 条记录过滤)",
            "sSearch": "筛选:",
            "oPaginate": {
                "sFirst": "首页",
                "sPrevious": "上页",
                "sNext": "下页",
                "sLast": "末页"
            },
        }
    });
}
//flowmeterDatatable
function flowmeterTable(dataSet, id) {
    for (let i = 0; i < dataSet.length; i++) {
        dataSet[i].time = dataSet[i].time + "";
        if (dataSet[i].time.length == 4) {
            dataSet[i].time = dataSet[i].time.substr(0, 4);
        }
        else if (dataSet[i].time.length == 6) {
            dataSet[i].time = dataSet[i].time.substr(0, 4) + "-" + dataSet[i].time.substr(4, 2);
        }
        else if (dataSet[i].time.length == 8) {
            dataSet[i].time = dataSet[i].time.substr(0, 4) + "-" + dataSet[i].time.substr(4, 2) + "-" + dataSet[i].time.substr(6, 2);
        }
        else if (dataSet[i].time.length == 10) {
            dataSet[i].time = dataSet[i].time.substr(0, 4) + "-" + dataSet[i].time.substr(4, 2) + "-" + dataSet[i].time.substr(6, 2) + " " + dataSet[i].time.substr(8, 2) + ":" + "00";
        }
        else if (data[i].time.length == 12) {
            dataSet[i].time = dataSet[i].time.substr(0, 4) + "-" + dataSet[i].time.substr(4, 2) + "-" + dataSet[i].time.substr(6, 2) + " " + dataSet[i].time.substr(8, 2) + ":" + dataSet[i].time.substr(10, 2);
        }
    }
    if ($(id).hasClass('dataTable')) {
        dttable = $(id).dataTable();
        dttable.fnClearTable(); //清空一下table
        dttable.fnDestroy(); //还原初始化了的datatable
    }
    dttable = $(id).dataTable({
        "data": dataSet,
        "columns": [
            { "title": "抄表时间", data: "time" },
            { "title": "流量", data: "value" }
        ],
        pageLength: 25,
        responsive: true,
        dom: '<"html5buttons"B>lipt',
        buttons: [
            { extend: 'copy' },
            { extend: 'excel', title: 'ExampleFile' },
            { extend: 'pdf', title: 'ExampleFile' },

            {
                extend: 'print',
                customize: function (win) {
                    $(win.document.body).addClass('white-bg');
                    $(win.document.body).css('font-size', '10px');

                    $(win.document.body).find('table')
                        .addClass('compact')
                        .css('font-size', 'inherit');
                }
            }
        ],
        "language": {
            "lengthMenu": "每页 _MENU_ 条记录",
            "zeroRecords": "没有找到记录",
            "info": "第 _PAGE_ 页 ( 总共 _PAGES_ 页 )",
            "infoEmpty": "无记录",
            "infoFiltered": "(从 _MAX_ 条记录过滤)",
            "sSearch": "筛选:",
            "oPaginate": {
                "sFirst": "首页",
                "sPrevious": "上页",
                "sNext": "下页",
                "sLast": "末页"
            },
        }
    });
}
//pressuremeterDatatable
function pressuremeterTable(dataSet, id) {
    if ($(id).hasClass('dataTable')) {
        dttable = $(id).dataTable();
        dttable.fnClearTable(); //清空一下table
        dttable.fnDestroy(); //还原初始化了的datatable
    }
    dttable = $(id).dataTable({
        "data": dataSet,
        "columns": [
            { "title": "抄表时间", data: "time" },
            { "title": "时点值(Mpa)", data: " " },  //待加入的数值
            { "title": "最小值(Mpa)", data: " " },
            { "title": "最大值(Mpa)", data: " " },
            { "title": "平均值(Mpa)", data: " " }
        ],
        pageLength: 25,
        responsive: true,
        dom: '<"html5buttons"B>t',
        buttons: [
            { extend: 'copy' },
            { extend: 'excel', title: 'ExampleFile' },
            { extend: 'pdf', title: 'ExampleFile' },

            {
                extend: 'print',
                customize: function (win) {
                    $(win.document.body).addClass('white-bg');
                    $(win.document.body).css('font-size', '10px');

                    $(win.document.body).find('table')
                        .addClass('compact')
                        .css('font-size', 'inherit');
                }
            }
        ],
        "language": {
            "lengthMenu": "每页 _MENU_ 条记录",
            "zeroRecords": "没有找到记录",
            "info": "第 _PAGE_ 页 ( 总共 _PAGES_ 页 )",
            "infoEmpty": "无记录",
            "infoFiltered": "(从 _MAX_ 条记录过滤)",
            "sSearch": "筛选:",
            "oPaginate": {
                "sFirst": "首页",
                "sPrevious": "上页",
                "sNext": "下页",
                "sLast": "末页"
            },
        }
    });
}