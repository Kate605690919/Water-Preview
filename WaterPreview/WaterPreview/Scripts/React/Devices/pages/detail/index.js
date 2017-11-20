class Detail extends React.Component {
    constructor(props) {
        super(props);
        this._uid = this.props.params.uid;
        let _this = this;
        this.state = { detail: null, analysis: null };
        fetch(`/FlowMeter/Detail`, { method: 'POST', headers: { 'Content-Type': 'application/x-www-form-urlencoded', }, body: _this._uid }).then((response) => {
            if (response.status !== 200) {
                throw new Error('Fail to get response with status ' + response.status);
            }
            response.json().then((res) => {
                _this.setState({ detail: res });
                $.get(`/flowmeter/Analysis?${_this._uid}&time=${dateFormat(res.flowmeter.FM_FlowCountLast, 2)}`, function (data) {
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
                                <Battery electricity={this.state.detail.status.FMS_MainBatteryStatus} content={'主电源'} />
                                <Battery electricity={this.state.detail.status.FMS_SecondaryBatteryStatus} content={'备用电源'} />
                                <Battery electricity={this.state.detail.status.FMS_ModemBatteryStatus} content={'通信电池'} />
                                <Battery electricity={this.state.detail.status.FMS_AntennaSignal} content={'信号强度'} />
                            </div>
                        </div>
                        <div className="ibox-content">
                            <div className="row" id="dataAnalysis">
                                <div className="col-md-3">
                                    <MiniCard bigH={{ header: '昨日总流量', content: analysis ? analysis.lastdaytotal : '加载中...' }} smallH={{ header: '变化趋势', content: analysis ? `${analysis.lastdayproportion}%` :'加载中...'}} />
                                </div>
                                <div className="col-md-3">
                                    <MiniCard bigH={{ header: '上月总流量', content: analysis ? analysis.monthflow : '加载中...' }} smallH={{ header: '变化趋势', content: analysis ? `${analysis.result}%` : '加载中...' }} />
                                </div>
                                <div className="col-md-6">
                                    <MiniCard bigH={{ header: '昨日凌晨2点-4点流量均值', content: analysis ? analysis.lastday : '加载中...' }} smallH={{ header: '夜间用水量*24*30/总用水量', content: analysis ? `${analysis.nightproportion}%` : '加载中...' }} />
                                </div>
                            </div>

                            <div className="tabs-container">
                                <ul className="nav nav-tabs">
                                    <li className="active"><a data-toggle="tab" href="#tab-1">统计数据</a></li>
                                    <li className=""><a data-toggle="tab" href="#tab-2">数据分析</a></li>
                                </ul>
                                <div className="tab-content">
                                    <div id="tab-1" className="tab-pane active">
                                        <DataCount uid={this.props.params.uid} />
                                    </div>
                                    <div id="tab-2" className="tab-pane">
                                        <DataAnalysis uid={this.props.params.uid} />
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
        header: state.deviceDetail.header
    };
};
Detail = ReactRedux.connect(mapStateToProps)(Detail);