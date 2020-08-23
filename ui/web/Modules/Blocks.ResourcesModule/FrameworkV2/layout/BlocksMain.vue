<template>
  <el-main :class="blocksClass" style="min-height:300px">
    <slot></slot>
  </el-main>
</template>
<script>
export default {
  name: "BlocksMain",
  data() {
    return {
      blocksClass: "blocks-web-page-main"
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
      this.blocksClass = "blocks-web-dialog-main";
    }
  }
};
</script>