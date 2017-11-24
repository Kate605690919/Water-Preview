class Dd extends React.Component {
    constructor(props) {
        super(props);
    }

    componentDidMount() {
        
    }
    shouldComponentUpdate(nextProps, nextState) {
        return (nextProps.detailInfo !== this.props.detailInfo);
    }
    render() {
        let dds = null, data = this.props.detailInfo.data, fm = [];
        dds = this.props.detailInfo.itemInfo.map(item => {
            return <DdItem itemInfo={ item } data={ data } />;
        });
        if (data.flowmeter.length) {
            fm = data.flowmeter.map(item => {
                console.log(item.FM_Code);
                return (
                    <li className="list-group-item">
                        <span className="pull-right">{item.FM_Description || '未知'}</span>
                        {item.FM_Code}
                    </li>
                );
            })
        }
        return (
            <div className="full-height-scroll">
                <strong>{this.props.detailInfo.title}</strong>
                <ul className="list-group clear-list">
                    { dds }
                </ul>
                {this.props.FM ?
                    (
                    <div><strong>绑定流量计</strong>
                    <ul className="list-group clear-list">
                        {fm.length ? fm : '暂无'}
                    </ul></div>
                    ):null}
            </div>
        );
    }
}