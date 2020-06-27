<template>
  <el-tabs
    v-model="vmodel"
    :type="type"
    :closable="closable"
    :addable="addable"
    :editable="editable"
    :tab-position="tabPosition"
    :stretch="stretch"
    :before-leave="beforeLeave"
    @tab-click="tabClick"
    @tab-remove="tabRemove"
    @tab-add="tabAdd"
    @edit="tabEdit"
  >
    <el-tab-pane
      v-for="item in optionsData"
      :key="item.name"
      :disabled="item.disabled"
      :label="item.label"
      :name="item.name"
      :closable="item.closable"
      :lazy="item.lazy"
    >
      <slot :name="item.name"></slot>
    </el-tab-pane>
  </el-tabs>
</template>
<script>
export default {
  name: "BlocksTabs",
  data() {
    return {
      vmodel: this.value
    };
  },
  setup(props, context) {},
  props: {
    value: {
      type: String
    },
    type: {
      type: String
    },
    closable: {
      type: Boolean,
      default: false
    },
    addable: {
      type: Boolean,
      default: false
    },
    editable: {
      type: Boolean,
      default: false
    },
    tabPosition: {
      type: String,
      default: "top"
    },
    stretch: {
      type: Boolean,
      default: false
    },
    beforeLeave: {
      type: Function,
      default: () => true
    },
    optionsData: {
      type: Array
    }
  },
  methods: {
    tabClick(tab, e) {
      this.$emit("blocks-click", tab, e);
    },
    tabRemove(e) {
      this.$emit("blocks-remove", e);
    },
    tabAdd() {
      this.$emit("blocks-add");
    },
    tabEdit(targetName, action) {
      this.$emit("blocks-edit", targetName, action);
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