<template>
  <validation-observer ref="TheValidationObserver" v-slot="{ errors }">
    <slot />
  </validation-observer>
</template>
<script>
import { ValidationObserver } from "vee-validate";
export default {
  name: "BlocksValidationObserver",
  components: {
    ValidationObserver
  },
  methods: {
    async validate() {
      let arr = [];
      for (let i of this.$slots.default) {
        if (i.tag.indexOf("BlocksValidationProvider") > -1) {
          var obj = {};
          let v = await i.componentInstance.validate();
          obj.name = i.componentInstance.vid
            ? i.componentInstance.vid
            : i.componentInstance.name
            ? i.componentInstance.name
            : i.componentInstance.$el.innerHTML;
          obj.failedRules = v.failedRules;
          obj.errors = v.errors;
          arr.push(obj);
        }
      }
      let valid = await this.$refs.TheValidationObserver.validate();
      return {
        valid: valid,
        content: arr
      };
    }
  }
};
</script>