<template>
  <div :class="classObj" class="app-wrapper">
    <div v-if="device==='mobile' && sidebar.opened" class="drawer-bg" @click="handleClickOutside" />
    <fragment>
      <logo />
      <sidebar ref="TheSidebar" class="sidebar-container" @menu-click="showMenu" />
    </fragment>
    <div class="main-container" ref="maincontainer">
      <div class="main-container-top" ref="maincontainertop">
        <navbar />
        <tags-view ref="tagView" />
      </div>
      <app-main />
    </div>
    <div class="all-menus-bg" ref="AllMenusBg" v-show="isShow" @click="closeMenu"></div>
    <div class="all-menus" ref="AllMenus">
      <el-button @click="deletCollect" icon="el-icon-delete" class="delete-collect-btn"></el-button>
      <el-button @click="closeMenu" icon="el-icon-close" class="close-menu-btn"></el-button>
      <div class="all-menus-content">
        <el-input
          ref="TheSearhInput"
          v-model="searchValue"
          @input="searchChange"
          prefix-icon="el-icon-search"
          placeholder="请输入关键字"
        ></el-input>
        <div class="visit">
          <div class="title">{{ Recently_Visited }}</div>
          <ul>
            <li v-for="item in visitList" :key="item.meta.uId">
              <el-button
                class="menu-btn"
                @click="clickMenu(item.meta.pId,item.meta.uId,item.path)"
              >{{item.meta.title}}</el-button>
            </li>
          </ul>
        </div>
        <div class="card-list">
          <div class="card-list-col">
            <div
              class="card-list-item"
              v-for="item in firstCol"
              :key="item.meta.uId"
              v-show="item.meta.show"
            >
              <div class="title">
                {{item.meta.title}}
                <a :ref="item.meta.title">&nbsp;</a>
              </div>
              <ul>
                <li v-for="child in item.children" :key="child.meta.uId" v-show="child.meta.show">
                  <el-button
                    class="menu-btn"
                    @click="clickMenu(item.meta.uId,child.meta.uId,child.path)"
                  >{{child.meta.title}}</el-button>
                  <i
                    class="star-icon"
                    :class="child.meta.star?'el-icon-star-on':'el-icon-star-off'"
                    @click="collect(item.meta.uId,child.meta.uId)"
                  ></i>
                </li>
              </ul>
            </div>
          </div>
          <div class="card-list-col">
            <div
              class="card-list-item"
              v-for="item in secondCol"
              :key="item.meta.uId"
              v-show="item.meta.show"
            >
              <div class="title">
                {{item.meta.title}}
                <a :ref="item.meta.title">&nbsp;</a>
              </div>
              <ul>
                <li v-for="child in item.children" :key="child.meta.uId" v-show="child.meta.show">
                  <el-button
                    class="menu-btn"
                    @click="clickMenu(item.meta.uId,child.meta.uId,child.path)"
                  >{{child.meta.title}}</el-button>
                  <i
                    class="star-icon"
                    :class="child.meta.star?'el-icon-star-on':'el-icon-star-off'"
                    @click="collect(item.meta.uId,child.meta.uId)"
                  ></i>
                </li>
              </ul>
            </div>
          </div>
          <div class="card-list-col">
            <div
              class="card-list-item"
              v-for="item in thirdCol"
              :key="item.meta.uId"
              v-show="item.meta.show"
            >
              <div class="title">
                {{item.meta.title}}
                <a :ref="item.meta.title">&nbsp;</a>
              </div>
              <ul>
                <li v-for="child in item.children" :key="child.meta.uId" v-show="child.meta.show">
                  <el-button
                    class="menu-btn"
                    @click="clickMenu(item.meta.uId,child.meta.uId,child.path)"
                  >{{child.meta.title}}</el-button>
                  <i
                    class="star-icon"
                    :class="child.meta.star?'el-icon-star-on':'el-icon-star-off'"
                    @click="collect(item.meta.uId,child.meta.uId)"
                  ></i>
                </li>
              </ul>
            </div>
          </div>
        </div>
        <div class="drawdot" ref="TheDrawdot">
          <ul>
            <li v-for="item in showList" :key="item.meta.uId" v-show="item.meta.show">
              <a href="javascript:;" @click="drowdotClick(item.meta.title)">{{item.meta.title}}</a>
            </li>
          </ul>
        </div>
      </div>
    </div>
    <div class="global-loading"></div>
  </div>
