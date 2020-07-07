import { Controller, Component, asyncCompatible, Prop } from "interface"
import add from './add.bl'
import edit from './edit.bl'

@Component
export default class Index extends Controller {
    constructor() {
        super();
    }
    @Prop({ type: Object }) container;
    get gridHeight() {
        return (this.container.height - this.$getRef("header").outerHeight()) / 2 + "px";
    }
    topGridOptions = {
    }
    bottomGridOptions = {
        floatingFilter: false
    };
    topgridApi = null;
    bottomgridApi = null;
    LanguageId = "";
    pageInfo = {};
    async topGridData(params) {
        let res = await this.$http({
            method: "post",
            url: "/api/services/SysMgrBussenssModule/Languages/GetPageList",
            data: params
        });
        this.$refs.TopGrid.setData(res.data.content);
    }
    async bottomGridData(params = {}) {
        this.bottomgridApi.showLoadingOverlay();
        if (JSON.stringify(params) == "{}") {
            this.pageInfo = {
                page: {
                    page: 1,
                    pageSize: 10
                }
            }
        }
        else {
            this.pageInfo = params;
        }
        let data = Object.assign({}, this.pageInfo, { LanguageId: this.LanguageId });
        let res = await this.$http({
            method: "post",
            url: "/api/services/SysMgrBussenssModule/Languages/GetDetailPageList",
            data
        });
        this.$refs.BottomGrid.setData(res.data.content);
    }
    async addClick() {
        let dialog = await this.$dialog.create({
            component: add,
            componentProps: {},
            title: this.$l("add")
        })
        await dialog.present();
        let res = await dialog.onDidDismiss();
        if (res) {
            this.$message(res);
            this.$refs.TopGrid.refresh();
        }
    }
    async editClick() {
        let rows = this.topgridApi.getSelectedRows();
        if (rows.length == 0) {
            this.$message({
                message: "请选择需要编辑数据",
                type: 'error'
            });
            return;
        }
        let data = {
            ID: rows[0].Id
        }
        let dialog = await this.$dialog.create({
            component: edit,
            componentProps: { formData: data },
            title: this.$l("edit")
        })
        await dialog.present();
        let res = await dialog.onDidDismiss();
        if (res) {
            this.$message(res);
            this.$refs.TopGrid.refresh();
        }
    }
    @asyncCompatible()
    async deleteClick() {
        let rows = this.topgridApi.getSelectedRows();
        if (rows.length == 0) {
            this.$message({
                message: "请选择需要删除的数据",
                type: 'error'
            });
            return;
        }
        let data = {
            IDS: [rows[0].Id]
        }
        await this.$confirm('确定需要删除吗？', '提示', {
            confirmButtonText: '确定',
            cancelButtonText: '取消',
            type: 'warning'
        }).then(async () => {
            let res = await this.$http({
                method: "post",
                url: "/api/services/SysMgrBussenssModule/Languages/Delete",
                data
            });
            this.$message({ message: res.data.content });
            this.$refs.TopGrid.refresh();
        });
    }
    viewWillEnter() {
        this.topGridOptions.columnDefs = [
            {
                headerName: "Id",
                field: "Id",
                hide: true
            },
            {
                headerName: "语言编码",
                field: "LanguageCode",
                blType: 'text',
                filter: true,
                checkboxSelection: true,
                sort: "desc",
                sortable: true
            },
            {
                headerName: "语言名称",
                field: "LanguageName",
                filter: true,
                blType: 'text'
            },
            {
                headerName: "语言图标",
                field: "LanguageIcon",
                blType: 'text'
            }
        ];
        this.bottomGridOptions.columnDefs = [
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
                blType: 'text'
            },
            {
                headerName: "使用模块",
                field: "LanguageModule",
                blType: 'text',
                hide: true
            },
            {
                headerName: "翻译编码",
                field: "LanguageKey",
                blType: 'text'
            },
            {
                headerName: "翻译内容",
                field: "LanguageValue",
                blType: 'text'
            }
        ];
    }
    async rowClicked(e) {
        this.LanguageId = e.data.Id;
        this.bottomGridData();
    }
    viewDidEnter() {
        this.topgridApi = this.$refs.TopGrid.api();
        this.bottomgridApi = this.$refs.BottomGrid.api();
    }
}