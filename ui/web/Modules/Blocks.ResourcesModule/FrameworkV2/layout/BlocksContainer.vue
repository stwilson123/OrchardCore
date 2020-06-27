<template>
  <el-container :class="blocksClass" :direction="newDirection">
    <slot></slot>
  </el-container>
</template>
<script>
export default {
  name: "BlocksContainer",
  data() {
    return {
      newDirection: "",
      blocksClass: "blocks-web-page-container"
    };
  },
  props: {
    direction: {
      type: String
      //default: () => "vertical"
    },
    type: {
      type: String,
      default: "page"
    }
  },
  created() {
    if (this.direction == undefined) {
      let hasheaderorfooter =
        this.$slots && this.$slots.default
          ? this.$slots.default.some(vnode => {
              let tag = vnode.componentOptions && vnode.componentOptions.tag;
              return tag === "bl-header" || tag === "bl-footer";
            })
          : false;
      if (hasheaderorfooter) {
        this.newDirection = "vertical";
      } else {
        this.newDirection = "horizontal";
      }
    } else {
      this.newDirection = this.direction;
    }
  },
  mounted() {
    if (this.type == "dialog") {
      this.blocksClass = "blocks-web-dialog-container";
    }
  }
};
</script>