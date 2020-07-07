import { Controller, Component, Prop, asyncCompatible } from "interface"
@Component
export default class codingruleselection extends Controller {
  @Prop({ type: Object }) formData;
  constructor() {
    super();
  }
  gridapi = null;
  gridDetailOptions = {
    paginationPageSize: 12,
    rowSelection: "single",
    defaultColDef: "ID",
    showPagination: false
  }

  async gridData(params) {
    let data = Object.assign({}, params);
    let res = await this.$http({
      method: "post",
      url: "/api/services/SysMgrBussenssModule/ProductFormat/GetPageList",
      data
    });
    this.$refs.DetailGrid.setData(res.data.content);
    await this.selectcheck();
  }

  async viewWillEnter() {
    this.gridDetailOptions.columnDefs = [
      {
        headerName: "ID",
        field: "ID",
        hide: true
      },
      {
        headerName: "编码",
        checkboxSelection: true,
        field: "Code",
        editable: false,
        sortable: true,
        blType: 'text',
        filter: true,
        width: 280,
      },
      {
        headerName: "名称",
        field: "Name",
        editable: false,
        sortable: true,
        blType: 'text',
        filter: true,
        width: 280,
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
        width: 280,
      }
    ]
  }
  async viewDidEnter() {
    this.gridapi = this.$refs.DetailGrid.api();
  }

  async  selectcheck(param) {
    let a = Object.assign({}, param, { ProductFuncID: this.formData.ID })
    let res = await this.$http({
      method: "post",
      url: "/api/services/SysMgrBussenssModule/Product4ElementInfo/GetElementRuleListByProFuncID",
      data: a
    });
    //选中行的勾选
    let selIds = res.data.content["ProductElementRuleIDs"];
    for (let i = 0; i < selIds.length; i++) {
      let node = this.gridapi.getRowNode(selIds[i])
      node.setSelected(true);
    }
  }
  ruleForm = {
  };
  @asyncCompatible()
  async submitForm() {
    let gridDetailData = this.$refs.DetailGrid.gridApi.getSelectedRows();
    if (gridDetailData.length == 0) {
      this.$message.warning("请选择保存的编码规则数据！");
      return;
    }

    if (gridDetailData.length > 1) {
      this.$message.warning("只能选择一条编码规则数据！");
      return;
    }
    let data = {
      ProductFuncID: this.formData.ID,
      ProductElementRuleIDs: gridDetailData.map(n => n.ID)
    };
    let res = await this.$http({
      method: "post",
      url: "/api/services/SysMgrBussenssModule/Product4ElementInfo/SaveProductElementRule",
      data: data
    });
    if (res.data.code != "200") {
      this.$message.error(res.data.msg);
      return;
    }
    this.$message.success(res.data.content);
    this.exit();
  }
  closeDialog() {
    this.exit({ message: "取消" });
  }
}