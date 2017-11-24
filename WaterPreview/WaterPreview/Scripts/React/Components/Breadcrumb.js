class Breadcrumb extends React.Component {
    constructor(props) {
        super(props);
    }
    //shouldComponentUpdate(nextProps, nextState) {
    //    console.log(this.props.btn.url !== this.nextProps.btn.url);
    //    return this.props.btn.url !== this.nextProps.btn.url;
    //}
    render() {
        let liEls = this.props.title.map((item) => {
            return <li key={item.id}><Link to={item.href || null}>{item.content}</Link></li>});
        return (
            <ol className="breadcrumb">
                { liEls }
            </ol>
        );
    }
}