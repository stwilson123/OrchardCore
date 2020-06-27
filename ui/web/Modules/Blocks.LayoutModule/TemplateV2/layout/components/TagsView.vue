<template>
  <div class="tags-view-container">
    <scroll-pane ref="scrollPane" class="tags-view-wrapper" style="margin:0 0 0 10px;">
      <router-link
        v-for="(tag,i) in visitedViews"
        ref="tag"
        :class="isActive(tag)?'active':''"
        :to="{ path: tag.path, query: tag.query, fullPath: tag.fullPath }"
        :key="tag.path"
        tag="span"
        class="tags-view-item"
        :style="isLast(i)"
        @click.middle.native="closeSelectedTag(tag)"
        @contextmenu.prevent.native="openMenu(tag,$event)"
      >
        {{ tag.title }}
        <span
          v-if="!tag.meta.affix"
          class="el-icon-close"
          @click.prevent.stop="closeSelectedTag(tag)"
        />
      </router-link>
    </scroll-pane>
    <ul v-show="visible" :style="{left:left+'px',top:top+'px'}" class="contextmenu">
      <li @click="refreshSelectedTag(selectedTag)">{{ $t('AuthentionModule.Tags_Refresh') }}</li>
      <li
        v-if="!(selectedTag.meta&&selectedTag.meta.affix)"
        @click="closeSelectedTag(selectedTag)"
      >{{$t('AuthentionModule.Tags_Close')}}</li>
      <li @click="closeOthersTags">{{ $t('AuthentionModule.Tags_CloseOther') }}</li>
      <li @click="closeAllTags(selectedTag)">{{ $t('AuthentionModule.Tags_CloseAll') }}</li>
    </ul>
  </div>
</template>

<script>
import ScrollPane from "./../../components/ScrollPane";
//import { generateTitle } from "./../../utils/i18n";
import path from "path";
import blocks from "blocks";
import { Bootstrapper, Controller } from "interface";

export default {
  components: { ScrollPane },
  data() {
    return {
      visible: false,
      top: 0,
      left: 0,
      selectedTag: {},
      affixTags: []
    };
  },
  computed: {
    visitedViews() {
      return this.$store.state.tagsView.visitedViews;
    },
    routers() {
      return this.$store.state.permission.routers;
    }
  },
  watch: {
    $route() {
      this.addTags();
      this.moveToCurrentTag();
    },
    visible(value) {
      if (value) {
        document.body.addEventListener("click", this.closeMenu);
      } else {
        document.body.removeEventListener("click", this.closeMenu);
      }
    }
  },
  mounted() {
    this.initTags();
    //this.addTags()
  },
  methods: {
    //generateTitle,
    isLast(i) {
      if (i == this.visitedViews.length - 1) {
        return "border-right: 1px solid #d8dce5;";
      } else {
        return "";
      }
    },
    isActive(route) {
      return route.path === this.$route.path;
    },
    filterAffixTags(routes, basePath = "/") {
      let tags = [];

      for (var i in routes) {
        var route = routes[i];
        if (route.meta && route.meta.affix) {
          tags.push({
            fullPath: path.resolve(basePath, route.path),
            path: path.resolve(basePath, route.path),
            name: route.name,
            meta: { ...route.meta }
          });
        }
        if (route.children) {
          const tempTags = this.filterAffixTags(route.children, route.path);
          if (tempTags.length >= 1) {
            tags = [...tags, ...tempTags];
          }
        }
      }
      // routes.forEach(route => {
      //   if (route.meta && route.meta.affix) {
      //     tags.push({
      //       fullPath: path.resolve(basePath, route.path),
      //       path: path.resolve(basePath, route.path),
      //       name: route.name,
      //       meta: { ...route.meta }
      //     })
      //   }
      //   if (route.children) {
      //     const tempTags = this.filterAffixTags(route.children, route.path)
      //     if (tempTags.length >= 1) {
      //       tags = [...tags, ...tempTags]
      //     }
      //   }
      // })
      return tags;
    },
    initTags() {
      var self = this;
      const affixTags = (this.affixTags = this.filterAffixTags(this.routers));
      for (const tag of affixTags) {
        // Must have tag name
        if (tag.name) {
          this.$store.dispatch("addVisitedView", tag);
        }
      }
      blocks.event.on("NavigationAdd", function(res) {
        var routes = self.$router.options.routes;
        var js = 0;
        var path;
        var moduleUrl = "";
        for (var i = 0; i < routes.length; i++) {
          var children = routes[i].children;
          if (children) {
            for (var j = 0; j < children.length; j++) {
              if (children[j].meta) {
                if (children[j].meta.uId) {
                  var uId = children[j].meta.uId;
                  if (uId) {
                    if (uId == res.uId) {
                      js++;
                      moduleUrl = routes[i].path;
                      path = children[j].path;
                      break;
                    }
                  }
                }
              }
            }
          }
        }
        if (js != 0) {
          self.$router.push({ path: moduleUrl + "/" + path });
        }
      });
    },
    addTags() {
      const { name } = this.$route;
      let uniqueKey = this.$route.meta.uniqueKey;
      let userMenu = this.$parent
        .getUserMenu()
        .filter(n => n.meta.uId == uniqueKey);

      let title = undefined;
      if (userMenu.length > 0) {
        title = userMenu[0].meta.title;
      }
      if (!title) {
        let newRoutes = Bootstrapper.RouteHelper.getRoute();
        for (const route of newRoutes) {
          if (route.meta && uniqueKey === route.meta.uniqueKey) {
            title = route.meta.title;
          }
          if (!route.children) continue;
          for (const routeChild of route.children) {
            if (routeChild.meta && uniqueKey === routeChild.meta.uniqueKey) {
              title = routeChild.meta.title;
            }
          }
        }
      }

      if (name) {
        this.$store.dispatch("addView", {
          view: this.$route,
          title: title ? title : "no-name"
        });
        var eventName = {
          uId: this.$route.meta.uId
          //routePath: this.$route.path
        };
        blocks.event.trigger("NavigationClick", eventName);
      }
      return false;
    },
    moveToCurrentTag() {
      const tags = this.$refs.tag;
      this.$nextTick(() => {
        for (const tag of tags) {
          if (tag.to.path === this.$route.path) {
            this.$refs.scrollPane.moveToTarget(tag);

            // when query is different then update
            if (tag.to.fullPath !== this.$route.fullPath) {
              this.$store.dispatch("updateVisitedView", this.$route);
            }
            break;
          }
        }
      });
    },
    refreshSelectedTag(view) {
      this.$store.dispatch("delCachedView", view).then(() => {
        const { fullPath, matched } = view;
        this.$nextTick(() => {
          if (matched && matched.length > 0) {
            var templateComs = matched.filter(
              m => m.meta && m.meta.isTemplate === true
            );
            if (templateComs && templateComs.length > 0) {
              this.$router.replace({
                path: `${templateComs[0].path}/layoutmodule/redirect` + fullPath
              });
              return;
            }
          }

          this.$router.replace({
            path: "/redirect" + fullPath
          });
        });
      });
    },
    closeSelectedTag(view) {
      this.$store.dispatch("delView", view).then(({ visitedViews }) => {
        if (this.isActive(view)) {
          this.toLastView(visitedViews);
        }
      });
    },
    closeOthersTags() {
      this.$router.push(this.selectedTag);
      this.$store.dispatch("delOthersViews", this.selectedTag).then(() => {
        this.moveToCurrentTag();
      });
    },
    closeAllTags(view) {
      this.$store.dispatch("delAllViews").then(({ visitedViews }) => {
        if (this.affixTags.some(tag => tag.path === view.path)) {
          return;
        }
        this.toLastView(visitedViews);
      });
    },
    toLastView(visitedViews) {
      const latestView = visitedViews.slice(-1)[0];
      if (latestView) {
        this.$router.push(latestView);
      } else {
        // You can set another route
        this.$router.push("/");
      }
    },
    openMenu(tag, e) {
      const menuMinWidth = 105;
      const offsetLeft = this.$el.getBoundingClientRect().left; // container margin left
      const offsetWidth = this.$el.offsetWidth; // container width
      const maxLeft = offsetWidth - menuMinWidth; // left boundary
      const left = e.clientX - offsetLeft + 15; // 15: margin right

      if (left > maxLeft) {
        this.left = maxLeft;
      } else {
        this.left = left;
      }
      //this.top = e.clientY
      this.top = 30;

      this.visible = true;
      this.selectedTag = tag;
    },
    closeMenu() {
      this.visible = false;
    }
  }
};
</script>