</template>

<script>
import { Logo, Navbar, Sidebar, AppMain, TagsView } from "./components";
import ResizeMixin from "./mixin/ResizeHandler";
import { ajaxRequest } from "./../utils/ajax";
import {
  setCollect,
  getCollect,
  removeCollect,
  setVisit,
  getVisit
} from "./../utils/menu";
import $ from "jquery";

export default {
  name: "Layout",
  data() {
    return {
      list: [],
      showList: [],
      visitList: [],
      searchValue: "",
      isShow: false,
      Recently_Visited: ""
    };
  },
  components: {
    Logo,
    Navbar,
    Sidebar,
    AppMain,
    TagsView
  },
  mixins: [ResizeMixin],
  computed: {
    sidebar() {
      return this.$store.state.app.sidebar;
    },
    device() {
      return this.$store.state.app.device;
    },
    classObj() {
      return {
        hideSidebar: !this.sidebar.opened,
        openSidebar: this.sidebar.opened,
        withoutAnimation: this.sidebar.withoutAnimation,
        mobile: this.device === "mobile"
      };
    },
    firstCol() {
      return this.showList.filter((value, index) => {
        if (index % 3 === 0) {
          return true;
        }
      });
    },
    secondCol() {
      return this.showList.filter((value, index) => {
        if (index % 3 === 1) {
          return true;
        }
      });
    },
    thirdCol() {
      return this.showList.filter((value, index) => {
        if (index % 3 === 2) {
          return true;
        }
      });
    }
  },
  watch: {
    classObj() {
      this.handleClass();
    }
  },
  methods: {
    handleClickOutside() {
      this.$store.dispatch("closeSideBar", { withoutAnimation: false });
    },
    showMenu() {
      if (!this.isShow) {
        this.clearTitleCss();
        this.isShow = true;
        $(this.$refs.AllMenus).animate({
          left: $(this.$refs.maincontainer).css("left")
        });
        this.$refs.TheSearhInput.focus();
      }
    },
    closeMenu() {
      this.isShow = false;
      this.$refs.TheSidebar.closeMenu();
      $(this.$refs.AllMenus).animate({ left: "-960px" });
    },
    deletCollect() {
      let self = this;
      this.closeMenu();
      this.$confirm("清空收藏吗？", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning"
      }).then(() => {
        removeCollect();
        let newList = self.list.filter((value, key, arr) => {
          if (value.children.length > 0) {
            let newchildren = value.children.filter((v, k, a) => {
              v.meta.star = false;
              return true;
            });
          }
          return true;
        });
        self.showList = newList;
        self.$refs.TheSidebar.delCollect();
      });
      // blocks.ui.dialog.confirm({
      //   content: "清空收藏吗？",
      //   title: "提示",
      //   btns: [
      //     {
      //       text: "确定",
      //       callback: () => {
      //         removeCollect();
      //         let newList = self.list.filter((value, key, arr) => {
      //           if (value.children.length > 0) {
      //             let newchildren = value.children.filter((v, k, a) => {
      //               v.meta.star = false;
      //               return true;
      //             });
      //           }
      //           return true;
      //         });
      //         self.showList = newList;
      //         self.$refs.TheSidebar.delCollect();
      //       }
      //     },
      //     {
      //       text: "取消",
      //       callback: () => {
      //         return;
      //       }
      //     }
      //   ],
      //   cancel: function() {
      //     return;
      //   }
      // });

      // this.$confirm("清空收藏吗？", "提示", {
      //   confirmButtonText: "确定",
      //   cancelButtonText: "取消",
      //   type: "warning",
      //   customClass:'delete-collect'
      // }).then(() => {
      //   removeCollect();
      //   let newList = this.list.filter((value, key, arr) => {
      //     if (value.children.length > 0) {
      //       let newchildren = value.children.filter((v, k, a) => {
      //         v.meta.star = false;
      //         return true;
      //       });
      //     }
      //     return true;
      //   });
      //   this.showList = newList;
      //   this.$message({
      //     type: "success",
      //     duration:'1000',
      //     message: "清空成功",
      //     onClose: () => {
      //       this.$refs.TheSidebar.delCollect();
      //     }
      //   });
      // });
    },
    searchChange() {
      let newValue = this.searchValue.trim();
      if (newValue == "" && this.searchValue.length != 0) {
        return;
      }
      this.clearTitleCss();
      let newList = this.list.filter((value, key, arr) => {
        if (value.children.length > 0) {
          let newchildren = value.children.filter((v, k, a) => {
            if (
              v.meta.title.toLowerCase().indexOf(newValue.toLowerCase()) != -1
            ) {
              v.meta.show = true;
              return true;
            } else {
              v.meta.show = false;
            }
          });
          if (newchildren.length > 0) {
            value.meta.show = true;
            return true;
          } else {
            value.meta.show = false;
            // if (value.meta.title.toLowerCase().indexOf(this.searchValue.toLowerCase()) != -1) {
            //   value.meta.show = true;
            //   // for(let i of value.children){
            //   //   i.meta.show = true;
            //   // }
            //   return true;
            // } else {
            //   value.meta.show = false;
            // }
          }
        }
      });
      this.showList = newList;
    },
    collect(pid, cid) {
      let li = this.list.find(n => n.meta.uId == pid);
      let lichild = li.children.find(n => n.meta.uId == cid);
      if (lichild != undefined) {
        lichild.meta.star = !lichild.meta.star;
        let myCollect = getCollect();
        if (lichild.meta.star) {
          myCollect.push({
            meta: {
              title: lichild.meta.title,
              uId: lichild.meta.uId,
              icon: lichild.meta.icon,
              star: lichild.meta.star,
              show: lichild.meta.show,
              pId: pid
            },
            path: lichild.path
          });
        } else {
          let index = -1;
          for (let i of myCollect) {
            index++;
            if (i.meta.uId == lichild.meta.uId) {
              break;
            }
          }
          myCollect.splice(index, 1);
        }
        setCollect(myCollect);
        this.$refs.TheSidebar.collect();
      }
    },
    clickMenu(pid, cid, path) {
      let li = this.list.find(n => n.meta.uId == pid);
      if (li != undefined) {
        let lichild = li.children.find(n => n.meta.uId == cid);
        if (lichild != undefined) {
          let visitList = getVisit();
          let temp = [
            {
              meta: {
                title: lichild.meta.title,
                uId: lichild.meta.uId,
                pId: li.meta.uId
              },
              path: lichild.path
            }
          ];
          let index = -1;
          for (let i of visitList) {
            index++;
            if (i.meta.uId == lichild.meta.uId) {
              visitList.splice(index, 1);
              break;
            }
          }
          let newTemp = temp.concat(visitList);
          if (newTemp.length > 6) {
            newTemp.splice(6, newTemp.length - 6);
          }
          setVisit(newTemp);
          this.visitList = newTemp;
        }
      }
      // let visitedViews = this.$store.getters.visitedViews;
      // let hasVisited = visitedViews.find(n => n.path == path);
      // if (hasVisited == undefined) {
      //   this.$router.push({ path: "/redirect" + path });
      // } else {
      //   this.$router.push({ path: path });
      // }
      this.$router.push({ path: path });
      this.isShow = false;
      this.$refs.TheSidebar.closeMenu();
      $(this.$refs.AllMenus).animate({ left: "-960px" });
    },
    getUserSingleRouter(userMenu) {
      var userRouter = [];
      var myCollect = getCollect();
      for (var i in userMenu) {
        var n = userMenu[i];
        var routerModel = {
          meta: {},
          children: []
        };
        userRouter.push(this.getChildren(n, routerModel, myCollect));
      }
      return userRouter;
    },
    getChildren(item, parentitem, myCollect) {
      var newRouter = parentitem;
      var itemsCount = item.Items.length;
      var icon = item.Icon;
      var pres = this.getChildComponent(item.Url, item.DisplayName, item.uId);
      if (itemsCount > 0) {
        newRouter.path = "/" + item.uId;
      } else {
        newRouter.path = "/" + pres.url;
      }
      this.$store.dispatch("SetUserMenus", {
        key: item.uId,
        url: "/" + item.Url
      });
      newRouter.component = pres.pcomponent;
      newRouter.meta.title = item.DisplayName;
      //newRouter.meta.star = true;
      newRouter.meta.show = true;
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
          childModel.path = "/" + cres.url;
          this.$store.dispatch("SetUserMenus", {
            key: m.uId,
            url: "/" + m.Url
          });
          childModel.component = cres.pcomponent;
          childModel.meta.title = m.DisplayName;
          childModel.meta.star = false;
          for (let i of myCollect) {
            if (i.meta.uId == m.uId) {
              childModel.meta.star = true;
              break;
            }
          }
          childModel.meta.show = true;
          if (cicon != null) {
            childModel.meta.icon = cicon;
          }
          childModel.meta.uId = m.uId;
          newRouter.children.push(childModel);
          this.getChildren(item.Items[j], newRouter.children[j], myCollect);
        }
      }
      return newRouter;
    },
    getChildComponent(url, title, uId) {
      var allRouter = this.$router.options.routes;
      var pcomponent;
      var ccomponent;
      let newUrl = url;
      var count = 0;
      let layout = "layout";
      let layoutJs = 0;
      let layoutComponent = allRouter.filter(n => n.name == layout);
      if (layoutComponent.length > 0) {
        let layoutChildren = [];
        for (let item of layoutComponent) {
          layoutChildren.splice(0, 0, ...item.children);
        }
        for (let item of layoutChildren) {
          let uniqueKey = item.uniqueKey;
          if (typeof item.children != "undefined") {
            for (let child of item.children) {
              uniqueKey = child.uniqueKey;
              if (uId == uniqueKey) {
                newUrl = layout + "/" + child.path + "/" + newUrl;
                layoutJs++;
                break;
              }
            }
          }
          if (layoutJs == 0) {
            if (uId == uniqueKey) {
              newUrl = layout + "/" + newUrl;
              layoutJs++;
              break;
            }
          } else {
            break;
          }
        }
      }
      if (layoutJs == 0) {
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
      }
      return {
        pcomponent: pcomponent,
        ccomponent: ccomponent,
        url: newUrl
      };
    },
    drowdotClick(title) {
      this.$refs[title][0].scrollIntoView(true);
      this.clearTitleCss();
      $(this.$refs[title][0])
        .parent()
        .css("color", "#ff8000");
    },
    clearTitleCss() {
      $(".card-list")
        .find(".card-list-col")
        .find(".card-list-item")
        .find(".title")
        .css("color", "");
    },
    handleClass() {
      let self = this;
      if (self.sidebar.opened) {
        var width = self.$children[1].$el.style.width;
        if (width == "") {
          width = "260px";
        }
        var intwidth = width.substring(0, width.indexOf("px"));
        // self.$children[0].$el.style.width = intwidth + "px";
        // self.$children[4].$el.style.width = "calc(100vw - " + intwidth + "px)";
        self.$refs.maincontainer.style.width =
          "calc(100vw - " + intwidth + "px)";
        self.$refs.maincontainer.style.left = intwidth + "px";
        //self.$refs.maincontainertop.style.left = intwidth + "px";
        let allMenusLeft = self.$refs.AllMenus.style.left;
        if (allMenusLeft != "") {
          let intLeft = allMenusLeft.substring(0, allMenusLeft.indexOf("px"));
          if (intLeft > 0) {
            self.$refs.AllMenus.style.left = intwidth + "px";
          }
        }
        self.$refs.TheSidebar.sideCloseMenu();
      } else {
        // self.$children[0].$el.style.width = "36px";
        // self.$children[4].$el.style.width = "calc(100vw - 36px)";
        self.$refs.maincontainer.style.width = "calc(100vw - 36px)";
        self.$refs.maincontainer.style.left = "36px";
        // self.$refs.maincontainertop.style.left = "36px";
        let allMenusLeft = self.$refs.AllMenus.style.left;
        if (allMenusLeft != "") {
          let intLeft = allMenusLeft.substring(0, allMenusLeft.indexOf("px"));
          if (intLeft > 0) {
            self.$refs.AllMenus.style.left = "36px";
          }
        }
        self.$refs.TheSidebar.sideOpenMenu();
      }
    },
    getUserMenu() {
      let arr = [];
      let list = this.list;
      for (let i of list) {
        for (let j of i.children) {
          arr.push(j);
        }
      }
      return arr;
    }
  },
  async created() {
    //let userMenu = [];
    // ajaxRequest({
    //   url: "/api/services/LayoutModule/SideBarNavigation/get",
    //   type: "post",
    //   success: function(res) {
    //     userMenu = res.content.Items;
    //   }
    // });
    let menusData = await this.$http({
      method: "get",
      url:
        "/api/services/navigation/navigation/GetCurrentUserNavigation?name=Main"
    });
    let userMenu = menusData.data.content;
    this.$store.dispatch("setMenus", userMenu);
    let userRouter = this.getUserSingleRouter(userMenu);
    this.list = userRouter;
    this.showList = userRouter;
  },
  mounted() {
    this.showList = Object.assign([], this.showList, this.list);
    this.visitList = getVisit();
    this.handleClass();
    this.$refs.tagView.addTags();
    this.$refs.TheSidebar.collect();
    this.Recently_Visited = this.$t("AuthentionModule.Recently_Visited");
  }
};
</script>

