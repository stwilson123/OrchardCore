import { Controller, Component, Prop, asyncCompatible } from "interface"
@Component
export default class add extends Controller {
    @Prop({ type: Object }) formData;
    constructor() {
        super();
    }
    selectData = [
        { id: "0", text: "是" },
        { id: "1", text: "否" }
    ]
    ruleForm = {

    };
    rules = {
        Code: [
            { required: true, message: '编码不能为空', trigger: 'blur' },
            { min: 2, max: 10, message: '长度在 2 到 10 个字符', trigger: 'blur' }
        ],
        Name: [
            { required: true, message: '名称不能为空', trigger: 'blur' },
            { validator: this.checkAge, trigger: 'blur' }
        ],
        IsVariable: [
            { required: true, message: '请选择是否为变量', trigger: ['blur', 'change'] }
        ]
    }
    @asyncCompatible()
    async submitForm() {
        this.$refs.ruleForm.validate(async (res, obj) => {
            if (res) {
                let data = Object.assign({}, this.ruleForm);
                data.registerTime = blocks.utility.dateConvert.toUtcDate(data.registerTime);
                let res = await this.$http({
                    method: "post",
                    url: "/api/services/SysMgrBussenssModule/ProductElementType/Add",
                    data: data
                });
                if (res.data.code != "200") {
                    this.$message.error(res.data.msg);
                    return;
                }
                this.$message.success(res.data.content);
                this.exit();
            }
        })
    }
    closeDialog() {
        this.exit({ message: "取消" });
    }
}