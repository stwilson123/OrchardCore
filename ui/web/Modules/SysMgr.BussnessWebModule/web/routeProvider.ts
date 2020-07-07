
import { IRouteProvider } from "interface";

export class RouteProvider extends IRouteProvider {
    public getRoutes(): any[] {
        return [{
            path: 'ThirdSystemCallWeb/index',
            uniqueKey: "ThirdSystemCallWeb",
            layout: "layout",
            component: () => import('./src/views/ThirdSystemCallWeb/index.bl')
        },
        {
            path: 'ThirdSystemCallWeb/edit',
            uniqueKey: "ThirdSystemCallWeb_edit",
            layout: "layout",
            component: () => import('./src/views/ThirdSystemCallWeb/edit.bl')
        },
        {
            path: 'ThirdSystemTypeWeb/index',
            uniqueKey: "ThirdSystemTypeWeb",
            layout: "layout",
            component: () => import('./src/views/ThirdSystemTypeWeb/index.bl')
        },
        {
            path: 'ThirdSystemTypeWeb/add',
            uniqueKey: "ThirdSystemTypeWeb_add",
            layout: "layout",
            component: () => import('./src/views/ThirdSystemTypeWeb/add.bl')
        },
        {
            path: 'ThirdSystemTypeWeb/edit',
            uniqueKey: "ThirdSystemTypeWeb_edit",
            layout: "layout",
            component: () => import('./src/views/ThirdSystemTypeWeb/edit.bl')
        },
        {
            path: 'ProductElementType/index',
            uniqueKey: "ProductElementType",
            layout: "layout",
            component: () => import('./src/views/ProductElementType/index.bl')
        },
        {
            path: 'ProductElementType/add',
            uniqueKey: "ProductElementType_add",
            layout: "layout",
            component: () => import('./src/views/ProductElementType/add.bl')
        },
        {
            path: 'ProductElementType/edit',
            uniqueKey: "ProductElementType_edit",
            layout: "layout",
            component: () => import('./src/views/ProductElementType/edit.bl')
        },
        {
            path: 'Menu/Index',
            uniqueKey: "MenuWeb",
            layout: "layout",
            component: () => import('./src/views/Menu/index.bl')
        },
        {
            path: 'InterfaceAudit/Index',
            uniqueKey: "InterfaceAuditWeb",
            layout: "layout",
            component: () => import('./src/views/InterfaceAudit/index.bl')
        },
        {
            path: 'Languages/Index',
            uniqueKey: "LanguagesWeb",
            layout: "layout",
            component: () => import('./src/views/Languages/index.bl')
        },
        {
            path: 'ProductElement/index',
            uniqueKey: "ProductElement",
            layout: "layout",
            component: () => import('./src/views/ProductElement/index.bl')
        },
        {
            path: 'ProductElement/add',
            uniqueKey: "ProductElement_add",
            layout: "layout",
            component: () => import('./src/views/ProductElement/add.bl')
        },
        {
            path: 'ProductElement/edit',
            uniqueKey: "ProductElement_edit",
            layout: "layout",
            component: () => import('./src/views/ProductElement/edit.bl')
        },
        {
            path: 'SetupWeb/index',
            uniqueKey: "SetupWeb",
            layout: "layout",
            component: () => import('./src/views/SetupWeb/index.bl')
        },
        {
            path: 'SetupWeb/add',
            uniqueKey: "SetupWeb_add",
            layout: "layout",
            component: () => import('./src/views/SetupWeb/add.bl'),
        },
        {
            path: 'SetupWeb/edit',
            uniqueKey: "SetupWeb_edit",
            layout: "layout",
            component: () => import('./src/views/SetupWeb/edit.bl')
        },
        {
            path: 'DepartmentWeb/index',
            uniqueKey: "DepartmentWeb",
            layout: "layout",
            component: () => import('./src/views/DepartmentWeb/index.bl')
        },
        {
            path: 'EmployeeWeb/index',
            uniqueKey: "EmployeeWeb",
            layout: "layout",
            component: () => import('./src/views/EmployeeWeb/index.bl')
        },
        {
            path: 'SysUserInfo_New/index',
            uniqueKey: "SysUserInfo_New_Index",
            layout: "layout",
            component: () => import('./src/views/SysUserInfo_New/index.bl')
        },
        {
            path: 'SysRoleInfo_New/index',
            uniqueKey: "SysRoleInfo_New_Index",
            layout: "layout",
            component: () => import('./src/views/SysRoleInfo_New/index.bl')
        },
        {
            path: 'ProductFormat/index',
            uniqueKey: "ProductFormat",
            layout: "layout",
            component: () => import('./src/views/ProductFormat/index.bl')
        },
        {
            path: 'ProductFormat/detail',
            uniqueKey: "ProductFormat_detail",
            layout: "layout",
            component: () => import('./src/views/ProductFormat/detail.bl')
        },
        {
            path: 'Product4ElementRule/index',
            uniqueKey: "Product4ElementRule",
            layout: "layout",
            component: () => import('./src/views/Product4ElementRule/index.bl')
        },
        {
            path: 'Product4ElementRule/codingruleselection',
            uniqueKey: "Product4ElementRule_codingruleselection",
            layout: "layout",
            component: () => import('./src/views/Product4ElementRule/codingruleselection.bl')
        },
        {
            path: 'Product4VarElement/index',
            uniqueKey: "Product4VarElement",
            layout: "layout",
            component: () => import('./src/views/Product4VarElement/index.bl')
        },
        {
            path: 'Product4VarElement/variableselection.bl',
            uniqueKey: "Product4VarElement_variableselection",
            layout: "layout",
            component: () => import('./src/views/Product4VarElement/variableselection.bl')
        },
        // {
        //     path: 'QuestionFeedBackWeb/index',
        //     uniqueKey: "QuestionFeedBackWeb",
        //     layout: "layout",
        //     component: () => import('./src/views/QuestionFeedBackWeb/index.bl')
        // },
        {
            path: 'ConfigFilesWeb/index',
            uniqueKey: "ConfigFilesWeb",
            layout: "layout",
            component: () => import('./src/views/ConfigFilesWeb/index.bl')
        },
        // {
        //     path: 'QuestionFeedBackWeb/add',
        //     uniqueKey: "QuestionFeedBackWeb_add",
        //     layout: "layout",
        //     component: () => import('./src/views/QuestionFeedBackWeb/add.bl')
        // },
        {
            path: 'sapinterface/index',
            uniqueKey: "SAPInterfaceWeb",
            layout: "layout",
            component: () => import('./src/views/SAPInterface/index.bl')
        },
        {
            path: 'erpdata/index',
            uniqueKey: "ERPDataWeb",
            layout: "layout",
            component: () => import('./src/views/ERPData/index.bl')
        }]
    }
}