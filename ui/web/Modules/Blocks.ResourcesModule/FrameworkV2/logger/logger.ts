let log = {};

enum logLevels {
    DEBUG = 1,
    INFO = 2,
    WARN = 3,
    ERROR = 4,
    FATAL = 5
}
class logger {
    defaultLogLevel = logLevels.WARN;

    constructor() {
    }

    log(logObject: any, logLevel: logLevels) {
        if (!window.console || !window.console.log) {
            return;
        }

        if (logLevel != undefined && logLevel < this.defaultLogLevel) {
            return;
        }
        switch (logLevel) {
            case logLevels.ERROR: console.error(logObject); break;
            case logLevels.WARN: console.warn(logObject); break;
            default: console.log(logObject);
        }
    }

    debug(logObject) {
        this.log("DEBUG: ", logLevels.DEBUG);
        this.log(logObject, logLevels.DEBUG);
    };

    info(logObject) {
        this.log("INFO: ", logLevels.INFO);
        this.log(logObject, logLevels.INFO);
    };

    warn(logObject) {
        this.log("WARN: ", logLevels.WARN);
        this.log(logObject, logLevels.WARN);
    };

    error(logObject) {
        this.log("ERROR: ", logLevels.ERROR);
        this.log(logObject, logLevels.ERROR);
    };

    fatal(logObject) {
        this.log("FATAL: ", logLevels.FATAL);
        this.log(logObject, logLevels.FATAL);
    };
}




export default new logger();