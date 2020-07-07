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
      url: "/api/services/SysMgrBussenssModule/Setup/GetSetupTypePageList",
      data: params
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
        headerName: "类型编码",
        field: "SetupTypeNo",
        checkboxSelection: true,
        filter: true,
        editable: false,
        sortable: true,
        blType: 'text',
        width: 550,
      },
      {
        headerName: "类型名称",
        field: "SetupTypeName",
        editable: false,
        sortable: true,
        blType: 'text',
        filter: true,
        width: 550,
      },
      {
        headerName: "类型值",
        field: "SetupTypeValue",
        editable: false,
        sortable: true,
        blType: 'text',
        filter: true,
        width: 400,
      }
    ]
  }
  async detailClick() {
    let rows = this.$refs.IndexGrid.gridApi.getSelectedRows();
    if (rows.length == 0) {
      this.$message.error("请选择需要查看的数据!");
      return;
    }
    if (rows.length > 1) {
      this.$message.error("只能选择一条数据进行操作!");
      return;
    }

    let parms = { SetupTypeNo: rows[0].SetupTypeNo };
    var dialog = await this.$dialog.create({
      component: detail,
      componentProps: {
        formData: parms
      },
      title: rows[0].SetupTypeName
    });

    dialog.present();
    await dialog.onDidDismiss();

    //刷新数据   
    this.$refs.IndexGrid.refresh();
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
    this.$refs.IndexGrid.refresh();
  }
  async editClick() {
    let rows = this.$refs.IndexGrid.gridApi.getSelectedRows();
    if (rows.length == 0) {
      this.$message.error("请选择需要编辑数据!");
      return;
    }
    if (rows.length > 1) {
      this.$message.error("只能选择一条数据进行编辑操作!");
      return;
    }

    let parms = { ID: rows[0].ID };
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
    this.$refs.IndexGrid.refresh();
  }
  @asyncCompatible()
  async deleteClick() {
    let rows = this.$refs.IndexGrid.gridApi.getSelectedRows();
    if (rows.length == 0) {
      this.$message.error("请选择需要删除的数据!");
      return;
    }
    await this.$confirm('删除此类型编码，会同步删除类型编码下的明细数据，确定需要删除吗？', this.$l("tips"), {
      confirmButtonText: '确定',
      cancelButtonText: '取消',
      type: 'warning'
    });
    let IDS = rows.map(n => n.ID)
    let res = await this.$http({
      method: "post",
      url: "/api/services/SysMgrBussenssModule/Setup/DeleteSetupTypeById",
      data: { IDS: IDS }
    });
    if (res.data.code != "200") {
      this.$message.error(res.data.msg);
      return;
    }
    this.$message.success(res.data.content);
    this.$refs.IndexGrid.refresh();
  }
}