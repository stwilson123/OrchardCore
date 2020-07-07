import { Controller, Component, Prop, asyncCompatible } from "interface"
@Component
export default class edit extends Controller {
  @Prop({ type: Object }) formData;
  constructor() {
    super();
  }
  selectData = []
  gridOptions = {
    singleClickEdit: true,
    showPagination: false,
    floatingFilter: false,
    rowSelection: "multiple",
    columnDefs: []
  };
  gridapi = null;
  gridColumnApi = null;

  //获取单笔数据
  async getDataByID() {
    let data = Object.assign({}, this.ruleForm);
    let result = await this.$http({
      method: "post",
      url: '/api/services/SysMgrBussenssModule/ProductFormat/GetOneById',
      data
    });

    if (result.data.code != "200") {
      this.$message.error(result.data.msg);
      return;
    }
    this.ruleForm.ID = result.data.content.ID;
    this.ruleForm.Code = result.data.content.Code;
    this.ruleForm.Name = result.data.content.Name;
    this.ruleForm.Description = result.data.content.Description;
    for (let i = 0; i < result.data.content.ProductElements.length; i++) {
      let newRow = Object.assign({});
      newRow.ID = i;
      newRow.ProductElementID = result.data.content.ProductElements[i]["ProductElementID"];
      newRow.ProductElementName = result.data.content.ProductElements[i]["ProductElementName"];
      newRow.ProductElementLength = result.data.content.ProductElements[i]["ProductElementLength"];
      newRow.ProductformatStart = result.data.content.ProductElements[i]["ProductformatStart"];
      newRow.ProductformatEnd = result.data.content.ProductElements[i]["ProductformatEnd"];
      let rows = [];
      rows.push(newRow);
      let addrow = {
        operate: "add",
        rows
      };
      this.$refs.TheGrid.setRowData(addrow);
    }
  }
  async getselectData() {
    const data = {
      page: {
        page: 1, pagesize: 100
      }
    }
    let resultData = await this.$http({
      method: "post",
      url: '/api/services/SysMgrBussenssModule/ProductElement/GetComboxList',
      data: data
    });
    let content = resultData.data.content;
    for (var i = 0; i < content.rows.length; i++) {
      this.selectData.push({
        text: content.rows[i].text, id: content.rows[i].id
      });
    }
    this.ruleForm.ID = this.formData.ID;
    await this.getDataByID();
    return content;
  }
  async viewWillEnter() {
    this.gridOptions.columnDefs = [
      {
        headerName: "ID",
        field: "ID",
        hide: true,
        width: 200,
        resizable: true
      },
      {
        headerName: "ProductElementID",
        field: "ProductElementName",
        hide: true,
        width: 200,
        resizable: true
      },
      {
        headerName: "名称",
        field: "ProductElementID",
        editable: true,
        blType: 'select',
        filter: true,
        blParams: {
          optionsData: this.selectData,
          clearable: true,
          "blocks-change": this.changename,
        },
        pinned: 'left'
      },
      {
        headerName: "长度",
        field: "ProductElementLength",
        editable: false,
        sortable: false,
        blType: 'text',
        width: 300
      },
      {
        headerName: "起始位",
        field: "ProductformatStart",
        editable: true,
        sortable: false,
        blType: 'text',
        width: 300
      },
      {
        headerName: "终止位",
        field: "ProductformatEnd",
        editable: true,
        sortable: false,
        blType: 'text',
        width: 300
      }
    ];
    await this.getselectData();
  }
  async viewDidEnter() {
    this.gridapi = this.$refs.TheGrid.api();
  }
  ruleForm = {
    ID: '', Code: '', Name: '', Description: ''
  };
  rules = {
    Code: [
      { required: true, message: '编码不能为空', trigger: 'blur' },
      { min: 2, max: 10, message: '长度在 2 到 10 个字符', trigger: 'blur' }
    ],
    Name: [
      { required: true, message: '名称不能为空', trigger: 'blur' },
      { min: 2, max: 10, message: '长度在 2 到 10 个字符', trigger: 'blur' }
    ]
  }
  async changename(data) {
    if (data != "") {
      let res = await this.$http({
        method: "post",
        url: "/api/services/SysMgrBussenssModule/ProductElement/GetAllList",
        data
      });
      let objdata = res.data.content;
      for (let i = 0; i < objdata.length; i++) {
        if (objdata[i].ID == data) {

          let length = objdata[i].Length;
          let name = objdata[i].Name;
          let row = this.gridapi.getSelectedRows()[0];
          let newRow = Object.assign({}, row);
          newRow.ProductElementLength = length;
          newRow.ProductElementName = name;
          newRow.ProductformatStart = 1;
          newRow.ProductformatEnd = length;
          let rows = [];
          rows.push(newRow);
          let updaterow = {
            operate: "update",
            rows
          };
          this.$refs.TheGrid.setRowData(updaterow);
        }
      }
    }
  }

  @asyncCompatible()
  async submitForm() {
    this.$refs.ruleForm.validate(async (res, obj) => {
      if (res) {
        let data = Object.assign({}, { ProductElements: this.$refs.TheGrid.getRowData() }, this.ruleForm);
        data.registerTime = blocks.utility.dateConvert.toUtcDate(data.registerTime);
        this.showLoadingSave = true;
        let res = await this.$http({
          method: "post",
          url: "/api/services/SysMgrBussenssModule/ProductFormat/Update",
          data: data
        });
        this.showLoadingSave = false;
        if (res.data.code != "200") {
          this.$message.error(res.data.msg);
          return;
        }
        this.$message.success(res.data.content);
        this.exit();
      }
    })
  }
  closeDialog() {
    this.exit({ message: "取消" });
  }
  //删除按钮
  async delClick() {
    let gridData = this.$refs.TheGrid.gridApi.getSelectedRows();
    let ids = gridData.map(n => n.ID);
    if (gridData.length == 0) {
      this.$message.warning(this.$l('PLEASE_CHOOSE_A', [this.$l("STORE_SITE_INFO")]));
      return;
    }
    let data = {
      operate: "delete",
      ids: ids
    };
    this.$refs.TheGrid.setRowData(data);
  }
  //添加按钮
  async addClick() {
    let array = [];
    array.push({
      ID: Math.floor(Math.random() * 10),
      ProductElementName: "",
      ProductElementID: "",
      ProductElementLength: "",
      ProductformatStart: "",
      ProductformatEnd: ""
    });
    if (array.length > 0) {
      let gridDatas = {
        operate: "add",
        rows: array
      }
      this.$refs.TheGrid.setRowData(gridDatas);
    }
  }
}