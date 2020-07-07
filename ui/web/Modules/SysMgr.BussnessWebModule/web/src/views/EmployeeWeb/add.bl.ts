import { Controller, Component, Prop, asyncCompatible, catchWrap } from "interface"
@Component
export default class add extends Controller {
  @Prop({ type: Object }) self;
  constructor() {
    super();
  }
  ruleForm = {
    Code: '', Name: '', combobox: '', EmpType: '', Email: '', Phone: '', Desc: ''
  };
  selectType = [
    { id: "A", text: "正式员工" },
    { id: "T", text: "临时员工" },
    { id: "Q", text: "离职员工" }
  ]
  selectcombobox = []
  async viewWillEnter() {
    await this.getselectcombobox();
  }
  //获取部门下拉
  async getselectcombobox() {
    let data = {
      page: {
        page: 1, pagesize: 100
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
      return;
    }
    let data = Object.assign({}, this.ruleForm);

    let res = await this.$http({
      method: "post",
      url: "/api/services/SysMgrBussenssModule/Employee/Add",
      data: data
      //
    });

    if (res.data.code != "200") {

      this.$message.error(res.data.msg);
      return;
    }

    this.$message.success(res.data.content);
    this.exit();

  }
}