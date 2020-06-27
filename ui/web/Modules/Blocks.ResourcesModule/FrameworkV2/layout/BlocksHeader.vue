<template>
  <el-header :class="blocksClass">
    <slot></slot>
  </el-header>
</template>
<script>
export default {
  name: "BlocksHeader",
  data() {
    return {
      blocksClass: "blocks-web-page-header"
    };
  },
  props: {
    type: {
      type: String,
      default: "page"
    }
  },
  mounted() {
    if (this.type == "dialog") {
      this.blocksClass = "blocks-web-dialog-header";
    }
  }
};
</script>