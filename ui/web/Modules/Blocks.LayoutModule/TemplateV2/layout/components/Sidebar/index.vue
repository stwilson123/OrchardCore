<template>
  <el-scrollbar wrap-class="scrollbar-wrapper">
    <div ref="TheService" class="sidebar-wrapper">
      <svg-icon :icon-class="blService" class="left-icon" />
      <el-button plain @click="showMenu" class="center-btn">{{ All_Modules }}</el-button>
      <svg-icon :icon-class="blRight" class="right-icon" />
      <div class="side-icon" @click="showMenu" title="全部功能">
        <svg-icon :icon-class="blService" />
      </div>
    </div>
    <el-menu
      style="top:42px;height:calc(100% - 42px)"
      :default-active="$route.path"
      :collapse="isCollapse"
      :background-color="variables.menuBg"
      :text-color="variables.menuText"
      :active-text-color="variables.menuActiveText"
      mode="vertical"
    >
      <sidebar-item
        v-for="route in newMenus"
        :key="route.path"
        :item="route"
        :base-path="route.path"
        v-dragging="{ item: route,list: newMenus}"
      />
    </el-menu>
    <div class="resize" @mousedown="mouseDown"></div>
  </el-scrollbar>
</template>

<script>
import { mapGetters } from "vuex";
import SidebarItem from "./SidebarItem";
import variables from "./../../../styles/variables.scss";
//import { moduleRoute } from "moduleLoader";
import Layout from "./../../Layout";
import axios from "axios";
import { ajaxRequest } from "./../../../utils/ajax";
import { getCollect, setCollect } from "./../../../utils/menu";

