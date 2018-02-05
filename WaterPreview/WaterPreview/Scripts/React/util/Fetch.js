class Fetch {
    setSessionStorate(key, data) {
        data = JSON.stringify(data);
        sessionStorage.setItem(key, data);
    }
    getSessionStorate(key) {
        let res = sessionStorage.getItem(key);
        return JSON.parse(res);
    }
    setLocalStorate(key, data) {
        data = JSON.stringify(data);
        localStorage.setItem(key, data);
    }
    getLocalStorate(key) {
        let res = localStorage.getItem(key);
        if (res) return JSON.parse(res);
        else return undefined;
    }
    /**
     * 同步fetch POST
     * @param {string} data 序列化过的对象
     * @param {string} url 纯url，不带参数
     */
    fetchSync_Post({ url, data }) {
        let token = this.getSessionStorate('token');
        let headers = new Headers();
        headers.append("Content-Type", "application/x-www-form-urlencoded");
        headers.append('access_token', token);
        let request = new Request(url, {
            headers: headers,
            method: "POST",
            body: data,
        });
        return new Promise((resolve, reject) => {
            fetch(request).then((response) => {
                if (response.status !== 200) {
                    throw new Error('Fail to get response with status ' + response.status);
                }
                response.json().then((res) => {
                    res = JSON.parse(res);
                    resolve(res);
                }).catch((error) => {
                    reject(error);
                });
            }).catch((error) => {
                reject(error);
            });
        });
    }
    /**
     * 同步fetch GET
     * @param {string} url url，带参数
     */
    fetchSync_Get(url) {
        let token = this.getSessionStorate('token');
        let headers = new Headers();
        headers.append('access_token', token);
        let request = new Request(url, {
            headers: headers,
            method: "GET"
        });
        return new Promise((resolve, reject) => {
            fetch(request).then((response) => {
                if (response.status !== 200) {
                    throw new Error('Fail to get response with status ' + response.status);
                }
                response.json().then((res) => {
                    res = JSON.parse(res);
                    resolve(res);
                }).catch((error) => {
                    reject(error);
                });
            }).catch((error) => {
                reject(error);
            });
        });
    }
    // get方法封装
    fetch({ url, success }) {
        let token = this.getSessionStorate('token');
        let headers = new Headers();
        headers.append('access_token', token);
        let request = new Request(url, {
            headers: headers,
            method: "GET"
        });
        fetch(request).then((response) => {
            if (response.status !== 200) {
                throw new Error('Fail to get response with status ' + response.status);
            }
            response.json().then((res) => {
                success(res);
            }).catch((error) => {
                console.error(error);
            });
        }).catch((error) => {
            console.error(error);
        });
    }
    // post方法封装
    fetch_Post({ url, data, success }) {
        let token = this.getSessionStorate('token');
        let headers = new Headers();
        headers.append("Content-Type", "application/x-www-form-urlencoded");
        headers.append('access_token', token);
        let request = new Request(url, {
            headers: headers,
            method: "POST",
            body: data,
        });
        fetch(request).then((response) => {
            if (response.status !== 200) {
                throw new Error('Fail to get response with status ' + response.status);
            }
            let headers = response.headers;
            response.json().then((res) => {
                success(res, headers);
            }).catch((error) => {
                console.error(error);
            });
        }).catch((error) => {
            console.error(error);
        });
    }
}
let $Fetch = new Fetch();