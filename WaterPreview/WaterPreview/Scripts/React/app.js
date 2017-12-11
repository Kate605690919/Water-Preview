const { Router, Route, IndexRoute, hashHistory, Link } = ReactRouter;
const { Provider } = ReactRedux;
const { createStore, combineReducers, applyMiddleware, compose } = Redux;

let AreaTreeId = null;
let changeEvt = {
    name: 'loaded',
    func: () => { }
}
function deleteClient(id) {
    const button = document.getElementById('table-wrapper-detailTable');
    button.setAttribute('data-id', id);
    button.click();
}
function detailClient(uid, id) {
    const button = document.getElementById('detail-wrapper-detailTable');
    button.setAttribute('data-uid', uid);
    button.setAttribute('data-id', id);
    button.click();
}
function dateFormat(data, trail) {
    let d = parseInt(data.substring(6, data.length - trail));
    d = new Date(d);
    let ar_date = [d.getFullYear(), d.getMonth() + 1, d.getDate()];
    function dFormat(i) {
        return i < 10 ? "0" + i.toString() : i;
    }
    for (var i = 0; i < ar_date.length; i++) ar_date[i] = dFormat(ar_date[i]);
    return ar_date.join('/');
}
const DeviceInfo = {
    home: {
        DeviceViewCount: {
            flowCount: localStorage.getItem('flowCount') || 3,
            pressureCount: localStorage.getItem('pressureCount') || 2
        },
        DeviceViewRank: {
            renderFlag: false
        },
        editDeviceViewCount: {
            formId: 'DeviceViewCount',
            itemInfo: [{
                id: 0, label: '流量计', input: { name: 'Flow_Count', value: 'flowCount' }
            }, {
                id: 1, label: '压力计', input: { name: 'Pressure_Count', value: 'pressureCount' }
            }]
        }
    },
    areaUid: '',
    device: ['FM', 'PM'],
    jsTreeInfo: {
        url: '/area/areatree',
        elId: 'jstreeArea',
        event: {
            name: 'select_node',
            function: (e, data) => {
                if (e) {
                    let a = document.getElementById('areaUid');
                    a.setAttribute('data-uid', data.selected[0]);
                    a.click();
                }
            }
        }
    },
    toolBar: [{
        value: 'FM', label: '流量计', name: 'Meter'
    }, {
        value: 'PM', label: '压力计', name: 'Meter'
    }, {
        value: 'QM', label: '水质计', name: 'Meter'
    }],
    FM: {
        status: Status.LOADING,
        tableInfo: {
            el: '#wrapper-FlowMeterTable',
            columns: [
                {
                    "data": "flowmeter.FM_Code", "title": "流量计编码",
                    render: function (data, type, full, meta) {
                        return '<a href="#/flowMeter/detail/uid=' + full.flowmeter.FM_UId + '">' + data + '</a>';
                    }
                },
                { "data": "flowmeter.FM_Description", "title": "描述" },
                { "data": "area.Ara_Name", "title": "区域" },
                {
                    "data": "flowmeter.FM_FlowCountLast", "title": "更新",
                    render: function (data, type, full, meta) {
                        return dateFormat(data, 7);
                    }
                },
                { "data": "status.FMS_FlowValue", "title": "行度" },
                { "data": "status.FMS_MainBatteryStatus", "title": "主电池" },
                { "data": "status.FMS_ModemBatteryStatus", "title": "通信电池" },
                { "data": "status.FMS_SecondaryBatteryStatus", "title": "备用电池" },
                { "data": ".status.FMS_AntennaSignal", "title": "信号" },
                { "data": "FMS_AntennaSignal", "defaultContent": "未知", "title": "操作" }
            ]
        },
        header: {
            title: [{
                content: '流量计'
            }]
        }
    },
    PM: {
        status: Status.LOADING,
        tableInfo: {
            el: '#wrapper-pressureTable',
            columns: [
                {
                    "data": "pressuremeter.PM_Code", "title": "压力计编码",
                    render: function (data, type, full, meta) {
                        return '<a href="#/pressuremeter/detail/pmUid=' + full.pressuremeter.PM_UId + '">' + data + '</a>';
                    }
                },
                { "data": "pressuremeter.PM_Description", "defaultContent": "未知", "title": "描述" },
                {
                    "data": "status.PMS_UpdateDt", "title": "更新",
                    render: function (data, type, full, meta) {
                        return dateFormat(data, 7);
                    }
                },
                { "data": "status.PMS_PressureValue", "title": "实时值" },
                { "data": "FMS_AntennaSignal", "defaultContent": "未知", "title": "操作" }
            ]
        },
        header: {
            title: [{
                content: '压力计'
            }]
        }
    },
    QM: {
        status: Status.LOADING,
        tableInfo: {
            el: '#wrapper-qualityTable',
            columns: [
                {
                    "data": "qualitymeter.QM_Code", "title": "压力计编码",
                    render: function (data, type, full, meta) {
                        return '<a href="javascript:void(0)">' + data + '</a>';
                    }
                },
                { "data": "qualitymeter.QM_Description", "title": "描述" },
                {
                    "data": "status.QMS_UpdateDt", "title": "更新",
                    render: function (data, type, full, meta) {
                        if (data) return dateFormat(data, 7);
                        else return '暂无';
                    }
                },
                { "data": "status.QMS_PressureValue", "title": "实时值", "defaultContent": "暂无" },
                { "data": "status.QMS_AntennaSignal", "defaultContent": "未知", "title": "操作" }
            ]
        },
        header: {
            title: [{
                content: '水质计'
            }]
        }
    },
    FeedBack: {
        header: {
            title: [{
                content: '反馈'
            }]
        }
    },
    deviceDetail: {
        FMDetail: {
            dataCount: {
                tableInfo: {
                    el: '#wrapper-detailTable',
                    columns: [
                        { "title": "抄表时间", data: "PH_Time" },
                        { "title": "时点值(Mpa)	", data: "PH_RealTimeValue" },
                        { "title": "最小(Mpa)", data: "PH_MinValue" },
                        { "title": "最大(Mpa)", data: "PH_MaxValue" },
                        { "title": "平均(Mpa)", data: "PH_AverageValue" }
                    ]
                },
            }
        },
        header: {
            title: [{
                href: '/Devices', content: '设备列表'
            }, {
                content: '设备详情'
            }]
        },
        dataAnalysis: {
        },
        dataCount: {
            tableInfo: {
                el: '#wrapper-detailTable',
                columns: [
                    { "title": "抄表时间", data: "time" },
                    { "title": "流量", data: "value" }
                ]
            },
        }
    }
};

