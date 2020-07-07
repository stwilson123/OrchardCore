import { Controller, Component, Prop, asyncCompatible, catchWrap } from "interface"
@Component
export default class edit extends Controller {
  @Prop({ type: Object }) formData;
  constructor() {
    super();
  }
  com4bobox = "";
  Emp4Type = "";
  selectType = [
    { id: "A", text: "正式员工" },
    { id: "T", text: "临时员工" },
    { id: "Q", text: "离职员工" }
  ]
  selectcombobox = []
  ruleForm = {
    ID: '', Code: '', Name: '', combobox: '', EmpType: '', Email: '', Phone: '', Desc: ''
  };
  rules = {
    Code: [
      { required: true, message: '请填写编码', trigger: 'blur' }
    ],
    Name: [
      { required: true, message: '请填写名称', trigger: 'blur' }
    ],
    combobox: [
      { required: true, message: '请填写所属部门', trigger: 'change' }
    ],
    EmpType: [
      { required: true, message: '请填写类型', trigger: 'change' }
    ]
  }
  check() {
    var checkEmail = /^[a-z0-9]+@([a-z0-9]+\.)+[a-z]{2,4}$/;
    if (checkEmail.test(this.ruleForm.Email) == false && this.ruleForm.Email != "" && this.ruleForm.Email != null) {
      this.$message({
        message: "请输入正确的邮箱",
        type: 'warning'
      });
      return false;

    }

    if (this.ruleForm.Phone != null && this.ruleForm.Phone != "") {
      var length = this.ruleForm.Phone.length;
      var mobile = /^(((13[0-9]{1})|(15[0-9]{1})|(18[0-9]{1})|(17[0-9]{1}))+\d{8})$/;
      if (length != 11 || mobile.test(this.ruleForm.Phone) == false) {
        this.$message({
          message: "请填写正确的电话号码",
          type: 'warning'
        });
        return false;
      }
    }
    return true;

  }
  async viewWillEnter() {
    await this.getselectcombobox();
    this.ruleForm.ID = this.formData.ID;
    await this.getDataByID();
  }
  //获取部门下拉
  async getselectcombobox() {
    let data = {
      page: {
      }
    }
    let resultData = await this.$http({
      method: "post",
      url: '/api/services/SysMgrBussenssModule/Department/GetComboxList',
      data: data
    });

    let content = resultData.data.content;
    for (var i = 0; i < content.rows.length; i++) {
      this.selectcombobox.push({
        text: content.rows[i].text, id: content.rows[i].id
      });
    }
  }
  async getDataByID() {
    let data = Object.assign({}, this.ruleForm);
    let result = await this.$http({
      method: "post",
      url: '/api/services/SysMgrBussenssModule/Employee/GetOneById',
      data
    });
    if (result.data.code != "200") {
      this.$message.error(result.data.msg);
      return;
    }
    this.ruleForm.Name = result.data.content.Name;
    this.ruleForm.Code = result.data.content.Code;
    this.ruleForm.Desc = result.data.content.Desc;
    this.ruleForm.combobox = result.data.content.combobox;
    this.ruleForm.EmpType = result.data.content.EmpType;
    this.ruleForm.Phone = result.data.content.Phone;
    this.ruleForm.Email = result.data.content.Email;
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
    if (!this.check()) {
      this.$message.error('失败');
      return;
    }
    let data = Object.assign({}, this.ruleForm);
    let res = await this.$http({
      method: "post",
      url: "/api/services/SysMgrBussenssModule/Employee/Update",
      data: data
    });
    if (res.data.code != "200") {
      this.$message.error(res.data.msg);
      return;
    }
    this.$message.success(res.data.content);
    this.exit();
  }
}