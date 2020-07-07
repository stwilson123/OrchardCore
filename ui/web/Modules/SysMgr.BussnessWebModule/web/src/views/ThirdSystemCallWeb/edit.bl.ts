import { Controller, Component, Prop, catchWrap, asyncCompatible } from "interface"
import blocks from "blocks"
@Component
export default class edit extends Controller {
    @Prop({ type: Object }) formData;
    constructor() {
        super();
    }
    NumDisable = false;
    ruleForm = {
        registerTime: "", ID: '', SystemID: '', SystemNo: '', SystemName: '', aaaaaa: Object, FunctionName: '', ParameterIn: '', ProcessTimeBegin: '', ProcessTimeEnd: '', ProcessResult: '', RequestTimes: '', ResponseValue: '', ExceptionMsg: ''
    };
    tmpParam;
    async viewWillEnter() {
        this.ruleForm.ProcessResult = this.formData.ProcessResult;
        switch (this.ruleForm.ProcessResult) {

            case "1":
                this.NumDisable = true;
                break;
            case "2":
                this.NumDisable = true;
                break;
            default:
                this.NumDisable = false;
                break;
        }
        this.ruleForm.ID = this.formData.ID;
        await this.getDataByID();
    }
    async getDataByID() {
        let data = Object.assign({}, this.ruleForm);
        let result = await this.$http({
            method: "post",
            url: '/api/services/SysMgrBussenssModule/ThirdSystemCall/GetOneById',
            data
        });
        if (result.data.code != "200") {
            this.$message.error(result.data.msg);
            return;
        }
        this.ruleForm.ID = result.data.content.ID;
        this.ruleForm.SystemID = result.data.content.SystemID;
        this.ruleForm.SystemNo = result.data.content.SystemNo;
        this.ruleForm.SystemName = result.data.content.SystemName;
        this.ruleForm.FunctionName = result.data.content.FunctionName;
        this.tmpParam = JSON.stringify(JSON.parse(result.data.content.ParameterIn), undefined, 4);
        if (this.tmpParam == "null") {
            this.ruleForm.ParameterIn = '';
        }
        else {
            this.ruleForm.ParameterIn = this.tmpParam;
        }
        this.ruleForm.ProcessTimeBegin = result.data.content.ProcessTimeBegin;
        this.ruleForm.ProcessTimeEnd = result.data.content.ProcessTimeEnd;
        this.ruleForm.RequestTimes = result.data.content.RequestTimes;
    }

    @asyncCompatible()
    async submitForm() {

        let [resObj, err] = await catchWrap(this.$refs.ruleForm.validate());
        if (resObj === false) {
            return;
        }
        if (this.ruleForm.ParameterIn != '') {
            var tmpParameterIn;
            try {
                tmpParameterIn = JSON.parse(this.ruleForm.ParameterIn);
            }
            catch{
                this.$message.error("传入参数不是正确的Json格式!");
                return;

            }
            this.ruleForm.ParameterIn = JSON.stringify(tmpParameterIn);
        }
        let data = Object.assign({}, this.ruleForm);
        let res = await this.$http({
            method: "post",
            url: "/api/services/SysMgrBussenssModule/ThirdSystemCall/Update",
            data: data
        });

        if (res.data.code != "200") {

            this.$message.error(res.data.msg);
            return;
        }
        this.exit({ type: "success", message: this.$l('succeed') });
    }

    closeDialog() {
        this.exit();
    }
}