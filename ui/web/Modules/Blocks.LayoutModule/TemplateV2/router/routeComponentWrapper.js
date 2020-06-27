import { Bootstrapper, Controller } from "interface";
let newRoutes = Bootstrapper.RouteHelper.getRoute();

let componentWrap = (component, name) => {
    let sourceComponent = component();
    let asyncResult = async () => {


        let renderComponent = await sourceComponent;

        return Controller.extend({
            name: name,
            props: { container: { type: Object } },
            data: function () {
                return {
                    loading: true,
                    container: { height: 0 }
                }
            },
            methods:
            {
                viewDataReadyFinish(theVIewObj) {
                    // this.$emit("viewDataReadyFinish");
                    this.loading = false;
                },
                ViewDidEnterStart() {
                    this.$emit("ViewDidEnterStart");
                }
            },
            render: function (h) {
                let isNeedLoading = component.extendOptions && component.extendOptions.methods && component.extendOptions.methods.viewWillEnter && (component.extendOptions.methods.viewWillEnter.toString().indexOf("_viewWillEnter.apply") > -1 ||
                    component.extendOptions.methods.viewWillEnter.toString().indexOf(".apply(this,arguments)}") > -1);
                return h(renderComponent.default, {
                    props: this.$props,
                    on: {
                        // exit: this.exit,
                        viewDataReadyFinish: this.viewDataReadyFinish,
                        ViewDidEnterStart: this.ViewDidEnterStart
                    },
                    directives: [
                        isNeedLoading === true ? {
                            name: 'loading',
                            value: this.loading,

                        } : {},

                    ],
                })
            }
        })
    }

    return asyncResult;
}

let routeWrap = (route) => {
    if (route.noCache === true)
        return;
    if (route.children && Array.isArray(route.children)) {
        for (const routeChild of route.children) {
            routeWrap(routeChild);
        }
        return;
    }
    if (!route.component)
        return;
    let componentWrapper = componentWrap(route.component, route.name);

    route.component = componentWrapper;
    //return routes;
}
for (const route of newRoutes) {
    routeWrap(route);
}
let RouteWrapper = newRoutes;
export { RouteWrapper };


