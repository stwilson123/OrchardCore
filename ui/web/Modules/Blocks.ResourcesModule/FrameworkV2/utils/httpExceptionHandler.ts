import eventManager from "../event/event";
import httpException from "./httpException";
import vue from "vue"

eventManager.on("vueGlbalError", (err, vm, info) => {
    if (!(err instanceof httpException))
        return;
    let exception: httpException = err;
    let errorMessage = "";
    if (!exception.Exception) {
        vue.prototype.$message.error("undefined exception.");
        return;
    }
    if (exception.Exception.request.status !== 200) {
        vue.prototype.$message.error(exception.Exception.request.statusText);
        return;
    }

    if (exception.Exception.data.code !== "200") {
        vue.prototype.$message.error(exception.Exception.data.msg);
        return;
    }
});