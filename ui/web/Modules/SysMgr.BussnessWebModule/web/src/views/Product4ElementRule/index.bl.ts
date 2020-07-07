import { Controller, Component, asyncCompatible } from "interface"
import codingruleselection from './codingruleselection.bl'
@Component
export default class Index extends Controller {
  constructor() {
    super();
  }
  gridApi = null;
  gridColumnApi = null;

  title = ""
  formData = {};
  StockCountType = ""
  IsConfirm = ""
  gridOptions = {
    rowSelection: "single",
    defaultColDef: "ID",
  }
  async gridData(params) {
    let data = Object.assign({ SetupType: 'SYS_BUSINESS' }, params);
    let res = await this.$http({
      method: "post",
      url: "/api/services/SysMgrBussenssModule/Setup/GetPageList",
      data
    });
    this.$refs.IndexGrid.setData(res.data.content);
  }

  //列表数据
  viewWillEnter() {
    this.gridOptions.columnDefs = [
      {
        headerName: "ID",
        field: "ID",
        hide: true
      },
      {
        headerName: "业务编码",
        field: "SetupNo",

        filter: true,
        editable: false,
        sortable: true,
        blType: 'text',
        width: 400,
      },
      {
        headerName: "业务名称",
        field: "SetupContents",
        editable: false,
        sortable: true,
        blType: 'text',
        filter: true,
        width: 400,
      },
      {
        headerName: "业务描述",
        field: "SetupParameter",
        editable: false,
        sortable: true,
        blType: 'text',
        filter: true,
        width: 400,
      }
    ]
  }
  get rowData() {
    return this.$refs.IndexGrid.getRowData();
  }
  //编码规则选择
  async addClick() {
    let gridData = this.$refs.IndexGrid.gridApi.getSelectedRows();
    if (gridData.length == 0) {
      this.$message.warning("请先选择一个业务功能点，并维护其可用的编码规则！");
      return;
    }
    if (gridData.length > 1) {

      this.$message.warning("只能选择一个业务功能点！");
      return;
    }
    var dialog = await this.$dialog.create({
      component: codingruleselection,
      componentProps: {
        formData: {
          ID: gridData[0].ID,
        }
      },
      title: "编码规则选择",
      customClass: "blocks-web-dialog",
      modal: false
    });
    dialog.present();
    await dialog.onDidDismiss();
    //刷新数据   
    this.$refs.IndexGrid.refresh();
  }
}