class ManageBase extends React.Component {
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
        fetch(`/client/GetDetail`, { method: 'POST', headers: { 'Content-Type': 'application/x-www-form-urlencoded', }, body: _this.props.params.uid }).then((response) => {
            if (response.status !== 200) {
                throw new Error('Fail to get response with status ' + response.status);
            }
            response.json().then((res) => {
                $.get(`/client/GetFlowMeter`, function (data) {
                    _this.setState({ FM: data, detail: res });
                });
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
        editInfo.header.title[1].href = `/Client/detail/${this.props.params.uid}`;
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
        editInfo: state.Client.editInfo
    };
};
const mapDispatchToProps = (dispatch) => {
    return {
        onChangeJsTree: (event) => {
            dispatch(changeJstree(event));
        }
    }
}
ManageBase = ReactRedux.connect(mapStateToProps, mapDispatchToProps)(ManageBase);




    //componentDidMount() {
    //    changeEvt = (e, data) => {
    //        if (data.node) {
    //            $('#formClient20 ').val(data.node.original.text);
    //            AreaTreeId = data.node.original.id;
    //            if (window.sessionStorage) {
    //                var storage = window.sessionStorage;
    //                storage.setItem("AreaTreeId", AreaTreeId);
    //            }
    //        }
    //    }
    //}
//componentWillUpdate() {
//    let unbinded = this.props.unbinded.FM, binded = this.props.binded;
//    if (!!unbinded) {
//        unbinded = unbinded.map((item) => {
//            return `<option value=${item.FM_Id}>${item.FM_Code}:${item.FM_Description}</option>`;
//        });
//    }
//    if (!!binded) {
//        binded = binded.map(item => {
//            return `<option value=${item.FM_Id} selected="selected" >${item.FM_Code}:${item.FM_Description}</option>`;
//        });
//    }
//    let select = unbinded ? unbinded.concat(binded).join('') : null;
//    const $dualistbox = $('select[name="id"]');
//    $dualistbox.append(select);
//    var demo1 = $dualistbox.bootstrapDualListbox({
//        nonSelectedListLabel: '未绑定流量计',
//        selectedListLabel: '已绑定流量计'
//    });
//}

//onInputChange(e) {
//    let name = e.target.name.slice(7);
//    this.props.unbinded.data[name] = e.target.value;
//    this.setState({ value: e.target.value });
//}

//onSubmit(e) {
//    e.preventDefault();
//    const uid = this.props.userUid,
//        AreaTreeId = window.sessionStorage.getItem("AreaTreeId");
//    console.log(uid, AreaTreeId)
//    $.ajax({
//        url: '/client/ModifyClient',
//        type: "POST",
//        dataType: "json",
//        data: $('#form').serialize() + '&Member_UserUid=' + uid + '&Member_AreaUid=' + AreaTreeId,
//        success: function (res) {
//            if (res) {
//                alert('修改成功！');
//                window.location.hash = '';
//            } else {
//                alert('修改失败，请重试！');
//            }
//        }
//    })
//}
//let data = this.props.unbinded.data;
//const { detailInfo, detailStatus } = this.props;
//console.log(this.props.unbinded["data"]);
//<div className="ibox">
//    <div className="ibox-title">
//        <Header header={this.props.editInfo.header} />
//    </div>
//    <div className="ibox-content">
//        <form id="form" action="#" className="wizard-big" onSubmit={this.onSubmit} >
//            <fieldset>
//                <h2>基本信息</h2>
//                <div className="form-group">
//                    <input id="formClient20" name="confirm" type="text" className="form-control required" placeholder="所属区域" value={data ? data.area ? data.area.Name : '' : ''} readOnly />
//                    <p className="addAeraHelp">温馨提示：点击左侧"区域参考"选择区域</p>
//                </div>
//                <div className="form-group">
//                    <input id="userName" name="Member_Name" type="text" className="form-control required" placeholder="客户名" value={data ? data.Name : ''} onChange={this.onInputChange} />
//                </div>
//                <div className="form-group">
//                    <input id="password" name="Member_RealName" type="text" className="form-control required" placeholder="真实姓名或公司名" value={data ? data.RealName : ''} onChange={this.onInputChange} />
//                </div>
//                <div className="form-group">
//                    <input id="confirm" name="Member_Phone" type="number" className="form-control required" placeholder="电话号码" value={data ? data.Phone : ''} onChange={this.onInputChange} />
//                </div>
//                <div className="form-group">
//                    <input id="confirm" name="Member_Memo" type="text" className="form-control required" placeholder="备注" value={data ? data.Memo : ''} onChange={this.onInputChange} />
//                </div>
//            </fieldset>
//            <fieldset>
//                <h2>绑定流量计</h2>
//                <div className="form-group">
//                    <select multiple="multiple" size="10" name="id" ></select>
//                </div>
//            </fieldset>
//            <div class="form-group">
//                <button class="btn btn-sm btn-private" type="submit">提交修改</button>
//            </div>
//        </form>
//    </div>
//</div>
//const mapStateToProps = (state) => {
//    if (window.sessionStorage) {
//        var userUid = window.sessionStorage.getItem("userUid");
//        var data = window.sessionStorage.getItem("detail");
//    }
//    console.log(state.Client.detailInfo.data);
//    return {
//        editInfo: state.Client.editInfo,
//        unbinded: state.Client.detailInfo,
//        userUid: state.userUid || userUid,
//        binded: eval(data),
//        detailInfo: state.Client.detailInfo,
//        detailStatus: state.Client.detailInfo.status
//    };
//};


