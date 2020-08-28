<template>
  <div class="flatpickr" :class="flatpickrClass" @mouseover="mouseOver" @mouseout="mouseOut">
    <!-- <div class="flatpickr_icon">
    </div>-->
    <i class="el-icon-date flatpickr_icon"></i>
    <flat-pickr
      v-model="vmodel"
      ref="flatPickr"
      :disabled="newDisabled"
      :config="config"
      :placeholder="placeholder"
      @blur="blurEvent"
    ></flat-pickr>
    <i class="el-icon-circle-close flatpickr_clear" v-show="showClear" data-clear></i>
    <!-- <div class="flatpickr_clear">
      <el-button icon="el-icon-close" type="danger" data-clear></el-button>
    </div>-->
  </div>
</template>
<script>
import { value } from "vue-function-api";
import flatPickr from "vue-flatpickr-component";
import "flatpickr/dist/flatpickr.css";
import { Mandarin } from "flatpickr/dist/l10n/zh.js";
//import { english } from "flatpickr/dist/l10n/default.js";
export default {
  name: "BlocksDatePicker",
  data() {
    return {
      vmodel: this.value,
      flatpickrClass: "",
      showClear: false
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
    },
    name: {
      type: String
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
    mouseOver() {
      if (this.vmodel != "" && !JSON.parse(this.newDisabled)) {
        this.showClear = true;
      }
    },
    mouseOut() {
      this.showClear = false;
    },
    blurEvent(e) {
      if (this.$parent && this.$parent["form"]) {
        this.$parent["form"].validateField(this.name);
      }
    }
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
    // button {
    //   border-radius: 0px;
    //   border: 1px solid #f56c6c;
    //   box-shadow: none;
    //   padding: 0px;
    //   margin: 0px;
    //   width: 40px;
    //   height: 36px;
    // }
  }
}
</style>