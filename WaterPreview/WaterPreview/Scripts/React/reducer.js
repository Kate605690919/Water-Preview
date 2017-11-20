function reducer(state = [], action) {
    let newState = Object.assign({}, state);
    switch (action.type) {
        //manageReducer
        case EDIT_MANAGE: {
            newState.formInfo.itemInfo = newState.formInfo.itemInfo.map((item) => { item.input.readOnly = null; return item });
            newState.formInfo.itemInfo[0].id = 9;
            newState.formInfo.btnContent = '提交';
            return {
                header: state.header,
                formInfo: Object.assign({}, newState.formInfo),
                jsTreeInfo: state.jsTreeInfo
            };
        }
        case CHANGE_JSTREE: {
            newState.jsTreeInfo.event = action.event;
            return { ...state, jsTreeInfo: Object.assign({}, newState.jsTreeInfo) };
        }

        //case DELETE_STARTED: {
        //    return { ...state, status: Status.LOADING};
        //}
        //case DELETE_SUCCESS: {
        //    return { ...state, status: Status.SUCCESS, Client: Object.assign({}, newState.Client)  };
        //}
        //case DELETE_FALLURE: {
        //    return { ...state, status: Status.FAILURE };
        //}

            //client/getAll
        case CLIENTALL_STARTED: {
            newState.Client.tableInfo.status = Status.LOADING;
            return { ...state, Client: Object.assign({}, newState.Client)};
        }
        case CLIENTALL_SUCCESS: {
            newState.Client.tableInfo.data = action.result;
            newState.Client.tableInfo.status = Status.SUCCESS;
            return { ...state, userUid: action.userUid, userId: action.userId, status: Status.SUCCESS, Client: Object.assign({}, newState.Client) };
        }
        case CLIENTALL_FAILURE: {
            newState.Client.tableInfo.status = Status.FAILURE;
            return { ...state, Client: Object.assign({}, newState.Client) };
        }
        //staff/getAll
        case STAFFALL_STARTED: {
            newState.Staff.tableInfo.status = Status.LOADING;
            return { ...state, Staff: Object.assign({}, newState.Staff) };
        }
        case STAFFALL_SUCCESS: {
            newState.Staff.tableInfo.data = action.result;
            newState.Staff.tableInfo.status = Status.SUCCESS;
            return { ...state, userUid: action.userUid, userId: action.userId, status: Status.SUCCESS, Staff: Object.assign({}, newState.Staff) };
        }
        case STAFFALL_FAILURE: {
            newState.Staff.tableInfo.status = Status.FAILURE;
            return { ...state, Staff: Object.assign({}, newState.Staff) };
        }

        //manageReducer
        case GET_FM_STARTED: {
            newState.FM.status = Status.LOADING;
            return { ...state, FM: Object.assign({}, newState.FM) };
        }
        case GET_FM_SUCCESS: {
            newState.FM.status = Status.SUCCESS;
            newState.FM.tableInfo.data = action.result.data;
            return { ...state, areaUid: action.areaUid, FM: Object.assign({}, newState.FM) };
        }
        case GET_FM_FAILURE: {
            newState.FM.status = Status.FAILURE;
            return { ...state, FM: Object.assign({}, newState.FM) };
        }
        case GET_PM_STARTED: {
            newState.PM.status = Status.LOADING;
            return { ...state, PM: Object.assign({}, newState.PM) };
        }
        case GET_PM_SUCCESS: {
            newState.PM.status = Status.SUCCESS;
            newState.PM.tableInfo.data = action.result.data;
            return { ...state, areaUid: action.areaUid, PM: Object.assign({}, newState.PM) };
        }
        case GET_PM_FAILURE: {
            newState.PM.status = Status.FAILURE;
            return { ...state, PM: Object.assign({}, newState.PM) };
        }
        case GET_QM_STARTED: {
            newState.QM.status = Status.LOADING;
            return { ...state, QM: Object.assign({}, newState.QM) };
        }
        case GET_QM_SUCCESS: {
            newState.QM.status = Status.SUCCESS;
            newState.QM.tableInfo.data = action.result.data;
            return { ...state, areaUid: action.areaUid, QM: Object.assign({}, newState.QM) };
        }
        case GET_QM_FAILURE: {
            newState.QM.status = Status.FAILURE;
            return { ...state, QM: Object.assign({}, newState.QM) };
        }
        case CHANGE_DEVICE: {
            let newState = Object.assign({}, state);
            newState.device = action.device;
            return { ...state, device: newState.device };
        }


        default: {
            return state;
        }
    }
}