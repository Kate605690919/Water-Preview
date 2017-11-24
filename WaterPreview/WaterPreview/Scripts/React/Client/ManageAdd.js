class ManageAdd extends React.Component {
    constructor(props) {
        super(props);
        this.props.onChangeJsTree({
            name: 'activate_node',
            function: (e, data) => {
                if (data.node) {
                    $('#formClient20 ').val(data.node.original.text);
                    AreaTreeId = data.node.original.id;
                    if (window.sessionStorage) {
                        var storage = window.sessionStorage;
                        storage.setItem("AreaTreeId", AreaTreeId);
                    }
                }
            }
        });
    }

    componentDidMount() {
        changeEvt = (e, data) => {
            if (data.node) {
                $('#formClient20 ').val(data.node.original.text);
                AreaTreeId = data.node.original.id;
            }
        }
    }

    render() {
        let { formInfo } = this.props;
        formInfo.data = {};
        return (
            <div className="ManageAdd">
                <Card header={formInfo.header} status={'success'} warn={true} init='Tip: 点击表中用户名可在此处查看客户详情'>
                    <p className="tips" style={{ padding: '5px', borderLeft: '3px rgb(255, 64, 129) solid' }}>
                        <h3>Tips: 职员默认密码是88888888,请务必提醒职员修改密码</h3>
                    </p>
                    <Form formInfo={formInfo} />
                </Card>
            </div >
        );
    }
}

const mapStateToProps = (state) => {
    return {
        formInfo: state.Client.formInfo
    };
};
const mapDispatchToProps = (dispatch) => {
    return {
        onChangeJsTree: (event) => {
            dispatch(changeJstree(event));
        }
    }
};

ManageAdd = ReactRedux.connect(mapStateToProps, mapDispatchToProps)(ManageAdd);
//<div className="ibox-title">
//    <Header header={this.props.formInfo.header} />
//</div>
//<div className='ibox-content'>
//    <Form formInfo={this.props.formInfo} />
//</div>