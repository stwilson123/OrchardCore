import { componentConfig } from "./eleComponent/config";

export default class BaseComponent {
    comType: object;
    props: object;
    comName: string;
    constructor(comName: string, comType: object, props: object) {
        this.comType = comType;
        this.props = props;
        this.comName = comName;
    }


    public getComponentOption(): Object {
        let comType = this.comType;
        let comConfig = componentConfig[this.comName];
        let comOption = {
            // Make the editor understand when to bind `my-input-type`
            // isComponent: (el) => el.tagName === "SECTION",

            // Model definition
            model: Object.assign({
                // Default properties
                defaults: {
                    // tagName: "section",

                    // draggable: "form, form *", // Can be dropped only inside `form` elements
                    //droppable: false, // Can't drop other elements inside
                    attributes: {
                        // Default attributes
                        // type: "text",
                        // name: "default-name",
                        // placeholder: "Insert text here",
                    },
                    traits: this.createtraits(this.props),
                    //     components: `
                    //     <h1>Header test</h1>
                    //     <p>Paragraph test</p>
                    //   `,
                    //droppable: true
                },
                comType: comType,
                getCom() {

                    if (typeof this._com === "undefined") {
                        try {
                            this._com = new this.comType();
                            this._com.$mount();
                        }
                        catch
                        {
                            this._com = undefined;
                        }
                    }
                    return this._com;
                },
                _com: undefined,
                init() {
                    debugger
                    //this.on("change:attributes:msg", this.handlePropChange);
                    for (const trait of this.defaults.traits) {
                        this.on(`change:attributes:${trait.name}`, function () { this.handleTraitChange.call(this, ...arguments, trait.name) });
                    }

                    // this.com = new this.comType();
                    // this.com.$mount();
                    // debugger
                    // this.com.$el.style.outline = "inherit";
                    // this.com.$el.style.outlineOffset = "inherit";


                },
                handleTraitChange(model, traitValue, obj, traitName) {
                    debugger
                    this.getCom()[traitName] = traitValue;
                },
            }, comConfig.component),
            view: {
                //  el: hl.$el,
                isVue: true,
                el: function () {
                    debugger
                    return this.model && this.model.getCom ? this.model.getCom().$el : undefined;
                },
                renderAttributes: function () {
                    let cacheAttrs = {};
                    let styleAttrs = {};
                    let styleKeys = ["class", "style"];
                    for (const attr of this.el.attributes) {
                        let cacheArray = cacheAttrs;
                        if (styleKeys.includes(attr.nodeName))
                            cacheArray = styleAttrs;
                        cacheArray[attr.nodeName] = attr.value;
                    }
                    this.updateAttributes();

                    this.updateClasses();
                    this.$el.attr(cacheAttrs);

                    for (const styleAttrKey in styleAttrs) {
                        let tempAttr = this.$el.attr(styleAttrKey) || "";
                        tempAttr += " " + styleAttrs[styleAttrKey];
                        this.$el.attr(styleAttrKey, tempAttr);
                    }

                },
                init() {
                    ;
                    // Do something in view on model property change
                    // this.listenTo(model, "change:prop", this.handlePropChange);

                    // // If you attach listeners on outside objects remember to unbind
                    // // them in `removed` function in order to avoid memory leaks
                    // this.onDocClick = this.onDocClick.bind(this);
                    // document.addEventListener("click", this.onDocClick);
                    debugger

                },
                onRender() {


                    // this.model.com.$mount(this.el);
                    // this.el = this.model.com.$el;
                    // this.$el = $(this.model.com.$el);
                    //this.$el.append(this.model.com.$el)
                    //  this.$el.append(createMaskComponent())
                    // if (typeof comConfig.component === "undefined")
                    //     this.$el.append(this.model.com.$el)

                },
                //override updatecontent for clear inner html.
                updateContent() {

                }
                // getChildrenSelector: comConfig.component && comConfig.component.containerTagName ? function () {
                //     debugger
                //     if (!this.el.hasChildNodes())
                //         this.$el.append(this.model.com.$el);
                //     console.debug(this.model.containerTagName)
                //     console.debug(this.tagName())
                //     return this.model.containerTagName;
                // } : undefined,
            },
        };

        if (comConfig.component && comConfig.component.containerTagName) {
            comOption.view["getChildrenSelector"] = function () {
                return this.model.containerTagName;
            }
        }

        return comOption;
    }

    createtraits(props: object): Array<object> {
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

    public getBlockOption(): Object {
        let comConfig = componentConfig[this.comName];
        return Object.assign({
            content: {
                type: this.comName,
                // script: `debugger`,
                // style: {
                //     display: 'contents',

                // },
            },
            name:this.comName
        }, comConfig.blocks)
    }
}