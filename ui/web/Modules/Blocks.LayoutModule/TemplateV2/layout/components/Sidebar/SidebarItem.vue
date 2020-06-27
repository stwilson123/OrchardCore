<template>
  <div v-if="!item.hidden" class="menu-wrapper">
    <template v-if="hasOneShowingChild(item.children,item) && (!onlyOneChild.children||onlyOneChild.noShowingChildren)&&!item.alwaysShow">
      <!--<app-link :to="resolvePath(onlyOneChild.path)"> -->
        <el-menu-item
          :index="resolvePath(onlyOneChild.path)"
          :class="{'submenu-title-noDropdown':!isNest}"
          @mouseover.native="mouseOver" 
          @mouseout.native="mouseOut"
          @click="clickMenu(onlyOneChild.meta.pId,onlyOneChild.meta.uId,resolvePath(onlyOneChild.path))"       
        >
          <item
            v-if="onlyOneChild.meta"
            :icon="onlyOneChild.meta.icon||(item.meta&&item.meta.icon)"
            :title="generateTitle(onlyOneChild.meta.title)"
          />
          <svg-icon icon-class="bl-close-white" class="menu-icon-close" @click="delClick(onlyOneChild.meta.pId,onlyOneChild.meta.uId)" v-show="closeShow" />
          <svg-icon icon-class="bl-drag-white" class="menu-icon-drag" v-show="closeShow" />
        </el-menu-item>
      <!--</app-link>-->
    </template>

    <el-submenu v-else ref="subMenu" :index="resolvePath(item.path)">
      <template slot="title">
        <item
          v-if="item.meta"
          :icon="item.meta && item.meta.icon"
          :title="generateTitle(item.meta.title)"
        />
      </template>
      <sidebar-item
        v-for="child in item.children"
        :is-nest="true"
        :item="child"
        :key="child.path"
        :base-path="child.path"
        class="nest-menu"
      />
    </el-submenu>
  </div>
</template>

<script>
import path from "path";
import { generateTitle } from "./../../../utils/i18n";
//import { isExternal } from "./../../../utils/validate";
import Item from "./Item";
import AppLink from "./Link";
import FixiOSBug from "./FixiOSBug";
import $ from 'jquery'

export default {
  name: "SidebarItem",
  components: { Item, AppLink },
  mixins: [FixiOSBug],
  props: {
    item: {
      type: Object,
      required: true
    },
    isNest: {
      type: Boolean,
      default: false
    },
    basePath: {
      type: String,
      default: ""
    }
  },
  data() {
    this.onlyOneChild = null;
    return {
      closeShow: false
    };
  },
  methods: {
    hasOneShowingChild(children = [], parent) {
      const showingChildren = children.filter(item => {
        if (item.hidden) {
          return false;
        } else {
          // Temp set(will be used if only has one showing child)
          this.onlyOneChild = item;
          return true;
        }
      });

      // When there is only one child router, the child router is displayed by default
      if (showingChildren.length === 1) {
        return true;
      }

      // Show parent if there are no child router to display
      if (showingChildren.length === 0) {
        this.onlyOneChild = { ...parent, path: "", noShowingChildren: true };
        return true;
      }

      return false;
    },
    resolvePath(routePath) {
      // if (isExternal(routePath)) {
      //   return routePath
      // }
      let repath = path.resolve(this.basePath, routePath);
      return repath;
    },
    clickMenu(pid, cid, path) {
      this.$parent.$parent.$parent.$parent.clickMenu(pid,cid,path);
      //this.$parent.$parent.$parent.$parent.closeMenu();
      //this.$router.push({ path: path });
    },
    mouseOver(){
      if(this.$store.getters.sidebar.opened){
        this.closeShow = true;
      }
    },
    mouseOut(){
      this.closeShow = false;
    },
    delClick(pid,uid) {
      this.$parent.$parent.$parent.$parent.collect(pid,uid);
      return false;
    },
    generateTitle
  }
  // mounted(){
  //   var parentEl = $(this.$el).parent()[0];
  //   this.$el.draggable = true;
  //   this.$el.flag = false;
  //   this.$el.ondragstart = ()=>{
	// 		this.$el.flag = true;
	// 	}
	// 	this.$el.ondragend = () =>{
	// 		this.$el.flag = false;
  //   }
  //   parentEl.ondragover = (e)=> {
  //     e.preventDefault();
  //   }
  //   parentEl.ondrop = (e)=> {
  //     let list = $(parentEl).find(".menu-wrapper");
  //     for(let i of list){
  //       if(i.flag){
  //         parentEl.appendChild(i);
  //       }
  //     }
  //   }
  // }
};
</script>
<style rel="stylesheet/scss" lang="scss">
.el-submenu__title i {
  color: #fff;
}
.menu-icon-close{
  width: 9px !important;
  height: 9px !important;
  margin: 0 !important;
  position: absolute;
  right: 30px;
  top: 16px;
  z-index: 10;
}
.menu-icon-drag{
  width: 10px !important;
  height: 10px !important;
  margin: 0 !important;
  position: absolute;
  right: 10px;
  top: 16px;
  z-index: 10;
  cursor: move;
}
</style>