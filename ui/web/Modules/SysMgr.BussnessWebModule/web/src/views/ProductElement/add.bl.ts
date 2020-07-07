import { Controller, Component, Prop, asyncCompatible } from "interface"
@Component
export default class add extends Controller {
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
  async viewWillEnter() {
    await this.getselectTypeData();
  }
  ruleForm = {
  };
  rules = {
    Code: [
      { required: true, message: '编码不能为空', trigger: 'blur' },
      { min: 2, max: 10, message: '长度在 2 到 10 个字符', trigger: 'blur' }
    ],
    Name: [
      { required: true, message: '名称不能为空', trigger: 'blur' },
      { min: 2, max: 10, message: '长度在 2 到 10 个字符', trigger: 'blur' }
    ],
    ElementTypeId: [
      { required: true, message: '请选择类型', trigger: ['blur', 'change'] }
    ]
  }
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

  @asyncCompatible()
  async submitForm() {
    this.$refs.ruleForm.validate(async (res, obj) => {
      if (res) {
        let data = Object.assign({}, this.ruleForm);
        data.registerTime = blocks.utility.dateConvert.toUtcDate(data.registerTime);
        let res = await this.$http({
          method: "post",
          url: "/api/services/SysMgrBussenssModule/ProductElement/Add",
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