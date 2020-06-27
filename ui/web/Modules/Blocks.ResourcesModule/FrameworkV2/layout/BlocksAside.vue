<template>
  <el-aside :width="width">
    <slot></slot>
  </el-aside>
</template>
<script>
export default {
  name: "BlocksAside",
  props: {
    width: {
      type: String,
      default: "300px"
    }
  }
};
</script>