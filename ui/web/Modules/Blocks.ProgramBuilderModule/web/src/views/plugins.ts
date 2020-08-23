import grapesjs from "grapesjs";
import panels from "./panels";
import styles from "./styles";
import components from "./components";
import "grapesjs/dist/css/grapes.min.css";
export default grapesjs.plugins.add('blocks-preset-webpage', (editor, opts = {}) => {
    let config = opts;
    // Load panels
    panels(editor, config);

    // Load styles
    styles(editor, config);

    components(editor,config);
});