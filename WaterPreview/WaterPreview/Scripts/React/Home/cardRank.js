class CardRank extends React.Component {
    constructor(props) {
        super(props);
        this.state = { PMViewLog: { status: 'loadding' }, ViewLog: { status: 'loadding' } };
    }
    componentWillReceiveProps(nextProps, nextState) {
        this.getList({
            url: '/FlowMeter/GetLastDayFlowList', stateName: 'ViewLog'
        });
        this.getList({
            url: '/PressureMeter/GetLastDayPressureList', stateName: 'PMViewLog'
        });
    }
    getList({ url, stateName }) {
        const _this = this;
        (async () => {
            try {
                let res = await $Fetch.fetchSync_Get(url);
                _this.setState({ [stateName]: { data: res, status: 'success' } });
            } catch (err) {
                _this.setState({ [stateName]: { error: err, status: 'failure' } });
            }
        })();
    }
    renderList({ state, cols }) {
        if (state == null || state.status === 'loadding') {
            return <Loading />;
        } else if (state.status === 'success') {
            return state.data.map((item, index, arr) => {
                return (
                    <li className="list-group-item" style={{ display: 'flex', 'justifyContent': 'space-between', 'borderTop': '1px solid #e7eaec' }}>
                        <span className={`label label-${cols[0][0]}`}>{cols[0][1]}</span>
                        <span style={{ 'width': '120px' }}>{eval(`item.${cols[1]}`)}</span>
                        <span style={{ 'width': '70px' }}>{eval(`item.${cols[2]}`)}<i className={`fa fa-level-${parseInt(eval(`item.${cols[3]}`)) >= 0 ? 'up' : 'down'}`}></i></span>
                    </li>);
            });
        } else if (state.status === 'failure') {
            return <h5>加载失败...</h5>;
        }
    }
    render() {
        let { PMViewLog, ViewLog } = this.state;
        let flowList = this.renderList({ state: ViewLog, cols: [['success', '流量计'], 'flowmeter.FM_Description', 'lastday_flow_proportion'] });
        let pressureList = this.renderList({ state: PMViewLog, cols: [['info', '压力计'], 'pressuremeter.PM_Description', 'lastday_pressure_proportion'] });

        return (
            <div className="commonDevice" style={{'width': '320px'}} >
                <h3 style={{marginBottom: '10px'}}>昨日流量/压力变化排行</h3>
                <ul className="list-group clear-list m-t" style={{ minHeight: '270px', 'marginTop': '0' }}>
                    <li className="list-group-item" style={{ display: 'flex', 'justifyContent': 'space-between', 'borderTop': '1px solid #e7eaec', 'color': 'rgb(158, 158, 158)' }}>
                        <span style={{ 'width': '60px' }}>类型</span>
                        <span style={{ 'width': '120px' }}>名称</span>
                        <span style={{ 'width': '90px' }}>昨日变化趋势</span>
                    </li>
                    {flowList}
                    {pressureList}
                </ul>
            </div>
        );
    }
}
const mapStateToProps = (state) => {
    return {
        renderFlag: state.home.DeviceViewRank.renderFlag
    };
};
CardRank = ReactRedux.connect(mapStateToProps)(CardRank);

