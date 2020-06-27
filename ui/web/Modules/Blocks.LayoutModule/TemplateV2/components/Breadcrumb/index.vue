<template>
  <el-breadcrumb class="app-breadcrumb" separator="/">
    <transition-group name="breadcrumb">
      <el-breadcrumb-item v-for="(item,index) in levelList" :key="item.path">
        <span
          v-if="item.redirect==='noredirect'||index==levelList.length-1"
          class="no-redirect"
        >{{ getTitle(item.meta) }}</span>
        <a v-else @click.prevent="handleLink(item)">{{ getTitle(item.meta) }}</a>
      </el-breadcrumb-item>
    </transition-group>
  </el-breadcrumb>
</template>

<script>
//import { generateTitle } from './../../utils/i18n'
import pathToRegexp from "path-to-regexp";

export default {
  data() {
    return {
      levelList: null
    };
  },
  watch: {
    $route() {
      this.getBreadcrumb();
    }
  },
  created() {
    this.getBreadcrumb();
  },
  methods: {
    getTitle(item) {
      if (item.uniqueKey) {
        let uniqueKey = item.uniqueKey;
        let userMenu = this.$parent.$parent
          .getUserMenu()
          .filter(n => n.meta.uId == uniqueKey);
        if (userMenu.length > 0) {
          return userMenu[0].meta.title;
        }
      }
      return item.title;
    },
    //generateTitle,
    getBreadcrumb() {
      let matched = this.$route.matched.filter(item => item.name);
      const first = matched[0];
      // if (first && first.name.trim().toLocaleLowerCase() !== 'Dashboard'.toLocaleLowerCase()) {
      //   matched = [
      //     {
      //       path: '/dashboard',
      //       meta: { title: 'HomePage'
      //     }
      //   }].concat(matched)
      // }
      var firstPath = first.path;
      var dashboardUrl = this.$store.getters.dashboardRoute.url;
      if (
        first &&
        firstPath.trim().toLocaleLowerCase() !==
          dashboardUrl.toLocaleLowerCase()
      ) {
        matched = [
          {
            path: dashboardUrl,
            meta: {
              title: this.$t(
                "AuthentionModule." + this.$store.getters.dashboardRoute.title
              )
            }
          }
        ].concat(matched);
      } else {
        first.meta.title = this.$t(
          "AuthentionModule." + this.$store.getters.dashboardRoute.title
        );
      }
      this.levelList = matched.filter(
        item =>
          item.meta &&
          (item.meta.title || item.meta.uniqueKey) &&
          item.meta.breadcrumb !== false
      );
    },
    pathCompile(path) {
      const { params } = this.$route;
      var toPath = pathToRegexp.compile(path);
      return toPath(params);
    },
    handleLink(item) {
      const { redirect, path } = item;
      if (redirect) {
        this.$router.push(redirect);
        return;
      }
      this.$router.push(this.pathCompile(path));
    }
  }
};
</script>

<style rel="stylesheet/scss" lang="scss" scoped>
.app-breadcrumb.el-breadcrumb {
  display: inline-block;
  font-size: 14px;
  line-height: 60px;
  margin-left: 8px;
  color: #080808;
  .no-redirect {
    color: #080808;
    cursor: text;
  }
}
</style>
