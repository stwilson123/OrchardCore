import { Controller, Component, Prop, asyncCompatible, catchWrap } from "interface"

@Component
export default class Edit extends Controller {
    constructor() {
        super();
    }
    @Prop({ type: Object }) formData;
    @Prop({ type: Object }) container;
    get mainHeight() {
        return this.container.height - this.$getRef("footer").outerHeight();
    }
    get gridHeight() {
        let paddingHeight = this.$getRef("theMain").outerHeight(true) - this.$getRef("theMain").height();
        return (this.mainHeight - this.$getRef("ruleForm").outerHeight() - paddingHeight);
    }
    ruleForm = {
    };
    rules = {
    }
    @asyncCompatible()
    async submitForm() {
        this.exit({ type: "success", message: "修改成功" });
    }
    closeDialog() {
        this.exit();
    }
    async viewWillEnter() {
    }
}