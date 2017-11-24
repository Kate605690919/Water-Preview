class Battery extends React.Component {
    constructor(props) {
        super(props);
    }
    render() {
        return (
            <div className="battery" style={{marginRight:'10px', cursor: 'pointer'}}>
                <div style={{ position: 'relative',width:'30px',border: '2px solid #ada9a9',borderRadius: '4px',padding: '1px'}}>
                    <span className="bbody" style={{ display: 'Block', position: 'relative', height: '10px', borderLeft: `${this.props.electricity*24}px solid rgb(1, 198, 224)`,borderRadius:'2px'}}>
                        <span className="btop" style={{
                            position: 'relative', height: '8px', width: '4px', background: '#333', display: 'block', position: 'absolute', background: '#ada9a9',top: '50%',left: '100%',transform: 'translate(50%,-50%)' }}>
                        </span>
                    </span>
                </div>
                <div className="popove-bottom" style={{ position: 'absolute', display: 'none', top: '100%'}}>
                    <div className="arrow" style={{ border: '8px solid transparent', borderBottomColor: 'rgb(31, 31, 31)' }}>                    </div>
                    <div className="popover-content" style={{ position: 'absolute', backgroundColor: '#111', color: '#fff', width: '100px', top: '8px', right: '100%', transform: 'translate(20%,0%)', padding: '10px 10px 0', borderRadius: '3px' }}>
                        <p>{`${this.props.content}: ${this.props.electricity * 100}%`}</p>
                    </div>
                </div>
            </div>
        );
    }
}