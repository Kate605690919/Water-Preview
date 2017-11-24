class Fetch {
    /**
     * 
     * @param {string} method 默认'GET'
     * @param {string} data 序列化过的对象
     * @param {string} url 纯url，不带参数
     * @param {object} storage {type: 'session'||'local', name: '...'}
     * @param {function} callBackSuccess 成功回调
     * @param {function} callBackError 失败回调
     */
    fetch({ method = 'GET', data, url, storage, callBackSuccess, callBackError }) {
        debugger;
        let headers = null;
        if (method === 'GET') {
            url = `${url}?${data}`;
            data = null;
        } else if (method === 'POST') {
            headers = { 'Content-Type': 'application/x-www-form-urlencoded', };
        }
        if (storage) {
            if (storage.type === 'session') {
                let data = $sessionStorage.getItem(storage.name);
                if (!data) {
                    fetch(url, { method: method, headers: headers, body: data }).then((response) => {
                        if (response.status !== 200) {
                            throw new Error('Fail to get response with status ' + response.status);
                        }
                        response.json().then((res) => {
                            $sessionStorage.setItem(storage.name, res)
                            if (callBackSuccess) callBackSuccess(res);
                        }).catch((error) => {
                            if (callBackError) callBackError(error);
                        });
                    }).catch((error) => {
                        if (callBackError) callBackError(error);
                    })
                } else {
                    return true;
                }
            }
        } else {
            fetch(url, { method: method, headers: headers, body: data }).then((response) => {
                if (response.status !== 200) {
                    throw new Error('Fail to get response with status ' + response.status);
                }
                response.json().then((res) => {
                    if (callBackSuccess) callBackSuccess(res);
                }).catch((error) => {
                    if (callBackError) callBackError(error);
                });
            }).catch((error) => {
                if (callBackError) callBackError(error);
            })
        }
    }
}
let $Fetch = new Fetch();