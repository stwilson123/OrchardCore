import { IRouteProvider } from "interface";

export class RouteProvider extends IRouteProvider {
    public getRoutes(): any[] {
        return [  {
            path: 'redirect/:path*',
           // name: 'redirect',
            layout: "layout",
            noCache:true,
            component: () => import('../TemplateV2/components/redirect/index.vue'),
     
        },
        {
            path: '/redirect/:path*',
            noCache:true,
            component: () => import('../TemplateV2/components/redirect/index.vue'),
     
        },
       ]
    }
}