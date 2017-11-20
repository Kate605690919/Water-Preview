class Manage extends React.Component {
    constructor(props) {
        super(props);
        this.onClick = this.onClick.bind(this);
        this.props.onChangeJsTree({
            name: 'select_node',
            function: (e, data) => {
                if (data.node) {
                    const areaUid = data.node.id;
                    manageAreaUid.setAttribute('data-uid', areaUid);
                    manageAreaUid.click();
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
    return {
        tableInfo: state.Client.tableInfo,
        detailInfo: state.Client.detailInfo,
        tableStatus: state.Client.tableInfo.status,
        detailStatus: state.Client.detailInfo.status
    };
};
const mapDispatchToProps = (dispatch) => {
    return {
        onGetDevice: (id) => {
            dispatch(clientDelete(id));
        },
        onChangeJsTree: (event) => {
            dispatch(changeJstree(event));
        }
    }
};

Manage = ReactRedux.connect(mapStateToProps, mapDispatchToProps)(Manage);
