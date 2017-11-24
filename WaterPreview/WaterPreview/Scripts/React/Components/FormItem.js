class FormItem extends React.Component {
    constructor(props) {
        super(props);
        this.onInputChange = this.onInputChange.bind(this);
        this.state = { value: this.props.value };
    }

    onInputChange(e) {
        this.setState({ value: e.target.value } );
    }

    render() {
        let { label, id, input, type } = this.props.itemInfo,
            value = this.props.value,
            formId = this.props.formId || null,
            help = input.help ? <p className="col-sm-offset-2 col-sm-10" style={{marginTop: '10px'}}>{input.help}</p> : null;
        type = type || 'show',
        input.type = input.type || 'text';
        input.placeholder = input.placeholder || null;
        return (
            <div className={"form-group "+type} id={formId}>
                <label for={formId} className="col-sm-2 control-label">{label}</label>
                <div className="col-sm-10">
                    <input type={input.type} className="form-control" id={formId + id} name={input.name || null} placeholder={input.placeholder} value={this.state.value} readOnly={input.readOnly} onChange={this.onInputChange} required={input.require} />
                </div>
                {help}
            </div>
        );
    }
}