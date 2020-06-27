import { Controller, Component, Prop, asyncCompatible, catchWrap } from "interface"
import blocks from "blocks"
@Component
export default class Add extends Controller {
    @Prop({ type: Object }) formData;
    selectData = [
        { value: "1", label: "张三" },
        { value: "2", label: "李四" },
        { value: "3", label: "王五" }
    ]
    Id = "";
    initData = {};
    selectRemoteData = [];
    theSelect2 = ["1", "2"];
    theSelect3 = "";
    theSelect4 = "";
    ruleFormRef = {};
    ruleForm = {
        Id: "",
        city: "",
        num: "0",
        registerTime: "",
        comboboxId: "1"
    };
    remoteUrl = "/api/services/BussnessWebModule/Combobox/GetComboboxList";
    rules = {
        city: [
            { required: true, message: '姓名不能为空', trigger: 'blur' },
            { min: 2, max: 10, message: '长度在 2 到 10 个字符', trigger: 'blur' }
        ],
        num: [
            { required: true, message: '年龄不能为空', trigger: 'blur' },
            { validator: this.checkAge, trigger: 'blur' }
        ],
        registerTime: [
            { required: true, message: '日期不能为空', trigger: 'blur' },
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
    @asyncCompatible()
    async remoteMethod(query, params) {

        let res = await this.$http({
            method: "post",
            url: params.url,
            data: params.data
        });
        // this.selectRemoteData = res.data.content.rows;
        this.$refs.theSelect4.setData(res.data.content.rows.concat([{ id: 1, text: 1 }, { id: 2, text: 2 }, { id: 3, text: 3 }, { id: 4, text: 4 }, { id: 5, text: 5 }, { id: 2, text: 2 }, { id: 3, text: 3 }, { id: 4, text: 4 }, { id: 5, text: 5 }, { id: 2, text: 2 }, { id: 3, text: 3 }, { id: 4, text: 4 }, { id: 5, text: 5 }]))
   
    }
    @asyncCompatible()
    async submitForm() {


        let [resObj, err] = await catchWrap(this.$refs.ruleForm.validate());
        if (resObj === false) {
            return;
        }
        let data = Object.assign({}, this.ruleForm);
        data.registerTime = blocks.utility.dateConvert.toUtcDate(data.registerTime);
        let res = await this.$http({
            method: "post",
            url: "/api/services/BussnessWebModule/test/add",
            data: data
        });

        this.exit({ type: "success", message: "新增成功" });
        // this.$refs.ruleForm.validate()
        //     .then(async () => {
        //         let data = Object.assign({}, this.ruleForm);
        //         data.registerTime = blocks.utility.dateConvert.toUtcDate(data.registerTime);
        //         let res = await this.$http({
        //             method: "post",
        //             url: "/api/services/BussnessWebModule/test/add",
        //             data: data
        //         });
        //         this.exit({ type: "success", message: "新增成功" });
        //     }).catch(() => {
        //     })
    }
    closeDialog() {
        this.exit({ message: "取消" });
    }
    async viewWillEnter() {

        this.initData = await this.$http({
            method: "post",
            url: "/api/services/BussnessWebModule/masterdata/get",
            data: { Id: this.formData.Id }
        });
        await Promise.all([this.$refs.theSelect4.initializeData({isInitializeSelectData:true,data:1}),this.$refs.theSelect5.initializeData({isInitializeSelectData:true,data:1})])
      
    }
    async viewDidEnter() {

        
    }
}