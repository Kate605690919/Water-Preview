class DataCount extends React.Component {
    constructor(props) {
        super(props);
        this._uid = this.props.uid;
        this.state = { echartData: false, data: null, value1: '2014/05/28', value2: '2014/05/30', check: true };
        this.onSearch = this.onSearch.bind(this);
        this.onCheckedChange = this.onCheckedChange.bind(this);
    }
    componentDidMount() {
        let _this = this;
        this.init();
    }
    shouldComponentUpdate(nextProps, nextState) {
        //return (this.state.data !== nextState.data);
        return nextState.echartData === true || nextState.echartData === false;
    }
    onSearch(e) {
        this.init();
    }
    onCheckedChange(e) {
        let check = e.target.checked;
        this.setState({ check: check });
    }
    init() {
        var startDate = $("input[name='start']")[0].value;
        var endDate = $("input[name='end']")[0].value;
        let _this = this;
        fetch(`/flowmeter/currentData`, { method: 'POST', headers: { 'Content-Type': 'application/x-www-form-urlencoded', }, body: `${_this._uid}&startDt=${startDate}&endDt=${endDate}` }).then((response) => {
            if (response.status !== 200) {
                throw new Error('Fail to get response with status ' + response.status);
            }
            response.json().then((res) => {
                if (res.length !== 0) {
                    this.setState({ echartData: true , data: res});
                } else {
                    this.setState({ echartData: false });
                }
            }).catch((error) => {
                console.error(error);
            });
        }).catch((error) => {
            console.error(error);
        });
    }
    //去空值？
    render() {
        this.props.tableInfo.data = this.state.data;
        return (
            <div className="ibox-content" style={{ borderColor: 'transparent' }}>
                <div className="row" id="dataStat" >
                    <div className="form-group" id="calender" style={{ display: 'flex', float: 'left'}}>                                
                        <DatePicker />
                        <button className="btn btn-primary btn-sm" id="calenderBtn" onClick={this.onSearch} >搜索</button>
                    </div>
                    <div className="tg-list-item" style={{ display: 'flex', float: 'right' }}>
                        <h4 style={{ margin: '0px 10px', lineHeight: '30px' }}>图/表切换</h4>
                        <input className="tgl tgl-ios" id="cb2" type="checkbox" checked={this.state.check} onChange={this.onCheckedChange}/>
                        <label className="tgl-btn" for="cb2"></label>
                    </div>
                </div>
                {this.state.check ? (<div className="row">
                    <div className="col-md-12">
                        {this.state.echartData ? (<Table tableInfo={this.props.tableInfo} />) : <h3 style={{ textAlign: 'center' }}>暂无数据</h3>}
                    </div>
                </div>) : (<div className="row">
                    <div className="col-md-12">
                        {this.state.echartData ? (<EchartsLine data={this.state.data} />) : <h3 style={{ textAlign: 'center' }}>暂无数据</h3>}
                    </div>
                    </div>)
                }
            </div>
        );
    }
}
const mapStateToProps = (state) => {
    return {
        tableInfo: state.deviceDetail.dataCount.tableInfo
    };
};
DataCount = ReactRedux.connect(mapStateToProps)(DataCount);

//<Table tableInfo={this.props.tableInfo} />