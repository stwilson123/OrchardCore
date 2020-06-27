<template>
  <div class="navbar">
    <hamburger
      :toggle-click="toggleSideBar"
      :is-active="sidebar.opened"
      class="hamburger-container"
    />
    <breadcrumb class="breadcrumb-container" />
    <div class="right-menu">
      <span class="factoryName" v-if="showFactory">{{ factoryName }}</span>
      <lang-select class="right-menu-item hover-effect" />
      <el-dropdown class="right-menu-item hover-effect" trigger="click">
        <span class="el-dropdown-link">
          <svg-icon icon-class="user-head"></svg-icon>
          {{ Name }}
          <i class="el-icon-arrow-down el-icon--right"></i>
        </span>
        <el-dropdown-menu slot="dropdown">
          <router-link :to="Url">
            <el-dropdown-item>{{ $t(Title) }}</el-dropdown-item>
          </router-link>
          <router-link v-if="modifyPasswordPageUrl === undefined" :to="modifyPasswordPageUrl">
            <el-dropdown-item>{{ $t(modifyPasswordPageName) }}</el-dropdown-item>
          </router-link>
          <el-dropdown-item divided>
            <span style="display:block;" @click="logout">{{ $t('AuthentionModule.LOGOUT') }}</span>
          </el-dropdown-item>
        </el-dropdown-menu>
      </el-dropdown>
      <template>
        <!-- <template v-if="device!=='mobile'"> -->
        <!-- <error-log class="errLog-container right-menu-item hover-effect"/>-->
        <!-- <screenfull class="right-menu-item hover-effect" />
        <screenfull class="right-menu-item hover-effect" />-->
        <screenfull class="right-menu-item hover-effect" style="margin-right:10px;" />
      </template>
    </div>
  </div>
</template>

<script>
import { mapGetters } from "vuex";
import Breadcrumb from "./../../components/Breadcrumb";
import Hamburger from "./../../components/Hamburger";
import ErrorLog from "./../../components/ErrorLog";
import Screenfull from "./../../components/Screenfull";
import LangSelect from "./../../components/LangSelect";

