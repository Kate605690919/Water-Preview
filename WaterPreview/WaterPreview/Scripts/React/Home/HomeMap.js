class HomeMap extends React.Component {
    constructor(props) {
        super(props);
    }
    componentDidMount() {
        let _this = this;
        $.get(`/Area/GetMapData`, function (data) {
            console.log(data);
            let mapEl = document.querySelector("#homeMap");
            initMap(mapEl, data);
        });
    }
    render() {
        return (
            <div id="homeMap" style={{ marginBottom: '10px' }}></div>
        );
    }
}
