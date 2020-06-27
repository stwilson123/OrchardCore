<template>
  <el-dialog
    custom-class="blocks-confirm"
    :close-on-click-modal="false"
    :show-close="false"
    :title="title"
    :visible.sync="visible"
    :modal-append-to-body="false"
    width="420px"
    top="30vh"
  >
    <i class="el-icon-warning" style="float:left;font-size:20px;color:#E6A23C;"></i>
    <div style="float:left;margin-top:1px;margin:1px 0 0 7px;">{{message}}</div>
    <div slot="footer" class="dialog-footer">
      <el-button @click="cancelEvent">{{cancelText}}</el-button>
      <el-button type="primary" @click="okEvent">{{okText}}</el-button>
    </div>
  </el-dialog>
</template>
<script>
export default {
  name: "BlocksConfirm",
  props: {
    visible: {
      type: Boolean,
      default: true
    },
    title: {
      type: String,
      default: "提示"
    },
    message: {
      type: String,
      default: "确定删除吗？"
    },
    okText: {
      type: String,
      default: "确定"
    },
    cancelText: {
      type: String,
      default: "取消"
    }
  },
  methods: {
    okEvent() {
      //this.visible = false;
      this.$emit("blocks-ok");
    },
    cancelEvent() {
      this.$emit("blocks-cancel");
    }
  }
};
</script>
<style rel="stylesheet/scss" lang="scss">
.blocks-confirm {
  .el-dialog__header {
    padding: 10px 15px;
  }
  .el-dialog__body {
    padding: 20px 15px;
  }
  .el-dialog__footer {
    padding: 5px 15px 10px 15px;
  }
}
</style>