export default {
  data() {
    return {
      showFactory: false,
      factoryName: ""
    };
  },
  components: {
    Breadcrumb,
    Hamburger,
    ErrorLog,
    Screenfull,
    LangSelect
  },
  computed: {
    ...mapGetters(["sidebar", "name", "avatar", "device"]),
    Name() {
      return this.$store.getters.name;
    },
    Title() {
      return "AuthentionModule." + this.$store.getters.dashboardRoute.title;
    },
    Url() {
      return this.$store.getters.dashboardRoute.url;
    },
    modifyPasswordPageUrl() {
      return this.$store.getters.dashboardRoute.modifyPasswordPageUrl;
    },
    modifyPasswordPageName() {
      return (
        "AuthentionModule." +
        this.$store.getters.dashboardRoute.modifyPasswordPageName
      );
    }
  },
  methods: {
    toggleSideBar() {
      //var bodywidth = document.body.clientWidth;
      if (this.$store.getters.sidebar.opened) {
        //this.$parent.$children[4].$el.style.width = bodywidth - 36 + "px";
        //this.$parent.$refs.maincontainer.style.margin = "0px 0px 0px 36px";
        //this.$parent.$children[3].$el.style.margin = "0px 0px 0px 0px";
        //this.$parent.$children[3].$el.style.width = bodywidth - 36 + "px";
        //this.$parent.$children[2].$el.style.width = bodywidth - 36 + "px";
        //this.$parent.$children[2].$el.style.margin = "0px 0px 0px 0px";

        //this.$parent.$children[0].$el.style.width = "36px";
        //this.$parent.$children[4].$el.style.width = "calc(100vw - 36px)";
        //this.$parent.$refs.maincontainer.style.width = "calc(100vw - 36px)";
        //this.$parent.$refs.maincontainer.style.left = "36px";
        //this.$parent.$refs.maincontainertop.style.left = "36px";
        let allMenusLeft = this.$parent.$refs.AllMenus.style.left;
        if (allMenusLeft != "") {
          let intLeft = allMenusLeft.substring(0, allMenusLeft.indexOf("px"));
          if (intLeft > 0) {
            this.$parent.$refs.AllMenus.style.left = "36px";
          }
        }
        this.$parent.$refs.TheSidebar.sideOpenMenu();
        //this.$parent.$refs.AllMenus.style.left = "36px";
        //this.$parent.$refs.TheDrawdot.style.left = "846px";
      } else {
        var width = this.$parent.$children[1].$el.style.width;
        if (width == "") {
          width = "260px";
        }
        var intwidth = width.substring(0, width.indexOf("px"));
        // this.$parent.$children[0].$el.style.width = intwidth + "px";
        // this.$parent.$children[4].$el.style.width =
        //   "calc(100vw - " + intwidth + "px)";
        this.$parent.$refs.maincontainer.style.width =
          "calc(100vw - " + intwidth + "px)";
        // this.$parent.$refs.maincontainer.style.left = intwidth + "px";
        // this.$parent.$refs.maincontainertop.style.left = intwidth + "px";
        let allMenusLeft = this.$parent.$refs.AllMenus.style.left;
        if (allMenusLeft != "") {
          let intLeft = allMenusLeft.substring(0, allMenusLeft.indexOf("px"));
          if (intLeft > 0) {
            this.$parent.$refs.AllMenus.style.left = intwidth + "px";
          }
        }
        this.$parent.$refs.TheSidebar.sideCloseMenu();
        //this.$parent.$refs.AllMenus.style.left = intwidth + "px";
        //this.$parent.$refs.TheDrawdot.style.left =(810 + parseInt(intwidth)) +"px";
      }
      this.$store.dispatch("toggleSideBar");
    },
    logout() {
      this.$store.dispatch("LogOut").then(() => {
        location.reload();
      });
    }
  },
  mounted() {
    if (localStorage.getItem("bl-factoryShow")) {
      if (localStorage.getItem("bl-factoryShow") == "true") {
        this.showFactory = true;
      } else {
        this.showFactory = false;
      }
      if (this.showFactory) {
        if (localStorage.getItem("bl-factoryName")) {
          //let factory = JSON.parse(localStorage.getItem("bl-factoryKey"));
          this.factoryName = localStorage.getItem("bl-factoryName");
        }
      }
    }
  }
};
</script>

<style rel="stylesheet/scss" lang="scss" scoped>
.factoryName {
  line-height: 65px;
  float: left;
  margin-right: 15px;
}
.el-dropdown-link {
  font-size: 14px;
}
.navbar {
  height: 60px;
  overflow: hidden;
  position: relative;
  z-index: 9999;
  background: #fff;
  box-shadow: none;
  border-bottom: 1px solid #d9d9d9;
  margin-bottom: 0px;
  //background: #D6E0ED;
  .hamburger-container {
    line-height: 60px;
    height: 100%;
    float: left;
    cursor: pointer;
    transition: background 0.3s;
    &:hover {
      background: rgba(0, 0, 0, 0.025);
    }
  }

  .breadcrumb-container {
    float: left;
  }

  .errLog-container {
    display: inline-block;
    vertical-align: top;
  }

  .right-menu {
    float: right;
    height: 100%;
    line-height: 60px;

    &:focus {
      outline: none;
    }

    .right-menu-item {
      display: inline-block;
      padding: 0 8px;
      height: 100%;
      font-size: 18px;
      color: #5a5e66;
      vertical-align: text-bottom;

      &.hover-effect {
        cursor: pointer;
        transition: background 0.3s;

        &:hover {
          background: rgba(0, 0, 0, 0.025);
        }
      }
    }
  }
}
</style>
