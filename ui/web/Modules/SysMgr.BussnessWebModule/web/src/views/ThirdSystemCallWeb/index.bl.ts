import { Controller, Component, asyncCompatible } from "interface"
import edit from './edit.bl'
@Component
export default class Index extends Controller {
  constructor() {
    super();
  }
  gridOptions = {
    paginationPageSize: 12,
    rowSelection: "multiple"
  }
  pageInfo = { page: { page: 1, pagesize: 50 } };
  gridApi = null
  gridColumnApi = null
  selectResult = [
    { id: "0", text: "新建" },
    { id: "1", text: "请求中" },
    { id: "2", text: "成功" },
    { id: "3", text: "失败" }
  ]
  async gridData(params) {
    let res = await this.$http({
      method: "post",
      url: "/api/services/SysMgrBussenssModule/ThirdSystemCall/GetPageList",
      data: params
    });
    this.$refs.AgGrid.setData(res.data.content);
  }
  async viewWillEnter() {
    this.gridOptions.columnDefs = [
      {
        headerName: "ID",
        field: "ID",
        hide: true
      },
      {
        headerName: "系统编号",
        field: "SystemNo",
        checkboxSelection: true,
        filter: true,
        editable: false,
        sortable: true,
        blType: 'text'
      },
      {
        headerName: "系统名称",
        field: "SystemName",
        editable: false,
        sortable: true,
        blType: 'text',
        filter: true,

      },
      {
        headerName: "方法名称",
        field: "FunctionName",
        editable: false,
        sortable: true,
        blType: 'text',
        filter: true,

      },
      {
        headerName: "开始处理时间",
        field: "ProcessTimeBegin",

        filter: true,
        sortable: true,
        blType: 'date',
        blParams: {
          config: {
            dateFmt: "yyyy-MM-dd HH:mm:ss"
          }
        },
        blFilterParams: {
          config: {
            dateFmt: "yyyy-MM-dd"
          }
        }
      },
      {
        headerName: "结束处理时间",
        field: "ProcessTimeEnd",
        filter: true,
        sortable: true,
        blType: 'date',
        blParams: {
          config: {
            dateFmt: "yyyy-MM-dd HH:mm:ss"
          }
        },
        blFilterParams: {
          config: {
            dateFmt: "yyyy-MM-dd"
          }
        }
      },
      {
        headerName: "请求次数",
        field: "RequestTimes",
        editable: false,
        sortable: true,
        blType: 'int',
        filter: true,
      },
      {
        headerName: "处理结果",
        field: "ProcessResult",
        editable: false,
        sortable: true,
        blType: 'select',
        filter: true,
        blParams: {
          optionsData: this.selectResult,
          clearable: true
        }
      },
      {
        headerName: "传入参数",
        field: "ParameterIn",
        editable: false,
        sortable: false,
        blType: 'text',
        filter: true,
      },
      {
        headerName: "返回值",
        field: "ResponseValue",
        editable: false,
        sortable: true,
        blType: 'text',
        filter: true
      },
      {
        headerName: "异常",
        field: "ExceptionMsg",
        editable: false,
        sortable: true,
        blType: 'text',
        filter: true
      }
    ]
  }

  viewDidEnter() {
    this.gridApi = this.$refs.AgGrid.api();
    this.gridColumnApi = this.$refs.AgGrid.columnApi();
  }

  async reCallClick() {
    let rows = this.$refs.AgGrid.gridApi.getSelectedRows();
    if (rows.length == 0) {
      this.$message.error("请选择需要重传的数据!");
      return;
    }
    if (rows[0].ProcessResult != 3) {
      this.$message.error("请选择处理结果为失败的数据执行重传操作!");
      return;
    }
    await this.$confirm('确定重传吗?', this.$l("tips"), {
      confirmButtonText: '确定',
      cancelButtonText: '取消',
      type: 'warning'
    });
    let IDS = rows.map(n => n.ID)
    let res = await this.$http({
      method: "post",
      url: "/api/services/SysMgrBussenssModule/ThirdSystemCall/ReCall",
      data: { IDS: IDS }
    });

    if (res.data.code != "200") {

      this.$message.error(res.data.msg);
      return;
    }
    this.$message.success(res.data.content);
    const data = { page: { page: 1, pagesize: 50 } }
    await this.gridData(data);
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

    let parms = { ID: rows[0].ID, ProcessResult: rows[0].ProcessResult };
    var dialog = await this.$dialog.create({
      component: edit,
      componentProps: {
        formData: parms
      },
      title: this.$l("edit"),
      customClass: "blocks-web-dialog",
      modal: false
    });
    dialog.present();
    await dialog.onDidDismiss();
    this.$refs.AgGrid.refresh();
  }
}