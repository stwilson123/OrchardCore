<template>
  <div class="login-container">
    <el-form :model="loginForm" class="login-form" auto-complete="on" label-position="left">
      <div class="title-container">
        <img :src="logoUrl" style="width:370px;height:70px;" />
      </div>
      <div class="line"></div>
      <el-form-item v-if="showFactory">
        <bl-select
          class="factoryClass"
          v-model="factoryId"
          :options-data="factoryData"
          @blocks-change="selectFactory"
        ></bl-select>
      </el-form-item>
      <el-form-item prop="username">
        <div class="frame">
          <span class="svg-container">
            <svg-icon icon-class="user" />
          </span>
          <el-input
            v-model="loginForm.username"
            :placeholder="$t('AuthentionModule.ACCOUNT_NAME')"
            name="username"
            type="text"
            auto-complete="on"
          />
          <span class="show-pwd" @click="delUserName">
            <svg-icon icon-class="close" style="width:12px;height:12px;" />
          </span>
        </div>
      </el-form-item>
      <el-form-item prop="password">
        <div class="frame">
          <span class="svg-container">
            <svg-icon icon-class="password" />
          </span>
          <el-input
            v-model="loginForm.password"
            :type="passwordType"
            :placeholder="$t('AuthentionModule.PASSWORD')"
            name="password"
            auto-complete="on"
            @keyup.enter.native="handleLogin"
          />
          <span class="show-pwd" @click="showPwd">
            <svg-icon
              :icon-class="passwordType === 'password' ? 'eye-close' : 'eye-open'"
              style="width:16px;height:16px;"
            />
          </span>
        </div>
      </el-form-item>
      <el-form-item style="margin-bottom:12px;">
        <el-checkbox
          v-model="loginForm.remember"
          style="color:#fff;"
        >{{$t('AuthentionModule.REMEMBER_PASSWORD')}}</el-checkbox>
      </el-form-item>
      <el-button
        :loading="loading"
        type="primary"
        @click.native.prevent="handleLogin"
        class="btnlogin"
      >{{ $t('AuthentionModule.LOGIN') }}</el-button>
    </el-form>
    <div class="copyright" v-html="copyright"></div>
  </div>
