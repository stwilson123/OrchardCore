import { Controller, Component, asyncCompatible, Prop } from "interface"
@Component
export default class bindingUser extends Controller {
    @Prop({ type: Object }) formData;
    ID;
    gridOptions1 = {
        paginationPageSize: 12,
        rowSelection: "multiple",
        showPagination: false
    };
    gridOptions2 = {
        paginationPageSize: 12,
        rowSelection: "multiple",
        showPagination: false
    };

    selectData = [
        { value: "0", label: "启用" },
        { value: "2", label: "停用" }
    ];
    async gridData1(params) {
        params = Object.assign({ RoleId: this.ID }, params);
        let res = await this.$http({
            method: "post",
            url: "/api/services/SysMgrBussenssModule/SysUserInfo/GetRoleUserList",
            data: params
        });
        this.$refs.AgGrid1.clearRowData();
        this.$refs.AgGrid1.setRowData({
            rows: res.data.content
        });
    }
    async gridData2(params) {
        params = Object.assign({ RoleId: this.ID }, params);
        let res = await this.$http({
            method: "post",
            url: "/api/services/SysMgrBussenssModule/SysUserInfo/GetRoleAuList",
            data: params
        });
        this.$refs.AgGrid2.setData(res.data.content);
    }

    async rowDoubleClicked() {
        this.rightAssignRoles();
    }
    async deleteClick() {
        let rows = this.$refs.AgGrid2.api().getSelectedRows();
        let delData = {
            operate: 'delete',
            ids: rows.map(n => n.ID)
        };
        let addData = {
            rows: rows
        }
        this.$refs.AgGrid2.setRowData(delData);
        this.$refs.AgGrid1.setRowData(addData);
    }
    async saveClick() {
        let rows = this.$refs.AgGrid2.getRowData();
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
            url: "/api/services/SysMgrBussenssModule/SysRoleInfo/SaveRoleAndUser",
            data: params
        });
        this.$refs.AgGrid1.refresh();
        this.$refs.AgGrid2.refresh();
        this.$message({
            message: res.data.content
        });
    }

    async rightClick() {
        this.rightAssignRoles();
    }
    async rightAssignRoles() {
        let rows = this.$refs.AgGrid1.api().getSelectedRows();
        let addData = {
            rows: rows
        }
        let delData = {
            operate: 'delete',
            ids: rows.map(n => n.ID)
        };
        this.$refs.AgGrid1.setRowData(delData);
        this.$refs.AgGrid2.setRowData(addData);
    }
    viewWillEnter() {
        this.gridOptions1.columnDefs = [
            {
                headerName: "ID",
                field: "ID",
                hide: true
            },
            {
                headerName: "账号",
                field: "UserCode",
                blType: 'text',
                filter: true,
                checkboxSelection: true,
                headerCheckboxSelection: true
            },
            {
                headerName: "姓名",
                field: "CName",
                blType: 'text',
                filter: true
            },
            {
                headerName: "状态",
                field: "State",
                blType: 'select',
                filter: true,
                blParams: {
                    optionsData: this.selectData,
                    clearable: true
                }
            },
            {
                headerName: "备注",
                field: "Memo",
                blType: 'text',
                filter: true
            }
        ];
        this.gridOptions2.columnDefs = [
            {
                headerName: "ID",
                field: "ID",
                hide: true
            },
            {
                headerName: "账号",
                field: "UserCode",
                blType: 'text',
                filter: true,
                checkboxSelection: true,
                headerCheckboxSelection: true
            },
            {
                headerName: "姓名",
                field: "CName",
                blType: 'text',
                filter: true
            },
            {
                headerName: "状态",
                field: "State",
                blType: 'select',
                filter: true,
                blParams: {
                    optionsData: this.selectData,
                    clearable: true
                }
            },
            {
                headerName: "备注",
                field: "Memo",
                blType: 'text',
                filter: true
            }
        ];
        this.ID = this.formData.ID;
    }
}
