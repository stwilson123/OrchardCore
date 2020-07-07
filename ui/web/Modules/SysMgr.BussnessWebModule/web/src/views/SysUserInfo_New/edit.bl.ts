import { Controller, Component, asyncCompatible, Prop, catchWrap } from "interface"
@Component
export default class Edit extends Controller {
    @Prop({ type: Object }) formData;
    constructor() {
        super();
    }
    ruleForm = {
        ID: "",
        UserCode: "",
        CName: "",
        Password: "",
        State: "0",
        Memo: ""
    };
    rules = {
        UserCode: [
            { required: true, message: '账号不能为空', trigger: 'blur' }
        ],
        CName: [
            { required: true, message: '姓名不能为空', trigger: 'blur' }
        ]
    }
    closeDialog() {
        this.exit();
    }
    @asyncCompatible()
    async submitForm() {
        let [resObj, err] = await catchWrap(this.$refs.ruleForm.validate());
        if (resObj === false) {
            return;
        }
        let data = Object.assign({}, this.ruleForm);
        await this.$http({
            method: "post",
            url: "/api/services/SysMgrBussenssModule/SysUserInfo/Update",
            data
        });
        this.exit({ type: "success", message: "修改成功" });
    }
    async viewWillEnter() {
        this.ruleForm = {
            UserCode: this.formData.UserCode,
            CName: this.formData.CName,
            Memo: this.formData.Memo,
            ID: this.formData.ID
        }
    }
}   