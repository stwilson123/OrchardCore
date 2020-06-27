<template>
  <validation-provider
    class="blocks-validate-provider"
    ref="TheValidationProvider"
    :vid="vid"
    :name="name"
    :rules="newRules"
    :disabled="disabled"
    :customMessages="customMessages"
    v-slot="{ errors }"
  >
    <slot />
    <div class="error-text" v-for="error in errors" :key="error">{{ error }}</div>
  </validation-provider>
</template>
<script>
import { value } from "vue-function-api";
import { ValidationProvider, extend } from "vee-validate";

export default {
  name: "BlocksValidationProvider",
  components: {
    ValidationProvider
  },
  data() {
    return {
      newRules: ""
    };
  },
  setup(props, context) {},
  methods: {
    validate() {
      return this.$refs.TheValidationProvider.validate();
    }
  },
  created() {
    if (this.ruleType == 1) {
      this.newRules = this.rules.join("|");
    } else if (this.ruleType == 2) {
      this.newRules = this.rules;
    } else if (this.ruleType == 3) {
      this.newRules = this.rules.rule;
      extend(this.rules.rule, {
        validate: this.rules.validate
      });
    } else {
      this.newRules = this.rules;
    }
  },
  props: {
    name: {
      type: String
    },
    vid: {
      type: String
    },
    rules: {
      type: Array
    },
    disabled: {
      type: Boolean,
      default: false
    },
    customMessages: {
      type: Object
    },
    ruleType: {
      type: Number
    }
  }
};
</script>
<style scoped>
.blocks-validate-provider {
  position: relative;
}
.error-text {
  font-size: var(--global-font-content-major);
  color: #ff0000;
  margin: 0;
  position: absolute;
  left: 0;
  top: 21px;
}
</style>