import { Controller, Component, asyncCompatible } from "interface"
import add from './add.bl'
import edit from './edit.bl'
@Component
export default class Index extends Controller {
  constructor() {
    super();
  }
  gridApi = null;
  gridColumnApi = null;
  selectType = [
    { id: "0", text: "是" },
    { id: "1", text: "否" }
  ]
  title = ""
  formData = {};
  StockCountType = ""
  IsConfirm = ""
  gridOptions = {
    defaultColDef: "ID"
  }
  async gridData(params) {
    let res = await this.$http({
      method: "post",
      url: "/api/services/SysMgrBussenssModule/ProductElement/GetPageList",
      data: params
    });
    this.$refs.IndexGrid.setData(res.data.content);
  }

  async addClick() {
    var dialog = await this.$dialog.create({
      component: add,
      componentProps: {
        self: this
      },
      title: this.$l("add"),
      customClass: "blocks-web-dialog",
      modal: false
    });
    dialog.present();
    await dialog.onDidDismiss();
    this.$refs.IndexGrid.refresh();
  }
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
      title: this.$l("edit"),
      customClass: "blocks-web-dialog",
      modal: false
    });
    dialog.present();
    await dialog.onDidDismiss();
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
      url: "/api/services/SysMgrBussenssModule/ProductElement/Delete",
      data: { IDS: IDS }
    });

    if (res.data.code != "200") {
      this.$message.error(res.data.msg);
      return;
    }

    this.$message.success(res.data.content);
    this.$refs.IndexGrid.refresh();
  }
  viewWillEnter() {
    this.gridOptions.columnDefs = [
      {
        headerName: "ID",
        field: "ID",
        hide: true
      },
      {
        headerName: "编码",
        field: "ProductElementCode",
        checkboxSelection: true,
        filter: true,
        editable: false,
        sortable: true,
        blType: 'text',
        width: 340
      },
      {
        headerName: "名称",
        field: "ProductElementName",
        editable: false,
        sortable: true,
        blType: 'text',
        filter: true,
        width: 340
      },
      {
        headerName: "类型",
        field: "ProductElementTypeName",
        editable: false,
        sortable: true,
        blType: 'text',
        filter: true,
        width: 340
      },
      {
        headerName: "位数",
        field: "ProductElementLength",
        editable: false,
        sortable: true,
        blType: 'float',
        filter: true,
        width: 120
      },
      {
        headerName: "描述",
        field: "ProductElementDescription",
        editable: false,
        sortable: true,
        blType: 'text',
        filter: true,
        width: 340
      }
    ]
  }
  get rowData() {
    return this.$refs.IndexGrid.getRowData();
  }
}