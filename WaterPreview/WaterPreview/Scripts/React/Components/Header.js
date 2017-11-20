class Header extends React.Component {
    constructor(props) {
        super(props);
        this.addEvent = this.addEvent.bind(this);
        this.editEvent = this.editEvent.bind(this);
    }
    addEvent(e) {
        let { url } = this.props.header.btn;
        window.open(url, '_self');
    }
    editEvent(e) {
        this.props.editManage();
    }

    render() {
        let { btn, content } = this.props.header, button = null;
        if (btn) {
            button = btn.map(item => {
                if (!!item.url) return <Link className="btn btn-primary" to={item.url} >{item.content}</Link>;
                else return <a className="btn btn-primary" onClick={eval(item.func)}></a>
            });
        }
        return (
            <div>
                <Breadcrumb title={this.props.header.title} />
                <div className="pull-right">
                    <div className="btn-group">
                        { button }
                    </div>
                </div>
            </div>
        );
    }
}

const mapDispatchToProps = (dispatch) => {
    return {
        editManage: () => {
            dispatch(editManage());
        }
    }
}

Header = ReactRedux.connect(null, mapDispatchToProps)(Header);