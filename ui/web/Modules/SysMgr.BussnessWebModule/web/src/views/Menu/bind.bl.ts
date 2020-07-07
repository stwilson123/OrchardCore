import { Controller, Component, Prop, asyncCompatible } from "interface"
@Component
export default class Bind extends Controller {
    @Prop({ type: Object }) formData;
    constructor() {
        super();
    }
    programParent = "";
    platForm = "";
    filterParamsLeft = {};
    filterParamsRight = {};
    gridOptionsLeft = {
        showPagination: false,
        rowSelection: "multiple"
    };
    gridApiLeft = null;
    gridApiRight = null;
    gridOptionsRight = {
        showPagination: false,
        floatingFilter: false,
        rowSelection: "multiple"
    };
    async gridDataLeft(params) {
        this.filterParamsLeft = params;
        let paramsObj = Object.assign({}, this.filterParamsLeft, { Platform: this.platForm })
        let res = await this.$http({
            method: "post",
            url: "/api/services/SysMgrBussenssModule/SysProgram/GetPageList",
            data: paramsObj
        });
        this.$refs.LeftGrid.setData(res.data.content);
    }
    async gridDataRight(params) {
        this.filterParamsRight = params;
        let paramsObj = Object.assign({}, this.filterParamsRight, { ProgramParent: this.programParent, Platform: this.platForm })
        let res = await this.$http({
            method: "post",
            url: "/api/services/SysMgrBussenssModule/SysProgram/GetPageList",
            data: paramsObj
        });
        this.$refs.RightGrid.setData(res.data.content);
    }
    async rightClick() {
        let rows = this.gridApiLeft.getSelectedRows();
        if (rows.length == 0) {
            this.$message({
                message: "请选择需要移动的数据",
                type: 'error'
            });
            return;
        }
        let addData = {
            rows: rows
        }
        let delData = {
            operate: 'delete',
            ids: rows.map(n => n.ID)
        };
        this.$refs.RightGrid.setRowData(addData);
        this.$refs.LeftGrid.setRowData(delData);
    }
    async deleteClick() {
        let rows = this.gridApiRight.getSelectedRows();
        if (rows.length == 0) {
            this.$message({
                message: "请选择需要删除的数据",
                type: 'error'
            });
            return;
        }
        let delData = {
            operate: 'delete',
            ids: rows.map(n => n.ID)
        };
        let addData = {
            rows: rows
        }
        this.$refs.RightGrid.setRowData(delData);
        this.$refs.LeftGrid.setRowData(addData);
    }
    @asyncCompatible()
    async saveClick() {
        let data = {
            ListsSysProgramInfos: this.rightRowData,
            ID: this.programParent,
            Platform: this.platForm
        }
        await this.$http({
            method: "post",
            url: "/api/services/SysMgrBussenssModule/SysProgram/Bind",
            data
        });
        this.exit({ success: true, data: { type: "success", message: "绑定成功" } });
    }

    async closeClick() {
        this.exit({ success: false });
    }
    async viewWillEnter() {
        this.programParent = this.formData.ProgramParent;
        this.platForm = this.formData.Platform;
        this.gridOptionsLeft.columnDefs = [{
            headerName: "ID",
            field: "ID",
            hide: true
        },
        {
            headerName: "名称",
            field: "Name",
            filter: true,
            blType: 'text',
            checkboxSelection: true,
            headerCheckboxSelection: true
        },
        {
            headerName: "编码",
            field: "Code",
            filter: true,
            blType: 'text'
        },
        {
            headerName: "备注",
            field: "Desc",
            blType: 'text'
        }];
        this.gridOptionsRight.columnDefs = [{
            headerName: "ID",
            field: "ID",
            hide: true
        },
        {
            headerName: "名称",
            field: "Name",
            blType: 'text',
            checkboxSelection: true,
            headerCheckboxSelection: true
        },
        {
            headerName: "编码",
            field: "Code",
            blType: 'text'
        },
        {
            headerName: "备注",
            field: "Desc",
            blType: 'text'
        }];
    }
    viewDidEnter() {
        this.gridApiLeft = this.$refs.LeftGrid.api();
        this.gridApiRight = this.$refs.RightGrid.api();
    }
    get rightRowData() {
        return this.$refs.RightGrid.getRowData();
    }
}