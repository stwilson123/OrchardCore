<template>
  <div class="logo-container"></div>
</template>
<script>
export default {
  name: "Logo"
};
</script>
<style rel="stylesheet/scss" lang="scss" scoped>
$bigurl: "/Modules/Blocks.LayoutModule/dist/static/img/logo_big.png";
$smallurl: "/Modules/Blocks.LayoutModule/dist/static/img/logo_small.png";
.logo-container {
  transition: none;
  width: 260px;
  height: 60px;
  position: fixed;
  font-size: 0px;
  top: 0;
  left: 0;
  z-index: 6900;
  overflow: hidden;
  background: #0066b3 url($bigurl) 20px center no-repeat;
  background-size: 200px 36px;
}
.hideSidebar {
  .logo-container {
    width: 36px !important;
    background: #0066b3 url($smallurl) center center no-repeat;
    background-size: 80%;
  }
}
</style>