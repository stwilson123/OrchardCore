import { Controller, Component, asyncCompatible, Prop } from "interface"
import add from './add.bl'
import edit from './edit.bl'
import password from './password.bl';
import assignRoles from './assignRoles.bl'
@Component
export default class Index extends Controller {
    constructor() {
        super();
    }
    @Prop({ type: Object }) container;
    get gridHeight() {
        return (this.container.height - this.$getRef("header").outerHeight()) + "px";
    }
    gridOptions = {
        paginationPageSize: 12,
        rowSelection: "multiple"
    }
    pageInfo = {
        page: {
        }
    }
    gridApi = null
    gridColumnApi = null
    selectData = [
        { id: "0", text: "启用" },
        { id: "2", text: "停用" },
    ];
    viewDidEnter() {
        this.gridApi = this.$refs.AgGrid.api();
        this.gridColumnApi = this.$refs.AgGrid.columnApi();
    }
    async gridData(params) {
        let res = await this.$http({
            method: "post",
            url: "/api/services/SysMgrBussenssModule/SysUserInfo/GetPageList",
            data: params
        });
        this.$refs.AgGrid.setData(res.data.content);
    }
    async addClick() {
        let dialog = await this.$dialog.create({
            component: add,
            title: "新增",
            componentProps: {}
        })
        await dialog.present();
        let result = await dialog.onDidDismiss();
        if (result) {
            this.$message(result);
            this.$refs.AgGrid.refresh();
        }
    }
    async disableClick() {
        let rows = this.gridApi.getSelectedRows();
        if (rows.length == 0) {
            this.$message({
                message: "请选择要修改的行",
                type: 'error'
            });
            return;
        }
        if (rows.length > 1) {
            this.$message({
                message: "只能选择一条数据！",
                type: 'error'
            });
            return;
        }
        await this.$confirm('确定停用吗?', '提示', {
            confirmButtonText: '确定',
            cancelButtonText: '取消',
            type: 'warning',

        }).then(async () => {
            let res = await this.$http({
                method: "post",
                url: "/api/services/SysMgrBussenssModule/SysUserInfo/Disable",
                data: rows[0]
            });
            if (res.data.code === "200") {
                this.$message({
                    message: "已停用",
                    type: 'success'
                });
            }
            else {
                this.$message({
                    message: res.data.msg,
                    type: 'error'
                });
            }
            this.$refs.AgGrid.refresh();
        })
    }

    async allotClick() {
        let rows = this.gridApi.getSelectedRows();
        if (rows.length == 0) {
            this.$message({
                message: "请选择要修改的行",
                type: 'error'
            });
            return;
        }
        let dialog = await this.$dialog.create({
            component: assignRoles,
            title: "权限分配",
            componentProps: { formData: rows[0] }
        })
        await dialog.present();
        let result = await dialog.onDidDismiss();
        if (result) {
            this.$message(result);
        }
        this.$refs.AgGrid.refresh();
    }
    async PasswordModificationClick() {
        let rows = this.gridApi.getSelectedRows();
        if (rows.length == 0) {
            this.$message({
                message: "请选择要修改的行",
                type: 'error',
                duration: 1000
            });
            return;
        }
        let dialog = await this.$dialog.create({
            component: password,
            title: "修改密码",
            componentProps: { formData: rows[0] },
        })
        await dialog.present();
        let result = await dialog.onDidDismiss();
        if (result) {
            this.$message(result);
        }
        this.$refs.AgGrid.refresh();
    }
    async enableClick() {
        let rows = this.gridApi.getSelectedRows();
        if (rows.length == 0) {
            this.$message({
                message: "请选择要修改的行",
                type: 'error',
                duration: 1000
            });
            return;
        }
        if (rows.length > 1) {
            this.$message({
                message: "只能选择一条数据！",
                type: 'error',
                duration: 1000
            });
            return;
        }
        await this.$confirm('确定启用吗?', '提示', {
            confirmButtonText: '确定',
            cancelButtonText: '取消',
            type: 'warning'
        }).then(async () => {
            let res = await this.$http({
                method: "post",
                url: "/api/services/SysMgrBussenssModule/SysUserInfo/Enable",
                data: rows[0]
            });

            if (res.data.code === "200") {
                this.$message({
                    message: "已启用",
                    type: "success"
                });
            } else {
                this.$message({
                    message: res.data.msg,
                    type: "error"
                });
            }
            this.$refs.AgGrid.refresh();
        });
    }

    async editClick() {
        let rows = this.gridApi.getSelectedRows();
        if (rows.length == 0) {
            this.$message({
                message: "请选择要修改的行",
                type: 'error'
            });
            return;
        }
        let dialog = await this.$dialog.create({
            component: edit,
            title: "编辑",
            componentProps: { formData: rows[0] },
        })
        await dialog.present();
        let result = await dialog.onDidDismiss();
        if (result) {
            this.$message(result);
            this.$refs.AgGrid.refresh();
        }
    }
    viewWillEnter() {
        this.gridOptions.columnDefs = [
            {
                headerName: "ID",
                field: "ID",
                hide: true
            },
            {
                headerName: "账号",
                field: "UserCode",
                filter: true,
                blType: 'text',
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
                field: "Memo"
            },
            {
                headerName: "角色",
                field: "Roles"
            }
        ];
    }
}