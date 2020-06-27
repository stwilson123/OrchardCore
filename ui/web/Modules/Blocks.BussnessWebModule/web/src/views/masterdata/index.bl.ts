import { Controller, Component, asyncCompatible, Prop } from "interface"
import add from './add.bl'
import edit from './edit.bl'
@Component
export default class Index extends Controller {
  @Prop({ type: Object }) container;
  gridOptions = {
    //paginationPageSize: 15,
    rowSelection: "multiple",
    editType: "fullRow",
    singleClickEdit: true,
    filterPanelInitExpand: true
    //showPagination: false
  }
  pageInfo = {
    page: {
    }
  }
  gridApi = null
  gridColumnApi = null
  selectData = [
    { id: 0, text: "张三" },
    { id: 1, text: "李四" },
    { id: 2, text: "王五" }
  ]
  formData = {};
  activeName = "second";
  tabsOptions = [];
  testProperty: string;
  confirmVisible = false;
  exceptionIndex = 0;

  constructor() {
    super();
    this.testProperty = "testProperty";
  }

  get gridHeight() {
    return (this.container.height - this.$getRef("header").outerHeight()) + "px";
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
    let dialog = await this.switch({
      uniqueKey: "MasterDataAdd",
    }, {
      componentProps: { formData: this.formData },
      title: "新增"
    })
    let result = await dialog.onDidDismiss();
    // let dialog = await this.$dialog.create({
    //   component: add,
    //   componentProps: { formData: this.formData },
    //   title: "新增"
    // })
    // await dialog.present();
    // let result = await dialog.onDidDismiss();
    //this.$message(result);
    //todo:something
    this.$refs.AgGrid.refresh();
  }

