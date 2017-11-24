class DdItem extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        const { itemInfo, data } = this.props;
        let value = null;
        try {
            value = eval('data.' + itemInfo.value)
        } catch (err) {
            value = null;
        }
        return (
            <li className="list-group-item" key={ itemInfo.id }>
                <span className="pull-right">{ value || '未知' }</span>
                { itemInfo.label } 
            </li>
        );
    }
}