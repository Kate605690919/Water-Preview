class HeaderTop extends React.Component {
    constructor(props) {
        super(props);
    }
    componentWillMount() {
        this._roleName = $('#roleName').val();
    }
    render() {
        return (
            <header className="navbar navbar-default navbar-fixed-top" role="navigation">
                <div className="navbar-header">
                    <button className="navbar-toggle" type="button" data-toggle="collapse" data-target=".navbar-responsive-collapse">
                        <span className="sr-only">Toggle Navigation</span>
                        <span className="icon-bar"></span>
                        <span className="icon-bar"></span>
                        <span className="icon-bar"></span>
                    </button>
                    <Link to="/" className="navbar-brand" activeClassName="active">智慧水务</Link>
                </div>
                <div className="collapse navbar-collapse navbar-responsive-collapse">
                    <ul className="nav navbar-nav">
                        <li><Link to="/Devices" id="flowMeterTitle" activeClassName="active">设备</Link></li>
                        <li><Link to="/feedback" id="flowMeterTitle" activeClassName="active">反馈</Link></li>
                    </ul>
                    <ul className="navbar navbar-nav navbar-right">
                        <li><a href="#"><span className="userLogined"> {this._roleName}</span></a></li>
                        <li><a href="/Home/login"><span className="glyphicon glyphicon-log-out"></span></a></li>
                    </ul>
                </div>
            </header>
        );
    }
}