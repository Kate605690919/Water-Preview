/**
 * device
 */

//actionType.js
const GET_FM_STARTED = 'FM/GET_STAETED';
const GET_FM_SUCCESS = 'FM/GET_SUCCESS';
const GET_FM_FAILURE = 'FM/GET_FAILURE';

const GET_PM_STARTED = 'PM/GET_STAETED';
const GET_PM_SUCCESS = 'PM/GET_SUCCESS';
const GET_PM_FAILURE = 'PM/GET_FAILURE';

const GET_QM_STARTED = 'QM/GET_STAETED';
const GET_QM_SUCCESS = 'QM/GET_SUCCESS';
const GET_QM_FAILURE = 'QM/GET_FAILURE';

const CHANGE_DEVICE = 'DEVICE/CHANGE';
//status.js
const Status = {
    LOADING: 'loading',
    SUCCESS: 'success',
    FAILURE: 'failure'
}
//action.js

const getFMStarted = () => ({
    type: GET_FM_STARTED
});

const getFMSuccess = (result, areaUid) => {
    result = JSON.parse(result);
    result.data = eval(result.data);
    return {
        type: GET_FM_SUCCESS,
        result: result,
        areaUid: areaUid
    }
};

const getFMFailure = (error) => ({
    type: GET_FM_FAILURE,
    error
});

const getPMStarted = () => ({
    type: GET_PM_STARTED
});

const getPMSuccess = (result, areaUid) => {
    result = JSON.parse(result);
    result.data = eval(result.data);
    return {
        type: GET_PM_SUCCESS,
        result: result,
        areaUid: areaUid
    }
};

const getPMFailure = (error) => ({
    type: GET_PM_FAILURE,
    error
});

const getQMStarted = () => ({
    type: GET_QM_STARTED
});

const getQMSuccess = (result, areaUid) => {
    result = JSON.parse(result);
    result.data = eval(result.data);
    return {
        type: GET_QM_SUCCESS,
        result: result,
        areaUid: areaUid
    }
};

const getQMFailure = (error) => ({
    type: GET_QM_FAILURE,
    error
});

const changeDevice = (device) => ({
    type: CHANGE_DEVICE,
    device
});

let FMNextId = 0;
const getFM = (areaUid) => {
    return (dispatch) => {
        const url = `/Area/GetFlowMeterByAreaUid?areaUid=${areaUid}`;    //(Guid areaUid)`
        const seqId = ++FMNextId;
        const dispatchIfVaild = (action) => {
            if (seqId === FMNextId) {
                return dispatch(action);
            }
        }
        dispatchIfVaild(getFMStarted());
        fetch(url).then((response) => {
            if (response.status !== 200) {
                throw new Error('Fail to get response with status ' + response.status);
            }
            response.json().then((responseJson) => {
                dispatchIfVaild(getFMSuccess(responseJson, areaUid));
            }).catch((error) => {
                console.error(error);
                dispatchIfVaild(getFMFailure(error));
            });
        }).catch((error) => {
            console.log(error);
            dispatchIfVaild(getFMFailure(error));
        })
    };
}
let PMNextId = 0;
const getPM = (areaUid) => {
    return (dispatch) => {
        const url = `/Area/GetPressureMeterByAreaUid?areaUid=${areaUid}`;    //(Guid areaUid)`
        const seqId = ++PMNextId;
        const dispatchIfVaild = (action) => {
            if (seqId === PMNextId) {
                return dispatch(action);
            }
        }
        dispatchIfVaild(getPMStarted());
        fetch(url).then((response) => {
            if (response.status !== 200) {
                throw new Error('Fail to get response with status ' + response.status);
            }
            response.json().then((responseJson) => {
                dispatchIfVaild(getPMSuccess(responseJson, areaUid));
            }).catch((error) => {
                console.log(error);
                dispatchIfVaild(getPMFailure(error));
            });
        }).catch((error) => {
            console.error(error);
            dispatchIfVaild(getPMFailure(error));
        })
    };
}
let QMNextId = 0;
const getQM = (areaUid) => {
    return (dispatch) => {
        const url = `/Area/GetQualityMeterByAreaUid?areaUid=${areaUid}`;    //(Guid areaUid)`
        const seqId = ++QMNextId;
        const dispatchIfVaild = (action) => {
            if (seqId === QMNextId) {
                return dispatch(action);
            }
        }
        dispatchIfVaild(getQMStarted());
        fetch(url).then((response) => {
            if (response.status !== 200) {
                throw new Error('Fail to get response with status ' + response.status);
            }
            response.json().then((responseJson) => {
                dispatchIfVaild(getQMSuccess(responseJson, areaUid));
            }).catch((error) => {
                console.error(error);
                dispatchIfVaild(getQMFailure(error));
            });
        }).catch((error) => {
            console.log(error);
            dispatchIfVaild(getQMFailure(error));
        })
    };
}
//reducer.js
//const getDeviceStarted = () => ({
//    type: GET_STARTED
//});

//const getDeviceSuccess = (result, areaUid) => {
//    result = JSON.parse(result);
//    result.data = eval(result.data);
//    return {
//        type: GET_SUCCESS,
//        result: result,
//        areaUid: areaUid
//    }
//};

//const getDeviceFailure = (error) => ({
//    type: GET_FALLURE,
//    error
//});
//const GET_STARTED = 'DEVICE/GET_STAETED';
//const GET_SUCCESS = 'DEVICE/GET_SUCCESS';
//const GET_FALLURE = 'DEVICE/GET_FALLURE';
//let GetFlowMeterByAreaUid = 0, GetPressureMeterByAreaUid = 0, GetQualityMeterByAreaUid = 0;
//const getDevice = (areaUid, device = 'GetFlowMeterByAreaUid') => {
//    return (dispatch) => {
//        const url = `/Area/${device}?areaUid=${areaUid}`;    //(Guid areaUid)`
//        const seqId = eval(`++${device}`);
//        const dispatchIfVaild = (action) => {
//            if (seqId === eval(device)) {
//                return dispatch(action);
//            }
//        }
//        dispatchIfVaild(getDeviceStarted());
//        fetch(url).then((response) => {
//            if (response.status !== 200) {
//                throw new Error('Fail to get response with status ' + response.status);
//            }
//            response.json().then((responseJson) => {
//                dispatchIfVaild(getDeviceSuccess(responseJson, areaUid));
//            }).catch((error) => {
//                console.log(error);
//                dispatchIfVaild(getDeviceFailure(error));
//            });
//        }).catch((error) => {
//            console.log(error);
//            dispatchIfVaild(getDeviceFailure(error));
//        })
//    };
//}