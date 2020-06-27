import { Controller, Component, asyncCompatible } from "interface"
import add from './add.bl'
@Component
export default class Index extends Controller {
  gridOptions = {
    paginationPageSize: 12,
    rowSelection: "multiple"
    //showPagination: false
  }
  pageInfo = {
    page: {
    }
  }
  gridApi = null
  gridColumnApi = null
  selectData = [
    { value: "11", label: "张三" },
    { value: "2", label: "李四" },
    { value: "3", label: "王五" }
  ]
  formData = {};
  activeName = "second";
  tabsOptions = [];
  confirmVisible = false;

  constructor() {
    super();
  }

  get rowData() {
    return this.$refs.AgGrid.getRowData();
  }

  confirmOk() {
    //todo:http 请求
    this.$message({
      type: 'success',
      message: '删除成功'
    });
    this.confirmVisible = false;
  }

  confirmCancel() {
    this.$message({
      type: 'info',
      message: '已取消删除'
    });
    this.confirmVisible = false;
  }

  async gridData(params) {
    //this.pageInfo = params
    let res = await this.$http({
      method: "post",
      url: "/api/services/BussnessWebModule/test/getserverdata",
      data: params
    });
    this.$refs.AgGrid.setData(res.data.content);
  }

  async addClick() {
    this.formData = {
      Id: '',
    }
    
   
    let dialog = await this.$dialog.create({
      component:add ,
      componentProps: { formData: this.formData },
      title: "新增"
    })
    await dialog.present();
    let result = await dialog.onDidDismiss();
    this.$message(result);
    //todo:something
    this.$refs.AgGrid.refresh();
  }

  async addFromOtherModuleClick() {
    this.formData = {
      Id: '',
    }
    
    let com = await this.getResources("BussnessWebModule","component","testResource");
    debugger
    let dialog = await this.$dialog.create({
      component:com ,
      componentProps: { formData: this.formData },
      title: "新增"
    })
    await dialog.present();
    let result = await dialog.onDidDismiss();
    this.$message(result);
    //todo:something
    this.$refs.AgGrid.refresh();
  }

  async editClick() {
    let rows = this.gridApi.getSelectedRows();
    if (rows.length == 0) {
      this.$message({
        message: "请选择要修改的行",
        type: 'error'
      });
      return;
    }
    if (rows.length > 1) {
      this.$message({
        message: "只能选择一行编辑",
        type: 'error'
      });
      return;
    }
    let data = Object.assign({}, rows[0]);
    this.formData = {
      Id: data.Id
    }
    let dialog = await this.$dialog.create({
      component: add,
      componentProps: { formData: this.formData },
      title: "修改"
    })
    await dialog.present();
    await dialog.onDidDismiss();
    console.log("edit finish")
  }

  @asyncCompatible()
  deleteClick() {
    let rows = this.gridApi.getSelectedRows();
    if (rows.length == 0) {
      this.$message({
        message: "请选择要删除的行",
        type: 'error'
      });
      return;
    }
    // let ids = rows.map(n => n.Id);
    // this.$confirm('确定删除吗？', '提示', {
    //   confirmButtonText: '确定',
    //   cancelButtonText: '取消',
    //   type: 'warning'
    // }).then(() => {
    //   let data = {
    //     operate: "delete",
    //     ids: ids
    //   };
    //   this.$refs.AgGrid.setRowData(data);
    //   //todo:http 请求
    //   this.$message({
    //     type: 'success',
    //     message: '删除成功'
    //   });
    // }).catch(() => {
    //   this.$message({
    //     type: 'info',
    //     message: '已取消删除'
    //   });
    // });
    this.confirmVisible = true;
  }

  temp2() {
    this.$router.push({ path: "/layout/BussnessWebModule/MasterData/Index1" })
  }

  selectChange(val) {
    console.log(val);
  }
  viewWillEnter() {
    this.gridOptions.columnDefs = [
      {
        headerName: "Id",
        field: "Id",
        checkboxSelection: true,
        headerCheckboxSelection: true
      },
      {
        headerName: this.$l("BussnessWeb_NameField"),
        field: "city",
        filter: true,
        editable: true,
        blType: 'text',
        flex: 400
      },
      {
        headerName: "年龄",
        field: "num",
        editable: true,
        blType: 'int',
        filter: true,
        pinned: "right"
      },
      {
        headerName: "年龄",
        field: "num1",
        editable: true,
        blType: 'int',
        filter: true,
       
      }, {
        headerName: "年龄",
        field: "num2",
        editable: true,
        blType: 'int',
        filter: true,
       
      },
      {
        headerName: "日期",
        field: "registerTime",
        editable: true,
        filter: true,
        blType: 'date',
        sortable: true,
        sort: "asc",
        blParams: {
          config: {
            dateFmt: "yyyy-MM-dd HH:mm:ss"
          }
        },
        width: 200
      },
      // {
      //   headerName: "选择框",
      //   field: "theSelect",
      //   editable: true,
      //   cellEditorFramework: "BlGridSelect",
      //   cellEditorParams: {
      //     optionsData: this.selectData,
      //     clearable: true
      //   },
      //   filterFramework: "BlGridSelect",
      //   filterParams: {
      //     optionsData: this.selectData,
      //     clearable: true
      //   },
      //   valueFormatter: params => {
      //     let text = "";
      //     switch (params.value) {
      //       case "1":
      //         text = "张三";
      //         break;
      //       case "2":
      //         text = "李四";
      //         break;
      //       case "3":
      //         text = "王五";
      //         break;
      //     }
      //     return text;
      //   }
      // },
      {
        headerName: "选择框",
        field: "comboboxId",
        editable: true,
        blType: 'select',
        filter: true,
        blParams: {
          optionsData: this.selectData,
          clearable: true,
          "blocks-change": this.selectChange
        },
        pinned: 'left'
      }
    ];
    this.tabsOptions = [{
      name: "first",
      label: this.$l("BussnessWeb_Grid"),
      disabled: false
    }, {
      name: "second",
      label: "Grid"
    }];
  }

  viewDidEnter() {
    this.gridApi = this.$refs.AgGrid.api();
    this.gridColumnApi = this.$refs.AgGrid.columnApi();
  }
}