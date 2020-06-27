<template>
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
    @click.native="clickEvent"
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
</template>
<script>
import { value } from "vue-function-api";
export default {
  name: "BlocksInput",
  data() {
    return {
      vmodel: this.value,
      showPrefix: false,
      showSuffix: false,
      showPrepend: false,
      showAppend: false
    };
  },
  setup(props, context) {
    let newDisabled = value(props.disabled);
    let newClass = value("");
    let setDisabled = b => {
      newDisabled.value = b;
    };
    let blurEvent = e => {
      context.emit("blocks-blur", e);
    };
    let focusEvent = e => {
      context.emit("blocks-focus", e);
    };
    let changeEvent = e => {
      context.emit("blocks-change", e);
    };
    let clearEvent = () => {
      context.emit("blocks-clear");
    };
    let inputEvent = e => {
      context.emit("blocks-input", e);
    };
    let clickEvent = e => {
      context.emit("blocks-click", e);
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
      blurEvent,
      focusEvent,
      changeEvent,
      clearEvent,
      inputEvent,
      foucs,
      blur,
      select,
      newClass,
      setClass,
      clickEvent
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