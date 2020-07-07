import { Controller, Component, asyncCompatible, catchWrap } from "interface"
@Component
export default class Add extends Controller {
    ruleForm = {
        Name: "",
        Remark: ""
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
            url: "/api/services/SysMgrBussenssModule/SysRoleInfo/Add",
            data
        });
        this.exit({ type: "success", message: "新增成功" });
    }
    closeDialog() {
        this.exit();
    }
}