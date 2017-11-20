class ManageFM extends React.Component {
    constructor(props) {
        super(props);
        let _this = this;
        this.onSubmit = this.onSubmit.bind(this);
        this.state = { unbindedFM: null, detail: null };
        fetch(`/client/GetDetail`, { method: 'POST', headers: { 'Content-Type': 'application/x-www-form-urlencoded', }, body: _this.props.params.uid }).then((response) => {
            if (response.status !== 200) {
                throw new Error('Fail to get response with status ' + response.status);
            }
            response.json().then((res) => {
                $.get(`/client/GetFlowMeter`, function (e) {
                    _this.setState({ unbindedFM: e, detail: res });
                })

            }).catch((error) => {
                console.error(error);
            });
        }).catch((error) => {
            console.error(error);
        });
    }
    onSubmit(e) {
        e.preventDefault();
        let _this = this;
        $.ajax({
            url: '/client/ModifyClientFlowMeter',
            type: "POST",
            dataType: "json",
            data: $('#form').serialize() + '&' + _this.props.params.uid,
            success: function (res) {
                if (res) {
                    alert('修改成功！');
                    window.location.hash = '/Client';
                } else {
                    alert('修改失败，请重试！');
                }
            }
        })
    }
    componentDidUpdate() {
        const { detail, unbindedFM } = this.state;
        let unbinded = unbindedFM, binded = detail.flowmeter;
        if (!!unbinded) {
            unbinded = unbinded.map((item) => {
                return `<option value=${item.FM_Id}>${item.FM_Code}:${item.FM_Description}</option>`;
            });
        }
        if (binded.length !== 0) {
            binded = binded.map(item => {
                return `<option value=${item.FM_Id} selected="selected" >${item.FM_Code}:${item.FM_Description}</option>`;
            });
        }
        let select = unbinded ? unbinded.concat(binded).join('') : null;
        const $dualistbox = $('select[name="id"]');
        $dualistbox.append(select);
        var demo1 = $dualistbox.bootstrapDualListbox({
            nonSelectedListLabel: '未绑定流量计',
            selectedListLabel: '已绑定流量计'
        });
    }
    render() {
        const { detail, unbindedFM} = this.state;
        let { detailInfo } = this.props;
        detailInfo.data = detail;
        return (
            <div className="manageDetail">
                <Card header={detailInfo.header} status={'success'} warn={true} init='Tip: 点击表中用户名可在此处查看客户详情'>
                    {detailInfo.data ? (
                        <form id="form" action="#" className="wizard-big" onSubmit={this.onSubmit} >
                        <div className="form-group">
                            <select multiple="multiple" size="10" name="id" ></select>
                        </div>
                        <div class="form-group">
                            <button class="btn btn-sm btn-private" type="submit">提交修改</button>
                        </div>
                        </form>) : <h3>Tip: 点击表中用户名可在此处查看客户详情</h3>}
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

ManageFM = ReactRedux.connect(mapStateToProps, mapDispatchToProps)(ManageFM);