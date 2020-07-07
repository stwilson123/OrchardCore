import { Controller, Component, asyncCompatible } from "interface"
import edit from './edit.bl'
import add from './add.bl'
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
  gridApi = null;
  gridColumnApi = null;
  async gridData(params) {
    let res = await this.$http({
      method: "post",
      url: "/api/services/SysMgrBussenssModule/ThirdSystemType/GetPageList",
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
        sort: "asc",
        blType: 'text',
        headerCheckboxSelection: true
      },
      {
        headerName: "系统名称",
        field: "SystemName",
        editable: false,
        sortable: true,
        blType: 'text',
        filter: true,
      }
    ]
  }

  viewDidEnter() {
    this.gridApi = this.$refs.AgGrid.api();
    this.gridColumnApi = this.$refs.AgGrid.columnApi();
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

    let parms = { ID: rows[0].ID };
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
  @asyncCompatible()
  async deleteClick() {
    let rows = this.$refs.AgGrid.gridApi.getSelectedRows();
    if (rows.length == 0) {
      this.$message.warning(this.$l("choose_data"));
      return;
    }
    let IDS = rows.map(n => n.ID);
    await this.$confirm(this.$l('confirm'), this.$l("tips"), {
      cancelButtonText: this.$l('cancel'),
      confirmButtonText: this.$l('CONFIRM0'),
      type: 'warning'
    }).then(async () => {
      let res = await this.$http({
        method: "post",
        url: "/api/services/SysMgrBussenssModule/ThirdSystemType/Delete",
        data: { IDs: IDS }
      });
      if (res.data.code != "200") {
        this.$message.error(res.data.msg);
        return;
      }
      this.$message.success(res.data.content);
      this.$refs.AgGrid.refresh();
    });
  }
}