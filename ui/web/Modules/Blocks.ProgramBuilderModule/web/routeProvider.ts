
import { IRouteProvider } from "interface";

export class RouteProvider extends IRouteProvider {
    public getRoutes(): any[] {
        return [{
            path: 'index',
            uniqueKey: "ThirdSystemCallWeb",
            //layout: "layout",
            component: () => import('./src/views/index.bl')
        }]
        
    }
}