</template>
<script>
export default {
  name: "Login",
  data() {
    const validateUsername = (rule, value, callback) => {
      callback();
    };
    const validatePassword = (rule, value, callback) => {
      callback();
    };
    return {
      loginForm: {
        username: "",
        password: "",
        remember: false
      },
      // rules: {
      //   username: [
      //     { required: true, trigger: "blur", message: "用户名不能为空" }
      //   ],
      //   password: [{ required: true, trigger: "blur", message: "密码不能为空" }]
      // },
      passwordType: "password",
      loading: false,
      showDialog: false,
      redirect: undefined,
      logoUrl: "/Modules/Blocks.LayoutModule/dist/static/img/logo_big.png",
      factoryId: "",
      factoryData: [],
      showFactory: false
    };
  },
  computed: {
    copyright() {
      return this.$t(
        "AuthentionModule." + this.$store.getters.dashboardRoute.copyRight
      );
    }
  },
  watch: {
    $route: {
      handler: function(route) {
        this.redirect = route.query && route.query.redirect;
      },
      immediate: true
    }
  },
  methods: {
    showPwd() {
      if (this.passwordType === "password") {
        this.passwordType = "text";
      } else {
        this.passwordType = "password";
      }
    },
    delUserName() {
      this.loginForm.username = "";
    },
    async handleLogin() {
      if (this.loginForm.username.trim() == "") {
        this.$message({
          type: "error",
          duration: 3000,
          message: this.$t("AuthentionModule.ACCOUNT_NAME_NULL")
        });
        return false;
      }
      if (this.loginForm.password.trim() == "") {
        this.$message({
          type: "error",
          duration: 3000,
          message: this.$t("AuthentionModule.PASSWORD_NULL")
        });
        return false;
      }
      this.loading = true;
      setTimeout(() => {
        this.loading = false;
      }, 2000);
      let res = await this.$store.dispatch("LoginByUsername", this.loginForm);
      if (this.loginForm.remember) {
        localStorage.setItem("blr2ewmsuid", this.loginForm.username);
        localStorage.setItem("blr2ewmspwd", this.loginForm.password);
      } else {
        if (localStorage.getItem("blr2ewmsuid")) {
          localStorage.removeItem("blr2ewmsuid");
        }
        if (localStorage.getItem("blr2ewmspwd")) {
          localStorage.removeItem("blr2ewmspwd");
        }
      }
      await this.$store.dispatch("GetUserInfo");
      if (this.redirect) {
        this.$router.push({ path: this.redirect || "/" });
      } else {
        this.$router.push({ path: this.$store.getters.dashboardRoute.url });
      }
      // this.$refs.loginForm.validate(valid => {
      //   if (valid) {
      //   } else {
      //     return false;
      //   }
      // });
    },
    selectFactory(val) {
      let factorys = this.factoryData.filter(n => n.id == val);
      window.location.href = factorys[0].path;
      //localStorage.setItem("bl-factoryKey", JSON.stringify(factorys[0]));
      //window.location.href = factorys[0].path + "?from=" + this.factoryId;
    }
  },
  async created() {
    let res = await this.$http({
      method: "post",
      url: "/api/services/LayoutModule/DashboardRoute/getfactory"
    });
    if (res.data.content) {
      let data = res.data.content;
      this.showFactory = data.onoff;
      if (data.onoff) {
        localStorage.setItem("bl-factoryShow", "true");
        this.factoryData = data.list;
        for (let f of this.factoryData) {
          if (f.isDefault) {
            this.factoryId = f.id;
            localStorage.setItem("bl-factoryName", f.text);
          }
        }
        // if (this.$route.query.from != undefined) {
        //   let path = window.location.href;
        //   let eqPath = path.substring(0, path.indexOf("?"));
        //   let factorys = this.factoryData.filter(n => n.path == eqPath);
        //   if (factorys.length > 0) {
        //     this.factoryId = factorys[0].id;
        //     localStorage.setItem("bl-factoryKey", JSON.stringify(factorys[0]));
        //   }
        // } else {
        //   if (localStorage.getItem("bl-factoryKey")) {
        //     let factory = JSON.parse(localStorage.getItem("bl-factoryKey"));
        //     let path = window.location.href;
        //     let eqPath = "";
        //     if (path.indexOf("?") == -1) {
        //       eqPath = path;
        //     } else {
        //       eqPath = path.substring(0, path.indexOf("?"));
        //     }
        //     let factorys = this.factoryData.filter(n => n.path == eqPath);
        //     if (eqPath != factory.path) {
        //       if (factorys.length > 0) {
        //         window.location.href = factory.path + "?from=" + factorys[0].id;
        //       }
        //     } else {
        //       this.factoryId = factorys[0].id;
        //       localStorage.setItem(
        //         "bl-factoryKey",
        //         JSON.stringify(factorys[0])
        //       );
        //     }
        //   } else {
        //     if (data.list.length > 0) {
        //       let path = window.location.href;
        //       let eqPath = "";
        //       if (path.indexOf("?") == -1) {
        //         eqPath = path;
        //       } else {
        //         eqPath = path.substring(0, path.indexOf("?"));
        //       }
        //       let factorys = this.factoryData.filter(n => n.path == eqPath);
        //       if (factorys.length > 0) {
        //         this.factoryId = factorys[0].id;
        //         localStorage.setItem(
        //           "bl-factoryKey",
        //           JSON.stringify(factorys[0])
        //         );
        //       }
        //     }
        //   }
        // }
      } else {
        localStorage.setItem("bl-factoryShow", "false");
      }
    }
  },
  mounted() {
    if (
      localStorage.getItem("blr2ewmsuid") &&
      localStorage.getItem("blr2ewmspwd")
    ) {
      this.loginForm.remember = true;
      this.loginForm.username = localStorage.getItem("blr2ewmsuid");
      this.loginForm.password = localStorage.getItem("blr2ewmspwd");
    }
  }
};
</script>

