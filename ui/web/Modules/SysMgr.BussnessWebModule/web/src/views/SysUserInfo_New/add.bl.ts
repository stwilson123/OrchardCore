import { Controller, Component, asyncCompatible, catchWrap } from "interface"
@Component
export default class Add extends Controller {
    ruleForm = {
        UserCode: "",
        CName: "",
        Password: "",
        State: "0",
        Memo: "",
        ID: ""
    }
    pPattern = /^.*(?=.{6,})(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?=.*[!@#$%^&*? ]).*$/;
    rules = {
        UserCode: [
            { required: true, message: '账号不能为空', trigger: 'blur' }
        ],
        CName: [
            { required: true, message: '姓名不能为空', trigger: 'blur' }
        ],
        Password: [
            { required: true, message: '密码不能为空', trigger: 'blur' }
        ]
    }
    selectData = [{ id: '0', text: '启用' }, { id: '2', text: '停用' }]
    validatorPassword(rule, value, callback) {
        if (!this.pPattern.test(value)) {
            return callback(new Error("你输入的密码规则不正确:密码最少6位，包括至少1个大写字母，1个小写字母，1个数字，1个特殊字符"));
        }
        callback();
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
            url: "/api/services/SysMgrBussenssModule/SysUserInfo/Add",
            data
        });
        this.exit({ type: "success", message: "新增成功" });
    }
}