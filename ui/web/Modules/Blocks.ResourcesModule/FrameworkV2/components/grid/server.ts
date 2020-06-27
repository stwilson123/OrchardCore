
export function getServerColumns(props, column, gridConfig, editingCellValidate) {
    let columnDefsDefalut = [];
    let hasUniqueKey = false;
    for (let item of column) {
        let colLen = column.filter(n => n.field == item.field).length;
        if (colLen > 1) {
            console.error("grid列重复");
        }
        let i = Object.assign({}, item);
        if (i.blType) {
            delete i.type;
            if (i.blType == "select") {
                i = selectColunm(i, gridConfig);
            } else if (i.blType == "date") {
                i = dateColunm(i);
            } else if (i.blType == "text") {
                i = textColunm(i);
            } else if (i.blType == "int") {
                i = intColunm(i);
            } else if (i.blType == "float") {
                i = floatColunm(i);
            }
            if (i.blParams) {
                if (i.blParams.rules) {
                    i.suppressKeyboardEvent = async params => {
                        var e = params.event;
                        if (e.which == 13) {
                            let allCols = gridConfig.columnApi.getAllColumns();
                            let validate = false;
                            for (let col of allCols) {
                                if (col.colDef.cellEditorFramework) {
                                    if (col.colDef.cellEditorFramework == "BlGridValidateInput" || col.colDef.cellEditorFramework == "BlGridValidateDatepicker") {
                                        let field = col.colDef.field;
                                        let comp = gridConfig.columnApi.getColumn(field).colDef.comp;
                                        if (comp) {
                                            validate = await comp.getValidate();
                                            if (!validate) {
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            if (validate) {
                                gridConfig.api.stopEditing();
                            }
                        }
                    }
                }
                if (i.blType == "text" || i.blType == "int" || i.blType == "float") {
                    if (!i.blParams["blocks-input"]) {
                        i.blParams["blocks-input"] = async (val, id, comp) => {
                            //let newrowEditingData = Object.assign([], rowEditingData.value);
                            // let rows = newrowEditingData.filter(n => n[props.uniqueKey] == id);
                            // if (rows.length > 0) {
                            //     let row = Object.assign({}, rows[0]);
                            //     let coLField = i.field;
                            //     row[coLField] = val;
                            //     let updateIndex = 0;
                            //     newrowEditingData.map((item, index) => {
                            //         if (item[props.uniqueKey] == row[props.uniqueKey]) {
                            //             updateIndex = index;
                            //         }
                            //     });
                            //     newrowEditingData.splice(updateIndex, 1, row);
                            //     rowEditingData.value = newrowEditingData;
                            // }
                            let validate = await comp.getValidate();
                            let newIndex = 0;
                            if (!validate) {
                                let validateRows = editingCellValidate.value.filter((n, index) => {
                                    if (n.id == id) {
                                        newIndex = index;
                                        return true;
                                    }
                                });
                                if (validateRows.length > 0) {
                                    let validateCells = validateRows[0].cells;
                                    let cellIndex = 0;
                                    let cells = validateCells.filter((n, index) => {
                                        if (n.colKey == i.field) {
                                            cellIndex = index;
                                            return true;
                                        }
                                    });
                                    let cellObj = {
                                        colKey: i.field,
                                        validate: validate
                                    }
                                    if (cells.length > 0) {
                                        validateCells.splice(cellIndex, 1, cellObj);
                                    }
                                    else {
                                        validateCells.push(cellObj);
                                    }
                                    let rowObj = {
                                        id: id,
                                        cells: validateCells
                                    }
                                    editingCellValidate.value.splice(newIndex, 1, rowObj);
                                } else {
                                    let rowObj = {
                                        id: id,
                                        cells: [{
                                            colKey: i.field,
                                            validate: validate
                                        }]
                                    }
                                    editingCellValidate.value.push(rowObj);
                                }
                            } else {
                                let validateRows = editingCellValidate.value.filter((n, index) => {
                                    if (n.id == id) {
                                        newIndex = index;
                                        return true;
                                    }
                                });
                                if (validateRows.length > 0) {
                                    let validateCells = validateRows[0].cells;
                                    let cellIndex = 0;
                                    let cells = validateCells.filter((n, index) => {
                                        cellIndex = index;
                                        if (n.colKey == i.field) {
                                            return true;
                                        }
                                    });
                                    if (cells.length > 0) {
                                        validateCells.splice(cellIndex, 1);
                                    }
                                    let rowObj = {
                                        id: id,
                                        cells: validateCells
                                    }
                                    editingCellValidate.value.splice(newIndex, 1, rowObj);
                                }
                            }
                        }
                    }
                }
                // else if (i.blType == "select") {
                //     if (!i.blParams["blocks-change"]) {
                //         i.blParams["blocks-change"] = (val, id, comp) => {
                //             let newrowEditingData = Object.assign([], rowEditingData.value);
                //             let rows = newrowEditingData.filter(n => n[props.uniqueKey] == id);
                //             if (rows.length > 0) {
                //                 let row = Object.assign({}, rows[0]);
                //                 let coLField = i.field;
                //                 row[coLField] = val;
                //                 let updateIndex = 0;
                //                 newrowEditingData.map((item, index) => {
                //                     if (item[props.uniqueKey] == row[props.uniqueKey]) {
                //                         updateIndex = index;
                //                     }
                //                 });
                //                 newrowEditingData.splice(updateIndex, 1, row);
                //                 rowEditingData.value = newrowEditingData;
                //             }
                //         }
                //     }
                // }
                // else if (i.blType == "date") {
                //     if (i.blParams.config) {
                //         if (!i.blParams.config["blocks-change"]) {
                //             i.blParams.config["blocks-change"] = (a, val, c, id, comp) => {
                //                 let newrowEditingData = Object.assign([], rowEditingData.value);
                //                 let rows = newrowEditingData.filter(n => n[props.uniqueKey] == id);
                //                 if (rows.length > 0) {
                //                     let row = Object.assign({}, rows[0]);
                //                     let coLField = i.field;
                //                     row[coLField] = val;
                //                     let updateIndex = 0;
                //                     newrowEditingData.map((item, index) => {
                //                         if (item[props.uniqueKey] == row[props.uniqueKey]) {
                //                             updateIndex = index;
                //                         }
                //                     });
                //                     newrowEditingData.splice(updateIndex, 1, row);
                //                     rowEditingData.value = newrowEditingData;
                //                 }
                //             }
                //         }
                //     }
                // }
            }
        }
        columnDefsDefalut.push(i);
        if (item.field == props.uniqueKey) {
            hasUniqueKey = true;
        }
    }
    if (!hasUniqueKey) {
        console.error("grid uniqueKey不存在");
    }
    return columnDefsDefalut;
}
function selectColunm(i, gridConfig) {
    i.cellEditorFramework = "BlGridSelect";
    i.cellEditorParams = i.blParams;
    if (i.filter) {
        if (i.blFilterType) {
            if (i.blFilterType == "int") {
                i.filter = "agNumberColumnFilter";
                i.filterParams = {
                    applyButton: true,
                    clearButton: true,
                    hideFilterInput: false,
                    suppressAndOrCondition: true,
                    filterOptions: getfilterOptions(i.blFilterType)
                };
            } else if (i.blFilterType == "string") {
                i.filter = "agTextColumnFilter";
                i.filterParams = {
                    applyButton: true,
                    clearButton: true,
                    hideFilterInput: false,
                    suppressAndOrCondition: true,
                    filterOptions: getfilterOptions(i.blFilterType)
                };
            } else {
                i.filterFramework = "BlGridSelect";
                i.filterParams = i.blParams;
                i.floatingFilterComponentFramework = "BlGridSelect";
                let ffcp = Object.assign(
                    {},
                    {
                        suppressFilterButton: true,
                        api: gridConfig.api
                    },
                    i.blFilterParams == undefined ? i.blParams : i.blFilterParams
                );
                i.floatingFilterComponentParams = ffcp;
            }
        } else {
            i.filterFramework = "BlGridSelect";
            i.filterParams = i.blParams;
            i.floatingFilterComponentFramework = "BlGridSelect";
            let ffcp = Object.assign(
                {},
                {
                    suppressFilterButton: true,
                    api: gridConfig.api
                },
                i.blFilterParams == undefined ? i.blParams : i.blFilterParams
            );
            i.floatingFilterComponentParams = ffcp;
        }
    }
    if (i.displayTextCol) {
        i.valueFormatter = p => {
            try {
                return p.data[i.displayTextCol] ? p.data[i.displayTextCol] : "";
            } catch (err) {
                return "";
            }
        };
    }
    else {
        if (i.blParams) {
            if (i.blParams.optionsData) {
                let itemValue = "id";
                let itemText = "text";
                if (i.blParams.itemValue) {
                    itemValue = i.blParams.itemValue;
                }
                if (i.blParams.itemText) {
                    itemText = i.blParams.itemText;
                }
                i.valueFormatter = p => {
                    try {
                        let selectData = i.blParams.optionsData.filter(
                            n => n[itemValue] == p.value
                        );
                        return selectData[0][itemText];
                    } catch (err) {
                        //console.error("select数据格式有误");
                        return "";
                    }
                };
            }
        }
    }
    if (i.blDataType) {
        if (i.blDataType == "string") {
            i.valueSetter = p => {
                p.data[i.field] = p.newValue;
                return true;
            };
        } else if (i.blDataType == "int") {
            i.valueSetter = p => {
                if (p.data[i.field] !== p.newValue) {
                    try {
                        let newValue = parseInt(p.newValue);
                        if (newValue.toString() == "NaN") {
                            return false;
                        } else {
                            p.data[i.field] = newValue;
                        }
                    } catch (err) {
                        return false;
                    }
                    return true;
                } else {
                    return false;
                }
            };
        }
    }
    return i;
}
function dateColunm(i) {
    if (i.blParams) {
        if (i.blParams.rules) {
            i.cellEditorFramework = "BlGridValidateDatepicker";
        }
        else {
            i.cellEditorFramework = "BlGridDatepicker";
        }
    }
    else {
        i.cellEditorFramework = "BlGridDatepicker";
    }
    i.cellEditorParams = i.blParams;
    if (i.filter) {
        i.filter = "agDateColumnFilter";
        i.filterParams = {
            applyButton: true,
            clearButton: true,
            hideFilterInput: false,
            suppressAndOrCondition: true,
            filterOptions: [
                {
                    displayKey: "contains",
                    displayName: "Contains",
                    test: (filterValue, cellValue) => {
                        if (cellValue == null) {
                            return false;
                        }
                        let fyear = filterValue.getFullYear().toString();
                        let fmonth = filterValue.getMonth().toString();
                        let fday = filterValue.getDate().toString();
                        let cyear = new Date(cellValue).getFullYear().toString();
                        let cmonth = new Date(cellValue).getMonth().toString();
                        let cday = new Date(cellValue).getDate().toString();
                        return fyear + fmonth + fday == cyear + cmonth + cday
                            ? true
                            : false;
                    }
                },
                {
                    displayKey: "inRange",
                    displayName: "In Range",
                    test: (filterValue, cellValue) => {
                        return true;
                    }
                }
            ],
            comparator: (filterLocalDateAtMidnight, cellValue) => {
                if (cellValue == null) return 0;
                let cellDate = new Date(cellValue);
                if (cellDate < filterLocalDateAtMidnight) {
                    return -1;
                } else if (cellDate > filterLocalDateAtMidnight) {
                    return 1;
                } else {
                    return 0;
                }
            }
        };
    }
    if (i.blParams) {
        if (i.blParams.config) {
            if (i.blDataType) {
                if (i.blDataType == "string") {
                    i.valueSetter = p => {
                        p.data[i.field] = p.newValue ? blocks.utility.dateConvert.format(
                            new Date(p.newValue),
                            i.blParams.config.dateFmt
                        ) : "";
                        return true;
                    };
                }
            }
            i.valueFormatter = p => {
                try {
                    return p.value ? blocks.utility.dateConvert.format(
                        new Date(p.value),
                        i.blParams.config.dateFmt
                    ) : "";
                } catch (e) {
                    return p.value;
                }
            };
        }
    }
    return i;
}
function textColunm(i) {
    if (i.blParams) {
        if (i.blParams.rules) {
            i.cellEditorFramework = "BlGridValidateInput";
        }
        else {
            i.cellEditorFramework = "BlGridInput";
        }
    }
    else {
        i.cellEditorFramework = "BlGridInput";
    }
    i.cellEditorParams = i.blParams;
    if (i.filter) {
        i.filter = "agTextColumnFilter";
        i.filterParams = {
            applyButton: true,
            clearButton: true,
            hideFilterInput: false,
            suppressAndOrCondition: true,
            filterOptions: getfilterOptions(i.blType)
        };
    }
    i.valueSetter = p => {
        p.data[i.field] = p.newValue;
        return true;
    };
    return i;
}
function intColunm(i) {
    if (i.blParams) {
        if (i.blParams.rules) {
            i.cellEditorFramework = "BlGridValidateInput";
        }
        else {
            i.cellEditorFramework = "BlGridInput";
        }
    }
    else {
        i.cellEditorFramework = "BlGridInput";
    }
    i.cellEditorParams = i.blParams;
    if (i.filter) {
        i.filter = "agNumberColumnFilter";
    }
    i.filterParams = {
        applyButton: true,
        clearButton: true,
        hideFilterInput: false,
        suppressAndOrCondition: true,
        filterOptions: getfilterOptions(i.blType)
    };
    i.valueSetter = p => {
        if (p.data[i.field] !== p.newValue) {
            try {
                let newValue = parseInt(p.newValue);
                if (newValue.toString() == "NaN") {
                    return false;
                } else {
                    p.data[i.field] = newValue;
                }
            } catch (err) {
                return false;
            }
            return true;
        } else {
            return false;
        }
    };
    return i;
}
function floatColunm(i) {
    if (i.blParams) {
        if (i.blParams.rules) {
            i.cellEditorFramework = "BlGridValidateInput";
        }
        else {
            i.cellEditorFramework = "BlGridInput";
        }
    }
    else {
        i.cellEditorFramework = "BlGridInput";
    }
    i.cellEditorParams = i.blParams;
    if (i.filter) {
        i.filter = "agNumberColumnFilter";
    }
    i.filterParams = {
        applyButton: true,
        clearButton: true,
        hideFilterInput: false,
        suppressAndOrCondition: true,
        filterOptions: getfilterOptions(i.blType)
    };
    i.valueSetter = p => {
        if (p.data[i.field] !== p.newValue) {
            try {
                let newValue = parseFloat(p.newValue);
                if (newValue.toString() == "NaN") {
                    return false;
                } else {
                    p.data[i.field] = newValue;
                }
            } catch (err) {
                return false;
            }
            return true;
        } else {
            return false;
        }
    };
    return i;
}
function getfilterOptions(type) {
    let filterOptions = [];
    switch (type) {
        case "int":
        case "float":
            filterOptions = [
                {
                    displayKey: "equals",
                    displayName: "Equals",
                    test: (filterValue, cellValue) => {
                        return true;
                    }
                },
                {
                    displayKey: "inRange",
                    displayName: "In Range",
                    test: (filterValue, cellValue) => {
                        return true;
                    }
                },
                {
                    displayKey: "lessThan",
                    displayName: "Less Than",
                    test: (filterValue, cellValue) => {
                        return true;
                    }
                },
                {
                    displayKey: "greaterThan",
                    displayName: "Greater Than",
                    test: (filterValue, cellValue) => {
                        return true;
                    }
                },
                {
                    displayKey: "lessThanOrEqual",
                    displayName: "Less Than Or Equal",
                    test: (filterValue, cellValue) => {
                        return true;
                    }
                },
                {
                    displayKey: "greaterThanOrEqual",
                    displayName: "Greater Than Or Equal",
                    test: (filterValue, cellValue) => {
                        return true;
                    }
                }
            ];
            break;
        case "text":
        case "string":
            filterOptions =
                [{
                    displayKey: "contains",
                    displayName: "Contains",
                    test: (filterValue, cellValue) => {
                        return true;
                    }
                }, {
                    displayKey: "equals",
                    displayName: "Equals",
                    test: (filterValue, cellValue) => {
                        return true;
                    }
                }, {
                    displayKey: "notEqual",
                    displayName: "NotEqual",
                    test: (filterValue, cellValue) => {
                        return true;
                    }
                }];
            break;
    }
    return filterOptions;
}