export default {
  components: { SidebarItem },
  data() {
    return {
      userMenu: [],
      newMenus: [],
      blService: "bl-service",
      blRight: "bl-right",
      All_Modules: ""
    };
  },
  computed: {
    ...mapGetters(["permission_routers", "sidebar"]),
    routers() {
      // var userMenu = [];
      // ajaxRequest({
      //   //url: "/LayoutModule/Layout/SideBarNavigation",
      //   url: "/api/services/LayoutModule/SideBarNavigation/get",
      //   type: "post",
      //   success: function(res) {
      //     userMenu = res.content.Items;
      //   }
      // });
      // var userRouter = this.getUserSingleRouter(userMenu);
      // return userRouter;
    },
    variables() {
      return variables;
    },
    isCollapse() {
      return !this.sidebar.opened;
    }
  },
  methods: {
    getUserSingleRouter(userMenu) {
      var self = this;
      var userRouter = [];
      for (var i in userMenu) {
        var n = userMenu[i];
        var routerModel = {
          meta: {},
          children: []
        };
        userRouter.push(this.getChildren(n, routerModel));
        // var newRouter = {};
        // var itemsCount = n.Items.length;
        // var name = n.DisplayName;
        // var uId = n.uId;
        // var icon = n.Icon;

        // if (itemsCount > 0) {
        //   var pres = this.getChildComponent(n.Url, name, uId);
        //   if (icon != null) {
        //     newRouter = {
        //       path: "/" + uId,
        //       //path: "/" + n.Items[0].RouteValues.area,
        //       component: pres.pcomponent,
        //       meta: {
        //         title: name,
        //         icon: icon,
        //         uId: uId
        //       },
        //       children: []
        //     };
        //   } else {
        //     newRouter = {
        //       path: "/" + uId,
        //       //path: "/" + n.Items[0].RouteValues.area,
        //       component: pres.pcomponent,
        //       meta: {
        //         title: name,
        //         icon: "menu",
        //         uId: uId
        //       },
        //       children: []
        //     };
        //   }
        //   for (var j in n.Items) {
        //     var m = n.Items[j];
        //     var cres = this.getChildComponent(m.Url, m.DisplayName, m.uId);
        //     var cicon = m.Icon;
        //     if(cicon != null){
        //       newRouter.children.push({
        //         path: "/" + m.Url,
        //         //path: m.Url.substring(m.Url.indexOf("/") + 1),
        //         component: cres.ccomponent,
        //         meta: {
        //           title: m.DisplayName,
        //           icon: cicon,
        //           uId: m.uId
        //         }
        //       });
        //     }
        //     else{
        //       newRouter.children.push({
        //         path: "/" + m.Url,
        //         //path: m.Url.substring(m.Url.indexOf("/") + 1),
        //         component: cres.ccomponent,
        //         meta: {
        //           title: m.DisplayName,
        //           icon: "",
        //           uId: m.uId
        //         }
        //       });
        //     }
        //   }
        // }
        // userRouter.push(newRouter);
      }
      return userRouter;
    },
    getChildren(item, parentitem) {
      var newRouter = parentitem;
      var itemsCount = item.Items.length;
      var icon = item.Icon;
      var pres = this.getChildComponent(item.Url, item.DisplayName, item.uId);
      if (itemsCount > 0) {
        newRouter.path = "/" + item.uId;
      } else {
        newRouter.path = "/" + item.Url;
      }
      newRouter.component = pres.pcomponent;
      newRouter.meta.title = item.DisplayName;
      if (icon != null) {
        newRouter.meta.icon = icon;
      }
      newRouter.meta.uId = item.uId;
      if (itemsCount > 0) {
        for (var j in item.Items) {
          var m = item.Items[j];
          var cres = this.getChildComponent(m.Url, m.DisplayName, m.uId);
          var cicon = m.Icon;
          var childModel = {
            meta: {},
            children: []
          };
          childModel.path = "/" + m.Url;
          childModel.component = cres.pcomponent;
          childModel.meta.title = m.DisplayName;
          if (cicon != null) {
            childModel.meta.icon = cicon;
          }
          childModel.meta.uId = m.uId;
          newRouter.children.push(childModel);
          this.getChildren(item.Items[j], newRouter.children[j]);
        }
      }
      return newRouter;
    },
    getChildComponent(url, title, uId) {
      var allRouter = this.$router.options.routes;
      var pcomponent;
      var ccomponent;
      var count = 0;
      for (var n = 0; n < allRouter.length; n++) {
        pcomponent = allRouter[n].component;
        if (typeof allRouter[n].children != "undefined") {
          for (var m = 0; m < allRouter[n].children.length; m++) {
            if (
              ("/" + url).toLowerCase() ==
              (
                allRouter[n].path +
                "/" +
                allRouter[n].children[m].path
              ).toLowerCase()
            ) {
              ccomponent = allRouter[n].children[m].component;
              allRouter[n].children[m].meta.title = title;
              allRouter[n].children[m].meta.uId = uId;
              count++;
              break;
            }
          }
          if (count != 0) {
            break;
          }
        }
      }
      return {
        pcomponent: pcomponent,
        ccomponent: ccomponent
      };
    },
    mouseDown(ev) {
      var self = this;
      var ev = ev || event;
      document.onmousemove = function(ev) {
        var clientX = ev.clientX;
        if (clientX >= 260 && clientX <= 350) {
          //var bodywidth = document.body.clientWidth;
          // self.$el.style.width = clientX + "px";
          // self.$parent.$children[0].$el.style.width = clientX + "px";
          // self.$parent.$children[4].$el.style.width = bodywidth - clientX + "px";
          // self.$parent.$refs.maincontainer.style.margin = "0px 0px 0px " + clientX + "px";
          // self.$parent.$children[3].$el.style.width = bodywidth - clientX + "px";
          // self.$parent.$children[3].$el.style.margin = "0px 0px 0px " + (clientX - 260) + "px";
          // self.$parent.$children[2].$el.style.width = bodywidth - clientX + "px";
          // self.$parent.$children[2].$el.style.margin = "0px 0px 0px " + (clientX - 260) + "px";

          self.$el.style.width = clientX + "px";
          self.$parent.$children[0].$el.style.width = clientX + "px";
          // self.$parent.$children[4].$el.style.width =
          //   "calc(100vw - " + clientX + "px)";
          self.$parent.$refs.maincontainer.style.width =
            "calc(100vw - " + clientX + "px)";
          self.$parent.$refs.maincontainer.style.left = clientX + "px";
          //self.$parent.$refs.maincontainertop.style.left = clientX + "px";
          let allMenusLeft = self.$parent.$refs.AllMenus.style.left;
          if (allMenusLeft != "") {
            let intLeft = allMenusLeft.substring(0, allMenusLeft.indexOf("px"));
            if (intLeft > 0) {
              self.$parent.$refs.AllMenus.style.left = clientX + "px";
            }
          }
          //self.$parent.$refs.AllMenus.style.left = clientX + "px";
          //self.$parent.$refs.TheDrawdot.style.left = (810 + clientX) + "px";
        }
      };
      document.onmouseup = function(ev) {
        document.onmousemove = null;
      };
    },
    showMenu() {
      this.$refs.TheService.classList.add("sidebar-wrapper-active");
      this.blService = "bl-service-active";
      this.blRight = "bl-right-active";
      if (!this.$store.getters.sidebar.opened) {
        this.$refs.TheService.classList.add("sidebar-wrapper-side");
      }
      this.$emit("menu-click");
    },
    delCollect() {
      this.newMenus = [];
    },
    collect() {
      this.newMenus = getCollect();
    },
    closeMenu() {
      this.$refs.TheService.classList.remove("sidebar-wrapper-active");
      this.blService = "bl-service";
      this.blRight = "bl-right";
      if (!this.$store.getters.sidebar.opened) {
        this.$refs.TheService.classList.add("sidebar-wrapper-side");
      }
    },
    sideOpenMenu() {
      this.$refs.TheService.classList.add("sidebar-wrapper-side");
    },
    sideCloseMenu() {
      this.$refs.TheService.classList.remove("sidebar-wrapper-side");
    }
  },
  mounted() {
    this.newMenus = getCollect();
    if (!this.$store.getters.sidebar.opened) {
      this.$refs.TheService.classList.add("sidebar-wrapper-side");
    }
    this.$dragging.$on("dragend", () => {
      setCollect(this.newMenus);
    });
    this.All_Modules = this.$t("AuthentionModule.All_Modules");
  }
};
</script>
<style rel="stylesheet/scss" lang="scss" scoped>
.resize {
  width: 10px;
  background: transparent;
  cursor: w-resize;
  position: absolute;
  font-size: 0px;
  top: 0;
  bottom: 0;
  right: 0;
  z-index: 1003;
  overflow: hidden;
}
.sidebar-wrapper {
  height: 42px;
  background: rgb(4, 85, 156);
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  z-index: 10;
  text-align: center;
  border-bottom: 1px solid rgba(255, 255, 255, 0.2);
  .left-icon {
    width: 15px;
    height: 15px;
    position: absolute;
    top: 13px;
    left: 20px;
    z-index: 1;
  }
  .center-btn {
    width: 100%;
    height: 100%;
    border: none;
    background: none;
    box-shadow: none;
    color: #fff;
    border-radius: 0;
    padding: 0 0 0 55px;
    text-align: left;
    position: absolute;
    right: 0;
    z-index: 3;
    left: 0;
  }
  .right-icon {
    width: 15px;
    height: 15px;
    position: absolute;
    top: 13px;
    right: 0;
    z-index: 2;
  }
  .side-icon {
    display: none;
    cursor: pointer;
    height: 42px;
    svg {
      width: 15px;
      height: 15px;
      position: absolute;
      top: 13px;
      left: 10px;
      z-index: 1;
    }
  }
  &:hover {
    background: #f7f7f7;
    .left-icon {
      color: #000;
    }
    .center-btn {
      color: #000;
    }
  }
}
.sidebar-wrapper-active {
  background: #f7f7f7;
  .center-btn {
    color: #000;
  }
}
.sidebar-wrapper-side {
  .left-icon {
    display: none;
  }
  .center-btn {
    display: none;
  }
  .right-icon {
    display: none;
  }
  .side-icon {
    display: block;
  }
}
</style>