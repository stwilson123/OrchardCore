<template>
  <bl-validation-provider
    ref="theValidation"
    :name="fieldName"
    :rules="rules"
    :ruleType="ruleType"
    :customMessages="customMessages"
  >
    <el-input
      ref="theinput"
      v-model="vmodel"
      :class="newClass"
      :type="type"
      :maxlength="maxlength"
      :minlength="minlength"
      :show-word-limit="showWordLimit"
      :placeholder="placeholder"
      :clearable="clearable"
      :show-password="showPassword"
      :disabled="newDisabled"
      :size="size"
      :prefix-icon="prefixIcon"
      :suffix-icon="suffixIcon"
      :rows="rows"
      :name="name"
      :readonly="readonly"
      :max="max"
      :min="min"
      :autofocus="autofocus"
      @blur="blurEvent"
      @focus="focusEvent"
      @change="changeEvent"
      @clear="clearEvent"
      @input="inputEvent"
    >
      <fragment slot="prefix" v-if="showPrefix">
        <slot name="prefix"></slot>
      </fragment>
      <fragment slot="suffix" v-if="showSuffix">
        <slot name="suffix"></slot>
      </fragment>
      <fragment slot="prepend" v-if="showPrepend">
        <slot name="prepend"></slot>
      </fragment>
      <fragment slot="append" v-if="showAppend">
        <slot name="append"></slot>
      </fragment>
    </el-input>
  </bl-validation-provider>
</template>
<script>
import { value } from "vue-function-api";
export default {
  name: "BlocksGridValidateInput",
  data() {
    return {
      vmodel: this.value,
      showPrefix: false,
      showSuffix: false,
      showPrepend: false,
      showAppend: false,
      params: {},
      id: null,
      fieldName: "",
      rules: [],
      customMessages: {}
    };
  },
  setup(props, context) {
    let newDisabled = value(props.disabled);
    let newClass = value("");
    let setDisabled = b => {
      newDisabled.value = b;
    };
    let clearEvent = () => {
      context.emit("blocks-clear");
    };
    let foucs = () => {
      context.refs.theinput.focus();
    };
    let blur = () => {
      context.refs.theinput.blur();
    };
    let select = () => {
      context.refs.theinput.select();
    };
    let setClass = name => {
      newClass.value = name;
    };
    return {
      newDisabled,
      setDisabled,
      clearEvent,
      foucs,
      blur,
      select,
      newClass,
      setClass
    };
  },
  props: {
    value: {
      type: String
    },
    type: {
      type: String
    },
    maxlength: {
      type: Number
    },
    minlength: {
      type: Number
    },
    showWordLimit: {
      type: Boolean,
      default: () => false
    },
    placeholder: {
      type: String
    },
    clearable: {
      type: Boolean,
      default: () => false
    },
    showPassword: {
      type: Boolean,
      default: () => false
    },
    disabled: {
      type: Boolean,
      default: () => false
    },
    size: {
      type: String
    },
    prefixIcon: {
      type: String
    },
    suffixIcon: {
      type: String
    },
    rows: {
      type: Number,
      default: () => 2
    },
    name: {
      type: String
    },
    readonly: {
      type: Boolean,
      default: () => false
    },
    max: {
      type: Object
    },
    min: {
      type: Object
    },
    autofocus: {
      type: Boolean,
      default: () => false
    },
    ruleType: {
      type: Number,
      default: 1
    }
  },
  methods: {
    async getValidate() {
      if (this.$refs.theValidation) {
        let res = await this.$refs.theValidation.validate();
        return res.valid;
      } else {
        return true;
      }
    },
    getValue() {
      return this.vmodel;
    },
    setValue(val) {
      this.vmodel = val;
    },
    inputEvent(e) {
      if (this.params["blocks-input"]) {
        this.params["blocks-input"](e, this.id, this);
      }
    },
    changeEvent(e) {
      if (this.params["blocks-change"]) {
        this.params["blocks-change"](e, this.id, this);
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
    }
  },
  created() {
    this.setClass("blocks-input-grid");
    if (this.params.value) {
      this.vmodel = this.params.value;
    }
    this.type = this.params.type;
    this.maxlength = this.params.maxlength;
    this.minlength = this.params.minlength;
    this.showWordLimit = this.params.showWordLimit;
    this.placeholder = this.params.placeholder;
    this.clearable = this.params.clearable;
    this.newDisabled = this.params.disabled;
    this.size = this.params.size;
    this.prefixIcon = this.params.prefixIcon;
    this.suffixIcon = this.params.suffixIcon;
    this.rows = this.params.name;
    this.readonly = this.params.readonly;
    this.max = this.params.max;
    this.min = this.params.min;
    this.autofocus = this.params.autofocus;
    if (this.params.colDef) {
      this.params.colDef.comp = this;
    }
    if (this.params.rules) {
      // let newRules = [];
      // let field = this.params.colDef.field;
      // for (let i of this.params.rules) {
      //   newRules.push(field + "-" + i);
      // }
      this.rules = this.params.rules;
    }
    if (this.params.ruleType) {
      this.ruleType = this.params.ruleType;
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
    if (this.$slots.prefix) {
      this.showPrefix = true;
    }
    if (this.$slots.suffix) {
      this.showSuffix = true;
    }
    if (this.$slots.prepend) {
      this.showPrepend = true;
    }
    if (this.$slots.append) {
      this.showAppend = true;
    }
  },
  watch: {
    value(newVal) {
      this.vmodel = newVal;
    },
    vmodel(newVal) {
      this.$emit("input", newVal);
    }
  }
};
</script>
<style rel="stylesheet/scss" lang="scss">
.blocks-input-grid {
  height: 26px;
  input {
    height: 25px !important;
    line-height: 25px !important;
  }
}
</style>