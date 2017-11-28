﻿class CardListItem extends React.Component {
    constructor(props) {
        super(props);
        this.state = { FlowList: { status: 'loadding' }, PressureList: {status: 'loadding'} };
        if (window.localStorage) {
            this._viewLog = localStorage.getItem('viewLog') ? JSON.parse(localStorage.getItem('viewLog')).slice(0, 3).map((item, index) => { return `fmUids=${item.uid}`; }) : [];
            this._PMViewLog = localStorage.getItem('PMViewLog') ? JSON.parse(localStorage.getItem('PMViewLog')).slice(0, 2).map((item, index) => { return `pmUids=${item.uid}`; }) : [];
            this._QMViewLog = localStorage.getItem('QMViewLog') ? JSON.parse(localStorage.getItem('QMViewLog')).slice(0, 1) : null;
        }
        let _this = this;
        fetch('/FlowMeter/GetMostVisitsFlowMeter', {
            method: 'POST',
            headers: { "Content-Type": "application/x-www-form-urlencoded" },
            body: _this._viewLog.join('&')
        }).then((response) => {
            if (response.status !== 200) {
                throw new Error('Fail to get response with status ' + response.status);
            }
            response.json().then((res) => {
                res = JSON.parse(res);
                _this.setState({ FlowList: { data: res, status: 'success' } });
            }).catch((error) => {
                _this.setState({ FlowList: { error: err, status: 'failure' } });
                console.error(error);
            });
            }).catch((error) => {
                _this.setState({ FlowList: { error: err, status: 'failure' } });
            console.error(error);
            });

            fetch('/PressureMeter/GetMostVisitsPressureMeter', {
                method: 'POST',
                headers: { "Content-Type": "application/x-www-form-urlencoded" },
                body: _this._PMViewLog.join('&')
            }).then((response) => {
                if (response.status !== 200) {
                    throw new Error('Fail to get response with status ' + response.status);
                }
                response.json().then((res) => {
                    res = JSON.parse(res);
                    _this.setState({ PressureList: { data: res, status: 'success' } });
                }).catch((error) => {
                    _this.setState({ PressureList: { error: err, status: 'failure' } });
                    console.error(error);
                });
            }).catch((error) => {
                _this.setState({ PressureList: { error: err, status: 'failure' } });
                console.error(error);
            });
    }
    componentDidMount() {
       
    }
    render() {
        let { FlowList, PressureList } = this.state;
        if (FlowList == null || FlowList.status === 'loadding') {
            var flowList = <h5>加载中...</h5>;
        } else if (FlowList.status === 'success'){
            var flowList = FlowList.data.map((item, index, arr) => {
                return (
                    <li className="list-group-item" style={{ display: 'flex', 'justifyContent': 'space-between', 'borderTop': '1px solid #e7eaec' }}>
                        <span className="label label-success">流量计</span>
                        <span>{item.flowmeter.FM_Description}</span>
                        <span>{parseInt(item.lastday_flow).toFixed(2)}</span>
                        <span>{item.lastday_flow_proportion}</span>
                    </li>);
            });
        }

        if (PressureList == null || PressureList.status === 'loadding') {
            var pressureList = <h5>加载中...</h5>;
        } else if (PressureList.status === 'success') {
            var pressureList = PressureList.data.map((item, index, arr) => {
                return (
                    <li className="list-group-item" style={{ display: 'flex', 'justifyContent': 'space-between', 'borderTop': '1px solid #e7eaec'  }}>
                        <span className="label label-success">压力计</span>
                        <span>{item.pressuremeter.PM_Description}</span>
                        <span>{parseInt(item.lastday_pressure).toFixed(2)}</span>
                        <span>{item.lastday_pressure_proportion}</span>
                    </li>);
            });
        }

        return (
            <ul className="list-group clear-list m-t">
                {flowList}
                {pressureList}
            </ul>
        );
    }
}

//(async () => {
//    try {
//        let res = await fetch(`/PressureMeter/GetMostVisitsPressureMeter`, {
//            method: 'POST',
//            headers: { "Content-Type": "application/x-www-form-urlencoded" },
//            body: _this._PMViewLog.join('&')
//        });
//        _this.setState({ PressureList: { data: res, status: 'success' } });
//    } catch (err) {
//        _this.setState({ PressureList: { error: err, status: 'failure' } });
//    }
//})();
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