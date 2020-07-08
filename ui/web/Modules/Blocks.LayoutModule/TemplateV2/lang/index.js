import Vue from 'vue'
import VueI18n from 'vue-i18n'
import Cookies from 'js-cookie'
import elementEnLocale from 'element-ui/lib/locale/lang/en' // element-ui lang
import elementZhLocale from 'element-ui/lib/locale/lang/zh-CN'// element-ui lang
import enLocale from './en'
import zhLocale from './zh'
import { ajaxRequest } from './../utils/ajax'
import { localizationManager } from './../../../Blocks.ResourcesModule/FrameworkV2/Localization/localizationManager'
import store from './../store'

Vue.use(VueI18n)

const messages = {
  en: {
    ...enLocale,
    ...elementEnLocale
  },
  zh: {
    ...zhLocale,
    ...elementZhLocale
  }
}

var newMessages = {};
var lang = Cookies.get('language') || 'en';
ajaxRequest({
  //url: '/LayoutModule/layout/getLanguage',
  url: '/api/services/localization/localization/get',
  type: 'get',
  success: function (res) {
    var res = res.content;
    store.dispatch('setLanguage', res.currentLang);
    localStorage.setItem("BlCurrentLang", res.currentLang);
    store.dispatch('setLangs', res.allLang);
    lang = res.currentLang;
    res.data.forEach(e => {
      if (e.dics.length > 0) {
        var obj = {};
        obj[e.name] = {};
        e.dics.forEach(n => {
          obj[e.name][n.Name] = n.Value;
        })
        newMessages = Object.assign({}, newMessages, obj);
      }
    });
  }
});
var resMessage = {};
resMessage[lang] = newMessages;
localizationManager.registerSources(resMessage);
if (lang == 'zh-CN') {
  resMessage[lang] = Object.assign({}, resMessage[lang], elementZhLocale)
}
else {
  resMessage[lang] = Object.assign({}, resMessage[lang], elementEnLocale)
}
const i18n = new VueI18n({
  // set locale
  // options: en | zh | es
  locale: lang, //Cookies.get('language') || 'zh',
  // set locale messages
  //messages
  messages: resMessage
})

export default i18n
