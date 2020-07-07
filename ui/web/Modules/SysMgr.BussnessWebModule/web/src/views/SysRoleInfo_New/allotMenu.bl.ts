import { Controller, Component, asyncCompatible, Prop } from "interface"
@Component

export default class allotMenu extends Controller {
    @Prop({ type: Object }) formData;

    props = {
        label: 'name',
        children: 'children'
    }
    data = [];
    NodeKeys = [];
    closeDialog() {
        this.exit();
    }
    @asyncCompatible()
    async saveClick() {
        var NodeKeys = this.$refs.tree.getCheckedNodes();
        let sysPrograms = [];
        NodeKeys.forEach(function (val, index) {
            if (val.PID !== null && val.PID !== "") { //&& val.url != "" && val.url != null
                sysPrograms.push({
                    PID: val.PID,
                    ID: val.id,
                    URL: val.urlkey,
                    Type: val.type
                });
            }
        });
        let postData = {
            ID: this.formData.ID,
            SysProgramInfos: sysPrograms
        }
        let res = await this.$http({
            method: "post",
            url: "/api/services/SysMgrBussenssModule/SysRoleInfo/Allot",
            data: postData
        });
        if (res.data.code === "200") {
            this.exit({ type: "success", message: "分配菜单成功" });
        } else {
            this.$message({ type: "error", message: res.data.msg });
        }
    }
    async viewDidEnter() {
        let postData = {
            ID: this.formData.ID
        }
        let res = await this.$http({
            method: "post",
            url: "/api/services/SysMgrBussenssModule/SysRoleInfo/GetAllELSysProgram",
            data: postData
        });
        this.data = res.data.content.ELSysPogramTreeDatas;
        this.NodeKeys = res.data.content.TreeCheckedNodeIDs;
    }
}