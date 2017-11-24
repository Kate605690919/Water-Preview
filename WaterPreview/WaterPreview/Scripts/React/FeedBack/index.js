class FeedBackApp extends React.Component {
    constructor(props) {
        super(props);
        this.onSubmit = this.onSubmit.bind(this);
        this.onInputChange = this.onInputChange.bind(this);
        this.state = { value: '' };
    }

    onInputChange(e) {
        this.setState({ value: e.target.value });
    }

    onSubmit(e) {
        e.preventDefault();
        $.ajax({
            type: "POST",
            url: '/Home/GetUserFeedBack',
            dataType: "json",
            data: { uid: userUid.value, data: feedback.value },
            success: function (e) {
                if (e) alert('提交成功！');
                else alert('提交失败！');
            }
        });
    }
    render() {
        return (
            <div id="App">
                <article class="ibox">
                    <header className="ibox-title">
                        <Header header={this.props.header} />
                    </header>
                    <article className='ibox-content' style={{overflow: 'hidden'}}>
                        <form id="form" action="#" className="wizard-big" onSubmit={this.onSubmit} >
                            <fieldset>
                                <div className="form-group col-sm-offset-2 col-sm-10">
                                    <textarea name="confirm" type="text" className="form-control" id="feedback" placeholder="请填写您的反馈信息" value={this.state.value} onChange={this.onInputChange} required />
                                    <input name="confirm" type="hidden" value={this.state.value} />
                                </div>
                            </fieldset>
                            <div className="form-group col-sm-offset-2 col-sm-10">
                                <button className="btn btn-sm btn-primary" type="submit">提交修改</button>
                            </div>
                        </form>
                    </article>
                </article>
            </div>
        );
    }
}
const mapStateToProps = (state) => {
    return {
        header: state.FeedBack.header
    };
};
FeedBackApp = ReactRedux.connect(mapStateToProps)(FeedBackApp);