class JsTree extends React.Component {
    constructor(props) {
        super(props);
        this.onClick = this.onClick.bind(this);
        this.onManageClick = this.onManageClick.bind(this);
        this.onStaffClick = this.onStaffClick.bind(this);
    }

    onClick(e) {
        const uid = e.target.getAttribute('data-uid');
        const device = this.props.device;
        if (uid.length > 10) {
            device.forEach((item) => {
                this.props[`onGet${item}`](uid);
            });
        }
    }
    onManageClick(e) {
        const uid = e.target.getAttribute('data-uid');
        if (uid.length > 10) {
            this.props.onManageClick(uid);
        }
    }
    onStaffClick(e) {
        const uid = e.target.getAttribute('data-uid');
        if (uid.length > 10) {
            this.props.onStaffClick(uid);
        }
    }
    componentWillUpdate() {
        let { url, elId, event } = this.props.jsTreeInfo;
        jstreeRender(url, elId, event);
        //有些页面需要改变节点change事件，需要在更新的时候重新渲染一遍
    }
    componentDidMount() {
        let { url, elId, event } = this.props.jsTreeInfo;
        jstreeRender(url, elId, event);
        //比如详情页刷新时需要重新渲染一遍树
    }

    render() {
        return (
            <div className="jsTree" style={{ position: 'fixed' }}>
                <div className="wrapper wrapper-content animated fadeInRight affix">
                    <div id="jstreeArea"></div>
                </div>
                <a id="areaUid" onClick={this.onClick} ></a>
                <a id="manageAreaUid" onClick={this.onManageClick} ></a>
                <a id="staffAreaUid" onClick={this.onStaffClick} ></a>
            </div>
        );
    }
}
const mapStateToProps = (state, ownProps) => {
    return {
        jsTreeInfo: state.jsTreeInfo,
        device: state.device
    };
};

const mapDispatchToProps = (dispatch) => {
    return {
        onGetFM: (uid) => {
            dispatch(getFM(uid));
        },
        onGetPM: (uid) => {
            dispatch(getPM(uid));
        },
        onGetQM: (uid) => {
            dispatch(getQM(uid));
        },
        onManageClick: (areaUid) => {
            dispatch(getClientAll(areaUid))
        },
        onStaffClick: (areaUid) => {
            dispatch(getStaffAll(areaUid))
        }
    }
};

JsTree = ReactRedux.connect(mapStateToProps, mapDispatchToProps)(JsTree);

function jstreeRender(url, elId, event) {
    event = event || { name: 'loaded', function: () => { console.log('jsTree has loaded.') } }
    $.get(url, {}, function (data) {
        $("#" + elId).on(event.name + '.jstree', event.function).jstree({
            "core": {                //所有的默认设置
                "animation": 0,          //打开或关闭动画持续的时间，默认为200
                "check_callback": false,  //允许所有的都交互操作（creat,rename,delete,move,copy）
                "themes": { "stripes": false },  //stripes为一个布尔值
                'data': data
            },
            "types": {  //存储所有类型为键值对（key value paris）的对象
                "#": {
                    "max_children": 4, //可以拥有的最大的直接子节点数，-1表示无限制（最好不要设为-1）
                    "max_depth": 5,    //可以拥有的最大嵌套数 ，1表示只有children而不能有grandchildren
                    "valid_children": ["root"],  //可以作为子节点的字符串数组
                },
                "root": {
                    "icon": "glyphicon glyphicon-paperclip",
                    "valid_children": ["default"]
                },
                "default": {
                    "icon": false
                }
            },
            "plugins": [
                "types", "state"    //wholerow扩大点击范围， type用于定义嵌套关系和图标
            ]
        });
    });
}