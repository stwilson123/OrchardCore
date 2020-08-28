import grapesjs from "grapesjs";
import panels from "./panels";
import styles from "./styles";
import components from "./components";
import "grapesjs/dist/css/grapes.min.css";
import { Console } from "console";
export default grapesjs.plugins.add('blocks-preset-webpage', (editor, opts = {}) => {
    let config = opts;
    // Load panels
    panels(editor, config);

    // Load styles
    styles(editor, config);

    components(editor,config);

    editor.on("component:add",function(){

        console.log("component:add");
    })

    // editor.on("component:add",function(){

    //     debugger
    // })
});