class DataStatus extends React.Component {
    constructor(props) {
        super(props);
    }
    render() {
        let status = this.props.status;
        if (!status) return <div>正在加载中...</div>
        else {
            return (
                <div className="dataStatus" style={{ display: 'flex', justifyContent: 'space-between'}}>
                    <div className="wrapper wrapper-content animated fadeInRight">
                        <div>
                            <span>主电源电量</span>
                            <small className="pull-right">{status.FMS_MainBatteryStatus * 100 + '%'} </small>
                        </div>
                        <div className="progress progress-small">
                            <div style={{ width: status.FMS_MainBatteryStatus * 100 + '%' }} className="progress-bar" id="mainBattery"></div>
                        </div>
                    </div>
                    <div className="wrapper wrapper-content animated fadeInRight">
                        <div>
                            <span>备用电源电量</span>
                            <small className="pull-right">{status.FMS_SecondaryBatteryStatus * 100 + '%'}</small>
                        </div>
                        <div className="progress progress-small">
                            <div style={{ width: status.FMS_SecondaryBatteryStatus * 100 + '%' }} className="progress-bar" id="secondaryBattery"></div>
                        </div>
                    </div>
                    <div className="wrapper wrapper-content animated fadeInRight">
                        <div>
                            <span>通信电池电量</span>
                            <small className="pull-right">{status.FMS_ModemBatteryStatus * 100 + '%'}</small>
                        </div>
                        <div className="progress progress-small">
                            <div style={{ width: status.FMS_ModemBatteryStatus * 100 + '%' }} className="progress-bar" id="modemBattery"></div>
                        </div>
                    </div>
                    <div className="wrapper wrapper-content animated fadeInRight">
                        <div>
                            <span>信号强度</span>
                            <small className="pull-right">{status.FMS_AntennaSignal * 100 + '%'}</small>
                        </div>
                        <div className="progress progress-small">
                            <div style={{ width: status.FMS_AntennaSignal * 100 + '%' }} className="progress-bar" id="Antenna"></div>
                        </div>
                    </div>
                </div>
            );
        }
    }
}