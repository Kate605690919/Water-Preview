class StaffPW extends React.Component {
    constructor(props) {
        super(props);
        this.state = { FM: null, detail: null };
        let _this = this;
        this.props.onChangeJsTree({
            name: 'select_node',
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
        fetch(`/staff/GetDetail`, { method: 'POST', headers: { 'Content-Type': 'application/x-www-form-urlencoded', }, body: _this.props.params.uid }).then((response) => {
            if (response.status !== 200) {
                throw new Error('Fail to get response with status ' + response.status);
            }
            response.json().then((res) => {
                _this.setState({ detail: res });
            }).catch((error) => {
                console.error(error);
            });
        }).catch((error) => {
            console.error(error);
        });
    }
    render() {
        let { editInfo } = this.props;
        editInfo.data = this.state.detail;
        editInfo.header.title[1].href = `/Staff/detail/${this.props.params.uid}`;
        return (
            <div className="ManageEdit">
                <Card header={editInfo.header} status={'success'} warn={true} init='Tip: 点击表中用户名可在此处查看客户详情'>
                    {editInfo.data ? <Form formInfo={editInfo} uid={this.props.params.uid ? this.props.params.uid.substr(4) : null} /> : null}
                </Card>
            </div>
        );
    }
}
const mapStateToProps = (state) => {
    if (window.sessionStorage) {
        var data = window.sessionStorage.getItem("detail");
    }
    return {
        editInfo: state.Staff.editInfo
    };
};
const mapDispatchToProps = (dispatch) => {
    return {
        onChangeJsTree: (event) => {
            dispatch(changeJstree(event));
        }
    }
}
StaffPW = ReactRedux.connect(mapStateToProps, mapDispatchToProps)(StaffPW);
