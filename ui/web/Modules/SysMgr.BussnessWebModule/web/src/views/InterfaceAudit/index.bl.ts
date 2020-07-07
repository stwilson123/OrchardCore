import { Controller, Component, asyncCompatible } from "interface"
import detail from './detail.bl'

@Component
export default class Index extends Controller {
    constructor() {
        super();
    }
    gridOptions = {
        paginationPageSize: 20
    }
    gridApi = null;
    gridColumnApi = null;
    async gridData(params) {
        let res = await this.$http({
            method: "post",
            url: "/api/services/SysMgrBussenssModule/InterfaceAudit/GetPageList",
            data: params
        });
        this.$refs.AgGrid.setData(res.data.content);
    }
    viewWillEnter() {
        this.gridOptions.columnDefs = [
            {
                headerName: "Id",
                field: "Id",
                hide: true
            },
            {
                headerName: "返回值",
                field: "OutParameters",
                blType: 'text',
                width: 300
            },
            {
                headerName: "方法描述",
                field: "MethodDescription",
                filter: true,
                blType: 'text'
            },
            {
                headerName: "请求参数",
                field: "Parameters",
                blType: 'text',
                width: 300
            },
            {
                headerName: "创建人",
                field: "UserAccount",
                filter: true,
                blType: 'text'
            },
            {
                headerName: "执行时间",
                field: "ExecutionTime",
                filter: true,
                blType: 'date',
                blParams: {
                    config: {
                        dateFmt: "yyyy-MM-dd HH:mm:ss"
                    }
                },
                sort: "desc",
                sortable: true
            },
            {
                headerName: "异常信息",
                field: "SysException",
                blType: 'text'
            }
        ];
    }
    async rowClicked(e) {
        let dialog = await this.$dialog.create({
            component: detail,
            componentProps: { paramsData: e.data },
            title: "详情"
        })
        await dialog.present();
        await dialog.onDidDismiss();
    }
    viewDidEnter() {
        this.gridApi = this.$refs.AgGrid.api();
        this.gridColumnApi = this.$refs.AgGrid.columnApi();
    }
}