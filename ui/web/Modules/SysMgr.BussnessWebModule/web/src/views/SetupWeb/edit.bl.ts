import { Controller, Component, Prop, asyncCompatible, catchWrap } from "interface"
@Component
export default class edit extends Controller {
    @Prop({ type: Object }) formData;
    constructor() {
        super();
    }
    index = 1;
    ruleForm = {
        ID: '', SetupTypeNo: '', SetupTypeName: '', SetupTypeValue: ""
    };
    rules = {
        SetupTypeNo: [
            { required: true, message: '类型编码不能为空', trigger: 'blur' }
        ],
        SetupTypeName: [
            { required: true, message: '类型名称不能为空', trigger: 'blur' }
        ]
    }
    gridApi = null
    gridColumnApi = null
    gridOptions = {
        showPagination: false,
        floatingFilter: false,
        rowSelection: "multiple",
    }
    async viewWillEnter() {
        this.gridOptions.columnDefs = [
            {
                headerName: "ID",
                field: "ID",
                hide: true,
                width: 200,
                resizable: true
            },
            {
                headerName: "编码",
                field: "SetupNo",
                checkboxSelection: true,
                editable: true,
                sortable: false,
                width: 300,
                blType: 'text'
            },
            {
                headerName: "名称",
                field: "SetupContents",
                editable: true,
                sortable: false,
                width: 500,
                blType: 'text'
            },
            {
                headerName: "参数值",
                field: "SetupParameter",
                editable: true,
                sortable: false,
                width: 500,
                blType: 'text'
            }
        ];
        this.ruleForm.ID = this.formData.ID;
        await this.getDataByID();
    }
    viewDidEnter() {
        this.gridApi = this.$refs.editGrid.api();
        this.gridColumnApi = this.$refs.editGrid.columnApi();
    }
    async getDataByID() {
        let data = Object.assign({}, this.ruleForm);
        let result = await this.$http({
            method: "post",
            url: '/api/services/SysMgrBussenssModule/Setup/GetSetupTypeById',
            data
        });
        if (result.data.code != "200") {
            this.$message.error(result.data.msg);
            return;
        }
        this.ruleForm.SetupTypeNo = result.data.content.SetupTypeNo;
        this.ruleForm.SetupTypeName = result.data.content.SetupTypeName;
        this.ruleForm.SetupTypeValue = result.data.content.SetupTypeValue;
        this.index = result.data.content.SetupList.length;
        let rtn = {
            rows: result.data.content.SetupList
        }
        this.$refs.editGrid.setRowData(rtn);
    }

    async cancelClick() {
        this.exit();
    }
    addClick() {

        let array = [];
        array.push({ ID: this.index, SetupNo: "", SetupContents: "", SetupParameter: "" });
        this.deleteDuplicateData(array);
    }
    delClick() {
        let rows = this.$refs.editGrid.gridApi.getSelectedRows();
        if (rows.length == 0) {
            this.$message.error("请选择需要删除的数据!");
            return;
        }
        let ids = rows.map(n => n.ID);
        let data = {
            operate: "delete",
            ids: ids
        };

        this.$refs.editGrid.setRowData(data);
        this.index--;
    }
    get rowData() {
        return this.$refs.editGrid.getRowData();
    }
    //表格去重
    deleteDuplicateData(array) {
        var gridInfoDetail = this.rowData; //从表中拿到数据 
        for (let i = array.length; i > 0; i--) {
            for (let j = 0; j < gridInfoDetail.length; j++) {
                if (array[i - 1].ID == gridInfoDetail[j].ID) { //判断重复的依据
                    array.splice(i - 1, 1);
                    break;
                }
            }
        }

        if (array.length > 0) {
            let gridDatas = {
                operate: "add",
                rows: array
            }
            this.$refs.editGrid.setRowData(gridDatas);
        }
    }
    @asyncCompatible()
    async submitForm() {
        let [resObj, err] = await catchWrap(this.$refs.ruleForm.validate());
        if (resObj === false) {
            return;
        }
        this.gridApi.stopEditing();
        var gridInfoDetail = this.rowData; //从表中拿到数据 
        for (let i = 0; i < gridInfoDetail.length; i++) {
            let j = i + 1;
            if (gridInfoDetail[i].SetupNo == "") {
                this.$message.error("第" + j + "行编码不能为空！");
                return;
            }
            if (gridInfoDetail[i].SetupContents == "") {
                this.$message.error("第" + j + "行名称不能为空");
                return;
            }
            if (gridInfoDetail[i].SetupParameter == "") {
                this.$message.error("第" + j + "行参数不能为空");
                return;
            }
        }
        let data = Object.assign({}, { SetupList: gridInfoDetail }, this.ruleForm);
        let res = await this.$http({
            method: "post",
            url: '/api/services/SysMgrBussenssModule/Setup/EditSetupTypeAndDetail',
            data
        });
        if (res.data.code != "200") {
            this.$message.error(res.data.msg);
            return;
        }
        this.$message.success(res.data.content);
        this.exit();
    }
}