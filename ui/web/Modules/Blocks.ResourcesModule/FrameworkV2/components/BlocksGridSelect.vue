<script>
import BlocksSelect from "./BlocksSelect.vue";
export default {
  name: "BlocksGridSelect",
  data() {
    return {
      params: {},
      id: null,
      api: null,
      model: {
        filterType: "gridSelect",
        type: "equals",
        filter: ""
      }
    };
  },
  mixins: [BlocksSelect],
  methods: {
    getValue() {
      return this.vmodel;
    },
    setValue(val) {
      this.vmodel = val;
    },
    isFilterActive() {
      return true;
    },
    getModel() {
      // if (Array.isArray(this.vmodel)) {
      //   this.model.filter = this.vmodel[0];
      // } else {
      //   if (this.vmodel != undefined) {
      //     this.model.filter = this.vmodel;
      //   }
      // }
      return this.model;
    },
    setModel(m) {
      this.model = m;
    },
    doesFilterPass() {
      return true;
    },
    filterChangedCallback: () => {},
    parentFilterInstance() {
      if (this.params.parentFilterInstance) {
        this.params.parentFilterInstance(instance => {
          if (this.multiple) {
            let m = {
              filterType: "gridSelect",
              type: "in",
              filter: JSON.stringify(this.vmodel)
            };
            this.setModel(m);
            instance.onFloatingFilterChanged(
              "equals",
              JSON.stringify(this.vmodel)
            );
          } else {
            let m = {
              filterType: "gridSelect",
              type: "equals",
              filter: this.vmodel
            };
            this.setModel(m);
            instance.onFloatingFilterChanged("equals", this.vmodel);
          }
        });
      }
    },
    onParentModelChanged(parentModel, event) {},
    onFloatingFilterChanged(op, val) {
      let filter = this.api.getFilterModel();
      filter[this.params.column.colId].filter = val;
      this.api.setFilterModel(filter);
    },
    changeEvent(e) {
      if (this.params["blocks-change"]) {
        this.params["blocks-change"](e, this.id, this);
      }
    },
    visibleChangeEvent(e) {
      if (this.params["blocks-visible"]) {
        this.params["blocks-visible"](e, this.id, this);
      }
      if (!e && this.multiple) {
        this.parentFilterInstance();
        this.filterChangedCallback();
      }
    },
    removeTagEvent(e) {
      if (this.params["blocks-remove"]) {
        this.params["blocks-remove"](e, this.id, this);
      }
      if (this.multiple) {
        this.parentFilterInstance();
        this.filterChangedCallback();
      }
    },
    clearEvent(e) {
      if (this.params["blocks-clear"]) {
        this.params["blocks-clear"](e, this.id, this);
      }
    },
    blurEvent(e) {
      if (this.params["blocks-blur"]) {
        this.params["blocks-blur"](e, this.id, this);
      }
    },
    focusEvent(e) {
      if (this.params["blocks-focus"]) {
        this.params["blocks-focus"](e, this.id, this);
      }
    },
    initEvent() {
      if (this.params["blocks-init"]) {
        this.params["blocks-init"](this.id, this);
      }
    }
  },
  created() {
    this.setClass("blocks-select-grid");
    if (this.params.value) {
      this.vmodel = this.params.value;
    }
    if (this.params.optionsData) {
      this.setData(this.params.optionsData);
      if (this.params.filterChangedCallback) {
        this.filterChangedCallback = this.params.filterChangedCallback;
      }
    }
    if (this.params.itemValue) {
      this.itemValue = this.params.itemValue;
    }
    if (this.params.itemText) {
      this.itemText = this.params.itemText;
    }
    this.api = this.params.api;
    this.multiple = this.params.multiple;
    this.disabled = this.params.disabled;
    this.size = this.params.size;
    this.clearable = this.params.clearable;
    this.collapseTags = this.params.collapseTags;
    this.multipleLimit = this.params.multipleLimit;
    this.name = this.params.name;
    this.autocomplete = this.params.autocomplete;
    this.placeholder = this.params.placeholder;
    this.filterable = this.params.filterable;
    this.allowCreate = this.params.allowCreate;
    this.filterMethod = this.params.filterMethod;
    if (this.params.remote) {
      this.remote = this.params.remote;
    }
    this.remoteMethod = this.params.remoteMethod;
    if (this.params.remoteUrl) {
      this.remoteUrl = this.params.remoteUrl;
    }
    if (this.params.remoteParams) {
      this.remoteParams = this.params.remoteParams;
    }
    this.loading = this.params.loading;
    this.loadingText = this.params.loadingText;
    this.noMatchText = this.params.noMatchText;
    this.noDataText = this.params.noDataText;
    this.popperClass = this.params.popperClass;
    this.selectClass = this.params.selectClass;
    this.reserveKeyword = this.params.reserveKeyword;
    this.popperAppendToBody = this.params.popperAppendToBody;
    if (this.params.colDef) {
      this.params.colDef.comp = this;
    }
    if (this.params.node) {
      this.id = this.params.node.id;
    }
    this.initEvent();
  },
  watch: {
    vmodel(newVal) {
      if (!this.multiple) {
        this.parentFilterInstance();
        this.filterChangedCallback();
      }
    }
  }
};
</script>
<style rel="stylesheet/scss" lang="scss">
.blocks-select-grid {
  height: 26px;
  font-weight: normal;
  .el-input {
    height: 26px;
    input {
      height: 26px !important;
      border: none;
    }
    .el-select__caret {
      line-height: 26px;
    }
  }
}
</style>