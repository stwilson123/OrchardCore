<template>
  <el-row :gutter="gutter" style="min-height:30px">
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