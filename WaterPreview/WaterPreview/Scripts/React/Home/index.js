class Home extends React.Component {
    constructor(props) {
        super(props);
    //    this.state = { PMViewLog: null, ViewLog: null, QMViewLog: null, FlowList: null, PressureList: null }
    //    if (window.localStorage) {
    //        this._viewLog = localStorage.getItem('viewLog')?JSON.parse(localStorage.getItem('viewLog')).slice(0,3).map((item, index) => { return `fmUids=${item.uid}`; }):[];
    //this._PMViewLog = localStorage.getItem('PMViewLog') ? JSON.parse(localStorage.getItem('PMViewLog')).slice(0,2).map((item, index) => { return `pmUids=${item.uid}`; }) : [];
    //        this._QMViewLog = localStorage.getItem('QMViewLog') ? JSON.parse(localStorage.getItem('QMViewLog')).slice(0,1) : null;
    //    }
        //this._viewLog = this._viewLog.map((item, index) => { return `fmUids=${item.uid}`; });
        //this._PMViewLog = this._PMViewLog.map((item, index) => { return `pmUids=${item.uid}`; });
        //let _this = this;

        //fetch(`/FlowMeter/GetLastDayFlowList`).then((response) => {
        //    if (response.status !== 200) {
        //        throw new Error('Fail to get response with status ' + response.status);
        //    }
        //    response.json().then((res) => {
        //        res = JSON.parse(res);
        //        console.log('flowlist',res);
        //        _this.setState({ FlowList: res });
        //    }).catch((error) => {
        //        console.error(error);
        //    });
        //}).catch((error) => {
        //    console.error(error);
        //    });

    //    fetch(`/PressureMeter/GetLastDayPressureList`).then((response) => {
    //        if (response.status !== 200) {
    //            throw new Error('Fail to get response with status ' + response.status);
    //        }
    //        response.json().then((res) => {
    //            res = JSON.parse(res);
    //            console.log('pressure', res);
    //            _this.setState({ PressureList: res });
    //        }).catch((error) => {
    //            console.error(error);
    //        });
    //    }).catch((error) => {
    //        console.error(error);
    //        });
    }
    componentDidMount() {
        //地图
        var idMap = document.querySelector("#homeMap");
        initMap(idMap);
    }
    render() {
        return (
            <div className="homeBody" style={{ width: '100%'}}>
                <div className="commonData">
                    <CardListItem />
                    <CardRank />
                    <div id="homeMap" style={{ marginBottom: '10px' }}></div>
                </div>
                <Footer />
            </div>
        );
    }
}

//render() {
//    let { ViewLog, PMViewLog, QMViewLog } = this.state;
//    return (
//        <div className="homeBody" style={{ width: '100%' }}>
//            <div className="commonData">
//                <div className="commonDevice">
//                    <div style={{ color: 'rgb(158, 158, 158)', marginBottom: '10px', fontSize: '14px' }}># 常用流量计</div>
//                    <CardListItem chart={false} bigH={
//                        ViewLog ? (ViewLog[0] ? { header: ViewLog[0].flowmeter.FM_Description, content: parseInt(ViewLog[0].lastday_flow).toFixed(2) } : { header: '暂无', content: '暂无' }) : { header: '加载中...', content: '加载中...' }
//                    } smallH={
//                        { header: '昨日总流量', content: ViewLog ? (ViewLog[0] ? ViewLog[0].lastday_flow_proportion : '暂无') : '加载中...' }
//                    } />
//                    <CardListItem chart={false} bigH={
//                        ViewLog ? (ViewLog[1] ? { header: ViewLog[1].flowmeter.FM_Description, content: parseInt(ViewLog[1].lastday_flow).toFixed(2) } : { header: '暂无', content: '暂无' }) : { header: '加载中...', content: '加载中...' }
//                    } smallH={
//                        { header: '昨日总流量', content: ViewLog ? (ViewLog[1] ? ViewLog[1].lastday_flow_proportion : '暂无') : '加载中...' }
//                    } />
//                    <CardListItem chart={false} bigH={
//                        ViewLog ? (ViewLog[2] ? { header: ViewLog[2].flowmeter.FM_Description, content: parseInt(ViewLog[2].lastday_flow).toFixed(2) } : { header: '暂无', content: '暂无' }) : { header: '加载中...', content: '加载中...' }
//                    } smallH={
//                        { header: '昨日总流量', content: ViewLog ? (ViewLog[2] ? ViewLog[2].lastday_flow_proportion : '暂无') : '加载中...' }
//                    } />
//                </div>
//                <div className="commonDevice">
//                    <div style={{ color: 'rgb(158, 158, 158)', marginBottom: '10px', fontSize: '14px' }}># 常用水压计</div>
//                    <CardListItem chart={false} bigH={
//                        PMViewLog ? (PMViewLog[0] ? { header: PMViewLog[0].pressuremeter.PM_Description, content: parseInt(PMViewLog[0].lastday_pressure).toFixed(2) } : { header: '暂无', content: '暂无' }) : { header: '加载中...', content: '加载中...' }
//                    } smallH={
//                        { header: '昨日总流量', content: PMViewLog ? (PMViewLog[0] ? PMViewLog[0].lastday_pressure_proportion : '暂无') : '加载中...' }
//                    } />
//                    <CardListItem chart={false} bigH={
//                        PMViewLog ? (PMViewLog[1] ? { header: PMViewLog[1].pressuremeter.PM_Description, content: parseInt(PMViewLog[1].lastday_pressure).toFixed(2) } : { header: '暂无', content: '暂无' }) : { header: '加载中...', content: '加载中...' }
//                    } smallH={
//                        { header: '昨日总流量', content: PMViewLog ? (PMViewLog[1] ? PMViewLog[1].lastday_pressure_proportion : '暂无') : '加载中...' }
//                    } />
//                </div>
//                <div className="commonDevice">
//                    <div style={{ color: 'rgb(158, 158, 158)', marginBottom: '10px', fontSize: '14px' }}># 常用水质计</div>
//                    <CardListItem chart={false} bigH={
//                        PMViewLog ? (PMViewLog[0] ? { header: PMViewLog[0].pressuremeter.PM_Description, content: parseInt(PMViewLog[0].lastday_pressure).toFixed(2) } : { header: '暂无', content: '暂无' }) : { header: '加载中...', content: '加载中...' }
//                    } smallH={
//                        { header: '昨日总流量', content: PMViewLog ? (PMViewLog[0] ? PMViewLog[0].lastday_pressure_proportion : '暂无') : '加载中...' }
//                    } />
//                    <CardListItem chart={false} bigH={
//                        PMViewLog ? (PMViewLog[1] ? { header: PMViewLog[1].pressuremeter.PM_Description, content: parseInt(PMViewLog[1].lastday_pressure).toFixed(2) } : { header: '暂无', content: '暂无' }) : { header: '加载中...', content: '加载中...' }
//                    } smallH={
//                        { header: '昨日总流量', content: PMViewLog ? (PMViewLog[1] ? PMViewLog[1].lastday_pressure_proportion : '暂无') : '加载中...' }
//                    } />
//                </div>
//                <div id="homeMap" style={{ marginBottom: '10px' }}></div>
//            </div>
//            <Footer />
//        </div>
//    );
//}