
import eleComponents  from "../../../../../../Blocks.ResourcesModule/FrameworkV2/index";
import { blocksConfig } from "./config";
import BaseComponent from "../BaseComponent";
import vue from "vue";
export default () => {

    let result = [];
    let vueComponents = eleComponents.components;
    for (const comKey in blocksConfig) {
        let vueComponent = vueComponents.find(c => c.name === comKey);
        let com = new BaseComponent(comKey, vue.extend(vueComponent), vueComponent);
        result.push({ name:comKey, component: com.getComponentOption(), blocks:com.getBlockOption })
    }
    return result;
}