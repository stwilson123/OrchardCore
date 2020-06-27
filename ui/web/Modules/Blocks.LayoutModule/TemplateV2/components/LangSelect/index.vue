<template>
  <el-dropdown trigger="click" class="international" @command="handleSetLanguage">
    <!-- <div>
      <svg-icon class-name="international-icon" icon-class="language"/>
    </div>-->
    <span class="el-dropdown-link">
      {{displayName}}
      <i class="el-icon-arrow-down el-icon--right"></i>
    </span>
    <el-dropdown-menu slot="dropdown">
      <!-- <el-dropdown-item :disabled="language==='zh'" command="zh">中文</el-dropdown-item>
      <el-dropdown-item :disabled="language==='en'" command="en">English</el-dropdown-item>-->
      <el-dropdown-item
        v-for="item in langs"
        :key="item"
        :disabled="language===item.name"
        :command="item.name"
      >{{item.displayName}}</el-dropdown-item>
    </el-dropdown-menu>
  </el-dropdown>
</template>

<script>
import { ajaxRequest } from "./../../utils/ajax";

export default {
  // data() {
  //   return {
  //     langs: [],
  //     language: "en"
  //   };
  // },
  computed: {
    displayName() {
      return this.langs.filter(n => n.name == this.language)[0].displayName;
    },
    language() {
      return this.$store.getters.language;
    },
    langs() {
      return this.$store.getters.langs;
    }
  },
  methods: {
    handleSetLanguage(lang) {
      var self = this;
      ajaxRequest({
        url: "/LayoutModule/layout/changeCulturevue",
        type: "get",
        params: {
          cultureName: lang
        },
        success: function(res) {
          if (res.success) {
            self.$i18n.locale = lang;
            self.$store.dispatch("setLanguage", lang);
            window.location.reload();
          }
        }
      });
    }
  }
};
</script>
<style rel="stylesheet/scss" lang="scss" scoped>
.el-dropdown-link {
  font-size: 14px;
}
</style>