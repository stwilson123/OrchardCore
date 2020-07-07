import { Controller, Component,Prop, asyncCompatible } from "interface"
@Component
export default class detail extends Controller {
  @Prop({ type: Object }) formData;
  constructor() {
    super();
  }
  gridApi = null;
  gridColumnApi = null;

  title = ""
  formData = {};
  pageInfo = { page: { page: 1, pagesize: 50 } };
  StockCountType = ""
  IsConfirm = ""
  gridOptions = {
    paginationPageSize: 12,
    rowSelection: "multiple",
    defaultColDef: "ID",
  }



  //列表
  async gridData(params) {
   // alert(this.formData.ProductFormatID);
    const data = {
        page: {
          page: 1, pageSize: 10
        },
        ProductFormatID: this.formData.ProductFormatID
      }

    let res = await this.$http({
      method: "post",
      url: "/api/services/SysMgrBussenssModule/ProductFormatDetail/GetPageList",
      data:data
    });

    this.$refs.IndexGrid.setData(res.data.content);


  }
 
  

 




  //列表数据
  viewWillEnter() {

    this.gridOptions. columnDefs= [
      {
        headerName: "ID",
        field: "ID",
        hide: true

      },

      {
        headerName:"名称",
        field: "Name",
        checkboxSelection: true,
        filter: true,
        editable: false,
        sortable: true,
        blType: 'text',
      },
      {
        headerName: "长度",
        field: "Length",
        editable: false,
        sortable: true,
        blType: 'float',
        filter: true,
        width: 120
      },
      {
        headerName: "序号",
        field: "Seq",
        editable: false,
        sortable: true,
        blType: 'float',
        filter: true,
        width: 120
      },
      {
        headerName: "起始位",
        field: "ProductformatStart",
        editable: false,
        sortable: true,
        blType: 'float',
        filter: true,
        width: 120
      },
      {
        headerName: "终止位",
        field: "ProductformatEnd",
        editable: false,
        sortable: true,
        blType: 'float',
        filter: true,
        width: 120
      },
    ]
  }

  get rowData() {
    return this.$refs.IndexGrid.getRowData();
  }

  viewDidEnter() {
   


  }

}