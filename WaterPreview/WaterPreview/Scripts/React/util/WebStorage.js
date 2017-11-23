class LocalStorage {
    constructor() {
        if (!window.localStorage) {
            alert('您的浏览器不支持localStorage，为了更流畅的体验，请您使用更高版本的浏览器');
            return this.enable = false;
        }
        return this.enable = true;
    }
    setItem(key, value) {
        if (this.enable) {
            localStorage.setItem(key, JSON.stringify(value));
        }
    }
    getItem(key) {
        if (this.enable) {
            return JSON.parse(localStorage.getItem(key));
        }
    }
}
let $localStorage = new LocalStorage;
class SessionStorage {
    constructor() {
        if (!window.sessionStorage) {
            alert('您的浏览器不支持sessionStorage，为了更流畅的体验，请您使用更高版本的浏览器');
            return this.enable = false;
        }
        return this.enable = true;
    }
    setItem(key,value) {
        if (this.enable) {
            sessionStorage.setItem(key, JSON.stringify(value));
        }
    }
    getItem(key) {
        if (this.enable) {
            return JSON.parse(sessionStorage.getItem(key));
        }
    }
}
let $sessionStorage = new SessionStorage;