class ManageDetail extends React.Component {
    constructor(props) {
        super(props);
        let _this = this;
        this.state = { detail: null };
        fetch(`/client/GetDetail`, { method: 'POST', headers: { 'Content-Type': 'application/x-www-form-urlencoded', }, body: _this.props.params.uid }).then((response) => {
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
        const { detail } = this.state;
        let { detailInfo } = this.props;
        detailInfo.data = detail;
        detailInfo.header.btn[0].url = `/Client/Editbase/${this.props.params.uid}`;
        detailInfo.header.btn[1].url = `/Client/EditFM/${this.props.params.uid}`;
        return (
            <div className="manageDetail">
                <Card header={detailInfo.header} status={'success'} warn={true} init='Tip: 点击表中用户名可在此处查看客户详情'>
                    {detailInfo.data ? <Dd detailInfo={detailInfo} FM={true}/> : <h3>Tip: 点击表中用户名可在此处查看客户详情</h3>}
                </Card>
            </div>
        );
    }
}

const mapStateToProps = (state) => {
    return {
        detailInfo: state.Client.detailInfo,
        detailStatus: state.Client.detailInfo.status
    };
};

ManageDetail = ReactRedux.connect(mapStateToProps, mapDispatchToProps)(ManageDetail);