  async addFromOtherModuleClick() {
    this.formData = {
      Id: '',
    }
    let com = await this.getResources("BussnessWebModule", "component", "testResource");
    let dialog = await this.$dialog.create({
      component: com,
      componentProps: { formData: this.formData },
      title: "新增"
    })
    await dialog.present();
    let result = await dialog.onDidDismiss();
    //this.$message(result);
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
      component: edit,
      componentProps: { formData: this.formData },
      title: "修改"
    })
    await dialog.present();
    await dialog.onDidDismiss();
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

  @asyncCompatible()
  async exceptionClick() {
    let httpRequests = [this.$http({
      method: "post",
      url: "/api/services/BussnessWebModule/MasterData/TestException",
      //data: params
    }), this.$http({
      method: "post",
      url: "",
      //data: params
    }), this.$http({
      method: "post",
      url: "/api/services/BussnessWebModule/MasterData/TestException1111",
      //data: params
    }), new Promise((then, reject) => {

    })];
    await httpRequests[this.exceptionIndex++ % (httpRequests.length + 1)]
  }
  async temp2() {
    let pushPromise = new Promise((resolve, reject) => {
      this.$router.push({ path: "/layout/BussnessWebModule/MasterData/Index1" }, () => {
        setTimeout(() => {
          resolve();
        }, (10 * 1000));
      }, () => {
      });
    });
    await pushPromise;
  }
  exportExcel() {
    this.$refs.AgGrid.exportExcel({
      url: "/api/services/BussnessWebModule/test/getserverdata"
    });
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
        headerName: "测试",
        field: "ceshi",
        filter: true,
        editable: true,
        blType: 'text',
        flex: 400,
        blParams: {
          rules: ["required", "email"],
          customMessages: { required: "必填", email: "email格式" }
        }
      },
      {
        headerName: "姓名",
        field: "city",
        filter: true,
        editable: true,
        blType: 'text',
        flex: 400,
        blParams: {
          //"blocks-input": this.cityInput,
          rules: ["required", "email"],
          customMessages: { required: "姓名不能为空", email: "姓名应为email格式" }
        }
      },
      {
        headerName: "年龄",
        field: "num",
        fieldToServer: "num",
        editable: true,
        blType: 'int',
        filter: true,
        blParams: {
          // ruleType: 3,
          // rules: {
          //   rule: "abc",
          //   validate: (val) => {
          //     if (val != 100) {
          //       return false;
          //     }
          //     return true;
          //   }
          // },
          // customMessages: { abc: "年龄自定义" }
          rules: ["required", "numeric"],
          customMessages: { required: "年龄不能为空", numeric: "必须为数字" }
        }
      },
      {
        headerName: "日期",
        field: "registerTime",
        editable: true,
        filter: true,
        blType: 'date',
        sortable: true,
        sort: "desc",
        //fieldToServer: "num",
        //blDataType: "string",
        blParams: {
          config: {
            dateFmt: "yyyy-MM-dd",
            //"blocks-change": this.registerTimeChange
          },
          //rules: ["required"]
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
        field: "isActive",
        editable: true,
        blType: 'select',
        //blDataType: "int",
        //blFilterType: "string",
        filter: true,
        blParams: {
          optionsData: this.selectData,
          //clearable: true,
          multiple: true
          // itemValue: 'id',
          // itemText: 'text',
          //"blocks-change": this.selectChange
        }
      },
      {
        headerName: "远程选择框",
        field: "comboboxId",
        editable: true,
        blType: 'select',
        filter: true,
        blParams: {
          filterable: true,
          remote: true,
          loading: true,
          remoteUrl: "/api/services/BussnessWebModule/Combobox/GetComboboxList",
          remoteMethod: this.remoteMethod,
          clearable: true
        },
        displayTextCol: "num"
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
  selectChange(val, id, comp) {
    //let row = this.gridApi.getSelectedRows()[0];
    let row = this.gridApi.getRowNode(id).data;
    row.num = parseInt(Math.random() * 100);
    row.isActive = val;
    let data = {
      operate: "update",
      rows: [].push(row)
    }
    this.$refs.AgGrid.setRowData(data);
    let numComp = this.$refs.AgGrid.getCellComponent("num");
    numComp.setValue(row.num);
    //console.log(this.rowData[0]);
  }
  registerTimeChange(selectedDates, dateStr, instance, id, comp) {
    // let row = this.gridApi.getSelectedRows()[0];
    // row.city = dateStr;
    // row.registerTime = dateStr;
    // row.num = parseInt(Math.random() * 100);
    // row.isActive = 1;
    // let data = {
    //   operate: "update",
    //   rows: [].push(row)
    // }
    // this.$refs.AgGrid.setRowData(data);
    // let cityComp = this.$refs.AgGrid.getCellComponent("city");
    // let numComp = this.$refs.AgGrid.getCellComponent("num");
    // let isActiveComp = this.$refs.AgGrid.getCellComponent("isActive");
    // cityComp.setValue(row.city);
    // numComp.setValue(row.num);
    // isActiveComp.setValue(row.isActive);
  }
  cityInput(val, id, comp) {
    let row = this.gridApi.getRowNode(id).data;
    row.city = val;
    row.num = parseInt(Math.random() * 100);
    row.isActive = 1;
    row.registerTime = new Date();
    let data = {
      operate: "update",
      rows: [].push(row)
    }
    this.$refs.AgGrid.setRowData(data);
    let numComp = this.$refs.AgGrid.getCellComponent("num");
    let registerTimeComp = this.$refs.AgGrid.getCellComponent("registerTime");
    let isActiveComp = this.$refs.AgGrid.getCellComponent("isActive");
    numComp.setValue(row.num);
    registerTimeComp.setValue(row.registerTime);
    isActiveComp.setValue(row.isActive);
  }
  @asyncCompatible()
  async remoteMethod(query, params) {
    if (query != "") {
      let res = await this.$http({
        method: "post",
        url: params.url,
        data: params.data
      });
      let dataSource = res.data.content.rows.concat([{ id: 1, text: 1 }, { id: 2, text: 2 }, { id: 3, text: 3 }, { id: 4, text: 4 },
      { id: 5, text: 5 }, { id: 6, text: 6 }, { id: 7, text: 7 }
      ]);
      params.api.setData(dataSource);
    }
    else {
      params.api.setData([]);
    }
  }
  viewDidEnter() {
    this.gridApi = this.$refs.AgGrid.api();
    this.gridColumnApi = this.$refs.AgGrid.columnApi();
  }
  getRowEditingData() {
    console.log(this.$refs.AgGrid.getRowData());
    console.log(this.$refs.AgGrid.getRowEditingData());
  }
}