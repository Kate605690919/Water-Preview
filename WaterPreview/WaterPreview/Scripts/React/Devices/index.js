class Devices extends React.Component {
    constructor(props) {
        super(props);
        this.props.onChangeJsTree({
            name: 'select_node',
            function: (e, data) => {
                if (e) {
                    let a = document.getElementById('areaUid');
                    a.setAttribute('data-uid', data.selected[0]);
                    a.click();
                }
            }
        });
    }
    render() {
        const { FM, PM, QM, device } = this.props;
        return (
            <div className="devices">
                <ToolBar />
                <section>
                    <DeviceCard header={FM.header} tableInfo={FM.tableInfo} status={FM.status} warn={device.indexOf('FM') + 1} />
                    <DeviceCard header={PM.header} tableInfo={PM.tableInfo} status={FM.status} warn={device.indexOf('PM') + 1} />
                    <DeviceCard header={QM.header} tableInfo={QM.tableInfo} status={FM.status} warn={device.indexOf('QM') + 1} />
                </section>
            </div >
        );
    }
}
const mapStateToProps = (state, ownProps) => {
    return {
        FM: state.FM,
        QM: state.QM,
        PM: state.PM,
        device: state.device
    };
};
const mapDispatchToProps = (dispatch) => {
    return {
        onChangeJsTree: (event) => {
            dispatch(changeJstree(event));
        }
    }
};

Devices = ReactRedux.connect(mapStateToProps, mapDispatchToProps)(Devices);