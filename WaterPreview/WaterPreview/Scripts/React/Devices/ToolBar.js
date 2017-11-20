class ToolBar extends React.Component {
    constructor(props) {
        super(props);
        this.onChange = this.onChange.bind(this);
    }

    onChange(evt) {
        const uid = this.props.areaUid;
        const checkboxs = document.getElementsByName('Meter');
        let devices = [];
        checkboxs.forEach((item) => {
            if (item.checked) {
                this.props[`onGet${item.value}`](uid);
                devices.push(item.value);
            }
        });
        this.props.onChangeDevice(devices);
    }

    render() {
        let checkBoxs = null;
        let device = this.props.device;
        checkBoxs = this.props.toolBar.map((item, index, arr) => {
            var check = !!(device.indexOf(item.value) + 1);
            return (<label style={{ marginRight: '5px' }}><input type="checkbox" name={item.name} value={item.value} onChange={this.onChange} checked={check} style={{marginRight: '3px', lineHeight: '20px'}}/>{item.label}</label>);
        });
        return (
            <div className="toolBar ibox-content" style={{marginBottom: '10px'}}>
                {checkBoxs}
            </div>
        );
    }
}

const mapStateToProps = (state, ownProps) => {
    return {
        toolBar: state.toolBar,
        status: state.status,
        areaUid: state.areaUid,
        device: state.device
    };
};

const mapDispatchToProps = (dispatch) => {
    return {
        onChangeDevice: (device) => {
            dispatch(changeDevice(device));
        },
        onGetFM: (uid) => {
            dispatch(getFM(uid));
        },
        onGetPM: (uid) => {
            dispatch(getPM(uid));
        },
        onGetQM: (uid) => {
            dispatch(getQM(uid));
        }
    }
};

ToolBar = ReactRedux.connect(mapStateToProps, mapDispatchToProps)(ToolBar);