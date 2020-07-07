import { Controller, Component, Prop, asyncCompatible, catchWrap } from "interface"
@Component
export default class add extends Controller {
  @Prop({ type: Object }) self;
  constructor() {
    super();
  }
  ruleForm = {
  };
  rules = {
    Code: [
      { required: true, message: '请填写编码', trigger: 'blur' }
    ],
    Name: [
      { required: true, message: '请填写名称', trigger: 'blur' }
    ]
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
      url: "/api/services/SysMgrBussenssModule/Department/Add",
      data: data
    });
    if (res.data.code != "200") {

      this.$message.error(res.data.msg);
      return;
    }
    this.exit({ type: "success", message: this.$l('succeed') });
  }
}