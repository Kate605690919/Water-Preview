class FormItem extends React.Component {
    constructor(props) {
        super(props);
        this.onInputChange = this.onInputChange.bind(this);
        this.state = { value: this.props.itemInfo.input.value };
    }

    onInputChange(e) {
        this.setState({ value: e.target.value });
    }

    render() {
        let { label, id, input, type } = this.props.itemInfo,
            formId = this.props.formId || null,
            help = input.help ? <p className="addAeraHelp">{input.help}</p> : null;
        type = type || 'show',
            input.type = input.type || 'text';
        input.placeholder = input.placeholder || null;
        return (
            <select multiple="multiple" size="10" name="duallistbox_demo1[]">
                <option value="option1">Option 1</option>
                <option value="option2">Option 2</option>
                <option value="option3" selected="selected">Option 3</option>
                <option value="option4">Option 4</option>
                <option value="option5">Option 5</option>
                <option value="option6" selected="selected">Option 6</option>
                <option value="option7">Option 7</option>
                <option value="option8">Option 8</option>
                <option value="option9">Option 9</option>
                <option value="option0">Option 10</option>
            </select>
        );
    }
}