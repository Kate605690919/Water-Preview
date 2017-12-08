const CHANGE_DEVICE_COUNT = 'DEVICE_COUNT/CHANGE';
const INCREASE_FC = 'FC/INCREASE';
const INCREASE_PC = 'PC/INCREASE';
const RENDER_RANK = 'RANK/RENDER';
//action.js

const changeDeviceCount = (FC, PC) => {
    return {
        type: CHANGE_DEVICE_COUNT,
        FC: FC,
        PC: PC
    }
};
const increaseFC = () => {
    return { type: INCREASE_FC }
}
const increasePC = () => {
    return { type: INCREASE_PC }
}
const onRenderRank = () => {
    return { type: RENDER_RANK }
}