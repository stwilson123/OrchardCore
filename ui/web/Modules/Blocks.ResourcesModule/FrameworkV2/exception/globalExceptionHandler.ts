import eventManager from "../event/event";
import logger from "../logger/logger"
import vue from "vue"
eventManager.on("vueGlbalError", (err, vm, info) => {

    if (err instanceof Error) {
        vue.prototype.$message.error(err.message);
        logger.error(err)
    }
});