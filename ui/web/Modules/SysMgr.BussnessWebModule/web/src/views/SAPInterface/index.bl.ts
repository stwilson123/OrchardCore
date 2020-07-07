import { Controller, Component, asyncCompatible, Prop } from "interface"
import edit from './edit.bl'
@Component
export default class Index extends Controller {
  constructor() {
    super();
  }
  @Prop({ type: Object }) container;
  gridOptions = {
    rowSelection: "multiple"
  }
  get gridHeight() {
    return (this.container.height - this.$getRef("header").outerHeight()) + "px";
  }
  pageInfo = {
    page: {
    }
  }
  gridApi = null;
  transferStatusData = [
    { id: 0, text: "待传送" },
    { id: 1, text: "已传送" }
  ];
  againCallData = [
    { id: 0, text: "未执行成功" },
    { id: 1, text: "执行成功" }
  ];
  async gridData(params) {
    this.pageInfo = params;
    let data = {
      op: {
        controller: "SAPInterfaceService.svc",
        action: 'GetViewData'
      },
      ViewPager: this.pageInfo.page
    }
    let res = await this.$http({
      method: "post",
      url: "/api/services/MESFoundationWebModule/SAPInterface/Apply",
      data
    });
    this.$refs.AgGrid.setData(res.data.content);
  }
  async resetClick() {
    let rows = this.gridApi.getSelectedRows();
    if (rows.length == 0) {
      this.$message({
        message: "请选择需要重新调用的接口",
        type: 'error'
      });
      return;
    }
    if (rows.length > 1) {
      this.$message({
        message: "只能选择一条数据",
        type: 'error'
      });
      return;
    }
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
    if (rows.length > 1) {
      this.$message({
        message: "只能选择一行编辑",
        type: 'error'
      });
      return;
    }
    let data = Object.assign({}, rows[0]);
    let dialog = await this.$dialog.create({
      component: edit,
      componentProps: { formData: { id: data.id } },
      title: "请编辑回写SAP的相关信息"
    })
    await dialog.present();
    let res = await dialog.onDidDismiss();
    if (res) {
      this.$message(res);
      this.$refs.AgGrid.refresh();
    }
  }
  viewWillEnter() {
    this.gridOptions.columnDefs = [
      {
        headerName: "id",
        field: "id",
        hide: true
      },
      {
        headerName: "地址",
        field: "url",
        checkboxSelection: true,
        headerCheckboxSelection: true
      },
      {
        headerName: "方法",
        field: "method",
        filter: true,
        blType: 'text',
        fieldToServer: "METHOD"
      },
      {
        headerName: "模块名",
        field: "modleCode",
        filter: true,
        blType: 'text',
        fieldToServer: "MODLE_CODE"
      },
      {
        headerName: "创建时间",
        field: "createDate",
        filter: true,
        blType: 'date',
        fieldToServer: "CREATEDATE"
      },
      {
        headerName: "最新调用时间",
        field: "updateDate",
        filter: true,
        blType: 'date',
        fieldToServer: "UPDATEDATE"
      },
      {
        headerName: "传送状态",
        field: "transferStatusString",
        filter: true,
        blType: 'select',
        fieldToServer: "TRANSFER_STATUS",
        blFilterParams: {
          optionsData: this.transferStatusData,
          clearable: true
        }
      },
      {
        headerName: "SAP回写字段",
        field: "inputparam"
      },
      {
        headerName: "MES凭证号",
        field: "mesDoc",
        filter: true,
        blType: 'text',
        fieldToServer: "MESDOC"
      },
      {
        headerName: "是否调用成功",
        field: "againCallString",
        filter: true,
        blType: 'select',
        fieldToServer: "AGAINCALL",
        blFilterParams: {
          optionsData: this.againCallData,
          clearable: true
        }
      },
      {
        headerName: "SAPDOC2",
        field: "SAPDoc2"
      }
    ];
  }
  viewDidEnter() {
    this.gridApi = this.$refs.AgGrid.api();
  }
}