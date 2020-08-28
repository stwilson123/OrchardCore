<template>
  <el-col :xs="xs" :sm="sm" style="min-height:30px">
    <slot></slot>
  </el-col>
</template>
<script>
export default {
  name: "BlocksCol",
  setup(props, context) {},
  props: {
    xs: {
      type: Object,
      default: 12
    },
    sm: {
      type: Object,
      default: 8
    }
  }
};
</script>