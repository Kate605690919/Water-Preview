class Home extends React.Component {
    constructor(props) {
        super(props);
        this.state = { date: '' };
    }
    render() {
        return (
            <div className="homeBody" style={{ width: '100%' }}>
                <div className="commonData">
                    <CardListItem />
                    <CardRank />
                    <HomeMap />
                </div>
                <Footer />
            </div>
        );
    }
}
