class Home extends React.Component {
    constructor(props) {
        super(props);
    }
    componentDidMount() {
        let preData = [83.8281696600678, 56.6014272481756, 52.4266481213743, 50.8078892242481, 74.3907730800399, 74.1093181674194, 100.646304358387, 162.319468066664, 176.903639682744, 149.563206077522, 163.148820126547, 107.050213940757, 156.266316216881, 101.982669373231, 99.8255425048380, 140.086194405267, 123.482363765647, 157.709357822321, 132.903577895222, 162.816489934505, 176.345672422926, 189.456355450432, 209.696443447765, 169.558472349785];
        let curData = [84.2500000000000, 60, 55.5000000000000, 53, 69.5000000000000, 79, 98, 161.750000000000, 176, 147.250000000000, 162.250000000000, 115.750000000000, 157.500000000000, 100.750000000000, 93.5000000000000, 145.500000000000, 127.250000000000, 154.750000000000, 116.750000000000, 158.750000000000, 181.750000000000, 200.500000000000, 222.500000000000, 165.500000000000];
        var det = [];
        for (let k = 0; k < curData.length; k++) {
            det[k] = Math.abs(curData[k] - preData[k]);
        };
        var index = 0;
        var maxTemp = 0;
        for (let i = 0; i < det.length; i++) {
            if (maxTemp < det[i]) {
                maxTemp = det[i];
                index = i;
            }
        }
        var myStackChart = echarts.init(document.querySelector("#stackChart"));
        var option = {
            tooltip: {
                trigger: 'axis',
                axisPointer: {
                    type: 'line'
                }
            },
            grid: {
                left: '1%',
                right: '1%',
                top: '1%',
                bottom: '1%'
            },
            xAxis: [
                {
                    type: 'category',
                    boundaryGap: false,
                    show: false,
                    data: ['00:00', '01:00', '02:00', '03:00', '04:00', '05:00', '06:00', '07:00', '08:00', '09:00', '10:00', '11:00', '12:00', '13:00', '14:00', '15:00', '16:00', '17:00', '18:00', '19:00', '20:00', '21:00', '22:00', '23:00']
                }
            ],
            yAxis: [
                {
                    type: 'value',
                    show: false
                }
            ],
            series: [
                {
                    name: '2016年4月1日各小时流量实际值',
                    type: 'line',
                    stack: '总量',
                    symbol: 'circle',
                    symbolSize: 1,
                    itemStyle: {
                        normal: { color: '#1ab394' }
                    },
                    lineStyle: {
                        normal: { width: 1, color: '#1ab394' }
                    },
                    areaStyle: {
                        normal: { color: '#1ab394' }
                    },
                    //data: [107.407402451144, 46.6603845157310, 53.0239968041645, 68.3708378683146, 49.5784529893234, 84.4318488618434, 137.005743859182, 143.224040201288, 181.403288294015, 156.979925598269, 138.785169387797, 170.874900782770, 144.470848674308, 119.869930074261, 110.872423941873, 61.0636549323388, 133.140162305593, 92.4758861592041, 117.181350178129, 119.511917674967, 136.469092339230, 156.264000775954, 140.697905635398, 106.863690508313]
                    data: curData,
                    markPoint: {
                        data: [{
                            coord: [index, Math.round(maxTemp)],
                        }],
                        name: "疑似异常值",
                        //label: {
                        //    normal: {
                        //        show: true,
                        //        position: 'top',
                        //        formatter: '#ed5565'
                        //    }
                        //}
                    }
                },
                {
                    name: '2016年4月1日各小时流量预测值',
                    type: 'line',
                    stack: 'null',
                    symbol: 'circle',
                    symbolSize: 1,
                    itemStyle: {
                        normal: { color: 'rgb(0,108,84)' }
                    },
                    lineStyle: {
                        normal: { width: 1, color: 'rgb(0,108,84)' }
                    },
                    areaStyle: {
                        normal: {
                            color: 'rgb(0,108,84)'
                        }
                    },
                    data: preData
                }
            ]
        };
        myStackChart.setOption(option);
        //地图
        var idMap = document.querySelector("#homeMap");
        initMap(idMap);
    }
    render() {
        return (
            <div className="homeBody" style={{width: '100%'}}>
                <div className="row">
                    <div className="col-md-2 homeCard">
                        <MiniCard bigH={{ header: '旺塘泵站', content: '706.159' }} smallH={{ header: '昨日总流量', content: '3%' }} />
                    </div>
                    <div className="col-md-2 homeCard">
                        <MiniCard bigH={{ header: '佳兆业-城市广场', content: '604.531' }} smallH={{ header: '昨日总流量', content: '2%' }} />
                    </div>
                    <div className="col-md-2 homeCard">
                        <MiniCard bigH={{ header: '慢城金积嘉', content: '123.7969' }} smallH={{ header: '昨日总流量', content: '5%' }} />
                    </div>
                    <div className="col-md-2 homeCard">
                        <MiniCard bigH={{ header: '中粮地产', content: '21184.59' }} smallH={{ header: '昨日总流量', content: '40%' }} />
                    </div>
                    <div className="col-md-4 homeCard">
                        <div className="ibox float-e-margins">
                            <div className="ibox-title">
                                <span className="label label-primary pull-right">Flow</span>
                                <h5 id="cardA">旺塘泵站 昨日流量记录</h5>
                            </div>
                            <div className="ibox-content">
                                <div id="stackChart"></div>
                            </div>
                        </div>
                    </div>
                </div>
                <div className="row">
                    <div className="col-md-12" id="homeMap"></div>
                </div>
                <Footer />
            </div>
        );
    }
}