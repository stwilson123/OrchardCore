import { Controller, Component, Prop, asyncCompatible } from "interface"
@Component
export default class Detail extends Controller {
    @Prop({ type: Object }) paramsData;
    constructor() {
        super();
    }
    formData = {};
    newParameters;
    newOutParameters;
    newExecutionTime;
    cancelClick() {
        this.exit();
    }
    viewWillEnter() {
        this.formData = this.paramsData;
        this.newExecutionTime = blocks.utility.dateConvert.format(
            new Date(this.formData.ExecutionTime),
            "yyyy-MM-dd HH:mm:ss"
        );
        this.newParameters = this.formatJson(JSON.parse(this.formData.Parameters));
        this.newOutParameters = this.formatJson(JSON.parse(this.formData.OutParameters));
    }
    formatJson(json) {
        var rep = "~";
        var jsonStr = JSON.stringify(json, null, rep)
        var str = "";
        for (var i = 0; i < jsonStr.length; i++) {
            var text2 = jsonStr.charAt(i)
            if (i > 1) {
                var text = jsonStr.charAt(i - 1)
                if (rep != text && rep == text2) {
                    str += "<br />"
                }
            }
            str += text2;
        }
        jsonStr = "";
        for (var i = 0; i < str.length; i++) {
            var text = str.charAt(i);
            if (rep == text)
                jsonStr += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
            else {
                jsonStr += text;
            }
            if (i == str.length - 2)
                jsonStr += "<br />"
        }
        return jsonStr;
    }
}