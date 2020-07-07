import { Controller, Component, Prop, asyncCompatible, catchWrap } from "interface"
@Component
export default class add extends Controller {
    @Prop({ type: Object }) formData;
    constructor() {
        super();
    }
    addData = { FileType: "" };
    serverpath = "";
    filename = "";
    selectFileType = [];
    ruleForm = {
        FileFunction: '', FileType: '', FileName: '', FilePath: '', FileData: {}, Id: ''
    };
    rules = {
        FileFunction: [
            { required: true, message: '请维护功能说明', trigger: 'blur' }
        ],
        FileType: [
            { required: true, message: '请选择文件类型', trigger: 'blur' }
        ]
    }
    async viewWillEnter() {
        this.ruleForm.Id = this.formData.ID;
        await this.getselectType();
        await this.getUploadPath();
        await this.getDataByID();
    }
    async getDataByID() {
        let data = Object.assign({}, this.ruleForm);
        let res = await this.$http({
            method: "post",
            url: "/api/services/SysMgrBussenssModule/ConfigFiles/GetOneById",
            data
        });
        this.ruleForm.FileFunction = res.data.content.FileFunction;
        this.ruleForm.FileType = res.data.content.FileType;
        this.filename = res.data.content.FileName;
    }

    async getselectType() {
        let data = {
            page: {
            },
            ID: "ConfigFileType"
        }
        let resultData = await this.$http({
            method: "post",
            url: '/api/services/FactoryCfgBussenssModule/Dictionary/GetListByTypeNo',
            data: data
        });

        let content = resultData.data.content;
        for (var i = 0; i < content.length; i++) {
            this.selectFileType.push({
                text: content[i].DicName, id: content[i].DicNo, url: content[i].DicValue
            });
        }
    }

    selectTypeChange(data) {
        for (var i = 0; i < this.selectFileType.length; i++) {
            if (data == this.selectFileType[i].id) {
                this.addData.FileType = this.selectFileType[i].url;
                break;
            } else {
                continue;
            }
        }
    }

    async uploadClick() {
        if (this.$refs.upload.uploadFiles > 0 && this.filename != this.$refs.upload.uploadFiles[0].name) {
            await this.$confirm("选择的文件与服务器上的文件名不一致，是否确定上传?", this.$l("tips"), {
                confirmButtonText: this.$l("YES"),
                cancelButtonText: this.$l("cancel"),
                type: 'warning'
            });
            this.$refs.upload.submit();
        }
        else {
            this.$refs.upload.submit();
        }
    }
    //获取系统配置数据
    async getUploadPath() {
        let data = {
            SetupNo: "UploadPath"
        }
        let resultData = await this.$http({
            method: "post",
            url: '/api/services/SysMgrBussenssModule/Setup/GetOneByCode',
            data: data
        });
        let content = resultData.data.content;
        if (content != null) {
            this.serverpath = content.SetupParameter;
        }
    }
    beforeUpload(file) {
        // var testmsg = file.name.substring(file.name.lastIndexOf('.') + 1)
        // const extension = testmsg === 'xls'
        // const extension2 = testmsg === 'xlsx'
        // const extension3 = testmsg === 'xml'
        // if (!extension && !extension2 && !extension3) {
        //     this.$message({
        //         message: '上传文件只能是 xml、xls、xlsx格式!',
        //         type: 'warning'
        //     });
        // }
        // return extension || extension2 || extension3
    }
    onProgress(event, file, fileList) {
    }
    handleError(err, file, fileList) {
        this.$message({
            message: '失败',
            type: 'error'
        });
        return;
    }
    async handleSuccess(response, file, fileList) {
        if (response.code == "101") {
            this.$message.success(response.msg);
            return
        }
        this.ruleForm.FileName = file.name;
        this.ruleForm.FilePath = this.serverpath + this.addData.FileType + file.name;

        let datas = Object.assign({}, this.ruleForm);
        let res = await this.$http({
            method: "post",
            url: "/api/services/SysMgrBussenssModule/ConfigFiles/Update",
            data: datas
        });

        if (res.data.code != "200") {
            this.$message.error(res.data.msg);
            return;
        }
        this.$message.success(res.data.content);
        this.exit();
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
            url: "/api/services/SysMgrBussenssModule/ConfigFiles/Update",
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