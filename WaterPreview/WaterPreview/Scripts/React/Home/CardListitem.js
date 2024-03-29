﻿class CardListItem extends React.Component {
    constructor(props) {
        super(props);
        this.state = { FlowList: { status: 'loadding' }, PressureList: {status: 'loadding'} };
        this.onEditViewCount = this.onEditViewCount.bind(this);
        this.onIncreaseFC = this.onIncreaseFC.bind(this);
        this.onIncreasePC = this.onIncreasePC.bind(this);
        this.onRenderRank = this.onRenderRank.bind(this);
        this.getUids({ url: '/FlowMeter/GetMostVisitsFlowMeter', stateName: 'FlowList' });
        this.getUids({ url: '/PressureMeter/GetMostVisitsPressureMeter', stateName: 'PressureList' });
    }
    onEditViewCount() {
        const FC = $('#DeviceViewCount0').val();
        const PC = $('#DeviceViewCount1').val();
        this.props.onChangeDeviceCount(FC, PC);
    }
    onIncreaseFC() {
        this.props.increaseFC();
    }
    onIncreasePC() {
        this.props.increasePC();
    }
    onRenderRank() {
        this.props.renderRank();
    }
    getUids({ url, stateName }) {
        let _this = this;
        (async () => {
            try {
                let res = await $Fetch.fetchSync_Post({ url: url });
                if (stateName === 'FlowList')    _this.onRenderRank();
                _this.setState({ [stateName]: { data: res, status: 'success' } });
            } catch (err) {
                _this.setState({ [stateName]: { error: err, status: 'failure' } });
            }
        })();
    }
    getList({ state, cols, length = 3 }) {
        length = parseInt(length);
        if (state == null || state.status === 'loadding') {
            return <Loading />;
        } else if (state.status === 'success') {
            return state.data.slice(0, length).map((item, index, arr) => {
                return (
                    <li className="list-group-item" style={{ display: 'flex', 'justifyContent': 'space-between', 'borderTop': '1px solid #e7eaec' }}>
                        <span className={`label label-${cols[0][0]}`}>{cols[0][1]}</span>
                        <span style={{'width': '120px'}}>{eval(`item.${cols[1]}`)}</span>
                        <span style={{ 'width': '65px' }}>{parseInt(eval(`item.${cols[2]}`)).toFixed(2)}</span>
                        <span style={{ 'width': '65px' }}>{eval(`item.${cols[3]}`)}<i className={`fa fa-level-${parseInt(eval(`item.${cols[3]}`)) >= 0 ? 'up' : 'down'}`}></i></span>
                    </li>);
            });
        } else if (state.status === 'failure') {
            return <h5>加载失败...</h5>;
        }
    }
    render() {
        let { FlowList, PressureList } = this.state;
        let { formInfo, flowCount, pressureCount } = this.props;
        let flowList = this.getList({ length: flowCount, state: FlowList, cols: [['success','流量计'], 'flowmeter.FM_Description', 'lastday_flow', 'lastday_flow_proportion'] });
        let pressureList = this.getList({ length: pressureCount, state: PressureList, cols: [['info', '压力计'], 'pressuremeter.PM_Description', 'lastday_pressure', 'lastday_pressure_proportion'] });
        formInfo.data = { flowCount: flowCount, pressureCount: pressureCount};
        return (
            <div className="commonDevice" style={{ 'borderRight': '1px solid rgb(231, 234, 236)', 'paddingRight': '20px',  'width': '380px' }}>
                <div style={{ display: 'flex', 'justifyContent': 'space-between', }}>
                    <h3>常用设备</h3>
                    <a href='javascript:void(0)' data-toggle="modal" data-target="#myModal"><i className="fa fa-cog fa-2" aria-hidden="true" style={{'lineHeight': '32.33px', 'fontSize': '16px'}}></i></a>
                </div>
                <ul className="list-group clear-list m-t" style={{ minHeight: '270px', 'marginTop': '0' }}>
                    <li className="list-group-item" style={{ display: 'flex', 'justifyContent': 'space-between', 'borderTop': '1px solid #e7eaec', 'color': 'rgb(158, 158, 158)' }}>
                        <span>类型</span>
                        <span>名称</span>
                        <span>昨日流量/压力</span>
                        <span>昨日变化趋势</span>
                    </li>
                    {flowList}
                    <li className="list-group-item" style={{ display: 'flex', 'justifyContent': 'flex-end', 'borderTop': '1px solid #e7eaec', 'color': 'rgb(158, 158, 158)' }}>
                        <span></span>
                        <span></span>
                        <a className="label label-default" style={{ 'lineHeight': '15px', 'marginRight': '10px' }} onClick={this.onIncreaseFC}>下一个<i aria-hidden="true" className="fa fa-arrow-down" style={{ 'marginLeft': '5px' }}></i></a>
                        <Link to="/Devices" className="label label-default" style={{ 'lineHeight': '15px' }}>更多<i aria-hidden="true" className="fa fa-hand-o-right" style={{ 'marginLeft': '5px'}}></i></Link>
                    </li>
                    {pressureList}
                    <li className="list-group-item" style={{ display: 'flex', 'justifyContent': 'flex-end', 'borderTop': '1px solid #e7eaec', 'color': 'rgb(158, 158, 158)' }}>
                        <span></span>
                        <span></span>
                        <a className="label label-default" style={{ 'lineHeight': '15px', 'marginRight': '10px' }} onClick={this.onIncreasePC}>下一个<i aria-hidden="true" className="fa fa-arrow-down" style={{ 'marginLeft': '5px' }}></i></a>
                        <Link to="/Devices" className="label label-default" style={{ 'lineHeight': '15px' }}>更多<i aria-hidden="true" className="fa fa-hand-o-right" style={{ 'marginLeft': '5px' }}></i></Link>
                    </li>
                </ul>

                <div className="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
                    <div className="modal-dialog" role="document">
                        <div className="modal-content">
                            <div className="modal-header">
                                <button type="button" className="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                <h4 className="modal-title" id="myModalLabel">更改设备显示数量</h4>
                            </div>
                            <div className="modal-body">
                                <Form formInfo={formInfo}/>
                            </div>
                            <div className="modal-footer">
                                <button type="button" className="btn btn-primary" data-dismiss="modal" onClick={this.onEditViewCount}>保存修改</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}
const mapStateToProps = (state) => {
    return {
        formInfo: state.home.editDeviceViewCount,
        flowCount: parseInt(state.home.DeviceViewCount.flowCount),
        pressureCount: parseInt(state.home.DeviceViewCount.pressureCount),
    };
};
const mapDispatchToProps = (dispatch) => {
    return {
        onChangeDeviceCount: (FC, PC) => {
            dispatch(changeDeviceCount(FC, PC));
        },
        increaseFC: () => {
            dispatch(increaseFC());
        },
        increasePC: () => {
            dispatch(increasePC());
        },
        renderRank: () => {
            dispatch(onRenderRank());
        }
    }
}
CardListItem = ReactRedux.connect(mapStateToProps, mapDispatchToProps)(CardListItem);
//<div className="ibox MiniCard">
//    <div className="ibox-title" style={{ padding: '0 10px', minHeight: '30px' }}>
//        <span className="no-margin" style={{ lineHeight: '30px', margin: 0 }}>{this.props.bigH.header}</span>
//        <span className="pull-right no-margin" style={{ lineHeight: '30px' }}>{this.props.bigH.content}</span>
//    </div>
//    <div className="ibox-content">
//        {this.props.chart ? <div id="stackChart" ref="stackChart" style={{ marginBottom: '10px' }}></div> : null}
//        <span>{this.props.smallH.header}</span>
//        <span className="font-bold text-success" style={{ margin: 0 }}>{this.props.smallH.content}<i className={`fa fa-level-${parseInt(this.props.smallH.content) >= 0 ? 'up' : 'down'}`}></i></span>
//    </div>
//</div>
//画图
//if (this.refs.stackChart) {
//    let preData = [83.8281696600678, 56.6014272481756, 52.4266481213743, 50.8078892242481, 74.3907730800399, 74.1093181674194, 100.646304358387, 162.319468066664, 176.903639682744, 149.563206077522, 163.148820126547, 107.050213940757, 156.266316216881, 101.982669373231, 99.8255425048380, 140.086194405267, 123.482363765647, 157.709357822321, 132.903577895222, 162.816489934505, 176.345672422926, 189.456355450432, 209.696443447765, 169.558472349785];
//    let curData = [84.2500000000000, 60, 55.5000000000000, 53, 69.5000000000000, 79, 98, 161.750000000000, 176, 147.250000000000, 162.250000000000, 115.750000000000, 157.500000000000, 100.750000000000, 93.5000000000000, 145.500000000000, 127.250000000000, 154.750000000000, 116.750000000000, 158.750000000000, 181.750000000000, 200.500000000000, 222.500000000000, 165.500000000000];
//    var det = [];
//    for (let k = 0; k < curData.length; k++) {
//        det[k] = Math.abs(curData[k] - preData[k]);
//    };
//    var index = 0;
//    var maxTemp = 0;
//    for (let i = 0; i < det.length; i++) {
//        if (maxTemp < det[i]) {
//            maxTemp = det[i];
//            index = i;
//        }
//    }
//    var myStackChart = echarts.init(this.refs.stackChart);
//    //var myStackChart = echarts.init(document.querySelector("#stackChart"));

//    var option = {
//        tooltip: {
//            trigger: 'axis',
//            axisPointer: {
//                type: 'line'
//            }
//        },
//        grid: {
//            left: '1%',
//            right: '1%',
//            top: '1%',
//            bottom: '1%'
//        },
//        xAxis: [
//            {
//                type: 'category',
//                boundaryGap: false,
//                show: false,
//                data: ['00:00', '01:00', '02:00', '03:00', '04:00', '05:00', '06:00', '07:00', '08:00', '09:00', '10:00', '11:00', '12:00', '13:00', '14:00', '15:00', '16:00', '17:00', '18:00', '19:00', '20:00', '21:00', '22:00', '23:00']
//            }
//        ],
//        yAxis: [
//            {
//                type: 'value',
//                show: false
//            }
//        ],
//        series: [
//            {
//                name: '2016年4月1日各小时流量实际值',
//                type: 'line',
//                stack: '总量',
//                symbol: 'circle',
//                symbolSize: 1,
//                itemStyle: {
//                    normal: { color: '#1ab394' }
//                },
//                lineStyle: {
//                    normal: { width: 1, color: '#1ab394' }
//                },
//                areaStyle: {
//                    normal: { color: '#1ab394' }
//                },
//                //data: [107.407402451144, 46.6603845157310, 53.0239968041645, 68.3708378683146, 49.5784529893234, 84.4318488618434, 137.005743859182, 143.224040201288, 181.403288294015, 156.979925598269, 138.785169387797, 170.874900782770, 144.470848674308, 119.869930074261, 110.872423941873, 61.0636549323388, 133.140162305593, 92.4758861592041, 117.181350178129, 119.511917674967, 136.469092339230, 156.264000775954, 140.697905635398, 106.863690508313]
//                data: curData,
//                markPoint: {
//                    data: [{
//                        coord: [index, Math.round(maxTemp)],
//                    }],
//                    name: "疑似异常值",
//                    //label: {
//                    //    normal: {
//                    //        show: true,
//                    //        position: 'top',
//                    //        formatter: '#ed5565'
//                    //    }
//                    //}
//                }
//            },
//            {
//                name: '2016年4月1日各小时流量预测值',
//                type: 'line',
//                stack: 'null',
//                symbol: 'circle',
//                symbolSize: 1,
//                itemStyle: {
//                    normal: { color: 'rgb(0,108,84)' }
//                },
//                lineStyle: {
//                    normal: { width: 1, color: 'rgb(0,108,84)' }
//                },
//                areaStyle: {
//                    normal: {
//                        color: 'rgb(0,108,84)'
//                    }
//                },
//                data: preData
//            }
//        ]
//    };
//    myStackChart.setOption(option);
//}