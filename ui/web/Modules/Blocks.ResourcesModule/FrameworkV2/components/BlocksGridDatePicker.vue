<script>
import BlocksDatePicker from "./BlocksDatePicker.vue";
export default {
  name: "BlocksGridDatePicker",
  data() {
    return {
      vmodel: "",
      firstSelect: true,
      params: {},
      flatpickrClass: "",
      id: null,
      api: null,
      model: {
        filterType: "gridDate",
        dateFrom: null,
        dateTo: null,
        type: "equals"
      }
    };
  },
  mixins: [BlocksDatePicker],
  methods: {
    getDate() {
      if (this.vmodel) {
        return new Date(this.vmodel);
      }
      return null;
    },
    setDate(date) {
      if (date) {
        this.vmodel = date;
      } else {
        this.vmodel = "";
      }
    },
    getValue() {
      let d = this.vmodel;
      if (this.modelType == "date") {
        return new Date(d);
      } else {
        return d;
      }
    },
    setValue(val) {
      this.vmodel = val;
    },
    isFilterActive() {
      return true;
    },
    doesFilterPass() {
      return true;
    },
    getModel() {
      if (this.vmodel != undefined) {
        this.model.dateFrom = this.vmodel;
      }
      return this.model;
    },
    setModel(m) {
      this.model = m;
    },
    filterChangedCallback: () => {},
    parentFilterInstance() {
      let self = this;
      if (this.params.parentFilterInstance) {
        this.params.parentFilterInstance(instance => {
          instance.onFloatingFilterChanged("contains", this.vmodel);
        });
      }
    },
    onParentModelChanged() {},
    onFloatingFilterChanged(op, val) {
      let filter = this.api.getFilterModel();
      //filter[this.params.column.colId].filterType = "gridDate";
      filter[this.params.column.colId].dateFrom = val;
      this.api.setFilterModel(filter);
    },
    onDateChanged() {
      if (this.vmodel || this.firstSelect) {
        if (this.params.onDateChanged) {
          this.firstSelect = false;
          this.params.onDateChanged();
        }
      }
    }
  },
  created() {
    if (this.params.value) {
      this.vmodel = this.params.value;
    }
    if (this.params.onFloatingFilterChanged) {
      this.flatpickrClass =
        "blocks-flatpickr-grid blocks-flatpickr-grid-filter";
    } else {
      this.flatpickrClass = "blocks-flatpickr-grid";
    }
    if (this.params.filterChangedCallback) {
      this.filterChangedCallback = this.params.filterChangedCallback;
    }
    // this.config.onChange = (a, b, c) => {
    //   if (this.params.onDateChanged) {
    //     this.params.onDateChanged();
    //   }
    // };
    this.api = this.params.api;
    if (this.params.config) {
      if (this.params.config.modelType) {
        this.modelType = this.params.config.modelType;
      }
      if (this.params.config["blocks-change"]) {
        this.config.onChange = (a, b, c) => {
          this.params.config["blocks-change"](a, b, c, this.id, this);
        };
      }
      this.setConfig(this.params.config);
    }
    if (this.params.onDateChanged) {
      this.config.onChange = this.onDateChanged.bind(this);
    }
    if (this.params.colDef) {
      this.params.colDef.comp = this;
    }
    if (this.params.node) {
      this.id = this.params.node.id;
    }
  },
  mounted() {
    this.$nextTick(() => {
      if (this.$refs.flatPickr)
        this.$refs.flatPickr.fp.calendarContainer.classList.add(
          "ag-custom-component-popup"
        );
    });
  }
};
</script>
<style rel="stylesheet/scss" lang="scss" scoped>
.flatpickr {
  overflow: hidden;

  position: relative;
  .flatpickr-input {
    -webkit-appearance: none;
    background-color: #fff;
    background-image: none;
    border: 1px solid #dcdfe6;
    box-sizing: border-box;
    color: #606266;
    display: inline-block;
    font-size: inherit;
    height: 36px;
    outline: none;
    padding: 0 25px 0 28px !important;
    width: 100%;
  }
  .flatpickr_icon {
    position: absolute;
    z-index: 1;
    font-size: 14px;
    left: 8px;
    top: 11px;
  }
  .flatpickr_clear {
    position: absolute;
    z-index: 1;
    font-size: 14px;
    right: 8px;
    top: 11px;
    color: #aaa;
    cursor: pointer;
  }
}
.blocks-flatpickr-grid {
  height: 25px;
  width: 100%;
  .flatpickr-input {
    height: 25px;
    padding: 0 25px !important;
  }
  .flatpickr_icon {
    left: 5px;
    top: 7px;
  }
  .flatpickr_clear {
    right: 5px;
    top: 7px;
  }
}
.blocks-flatpickr-grid-filter {
  height: 26px;
  margin-top: 3px;
}
</style>