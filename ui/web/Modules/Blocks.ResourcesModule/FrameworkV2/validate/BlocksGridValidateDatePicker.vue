<template>
  <bl-validation-provider
    ref="theValidation"
    :name="fieldName"
    :rules="rules"
    :customMessages="customMessages"
  >
    <div class="flatpickr" :class="flatpickrClass" @mouseover="mouseOver" @mouseout="mouseOut">
      <i class="el-icon-date flatpickr_icon"></i>
      <flat-pickr
        v-model="vmodel"
        ref="flatPickr"
        :disabled="newDisabled"
        :config="config"
        :placeholder="placeholder"
      ></flat-pickr>
      <i class="el-icon-circle-close flatpickr_clear" v-show="showClear" data-clear></i>
    </div>
  </bl-validation-provider>
</template>
<script>
import { value } from "vue-function-api";
import flatPickr from "vue-flatpickr-component";
import "flatpickr/dist/flatpickr.css";
import { Mandarin } from "flatpickr/dist/l10n/zh.js";
export default {
  name: "BlocksGridValidateDatePicker",
  data() {
    return {
      showClear: false,
      vmodel: "",
      firstSelect: true,
      params: {},
      id: null,
      flatpickrClass: "",
      api: null,
      model: {
        filterType: "gridDate",
        dateFrom: null,
        dateTo: null,
        type: "equals"
      },
      fieldName: "",
      rules: [],
      customMessages: {}
    };
  },
  setup(props, context) {
    let config = {};
    config.wrap = true;
    config.dateFormat = props.dateFmt
      .replace(/yyyy/, "Y")
      .replace(/MM/, "m")
      .replace(/dd/, "d")
      .replace(/HH/, "H")
      .replace(/mm/, "i")
      .replace(/ss/, "S");
    if (props.dateFmt.indexOf("HH") > -1) {
      config.enableTime = true;
      if (props.dateFmt.indexOf("ss") > -1) {
        config.enableSeconds = true;
      }
      if (props.dateFmt.indexOf("yyyy") == -1) {
        config.noCalendar = true;
      } else {
        config.noCalendar = false;
      }
    }
    config.altInput = false;
    if (!JSON.parse(props.readOnly)) {
      config.allowInput = true;
    }
    config.mode = props.mode;
    config.minDate = props.minDate;
    config.maxDate = props.maxDate;
    config.time_24hr = true;
    if (localStorage.getItem("currentLang") === "zh-CN") {
      config.locale = Mandarin;
    }
    config.onOpen = (selectedDates, dateStr, instance) => {
      if (props.dateFmt.indexOf("HH") > -1) {
        let className = instance.calendarContainer.className;
        if (className.indexOf("showTimeInput") === -1) {
          instance.calendarContainer.className = className + " showTimeInput";
        }
      }
      context.emit("blocks-open", selectedDates, dateStr, instance);
    };
    config.onChange = (selectedDates, dateStr, instance) => {
      context.emit("blocks-change", selectedDates, dateStr, instance);
    };
    config.onClose = (selectedDates, dateStr, instance) => {
      context.emit("blocks-close", selectedDates, dateStr, instance);
    };
    let setDateFmt = dateFmt => {
      config.dateFormat = dateFmt
        .replace(/yyyy/, "Y")
        .replace(/MM/, "m")
        .replace(/dd/, "d")
        .replace(/HH/, "H")
        .replace(/mm/, "i")
        .replace(/ss/, "S");
      if (dateFmt.indexOf("HH") > -1) {
        config.enableTime = true;
        if (dateFmt.indexOf("ss") > -1) {
          config.enableSeconds = true;
        }
        if (dateFmt.indexOf("yyyy") == -1) {
          config.noCalendar = true;
        } else {
          config.noCalendar = false;
        }
      }
    };
    let setConfig = c => {
      if (c.dateFmt) {
        setDateFmt(c.dateFmt);
      }
      let newConfig = Object.assign({}, config, c);
      config = newConfig;
    };
    let newDisabled = value(props.disabled);
    let setDisabled = b => {
      newDisabled.value = b;
    };
    return {
      config,
      setDateFmt,
      newDisabled,
      setDisabled,
      setConfig
    };
  },
  components: { flatPickr },
  props: {
    value: {
      type: Object
    },
    modelType: {
      type: String,
      default: () => "date"
    },
    dateFmt: {
      type: String,
      default: () => "yyyy-MM-dd"
    },
    readOnly: {
      type: Boolean,
      default: true
    },
    minDate: {
      type: String,
      defualt: () => "1900-01-01 00:00:00"
    },
    maxDate: {
      type: String,
      defualt: () => "2099-12-31 23:59:59"
    },
    mode: {
      type: String,
      default: () => "single"
    },
    placeholder: {
      type: String
    },
    disabled: {
      type: Boolean,
      default: () => false
    }
  },
  watch: {
    value(newVal) {
      this.vmodel = newVal;
    },
    vmodel(newVal) {
      this.$emit("input", newVal);
      if (newVal == "" || newVal == null) {
        this.showClear = false;
      }
    }
  },
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
    async getValidate() {
      if (this.$refs.theValidation) {
        let res = await this.$refs.theValidation.validate();
        return res.valid;
      } else {
        return true;
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
    mouseOver() {
      if (this.vmodel != "" && !JSON.parse(this.newDisabled)) {
        this.showClear = true;
      }
    },
    mouseOut() {
      this.showClear = false;
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
    if (this.params.rules) {
      this.rules = this.params.rules;
    }
    if (this.params.customMessages) {
      this.customMessages = this.params.customMessages;
    }
    if (this.params.fieldName) {
      this.fieldName = this.params.fieldName;
    } else {
      this.fieldName = this.params.colDef.headerName;
    }
    if (this.params.node) {
      this.id = this.params.node.id;
    }
  },
  mounted() {
    this.$nextTick(() => {
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