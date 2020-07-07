import { Controller, Component, asyncCompatible, catchWrap } from "interface"
@Component
export default class add extends Controller {
  constructor() {
    super();
  }
  index = 1;
  ruleForm = {
    SetupTypeNo: "", SetupTypeName: "", SetupTypeValue: ""
  };
  gridApi = null
  gridColumnApi = null
  rules = {
    SetupTypeNo: [
      { required: true, message: '类型编码不能为空', trigger: 'blur' }
    ],
    SetupTypeName: [
      { required: true, message: '类型名称不能为空', trigger: 'blur' }
    ]
  }
  gridOptions = {
    showPagination: false,
    floatingFilter: false,
    rowSelection: "multiple",
    columnDefs: [
      {
        headerName: "ID",
        field: "ID",
        hide: true,
        width: 200,
        resizable: true
      },
      {
        headerName: "编码",
        field: "SetupNo",
        checkboxSelection: true,
        editable: true,
        sortable: false,
        width: 300,
        blType: 'text'
      },
      {
        headerName: "名称",
        field: "SetupContents",
        editable: true,
        sortable: false,
        width: 500,
        blType: 'text'
      },
      {
        headerName: "参数值",
        field: "SetupParameter",
        editable: true,
        sortable: false,
        width: 500,
        blType: 'text'
      }
    ]
  };
  async cancelClick() {
    this.exit();
  }
  viewDidEnter() {
    this.gridApi = this.$refs.addGrid.api();
    this.gridColumnApi = this.$refs.addGrid.columnApi();
  }
  addClick() {
    let array = [];
    array.push({ ID: this.index, SetupNo: "", SetupContents: "", SetupParameter: "" });
    this.deleteDuplicateData(array);
  }
  delClick() {
    let rows = this.$refs.addGrid.gridApi.getSelectedRows();
    if (rows.length == 0) {
      this.$message.error("请选择需要删除的数据!");
      return;
    }
    let ids = rows.map(n => n.ID);
    let data = {
      operate: "delete",
      ids: ids
    };

    this.$refs.addGrid.setRowData(data);
    this.index--;
  }
  get rowData() {
    return this.$refs.addGrid.getRowData();
  }
  //表格去重
  deleteDuplicateData(array) {
    var gridInfoDetail = this.rowData; //从表中拿到数据 
    for (let i = array.length; i > 0; i--) {
      for (let j = 0; j < gridInfoDetail.length; j++) {
        if (array[i - 1].ID == gridInfoDetail[j].ID) { //判断重复的依据
          array.splice(i - 1, 1);
          break;
        }
      }
    }

    if (array.length > 0) {
      let gridDatas = {
        operate: "add",
        rows: array
      }
      this.$refs.addGrid.setRowData(gridDatas);
    }
  }
  @asyncCompatible()
  async submitForm() {
    let [resObj, err] = await catchWrap(this.$refs.ruleForm.validate());
    if (resObj === false) {
      return;
    }
    this.gridApi.stopEditing();
    var gridInfoDetail = this.rowData; //从表中拿到数据 
    for (let i = 0; i < gridInfoDetail.length; i++) {
      let j = i + 1;
      if (gridInfoDetail[i].SetupNo == "") {
        this.$message.error("第" + j + "行编码不能为空！");
        return;
      }
      if (gridInfoDetail[i].SetupContents == "") {
        this.$message.error("第" + j + "行名称不能为空");
        return;
      }
      if (gridInfoDetail[i].SetupParameter == "") {
        this.$message.error("第" + j + "行参数不能为空");
        return;
      }
    }
    let data = Object.assign({}, { SetupList: gridInfoDetail }, this.ruleForm);
    let res = await this.$http({
      method: "post",
      url: '/api/services/SysMgrBussenssModule/Setup/AddSetupTypeAndDetail',
      data
    });
    if (res.data.code != "200") {
      this.$message.error(res.data.msg);
      return;
    }
    this.$message.success(res.data.content);
    this.exit();
  }
}