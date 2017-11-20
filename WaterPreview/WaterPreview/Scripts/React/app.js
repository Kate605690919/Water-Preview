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
                            return '<a href="javascript:void(0)">' + data + '</a>';
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
        header: {
            title: [{
                    href: '/flowmeter', content: '设备列表'
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
    },
    Client: {
        tableInfo: {
            el: '#wrapper-detailTable',
            //event: {
            //    //initComplete: (settings, json) => {
            //    //    const button = document.getElementById('detail-wrapper-detailTable');
            //    //    button.click();
            //    //}
            //},
            columns: [
                {
                    "data": "Name", "title": "客户名",
                    render: function (data, type, full, meta) {
                        return '<a href="#/client/detail/uid=' + full.Uid + '">' + data + '</a>';
                    }
                },
                { "data": "RealName", "title": "真实姓名或公司名" },
                { "data": "area.Ara_Name", "defaultContent": "未知", "title": "所属区域" },
                {
                    "defaultContent": "删除", "title": "操作",
                    render: function (data, type, full, meta) {
                        return '<a href="javascript:void(0)"  data-id=' + full.Id + ' onclick=deleteClient(this.getAttribute("data-id"))>删除</a>';
                    }
                }
            ],
            header: {
                title: [{
                    href: '/Client', content: '客户管理'
                }],
                btn: [{
                    content: "添加客户",
                    url: '/Client/Add'
                }]
            }
        },
        editInfo: {
            formId: 'formClient2',
            itemInfo: [{
                id: 0, label: '所属区域', input: { help: '温馨提示：点击左侧"区域参考"选择区域', value: 'area.Ara_Name', readOnly: true }
            }, {
                    id: 1, label: '客户名', input: { name: 'Member_Name', value: 'Name' }
            }, {
                    id: 2, label: '真实姓名或公司名', input: { name: 'Member_RealName', value: 'RealName' }
            }, {
                    id: 3, label: '电话号码', input: { type: 'number', name: 'Member_Phone', value: 'Phone' }
            }, {
                    id: 4, label: '备注', input: { name: 'Member_Memo', value: 'Memo' }
            }],
            func: {
                url: '/Client/ModifyClient',
                data: "$('#formClient2').serialize() + '&Member_AreaUid=' + AreaTreeId + '&Member_UserUid=' + _this.props.uid",
                successFunc: (res) => {
                    if (res) {
                        debugger;
                        alert('提交成功！');
                        window.location.hash = '#/Client';
                    } else {
                        alert('提交失败！请重试.');
                    }
                }
            },
            btnContent: '提交',
            header: {
                title: [{
                    href: '/Client', content: '客户管理'
                }, {
                    href: '/Client', content: '客户详情'
                }, {
                    content: '编辑客户信息'
                }]
            }
        },
        formInfo: {
            formId: 'formClient2',
            itemInfo: [{
                id: 0, label: '所属区域', input: { help: '温馨提示：点击左侧"区域参考"选择区域', readOnly: true }
            }, {
                id: 1, label: '客户名', input: { name: 'Member_Name' }
            }, {
                id: 2, label: '真实姓名或公司名', input: { name: 'Member_RealName' }
            }, {
                id: 3, label: '电话号码', input: { type: 'number', name: 'Member_Phone' }
            }, {
                id: 4, label: '备注', input: { name: 'Member_Memo' }
            }],
            func: {
                url: '/Client/AddClient',
                data: "$('#formClient2').serialize() + '&Member_AreaUid=' + AreaTreeId",
                successFunc: (res) => {
                    if (res) {
                        alert('提交成功！');
                        window.location.hash = '#/Client';
                    } else {
                        alert('提交失败！请重试.');
                    }
                }
            },
            btnContent: '提交',
            header: {
                title: [{
                    href: '/Client', content: '客户管理'
                }, {
                    content: '添加客户'
                }]
            }
        },
        detailInfo: {
            ddId: 'ddClient',
            title: '基本信息',
            status: 'init',
            itemInfo: [{
                id: 0, label: '客户名', value: 'Name'
            }, {
                id: 1, label: '所属区域', value: 'area.Ara_Name'
            }, {
                id: 2, label: '真实姓名或公司名', value: 'RealName'
            }, {
                id: 3, label: '电话号码', value: 'Phone'
            }, {
                id: 4, label: '备注', value: 'Memo'
            }],
            btnContent: null,
            header: {
                title: [{
                    href: '/Client', content: '客户管理'
                }, {
                    content: '客户详情'
                }],
                btn: [{
                    content: "修改基本信息",
                    url: '/Client/Editbase'
                }, {
                    content: "绑定流量计",
                    url: '/Client/EditFM'
                }]
            }
        }
    },
    Staff: {
        tableInfo: {
            el: '#wrapper-detailTable',
            columns: [
                {
                    "data": "Name", "defaultContent": "未知", "title": "名称",
                    render: function (data, type, full, meta) {
                        return '<a href="#/staff/detail/uid=' + full.Uid + '">' + data + '</a>';
                    }
                },
                { "data": "area.Ara_Name", "defaultContent": "未知", "title": "区域" },
                { "data": "roles", "defaultContent": "未知", "title": "职位" },
                { "data": "Memo", "defaultContent": "未知", "title": "备注" },
                {
                    "defaultContent": "删除", "title": "操作",
                    render: function (data, type, full, meta) {
                        return '<a href="javascript:void(0)"  data-id=' + full.Id + ' onclick=deleteClient(this.getAttribute("data-id"))>删除</a>';
                    }
                }
            ],
            header: {
                title: [{
                    href: '/Staff', content: '职员管理'
                }],
                btn: [{
                    content: "添加职员",
                    url: '/Staff/Add'
                }]
            }
        },
        editInfo: {
            formId: 'formClient2',
            itemInfo: [{
                id: 0, label: '所属区域', input: { help: '温馨提示：点击左侧"区域参考"选择区域', value: '', readOnly: true }
            }, {
                id: 1, label: '客户名', input: { name: 'Member_Name', value: 'Name' }
            }, {
                id: 2, label: '真实姓名或公司名', input: { name: 'Member_RealName', value: 'RealName' }
            }, {
                id: 3, label: '电话号码', input: { type: 'number', name: 'Member_Phone', value: 'Phone' }
            }, {
                id: 4, label: '备注', input: { name: 'Member_Memo', value: 'Memo' }
            }],
            func: {
                url: '/Staff/ModifyClient',
                data: "$('#formClient2').serialize() + '&Member_AreaUid=' + AreaTreeId + '&Member_UserUid=' + _this.props.uid",
                successFunc: (res) => {
                    if (res) {
                        debugger;
                        alert('提交成功！');
                        window.location.hash = '#/Staff';
                    } else {
                        alert('提交失败！请重试.');
                    }
                }
            },
            btnContent: '提交',
            header: {
                title: [{
                    href: '/Staff', content: '职员管理'
                }, {
                    href: '/Staff', content: '职员详情'
                }, {
                    content: '编辑职员信息'
                }]
            }
        },
        editPW: {
            formId: 'formClient2',
            itemInfo: [{
                id: 0, label: '原密码', input: { name:''}
            }, {
                id: 1, label: '新密码', input: { name: 'Member_Name' }
            }, {
                id: 2, label: '再次输入新密码', input: { name: 'Member_RealName'}
            }],
            func: {
                url: '/Staff/ModifyClient',
                data: "$('#formClient2').serialize() + '&Member_AreaUid=' + AreaTreeId + '&Member_UserUid=' + _this.props.uid",
                successFunc: (res) => {
                    if (res) {
                        debugger;
                        alert('提交成功！');
                        window.location.hash = '#/Client';
                    } else {
                        alert('提交失败！请重试.');
                    }
                }
            },
            btnContent: '提交',
            header: {
                title: [{
                    href: '/Staff', content: '职员管理'
                }, {
                    href: '/Staff', content: '职员详情'
                }, {
                    content: '编辑职员信息'
                }]
            }
        },
        formInfo: {
            formId: 'formClient2',
            itemInfo: [{
                id: 0, label: '所属区域', input: { help: '温馨提示：点击左侧"区域参考"选择区域', readOnly: true }
            }, {
                id: 1, label: '用户名(*)', input: { name: 'Member_Name' }
            }, {
                id: 2, label: '真实姓名(*)', input: { name: 'Member_RealName' }
            }, {
                id: 3, label: '电话号码(*)', input: { type: 'number', name: 'Member_Phone' }
            }, {
                id: 4, label: '备注', input: { name: 'Member_Memo' }
            }],
            func: {
                url: '/Staff/AddClient',
                data: "$('#formClient2').serialize() + '&Member_AreaUid=' + AreaTreeId",
                successFunc: (res) => {
                    if (res) {
                        alert('提交成功！');
                        window.location.hash = '/Staff';
                    } else {
                        alert('提交失败！请重试.');
                    }
                }
            },
            btnContent: '提交',
            header: {
                title: [{
                    href: '/Staff', content: '职员管理'
                }, {
                    content: '添加职员'
                }]
            }
        },
        detailInfo: {
            ddId: 'ddClient',
            title: '基本信息',
            status: 'init',
            itemInfo: [{
                id: 0, label: '客户名', value: 'Name'
            }, {
                id: 1, label: '所属区域', value: 'area.Ara_Name'
            }, {
                id: 2, label: '真实姓名或公司名', value: 'RealName'
            }, {
                id: 3, label: '电话号码', value: 'Phone'
            }, {
                id: 4, label: '备注', value: 'Memo'
            }],
            btnContent: null,
            header: {
                title: [{
                    href: '/Staff', content: '职员管理'
                }, {
                    content: '职员详情'
                }],
                btn: [{
                    content: "修改基本信息",
                    url: '/Staff/Editbase'
                }]
            }
        }
    }
};

let store = createStore(reducer, DeviceInfo, applyMiddleware(ReduxThunk.default));
class DeviceApp extends React.Component {
    render() {
        return (
            <div id="App">
                <aside>
                    <JsTree jsTreeInfo={this.props.jsTreeInfo}/>
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
                <Route path="client" component={ManageApp}>
                    <IndexRoute component={Manage}></IndexRoute>
                    <Route path="Add" component={ManageAdd} />
                    <Route path="detail/:uid" component={ManageDetail} />
                    <Route path="editbase/:uid" component={ManageBase} />
                    <Route path="editfm/:uid" component={ManageFM} />
                </Route>
                <Route path="staff" component={StaffApp}>
                    <IndexRoute component={Staff}></IndexRoute>
                    <Route path="Add" component={StaffAdd} />
                    <Route path="detail/:uid" component={StaffDetail} />
                    <Route path="editbase/:uid" component={StaffBase} />
                </Route>
                <Route path="flowmeter" component={DeviceApp}>
                    <IndexRoute component={Devices}></IndexRoute>
                    <Route path="detail/:uid" component={Detail}></Route>
                </Route>
                <Route path="feedback" component={FeedBackApp}></Route>
            </Route>
        </Router>
    </Provider>,
    document.querySelector('#content')
);
