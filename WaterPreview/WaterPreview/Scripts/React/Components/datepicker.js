class DatePicker extends React.Component {
    constructor(props) {
        super(props);
    }

    componentDidMount() {
        $('#calender .input-daterange').datepicker({
            format: "yyyy/mm/dd",
            keyboardNavigation: false,
            forceParse: false,
            autoclose: true,
            language: 'cn'
        });
    }

    shouldComponentUpdate() {
        return false;
    }
    render() {
        return (
            <div className="input-daterange input-group" id="datepicker" style={{ marginRight: '10px' }}>
                <input type="text" className="input-sm form-control" name="start" value="2016/03/01" />
                <span className="input-group-addon">至</span>
                <input type="text" className="input-sm form-control" name="end" value="2016/05/28" />
            </div>
        );
    }
}