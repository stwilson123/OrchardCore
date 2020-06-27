<template>
  <el-dialog
    :visible.sync="newVisible"
    :title="title"
    :width="width"
    :fullscreen="fullscreen"
    :top="top"
    :modal="modal"
    :modal-append-to-body="modalAppendToBody"
    :append-to-body="appendToBody"
    :lock-scroll="lockScroll"
    :custom-class="customClass"
    :close-on-click-modal="closeOnClickModal"
    :close-on-press-escape="closeOnPressEscape"
    :show-close="showClose"
    :before-close="beforeClose"
    :center="center"
    :destroy-on-close="destroyOnClose"
    @open="openEvent"
    @close="closeEvent"
    @opened="openedEvent"
    @closed="closedEvent"
  >
    <template v-slot:title>
      <slot name="title"></slot>
    </template>
    <slot></slot>
    <template v-slot:footer class="dialog-footer">
      <slot name="footer"></slot>
    </template>
  </el-dialog>
</template>
<script>
import { value } from "vue-function-api";
export default {
  name: "BlocksDialog",
  setup(props, context) {
    let newVisible = value(props.visible);
    return {
      newVisible
    };
  },
  props: {
    visible: {
      type: Boolean,
      default: () => false
    },
    title: {
      type: String
    },
    width: {
      type: String
    },
    fullscreen: {
      type: Boolean,
      default: () => false
    },
    top: {
      type: String,
      default: "60px"
    },
    modal: {
      type: Boolean,
      default: () => false
    },
    modalAppendToBody: {
      type: Boolean,
      default: () => false
    },
    appendToBody: {
      type: Boolean,
      default: () => false
    },
    lockScroll: {
      type: Boolean,
      default: () => true
    },
    customClass: {
      type: String,
      default: "blocks-web-dialog"
    },
    closeOnClickModal: {
      type: Boolean,
      default: () => false
    },
    closeOnPressEscape: {
      type: Boolean,
      default: () => true
    },
    showClose: {
      type: Boolean,
      default: () => true
    },
    beforeClose: {
      type: Function,
      default: d => {
        d();
      }
    },
    center: {
      type: Boolean,
      default: () => false
    },
    destroyOnClose: {
      type: Boolean,
      default: () => false
    }
  },
  methods: {
    openEvent(e) {
      this.$emit("blocks-open", e);
    },
    openedEvent(e) {
      this.visible = true;
      this.$emit("blocks-opened", e);
    },
    closeEvent(e) {
      this.$emit("blocks-close", e);
    },
    closedEvent(e) {
      this.visible = false;
      this.$emit("blocks-closed", e);
    }
  },
  watch: {
    visible(newVal) {
      this.newVisible = newVal;
    }
  }
};
</script>