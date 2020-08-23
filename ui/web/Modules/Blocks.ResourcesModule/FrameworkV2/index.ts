
import BlocksInput from "./components/BlocksInput.vue"
import BlocksButton from "./components/BlocksButton.vue"
import BlocksSelect from "./components/BlocksSelect.vue"
import BlocksDatePicker from './components/BlocksDatePicker.vue'
import BlocksGrid from './components/BlocksGrid.vue'
import BlocksGridDatePicker from './components/BlocksGridDatePicker.vue'
import BlocksGridSelect from './components/BlocksGridSelect.vue'
import BlocksGridInput from './components/BlocksGridInput.vue'
import BlocksGridButton from './components/BlocksGridButton.vue'
import BlocksDialog from './components/BlocksDialog.vue'
import BlocksConfirm from './components/BlocksConfirm.vue'

import BlocksContainer from './layout/BlocksContainer.vue'
import BlocksAside from './layout/BlocksAside.vue'
import BlocksHeader from './layout/BlocksHeader.vue'
import BlocksMain from './layout/BlocksMain.vue'
import BlocksFooter from './layout/BlocksFooter.vue'
import BlocksTabs from './layout/BlocksTabs.vue'
import BlocksForm from './layout/BlocksForm.vue'
import BlocksFormItem from './layout/BlocksFormItem.vue'
import BlocksRow from './layout/BlocksRow.vue'
import BlocksCol from './layout/BlocksCol.vue'

import BlocksValidationObserver from './validate/BlocksValidationObserver.vue'
import BlocksValidationProvider from './validate/BlocksValidationProvider.vue'
import BlocksGridValidateDatePicker from './validate/BlocksGridValidateDatePicker.vue'
import BlocksGridValidateInput from './validate/BlocksGridValidateInput.vue'
import BlocksDialogController, { BlocksDialogController as DialogController } from "./controller/BlocksDialogController"
import { RefElement, refNullElement } from './controller/RefElement'
import { extend, localize, configure } from "vee-validate";
import * as rules from "vee-validate/dist/rules";

import { plugin } from 'vue-function-api';
import Vue from "vue";
import Element from "element-ui";
import 'element-ui/lib/theme-chalk/index.css'
const components = [
    { name: "BlInput", component: BlocksInput },
    { name: "BlButton", component: BlocksButton },

    { name: "BlSelect", component: BlocksSelect },
    { name: "BlDatepicker", component: BlocksDatePicker },
    { name: "BlGrid", component: BlocksGrid },
    { name: "BlGridSelect", component: BlocksGridSelect },
    { name: "BlGridInput", component: BlocksGridInput },
    { name: "BlGridDatepicker", component: BlocksGridDatePicker },
    { name: "BlGridButton", component: BlocksGridButton },
    { name: "BlDialog", component: BlocksDialog },
    { name: "BlConfirm", component: BlocksConfirm },
    { name: "BlContainer", component: BlocksContainer },
    { name: "BlAside", component: BlocksAside },
    { name: "BlHeader", component: BlocksHeader },
    { name: "BlMain", component: BlocksMain },
    { name: "BlFooter", component: BlocksFooter },
    { name: "BlTabs", component: BlocksTabs },
    { name: "BlForm", component: BlocksForm },
    { name: "BlFormItem", component: BlocksFormItem },
    { name: "BlRow", component: BlocksRow },
    { name: "BlCol", component: BlocksCol },
    { name: "BlValidationObserver", component: BlocksValidationObserver },
    { name: "BlValidationProvider", component: BlocksValidationProvider },
    { name: "BlGridValidateInput", component: BlocksGridValidateInput },
    { name: "BlGridValidateDatepicker", component: BlocksGridValidateDatePicker },
    { name: "BlDialogController", component: BlocksDialogController },

]


const install = function (Vue, opts = {}) {
    // locale.use(opts.locale);
    // locale.i18n(opts.i18n);

    for (let rule in rules) {
        extend(rule, rules[rule]);
    }
    //   import zh_CN from 'vee-validate/dist/locale/zh_CN.json';
    //   import en from 'vee-validate/dist/locale/en.json';
    //   if (localStorage.getItem("BlCurrentLang") == "en") {
    //     localize('en', en)
    //   }
    //   else {
    //     localize('zh_CN', zh_CN)
    //   }

    Vue.use(plugin)

    components.forEach(com => {
        Vue.component(com.name, com.component);
    });

    // Vue.use(InfiniteScroll);
    // Vue.use(Loading.directive);

    // Vue.prototype.$ELEMENT = {
    //     size: opts.size || '',
    //     zIndex: opts.zIndex || 2000
    // };

    Vue.prototype.$getRef = function (refName) {
        if (!this.$refs[refName] || !this.$refs[refName].$el)
            return refNullElement;
        return new RefElement(this.$refs[refName].$el);
    }
    Vue.prototype.$modal = () => new DialogController();
    Object.defineProperty(Vue.prototype, '$dialog', {
        get: function () {
            var container = this.$el;
            while (!(container != null && container.parentNode != null && container.parentNode.tagName === "SECTION" && container.parentNode.classList.contains("app-main"))) {
                container = container.parentNode;
            }
            return new DialogController(container,this.UniqueKey);
        }
    });
};


/* istanbul ignore if */
if (typeof window !== 'undefined' && window.Vue) {
    install(window.Vue);
}


export default {
    // version: '2.13.0',
    // locale: locale.use,
    // i18n: locale.i18n,
    install,
    BlocksInput,
    BlocksButton,
    BlocksSelect,
    BlocksDatePicker,
    BlocksGrid,
    BlocksDialog,
    BlocksConfirm,
    BlocksContainer,
    BlocksAside,
    BlocksHeader,
    BlocksMain,
    BlocksFooter,
    BlocksTabs,
    BlocksForm,
    BlocksFormItem,
    BlocksRow,
    BlocksCol,
    BlocksValidationObserver,
    BlocksValidationProvider,
    BlocksDialogController,
    components,
    Element
};