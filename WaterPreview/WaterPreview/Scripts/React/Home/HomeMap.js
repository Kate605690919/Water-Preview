class HomeMap extends React.Component {
    constructor(props) {
        super(props);
    }
    componentDidMount() {
        let _this = this;
        // $.get(`/Area/GetMapData`, function (data) {
        //     console.log(data);
        //     let mapEl = document.querySelector("#homeMap");
        //     initMap(mapEl, data);
        // });
        $Fetch.fetch({
            url:'/Area/GetMapData',
            success: (data) => {
                let mapEl = document.querySelector("#homeMap");
                initMap(mapEl, data);
            }
        })
        // $.ajax({
        //     url:'/Area/GetMapData',
        //     methods: 'GET',
        //     xhrFields:{withCredentials:true},
        //     beforeSend: function(request) {
        //         request.setRequestHeader("access_token", 'token');
        //     },
        //     success: (data, status, xhr) => {
        //         console.log(data);
        //         let mapEl = document.querySelector("#homeMap");
        //         initMap(mapEl, data);
        //     }
        // });
    }
    render() {
        return (
            <div className="map">
                <div id="homeMap" style={{ marginBottom: '10px' }}></div>
                <div id="legend">
                    <ul>
                        <li><i></i><span>流量计</span></li>
                        <li><i></i><span>压力计</span></li>
                        <li><i></i><span>水质计</span></li>
                    </ul>
                </div>
            </div>
        );
    }
}
