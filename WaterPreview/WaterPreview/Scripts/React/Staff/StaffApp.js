class StaffApp extends React.Component {
    constructor(props) {
        super(props);
    }
    componentDidUpdate() {
        console.log('update');
        setTimeout(() => { document.querySelector('.jstree-clicked').click(); }, 300);
    }
    render() {
        return (
            <div id="App">
                <aside>
                    <JsTree jsTreeInfo={this.props.jsTreeInfo} />
                </aside>
                <article>
                    {this.props.children}
                </article>
            </div>
        );
    }
}
const mapStateToProps = (state, ownProps) => {
    return {
        jsTreeInfo: state.Staff.jsTreeInfo
    };
};
StaffApp = ReactRedux.connect(mapStateToProps)(StaffApp);