let store = createStore(reducer, DeviceInfo, applyMiddleware(ReduxThunk.default));
class DeviceApp extends React.Component {
    render() {
        return (
            <div id="App">
                <aside>
                    <JsTree jsTreeInfo={this.props.jsTreeInfo} />
                </aside>
                <article>
                    {this.props.children}
                </article>
            </div>
        );
    }
}
const mapStateToProps = (state, ownProps) => {
    return {
        jsTreeInfo: state.jsTreeInfo
    };
};
DeviceApp = ReactRedux.connect(mapStateToProps)(DeviceApp);

class App extends React.Component {
    render() {
        return (
            <div id="root">
                <HeaderTop />
                {this.props.children}
                <Footer />
            </div>
        );
    }
}
ReactDOM.render(
    <Provider store={store}>
        <Router history={hashHistory}>
            <Route path="/" component={App}>
                <IndexRoute component={Home}></IndexRoute>
                <Route path="devices" component={DeviceApp}>
                    <IndexRoute component={Devices}></IndexRoute>
                    <Route path="/flowmeter/detail/:uid" component={Detail}></Route>
                    <Route path="/pressuremeter/detail/:uid" component={PMDetail}></Route>
                </Route>
                <Route path="feedback" component={FeedBackApp}></Route>
            </Route>
        </Router>
    </Provider>,
    document.querySelector('#content')
);
