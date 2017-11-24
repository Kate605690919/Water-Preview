class ManageApp extends React.Component {
    constructor(props) {
        super(props);
    }
    componentDidUpdate() {
        console.log('update');
        document.querySelector('.jstree-clicked').click();
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
        jsTreeInfo: state.Client.jsTreeInfo
    };
};
ManageApp = ReactRedux.connect(mapStateToProps)(ManageApp);