class Staff extends React.Component {
    constructor(props) {
        super(props);
        this.onClick = this.onClick.bind(this);
        this.props.onChangeJsTree({
            name: 'select_node',
            function: (e, data) => {
                if (data.node) {
                    const areaUid = data.node.id;
                    staffAreaUid.setAttribute('data-uid', areaUid);
                    staffAreaUid.click();
                }
            }
        });
    }
    onClick(e) {
        if (confirm("确定删除该条信息？")) {
            const id = e.target.getAttribute('data-id');
            this.props.onGetDevice(id);
        } 
    }
    render() {
        const { tableInfo, tableStatus, detailInfo, detailStatus } = this.props;
        return (
            <div className="manage">
                <Card header={tableInfo.header} status={tableStatus} warn={true}>
                    <Table tableInfo={tableInfo} />
                </Card>
                <button className="hidden" id={'table-' + this.props.tableInfo.el.substr(1)} onClick={this.onClick}></button>
                <Footer />
            </div >
        );
    }
}
const mapStateToProps = (state, ownProps) => {
    console.log(state.Staff.tableInfo)
    return {
        tableInfo: state.Staff.tableInfo,
        detailInfo: state.Staff.detailInfo,
        tableStatus: state.Staff.tableInfo.status,
        detailStatus: state.Staff.detailInfo.status
    };
};
const mapDispatchToProps = (dispatch) => {
    return {
        onGetDevice: (id) => {
            dispatch(staffDelete(id));
        },
        onChangeJsTree: (event) => {
            dispatch(changeJstree(event));
        }
    }
};

Staff = ReactRedux.connect(mapStateToProps, mapDispatchToProps)(Staff);
