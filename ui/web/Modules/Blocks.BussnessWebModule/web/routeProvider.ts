import { IRouteProvider } from "interface";

export class RouteProvider extends IRouteProvider {
    public getRoutes(): any[] {
        return [{
            path: 'masterdata/index',
            uniqueKey: "MasterData",
            layout: "layout",
            //name: 'masterdata_index',
            component: () => import('./src/views/masterdata/index.bl'),
            // meta: {
            //     title: "样板一"
            // }
        },
        {
            path: 'masterdata/index1',
            uniqueKey: "MasterData1",
            layout: "layout",
            component: () => import('./src/views/masterdata/index1.bl'),
            meta: {
                title: "样板二"
            }
        }, {
            path: 'masterdata/layoutdemo',
            uniqueKey: "layoutdemo",
            layout: "layout",
            component: () => import('./src/views/masterdata/layoutdemo.bl'),
            meta: {
                title: "布局样板"
            }
        },
        {
            path: 'masterdata/add',
            uniqueKey: "MasterDataAdd",
            layout: "layout",
            //name: 'masterdata_index',
            component: () => import('./src/views/masterdata/add.bl'),
        },]
    }
}
