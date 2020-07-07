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
  gridApi = null
  gridColumnApi = null
  selectType = [
    { id: "A", text: "正式员工" },
    { id: "T", text: "临时员工" },
    { id: "Q", text: "离职员工" }
  ]
  async gridData(params) {
    let res = await this.$http({
      method: "post",
      url: "/api/services/SysMgrBussenssModule/Employee/GetPageList",
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
        headerName: "编码",
        field: "Code",
        checkboxSelection: true,
        filter: true,
        editable: false,
        sortable: true,
        sort: "asc",
        blType: 'text',
        headerCheckboxSelection: true

      },
      {
        headerName: "名称",
        field: "Name",
        editable: false,
        sortable: true,
        blType: 'text',
        filter: true,
      },
      {
        headerName: "所属部门",
        field: "DeptName",
        editable: false,
        sortable: true,
        blType: 'text',
        filter: true,
      },
      {
        headerName: "类型",
        field: "EmpType",
        editable: false,
        sortable: true,
        blType: 'select',
        filter: true,
        blParams: {
          optionsData: this.selectType,
          clearable: true
        }
      },
      {
        headerName: "email",
        field: "Email",
        editable: false,
        sortable: true,
        blType: 'text',
        filter: true,
      },
      {
        headerName: "电话",
        field: "Phone",
        editable: false,
        sortable: true,
        blType: 'text',
        filter: true,
      },
      {
        headerName: "描述",
        field: "Desc",
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
      title: this.$l("add"), //"新增"
      customClass: "blocks-web-dialog",
      modal: false
    });

    dialog.present();
    await dialog.onDidDismiss();

    //刷新数据   
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

    //刷新数据   
    this.$refs.AgGrid.refresh();
  }
  @asyncCompatible()
  async deleteClick() {
    let rows = this.$refs.AgGrid.gridApi.getSelectedRows();
    if (rows.length == 0) {
      this.$message.error("请选择需要删除的数据!");
      return;
    }
    await this.$confirm('确定删除吗?', this.$l("tips"), {
      confirmButtonText: '确定',
      cancelButtonText: '取消',
      type: 'warning'
    });
    let IDS = rows.map(n => n.ID)
    let res = await this.$http({
      method: "post",
      url: "/api/services/SysMgrBussenssModule/Employee/Delete",
      data: { IDS: IDS }
      //
    });

    if (res.data.code != "200") {

      this.$message.error(res.data.msg);
      return;
    }

    this.$message.success(res.data.content);

    this.$refs.AgGrid.refresh();

  }
}