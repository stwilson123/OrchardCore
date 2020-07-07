import { Controller, Component, asyncCompatible, Prop } from "interface"

@Component
export default class password extends Controller {
    @Prop({ type: Object }) formData;
    ruleForm = {
        ID: "",
        OldPassword: "",
        NewPassword: "",
        ConfirmPassword: ""
    };
    rules = {
        OldPassword: [
            { required: true, message: '原始密码不能为空', trigger: 'blur' },
            { validator: this.validatorOldPassword, trigger: 'blur' }
        ],
        NewPassword: [
            { required: true, message: '新密码不能为空', trigger: 'blur' }
        ],
        ConfirmPassword: [
            { required: true, message: '确认密码不能为空', trigger: 'blur' }
        ]
    }
    pPattern = /^.*(?=.{6,})(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?=.*[!@#$%^&*? ]).*$/;
    async  validatorOldPassword(rule, value, callback) {
        let data = Object.assign({}, this.ruleForm);
        let res = await this.$http({
            method: "post",
            url: "/api/services/SysMgrBussenssModule/SysUserInfo/validatorPassword",
            data: data
        });
        if (res.data.code === "101") {
            return callback(new Error("原始密码不正确！"));
        }
        callback();
    }

    validatorNewPassword(rule, value, callback) {
        if (!this.pPattern.test(value)) {
            return callback(new Error("你输入的密码规则不正确:密码最少6位，包括至少1个大写字母，1个小写字母，1个数字，1个特殊字符"));
        }

        callback();
    }
    validatorConfirmPassword(rule, value, callback) {
        if (!this.pPattern.test(value)) {
            return callback(new Error("你输入的密码规则不正确:密码最少6位，包括至少1个大写字母，1个小写字母，1个数字，1个特殊字符"));
        }
        if (this.ruleForm.NewPassword !== this.ruleForm.ConfirmPassword) {

            return callback(new Error("两次密码不一致"));
        }
        callback();
    }
    constructor() {
        super();
    }
    closeDialog() {
        this.exit({ message: "取消" });
    }

    @asyncCompatible()
    async submitForm() {
        this.$refs.ruleForm.validate(async (res, obj) => {
            if (res) {
                let data = Object.assign({}, this.ruleForm);
                let res = await this.$http({
                    method: "post",
                    url: "/api/services/SysMgrBussenssModule/SysUserInfo/PasswordModification",
                    data: data
                });
                if (res.data.code == "200") {
                    this.exit({
                        type: "success",
                        message: "修改密码成功"
                    });
                } else {
                    this.$message({
                        type: "error",
                        message: res.data.msg,
                        duration: 1000
                    });
                }
            }
        })
    }
    async viewWillEnter() {
        this.ruleForm.ID = this.formData.ID;
    }
}