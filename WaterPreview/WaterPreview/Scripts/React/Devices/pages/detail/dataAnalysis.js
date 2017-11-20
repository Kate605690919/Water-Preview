class DataAnalysis extends React.Component {
    constructor(props) {
        super(props);
        this._uid = this.props.uid;
        this.state = { detail: null };
        
    }
    componentDidMount() {
        let _this = this;
        $('a[href="#tab-2"]').on('shown.bs.tab', function (e) {
            _this.init();
        })
    }
    init() {
        let _this = this;
        $.get(`/flowmeter/RecentFlowData?${this._uid}`, function (data) {
            _this.setState({ detail: true });
            _this.mychart("heatMapFm", data);
        });
    }
    mychart(id, data) {
        var myChart = echarts.init(document.getElementById(id));
        //获取dataMax，dataMin
        var dataTime = new Array();
        var days = new Array();
        var hours = new Array();
        var dataMax = 0;
        for (var i = 0; i < (data.value).length - 1; i++) {
            if (dataMax < data.value[i]) {
                dataMax = data.value[i];
            };
        }
        var dataMin = dataMax;
        for (var i = 0; i < (data.value).length - 1; i++) {
            if (dataMin > data.value[i]) {
                dataMin = data.value[i];
            };
        }
        //获取hours和days数组以及datas数组
        var datas = [];
        for (var i = 0; i < (data.time).length; i++) {
            dataTime[i] = (data.time[i]).toString();
            hours.push(dataTime[i].substring(8));
            days.push(dataTime[i].substring(0, 8));
            datas[i] = [hours[i], days[i], Math.round(data.value[i])];
        };
        Array.prototype.unique3 = function () {
            var res = [];
            var json = {};
            for (var i = 0; i < this.length; i++) {
                if (!json[this[i]]) {
                    res.push(this[i]);
                    json[this[i]] = 1;
                }
            }
            return res;
        };
        //将横纵坐标的坐标值按从小到大排序
        hours = this.bubbleSort(hours);
        days = this.bubbleSort(days);

        hours = hours.unique3();
        days = days.unique3();
        let option = {
            title: {
                text: '流量热力图',
                subtext: '一段连续时间每天24小时流量统计'
            },
            tooltip: {
                position: 'top'
            },
            toolbox: {
                feature: {
                    saveAsImage: {}
                }
            },
            animation: false,
            grid: {
                height: '68%',
                y: '15%',
                x: '15%'
            },
            xAxis: {
                type: 'category',
                data: hours
            },
            yAxis: {
                type: 'category',
                data: days
            },
            visualMap: {
                min: dataMin,
                max: dataMax,
                calculable: true,
                orient: 'horizontal',
                left: 'center',
                bottom: '0%',
                target: {
                    inRange: {
                        color: ['#b5e2ff', '#2A8FBD', '#00466B']
                    }
                },
                controller: {
                    inRange: {
                        color: ['#b5e2ff', '#2A8FBD', '#00466B']
                    }
                }
            },
            series: [{
                name: '流量',
                type: 'heatmap',
                data: datas,
                label: {
                    normal: {
                        show: false
                    }
                },
                itemStyle: {
                    emphasis: {
                        shadowBlur: 10,
                        shadowColor: 'rgba(0, 0, 0, 0.3)'
                    }
                }
            }]
        };
        myChart.setOption(option);
    }
    //冒泡排序: bubbleSort()
    bubbleSort(arg) {
        for (var i = 0; i < arg.length; i++)
            for (var j = 0; j < arg.length - i; j++) {
                if (arg[j] > arg[j + 1]) {
                    var Temp = arg[j];
                    arg[j] = arg[j + 1];
                    arg[j + 1] = Temp;
                };
            };
        return (arg);
    }
    render() {
        if (!this.state.detail) { return <div>正在加载中...</div> }
        else {
            return (
                <div className="dataAnalysis">
                    <div className="row" style={{ padding: '10px 15px' }}>
                        <div id="heatMapFm" className="animated fadeInRight"></div>
                    </div>
                </div>
            );
        }
    }
}
const mapStateToProps = (state) => {
    return {
        header: state.deviceDetail.dataAnalysis.header
    };
};
DataAnalysis = ReactRedux.connect(mapStateToProps)(DataAnalysis);