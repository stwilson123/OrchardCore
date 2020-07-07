import { Controller, Component, asyncCompatible } from "interface"
import edit from './edit.bl'
import add from './add.bl'
@Component
export default class Index extends Controller {
    constructor() {
        super();
    }
    gridOptions = {
        rowSelection: "multiple",
    }
    selectType = []
    gridApi = null
    gridColumnApi = null
    downloadhttp = "";
    downloadname = "";
    async gridData(params) {
        let res = await this.$http({
            method: "post",
            url: "/api/services/SysMgrBussenssModule/ConfigFiles/GetPageList",
            data: params
        });
        this.$refs.AgGrid.setData(res.data.content);
    }
    async viewWillEnter() {
        this.gridOptions.columnDefs = [
            {
                headerName: "ID",
                field: "Id",
                hide: true
            },
            {
                headerName: "类型",
                field: "FileType",
                checkboxSelection: true,
                filter: true,
                editable: false,
                blType: 'select',
                headerCheckboxSelection: true,
                blParams: {
                    optionsData: this.selectType,
                    clearable: true
                }
            },
            {
                headerName: "功能说明",
                field: "FileFunction",
                editable: false,
                sortable: true,
                blType: 'text',
                filter: true,

            },
            {
                headerName: "名称",
                field: "FileName",
                editable: false,
                sortable: true,
                blType: 'text',
                filter: true,

            },
            {
                headerName: "存放路径",
                field: "FilePath",
                editable: false,
                sortable: true,
                blType: 'text',
                filter: true,

            },
            {
                headerName: "上传时间",//"创建时间",
                field: "CreateDate",
                editable: true,
                filter: true,
                sortable: true,
                sort: "asc",
                blType: 'date',
                blParams: {
                    config: {
                        dateFmt: "yyyy-MM-dd HH:mm:ss"
                    }
                }
            },
            {
                headerName: "上传人员",
                field: "Creater",
                editable: false,
                sortable: true,
                blType: 'text',
                filter: true,

            }
        ]
        await this.getselectType();
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
            this.selectType.push({
                text: content[i].DicName, id: content[i].DicNo
            });
        }
    }
    viewDidEnter() {
        this.gridApi = this.$refs.AgGrid.api();
        this.gridColumnApi = this.$refs.AgGrid.columnApi();
    }
    async addClick() {
        var dialog = await this.$dialog.create({
            component: add,
            componentProps: {
                self: this
            },
            title: this.$l("add")
        });

        dialog.present();
        await dialog.onDidDismiss();

        //刷新数据   
        this.$refs.AgGrid.refresh();
    }
    async editClick() {
        let rows = this.$refs.AgGrid.gridApi.getSelectedRows();
        if (rows.length == 0) {
            this.$message.error("请选择需要编辑数据!");
            return;
        }
        if (rows.length > 1) {
            this.$message.error("只能选择一条数据进行编辑操作!");
            return;
        }

        let parms = { ID: rows[0].Id };
        var dialog = await this.$dialog.create({
            component: edit,
            componentProps: {
                formData: parms
            },
            title: this.$l("edit")
        });

        dialog.present();
        await dialog.onDidDismiss();

        //刷新数据   
        this.$refs.AgGrid.refresh();
    }
    @asyncCompatible()
    async deleteClick() {
        let rows = this.$refs.AgGrid.gridApi.getSelectedRows();
        if (rows.length == 0) {
            this.$message.error("请选择需要删除的数据!");
            return;
        }
        await this.$confirm('确定删除吗?', this.$l("tips"), {
            confirmButtonText: '确定',
            cancelButtonText: '取消',
            type: 'warning'
        });
        let IDS = rows.map(n => n.Id);
        let res = await this.$http({
            method: "post",
            url: "/api/services/SysMgrBussenssModule/ConfigFiles/Delete",
            data: { IDS: IDS }
        });

        if (res.data.code != "200") {

            this.$message.error(res.data.msg);
            return;
        }
        this.$message.success(res.data.content);
        this.$refs.AgGrid.refresh();
    }
    @asyncCompatible()
    async downloadClick() {
        var filepath;
        var filename;
        var rows = this.$refs.AgGrid.gridApi.getSelectedRows();
        let selectRowIds = rows.map(n => n.Id);
        if (selectRowIds.length === 0) {
            this.$message.error(this.$l("choose_data"));
            return false;
        }
        if (selectRowIds.length > 1) {
            this.$message.error(this.$l("choose_data"));
            return false;
        }
        filepath = rows[0].FilePath;
        filename = rows[0].FileName;
        this.downloadhttp = filepath;
        this.downloadname = filename;
        //window.location.href=this.downloadhttp;
        let link: HTMLElement = document.createElement('a');
        link.setAttribute("href", filepath);
        link.setAttribute("download", filename);
        link.style.visibility = 'hidden';
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    }
}