<template>
  <section class="app-main" ref="section">
    <transition name="fade-transform" mode="out-in">
      <keep-alive :include="cachedViews">
      <!-- <keep-alive> -->
        <router-view
          :key="key"
          :container="container"
          
          @ViewDidEnterStart="ViewDidEnterStart"
          ref="theView"
        />
      </keep-alive>
    </transition>
  </section>
</template>

<script>
import { Loading } from "element-ui";
import $ from "jquery";
export default {
  name: "AppMain",
  data: function() {
    return {
      viewMap: new Map(),
      container: { height: 0 },
      viewTimes: 0,
      firstMounted: true
    };
  },
  methods: {
    // ViewDidEnterStart(theVIewObj) {
    //   if (theVIewObj) {
    //     let viewWillEnterFun = theVIewObj.$options.methods.viewWillEnter.toString();
    //     if (
    //       (viewWillEnterFun.indexOf("_viewWillEnter.apply") > -1 ||
    //         viewWillEnterFun.indexOf(".apply(this,arguments)}") > -1) &&
    //       !this.viewMap.has(theVIewObj.$el)
    //     ) {
    //       let el = document.createElement("div");
    //       el.style.height = this.$refs.theView.$el.offsetHeight + "px";
    //       el.style.width = this.$refs.theView.$el.offsetWidth + "px";
    //       el.style.position = "absolute";
    //       this.viewMap.set(theVIewObj.$el, {
    //         loadingService: Loading.service({
    //           target: theVIewObj.$el.appendChild(el)
    //         }),
    //         layout: el
    //       });
    //     }
    //   }
    // },
    // viewReaderFinish(theVIewObj) {
    //   if (this.viewMap.has(theVIewObj.$el)) {
    //     let viewLoading = this.viewMap.get(theVIewObj.$el);
    //     viewLoading.loadingService.close();
    //     viewLoading.layout.parentElement.removeChild(viewLoading.layout);
    //     this.viewMap.delete(theVIewObj.$el);
    //   }
    // },
    ViewDidEnterStart() {
      if (this.firstMounted) return;
      let containerHeight = this.getContainerHeight();
      let strHeight = containerHeight.toString();
      if (strHeight.indexOf(".") != -1) {
        let zero = "";
        for (let i = 0; i <= this.viewTimes; i++) {
          zero += "0";
        }
        this.container.height = parseFloat(strHeight + zero + "1");
      } else {
        this.container.height = parseFloat(strHeight + ".01");
      }
      if (this.viewTimes > 5) {
        this.viewTimes = 0;
      }
      this.viewTimes++;
    },
    getContainerHeight() {

      let height = $(".app-main:first")
        .children(":first")
        .height();
      return height;
    }
  },
  computed: {
    cachedViews() {
      return this.$store.state.tagsView.cachedViews;
    },
    key() {
      return this.$route.fullPath;
    }
  },
  beforeMount() {},
  async mounted() {
    this.container.height = this.getContainerHeight();
   // await this.$refs.theView.viewAnimationEnd();
    this.firstMounted = false;
    $(window).resize(() => {
      this.container.height = this.getContainerHeight();
    });
  }
};
</script>

<style scoped>
.app-main {
  /* min-height: calc(100vh - 103px); */
  /* height: calc(100vh - 103px); */
  width: 100%;
  position: relative;
  overflow: hidden;
  /* margin-top: 102px; */
  background: var(--global-background-color);
}
</style>
