


import componentOptionsFun from "./eleComponent";
let createComponent = (editor, option) => {

    editor.DomComponents.addType(option.name, option.component);
    let blockManager = editor.BlockManager;

    blockManager.add(option.name, option.blocks);

}


export default (editor, config) => {
    debugger
    let options = componentOptionsFun();
    for (const option of options) {
        createComponent(editor, option);
    }
};


