import { Controller, Component, asyncCompatible, Prop } from "interface"
import add from './add.bl'
import edit from './edit.bl'
import bind from './bind.bl'

@Component
export default class Index extends Controller {
    constructor() {
        super();
    }
    @Prop({ type: Object }) container;
    get mainHeight() {
        return this.container.height;
    }
    get gridHeight() {
        return this.container.height - this.$getRef("header").outerHeight();
    }
    gridOptions = {
        showPagination: false,
        floatingFilter: false
    }
    gridApi = null;
    gridColumnApi = null;
    ID = "";
    platForm = 1;
    draggable = false;
    treeData = [];
    treeLoading = false;
    filterParams = {};
    showAddButton = true;
    async gridData(params) {
        this.gridApi.showLoadingOverlay();
        this.filterParams = params;
        let objParams = Object.assign({}, this.filterParams, { ProgramParent: this.ID, Platform: this.platForm })
        let res = await this.$http({
            method: "post",
            url: "/api/services/SysMgrBussenssModule/SysProgram/GetPageList",
            data: objParams
        });
        this.$refs.AgGrid.setData(res.data.content);
    }
    async addClick() {
        let data = {
            ID: this.ID,
            platForm: this.platForm
        }
        let dialog = await this.$dialog.create({
            component: add,
            componentProps: { formData: data },
            title: "新增"
        })
        await dialog.present();
        let res = await dialog.onDidDismiss();
        if (res) {
            if (res.success) {
                this.$message(res.data);
                this.getMenus(this.platForm);
            }
        }
    }
    async editClick() {
        if (this.ID == '') {
            this.$message({ type: "error", message: "请选择需要编辑的数据" });
            return;
        }
        let data = {
            ID: this.ID
        }
        let dialog = await this.$dialog.create({
            component: edit,
            componentProps: { formData: data },
            title: "修改"
        })
        await dialog.present();
        let res = await dialog.onDidDismiss();
        if (res) {
            if (res.success) {
                this.$message(res.data);
                this.getMenus(this.platForm);
            }
        }
    }
    @asyncCompatible()
    async deleteClick() {
        if (this.ID == '') {
            this.$message({ type: "error", message: "请选择需要编辑的数据" });
            return;
        }
        await this.$confirm('确定需要删除吗？', '提示', {
            confirmButtonText: '确定',
            cancelButtonText: '取消',
            type: 'warning'
        }).then(async () => {
            let res = await this.$http({
                method: "post",
                url: "/api/services/SysMgrBussenssModule/Menu/Delete",
                data: { ID: this.ID }
            });
            if (res.data.content.success) {
                this.$message({ type: "success", message: res.data.content.msg });
                this.getMenus(this.platForm);
            }
            else {
                this.$message({ type: "error", message: res.data.content.msg });
            }
        });
    }
    async bindClick() {
        if (this.ID == '') {
            this.$message({ type: "error", message: "请选择需要绑定的数据" });
            return;
        }
        let data = {
            ProgramParent: this.ID,
            Platform: this.platForm
        }
        let dialog = await this.$dialog.create({
            component: bind,
            componentProps: { formData: data },
            title: "绑定菜单"
        })
        await dialog.present();
        let res = await dialog.onDidDismiss();
        if (res) {
            if (res.success) {
                this.$message(res.data);
                this.gridData(this.filterParams);
            }
        }
    }
    @asyncCompatible()
    pcMenuClick() {
        this.ID = '';
        this.platForm = 1;
        this.draggable = false;
        this.$refs.pcMenuButton.setDisabled(true);
        this.$refs.mobileMenuButton.setDisabled(false);
        this.$refs.allowDragButton.setDisabled(false);
        this.$refs.forbidDragButton.setDisabled(true);
        this.getMenus(this.platForm);
    }
    @asyncCompatible()
    mobileMenuClick() {
        this.ID = '';
        this.platForm = 2;
        this.draggable = false;
        this.$refs.pcMenuButton.setDisabled(false);
        this.$refs.mobileMenuButton.setDisabled(true);
        this.$refs.allowDragButton.setDisabled(false);
        this.$refs.forbidDragButton.setDisabled(true);
        this.getMenus(this.platForm);
    }
    @asyncCompatible()
    allowDropClick() {
        this.draggable = true;
        this.$refs.allowDragButton.setDisabled(true);
        this.$refs.forbidDragButton.setDisabled(false);
    }
    @asyncCompatible()
    cancelDropClick() {
        this.draggable = false;
        this.$refs.allowDragButton.setDisabled(false);
        this.$refs.forbidDragButton.setDisabled(true);
    }
    async treeClick(data, node, e) {
        this.ID = data.id;
        this.gridData(this.filterParams);
        let res = await this.$http({
            method: "post",
            url: "/api/services/SysMgrBussenssModule/Menu/GetOneById",
            data: { ID: this.ID }
        });
        let uId = res.data.content.UId;
        if (uId != null) {
            this.showAddButton = false;
        }
        else {
            this.showAddButton = true;
        }
    }
    async handleDrop(draggingNode, dropNode, dropType, e) {
        this.treeLoading = true;
        let moveType = '';
        if (dropType == "before") {
            moveType = "prev";
        }
        else if (dropType == "after") {
            moveType = "next";
        }
        else {
            moveType = "inner";
        }
        let param = {
            id: draggingNode.data.id,
            targetId: dropNode.data.id,
            moveType: moveType
        };
        let res = await this.$http({
            method: "post",
            url: "/api/services/SysMgrBussenssModule/Menu/DropSort",
            data: param
        });
        this.treeLoading = false;
    }
    allowDrop() {
        return true;
    }
    allowDrag() {
        return true;
    }
    async getMenus(p) {
        this.treeLoading = true;
        let menuParams = Object.assign({}, { platform: p, ID: "" })
        let res = await this.$http({
            method: "post",
            url: "/api/services/SysMgrBussenssModule/Menu/GetPageList",
            data: menuParams
        });
        let originData = res.data.content;
        let rootData = originData.filter(n => n.pId == null);
        let newData = [];
        for (let item of rootData) {
            let children = originData.filter(n => n.pId == item.id);
            let obj = {
                id: item.id,
                label: item.name,
                children: []
            }
            for (let child of children) {
                obj.children.push({
                    id: child.id,
                    label: child.name
                });
            }
            newData.push(obj);
        }
        this.treeData = newData;
        this.treeLoading = false;
    }
    async viewWillEnter() {
        this.gridOptions.columnDefs = [
            {
                headerName: "ID",
                field: "ID",
                hide: true
            },
            {
                headerName: "名称",
                field: "Name",
                blType: 'text'
            },
            {
                headerName: "编码",
                field: "Code",
                blType: 'text'
            },
            {
                headerName: "图标",
                field: "Icon",
                blType: 'text'
            },
            {
                headerName: "备注",
                field: "Desc",
                blType: 'text'
            },
            {
                headerName: "排序号",
                field: "Sort",
                blType: 'text'
            }
        ];
        await this.getMenus(this.platForm);
    }
    viewDidEnter() {
        this.gridApi = this.$refs.AgGrid.api();
        this.gridColumnApi = this.$refs.AgGrid.columnApi();
    }
}