<style rel="stylesheet/scss" lang="scss">
$light_gray: #080808;
$cursor: #080808;

@supports (-webkit-mask: none) and (not (cater-color: $cursor)) {
  .login-container .el-input input {
    color: $cursor;
    // &::first-line {
    //   color: $light_gray;
    // }
    &::-webkit-input-placeholder {
      color: #c0c8cc;
    }
  }
}

/* reset element-ui css */
.login-container {
  .el-input {
    display: inline-block;
    height: 47px;
    width: 85%;
    input {
      background: transparent;
      border: 0px;
      -webkit-appearance: none;
      border-radius: 0px;
      padding: 12px 5px 12px 15px;
      color: #080808;
      height: 47px;
      caret-color: $cursor;
      &:-webkit-autofill {
        -webkit-text-fill-color: $cursor !important;
      }
    }
    input::-ms-clear,
    input::-ms-reveal {
      display: none;
    }
  }
  .el-form-item {
    // border: 1px solid rgba(255, 255, 255, 0.1);
    // background: rgba(0, 0, 0, 0.1);
    // border-radius: 5px;
    // color: #454545;
    height: 42px;
    margin-bottom: 27px;
  }
}
.factoryClass {
  background: #fff;
  border-radius: 21px;
  .el-input {
    width: 100%;
  }
}
</style>

<style rel="stylesheet/scss" lang="scss" scoped>
$bg: #2d3a4b;
$dark_gray: #889aa4;
$light_gray: #eee;

.login-container {
  min-height: 100%;
  width: 100%;
  //background-color: $bg;
  background: -webkit-radial-gradient(#0066b3, #005798, #005798);
  background: -o-radial-gradient(#0066b3, #005798, #005798);
  background: -moz-radial-gradient(#0066b3, #005798, #005798);
  background: radial-gradient(#0066b3, #005798, #005798);
  overflow: hidden;
  .login-form {
    width: 480px;
    //max-width: 100%;
    //padding: 160px 35px 0;
    //margin: 0 auto;
    overflow: hidden;
    position: absolute;
    height: 460px;
    top: 50%;
    left: 50%;
    margin: -230px 0px 0px -240px;
    .el-form-item {
      .frame {
        background: #fff;
        border-radius: 21px;
        height: 47px;
        border: 1px solid #fff;
        &:hover {
          border-color: #46a0f5;
        }
      }
    }
  }
  .tips {
    font-size: 14px;
    color: #fff;
    margin-bottom: 10px;
    span {
      &:first-of-type {
        margin-right: 16px;
      }
    }
  }
  .svg-container {
    padding: 6px 5px 6px 15px;
    color: $dark_gray;
    vertical-align: middle;
    width: 30px;
    display: inline-block;
  }
  .title-container {
    position: relative;
    .title {
      font-size: 26px;
      color: $light_gray;
      margin: 0px auto 40px auto;
      text-align: center;
      font-weight: bold;
    }
    .set-language {
      color: #fff;
      position: absolute;
      top: 3px;
      font-size: 18px;
      right: 0px;
      cursor: pointer;
    }
  }
  .show-pwd {
    position: absolute;
    right: 15px;
    top: 7px;
    font-size: 16px;
    color: $dark_gray;
    cursor: pointer;
    user-select: none;
  }
  .thirdparty-button {
    position: absolute;
    right: 0;
    bottom: 6px;
  }
}
.btnlogin {
  width: 100%;
  margin-bottom: 30px;
  height: 42px;
  background: rgba(70, 160, 245, 1);
  border-radius: 21px;
  border: none;
  &:hover {
    background: rgba(0, 124, 218, 1);
  }
}
.line {
  width: 475px;
  height: 1px;
  background: rgba(70, 160, 245, 1);
  opacity: 0.3;
  margin: 36px 0px;
}
.copyright {
  width: 600px;
  height: 20px;
  font-size: 12px;
  font-weight: 400;
  color: rgba(255, 255, 255, 1);
  opacity: 0.3;
  position: absolute;
  z-index: 100;
  bottom: 15px;
  text-align: center;
  left: 50%;
  margin-left: -300px;
}
</style>
