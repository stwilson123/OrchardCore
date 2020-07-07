import { Controller, Component, Prop, asyncCompatible } from "interface"
@Component
export default class variableselection extends Controller {
  @Prop({ type: Object }) formData;
  constructor() {
    super();
  }
  gridapi = null;
  gridDetailOptions = {
    rowSelection: "multiple",
    defaultColDef: "ID",
    showPagination: false
  }
  async gridData(params) {
    let data = Object.assign({ IsVariable: '0' }, params);
    let res = await this.$http({
      method: "post",
      url: "/api/services/SysMgrBussenssModule/ProductElementType/GetPageList",
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
        width: 420,
      },
      {
        headerName: "名称",
        field: "Name",
        editable: false,
        sortable: true,
        blType: 'text',
        filter: true,
        width: 420,
      }
    ]
  }
  async viewDidEnter() {
    this.gridapi = this.$refs.DetailGrid.api();
  }

  async selectcheck(param) {
    let a = Object.assign({}, param, { ProductFuncID: this.formData.ID })
    let res = await this.$http({
      method: "post",
      url: "/api/services/SysMgrBussenssModule/Product4ElementInfo/GetValElementListByProFuncID",
      data: a
    });
    let selIds = res.data.content["ProductElementTypeIDs"];
    if (selIds.length > 0) {
      for (let i = 0; i < selIds.length; i++) {
        let node = this.gridapi.getRowNode(selIds[i])
        node.setSelected(true);
      }
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
    let data = {
      ProductFuncID: this.formData.ID,
      ProductElementTypeIDs: gridDetailData.map(n => n.ID)
    };
    let res = await this.$http({
      method: "post",
      url: "/api/services/SysMgrBussenssModule/Product4ElementInfo/SaveProductVarElement",
      data
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