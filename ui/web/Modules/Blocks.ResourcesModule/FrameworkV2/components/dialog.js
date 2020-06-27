; define(['jquery', 'layer', 'blocks'], function ($, layer, blocks) {
    require('node_modules/layui-src/release/layer/dist/theme/default/layer.css');

    var { localizationManager, languageEnum } = require('../Localization/localizationManager');
    localizationManager.registerComponent(languageEnum.zhCn, function () {
        var l = require('./language/dialog/zh')
        dialogLang(l)
    });
    localizationManager.registerComponent(languageEnum.en, function () {
        var l = require('./language/dialog/en')
        dialogLang(l)
    });
    var clickSwitch = true;

    function dialogLang(l) {
        dialogOption.config.confirm.title = l.title;
        dialogOption.config.confirm.btn[0] = l.btn.yes;
        dialogOption.config.confirm.btn[1] = l.btn.cancel;

        dialogOption.config.info.title = l.title;
        dialogOption.config.success.title = l.title;
        dialogOption.config.warn.title = l.title;
        dialogOption.config.error.title = l.title;

        dialogOption.config.info.btn[0] = l.btn.yes;
        dialogOption.config.success.btn[0] = l.btn.yes;
        dialogOption.config.warn.btn[0] = l.btn.yes;
        dialogOption.config.error.btn[0] = l.btn.yes;
    }

    var dialogIdMap = {};
    var dialogOption = {
        config: {
            'default': {},
            info: {
                btn: ['yes']
            },
            success: {
                icon: 1,
                time: 3000,
                btn: ['yes']
            },
            warn: {
                icon: 0,
                btn: ['yes']
            },
            error: {
                icon: 2,
                btn: ['yes']
            },
            confirm: {
                icon: 3,
                title: 'Are you sure?',
                btn: ['yes', 'cancel']
            },
            dialog: {
                type: 1,
                title: "",
                offset: "auto",
                isMaxmin: true,
                area: ['80%', 'auto'],
                closeBtn: 0,
                content: '',
                cancel: function () {
                    return true;
                },
                end: function () {
                    // if (!ValidateHelper.isNullOrEmpty(settings.onEnd)) {
                    //     var returnData = ValidateHelper.isNullOrEmpty(returnValueFunction) ? null : returnValueFunction;
                    //     settings.onEnd(returnData);
                    // }
                },
                resize: true
            },
            loading: { type: 3, icon: 1, resize: !1, shade: .1 }
        }
    };

    var dialog = function (setting) {
        this.dialogIndex = setting.dialogIndex;
        this.passData = setting.passData;
        this.viewComponent = layer;
    };
    dialog.prototype = {
        close: function () {
            this.viewComponent.close(dialogIdMap[this.dialogIndex]);
            delete dialogIdMap[this.dialogIndex];
        },
        style: function (style) {
            this.viewComponent.style(style);
        },
        title: function (title) {
            this.viewComponent.title(title);
        }
    };
    /* MESSAGE **************************************************/
    var show = function (option) {
        if (!option){
            throw "option can't be null";
        } 
        $(".layui-layer-move").remove();
        var newOption = option;
        if(option.btns) {
            newOption = $.extend({}, newOption, true);
            newOption.btn = [];
            for (var i = 0; i < option.btns.length; i++) {
                var btnObj = newOption.btns[i];
                if (btnObj.text)
                    newOption.btn.push(btnObj.text);
                if (btnObj.callback) {              
                    (function(cb){
                        if(i === 0)
                            newOption['btn' + (i + 1)] = function (index) {
                                var result = cb.apply(this, arguments);
                                var returnValue = result ? result : true;
                                if(returnValue)
                                    layer.close(index);
                                return returnValue;
                            };
                        else
                            newOption['btn' + (i + 1)] = function (index) {
                                var result = cb.apply(this, arguments);
                                return result ? result : true;
                            };                        
                    })(btnObj.callback);                    
                }
            }
        }
        if (newOption.fixed) {
            if (!newOption.fixed) {
                newOption.fixed = false;
            }
        }
        if (newOption.dialogType) {
            if (newOption.dialogType == "loading") {
                newOption.zIndex = 30000000;
            }
        }
        
        // if (!newOption.title) {
        //     newOption.title = newOption.message;
        //     newOption.message = undefined;
        // }
        // newOption.title = newOption.message;
        if (!newOption.content)
            newOption.content = '';

        var succsesBack = {
            success: function (layero, index) {
                clickSwitch = true;
            }
        }
        var opts = $.extend(
            {},
            dialogOption.config['default'],
            dialogOption.config[option.dialogType],
            newOption,
            succsesBack
        );
        var index = layer.open(opts);
        return index;
    };

    var dialogUI = {};
    dialogUI.info = function (option) {
        return show($.extend(option, { dialogType: 'info' }));
    };

    dialogUI.success = function (option) {
        return show($.extend(option, { dialogType: 'success' }));
    };

    dialogUI.warn = function (option) {
        return show($.extend(option, { dialogType: 'warn' }));

    };

    dialogUI.error = function (option) {
        return show($.extend(option, { dialogType: 'warn' }));
    };

    dialogUI.confirm = function (option) {


        return show($.extend(option, { dialogType: 'confirm' }));
        // var userOpts = {
        //     text: message
        // };
        //
        // if ($.isFunction(titleOrCallback)) {
        //     callback = titleOrCallback;
        // } else if (titleOrCallback) {
        //     userOpts.title = titleOrCallback;
        // }
        // ;
        //
        // var opts = $.extend(
        //     {},
        //     abp.libs.sweetAlert.config['default'],
        //     abp.libs.sweetAlert.config.confirm,
        //     userOpts
        // );
        //
        // return $.Deferred(function ($dfd) {
        //     sweetAlert(opts, function (isConfirmed) {
        //         callback && callback(isConfirmed);
        //         $dfd.resolve(isConfirmed);
        //     });
        // });
    };

    function pathToRelative(path, modulePrefix, fileExtensionName) {
        var moduleFrefix = modulePrefix;
        var startIndex = path.indexOf(moduleFrefix);
        var endIndex = path.lastIndexOf(fileExtensionName);

        return path.slice(startIndex > -1 ? startIndex + moduleFrefix.length + '\\'.length : 0, endIndex > -1 ? endIndex : undefined);
    }

    dialogUI.dialog = function (option) {
        if(!clickSwitch){
            return;
        }
        clickSwitch = false;
        var pathHelper = require('path');
        var { moduleRoute } = require('moduleLoader');
        var Vue = require('vue');
        var url = window.location.hash.substring(1);
        url = pathHelper.resolve(url, '../');
        url = pathHelper.resolve(url, option.url);
        var passData = option.passData;
        var relativeUrl = pathHelper.relative('/', url);
        var moduleName = relativeUrl.substr(0, relativeUrl.indexOf('/'));
        var subUrl = pathHelper.relative(moduleName, relativeUrl);
        var WrapperId = ('' + Math.random()).replace('0.', '');
        var dialogContent = WrapperId + 'dialog';
        var dataWrapper = '<div id="' + WrapperId + '" style="height:100%"></div>';

        var curRoute = moduleRoute.getRoute(moduleName).find(elRoute => {
            return elRoute.path === subUrl;
        });
        curRoute.component(function (model) {
            var viewModel = model.viewModel;
            var currentModule = model.module;
            var endCallback = option.end;

            var dialogComponent = Vue.extend(viewModel);
            
            blocks.event.trigger('RoutePath.Set',{ routePath:"/"+relativeUrl});

            var component = new dialogComponent().$mount();
            var dialogDom = $(dataWrapper).append(component.$el);
            $(document.body).append(dialogDom);

            var layerIndex = show($.extend(option, {
                id: dialogContent, dialogType: 'dialog', content: dialogDom, end: function () {
                    var currentModuleResult;

                    try {
                        if (currentModule)
                            currentModuleResult = currentModule.displose();
                        component.$destroy();
                        dialogDom.remove();
                    }
                    finally {
                        if (endCallback)
                            endCallback(currentModuleResult);
                    }
                }
            }));
            dialogIdMap[dialogContent] = layerIndex;
        }, { moduleName: moduleName, currentPage: new dialog({ dialogIndex: dialogContent, passData: passData }) });
    };

    var loadingDep = 0, dialogIndex = null;

    dialogUI.loading =
        {
            open: function (option) {
                if (loadingDep < 1)
                    dialogIndex = show($.extend(option, { dialogType: 'loading' }));

                loadingDep++;
                return dialogIndex;

            },
            close: function () {
                if (--loadingDep < 1)
                    layer.close(dialogIndex);
            }
        };
    return dialogUI;

});