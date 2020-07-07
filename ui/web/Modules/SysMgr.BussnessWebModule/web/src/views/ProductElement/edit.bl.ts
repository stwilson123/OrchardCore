import { Controller, Component, Prop, asyncCompatible } from "interface"
@Component
export default class edit extends Controller {
  @Prop({ type: Object }) formData;
  constructor() {
    super();
  }
  selectTypeData = []
  SelectResetDate = [
    { id: "0", text: "无" },
    { id: "1", text: "年" },
    { id: "2", text: "月" },
    { id: "3", text: "日" }
  ]
  ruleForm = {
    ID: '', Code: '', Name: '', ResetDate: '', AutoIncrement: '', Length: '', Default: '', Description: '', ElementTypeId: ''
  };
  rules = {
    Code: [
      { required: true, message: '请填写编号', trigger: 'blur' }
    ],
    Name: [
      { required: true, message: '请填写名称', trigger: 'blur' }
    ],
    ElementTypeId: [
      { required: true, message: '请选择类型', trigger: ['blur', 'change'] }
    ]
  }
  async viewWillEnter() {
    await this.getselectTypeData();
    this.ruleForm.ID = this.formData.ID;
    await this.getDataByID();
  }
  //获取类型数据
  async getselectTypeData() {
    const data = {
      page: {
        page: 1, pagesize: 100
      }
    }
    let resultData = await this.$http({
      method: "post",
      url: '/api/services/SysMgrBussenssModule/ProductElementType/GetComboxList',
      data
    });
    let content = resultData.data.content;
    for (var i = 0; i < content.rows.length; i++) {
      this.selectTypeData.push({
        text: content.rows[i].text, id: content.rows[i].id
      });
    }
    return content;
  }

  async getDataByID() {
    let data = Object.assign({}, this.ruleForm);
    let result = await this.$http({
      method: "post",
      url: '/api/services/SysMgrBussenssModule/ProductElement/GetOneById',
      data
    });
    if (result.data.code != "200") {
      this.$message.error(result.data.msg);
      return;
    }
    this.ruleForm.ID = result.data.content.ID;
    this.ruleForm.Code = result.data.content.Code;
    this.ruleForm.Name = result.data.content.Name;
    this.ruleForm.ResetDate = result.data.content.ResetDate;
    this.ruleForm.AutoIncrement = result.data.content.AutoIncrement;
    this.ruleForm.Length = result.data.content.Length;
    this.ruleForm.Default = result.data.content.Default;
    this.ruleForm.Description = result.data.content.Description;
    this.ruleForm.ElementTypeId = result.data.content.ElementTypeId;
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
          url: "/api/services/SysMgrBussenssModule/ProductElement/Update",
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