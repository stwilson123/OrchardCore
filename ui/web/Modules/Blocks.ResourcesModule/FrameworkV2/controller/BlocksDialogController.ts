import Vue, { CreateElement, RenderContext, VNodeData } from "vue";
import Dialog from "../components/BlocksDialog.vue";
import { isFunction } from "util";
import $ from "jquery"
export default {
    name: 'BlocksDialogController',
    // functional: true,
    props: {
        component: {
            type: Object
        },
        componentProps: {
            type: Object
        },
        ...Dialog.props
    },
    data: function () {
        return {
            dismissResolve: undefined,
            loading: true,
            container: { height: 0 }
        }
    },
    methods: {
        onDidDismiss: function () {
            let _this = this;
            return new Promise(function (resolve) {
                //debugger
                _this.dismissResolve = resolve;
            });
        },
        exit: function (...args) {
            this.visible = false;
            if (this.dismissResolve)
                this.dismissResolve(...args);
        },
        beforeClose: function (resolve) {
            if (this.dismissResolve)
                this.dismissResolve();
            resolve();
        },
        viewReaderFinish(theVIewObj) {

            this.loading = false;
        },
        getContainerHeight(dialogObj) {
            //debugger
            let head = $(dialogObj.$el).find(".el-dialog__header:first-child");
            let height = $(dialogObj.$el).find(".el-dialog:first").innerHeight() - head.outerHeight();

            return height;
        },
        async dialogOpen() {
            await this.$nextTick();
            await this.$refs.dialogComponent.viewAnimationEnd();
            this.container.height = this.getContainerHeight(this.$refs.dialog);
        },
        resize() {
            this.container.height = this.getContainerHeight(this.$refs.dialog);
        }

    },
    mounted() {

        this.visible = true
        $(window).on("resize", this.resize);
    },
    beforeDestroy() {
        $(window).off("resize", this.resize);

    },
    render(h: CreateElement) {
        // if (props && !props.animationBuilder) {
        //     props.animationBuilder = animationBuilder.bind(parent);
        // }
        let component = this.$props.component;
        if (component.__esModule)
            component = component.default;
        if (this.$props.componentProps) {
            if (!this.$props.componentProps.container) {
                this.$props.componentProps.container = this.container;
            }
        }
        let isNeedLoading = !component.extendOptions || !component.extendOptions.methods || !component.extendOptions.methods.viewWillEnter || component.extendOptions.methods.viewWillEnter.toString().indexOf("_viewWillEnter.apply") > -1 || 
        component.extendOptions.methods.viewWillEnter.toString().indexOf(".apply(this,arguments)}") > -1;
        return h(Dialog, { props: this.$props, on: { beforeClose: this.beforeClose, 'blocks-open': this.dialogOpen }, ref: 'dialog', }, [h(component, {
            props: this.$props.componentProps, on: {
                exit: this.exit,
                viewReaderFinish: this.viewReaderFinish,

            },
            ref: 'dialogComponent',
            directives: [
                isNeedLoading === true ? {
                    name: 'loading',
                    value: this.loading,

                } : {},

            ],
        })]);
    }
};



export class BlocksDialogController {
    //  private _vue: any;

    private static _cacheDialog = [];
    private _vueObj: any;
    private _uniqueKey:string;
    constructor(vueObj: any,UniqueKey:string) {
        //debugger;`
        this._vueObj = vueObj;
        this._uniqueKey = UniqueKey;
    }

    async create(options: UiControllerOption): Promise<BlocksDialog> {

        if (!options)
            throw new Error("options shouldn't null.")
        options.vueObj = this._vueObj;
        if(options.componentProps && !options.componentProps.UniqueKey )
        {
            options.componentProps.UniqueKey = this._uniqueKey;
        }
        return await new BlocksDialog(options).create();
    }

}

export class BlocksDialog {

    private _dialogComponent: any;
    private _dialogObj: any;
    private _options: UiControllerOption;
    private _el: any;
    private _parentContainer: any;
    constructor(options: UiControllerOption) {
        // this._vue = vueObj;
        this._dialogComponent = Vue.component('BlDialogController');
        this._options = options;
    }

    async create(): Promise<BlocksDialog> {
        //debugger
        let defaultOptions: any = {
            visible: false,

        }
        if (typeof this._options.component === "function" && this._options.component.toString().indexOf(".then(") > -1) {
            defaultOptions.component = await this._options.component();
        }

        this._dialogObj = new this._dialogComponent({
            propsData: Object.assign(this._options, defaultOptions),
        });
        
        let parentContainer = this._options.getContainer;
        parentContainer = parentContainer ? (isFunction(parentContainer) ? parentContainer() : parentContainer) : (this._options.vueObj ? this._options.vueObj : document.body);

        if (!parentContainer.appendChild)
            throw new Error("ParentContainer isn't HtmlContainer")
        let el = document.createElement("div");
        this._parentContainer = parentContainer;
        this._el = parentContainer.appendChild(el);
        this._dialogObj.$mount(this._el);

        return this;
    }
    async present(): Promise<BlocksDialog> {
        this._dialogObj.visible = true;
        return this;
    }

    async dismiss(): Promise<BlocksDialog> {

        this._dialogObj.visible = false;
        await this._dialogObj.$nextTick();
        return this;
    }

    async onDidDismiss() {

        let result = await this._dialogObj.onDidDismiss();
        setTimeout(() => {
            this._dialogObj.$destroy();
            if (this._parentContainer) {
                this._parentContainer.removeChild(this._dialogObj.$el);
            }
        }, 200);


        return result;
    }
}



class UiControllerOption {

    component: any;
    componentProps: ComponentProps;
    getContainer: Function | HTMLElement;
    vueObj: any;
  
    visible: Boolean;
    title: String;
    width: String;
    fullscreen: Boolean;

    top: String;

    modal: Boolean;

    lockScroll: Boolean;


    closeOnClickModal: Boolean;
    closeOnPressEscape: Boolean;
    showClose: Boolean;
    beforeClose: Function;
    center: Boolean;
    destroyOnClose: Boolean;
    customClass: String;

}
class ComponentProps {
    propsData: any;
}
