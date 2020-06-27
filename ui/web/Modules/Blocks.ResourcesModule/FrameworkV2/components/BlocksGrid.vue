<template>
  <fragment>
    <ag-grid-vue
      ref="theGrid"
      class="ag-theme-balham"
      :class="gridClass"
      :style="gridStyle"
      :gridOptions="gridConfig"
      :rowData="rowData"
      :columnDefs="columnDefs"
      :defaultColDef="gridDefaultColDef"
      :deltaRowDataMode="true"
      :modules="modules"
      :sideBar="sideBar"
      @gridReady="gridReady"
      @cellClicked="cellClicked"
      @cellDoubleClicked="cellDoubleClicked"
      @cellFocused="cellFocused"
      @rowClicked="rowClicked"
      @rowDoubleClicked="rowDoubleClicked"
      @rowSelected="rowSelected"
      @selectionChanged="selectionChanged"
      @cellValueChanged="cellValueChanged"
      @rowValueChanged="rowValueChanged"
      @cellEditingStarted="cellEditingStarted"
      @cellEditingStopped="cellEditingStopped"
      @rowEditingStarted="rowEditingStarted"
      @rowEditingStopped="rowEditingStopped"
      @sortChanged="sortChanged"
      @filterChanged="filterChanged"
      @rowDragEnter="rowDragEnter"
      @rowDragMove="rowDragMove"
      @rowDragLeave="rowDragLeave"
      @rowDragEnd="rowDragEnd"
    ></ag-grid-vue>
    <div v-if="pageShow" class="blocks-grid-page-default" :style="pageStyle">
      <span>每页</span>
      <el-select
        v-model="pageSelectModel"
        @change="serverSelectPageChange"
        style="width:75px;"
        size="mini"
      >
        <el-option
          v-for="item in pageSelectData"
          :key="item.value"
          :label="item.label"
          :value="item.value"
        ></el-option>
      </el-select>
      <span>条</span>
      <button
        type="button"
        :class="disabledClassFirst"
        :disabled="disabledFirst"
        @click="customerFirst"
      >
        <i class="el-icon el-icon-d-arrow-left"></i>
      </button>
      <button
        type="button"
        :class="disabledClassFirst"
        :disabled="disabledFirst"
        @click="customerPrev"
      >
        <i class="el-icon el-icon-arrow-left"></i>
      </button>
      <span>第{{currentPage}}页&nbsp;共{{totalPages}}页</span>
      <button
        type="button"
        :class="disabledClassLast"
        :disabled="disabledLast"
        @click="customerNext"
      >
        <i class="el-icon el-icon-arrow-right"></i>
      </button>
      <button
        type="button"
        :class="disabledClassLast"
        :disabled="disabledLast"
        @click="customerLast"
      >
        <i class="el-icon el-icon-d-arrow-right"></i>
      </button>
      <span>共 {{dataCount}} 条</span>
    </div>
    <div v-if="agPageShow" class="blocks-grid-page-default" :style="pageStyle">
      <span>每页</span>
      <el-select
        v-model="pageSelectModel"
        @change="clientSelectPageChange"
        style="width:75px;"
        size="mini"
      >
        <el-option
          v-for="item in pageSelectData"
          :key="item.value"
          :label="item.label"
          :value="item.value"
        ></el-option>
      </el-select>
      <span>条</span>
      <button
        type="button"
        :class="disabledClassFirst"
        :disabled="disabledFirst"
        @click="agCustomerFirst"
      >
        <i class="el-icon el-icon-d-arrow-left"></i>
      </button>
      <button
        type="button"
        :class="disabledClassFirst"
        :disabled="disabledFirst"
        @click="agCustomerPrev"
      >
        <i class="el-icon el-icon-arrow-left"></i>
      </button>
      <span>第{{currentPage}}页&nbsp;共{{totalPages}}页</span>
      <button
        type="button"
        :class="disabledClassLast"
        :disabled="disabledLast"
        @click="agCustomerNext"
      >
        <i class="el-icon el-icon-arrow-right"></i>
      </button>
      <button
        type="button"
        :class="disabledClassLast"
        :disabled="disabledLast"
        @click="agCustomerLast"
      >
        <i class="el-icon el-icon-d-arrow-right"></i>
      </button>
      <span>共 {{dataCount}} 条</span>
    </div>
  </fragment>
</template>
<script>
//import { AgGridVue } from "ag-grid-vue";
import { AgGridVue } from "@ag-grid-community/vue";
import { value, watch } from "vue-function-api";
import { getLang } from "./language/grid/lang";
import { getClientColumns } from "./grid/client.ts";
import { getServerColumns } from "./grid/server.ts";
import $ from "jquery";
import { AllModules } from "gridModules";
import "@ag-grid-community/all-modules/dist/styles/ag-grid.css";
import "@ag-grid-community/all-modules/dist/styles/ag-theme-balham.css";

