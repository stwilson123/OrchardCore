import { ITemplateProvider, TemplateResult } from "interface";
import layout from "../TemplateV2/layout/Layout.vue"
export class LayoutTemplateProvider extends ITemplateProvider {
    public getTemplate(): TemplateResult[] {
        return [{
            name: "layout",
            path: "/layout",
            //component: layout,
            components: { default: layout },
            meta: {
                isTemplate: true
            }
        }];
    }
}