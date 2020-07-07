import { Controller, Component, Prop, asyncCompatible } from "interface"
@Component
export default class edit extends Controller {
  @Prop({ type: Object }) formData;
  constructor() {
    super();
  }
  selectData = [
    { id: "0", text: "是" },
    { id: "1", text: "否" }
  ]
  ruleForm = {
    ID: '', Code: '', Name: '', IsVariable: '0',
  };
  rules = {
    Code: [
      { required: true, message: '请填写编号', trigger: 'blur' }
    ],
    Name: [
      { required: true, message: '请填写名称', trigger: 'blur' }
    ],
    IsVariable: [
      { required: true, message: '请选择是否为变量', trigger: ['blur', 'change'] }
    ]
  }
  async viewWillEnter() {
    this.ruleForm.ID = this.formData.ID;
    await this.getDataByID();
  }
  async getDataByID() {
    let data = Object.assign({}, this.ruleForm);
    let result = await this.$http({
      method: "post",
      url: '/api/services/SysMgrBussenssModule/ProductElementType/GetOneById',
      data
    });
    if (result.data.code != "200") {
      this.$message.error(result.data.msg);
      return;
    }
    this.ruleForm.ID = result.data.content.ID;
    this.ruleForm.Code = result.data.content.Code;
    this.ruleForm.Name = result.data.content.Name;
    this.ruleForm.IsVariable = result.data.content.IsVariable;
  }

  async closeDialog() {
    this.exit();
  }
  @asyncCompatible()
  async submitForm() {
    this.$refs.ruleForm.validate(async (res, obj) => {
      if (res) {

        let data = Object.assign({}, this.ruleForm);
        let res = await this.$http({
          method: "post",
          url: "/api/services/SysMgrBussenssModule/ProductElementType/Update",
          data
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
}