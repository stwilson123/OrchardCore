import { Controller, Component, Prop, asyncCompatible } from "interface"
@Component
export default class Add extends Controller {
    @Prop({ type: Object }) formData;
    constructor() {
        super();
    }
    ruleForm = {
        Code: '', Icon: '', IndexIcon: '', Desc: '', Sort: 0, PId: '', Platform: ''
    };
    rules = {
        Code: [
            { required: true, message: '编码不能为空', trigger: 'blur' }
        ]
    }
    @asyncCompatible()
    async saveClick() {
        this.$refs.ruleForm.validate(async (res, obj) => {
            if (res) {
                let res = await this.$http({
                    method: "post",
                    url: "/api/services/SysMgrBussenssModule/Menu/Add",
                    data: this.ruleForm
                });
                this.exit({ success: true, data: { type: "success", message: "新增成功" } });
            }
        })
    }
    cancelClick() {
        this.exit({ success: false });
    }
    async viewWillEnter() {
        this.ruleForm.PId = this.formData.ID;
        this.ruleForm.Platform = this.formData.platForm;
    }
}