import { Controller, Component, asyncCompatible, Prop } from "interface"
import add from './add.bl'
import edit from './edit.bl'
import allotMenu from './allotMenu.bl'
import bindingUser from './bindingUser.bl';
import copyRole from './copyRole.bl'
@Component
export default class Index extends Controller {
    gridOptions = {
        paginationPageSize: 12,
        rowSelection: "multiple"
    }
    @Prop({ type: Object }) container;
    get gridHeight() {
        return (this.container.height - this.$getRef("header").outerHeight()) + "px";
    }
    gridApi = null;
    viewDidEnter() {
        this.gridApi = this.$refs.AgGrid.api();
    }
    async gridData(params) {
        let res = await this.$http({
            method: "post",
            url: "/api/services/SysMgrBussenssModule/SysRoleInfo/GetPageList",
            data: params
        });
        this.$refs.AgGrid.setData(res.data.content);
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
    @asyncCompatible()
    async deleteClick() {
        let rows = this.gridApi.getSelectedRows();
        if (rows.length == 0) {
            this.$message({
                message: "请选择要删除的行",
                type: 'error'
            });
            return;
        }
        let ids = [];
        rows.forEach(element => {
            ids.push(element.ID);
        });
        let params = { IDS: ids };
        let res = await this.$http({
            method: "post",
            url: "/api/services/SysMgrBussenssModule/SysRoleInfo/Delete",
            data: params
        });
        if (res.data.code === "200") {
            this.$message({
                message: "删除成功",
                type: "success"
            });
        }
        else {
            this.$message({
                message: res.data.msg,
                type: "error"
            });
        }
        this.$refs.AgGrid.refresh();
    }
    async allotClick() {
        let rows = this.gridApi.getSelectedRows();
        if (rows.length == 0) {
            this.$message({
                message: "请选择要分配菜单的数据",
                type: 'error'
            });
            return;
        }
        let dialog = await this.$dialog.create({
            component: allotMenu,
            title: "分配菜单",
            componentProps: { formData: rows[0] },
        })
        await dialog.present();
        let result = await dialog.onDidDismiss();
        if (result) {
            this.$message(result);
        }
        this.$refs.AgGrid.refresh();
    }
    async bindingClick() {
        let rows = this.gridApi.getSelectedRows();
        if (rows.length == 0) {
            this.$message({
                message: "请选择要分配菜单的数据",
                type: 'error'
            });
            return;
        }
        let dialog = await this.$dialog.create({
            component: bindingUser,
            title: "绑定用户",
            componentProps: { formData: rows[0] },
        })
        await dialog.present();
        let result = await dialog.onDidDismiss();
        if (result) {
            this.$message(result);
        }
        this.$refs.AgGrid.refresh();
    }

    async copyClick() {
        let rows = this.gridApi.getSelectedRows();
        if (rows.length == 0) {
            this.$message({
                message: "请选择要分配菜单的数据",
                type: 'error'
            });
            return;
        }
        let dialog = await this.$dialog.create({
            component: copyRole,
            title: "绑定用户",
            componentProps: { formData: rows[0] },
        })
        await dialog.present();

        let result = await dialog.onDidDismiss();
        if (result) {
            this.$message(result);
        }
        this.$refs.AgGrid.refresh();
    }
    viewWillEnter() {
        this.gridOptions.columnDefs = [
            {
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
                headerName: "备注",
                field: "Remark",
                blType: 'text',
                filter: true
            }
        ];
    }
}