


import vueComponents from "../../../../../Blocks.ResourcesModule/FrameworkV2/index";
import vue from "vue";
import { hyphenate } from "../utils/strHelper";
import $ from "jquery";
let components = vueComponents.components;
let createComponent = (editor, vueComponent) => {

    let { name, component } = vueComponent;
    let comName = name;
    let tagName = hyphenate(name);
    let comConfig = componentConfig[comName];
    let comType = vue.extend(component);
    debugger
    editor.DomComponents.addType(tagName, {
        // Make the editor understand when to bind `my-input-type`
        isComponent: (el) => el.tagName === "SECTION",

        // Model definition
        model: Object.assign({
            // Default properties
            defaults: {
                tagName: tagName,

                // draggable: "form, form *", // Can be dropped only inside `form` elements
                //droppable: false, // Can't drop other elements inside
                attributes: {
                    // Default attributes
                    type: "text",
                    name: "default-name",
                    placeholder: "Insert text here",
                },
                traits: createtraits(component.props),
                //     components: `
                //     <h1>Header test</h1>
                //     <p>Paragraph test</p>
                //   `,
                droppable: true
            },
            comType: comType,
            com: null,
            init() {
                debugger;
                //this.on("change:attributes:msg", this.handlePropChange);

                this.com = new this.comType();
                this.com.$mount();

            },
            handlePropChange(model, trait) {
                this.com.msg = trait.msg;
                debugger
            },
        }, comConfig.component),
        view: {
            //  el: hl.$el,
            init() {
                debugger;
                // Do something in view on model property change
                // this.listenTo(model, "change:prop", this.handlePropChange);

                // // If you attach listeners on outside objects remember to unbind
                // // them in `removed` function in order to avoid memory leaks
                // this.onDocClick = this.onDocClick.bind(this);
                // document.addEventListener("click", this.onDocClick);

            },
            onRender() {
                // this.$el.remove();
                // this.$el = $(this.model.com.$el);


                // this.model.com.$mount(this.el);
                // this.el = this.model.com.$el;
                // this.$el = $(this.model.com.$el);
                //this.$el.append(this.model.com.$el)
                //  this.$el.append(createMaskComponent())
            },
            getChildrenSelector:comConfig.component && comConfig.component.containerTagName ? function() {
                debugger
                this.$el.append(this.model.com.$el);
                console.debug(this.model.containerTagName)
                console.debug(this.tagName())
                return this.model.containerTagName || this.tagName();
            }:undefined,
        },
    });
    let blockManager = editor.BlockManager;

     
    blockManager.add(tagName, Object.assign({
        content: {
            type: tagName,
        },
    }, comConfig.blocks));
}


let componentConfig = {
    "BlInput": {

        blocks: {
            label: 'Input',
            category: 'Basic',
        }
    },
    "BlButton": {
        blocks: {
            label: 'Button',
            category: 'Basic',
        }
    },
    "BlSelect": {
        blocks: {
            label: 'Select',
            category: 'Basic',
        }
    },
    "BlDatepicker": {
        blocks: {
            label: 'Datepicker',
            category: 'Basic',
        }
    },
    "BlGrid": {
        blocks: {
            label: 'Grid',
            category: 'Basic',
        }
    },
    "BlDialog": {
        blocks: {
            label: 'Dialog',
            category: 'Layout',
        }
    },
    "BlContainer": {
        blocks: {
            label: 'Container',
            category: 'Layout',
        },
        component: {
            containerTagName: 'section'
        }
    },
    "BlAside": {
        blocks: {
            label: 'Aside',
            category: 'Layout',
        },
        component: {
            containerTagName: 'aside'
        }
    },
    "BlHeader": {
        blocks: {
            label: 'Header',
            category: 'Layout',
        },
        component: {
            containerTagName: 'header'
        }
    },
    "BlMain": {
        blocks: {
            label: 'Main',
            category: 'Layout',
        },
        component: {
            containerTagName: 'main'
        }
    },
    "BlFooter": {
        blocks: {
            label: 'Footer',
            category: 'Layout',
        },
        component: {
            containerTagName: 'footer'
        }
    },
    "BlTabs": {
        blocks: {
            label: 'Tabs',
            category: 'Layout',
        }
    },
    "BlForm": {
        blocks: {
            label: 'Form',
            category: 'Layout',
        }
    },
    "BlFormItem": {
        blocks: {
            label: 'FormItem',
            category: 'Layout',
        }
    },
    "BlRow": {
        blocks: {
            label: 'Row',
            category: 'Layout',
        }
    },
    "BlCol": {
        blocks: {
            label: 'Row',
            category: 'Layout',
        }
    },
    "BlValidationObserver": {
        blocks: {
            label: 'ValidationObserver',
            category: 'Layout',
        }
    },
    "BlValidationProvider": {
        blocks: {
            label: 'ValidationProvider',
            category: 'Layout',
        }
    },
}

let createtraits = (props: object): Array<object> => {
    let result = [];
    for (let pName in props) {
        let pType: string;
        let p = props[pName];
        switch (p.type) {
            case Boolean: p = "checkbox"; break;
            case Number: p = "number"; break;
            default: p = "text"; break;

        }
        result.push({
            type: p,
            name: pName,
            label: pName,
        })
    }


    return result;
}

export default (editor, config) => {

    for (const com of components) {
        if (componentConfig[com.name] !== undefined)
            createComponent(editor, com);
    }

};


