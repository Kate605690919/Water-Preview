function select(arr, key) {
    var ret = [];
    for (var index in arr) {
        if (arr[index][key]!=undefined)
        ret.push(arr[index][key]);
    }
    return ret;
}


function dataStatFlow(id, data) {
    //基于准备好的DOM，初始化echarts的实例
    var myChartLine = echarts.init(document.getElementById(id));
    //坐标的数据
    var pattern1 = "-";
    var pattern2 = " ";
    var pattern3 = "-";
    var timeTemp = [];
    var dataTemp = [];
    for (let k = 0; k < data.length; k++) {
        dataTemp[k] = data[k];
    }
    for (let n = 0; n < data.length; n++) {
        timeTemp[n] = dataTemp[n].time;
        //flowDataYAxis[i] = data[i].value;
        dataTemp[n].time = timeTemp[n].split(pattern1).join("");
        dataTemp[n].time = timeTemp[n].split(pattern2).join("");
        dataTemp[n].time = timeTemp[n].split(pattern3).join("");
    }
    
    console.log(dataTemp);
    function bubbleSort(arg) {
        for (i = 0; i < arg.length; i++)
            for (j = 0; j < arg.length - i - 1 ; j++) {
                if (arg[j].time > arg[j + 1].time) {
                    Temp = arg[j];
                    arg[j] = arg[j + 1];
                    arg[j + 1] = Temp;
                };
            };
        return (arg);
    }
    var dataTemp = bubbleSort(dataTemp);
    var dataXAxis = select(dataTemp,"time");
    var flowDataYAxis = select(dataTemp, "value");
        
    
    //var dataXAxis = ["2017040609", "2017040608", "2017040607", "2017040606", "2017040605", "2017040604", "2017040603", "2017040602", "2017040601", "2017040600"];
    //var flowDataYAxis = [16.50, 225.00, 209.75, 124.50, 79.00, 87.00, 78.50, 95.50, 254.75, 304.25];
    //var maxDataYAxis = [198.68, 276.33, 263.06, 194.70, 112.19, 121.19, 112.86, 230.34, 335.54, 351.74];
    //var minDataYAxis = [194.93, 180.14, 142.35, 78.95, -4.68, 59.72, -4.95, -5.05, 3.03, 329.96, 116.11];

    //指定图表的配置项和数据
    var optionLine = {
        title: {
            text: '流量计数据统计图',
            subtext: '对应统计数据表格数据显示'
        },
        tooltip: {
            trigger: 'axis',
            axisPointer: {
                type: 'line',   //显示横纵坐标实时值
                lineStyle: {
                    type: 'dotted',
                    color: '#188df0',//'999'
                    shadowColor: 'rgba(0, 0, 0, 0.5)',
                    shadowBlur: 10
                },
                label: {
                    backgroundColor: ''  //#6a7985
                }
            }
        },  //提示框
        legend: {
            top: '1%',
            right: '18%',
            data: ['流量'] 
            //, '最大值', '最小值
        },
        toolbox: {
            feature: {
                saveAsImage: {}, //保存为图片
                magicType: {     //动态类型切换
                    type: ['line', 'bar']
                },
                restore: {}  //配置项还原
            }
        },    //工具栏 
        grid: {
            show: false,
            top: '15%',   //距离上边多少
            left: '2%',
            right: '5%',
            // bottom: '8%',
            containLabel: true
            // backgroundColor: '#eee'
        },
        xAxis: {
            data: dataXAxis,
            type: 'category',
            nameLocation: 'middle',
            boundaryGap: false, //坐标轴两边留白策略？
            axisLabel: {
                inside: false,
                textStyle: {
                    color: '#3b3c40'
                }
            },
            axisTick: {         //显示刻度
                show: false
            },
            axisLine: {
                show: false    //显示坐标轴
            },
            z: 10             //等同于z-index
        },
        yAxis: {
            type: 'value',
            axisTick: {        //显示刻度
                show: false
            },
            axisLine: {
                show: false    //显示坐标轴
            },
            axisLabel: {
                textStyle: {
                    color: '#999'
                }
            }
        },
        dataZoom: [
            {
                type: 'inside'
            },
            {
                start: 0,
                end: 10,
                handleIcon: 'M10.7,11.9v-1.3H9.3v1.3c-4.9,0.3-8.8,4.4-8.8,9.4c0,5,3.9,9.1,8.8,9.4v1.3h1.3v-1.3c4.9-0.3,8.8-4.4,8.8-9.4C19.5,16.3,15.6,12.2,10.7,11.9z M13.3,24.4H6.7V23h6.6V24.4z M13.3,19.6H6.7v-1.4h6.6V19.6z',
                handleSize: '80%',
                handleStyle: {
                    color: '#fff',
                    shadowBlur: 3,
                    shadowColor: 'rgba(0, 0, 0, 0.6)',
                    shadowOffsetX: 2,
                    shadowOffsetY: 2
                }
            }
        ],
        series: [
            {
                name: '流量',
                type: 'line',
                itemStyle: {
                    normal: {
                        color: new echarts.graphic.LinearGradient(
                            0, 0, 0, 1,
                            [
                                { offset: 0, color: '#83bff6' },
                                { offset: 0.5, color: '#188df0' },
                                { offset: 1, color: '#188df0' }
                            ]
                        )
                    },
                    emphasis: {
                        color: new echarts.graphic.LinearGradient(
                            0, 0, 0, 1,
                                [
                                    { offset: 0, color: '#2378f7' },
                                    { offset: 0.7, color: '#2378f7' },
                                    { offset: 1, color: '#83bff6' }
                                ]
                            )
                    }
                },
                data: flowDataYAxis,
                markPoint: {
                    label: {
                        normal: {
                            formatter: function (param) {
                                return param != null ? Math.round(param.value) : ''; 
                            }
                        }
                    },
                    data: [
                        {
                            name: 'highest value',
                            type: 'max'
                        },
                        {
                            name: 'lowest value',
                            type: 'min'
                        },
                    ],
                    tooltip: {
                        formatter: function (param) {
                            return param.name + '<br>' + (param.data.coord || '');
                        }
                    }
                },
                markLine: {
                    data: [
                        {
                            name: 'average value on close',
                            type: 'average'
                        }
                    ]
                }
            },
            //{
            //    name: '最大值',
            //    type: 'line',
            //    itemStyle: {
            //        normal: {
            //            color: new echarts.graphic.LinearGradient(
            //                0, 0, 0, 1,
            //                [
            //                    { offset: 0, color: '#48D1CC' },
            //                    { offset: 0.5, color: '#20B2AA' },
            //                    { offset: 1, color: '#008B8B' }
            //                ]
            //            )
            //        },
            //        emphasis: {
            //            color: new echarts.graphic.LinearGradient(
            //                0, 0, 0, 1,
            //                	[
            //                    	{ offset: 0, color: '#008B8B' },
            //                    	{ offset: 0.7, color: '#20B2AA' },
            //                    	{ offset: 1, color: '#48D1CC' }
            //                	]
            //            	)
            //        }
            //    },
            //    data: maxDataYAxis,
            //    markPoint: {
            //        label: {
            //            normal: {
            //                formatter: function (param) {
            //                    return param != null ? Math.round(param.value) : '';
            //                }
            //            }
            //        },
            //        data: [
            //            {
            //                name: 'highest value',
            //                type: 'max'
            //            },
            //            {
            //                name: 'lowest value',
            //                type: 'min'
            //            },
            //            {
            //                name: 'average value on close',
            //                type: 'average'
            //            }
            //        ],
            //        tooltip: {
            //            formatter: function (param) {
            //                return param.name + '<br>' + (param.data.coord || '');
            //            }
            //        }
            //    },
            //},
            //{
            //    name: '最小值',
            //    type: 'line',
            //    itemStyle: {
            //        normal: {
            //            color: new echarts.graphic.LinearGradient(
            //                0, 0, 0, 1,
            //                [
            //                    { offset: 0, color: '#F5DEB3' },
            //                    { offset: 0.5, color: '#FFE4B5' },
            //                    { offset: 1, color: '#FFA500' }
            //                ]
            //            )
            //        },
            //        emphasis: {
            //            color: new echarts.graphic.LinearGradient(
            //                0, 0, 0, 1,
            //                	[
            //                    	{ offset: 0, color: '#FFA500' },
            //                    	{ offset: 0.7, color: '#FFE4B5' },
            //                    	{ offset: 1, color: '#F5DEB3' }
            //                	]
            //            	)
            //        }
            //    },
            //    data: minDataYAxis,
            //    markPoint: {
            //        label: {
            //            normal: {
            //                formatter: function (param) {
            //                    return param != null ? Math.round(param.value) : '';
            //                }
            //            }
            //        },
            //        data: [
            //            {
            //                name: 'highest value',
            //                type: 'max'
            //            },
            //            {
            //                name: 'lowest value',
            //                type: 'min'
            //            },
            //            {
            //                name: 'average value on close',
            //                type: 'average'
            //            }
            //        ],
            //        tooltip: {
            //            formatter: function (param) {
            //                return param.name + '<br>' + (param.data.coord || '');
            //            }
            //        }
            //    },
            //},
        ]
    };
    //使用刚制定的配置项和数据项显示图表
    myChartLine.setOption(optionLine);
}