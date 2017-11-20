class Card extends React.Component {
    constructor(props) {
        super(props);
    }
    render() {
        const { status, warn, header } = this.props;
        if (!warn) { return null; }
        let content = null;
        switch (status) {
            case Status.LOADING: {
                content = <h3>信息加载中...</h3>;
                break;
            }
            case Status.SUCCESS: {
                content = this.props.children;
                break;
            }
            case Status.FAILURE: {
                content = <h3>信息加载失败，请重试</h3>;
                break;
            }
            case 'init': {
                content = <h3>{this.props.init || '信息加载中...' }</h3>;
            }
            default: {
                content = <h3>{this.props.init || '信息加载中...'}</h3>;
                //throw new Error('unexpected status ' + status);
            }
        }
        return (
            <div className="devices">
                <article className="ibox">
                    <header className="ibox-title">
                        <Header header={header} />
                    </header>
                    <article className='ibox-content'>
                        {content}
                    </article>
                </article>
            </div >
        );
    }
}
