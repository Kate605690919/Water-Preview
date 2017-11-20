class DeviceCard extends React.Component {
    constructor(props) {
        super(props);
    }
    render() {
        const { status, tableInfo, warn, header } = this.props;
        if (!warn) {
            return null;
        }
        let tables = null;
        switch (status) {
            case Status.LOADING: {
                tables = <div className="col-xs-9">信息加载中...</div>;
                break;
            }
            case Status.SUCCESS: {
                return (
                    <article className="ibox">
                        <header className="ibox-title">
                            <Header header={header} />
                        </header>
                        <article className='ibox-content'>
                            <div className="tables">
                                {tableInfo.data ? <Table tableInfo={tableInfo} /> : (<p><h3 style={{ textAlign: 'center' }}>暂无数据</h3></p>)}
                                <Table tableInfo={tableInfo} />
                            </div>
                        </article>
                    </article>
                );
                break;
            }
            case Status.FAILURE: {
                tables = <div className="col-xs-9">信息加载失败，请重试</div>;
                break;
            }
            default: {
                tables = <div className="col-xs-9">信息加载失败，请重试</div>;
                //throw new Error('unexpected status ' + status);
            }
        }
        return (
            <div className="devices">
                <section>
                    { tables }
                </section>
            </div >
        );
    }
}
