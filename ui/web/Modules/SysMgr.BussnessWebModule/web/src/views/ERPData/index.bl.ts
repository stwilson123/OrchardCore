import { Controller, Component, asyncCompatible, Prop } from "interface"
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
    return this.container.height + "px";
  }
  pageInfo = {
    page: {
    }
  }
  async gridData(params) {
    this.pageInfo = params;
    let data = {
      op: {
        controller: "ERPDATAService.svc",
        action: 'GetInfoByPage'
      },
      MaintainPager: this.pageInfo.page
    }
    let res = await this.$http({
      method: "post",
      url: "/api/services/MESFoundationWebModule/ERPData/Apply",
      data
    });
    this.$refs.AgGrid.setData(res.data.content);
  }
  viewWillEnter() {
    this.gridOptions.columnDefs = [
      {
        headerName: "ID",
        field: "ID",
        hide: true
      },
      {
        headerName: "方法名",
        field: "Method",
        checkboxSelection: true,
        headerCheckboxSelection: true,
        filter: true,
        blType: 'text',
        fieldToServer: "METHOD"
      },
      {
        headerName: "输入参数",
        field: "InPutparam"
      },
      {
        headerName: "输出参数",
        field: "OutPutparam"
      },
      {
        headerName: "模块代码",
        field: "ModleCode",
        filter: true,
        blType: 'text',
        fieldToServer: "MODLE_CODE"
      },
      {
        headerName: "调用时间",
        field: "CreateDate",
        filter: true,
        blType: 'date',
        fieldToServer: "CREATEDATE"
      },
      {
        headerName: "MES凭证号",
        field: "MesDocNo",
        filter: true,
        blType: 'text',
        fieldToServer: "MESDOCNO"
      },
      {
        headerName: "SAP凭证号",
        field: "SapDocNo",
        filter: true,
        blType: 'text',
        fieldToServer: "SAPDOCNO"
      },
      {
        headerName: "SAP凭证号2",
        field: "SapDocNo2"
      }
    ];
  }
}