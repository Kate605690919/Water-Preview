const EDIT_MANAGE = 'MANAGE/EDIT';
const CHANGE_JSTREE = 'JATREE/CHANGE';

const DELETE_STARTED = 'CLIENT/DELETE_STAETED';
const DELETE_SUCCESS = 'CLIENT/DELETE_SUCCESS';
const DELETE_FALLURE = 'CLIENT/DELETE_FALLURE';
const STAFFDELETE_STARTED = 'STAFF/DELETE_STAETED';
const STAFFDELETE_SUCCESS = 'STAFF/DELETE_SUCCESS';
const STAFFDELETE_FALLURE = 'STAFF/DELETE_FALLURE';

const CLIENTALL_STARTED = 'GET/CLIENTALL_STAETED';
const CLIENTALL_SUCCESS = 'GET/CLIENTALL_SUCCESS';
const CLIENTALL_FAILURE = 'GET/CLIENTALL_FAILURE';
const STAFFALL_STARTED = 'GET/STAFFALL_STAETED';
const STAFFALL_SUCCESS = 'GET/STAFFALL_SUCCESS';
const STAFFALL_FAILURE = 'GET/STAFFALL_FAILURE';

const editManage = () => ({
    type: EDIT_MANAGE
});
const changeJstree = (event) => ({
    type: CHANGE_JSTREE,
    event
});

const getClientAllStarted = () => ({
    type: CLIENTALL_STARTED
});

const getClientAllSuccess = (result) => ({
    type: CLIENTALL_SUCCESS,
    result
});

const getClientAllFailure = (error) => ({
    type: CLIENTALL_FAILURE,
    error
});
//getStaffAll
const getStaffAllStarted = () => ({
    type: STAFFALL_STARTED
});

const getStaffAllSuccess = (result) => ({
    type: STAFFALL_SUCCESS,
    result
});

const getStaffAllFailure = (error) => ({
    type: STAFFALL_FAILURE,
    error
});

const deleteClientStarted = () => ({
    type: DELETE_STARTED
});

const deleteClientSuccess = (result) => ({
        type: DELETE_SUCCESS
});

const deleteClientFailure = (error) => ({
    type: DELETE_FALLURE,
    error
});

const deleteStaffStarted = () => ({
    type: STAFFDELETE_STARTED
});

const deleteStaffSuccess = (result) => ({
    type: STAFFDELETE_SUCCESS
});

const deleteStaffFailure = (error) => ({
    type: STAFFDELETE_FALLURE,
    error
});
let nextClientSeqId = 0;
const getClientAll = (areaUid) => {
    return (dispatch) => {
        const url = `/Client/getAll?areaUid=${areaUid}`;
        const seqId = ++nextClientSeqId;
        const dispatchIfVaild = (action) => {
            if (seqId === nextClientSeqId) {
                return dispatch(action);
            }
        }
        dispatchIfVaild(getClientAllStarted());
        fetch(url).then((response) => {
            if (response.status !== 200) {
                throw new Error('Fail to get response with status ' + response.status);
            }
            response.json().then((res) => {
                    dispatchIfVaild(getClientAllSuccess(res));
            }).catch((error) => {
                console.error(error);
                dispatchIfVaild(getClientAllFailure(error));
            });
        }).catch((error) => {
            dispatchIfVaild(getClientAllFailure(error));
        })
    };
}
///Staff/getAll
let nextStaffSeqId = 0;
const getStaffAll = (areaUid) => {
    return (dispatch) => {
        const url = `/Staff/getAll?areaUid=${areaUid}`;
        console.log(url)
        const seqId = ++nextClientSeqId;
        const dispatchIfVaild = (action) => {
            if (seqId === nextClientSeqId) {
                return dispatch(action);
            }
        }
        dispatchIfVaild(getStaffAllStarted());
        fetch(url).then((response) => {
            if (response.status !== 200) {
                throw new Error('Fail to get response with status ' + response.status);
            }
            response.json().then((res) => {
                dispatchIfVaild(getStaffAllSuccess(res));
            }).catch((error) => {
                console.error(error);
                dispatchIfVaild(getStaffAllFailure(error));
            });
        }).catch((error) => {
            dispatchIfVaild(getStaffAllFailure(error));
        })
    };
}
//client/delete
let nextDeleteSeqId = 0;
const clientDelete = (id) => {
    return (dispatch) => {
        const url = `/Client/DeleteClient?id=${id}`;
        const seqId = ++nextDeleteSeqId;
        const dispatchIfVaild = (action) => {
            if (seqId === nextDeleteSeqId) {
                return dispatch(action);
            }
        }
        dispatchIfVaild(deleteClientStarted());
        fetch(url).then((response) => {
            if (response.status !== 200) {
                throw new Error('Fail to get response with status ' + response.status);
            }
            response.json().then((res) => {
                if (res) {
                    alert('删除成功！');
                    manageAreaUid.click()
                    dispatchIfVaild(deleteClientSuccess());
                } else {
                    alert('删除失败！');
                }
            }).catch((error) => {
                console.error(error);
                dispatchIfVaild(deleteClientFailure(error));
            });
        }).catch((error) => {
            console.error(error);
            dispatchIfVaild(deleteClientFailure(error));
        })
    };
}
//staff/delete
const staffDelete = (id) => {
    return (dispatch) => {
        const url = `/Staff/DeleteClient?id=${id}`;
        const seqId = ++nextDeleteSeqId;
        const dispatchIfVaild = (action) => {
            if (seqId === nextDeleteSeqId) {
                return dispatch(action);
            }
        }
        dispatchIfVaild(deleteStaffStarted());
        fetch(url).then((response) => {
            if (response.status !== 200) {
                throw new Error('Fail to get response with status ' + response.status);
            }
            response.json().then((res) => {
                if (res) {
                    alert('删除成功！');
                    staffAreaUid.click()
                    dispatchIfVaild(deleteStaffSuccess());
                } else {
                    alert('删除失败！');
                }
            }).catch((error) => {
                console.error(error);
                dispatchIfVaild(deleteStaffFailure(error));
            });
        }).catch((error) => {
            console.error(error);
            dispatchIfVaild(deleteStaffFailure(error));
        })
    };
}