export default {
  name: "BlocksGrid",
  components: {
    AgGridVue
  },
  data() {
    return {
      gridApi: null,
      gridColumnApi: null,
      pageOptions: {
        _search: false
      },
      rowData: [],
      pageSelectModel: 0,
      pageSelectData: [
        { value: 30, label: "30" },
        { value: 50, label: "50" },
        { value: 100, label: "100" },
        { value: 200, label: "200" },
        { value: 500, label: "500" },
        { value: 1000, label: "1000" }
      ],
      modules: AllModules
    };
  },
  setup(props, context) {
    let pageShow = value(false);
    let agPageShow = value(false);
    let pageSize = value(0);
    let dataCount = value(0);
    let currentPage = value(0);
    let totalPages = value(0);
    //let rowEditingData = value([]);
    let editingCellValidate = value([]);
    //config
    let defaultOptions = {
      getRowNodeId: data => {
        return data[props.uniqueKey];
      },
      frameworkComponents: {
        agDateInput: "BlGridDatepicker"
        //agDateColumnFilter: "BlGridDatepicker",
        //agDateColumnFloatingFilter:"BlGridDatepicker"
      },
      columnTypes: {
        numberColumn: { filter: "agNumberColumnFilter" },
        textColumn: { filter: "agTextColumnFilter" },
        dateColumn: { filter: "agDateColumnFilter" }
      },
      suppressPaginationPanel: true,
      suppressScrollOnNewData: true,
      floatingFilter: true,
      enableCellTextSelection: true,
      columnDefs: [],
      functionsReadOnly: true,
      sideBar:
        props.filterPanelInitExpand === true ||
        (props.gridOptions && props.gridOptions.filterPanelInitExpand === true)
          ? {
              toolPanels: [
                {
                  id: "columns",
                  labelDefault: "Columns",
                  labelKey: "columns",
                  iconKey: "columns",
                  toolPanel: "agColumnsToolPanel"
                },
                {
                  id: "filters",
                  labelDefault: "Filters",
                  labelKey: "filters",
                  iconKey: "filter",
                  toolPanel: "agFiltersToolPanel"
                }
              ],
              //position: "left",
              defaultToolPanel: "none"
            }
          : "false",
      rowGroupPanelShow: "onlyWhenGrouping"
    };
    let defaultDefaultColDef = {
      minWidth: 100,
      filterParams: {
        applyButton: true,
        clearButton: true
      },
      resizable: true,
      unSortIcon: true,
      suppressMenu: true,
      //filter: true,
      enableValue: true,
      enableRowGroup: true
    };
    let gridDefaultColDef = Object.assign(
      defaultDefaultColDef,
      props.defaultColDef
    );
    pageSize.value = props.gridOptions.paginationPageSize
      ? props.gridOptions.paginationPageSize
      : 30;
    if (props.gridType == "server") {
      defaultOptions.pagination = false;
      if (
        props.gridOptions.showPagination ||
        props.gridOptions.showPagination == undefined
      ) {
        pageShow.value = true;
      }
    } else {
      if (
        props.gridOptions.showPagination ||
        props.gridOptions.showPagination == undefined
      ) {
        agPageShow.value = true;
        defaultOptions.pagination = true;
        defaultOptions.paginationPageSize = pageSize.value;
      } else {
        defaultOptions.pagination = false;
      }
    }
    let gridConfig = Object.assign({}, defaultOptions, props.gridOptions);
    let columnDefsCustom = props.gridOptions.columnDefs;
    if (props.gridType == "server") {
      gridConfig.columnDefs = getServerColumns(
        props,
        columnDefsCustom,
        gridConfig,
        editingCellValidate
      );
    } else {
      gridConfig.columnDefs = getClientColumns(
        props,
        columnDefsCustom,
        gridConfig,
        editingCellValidate
      );
    }
    let setDataCount = (count, pageIndex, pSize = pageSize.value) => {
      dataCount.value = count;
      currentPage.value = pageIndex;
      pageSize.value = pSize;
      totalPages.value = parseInt(
        count % pageSize.value == 0
          ? count / pageSize.value
          : count / pageSize.value + 1
      );
    };
    return {
      gridConfig,
      gridDefaultColDef,
      pageShow,
      agPageShow,
      currentPage,
      dataCount,
      pageSize,
      totalPages,
      setDataCount,
      //rowEditingData,
      editingCellValidate
    };
  },
  props: {
    gridType: {
      type: String,
      default: () => "server"
    },
    noLoadData: {
      type: Boolean,
      default: () => false
    },
    uniqueKey: {
      type: String,
      default: () => "Id"
    },
    gridOptions: {
      type: Object
    },
    columnDefs: {
      type: Array
    },
    defaultColDef: {
      type: Object
    },
    gridClass: {
      type: String
    },
    gridStyle: {
      type: String,
      default: () => ""
    },
    pageClass: {
      type: String
    },
    pageStyle: {
      type: String,
      default: () => ""
    },
    columnsAutoFit: {
      type: Boolean,
      default: () => true
    },
    filterPanelInitExpand: {
      type: Boolean,
      default: () => false
    }
  },
  methods: {
    api() {
      return this.gridConfig.api;
    },
    columnApi() {
      return this.gridConfig.columnApi;
    },
    getCellComponent(field) {
      return this.gridColumnApi.getColumn(field).colDef.comp;
    },
    getRowData() {
      return this.rowData;
    },
    getRowEditingData() {
      let instances = this.gridApi.getCellEditorInstances();
      let cellDefs = this.gridApi.getEditingCells();
      let rowEditingData = [];
      let thisrowIndex = -1;
      let endLen = instances.length;
      let newObj = {};
      for (let i in instances) {
        if (thisrowIndex == -1) {
          thisrowIndex = cellDefs[i].rowIndex;
          newObj[cellDefs[i].column.colId] = instances[i].getValue();
        } else {
          if (thisrowIndex == cellDefs[i].rowIndex) {
            newObj[cellDefs[i].column.colId] = instances[i].getValue();
          } else {
            newObj[this.uniqueKey] = this.rowData[thisrowIndex][this.uniqueKey];
            rowEditingData.push(newObj);
            newObj = {};
            thisrowIndex = cellDefs[i].rowIndex;
            newObj[cellDefs[i].column.colId] = instances[i].getValue();
          }
        }
        if (i == endLen - 1) {
          newObj[this.uniqueKey] = this.rowData[thisrowIndex][this.uniqueKey];
          rowEditingData.push(newObj);
        }
      }
      return rowEditingData;
    },
    setRowData(options) {
      let defaultOptions = {
        operate: "add",
        index: 9999,
        rows: [],
        ids: []
      };
      let config = Object.assign({}, defaultOptions, options);
      if (config.operate == "add") {
        if (config.rows.length > 0) {
          let newRows = Object.assign([], config.rows);
          let rowDataIds = this.rowData.map((item, index, arr) => {
            return item[this.uniqueKey];
          });
          let Ids = config.rows.map((item, index, arr) => {
            return item[this.uniqueKey];
          });
          let containIds = rowDataIds.filter(i => Ids.includes(i));
          if (containIds.length > 0) {
            console.error(
              "grid unique-key：" +
                JSON.stringify(containIds) +
                "重复，新增失败。"
            );
          } else {
            if (config.index == 9999) {
              //this.rowData = this.rowData.concat(newRows);
              for (let item of newRows) {
                let row = Object.assign({}, item);
                this.rowData.splice(9999, 0, row);
              }
            } else {
              for (let item of newRows) {
                let row = Object.assign({}, item);
                this.rowData.splice(config.index, 0, row);
              }
              // if (this.rowData.length > 0) {
              //   let newRowData = [];
              //   let js = 0;
              //   let hasAdd = false;
              //   for (let i of this.rowData) {
              //     if (!hasAdd) {
              //       if (js >= config.index) {
              //         newRowData = newRowData.concat(newRows);
              //         hasAdd = true;
              //       }
              //     }
              //     newRowData.push(i);
              //     js++;
              //   }
              //   this.rowData = newRowData;
              // } else {
              //   for (let item of newRows) {
              //     let row = Object.assign(true, {}, item);
              //     this.rowData.splice(0, 0, row);
              //   }
              //   this.rowData = this.rowData.concat(newRows);
              // }
            }
          }
        }
      } else if (config.operate == "update") {
        if (config.rows.length > 0) {
          let row = Object.assign({}, config.rows[0]);
          let updateIndex = 0;
          this.rowData.map((item, index, arr) => {
            if (item[this.uniqueKey] == row[this.uniqueKey]) {
              updateIndex = index;
            }
          });
          this.rowData.splice(updateIndex, 1, row);
        }
        // if (config.rows.length > 0) {
        //   let newRowData = [];
        //   for (let i of this.rowData) {
        //     let id = i[this.uniqueKey];
        //     let js = 0;
        //     for (let r of config.rows) {
        //       if (r[this.uniqueKey] == id) {
        //         newRowData.push(r);
        //         js++;
        //         break;
        //       }
        //     }
        //     if (js == 0) {
        //       newRowData.push(i);
        //     }
        //   }
        //   //this.gridApi.updateRowData(newRowData);
        //   this.rowData = newRowData;
        // }
      } else if (config.operate == "delete") {
        if (config.ids.length > 0) {
          let newRowData = this.rowData.filter(
            i => !config.ids.includes(i[this.uniqueKey])
          );
          this.rowData = newRowData;
        }
      }
    },
    clearRowData() {
      this.rowData = [];
      this.setDataCount(0, 1);
    },
    setData(data) {
      if (this.gridType == "server") {
        this.rowData = data.rows;
        if (data.pagerInfo) {
          this.setDataCount(
            data.pagerInfo.records,
            data.pagerInfo.page,
            data.pagerInfo.pageSize
          );
        }
      } else {
        this.rowData = data.rows;
        this.setDataCount(data.rows.length, 1);
      }
    },
    refresh(params = {}) {
      this.pageOptions.nd = Math.random();
      this.pageOptions.page = 1;
      Object.assign(this.pageOptions, params);
      this.getServerData("refresh");
    },
    async exportExcel(options) {
      let self = this;
      let loading = this.$loading({
        lock: true,
        spinner: "el-icon-loading"
      });
      let newcolNames = [];
      for (let col of this.gridColumnApi.getAllColumns()) {
        if (!col.hide) {
          newcolNames.push({
            name: col.colDef.headerName,
            colModel: {
              name: col.colDef.field,
              displayTextCol: col.colDef.displayTextCol
                ? col.colDef.displayTextCol
                : undefined,
              dataSource: col.colDef.blParams
                ? col.colDef.blParams.optionsData || []
                : [],
              formatType: {
                type: col.colDef.blType || "",
                desformat: col.colDef.blParams
                  ? col.colDef.blParams.config
                    ? col.colDef.blParams.config.dateFmt || "yyyy-MM-dd"
                    : "yyyy-MM-dd"
                  : "yyyy-MM-dd"
              },
              url: col.colDef.blParams
                ? col.colDef.blParams.remoteUrl || ""
                : ""
            }
          });
        }
      }
      let postData = { page: this.pageOptions };
      if (options.data) {
        postData = Object.assign({}, options.data);
      }
      if (options.all) {
        if (options.count) {
          postData.page.pageSize = options.count;
        } else {
          delete postData.page.pageSize;
        }
      }
      let params = {
        url: options.url,
        colNames: newcolNames,
        postData: JSON.stringify(postData)
      };
      var xmlhttp = new XMLHttpRequest();
      xmlhttp.open("POST", "/Resources/Export/Excel", true);
      xmlhttp.responseType = "blob";
      xmlhttp.setRequestHeader(
        "Content-Type",
        "application/x-www-form-urlencoded;"
      );
      xmlhttp.setRequestHeader(
        "X-XSRF-TOKEN",
        blocks.utility.cookie.getCookieValue("XSRF-TOKEN")
      );
      xmlhttp.send("param=" + JSON.stringify(params));

      xmlhttp.onload = function() {
        if (this.status === 200) {
          var blob = this.response;
          var ran = parseInt(Math.random() * 10000000000);
          self.downloadFile(ran.toString(), blob, loading);
        }
      };
    },
    downloadFile(fileName, content, loading) {
      if (window.navigator.msSaveOrOpenBlob) {
        navigator.msSaveBlob(content, fileName + ".xls");
      } else {
        $("body").append("<a id='" + fileName + "' style='display:none;'></a>");
        var aLink = document.getElementById(fileName);
        var blob = new Blob([content], { type: "application/vnd.ms-excel" });
        var evt = document.createEvent("HTMLEvents");
        evt.initEvent("click", false, false);
        aLink.download = fileName + ".xls";
        aLink.href = URL.createObjectURL(blob);
        aLink.dispatchEvent(evt);
        aLink.click();
        $("#" + fileName).remove();
      }
      loading.close();
    },
    getServerData(eventType) {
      if (!this.noLoadData || eventType != "gridReady") {
        this.gridApi.showLoadingOverlay();
        let params = Object.assign(
          {},
          { page: this.pageOptions },
          { eventType }
        );
        if (
          !(
            this.gridOptions.showPagination ||
            this.gridOptions.showPagination == undefined
          )
        ) {
          params.page.pageSize = undefined;
        }
        this.$emit("gridData", params);
      }
    },
    gridReady() {
      this.pageOptions.nd = Math.random();
      this.pageOptions.page = 1;
      this.pageOptions.pageSize = this.pageSize;
      let columns = this.gridColumnApi.getAllColumns();
      for (let c of columns) {
        if (c.sort != undefined) {
          if (c.colDef.fieldToServer) {
            this.pageOptions.sidx = c.colDef.fieldToServer;
          } else {
            this.pageOptions.sidx = c.colDef.field;
          }
          this.pageOptions.sord = c.sort;
        }
      }
      this.getServerData("gridReady");
      this.$emit("gridReady");
      if (this.columnsAutoFit) {
        this.gridApi.sizeColumnsToFit();
      }
    },
    sortChanged() {
      if (this.gridType == "server") {
        let sortModel = this.gridApi.getSortModel();
        if (sortModel.length > 0) {
          this.pageOptions.nd = Math.random();
          this.pageOptions.page = 1;
          let columns = this.gridColumnApi.getAllColumns();
          let col = columns.filter(n => n.colId == sortModel[0].colId);
          if (col.length > 0) {
            if (col[0].colDef.fieldToServer) {
              this.pageOptions.sidx = col[0].colDef.fieldToServer;
            } else {
              this.pageOptions.sidx = sortModel[0].colId;
            }
          } else {
            this.pageOptions.sidx = sortModel[0].colId;
          }
          this.pageOptions.sord = sortModel[0].sort;
          this.getServerData("sortChanged");
        }
      }
      this.$emit("sortChanged");
    },
    filterChanged() {
      if (this.gridType == "server") {
        let filterModel = this.gridApi.getFilterModel();
        this.pageOptions.nd = Math.random();
        this.pageOptions.page = 1;
        let filter = {
          groupOp: "AND"
        };
        let rules = [];
        if (JSON.stringify(filterModel) == "{}") {
          this.pageOptions._search = false;
          this.pageOptions.filters = undefined;
        } else {
          this.pageOptions._search = true;
          for (let col1 in filterModel) {
            let col = col1;
            let thisCol = this.gridColumnApi
              .getAllColumns()
              .filter(n => n.colDef.field == col);
            if (thisCol.length > 0) {
              let fieldToServer = thisCol[0].colDef.fieldToServer;
              if (fieldToServer) {
                col = fieldToServer;
              }
            }
            let obj = filterModel[col1];
            if (obj.operator) {
              //filter.groupOp = obj.operator;
              //condition1
              let rule1 = {
                data: obj.condition1.filter || obj.condition1.dateFrom,
                field: col
              };
              if (obj.condition1.type == "contains") {
                rule1.op = "cn";
              } else if (obj.condition1.type == "equals") {
                rule1.op = "eq";
              } else if (obj.condition1.type == "notEqual") {
                rule1.op = "ne";
              } else if (obj.condition1.type == "greaterThan") {
                rule1.op = "gt";
              } else if (obj.condition1.type == "lessThan") {
                rule1.op = "lt";
              } else if (obj.condition1.type == "lessThanOrEqual") {
                rule1.op = "le";
              } else if (obj.condition1.type == "greaterThanOrEqual") {
                rule1.op = "ge";
              } else if (obj.condition1.type == "startsWith") {
                rule1.op = "bw";
              } else if (obj.condition1.type == "endsWith") {
                rule1.op = "ew";
              } else {
                rule1.op = "eq";
              }
              if (
                obj.condition1.type == "inRange" ||
                (obj.filterType == "date" && obj.condition1.type == "contains")
              ) {
                let rule3 = {
                  data:
                    obj.condition1.filter ||
                    obj.condition1.dateFrom + " 00:00:00",
                  field: col,
                  op: "ge"
                };
                let rule4 = {
                  data:
                    obj.condition1.filterTo ||
                    obj.condition1.dateTo + " 23:59:59.999",
                  field: col,
                  op: "le"
                };
                if (rule3.data != "") {
                  rules.push(rule3);
                }
                if (rule4.data != "") {
                  rules.push(rule4);
                }
              } else {
                if (rule1.data != "") {
                  rules.push(rule1);
                }
              }
              //condition2
              let rule2 = {
                data: obj.condition2.filter || obj.condition2.dateFrom,
                field: col
              };
              if (obj.condition2.type == "contains") {
                rule2.op = "cn";
              } else if (obj.condition2.type == "equals") {
                rule2.op = "eq";
              } else if (obj.condition2.type == "notEqual") {
                rule2.op = "ne";
              } else if (obj.condition2.type == "greaterThan") {
                rule2.op = "gt";
              } else if (obj.condition2.type == "lessThan") {
                rule2.op = "lt";
              } else if (obj.condition2.type == "lessThanOrEqual") {
                rule2.op = "le";
              } else if (obj.condition2.type == "greaterThanOrEqual") {
                rule2.op = "ge";
              } else if (obj.condition2.type == "startsWith") {
                rule2.op = "bw";
              } else if (obj.condition2.type == "endsWith") {
                rule2.op = "ew";
              } else {
                rule2.op = "eq";
              }
              if (
                obj.condition2.type == "inRange" ||
                (obj.filterType == "date" && obj.condition2.type == "contains")
              ) {
                let rule3 = {
                  data:
                    obj.condition2.filter ||
                    obj.condition2.dateFrom + " 00:00:00",
                  field: col,
                  op: "ge"
                };
                let rule4 = {
                  data:
                    obj.condition2.filterTo ||
                    obj.condition2.dateTo + " 23:59:59.999",
                  field: col,
                  op: "le"
                };
                if (rule3.data != "") {
                  rules.push(rule3);
                }
                if (rule4.data != "") {
                  rules.push(rule4);
                }
              } else {
                if (rule2.data != "") {
                  rules.push(rule2);
                }
              }
            } else if (obj.filterType == "gridSelect") {
              //filter.groupOp = "OR";
              // for (let con of obj.conditions) {
              //   let rule = {
              //     data: con,
              //     field: col,
              //     op: "eq"
              //   };
              //   if (con != "") {
              //     rules.push(rule);
              //   }
              // }
              if (obj.filter !== "") {
                let filterValue;
                try {
                  filterValue = JSON.parse(obj.filter);
                } catch (error) {
                  filterValue = obj.filter;
                }
                if (Array.isArray(filterValue)) {
                  let rule = {
                    data: obj.filter,
                    field: col,
                    op: "in"
                  };
                  rules.push(rule);
                } else {
                  let rule = {
                    data: obj.filter,
                    field: col,
                    op: "eq"
                  };
                  rules.push(rule);
                }
              }
            } else if (obj.filterType == "number") {
              if (obj.type == "inRange") {
                let rule1 = {
                  data: obj.filter,
                  field: col,
                  op: "ge"
                };
                let rule2 = {
                  data: obj.filterTo,
                  field: col,
                  op: "le"
                };
                if (rule1.data != "") {
                  rules.push(rule1);
                }
                if (rule2.data != "") {
                  rules.push(rule2);
                }
              } else {
                let rule = {
                  data: obj.filter || obj.dateFrom,
                  field: col
                };
                if (obj.type == "contains") {
                  rule.op = "cn";
                } else if (obj.type == "equals") {
                  rule.op = "eq";
                } else if (obj.type == "notEqual") {
                  rule.op = "ne";
                } else if (obj.type == "greaterThan") {
                  rule.op = "gt";
                } else if (obj.type == "lessThan") {
                  rule.op = "lt";
                } else if (obj.type == "lessThanOrEqual") {
                  rule.op = "le";
                } else if (obj.type == "greaterThanOrEqual") {
                  rule.op = "ge";
                } else {
                  rule.op = "eq";
                }
                if (rule.data != "") {
                  rules.push(rule);
                }
              }
            } else if (obj.filterType == "date") {
              if (obj.type == "inRange") {
                let rule1 = {
                  data: obj.dateFrom.substring(0, 10) + " 00:00:00",
                  field: col,
                  op: "ge"
                };
                let rule2 = {
                  data: obj.dateTo.substring(0, 10) + " 23:59:59.999",
                  field: col,
                  op: "le"
                };
                if (obj.dateFrom != "") {
                  rules.push(rule1);
                }
                if (obj.dateTo != "") {
                  rules.push(rule2);
                }
              } else {
                if (obj.dateTo == "null null") {
                  let data = obj.dateFrom;
                  let params = this.gridColumnApi.getColumn(col1).colDef
                    .blFilterParams
                    ? this.gridColumnApi.getColumn(col1).colDef.blFilterParams
                    : this.gridColumnApi.getColumn(col1).colDef.blParams;
                  let config = params
                    ? params.config
                    : { dateFmt: "yyyy-MM-dd" };
                  let dateFmt = config ? config.dateFmt : "yyyy-MM-dd";
                  let rule1 = {
                    field: col,
                    op: "ge",
                    data
                  };
                  let rule2 = {
                    field: col,
                    op: "le",
                    data
                  };
                  if (dateFmt.indexOf("dd") > -1) {
                    let subData = data.substring(0, 10);
                    rule1.data = subData + " 00:00:00";
                    rule2.data = subData + " 23:59:59.999";
                  } else if (dateFmt.indexOf("MM") > -1) {
                    let subData = data.substring(0, 7);
                    rule1.data = subData + "-01 00:00:00";
                    rule2.data = subData + "-31 23:59:59.999";
                  } else if (dateFmt.indexOf("yyyy") > -1) {
                    let subData = data.substring(0, 4);
                    rule1.data = subData + "-01-01 00:00:00";
                    rule2.data = subData + "-12-31 23:59:59.999";
                  }
                  rules.push(rule1);
                  rules.push(rule2);
                } else {
                  let rule = {
                    data: obj.dateFrom,
                    field: col
                  };
                  if (obj.type == "equals") {
                    rule.op = "eq";
                  } else if (obj.type == "notEqual") {
                    rule.op = "ne";
                  } else if (obj.type == "greaterThan") {
                    rule.op = "gt";
                    rule.data = obj.dateFrom.substring(0, 10) + " 23:59:59.999";
                  } else if (obj.type == "lessThan") {
                    rule.op = "lt";
                    rule.data = obj.dateFrom.substring(0, 10) + " 00:00:00";
                  } else if (obj.type == "lessThanOrEqual") {
                    rule.op = "le";
                    rule.data = obj.dateFrom.substring(0, 10) + " 23:59:59.999";
                  } else if (obj.type == "greaterThanOrEqual") {
                    rule.op = "ge";
                    rule.data = obj.dateFrom.substring(0, 10) + " 00:00:00";
                  } else {
                    rule.op = "eq";
                  }
                  if (rule.data != "") {
                    rules.push(rule);
                  }
                }
              }
            } else {
              let rule = {
                data: obj.filter || obj.dateFrom,
                field: col
              };
              if (obj.type == "contains") {
                rule.op = "cn";
              } else if (obj.type == "equals") {
                rule.op = "eq";
              } else if (obj.type == "notEqual") {
                rule.op = "ne";
              } else if (obj.type == "greaterThan") {
                rule.op = "gt";
              } else if (obj.type == "lessThan") {
                rule.op = "lt";
              } else if (obj.type == "lessThanOrEqual") {
                rule.op = "le";
              } else if (obj.type == "greaterThanOrEqual") {
                rule.op = "ge";
              } else if (obj.type == "startsWith") {
                rule.op = "bw";
              } else if (obj.type == "endsWith") {
                rule.op = "ew";
              } else {
                rule.op = "eq";
              }
              if (rule.data != "") {
                rules.push(rule);
              }
            }
          }
          if (rules.length > 0) {
            filter.rules = rules;
            this.pageOptions.filters = filter;
          } else {
            this.pageOptions._search = false;
            this.pageOptions.filters = undefined;
          }
        }
        this.getServerData("filterChanged");
      }
      this.$emit("filterChanged");
    },
    pageChanged(p) {
      this.pageOptions.nd = Math.random();
      this.pageOptions.page = p;
      this.getServerData("pageChanged");
    },
    customerFirst() {
      this.currentPage = 1;
      this.pageChanged(this.currentPage);
    },
    customerPrev() {
      this.currentPage--;
      this.pageChanged(this.currentPage);
    },
    customerNext() {
      this.currentPage++;
      this.pageChanged(this.currentPage);
    },
    customerLast() {
      this.currentPage = this.totalPages;
      this.pageChanged(this.currentPage);
    },
    agCustomerFirst() {
      this.currentPage = 1;
      this.gridApi.paginationGoToFirstPage();
    },
    agCustomerPrev() {
      this.currentPage--;
      this.gridApi.paginationGoToPreviousPage();
    },
    agCustomerNext() {
      this.currentPage++;
      this.gridApi.paginationGoToNextPage();
    },
    agCustomerLast() {
      this.currentPage = this.totalPages;
      this.gridApi.paginationGoToLastPage();
    },
    cellClicked(e) {
      this.$emit("cellClicked", e);
    },
    cellDoubleClicked(e) {
      this.$emit("cellDoubleClicked", e);
    },
    cellFocused(e) {
      this.$emit("cellFocused", e);
    },
    rowClicked(e) {
      this.$emit("rowClicked", e);
    },
    rowDoubleClicked(e) {
      this.$emit("rowDoubleClicked", e);
    },
    rowSelected(e) {
      this.$emit("rowSelected", e);
    },
    selectionChanged(e) {
      this.$emit("selectionChanged", e);
    },
    cellValueChanged(e) {
      this.$emit("cellValueChanged", e);
    },
    rowValueChanged(e) {
      this.$emit("rowValueChanged", e);
    },
    cellEditingStarted(e) {
      // if (!this.gridConfig.editType) {
      //   let cellDefs = this.gridApi.getEditingCells();
      //   if (cellDefs.length > 0) {
      //     let rowIndex = cellDefs[0].rowIndex;
      //     let newData = [];
      //     newData.push(this.rowData[rowIndex]);
      //     this.rowEditingData = newData;
      //   }
      // }
      this.$emit("cellEditingStarted", e);
    },
    cellEditingStopped(e) {
      if (!this.gridConfig.editType) {
        let id = e.data[this.uniqueKey];
        let colKey = e.colDef.field;
        let editingRows = this.editingCellValidate.filter(n => n.id == id);
        if (editingRows.length > 0) {
          let editingCells = editingRows[0].cells;
          let cells = editingCells.filter(n => n.colKey == colKey);
          if (cells.length > 0) {
            this.gridConfig.suppressClickEdit = true;
            this.gridApi.startEditingCell({
              rowIndex: e.rowIndex,
              colKey: colKey
            });
          } else {
            this.gridConfig.suppressClickEdit = false;
          }
        } else {
          this.gridConfig.suppressClickEdit = false;
        }
      }
      this.$emit("cellEditingStopped", e);
    },
    rowEditingStarted(e) {
      // if (this.gridConfig.editType) {
      //   if (this.gridConfig.editType == "fullRow") {
      //     let cellDefs = this.gridApi.getEditingCells();
      //     if (cellDefs.length > 0) {
      //       let rowIndex = cellDefs[0].rowIndex;
      //       let newData = [];
      //       newData.push(this.rowData[rowIndex]);
      //       this.rowEditingData = newData;
      //     }
      //   }
      // }
      this.$emit("rowEditingStarted", e);
    },
    rowEditingStopped(e) {
      if (this.gridConfig.editType) {
        if (this.gridConfig.editType == "fullRow") {
          let id = e.data[this.uniqueKey];
          let editingRows = this.editingCellValidate.filter(n => n.id == id);
          if (editingRows.length > 0) {
            let editingCells = editingRows[0].cells;
            if (editingCells.length > 0) {
              this.gridConfig.suppressClickEdit = true;
              this.gridApi.startEditingCell({
                rowIndex: e.rowIndex,
                colKey: editingCells[0].colKey
              });
            } else {
              this.gridConfig.suppressClickEdit = false;
            }
          } else {
            this.gridConfig.suppressClickEdit = false;
          }
        }
      }
      this.$emit("rowEditingStopped", e);
    },
    rowDragEnter(e) {
      this.$emit("rowDragEnter", e);
    },
    rowDragMove(e) {
      this.$emit("rowDragMove", e);
    },
    rowDragLeave(e) {
      this.$emit("rowDragLeave", e);
    },
    rowDragEnd(e) {
      this.$emit("rowDragEnd", e);
    },
    serverSelectPageChange(val) {
      this.pageOptions.nd = Math.random();
      this.pageOptions.page = 1;
      this.pageOptions.pageSize = val;
      this.getServerData("selectPageChanged");
    },
    clientSelectPageChange(val) {
      this.gridApi.paginationSetPageSize(val);
      this.gridApi.paginationGoToFirstPage();
      this.setDataCount(this.rowData.length, 1, val);
    }
  },
  beforeMount() {
    let currentLang = "en";
    if (localStorage.getItem("BlCurrentLang")) {
      currentLang = localStorage.getItem("BlCurrentLang");
    }
    this.gridConfig.localeText = getLang(currentLang);
    let len = this.pageSelectData.filter(n => n.value == this.pageSize).length;
    if (len == 0) {
      this.pageSelectData.push({
        value: this.pageSize,
        label: this.pageSize.toString()
      });
    }
    this.pageSelectModel = this.pageSize;
  },
  mounted() {
    this.gridApi = this.gridConfig.api;
    this.gridColumnApi = this.gridConfig.columnApi;
    if (
      this.gridOptions.showPagination ||
      this.gridOptions.showPagination == undefined
    ) {
      let agPagePanel = this.$refs.theGrid.$el.querySelector(
        ".ag-paging-panel"
      );
      agPagePanel.style.cssText = "display:flex!important;visibility:hidden";
    }
    if (this.columnsAutoFit === true) {
      $(window).resize(() => {
        this.gridApi.sizeColumnsToFit();
      });
    }
    if (this.filterPanelInitExpand) {
      $(window).resize(() => {
        let columnsToolPanelId = "columns"; // default columns instance id
        let columnsToolPanel = this.gridApi.getToolPanelInstance(
          columnsToolPanelId
        );
        columnsToolPanel.expandColumnGroups();
      });
    }
    // for (let item of this.$refs.theGrid.$el.querySelectorAll(
    //   ".ag-floating-filter-input"
    // )) {
    //   item.addEventListener("keyup", fnFloatingFilterInput);
    // }
  },
  computed: {
    disabledFirst() {
      return this.currentPage == 1 ? true : false;
    },
    disabledLast() {
      return this.currentPage == this.totalPages ? true : false;
    },
    disabledClassFirst() {
      return this.currentPage == 1 ? "todisabled" : "";
    },
    disabledClassLast() {
      return this.currentPage == this.totalPages ? "todisabled" : "";
    }
  }
};
// function fnFloatingFilterInput(e) {
//   if (e.keyCode == "13") {
//     this.disabled = true;
//     this.removeEventListener("keyup", fnFloatingFilterInput);
//   }
// }
</script>
<style rel="stylesheet/scss" lang="scss">
.blocks-grid-page-default {
  position: relative;
  background: #fff;
  font-weight: normal !important;
  text-align: right;
  padding: 4px 10px;
  margin-top: -37px;
  font-size: 13px;
  border-left: 1px solid #bdc3c7;
  border-right: 1px solid #bdc3c7;
  border-bottom: 1px solid #bdc3c7;
  button {
    padding: 0 !important;
    width: 20px;
    height: 20px;
    min-width: 0 !important;
    cursor: pointer !important;
    background: none;
    border: none;
  }
  button.todisabled {
    color: #ddd;
    cursor: default !important;
  }
  .el-pager {
    li.number {
      min-width: 20px;
      font-weight: normal;
    }
    li.active {
      font-weight: bold;
    }
  }
  .el-pagination__jump {
    margin: 0 0 0 10px;
  }
}
.ag-theme-balham .ag-filter input[type="radio"] {
  position: relative;
  left: 0;
  opacity: 1;
}
.ag-paging-panel {
  height: 37px;
}
</style>