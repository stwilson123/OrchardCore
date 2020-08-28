<template>
  <el-form
    ref="theForm"
    :model="model"
    :rules="rules"
    :label-width="labelWidth"
    :label-position="labelPosition"
    style="min-height:100px"
  >
    <slot></slot>
  </el-form>
</template>
<script>
export default {
  name: "BlocksForm",
  setup(props, context) {},
  props: {
    model: {
      type: Object
    },
    rules: {
      type: Object
    },
    labelWidth: {
      type: String,
      default: "auto"
    },
    labelPosition: {
      type: String,
      default: "top"
    }
  },
  methods: {
    validate(f) {
      if (f) {
        return this.$refs.theForm.validate(f);
      } else {
        return this.$refs.theForm.validate();
      }
    },
    validateField() {
      return this.$refs.theForm.validateField();
    },
    resetFields() {
      return this.$refs.theForm.resetFields();
    },
    clearValidate() {
      return this.$refs.theForm.clearValidate();
    }
  }
};
</script>