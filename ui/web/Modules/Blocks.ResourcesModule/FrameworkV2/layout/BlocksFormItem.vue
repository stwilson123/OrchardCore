<template>
  <el-form-item :label="label" :prop="prop">
    <slot></slot>
  </el-form-item>
</template>
<script>
export default {
  name: "BlocksFormItem",
  setup(props, context) {},
  props: {
    label: {
      type: String
    },
    prop: {
      type: String
    }
  }
};
</script>