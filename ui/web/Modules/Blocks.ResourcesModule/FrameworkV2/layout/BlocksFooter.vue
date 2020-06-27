<template>
  <el-footer :class="blocksClass">
    <slot></slot>
  </el-footer>
</template>
<script>
export default {
  name: "BlocksFooter",
  data() {
    return {
      blocksClass: "blocks-web-page-footer"
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
      this.blocksClass = "blocks-web-dialog-footer";
    }
  }
};
</script>