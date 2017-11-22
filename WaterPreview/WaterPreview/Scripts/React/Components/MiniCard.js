class MiniCard extends React.Component{
    constructor(props) {
        super(props);
    }
    render() {
        return (
            <div className="ibox MiniCard">
                <div className="ibox-title" style={{padding: '0 10px', minHeight: '30px'}}>
                    <h5 className="no-margin" style={{lineHeight: '30px'}}>{this.props.bigH.header}</h5>
                    <h5 className="pull-right no-margin" style={{ lineHeight: '30px'}}>{this.props.bigH.content}</h5>
                </div>
                <div className="ibox-content" style={{padding: '10px 15px'}}>
                    <div className="stat-percent font-bold text-success">{this.props.smallH.content}<i className={`fa fa-level-${parseInt(this.props.smallH.content) >= 0 ? 'up' : 'down'}`}></i></div>
                    <small>{this.props.smallH.header}</small>
                </div>
            </div>
        );
    }
}