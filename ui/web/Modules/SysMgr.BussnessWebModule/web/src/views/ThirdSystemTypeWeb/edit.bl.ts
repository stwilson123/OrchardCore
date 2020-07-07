import { Controller, Component, Prop, asyncCompatible, catchWrap } from "interface"
@Component
export default class edit extends Controller {
  @Prop({ type: Object }) formData;
  constructor() {
    super();
  }

  ruleForm = {
    ID: '', SystemNo: '', SystemName: ''
  };
  rules = {
    SystemNo: [
      { required: true, message: '请填写系统编号', trigger: 'blur' }
    ],
    SystemName: [
      { required: true, message: '请填写系统名称', trigger: 'blur' }
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
      url: '/api/services/SysMgrBussenssModule/ThirdSystemType/GetOneById',
      data
    });
    if (result.data.code != "200") {
      this.$message.error(result.data.msg);
      return;
    }
    this.ruleForm.ID = result.data.content.ID;
    this.ruleForm.SystemNo = result.data.content.SystemNo;
    this.ruleForm.SystemName = result.data.content.SystemName;
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
      url: "/api/services/SysMgrBussenssModule/ThirdSystemType/Update",
      data: data
    });
    if (res.data.code != "200") {
      this.$message.error(res.data.msg);
      return;
    }
    this.exit({ type: "success", message: this.$l('succeed') });
  }
}