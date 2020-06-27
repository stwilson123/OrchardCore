import { Controller, Component, Prop, asyncCompatible, catchWrap } from "interface"
import blocks from "blocks"

@Component
export default class Add extends Controller {
    @Prop({ type: Object }) formData;
    @Prop({ type: Object })
    container;
    selectData = [
        { id: "1", text: "张三" },
        { id: "2", text: "李四" },
        { id: "3", text: "王五" }
    ]
    Id = "";
    inputModel = "";
    theSelect2 = ["1", "2"];
    theSelect3 = "";
    theSelect4 = "";
    remoteUrl = "/api/services/BussnessWebModule/Combobox/GetComboboxList";
    remoteCount = 4;
    remoteIndex = 0;
    ruleForm = {
        Id: "",
        city: "",
        num: "0",
        registerTime: new Date(),
        comboboxId: "1"
    };
    rules = {
        city: [
            { required: true, message: '姓名不能为空', trigger: ['blur', 'change'] },
            { min: 2, max: 10, message: '长度在 2 到 10 个字符', trigger: 'blur' }
        ],
        num: [
            { required: true, message: '年龄不能为空', trigger: 'blur' },
            { validator: this.checkAge, trigger: 'blur' }
        ],
        registerTime: [
            { required: true, message: '日期不能为空', trigger: ['change', 'blur'] },
        ],
        combobox: [
            { required: true, message: '下拉框不能为空', trigger: ['blur', 'change'] },
        ],
    }

    constructor() {
        super();
    }

    get mainHeight() {
        //debugger
        return this.container.height - this.$getRef("footer").outerHeight();
    }

    get gridHeight() {
        //debugger
        let paddingHeight = this.$getRef("theMain").outerHeight(true) - this.$getRef("theMain").height();
        return (this.mainHeight - this.$getRef("ruleForm").outerHeight() - paddingHeight);
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
        if (query != "") {
            // this.selectRemoteData = [
            //     { id: "1", text: "甲" },
            //     { id: "2", text: "乙" },
            //     { id: "3", text: "丙" }
            // ]
            let res = await this.$http({
                method: "post",
                url: params.url,
                data: params.data
            });
            // this.selectRemoteData = res.data.content.rows;
            let dataSource = res.data.content.rows.concat([{ id: 1, text: 1 }, { id: 2, text: 2 }, { id: 3, text: 3 }, { id: 4, text: 4 },
            { id: 5, text: 5 }, { id: 6, text: 6 }, { id: 7, text: 7 }, //{ id: 2, text: 2 }, { id: 3, text: 3 }, { id: 4, text: 4 }, { id: 5, text: 5 }, 
                // { id: 2, text: 2 }, { id: 3, text: 3 }, { id: 4, text: 4 }, { id: 5, text: 5 }
            ]);
            params.api.setData(this.remoteIndex++ < this.remoteCount ? dataSource : [])
        }
        else {
            params.api.setData([]);
        }
    }
    @asyncCompatible()
    async submitForm() {
        let [resobject, err] = await catchWrap(this.$refs.ruleForm.validate());
        if (resobject === false) {
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

        // this.$refs.ruleForm.validate(async (validate, obj) => {
        //     if (validate) {
        //         let data = Object.assign({}, this.ruleForm);
        //         data.registerTime = blocks.utility.dateConvert.toUtcDate(data.registerTime);
        //         let res = await this.$http({
        //             method: "post",
        //             url: "/api/services/BussnessWebModule/test/add",
        //             data: data
        //         });
        //         this.exit({ type: "success", message: "新增成功" });
        //     }
        // })
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
        this.Id = this.formData.Id;
        //todo:http 请求
        if (this.Id != "") {
            let obj = {
                Id: this.Id,
                city: "test",
                num: "10",
                registerTime: '2019-12-10 10:05:23',
                comboboxId: "1"
            }
            this.ruleForm = obj;
            // this.ruleForm.Id = this.Id;
            // this.ruleForm.city = "test";
            // this.ruleForm.num = 10;
            // this.ruleForm.registerTime = '2019-12-10 10:05:23';
            // this.ruleForm.comboboxId = "1";
        }
        //await this.$nextTick(() => { console.log(1)})
    }
    viewDidEnter() {
    }
}