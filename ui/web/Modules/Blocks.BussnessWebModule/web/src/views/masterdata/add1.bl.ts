import { Controller, Component, Prop } from "interface"
// import add from "./add.bl"
@Component
export default class Add extends Controller {
    @Prop({ type: Object }) formData;
    selectData = [
        { value: "1", label: "张三" },
        { value: "2", label: "李四" },
        { value: "3", label: "王五" }
    ]
    ID = "";
    btnloading = false;
    selectRemoteData = [];
    theSelect2 = ["1", "2"];
    theSelect3 = "";
    theSelect4 = "";
    ruleForm = {}
    rules = {
        name: [
            { required: true, message: '姓名不能为空', trigger: 'blur' },
            { min: 2, max: 10, message: '长度在 2 到 10 个字符', trigger: 'blur' }
        ],
        age: [
            { required: true, message: '年龄不能为空', trigger: 'blur' },
            { validator: this.checkAge, trigger: 'blur' }
        ]
    }

    constructor() {
        super();
    }

    checkAge(rule, value, callback) {
        if (!value) {
            return callback(new Error('年龄不能为0'));
        }
        if (!Number.isInteger(parseInt(value))) {
            callback(new Error('请输入数字'));
        } else {
            if (parseInt(value) < 1 || parseInt(value) > 150) {
                callback(new Error('年龄为1-150'));
            } else {
                callback();
            }
        }
    }
    async remoteMethod(query) {
        //todo:http 请求
        if (query != "") {
            this.selectRemoteData = [
                { value: "1", label: "甲" },
                { value: "2", label: "乙" },
                { value: "3", label: "丙" }
            ]
        }
        else {
            this.selectRemoteData = [];
        }
    }
    async submitForm() {
        this.btnloading = true;
        this.$refs.ruleForm.validate()
            .then(() => {
                //todo:http 请求
                this.exit();
            }).catch(() => {
                setTimeout(() => {
                    this.btnloading = false;
                }, 200)
            })
        // this.$refs.ruleForm.validate((valid) => {
        //     if (valid) {
        //         //todo:http 请求
        //         this.exit();
        //     } else {
        //         return false;
        //     }
        // });
    }
    async closeDialog() {
        // let dialog = await this.$dialog.create({
        //     component: add,
        //    // componentProps: { formData: this.formData },
        //     title: "新增"
        //   })
        //   await dialog.present();
        //   let result =  await dialog.onDidDismiss();
    }
    async viewWillEnter() {
        // this.ID = this.formData.ID;
        // //todo:http 请求
        // if (this.ID != "") {
        //     this.ruleForm.ID = this.ID;
        //     this.ruleForm.name = "test";
        //     this.ruleForm.age = 10;
        //     this.ruleForm.theDate = '2019-12-10 10:05:23';
        //     this.ruleForm.theSelect = "1";
        // }
    }
    viewDidEnter() {
    }
}