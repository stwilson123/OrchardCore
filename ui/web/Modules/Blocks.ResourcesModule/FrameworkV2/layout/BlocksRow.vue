<template>
  <el-row :gutter="gutter">
    <slot></slot>
  </el-row>
</template>
<script>
export default {
  name: "BlocksRow",
  setup(props, context) {},
  props: {
    gutter: {
      type: Number,
      default: 28
    }
  }
};
</script>