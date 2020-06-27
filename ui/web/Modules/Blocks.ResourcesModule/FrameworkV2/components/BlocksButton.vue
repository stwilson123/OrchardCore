<template>
  <el-button
    class="blocks-button-default"
    :size="size"
    :type="type"
    :plain="plain"
    :round="round"
    :circle="circle"
    :loading="newLoading"
    :disabled="newDisabled"
    :icon="icon"
    :autofocus="autofocus"
    @click="click"
  >
    <slot></slot>
  </el-button>
</template>
<script>
import { value } from "vue-function-api";
export default {
  name: "BlocksButton",
  setup(props, context) {
    let newDisabled = value(props.disabled);
    let newLoading = value(false);
    let setDisabled = b => {
      newDisabled.value = b;
    };
    let click = () => {
      if (props.loading) {
        newLoading.value = true;
        context.emit("blocks-click", () => {
          setTimeout(() => {
            newLoading.value = false;
          }, 100);
        });
      } else {
        context.emit("blocks-click");
      }
    };
    return {
      click,
      newDisabled,
      newLoading,
      setDisabled
    };
  },
  props: {
    size: {
      type: String
    },
    type: {
      type: String
    },
    plain: {
      type: Boolean,
      default: () => false
    },
    round: {
      type: Boolean,
      default: () => false
    },
    circle: {
      type: Boolean,
      default: () => false
    },
    loading: {
      type: Boolean,
      default: () => false
    },
    disabled: {
      type: Boolean,
      default: () => false
    },
    icon: {
      type: String
    },
    autofocus: {
      type: Boolean,
      default: () => false
    }
  }
};
</script>