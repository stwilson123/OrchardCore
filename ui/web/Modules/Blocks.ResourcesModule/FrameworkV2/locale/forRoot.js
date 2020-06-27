

let forRoot = (rootName,subObj) => {

    let rootObj = {} ;
    rootObj[rootName] = subObj;
    return rootObj
}

export { forRoot }