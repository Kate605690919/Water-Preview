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
                    <a href="/Home/index" className="navbar-brand">智慧水务</a>
                </div>
                <div className="collapse navbar-collapse navbar-responsive-collapse">
                    <ul className="nav navbar-nav">
                        <li><Link to="/FlowMeter" id="flowMeterTitle"><span className="glyphicon glyphicon-tint"></span>设备</Link></li>
                        {role.value !== '3' ? (<li className="dropdown">
                            <a href="#" data-toggle="dropdown" className="drpown-toggle" id="cogTitle">
                                <span className="glyphicon glyphicon-cog"></span>管理<span className="caret"></span>
                            </a>
                            <ul className="dropdown-menu">
                                <li><Link to="/">区域管理</Link></li>
                                <li role="presentation" className="divider"></li>
                                <li><a href="#/client">客户管理</a></li>
                                <li><a href="#/staff">职员管理</a></li>
                                <li><Link to="javascript:void(0)">职位管理</Link></li>
                                <li><Link to="javascript:void(0)">权限管理</Link></li>
                                <li><Link to="javascript:void(0)">修改密码</Link></li>
                            </ul>
                        </li>) : null}
                        <li><Link to="/feedback" id="flowMeterTitle"><span className="glyphicon glyphicon-tint"></span>反馈</Link></li>
                    </ul>
                    <ul className="navbar navbar-nav navbar-right">
                        <li><a href="#"><span className="userLogined"> {`${this._roleName}已登录`}</span></a></li>
                        <li><a href="#"><span className="glyphicon glyphicon-refresh"></span>刷新</a></li>
                        <li><a href="#"><span className="glyphicon glyphicon-warning-sign"></span>异常</a></li>
                        <li><a href="/Home/login"><span className="glyphicon glyphicon-log-out"></span>退出</a></li>
                    </ul>
                </div>
            </header>
        );
    }
}