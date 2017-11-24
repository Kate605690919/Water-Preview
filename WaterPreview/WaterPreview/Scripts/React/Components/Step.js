class Step extends React.Component {
    constructor(props) {
        super(props);
    }

    componentDidMount() {
        let steps = this.props.steps;
        steps = this.props.steps || { name: 'onFinished', func: () => { console.log('no function has been binded') } };
        $('#wizard').steps({ [steps.name]: function () { eval(steps.func) } });
    }

    render() {
        let children = this.props.children;
        console.log(this.props.formInfo)
        return (
            <div id="wizard" ref="mySteps">
                <h1>基本信息</h1>
                <div className='step-content'>
                    <Form formInfo={this.props.formInfo} />
                </div>
            </div>
        );
    }
}