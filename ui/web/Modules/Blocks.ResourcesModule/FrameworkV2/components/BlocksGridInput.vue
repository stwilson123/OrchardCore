<script>
import BlocksInput from "./BlocksInput.vue";
export default {
  name: "BlocksGridInput",
  data() {
    return {
      params: {},
      id: null
    };
  },
  mixins: [BlocksInput],
  methods: {
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
    },
    clickEvent(e) {
      if (this.params["blocks-click"]) {
        this.params["blocks-click"](e, this.id, this);
      }
    },
    async getValidate() {
      return true;
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
    if (this.params.node) {
      this.id = this.params.node.id;
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