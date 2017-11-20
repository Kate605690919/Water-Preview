class Table extends React.Component {
    constructor(props) {
        super(props);
    }

    componentDidMount() {
        initTable(
            this.props.tableInfo
        );
    }
    shouldComponentUpdate() {
        debugger;
    }
    componentWillUpdate() {
        initTable(
            this.props.tableInfo
        );
    }

    shouldComponentUpdate(nextProps, nextState) {
        //return (nextProps.tableInfo !== this.props.tableInfo);
        return true;
    }

    render() {
        return (
            <div className="react-Table">
                <table className="table table-striped table-hover animated fadeInRight" id={this.props.tableInfo.el.substr(1)}></table>
            </div>
        );
    }
}

//{el:element,columns:[],url:str}
function initTable({ el, columns, url, columnDefs, event, data }) {
    if (!!url) {
        $.ajax({
            type: "POST",
            url: url,
            dataType: "json",
            columnDefs: columnDefs,
            success: function (data) {
                initDataTable(el, columns, event, data);
            }
        });
    } else if (!!data) {
        initDataTable(el, columns, event, data);
    }
}
function initDataTable(el, columns, event, data) {
    event = event || { initComplete: () => { console.log('initComplete') } };
    var table_list = $(el).on('xhr.dt', function (e, settings, json, xhr) {
        const button = document.getElementById('detail-wrapper-detailTable');
        button.click();
        console.log('bbb');
    }).DataTable({
        data: data,
        destroy: true,
        columns: columns,
        pageLength: 10,
        dom: '<"html5buttons"B>fltp',
        lengthMenu: [10, 25, 50, 100],
        initComplete: event.initComplete,
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
        language: {
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