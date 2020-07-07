import { Controller, Component, Prop, asyncCompatible, catchWrap } from "interface"
@Component
export default class edit extends Controller {
  @Prop({ type: Object }) formData;
  constructor() {
    super();
  }

  ruleForm = {
    ID: '', Code: '', Name: '', Desc: ''
  };
  rules = {
    Code: [
      { required: true, message: '请填写编码', trigger: 'blur' }
    ],
    Name: [
      { required: true, message: '请填写名称', trigger: 'blur' }
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
      url: '/api/services/SysMgrBussenssModule/Department/GetOneById',
      data
    });
    if (result.data.code != "200") {
      this.$message.error(result.data.msg);
      return;
    }
    this.ruleForm.Name = result.data.content.Name;
    this.ruleForm.Code = result.data.content.Code;
    this.ruleForm.Desc = result.data.content.Desc;
  }

  async cancelClick() {
    this.exit();
  }
  @asyncCompatible()
  async submitForm() {
    let [resObj, err] = await catchWrap(this.$refs.ruleForm.validate());
    if (resObj === false) {
      return;
    }

    let data = Object.assign({}, this.ruleForm);
    let res = await this.$http({
      method: "post",
      url: "/api/services/SysMgrBussenssModule/Department/Update",
      data: data
      //
    });

    if (res.data.code != "200") {

      this.$message.error(res.data.msg);
      return;
    }

    this.exit({ type: "success", message: this.$l('succeed') });

  }
}