import { Controller, Component, Prop, asyncCompatible, catchWrap } from "interface"

import grapesjs from "grapesjs";

@Component
export default class Index extends Controller {

    viewDidEnter() {
        var editor = grapesjs.init({
            container: this.$refs.gjs,
            fromElement: true,
            noticeOnUnload: 0,
            plugins: ["blocks-preset-webpage"],
            panels: {},
            canvas: {
                styles: ['/pack/build/lib/component/web/webComponent_a4fe8be44daea154ed2f.css','/pack/build/lib/component/common/uidesigner.css'],
                
            },
            storageManager: {
                id: "",
                type: "remote",
                autosave: false,
                autoload: false,
                contentTypeJson: true,
                urlStore: `api/templates/${this.templateId}`,
                urlLoad: `api/templates/${this.templateId}`,
            },
            domComponents: {
                processor: (obj) => {
                    
                    if (obj.$$typeof) { // eg. this is a React Element
                        const compDef = {
                            type: obj.type,
                            components: obj.props.children,

                        };

                        return compDef;
                    }
                }
            }
        });


    }

}