<style rel="stylesheet/scss" lang="scss" scoped>
.tags-view-container {
  position: relative;
  z-index: 900;
  //top: 60px;
  height: 43px;
  width: 100%;
  background: #fff;
  border-bottom: 1px solid #d9d9d9;
  //box-shadow: 0 1px 3px 0 rgba(0, 0, 0, .12), 0 0 3px 0 rgba(0, 0, 0, .04);
  .tags-view-wrapper {
    .tags-view-item {
      display: inline-block;
      position: relative;
      cursor: pointer;
      height: 37px;
      line-height: 32px;
      color: #080808;
      background: #fff;
      padding: 0 20px;
      font-size: 14px;
      margin: 6px 0 0 0;
      text-align: center;
      border-top: 1px solid #d8dce5;
      border-left: 1px solid #d8dce5;
      &.active {
        border-top: 3px solid #0066b3;
        display: inline-table;
        color: #0066b3;
        // &::before {
        //   content: '';
        //   background: #fff;
        //   display: inline-block;
        //   width: 8px;
        //   height: 8px;
        //   border-radius: 50%;
        //   position: relative;
        //   margin-right: 2px;
        // }
      }
    }
  }
  .contextmenu {
    margin: 0;
    background: #fff;
    z-index: 3900;
    position: absolute;
    list-style-type: none;
    padding: 5px 0;
    border-radius: 4px;
    font-size: 12px;
    font-weight: 400;
    color: #333;
    box-shadow: 2px 2px 3px 0 rgba(0, 0, 0, 0.3);
    li {
      margin: 0;
      padding: 7px 16px;
      cursor: pointer;
      &:hover {
        background: #eee;
      }
    }
  }
}
</style>

<style rel="stylesheet/scss" lang="scss">
//reset element css of el-icon-close
.tags-view-wrapper {
  .tags-view-item {
    .el-icon-close {
      font-size: 12px;
      position: absolute;
      z-index: 100;
      right: 2px;
      top: 11px;
      vertical-align: 2px;
      border-radius: 50%;
      text-align: center;
      transition: all 0.3s cubic-bezier(0.645, 0.045, 0.355, 1);
      transform-origin: 100% 50%;
      // &:before {
      //   transform: scale(.6);
      //   display: inline-block;
      //   vertical-align: -3px;
      // }
      &:hover {
        background-color: #b4bccc;
        color: #fff;
      }
    }
  }
}
</style>
