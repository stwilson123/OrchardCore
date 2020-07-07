import { Controller, Component, asyncCompatible, Prop, catchWrap } from "interface"
@Component
export default class Edit extends Controller {
    @Prop({ type: Object }) formData;
    ruleForm = {
        Name: "",
        Remark: "",
        ID: ""
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
            url: "/api/services/SysMgrBussenssModule/SysRoleInfo/Edit",
            data
        });
        this.exit({ type: "success", message: "修改成功" });
    }
    closeDialog() {
        this.exit();
    }
    async viewWillEnter() {
        this.ruleForm = {
            Name: this.formData.Name,
            Remark: this.formData.Remark,
            ID: this.formData.ID
        }
    }
}