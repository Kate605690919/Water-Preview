let { Checkbox } = antd;
const CheckboxGroup = Checkbox.Group;

const plainOptions = [
    { label: '流量计', value: 'FM' },
    { label: '压力计', value: 'PM' },
    { label: '水质计', value: 'QM' }
];
//const defaultCheckedList = ['FM', 'PM'];
class ToolBar extends React.Component {
    constructor(props) {
        super(props);
        this.onChange = this.onChange.bind(this);
        this.state = {
            checkedList: this.props.device,
            indeterminate: true,
            checkAll: false
        }
    }

    onChange(checkedList) {
        const uid = this.props.areaUid;
        checkedList.forEach((item) => {
                this.props[`onGet${item}`](uid);
        });
        this.props.onChangeDevice(checkedList);
        this.setState({
            checkedList,
            indeterminate: !!checkedList.length && (checkedList.length < plainOptions.length),
            checkAll: checkedList.length === plainOptions.length,
        });
    }
    render() {
        return (
            <div className="toolBar ibox-content" style={{ marginBottom: '10px' }}>
                <CheckboxGroup options={plainOptions} defaultValue={this.props.device} onChange={this.onChange} />
            </div>
        );
    }
}
//{ checkBoxs }
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