<style rel="stylesheet/scss" lang="scss">
.all-menus {
  .el-input {
    input {
      border: none;
      background: #f7f7f7;
      border-bottom: 1px solid #dcdcdc;
      border-radius: 0px;
      width: 80%;
    }
  }
}
.global-loading {
  .el-loading-mask {
    .el-loading-spinner i {
      color: rgb(4, 85, 156);
      font-size: 18px;
    }
  }
}
</style>

<style rel="stylesheet/scss" lang="scss" scoped>
@import "./../styles/mixin.scss";
.app-wrapper {
  @include clearfix;
  position: relative;
  height: 100%;
  width: 100%;
  &.mobile.openSidebar {
    position: fixed;
    top: 0;
  }
}
.global-loading {
  position: fixed;
  height: 25px;
  width: 50px;
  z-index: 6800;
  left: 50%;
  top: 30px;
  margin: 0 0 0 -25px;
}
.delete-collect-btn {
  background: none;
  padding: 0;
  box-shadow: none;
  border: none;
  position: absolute;
  top: 10px;
  right: 35px;
}
.close-menu-btn {
  background: none;
  padding: 0;
  box-shadow: none;
  border: none;
  position: absolute;
  top: 10px;
  right: 15px;
}
.menu-btn {
  border: none;
  border-radius: 0;
  background: #f7f7f7;
  box-shadow: none;
  padding: 0;
}
.star-icon {
  float: right;
  color: #ff8000;
  margin: 5px 0 0 0;
  font-size: 16px;
}
.drawer-bg {
  background: #000;
  opacity: 0.3;
  width: 100%;
  top: 0;
  height: 100%;
  position: absolute;
  z-index: 999;
}
.all-menus-bg {
  position: absolute;
  width: calc(100vw - 36px);
  background: rgba(0, 0, 0, 0.5);
  height: calc(100vh - 60px);
  left: 36px;
  top: 60px;
  z-index: 6800;
  overflow: hidden;
}
.all-menus {
  position: absolute;
  width: 960px;
  background: #f7f7f7;
  height: calc(100vh - 60px);
  //left: 260px;
  left: -960px;
  top: 60px;
  // padding: 30px 20px 60px 20px;
  z-index: 6800;
  border-right: 1px solid #dcdcdc;
  overflow: hidden;
  // overflow-y: auto;
  .all-menus-content {
    margin: 30px 20px 30px 0;
    padding: 0px 20px;
    overflow: hidden;
    overflow-y: auto;
    height: calc(100vh - 122px);
  }
}
.all-menus .visit {
  margin: 20px;
  overflow: hidden;
}
.all-menus .visit .title {
  font-weight: bold;
  font-size: 16px;
  margin-bottom: 10px;
}
.all-menus .visit ul {
  margin: 0;
  padding: 0;
}
.all-menus .visit ul li {
  list-style-type: none;
  float: left;
  width: 240px;
  line-height: 26px;
}
.card-list {
  margin: 15px 0;
  overflow: hidden;
}
.card-list .card-list-col {
  float: left;
  width: 240px;
}
.card-list .card-list-item {
  float: left;
  margin: 0px 20px;
  width: 200px;
  overflow: hidden;
}
.card-list .card-list-item .title {
  font-weight: bold;
  font-size: 16px;
  margin: 20px 0 15px 0;
}
.card-list .card-list-item ul {
  padding: 0;
  margin: 0;
}
.card-list .card-list-item ul li {
  cursor: pointer;
  line-height: 26px;
  list-style-type: none;
}
.all-menus .drawdot {
  position: absolute;
  right: 35px;
  top: 95px;
  // position: fixed;
  // top: 180px;
  // left: 1070px;
  width: 125px;
  padding: 5px 0 5px 15px;
  border-left: 1px solid #dcdcdc;
}
.all-menus .drawdot ul {
  padding: 0;
  margin: 0;
}
.all-menus .drawdot ul li {
  list-style-type: none;
  line-height: 22px;
}
.all-menus .drawdot ul li a {
  font-size: 12px;
  background: none;
}
</style>
