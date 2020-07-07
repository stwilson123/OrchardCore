import { Controller, Component, Prop, asyncCompatible } from "interface"
@Component
export default class Edit extends Controller {
    @Prop({ type: Object }) formData;
    constructor() {
        super();
    }
    ruleForm = {
        ID: '', Code: '', Icon: '', IndexIcon: '', Desc: '', Sort: 0
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
                    url: '/api/services/SysMgrBussenssModule/Menu/Edit',
                    data: this.ruleForm
                });
                this.exit({ success: true, data: { type: "success", message: "编辑成功" } });
            }
        })
    }
    cancelClick() {
        this.exit({ success: false });
    }
    async viewWillEnter() {
        this.ruleForm.ID = this.formData.ID;
        let res = await this.$http({
            method: "post",
            url: "/api/services/SysMgrBussenssModule/Menu/GetOneById",
            data: this.ruleForm
        });
        this.ruleForm.Code = res.data.content.Code;
        this.ruleForm.Desc = res.data.content.Desc;
        this.ruleForm.Icon = res.data.content.Icon;
        this.ruleForm.IndexIcon = res.data.content.IndexIcon;
        this.ruleForm.Sort = res.data.content.Sort;
    }
}