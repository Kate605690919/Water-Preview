class Detail extends React.Component {
    constructor(props) {
        super(props);
        this._uid = this.props.params.uid;
        let _this = this;
        this.state = { detail: null, analysis: null };
        //pv统计
        //localStorage.setItem('viewLog', null);
        if (window.localStorage) {
            let viewLog = null;
            try {
                viewLog = JSON.parse(localStorage.getItem('viewLog'));
            } catch (error) {
                if (error instanceof SyntaxError) {
                    viewLog = null;
                }
            }
            let obj = {};
            if (Array.isArray(viewLog)) {
                for (let i = 0; i < viewLog.length; i++) {
                    if (viewLog[i].uid == _this._uid.substr(4)) {
                        viewLog[i].count = parseInt(viewLog[i].count) + 1;
                        obj = viewLog[i];
                        viewLog.splice(i, 1);
                    } else {
                        obj = { uid: _this._uid.substr(4), count: 1 }
                    }
                }
                viewLog[viewLog.length] = { count: -1 };
                for (let j = 0; j < viewLog.length; j++) {
                    if (parseInt(viewLog[j].count) < obj.count) {
                        viewLog.splice(j, 0, obj);
                        viewLog.pop(1);
                        break;
                    }
                }
            } else {
                viewLog = [{ uid: _this._uid.substr(4), count: 1 }];
            }
            localStorage.setItem('viewLog', JSON.stringify(viewLog));
        }
        fetch(`/FlowMeter/Detail`, { method: 'POST', headers: { 'Content-Type': 'application/x-www-form-urlencoded', }, body: _this._uid }).then((response) => {
            if (response.status !== 200) {
                throw new Error('Fail to get response with status ' + response.status);
            }
            response.json().then((res) => {
                _this.setState({ detail: res[0] });
                $.get(`/flowmeter/Analysis?${_this._uid}&time=${dateFormat(res[0].flowmeter.FM_FlowCountLast, 2)}`, function (data) {
                    _this.setState({ analysis: data });
                });
            }).catch((error) => {
                console.error(error);
            });
        }).catch((error) => {
            console.error(error);
        })
    }
    render() {
        if (this.state.detail) {
            let { ara, flowmeter, status } = this.state.detail;
            let analysis = this.state.analysis;
            this.props.header.title[1].content = `设备详情(${flowmeter.FM_Code} ${flowmeter.FM_Description} ${dateFormat(flowmeter.FM_FlowCountLast, 2)})`
            return (
                <div className="wrapper wrapper-content animated fadeInRight">
                    <div className="ibox float-e-margins">
                        <div className="ibox-title" style={{ position: 'relative', zIndex: '999' }}>
                            <Header header={this.props.header} />
                            <div className="battery-group" style={{ position: 'absolute', right: 0, display: 'flex' }}>
                                <Battery electricity={status.FMS_MainBatteryStatus} content={'主电源'} />
                                <Battery electricity={status.FMS_SecondaryBatteryStatus} content={'备用电源'} />
                                <Battery electricity={status.FMS_ModemBatteryStatus} content={'通信电池'} />
                                <Battery electricity={status.FMS_AntennaSignal} content={'信号强度'} />
                            </div>
                        </div>
                        <div className="ibox-content">
                            <div className="row" id="dataAnalysis">
                                <div className="col-md-3">
                                    <MiniCard bigH={{ header: '昨日总流量', content: analysis ? analysis.lastday_flow : '加载中...' }} smallH={{ header: '变化趋势', content: analysis ? `${analysis.lastday_flow_proportion}` : '加载中...' }} />
                                </div>
                                <div className="col-md-3">
                                    <MiniCard bigH={{ header: '上月总流量', content: analysis ? analysis.month_flow : '加载中...' }} smallH={{ header: '变化趋势', content: analysis ? `${analysis.month_flow_proportion}` : '加载中...' }} />
                                </div>
                                <div className="col-md-6">
                                    <MiniCard bigH={{ header: '昨日凌晨2点-4点流量均值', content: analysis ? analysis.night_flow : '加载中...' }} smallH={{ header: '夜间用水量*24*30/总用水量', content: analysis ? `${analysis.night_flow_proportion}` : '加载中...' }} />
                                </div>
                            </div>

                            <div className="tabs-container">
                                <ul className="nav nav-tabs">
                                    <li className="active"><a data-toggle="tab" href="#tab-1">统计数据</a></li>
                                    <li className=""><a data-toggle="tab" href="#tab-2">数据分析</a></li>
                                </ul>
                                <div className="tab-content">
                                    <div id="tab-1" className="tab-pane active">
                                        <DataCount uid={this.props.params.uid} url="/flowmeter/currentData" tableInfo={this.props.tableInfo}/>
                                    </div>
                                    <div id="tab-2" className="tab-pane">
                                        <DataAnalysis uid={this.props.params.uid} url="/flowmeter/RecentFlowData" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            )
        } else {
            return <div>正在加载中...</div>;
        }
    }
}
const mapStateToProps = (state) => {
    return {
        header: state.deviceDetail.header,
        tableInfo: state.deviceDetail.dataCount.tableInfo
    };
};
Detail = ReactRedux.connect(mapStateToProps)(Detail);