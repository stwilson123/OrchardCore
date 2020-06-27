import { BlocksModule } from 'interface'
// import { moduleLoader, moduleRoute } from "moduleLoader";

// moduleLoader.Create({
//   provider: () => require.context("../src/Views", true, /\.(js|html|htm|vue)$/),
//   moduleName: 'AuthentionModule'
// }).Init();

// moduleRoute.addRoute('login', { component: (resolve) => resolve(require('../src/views/login')) });
export class CurrentModule extends BlocksModule {
  public readonly moduleName = "AuthentionModule";
  constructor() {
    super();
  }
}