window.eventCallbacks = window.eventCallbacks ? window.eventCallbacks : {};
class event {
    _callbacks: any = window.eventCallbacks;

    on = (eventName, callback, isOnce?) => {
        let events = eventName.split(" ");
        events.forEach((event) => {
            if (!this._callbacks[event]) {
                this._callbacks[event] = [];
            }
            this._callbacks[event].push({ callback: callback, isOnce: isOnce });
        })
    };

    off = (eventName, callback) => {
        let callbacks = this._callbacks[eventName];
        if (!callbacks) {
            return;
        }

        let index = -1;
        for (let i = 0; i < callbacks.length; i++) {
            if (callbacks[i].callback === callback) {
                index = i;
                break;
            }
        }

        if (index < 0) {
            return;
        }

        this._callbacks[eventName].splice(index, 1);
    };

    trigger = (eventName, ...args) => {
        let callbacks = this._callbacks[eventName];
        if (!callbacks || !callbacks.length) {
            return;
        }
        let OnceArray = [];
        for (let i = 0; i < callbacks.length; i++) {
            let eventCallback = callbacks[i];
            if (eventCallback.isOnce)
                OnceArray.push(i);
            callbacks[i].callback(...args);
        }
        for (let OnceArrayIndex in OnceArray) {
            callbacks.splice(OnceArray[OnceArrayIndex], 1);
        }
    };
}

let eventManager = new event();
export default eventManager;

