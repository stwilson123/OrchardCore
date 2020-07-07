import { Controller, Component, asyncCompatible } from "interface"
import add from './add.bl'
import edit from './edit.bl'
import detail from './detail.bl'
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
    rowSelection: "multiple",
    defaultColDef: "ID",
  }
  //列表
  async gridData(params) {
    let res = await this.$http({
      method: "post",
      url: "/api/services/SysMgrBussenssModule/ProductFormat/GetPageList",
      data: params
    });
    this.$refs.IndexGrid.setData(res.data.content);
  }
  //新增
  async addClick() {

    var dialog = await this.$dialog.create({
      component: add,
      componentProps: {
        self: this
      },
      title: this.$l("add"), //"新增"
      customClass: "blocks-web-dialog",
      modal: false
    });
    dialog.present();
    await dialog.onDidDismiss();
    //刷新数据
    this.$refs.IndexGrid.refresh();
  }
  async detailClick() {
    let rows = this.$refs.IndexGrid.gridApi.getSelectedRows();
    if (rows.length == 0) {
      this.$message.error(this.$l("choose_data"));
      return;
    }
    if (rows.length > 1) {
      this.$message.error(this.$l("choose_one"));
      return;
    }
    var dialog = await this.$dialog.create({
      component: detail,
      componentProps: {
        formData: {
          ProductFormatID: rows[0].ID
        }
      },
      title: this.$l("DETAIL"),//"盘库详情",
      customClass: "blocks-web-dialog",
      modal: false
    });
    dialog.present();
    await dialog.onDidDismiss();
  }

  //编辑
  async editClick() {
    let rows = this.$refs.IndexGrid.gridApi.getSelectedRows();
    if (rows.length == 0) {
      this.$message.error(this.$l("choose_data"));
      return;
    }
    if (rows.length > 1) {
      this.$message.error(this.$l("choose_one"));
      return;
    }

    let parms = { ID: rows[0].ID };
    var dialog = await this.$dialog.create({
      //cssClass: "blocks-dialog-4",
      component: edit,
      componentProps: {
        formData: parms
      },
      title: this.$l("edit"),//"编辑",
      customClass: "blocks-web-dialog",
      modal: false
    });

    dialog.present();
    await dialog.onDidDismiss();
    //刷新数据   
    this.$refs.IndexGrid.refresh();
  }


  @asyncCompatible()
  async deleteClick() {
    let rows = this.$refs.IndexGrid.gridApi.getSelectedRows();
    if (rows.length == 0) {
      this.$message.error(this.$l("choose_data"));
      return;
    }
    let IDS = rows.map(n => n.ID)
    await this.$confirm('确定删除吗?', this.$l("tips"), {
      confirmButtonText: '确定',
      cancelButtonText: '取消',
      type: 'warning'
    });

    let res = await this.$http({
      method: "post",
      url: "/api/services/SysMgrBussenssModule/ProductFormat/Delete",
      data: { IDS: IDS }
    });
    if (res.data.code != "200") {
      this.$message.error(res.data.msg);
      return;
    }

    this.$message.success(res.data.content);
    this.$refs.IndexGrid.refresh();
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
        headerName: "编码",
        field: "Code",
        checkboxSelection: true,
        filter: true,
        editable: false,
        sortable: true,
        blType: 'text',
        width: 340,
      },
      {
        headerName: "名称",
        field: "Name",
        editable: false,
        sortable: true,
        blType: 'text',
        filter: true,
        width: 340,
      },
      {
        headerName: "位数",
        field: "Length",
        editable: false,
        sortable: true,
        blType: 'float',
        filter: true,
        width: 120
      },
      {
        headerName: "描述",
        field: "Description",
        editable: false,
        sortable: true,
        blType: 'text',
        filter: true,
        width: 340,
      }
    ]
  }
  get rowData() {
    return this.$refs.IndexGrid.getRowData();
  }
}