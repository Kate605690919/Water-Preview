class Home extends React.Component {
    constructor(props) {
        super(props);
        if (window.localStorage) {
            this._viewLog = localStorage.getItem('viewLog')?JSON.parse(localStorage.getItem('viewLog')):null;
            this._PMViewLog = localStorage.getItem('PMViewLog') ? JSON.parse(localStorage.getItem('PMViewLog')) : null;
            this._QMViewLog = localStorage.getItem('QMViewLog') ? JSON.parse(localStorage.getItem('QMViewLog')) : null;
        }
    }
    componentDidMount() {
        //地图
        var idMap = document.querySelector("#homeMap");
        initMap(idMap);
    }
    render() {
        return (
            <div className="homeBody" style={{ width: '100%' }}>
                <div className="row">
                    <div className="col-md-2 homeCard">
                        <MiniCard chart={true} bigH={{ header: this._viewLog ? (this._viewLog[0] ? this._viewLog[0].uid : '暂无') : '暂无', content: '706.159' }} smallH={{ header: '昨日总流量', content: '3%' }} />
                    </div>
                    <div className="col-md-2 homeCard">
                        <MiniCard chart={true} bigH={{ header: this._viewLog ? (this._viewLog[1] ? this._viewLog[1].uid : '暂无') : '暂无', content: '604.531' }} smallH={{ header: '昨日总流量', content: '2%' }} />
                    </div>
                    <div className="col-md-2 homeCard">
                        <MiniCard chart={true} bigH={{ header: this._viewLog ? (this._viewLog[2] ? this._viewLog[2].uid : '暂无') : '暂无', content: '123.7969' }} smallH={{ header: '昨日总流量', content: '5%' }} />
                    </div>
                    <div className="col-md-2 homeCard">
                        <MiniCard chart={true} bigH={{ header: this._PMViewLog ? (this._PMViewLog[0] ? this._PMViewLog[0].uid : '暂无') : '暂无', content: '21184.59' }} smallH={{ header: '昨日总流量', content: '40%' }} />
                    </div>
                    <div className="col-md-2 homeCard">
                        <MiniCard chart={true} bigH={{ header: this._PMViewLog ? (this._PMViewLog[1] ? this._PMViewLog[1].uid : '暂无') : '暂无', content: '21184.59' }} smallH={{ header: '昨日总流量', content: '40%' }} />
                    </div>
                    <div className="col-md-2 homeCard">
                        <MiniCard chart={true} bigH={{ header: this._QMViewLog ? (this._QMViewLog[0] ? this._QMViewLog[0].uid : '暂无') : '暂无', content: '21184.59' }} smallH={{ header: '昨日总流量', content: '40%' }} />
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