import { Controller, Component, Prop } from "interface"

@Component
export default class Index1 extends Controller {
    @Prop({ type: Object })
    container;
    gridOptions = {
        //paginationPageSize: 11
    }
    selectData = [
        { id: "1", text: "张三" },
        { id: "2", text: "李四" },
        { id: "3", text: "王五" }
    ]
    gridApi = null
    gridColumnApi = null

    constructor() {
        super();
    }

    get gridHeight() {
        return (this.container.height - 106) + "px";
    }

    async gridData(params) {
        let res = await this.$http({
            method: "post",
            url: "/api/services/BussnessWebModule/test/getclientdata",
            data: params
        });
        this.$refs.TheGrid.setData(res.data.content)
    }
    temp1() {
        this.$router.push({ path: "/layout/BussnessWebModule/MasterData/Index" })
    }
    viewWillEnter() {
        this.gridOptions.columnDefs = [
            {
                headerName: "Id",
                field: "Id",
                checkboxSelection: true,
                resizable: true
            },
            {
                headerName: "姓名",
                field: "name",
                filter: true,
                editable: true,
                blType: 'text'
            },
            {
                headerName: "年龄",
                field: "age",
                editable: true,
                sortable: true,
                blType: 'int',
                filter: true,
                blParams: {
                    rules: ["required", "numeric"]
                }
            },
            {
                headerName: "日期",
                field: "theDate",
                editable: true,
                filter: true,
                blType: 'date',
                blParams: {
                    config: {
                        dateFmt: "yyyy-MM-dd"
                    },
                    rules: ["required"]
                },
                blFilterParams: {
                    config: {
                        dateFmt: "yyyy-MM-dd"
                    }
                },
                width: 200
            },
            {
                headerName: "选择框",
                field: "theSelect",
                editable: true,
                blType: 'select',
                filter: true,
                //blDataType: "int",
                //blFilterType: "string",
                blParams: {
                    optionsData: this.selectData,
                    clearable: true
                }
            }
        ];
    }
    viewDidEnter() {
        this.gridApi = this.$refs.TheGrid.api();
        this.gridColumnApi = this.$refs.TheGrid.columnApi();
    }
}