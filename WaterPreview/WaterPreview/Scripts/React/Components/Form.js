class Form extends React.Component {
    constructor(props) {
        super(props);
    }

    componentDidMount() {
        if (this.props.formInfo.func) {
            let form = this.refs.form;
            let _this = this;
            let { url, data, successFunc } = this.props.formInfo.func;
            $(form).submit((e) => {
                e.preventDefault();
                $.ajax({
                    url: url,  //数据传输到哪里
                    type: "POST",
                    dataType: "json",
                    data: eval(data), //跟随后台的数据格式
                    success: function (res) {
                        successFunc(res);
                    }
                })
            });
        }
        else {
            $(form).submit((e) => {
                e.preventDefault();
            });
        }
    }
    render() {
        let formEls = null, btn = null, value = null, _this = this;
        let bindMap = this.props.formInfo.itemInfo.map.bind(this);
        //if (!this.props.formInfo.data) {
        //    return false;
        //} else {
            try {
                formEls = this.props.formInfo.itemInfo.map(function (item) {
                    try {
                        value = eval(`_this.props.formInfo.data.${item.input.value}`);
                    } catch (error) {
                        value = null;
                    }
                    return <FormItem itemInfo={item} formId={_this.props.formInfo.formId} value={value} />;
                });
            } catch (error) {

            }
        //}

        //try {
        //    debugger;
        //    formEls = this.props.formInfo.itemInfo.map(function(item) {
        //        value = eval(`_this.props.formInfo.data.${item.input.value}`);
        //        return <FormItem itemInfo={item} formId={_this.props.formInfo.formId} value={value} />;
        //    });
        //    } catch (error) {
        //    if (error instanceof TypeError) {
        //        return <FormItem itemInfo={item} formId={_this.props.formInfo.formId} value={null} />;
        //    }
        //}
        if (this.props.formInfo.btnContent) {
            btn = <button className="btn btn-md btn-primary pull-left m-t-n-xs" type="submit" style={{ backgroundColor: 'rgb(0, 198, 224)', borderColor: 'rgb(0, 188, 212)' }}> <strong>{ this.props.formInfo.btnContent }</strong></button >
        } else {
            btn = null;
        }
        return (
            <form id={this.props.formInfo.formId} className='form-horizontal' method='post' ref="form" >
                {formEls}
                <div className="form-group">
                    <div className="col-sm-offset-2 col-sm-10">
                        {btn}
                    </div>
                </div>
            </form>
        );
    }
}