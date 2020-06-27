import { BlocksModule } from 'interface'
import Vue from 'vue'
// import "core-js"
import blocksComponent from "../FrameworkV2/index"
import eventManager from "../FrameworkV2/event/event";
import "../FrameworkV2/exception/exceptionModule";
export class CurrentModule extends BlocksModule {
  public readonly moduleName = "ResourcesModule";
  constructor() {
    super();
  }
  preInitialize() {

    Vue.use(blocksComponent);


    Vue.config.errorHandler = function (err, vm, info) {
      eventManager.trigger("vueGlbalError", err, vm, info);
    }

  }
}