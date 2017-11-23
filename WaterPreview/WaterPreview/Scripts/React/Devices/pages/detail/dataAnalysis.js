class DataAnalysis extends React.Component {
    constructor(props) {
        super(props);
        this._uid = this.props.uid;
        this.state = { detail: null };
        
    }
    componentDidMount() {
        let _this = this;
        //不在这个时候触发的话，会检测不到宽度，导致图没有宽度就看不到
        //但是这样就会造成另一个问题，每次点击都会get一遍，应该旨在第一次点击的时候有用
        //解决方法，在绑定的handler里面去掉这个handler的监听即可~
        //let initHandler = function (e) { _this.init(); };
        $('a[href="#tab-2"]').on('shown.bs.tab', _this, _this.init);
    }

    init(e) {
        const _this = e.data;
        //$.get(`/flowmeter/RecentFlowData?${_this._uid}`, function (data) {
        debugger;
        $.post(_this.props.url, _this._uid, function (data) {
            _this.setState({ detail: data });
        });
        $('a[href="#tab-2"]').off('shown.bs.tab', _this.init);
    }

    render() {
        if (!this.state.detail) { return <div>正在加载中...</div> }
        else {
            return (
                <div className="dataAnalysis">
                    <div className="row" style={{ padding: '10px 15px' }}>
                        <HeatMapFm data={this.state.detail} />
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