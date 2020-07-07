import { Controller, Component, asyncCompatible, Prop } from "interface"
@Component
export default class copyRole extends Controller {
    @Prop({ type: Object }) formData;
    ID;
    gridOptions = {
        paginationPageSize: 12,
        rowSelection: "multiple"
    }
    gridApi = null
    async gridData(params) {
        let res = await this.$http({
            method: "post",
            url: "/api/services/SysMgrBussenssModule/SysRoleInfo/GetPageList",
            data: params
        });
        this.$refs.AgGrid.setData(res.data.content);
    }
    viewDidEnter() {
        this.gridApi = this.$refs.AgGrid.api();
    }
    closeDialog() {
        this.exit();
    }
    @asyncCompatible()
    async saveClick() {
        let rows = this.gridApi.getSelectedRows();
        if (rows.length == 0) {
            this.$message({
                message: "请选择要复制的权限的行",
                type: 'error'
            });
            return;
        }
        let array = [];
        rows.forEach(element => {
            array.push(element.ID);
        });
        var params = {
            RoleInfoID: this.ID,
            Ids: array
        };
        let res = await this.$http({
            method: "post",
            url: "/api/services/SysMgrBussenssModule/SysRoleInfo/SaveRoleMenu",
            data: params
        });
        if (res.data.code == "200") {
            this.exit({ type: "success", message: "复制成功" });
        } else {
            this.$message({
                message: res.data.msg
            });
        }
    }

    viewWillEnter() {
        this.gridOptions.columnDefs = [
            {
                headerName: "ID",
                field: "ID",
                hide: true,
                checkboxSelection: true,
                headerCheckboxSelection: true
            },
            {
                headerName: "名称",
                field: "Name",
                filter: true,
                blType: 'text',
                sortable: true,
                sort: "asc",
                checkboxSelection: true,
                headerCheckboxSelection: true
            },
            {
                headerName: "备注",
                field: "Remark",
                blType: 'text',
                filter: true
            }
        ];
        this.ID = this.formData.ID;
    }
}