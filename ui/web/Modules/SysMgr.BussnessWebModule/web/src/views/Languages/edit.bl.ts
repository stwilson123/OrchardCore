import { Controller, Component, asyncCompatible, Prop, catchWrap } from "interface"
@Component
export default class Edit extends Controller {
    constructor() {
        super();
    }
    @Prop({ type: Object }) formData;
    @Prop({ type: Object }) container;
    get mainHeight() {
        return this.container.height - this.$getRef("footer").outerHeight();
    }
    get gridHeight() {
        let paddingHeight = this.$getRef("theMain").outerHeight(true) - this.$getRef("theMain").height();
        return (this.mainHeight - this.$getRef("ruleForm").outerHeight() - paddingHeight - 32);
    }
    index = 0;
    gridApi = null;
    gridOptions = {
        showPagination: false,
        rowSelection: "multiple",
        singleClickEdit: true
    }
    ruleForm = {
        ID: '', LanguageCode: '', LanguageName: '', LanguageIcon: ''
    };
    rules = {
        LanguageCode: [
            { required: true, message: '语言编码不能为空', trigger: 'blur' }
        ],
        LanguageName: [
            { required: true, message: '语言名称不能为空', trigger: 'blur' }
        ]
    }
    @asyncCompatible()
    async saveClick() {
        let [resObj, err] = await catchWrap(this.$refs.ruleForm.validate());
        if (resObj === false) {
            return;
        }
        this.gridApi.stopEditing();
        let data = Object.assign({}, { LanguageTextsInfos: this.rowData }, this.ruleForm);
        await this.$http({
            method: "post",
            url: "/api/services/SysMgrBussenssModule/Languages/Update",
            data
        })
        this.exit({ type: "success", message: "修改成功" });
    }
    cancelClick() {
        this.exit();
    }
    addClick() {
        this.index++;
        let rows = [];
        rows.push({ Id: this.index, LanguageModule: "", LanguageKey: "", LanguageValue: "" });
        let data = {
            index: 0,
            rows
        }
        this.$refs.AgGrid.setRowData(data);
    }
    async deleteClick() {
        let rows = this.gridApi.getSelectedRows();
        if (rows.length == 0) {
            this.$message({
                message: "请选择需要删除的数据",
                type: 'error'
            });
            return;
        }
        let data = {
            operate: "delete",
            ids: rows.map(n => n.Id)
        };
        this.$refs.AgGrid.setRowData(data);
    }
    async viewWillEnter() {
        this.gridOptions.columnDefs = [
            {
                headerName: "Id",
                field: "Id",
                hide: true
            },
            {
                headerName: "语言ID",
                field: "LanguageId",
                hide: true
            },
            {
                headerName: "语言编码",
                field: "LanguageCode",
                hide: true
            },
            {
                headerName: "所属模块",
                field: "LanguageModule",
                hide: true
            },
            {
                headerName: "翻译编码",
                field: "LanguageKey",
                blType: 'text',
                checkboxSelection: true,
                editable: true,
                filter: true,
                width: 400
            },
            {
                headerName: "翻译内容",
                field: "LanguageValue",
                blType: 'text',
                editable: true,
                filter: true,
                width: 400
            }
        ];
        this.ruleForm.ID = this.formData.ID;
        let res = await this.$http({
            method: "post",
            url: "/api/services/SysMgrBussenssModule/Languages/GetOneById",
            data: this.ruleForm
        });
        this.ruleForm.LanguageCode = res.data.content.LanguageCode;
        this.ruleForm.LanguageName = res.data.content.LanguageName;
        this.ruleForm.LanguageIcon = res.data.content.LanguageIcon;
        let rows = {
            rows: res.data.content.LanguageTextsInfos
        };
        this.$refs.AgGrid.setRowData(rows);
    }
    async viewDidEnter() {
        this.gridApi = this.$refs.AgGrid.api();
    }
    get rowData() {
        return this.$refs.AgGrid.getRowData();
    }
}