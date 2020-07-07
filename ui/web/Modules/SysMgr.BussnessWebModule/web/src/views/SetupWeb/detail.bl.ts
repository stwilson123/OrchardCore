import { Controller, Component, Prop } from "interface"
@Component
export default class detail extends Controller {
    @Prop({ type: Object }) formData;
    constructor() {
        super();
    }
    gridApi = null;
    gridColumnApi = null;
    title = ""
    pageInfo = { page: { page: 1, pagesize: 50 } };
    StockCountType = ""
    IsConfirm = ""
    gridOptions = {
        paginationPageSize: 12,
        rowSelection: "multiple",
        defaultColDef: "ID",
    }
    viewModel = {
        SetupType: ''
    };

    async gridData(params) {
        let data = Object.assign({}, params, this.viewModel);
        let res = await this.$http({
            method: "post",
            url: "/api/services/SysMgrBussenssModule/Setup/GetPageList",
            data: data
        });
        this.$refs.detailGrid.setData(res.data.content);
    }

    viewWillEnter() {
        this.viewModel.SetupType = this.formData.SetupTypeNo
        this.gridOptions.columnDefs = [
            {
                headerName: "ID",
                field: "ID",
                hide: true
            },
            {
                headerName: "编码",
                field: "SetupNo",
                filter: true,
                editable: false,
                sortable: true,
                blType: 'text',
                width: 550
            },
            {
                headerName: "名称",
                field: "SetupContents",
                editable: false,
                sortable: true,
                blType: 'text',
                filter: true,
                width: 550
            },
            {
                headerName: "参数值",
                field: "SetupParameter",
                editable: false,
                sortable: true,
                blType: 'text',
                filter: true,
                width: 400
            }
        ]
    }
}