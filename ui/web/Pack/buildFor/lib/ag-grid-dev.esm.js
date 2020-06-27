/**
 * @ag-grid-enterprise/all-modules - Advanced Data Grid / Data Table supporting Javascript / React / AngularJS / Web Components * @version v23.0.2
 * @link http://www.ag-grid.com/
' * @license Commercial
 */

import { _, Events, RowNode, Constants, GridOptionsWrapper, ChangedPath, Autowired, Optional, PostConstruct, Bean, ModuleNames, RowNodeBlock, Qualifier, RowNodeCache, RowNodeBlockLoader, NumberSequence, PreDestroy, BeanStub, GroupInstanceIdCreator, ColumnGroup, RefSelector, PreConstruct, Component, AgCheckbox, CssClassApplier, Column, DragSourceType, TouchListener, OriginalColumnGroup, DragAndDropService, VirtualList, HorizontalDirection, VerticalDirection, ModuleRegistry } from '@ag-grid-community/core';

var ClientSideNodeManager = /** @class */ (function () {
    function ClientSideNodeManager(rootNode, gridOptionsWrapper, context, eventService, columnController, gridApi, columnApi, selectionController) {
        this.nextId = 0;
        // when user is provide the id's, we also keep a map of ids to row nodes for convenience
        this.allNodesMap = {};
        this.rootNode = rootNode;
        this.gridOptionsWrapper = gridOptionsWrapper;
        this.context = context;
        this.eventService = eventService;
        this.columnController = columnController;
        this.gridApi = gridApi;
        this.columnApi = columnApi;
        this.selectionController = selectionController;
        this.rootNode.group = true;
        this.rootNode.level = -1;
        this.rootNode.id = ClientSideNodeManager.ROOT_NODE_ID;
        this.rootNode.allLeafChildren = [];
        this.rootNode.childrenAfterGroup = [];
        this.rootNode.childrenAfterSort = [];
        this.rootNode.childrenAfterFilter = [];
        // if we make this class a bean, then can annotate postConstruct
        this.postConstruct();
    }
    // @PostConstruct - this is not a bean, so postConstruct called by constructor
    ClientSideNodeManager.prototype.postConstruct = function () {
        // func below doesn't have 'this' pointer, so need to pull out these bits
        this.getNodeChildDetails = this.gridOptionsWrapper.getNodeChildDetailsFunc();
        this.suppressParentsInRowNodes = this.gridOptionsWrapper.isSuppressParentsInRowNodes();
        this.doesDataFlower = this.gridOptionsWrapper.getDoesDataFlowerFunc();
        this.isRowMasterFunc = this.gridOptionsWrapper.getIsRowMasterFunc();
        this.doingLegacyTreeData = _.exists(this.getNodeChildDetails);
        this.doingMasterDetail = this.gridOptionsWrapper.isMasterDetail();
        if (this.getNodeChildDetails) {
            console.warn("ag-Grid: the callback nodeChildDetailsFunc() is now deprecated. The new way of doing\n                                    tree data in ag-Grid was introduced in v14 (released November 2017). In the next\n                                    major release of ag-Grid we will be dropping support for the old version of\n                                    tree data. If you are reading this message, please go to the docs to see how\n                                    to implement Tree Data without using nodeChildDetailsFunc().");
        }
    };
    ClientSideNodeManager.prototype.getCopyOfNodesMap = function () {
        var result = _.cloneObject(this.allNodesMap);
        return result;
    };
    ClientSideNodeManager.prototype.getRowNode = function (id) {
        return this.allNodesMap[id];
    };
    ClientSideNodeManager.prototype.setRowData = function (rowData) {
        this.rootNode.childrenAfterFilter = null;
        this.rootNode.childrenAfterGroup = null;
        this.rootNode.childrenAfterSort = null;
        this.rootNode.childrenMapped = null;
        this.nextId = 0;
        this.allNodesMap = {};
        if (!rowData) {
            this.rootNode.allLeafChildren = [];
            this.rootNode.childrenAfterGroup = [];
            return;
        }
        // kick off recursion
        // we add rootNode as the parent, however if using ag-grid-enterprise, the grouping stage
        // sets the parent node on each row (even if we are not grouping). so setting parent node
        // here is for benefit of ag-grid-community users
        var result = this.recursiveFunction(rowData, this.rootNode, ClientSideNodeManager.TOP_LEVEL);
        if (this.doingLegacyTreeData) {
            this.rootNode.childrenAfterGroup = result;
            this.setLeafChildren(this.rootNode);
        }
        else {
            this.rootNode.allLeafChildren = result;
        }
    };
    ClientSideNodeManager.prototype.updateRowData = function (rowDataTran, rowNodeOrder) {
        if (this.isLegacyTreeData()) {
            return null;
        }
        var rowNodeTransaction = {
            remove: [],
            update: [],
            add: []
        };
        this.executeAdd(rowDataTran, rowNodeTransaction);
        this.executeRemove(rowDataTran, rowNodeTransaction);
        this.executeUpdate(rowDataTran, rowNodeTransaction);
        if (rowNodeOrder) {
            _.sortRowNodesByOrder(this.rootNode.allLeafChildren, rowNodeOrder);
        }
        return rowNodeTransaction;
    };
    ClientSideNodeManager.prototype.executeAdd = function (rowDataTran, rowNodeTransaction) {
        var _this = this;
        var add = rowDataTran.add, addIndex = rowDataTran.addIndex;
        if (!add) {
            return;
        }
        var useIndex = typeof addIndex === 'number' && addIndex >= 0;
        if (useIndex) {
            // items get inserted in reverse order for index insertion
            add.reverse().forEach(function (item) {
                var newRowNode = _this.addRowNode(item, addIndex);
                rowNodeTransaction.add.push(newRowNode);
            });
        }
        else {
            add.forEach(function (item) {
                var newRowNode = _this.addRowNode(item);
                rowNodeTransaction.add.push(newRowNode);
            });
        }
    };
    ClientSideNodeManager.prototype.executeRemove = function (rowDataTran, rowNodeTransaction) {
        var _this = this;
        var remove = rowDataTran.remove;
        if (!remove) {
            return;
        }
        var rowIdsRemoved = {};
        var anyNodesSelected = false;
        remove.forEach(function (item) {
            var rowNode = _this.lookupRowNode(item);
            if (!rowNode) {
                return;
            }
            if (rowNode.isSelected()) {
                anyNodesSelected = true;
            }
            // do delete - setting 'tailingNodeInSequence = true' to ensure EVENT_SELECTION_CHANGED is not raised for
            // each row node updated, instead it is raised once by the calling code if any selected nodes exist.
            rowNode.setSelected(false, false, true);
            // so row renderer knows to fade row out (and not reposition it)
            rowNode.clearRowTop();
            // NOTE: were we could remove from allLeaveChildren, however _.removeFromArray() is expensive, especially
            // if called multiple times (eg deleting lots of rows) and if allLeafChildren is a large list
            rowIdsRemoved[rowNode.id] = true;
            // _.removeFromArray(this.rootNode.allLeafChildren, rowNode);
            delete _this.allNodesMap[rowNode.id];
            rowNodeTransaction.remove.push(rowNode);
        });
        this.rootNode.allLeafChildren = this.rootNode.allLeafChildren.filter(function (rowNode) { return !rowIdsRemoved[rowNode.id]; });
        if (anyNodesSelected) {
            this.selectionController.updateGroupsFromChildrenSelections();
            var event_1 = {
                type: Events.EVENT_SELECTION_CHANGED,
                api: this.gridApi,
                columnApi: this.columnApi
            };
            this.eventService.dispatchEvent(event_1);
        }
    };
    ClientSideNodeManager.prototype.executeUpdate = function (rowDataTran, rowNodeTransaction) {
        var _this = this;
        var update = rowDataTran.update;
        if (!update) {
            return;
        }
        update.forEach(function (item) {
            var rowNode = _this.lookupRowNode(item);
            if (!rowNode) {
                return;
            }
            rowNode.updateData(item);
            rowNodeTransaction.update.push(rowNode);
        });
    };
    ClientSideNodeManager.prototype.addRowNode = function (data, index) {
        var newNode = this.createNode(data, this.rootNode, ClientSideNodeManager.TOP_LEVEL);
        if (_.exists(index)) {
            _.insertIntoArray(this.rootNode.allLeafChildren, newNode, index);
        }
        else {
            this.rootNode.allLeafChildren.push(newNode);
        }
        return newNode;
    };
    ClientSideNodeManager.prototype.lookupRowNode = function (data) {
        var rowNodeIdFunc = this.gridOptionsWrapper.getRowNodeIdFunc();
        var rowNode;
        if (_.exists(rowNodeIdFunc)) {
            // find rowNode using id
            var id = rowNodeIdFunc(data);
            rowNode = this.allNodesMap[id];
            if (!rowNode) {
                console.error("ag-Grid: could not find row id=" + id + ", data item was not found for this id");
                return null;
            }
        }
        else {
            // find rowNode using object references
            rowNode = _.find(this.rootNode.allLeafChildren, function (rowNode) { return rowNode.data === data; });
            if (!rowNode) {
                console.error("ag-Grid: could not find data item as object was not found", data);
                return null;
            }
        }
        return rowNode;
    };
    ClientSideNodeManager.prototype.recursiveFunction = function (rowData, parent, level) {
        var _this = this;
        // make sure the rowData is an array and not a string of json - this was a commonly reported problem on the forum
        if (typeof rowData === 'string') {
            console.warn('ag-Grid: rowData must be an array, however you passed in a string. If you are loading JSON, make sure you convert the JSON string to JavaScript objects first');
            return;
        }
        var rowNodes = [];
        rowData.forEach(function (dataItem) {
            var node = _this.createNode(dataItem, parent, level);
            rowNodes.push(node);
        });
        return rowNodes;
    };
    ClientSideNodeManager.prototype.createNode = function (dataItem, parent, level) {
        var node = new RowNode();
        this.context.wireBean(node);
        var doingTreeData = this.gridOptionsWrapper.isTreeData();
        var doingLegacyTreeData = !doingTreeData && _.exists(this.getNodeChildDetails);
        var nodeChildDetails = doingLegacyTreeData ? this.getNodeChildDetails(dataItem) : null;
        if (nodeChildDetails && nodeChildDetails.group) {
            node.group = true;
            node.childrenAfterGroup = this.recursiveFunction(nodeChildDetails.children, node, level + 1);
            node.expanded = nodeChildDetails.expanded === true;
            node.field = nodeChildDetails.field;
            node.key = nodeChildDetails.key;
            /** @deprecated is now 'master' */
            node.canFlower = node.master;
            // pull out all the leaf children and add to our node
            this.setLeafChildren(node);
        }
        else {
            node.group = false;
            if (doingTreeData) {
                node.master = false;
                node.expanded = false;
            }
            else {
                // this is the default, for when doing grid data
                if (this.doesDataFlower) {
                    node.master = this.doesDataFlower(dataItem);
                }
                else if (this.doingMasterDetail) {
                    // if we are doing master detail, then the
                    // default is that everything can flower.
                    if (this.isRowMasterFunc) {
                        node.master = this.isRowMasterFunc(dataItem);
                    }
                    else {
                        node.master = true;
                    }
                }
                else {
                    node.master = false;
                }
                var rowGroupColumns = this.columnController.getRowGroupColumns();
                var numRowGroupColumns = rowGroupColumns ? rowGroupColumns.length : 0;
                // need to take row group into account when determining level
                var masterRowLevel = level + numRowGroupColumns;
                node.expanded = node.master ? this.isExpanded(masterRowLevel) : false;
            }
        }
        // support for backwards compatibility, canFlow is now called 'master'
        node.canFlower = node.master;
        if (parent && !this.suppressParentsInRowNodes) {
            node.parent = parent;
        }
        node.level = level;
        node.setDataAndId(dataItem, this.nextId.toString());
        if (this.allNodesMap[node.id]) {
            console.warn("ag-grid: duplicate node id '" + node.id + "' detected from getRowNodeId callback, this could cause issues in your grid.");
        }
        this.allNodesMap[node.id] = node;
        this.nextId++;
        return node;
    };
    ClientSideNodeManager.prototype.isExpanded = function (level) {
        var expandByDefault = this.gridOptionsWrapper.getGroupDefaultExpanded();
        if (expandByDefault === -1) {
            return true;
        }
        else {
            return level < expandByDefault;
        }
    };
    // this is only used for doing legacy tree data
    ClientSideNodeManager.prototype.setLeafChildren = function (node) {
        node.allLeafChildren = [];
        if (node.childrenAfterGroup) {
            node.childrenAfterGroup.forEach(function (childAfterGroup) {
                if (childAfterGroup.group) {
                    if (childAfterGroup.allLeafChildren) {
                        childAfterGroup.allLeafChildren.forEach(function (leafChild) { return node.allLeafChildren.push(leafChild); });
                    }
                }
                else {
                    node.allLeafChildren.push(childAfterGroup);
                }
            });
        }
    };
    ClientSideNodeManager.prototype.isLegacyTreeData = function () {
        var rowsAlreadyGrouped = _.exists(this.gridOptionsWrapper.getNodeChildDetailsFunc());
        if (rowsAlreadyGrouped) {
            console.warn('ag-Grid: adding and removing rows is not supported when using nodeChildDetailsFunc, ie it is not ' +
                'supported for legacy tree data. Please see the docs on the new preferred way of providing tree data that works with delta updates.');
            return true;
        }
        else {
            return false;
        }
    };
    ClientSideNodeManager.TOP_LEVEL = 0;
    ClientSideNodeManager.ROOT_NODE_ID = 'ROOT_NODE_ID';
    return ClientSideNodeManager;
}());

var __decorate = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var RecursionType;
(function (RecursionType) {
    RecursionType[RecursionType["Normal"] = 0] = "Normal";
    RecursionType[RecursionType["AfterFilter"] = 1] = "AfterFilter";
    RecursionType[RecursionType["AfterFilterAndSort"] = 2] = "AfterFilterAndSort";
    RecursionType[RecursionType["PivotNodes"] = 3] = "PivotNodes";
})(RecursionType || (RecursionType = {}));
var ClientSideRowModel = /** @class */ (function () {
    function ClientSideRowModel() {
    }
    ClientSideRowModel.prototype.init = function () {
        var refreshEverythingFunc = this.refreshModel.bind(this, { step: Constants.STEP_EVERYTHING });
        var refreshEverythingAfterColsChangedFunc = this.refreshModel.bind(this, { step: Constants.STEP_EVERYTHING, afterColumnsChanged: true });
        this.eventService.addModalPriorityEventListener(Events.EVENT_COLUMN_EVERYTHING_CHANGED, refreshEverythingAfterColsChangedFunc);
        this.eventService.addModalPriorityEventListener(Events.EVENT_COLUMN_ROW_GROUP_CHANGED, refreshEverythingFunc);
        this.eventService.addModalPriorityEventListener(Events.EVENT_COLUMN_VALUE_CHANGED, this.onValueChanged.bind(this));
        this.eventService.addModalPriorityEventListener(Events.EVENT_COLUMN_PIVOT_CHANGED, this.refreshModel.bind(this, { step: Constants.STEP_PIVOT }));
        this.eventService.addModalPriorityEventListener(Events.EVENT_ROW_GROUP_OPENED, this.onRowGroupOpened.bind(this));
        this.eventService.addModalPriorityEventListener(Events.EVENT_FILTER_CHANGED, this.onFilterChanged.bind(this));
        this.eventService.addModalPriorityEventListener(Events.EVENT_SORT_CHANGED, this.onSortChanged.bind(this));
        this.eventService.addModalPriorityEventListener(Events.EVENT_COLUMN_PIVOT_MODE_CHANGED, refreshEverythingFunc);
        var refreshMapFunc = this.refreshModel.bind(this, {
            step: Constants.STEP_MAP,
            keepRenderedRows: true,
            animate: true
        });
        this.gridOptionsWrapper.addEventListener(GridOptionsWrapper.PROP_GROUP_REMOVE_SINGLE_CHILDREN, refreshMapFunc);
        this.gridOptionsWrapper.addEventListener(GridOptionsWrapper.PROP_GROUP_REMOVE_LOWEST_SINGLE_CHILDREN, refreshMapFunc);
        this.rootNode = new RowNode();
        this.nodeManager = new ClientSideNodeManager(this.rootNode, this.gridOptionsWrapper, this.context, this.eventService, this.columnController, this.gridApi, this.columnApi, this.selectionController);
        this.context.wireBean(this.rootNode);
    };
    ClientSideRowModel.prototype.start = function () {
        var rowData = this.gridOptionsWrapper.getRowData();
        if (rowData) {
            this.setRowData(rowData);
        }
    };
    ClientSideRowModel.prototype.ensureRowHeightsValid = function (startPixel, endPixel, startLimitIndex, endLimitIndex) {
        var atLeastOneChange;
        var res = false;
        // we do this multiple times as changing the row heights can also change the first and last rows,
        // so the first pass can make lots of rows smaller, which means the second pass we end up changing
        // more rows.
        do {
            atLeastOneChange = false;
            var rowAtStartPixel = this.getRowIndexAtPixel(startPixel);
            var rowAtEndPixel = this.getRowIndexAtPixel(endPixel);
            // keep check to current page if doing pagination
            var firstRow = Math.max(rowAtStartPixel, startLimitIndex);
            var lastRow = Math.min(rowAtEndPixel, endLimitIndex);
            for (var rowIndex = firstRow; rowIndex <= lastRow; rowIndex++) {
                var rowNode = this.getRow(rowIndex);
                if (rowNode.rowHeightEstimated) {
                    var rowHeight = this.gridOptionsWrapper.getRowHeightForNode(rowNode);
                    rowNode.setRowHeight(rowHeight.height);
                    atLeastOneChange = true;
                    res = true;
                }
            }
            if (atLeastOneChange) {
                this.setRowTops();
            }
        } while (atLeastOneChange);
        return res;
    };
    ClientSideRowModel.prototype.setRowTops = function () {
        var nextRowTop = 0;
        for (var i = 0; i < this.rowsToDisplay.length; i++) {
            // we don't estimate if doing fullHeight or autoHeight, as all rows get rendered all the time
            // with these two layouts.
            var allowEstimate = this.gridOptionsWrapper.getDomLayout() === Constants.DOM_LAYOUT_NORMAL;
            var rowNode = this.rowsToDisplay[i];
            if (_.missing(rowNode.rowHeight)) {
                var rowHeight = this.gridOptionsWrapper.getRowHeightForNode(rowNode, allowEstimate);
                rowNode.setRowHeight(rowHeight.height, rowHeight.estimated);
            }
            rowNode.setRowTop(nextRowTop);
            rowNode.setRowIndex(i);
            nextRowTop += rowNode.rowHeight;
        }
    };
    ClientSideRowModel.prototype.resetRowTops = function (rowNode, changedPath) {
        rowNode.clearRowTop();
        if (rowNode.hasChildren()) {
            if (rowNode.childrenAfterGroup) {
                // if a changedPath is active, it means we are here because of a transaction update or
                // a change detection. neither of these impacts the open/closed state of groups. so if
                // a group is not open this time, it was not open last time. so we know all closed groups
                // already have their top positions cleared. so there is no need to traverse all the way
                // when changedPath is active and the rowNode is not expanded.
                var skipChildren = changedPath.isActive() && !rowNode.expanded;
                if (!skipChildren) {
                    for (var i = 0; i < rowNode.childrenAfterGroup.length; i++) {
                        this.resetRowTops(rowNode.childrenAfterGroup[i], changedPath);
                    }
                }
            }
            if (rowNode.sibling) {
                rowNode.sibling.clearRowTop();
            }
        }
        if (rowNode.detailNode) {
            rowNode.detailNode.clearRowTop();
        }
    };
    // returns false if row was moved, otherwise true
    ClientSideRowModel.prototype.ensureRowsAtPixel = function (rowNodes, pixel, increment) {
        var _this = this;
        if (increment === void 0) { increment = 0; }
        var indexAtPixelNow = this.getRowIndexAtPixel(pixel);
        var rowNodeAtPixelNow = this.getRow(indexAtPixelNow);
        if (rowNodeAtPixelNow === rowNodes[0]) {
            return false;
        }
        rowNodes.forEach(function (rowNode) {
            _.removeFromArray(_this.rootNode.allLeafChildren, rowNode);
        });
        rowNodes.forEach(function (rowNode, idx) {
            _.insertIntoArray(_this.rootNode.allLeafChildren, rowNode, indexAtPixelNow + increment + idx);
        });
        this.refreshModel({
            step: Constants.STEP_EVERYTHING,
            keepRenderedRows: true,
            animate: true,
            keepEditingRows: true
        });
        return true;
    };
    ClientSideRowModel.prototype.highlightRowAtPixel = function (rowNode, pixel) {
        var indexAtPixelNow = pixel != null ? this.getRowIndexAtPixel(pixel) : null;
        var rowNodeAtPixelNow = indexAtPixelNow != null ? this.getRow(indexAtPixelNow) : null;
        if (rowNodeAtPixelNow === rowNode || !rowNode || pixel == null) {
            if (this.lastHighlightedRow) {
                this.lastHighlightedRow.setHighlighted(null);
                this.lastHighlightedRow = null;
            }
            return;
        }
        var rowTop = rowNodeAtPixelNow.rowTop, rowHeight = rowNodeAtPixelNow.rowHeight;
        var highlight = pixel - rowTop < rowHeight / 2 ? 'above' : 'below';
        if (this.lastHighlightedRow && this.lastHighlightedRow !== rowNodeAtPixelNow) {
            this.lastHighlightedRow.setHighlighted(null);
            this.lastHighlightedRow = null;
        }
        rowNodeAtPixelNow.setHighlighted(highlight);
        this.lastHighlightedRow = rowNodeAtPixelNow;
    };
    ClientSideRowModel.prototype.getLastHighlightedRowNode = function () {
        return this.lastHighlightedRow;
    };
    ClientSideRowModel.prototype.isLastRowFound = function () {
        return true;
    };
    ClientSideRowModel.prototype.getRowCount = function () {
        if (this.rowsToDisplay) {
            return this.rowsToDisplay.length;
        }
        return 0;
    };
    ClientSideRowModel.prototype.getTopLevelRowCount = function () {
        var showingRootNode = this.rowsToDisplay && this.rowsToDisplay[0] === this.rootNode;
        if (showingRootNode) {
            return 1;
        }
        return this.rootNode.childrenAfterFilter ? this.rootNode.childrenAfterFilter.length : 0;
    };
    ClientSideRowModel.prototype.getTopLevelRowDisplayedIndex = function (topLevelIndex) {
        var showingRootNode = this.rowsToDisplay && this.rowsToDisplay[0] === this.rootNode;
        if (showingRootNode) {
            return topLevelIndex;
        }
        var rowNode = this.rootNode.childrenAfterSort[topLevelIndex];
        if (this.gridOptionsWrapper.isGroupHideOpenParents()) {
            // if hideOpenParents, and this row open, then this row is now displayed at this index, first child is
            while (rowNode.expanded && rowNode.childrenAfterSort && rowNode.childrenAfterSort.length > 0) {
                rowNode = rowNode.childrenAfterSort[0];
            }
        }
        return rowNode.rowIndex;
    };
    ClientSideRowModel.prototype.getRowBounds = function (index) {
        if (_.missing(this.rowsToDisplay)) {
            return null;
        }
        var rowNode = this.rowsToDisplay[index];
        if (rowNode) {
            return {
                rowTop: rowNode.rowTop,
                rowHeight: rowNode.rowHeight
            };
        }
        return null;
    };
    ClientSideRowModel.prototype.onRowGroupOpened = function () {
        var animate = this.gridOptionsWrapper.isAnimateRows();
        this.refreshModel({ step: Constants.STEP_MAP, keepRenderedRows: true, animate: animate });
    };
    ClientSideRowModel.prototype.onFilterChanged = function (event) {
        if (event.afterDataChange) {
            return;
        }
        var animate = this.gridOptionsWrapper.isAnimateRows();
        this.refreshModel({ step: Constants.STEP_FILTER, keepRenderedRows: true, animate: animate });
    };
    ClientSideRowModel.prototype.onSortChanged = function () {
        var animate = this.gridOptionsWrapper.isAnimateRows();
        this.refreshModel({ step: Constants.STEP_SORT, keepRenderedRows: true, animate: animate, keepEditingRows: true });
    };
    ClientSideRowModel.prototype.getType = function () {
        return Constants.ROW_MODEL_TYPE_CLIENT_SIDE;
    };
    ClientSideRowModel.prototype.onValueChanged = function () {
        if (this.columnController.isPivotActive()) {
            this.refreshModel({ step: Constants.STEP_PIVOT });
        }
        else {
            this.refreshModel({ step: Constants.STEP_AGGREGATE });
        }
    };
    ClientSideRowModel.prototype.createChangePath = function (rowNodeTransactions) {
        // for updates, if the row is updated at all, then we re-calc all the values
        // in that row. we could compare each value to each old value, however if we
        // did this, we would be calling the valueService twice, once on the old value
        // and once on the new value. so it's less valueGetter calls if we just assume
        // each column is different. that way the changedPath is used so that only
        // the impacted parent rows are recalculated, parents who's children have
        // not changed are not impacted.
        var noTransactions = _.missingOrEmpty(rowNodeTransactions);
        var changedPath = new ChangedPath(false, this.rootNode);
        if (noTransactions || this.gridOptionsWrapper.isTreeData()) {
            changedPath.setInactive();
        }
        return changedPath;
    };
    ClientSideRowModel.prototype.refreshModel = function (params) {
        // this goes through the pipeline of stages. what's in my head is similar
        // to the diagram on this page:
        // http://commons.apache.org/sandbox/commons-pipeline/pipeline_basics.html
        // however we want to keep the results of each stage, hence we manually call
        // each step rather than have them chain each other.
        var _this = this;
        // fallthrough in below switch is on purpose,
        // eg if STEP_FILTER, then all steps below this
        // step get done
        // let start: number;
        // console.log('======= start =======');
        var changedPath = this.createChangePath(params.rowNodeTransactions);
        switch (params.step) {
            case Constants.STEP_EVERYTHING:
                // start = new Date().getTime();
                this.doRowGrouping(params.groupState, params.rowNodeTransactions, params.rowNodeOrder, changedPath, params.afterColumnsChanged);
            // console.log('rowGrouping = ' + (new Date().getTime() - start));
            case Constants.STEP_FILTER:
                // start = new Date().getTime();
                this.doFilter(changedPath);
            // console.log('filter = ' + (new Date().getTime() - start));
            case Constants.STEP_PIVOT:
                this.doPivot(changedPath);
            case Constants.STEP_AGGREGATE: // depends on agg fields
                // start = new Date().getTime();
                this.doAggregate(changedPath);
            // console.log('aggregation = ' + (new Date().getTime() - start));
            case Constants.STEP_SORT:
                // start = new Date().getTime();
                this.doSort(params.rowNodeTransactions, changedPath);
            // console.log('sort = ' + (new Date().getTime() - start));
            case Constants.STEP_MAP:
                // start = new Date().getTime();
                this.doRowsToDisplay();
            // console.log('rowsToDisplay = ' + (new Date().getTime() - start));
        }
        // set all row tops to null, then set row tops on all visible rows. if we don't
        // do this, then the algorithm below only sets row tops, old row tops from old rows
        // will still lie around
        this.resetRowTops(this.rootNode, changedPath);
        this.setRowTops();
        var event = {
            type: Events.EVENT_MODEL_UPDATED,
            api: this.gridApi,
            columnApi: this.columnApi,
            animate: params.animate,
            keepRenderedRows: params.keepRenderedRows,
            newData: params.newData,
            newPage: false
        };
        this.eventService.dispatchEvent(event);
        if (this.$scope) {
            window.setTimeout(function () {
                _this.$scope.$apply();
            }, 0);
        }
    };
    ClientSideRowModel.prototype.isEmpty = function () {
        var rowsMissing;
        var doingLegacyTreeData = _.exists(this.gridOptionsWrapper.getNodeChildDetailsFunc());
        if (doingLegacyTreeData) {
            rowsMissing = _.missing(this.rootNode.childrenAfterGroup) || this.rootNode.childrenAfterGroup.length === 0;
        }
        else {
            rowsMissing = _.missing(this.rootNode.allLeafChildren) || this.rootNode.allLeafChildren.length === 0;
        }
        var empty = _.missing(this.rootNode) || rowsMissing || !this.columnController.isReady();
        return empty;
    };
    ClientSideRowModel.prototype.isRowsToRender = function () {
        return _.exists(this.rowsToDisplay) && this.rowsToDisplay.length > 0;
    };
    ClientSideRowModel.prototype.getNodesInRangeForSelection = function (firstInRange, lastInRange) {
        // if lastSelectedNode is missing, we start at the first row
        var firstRowHit = !lastInRange;
        var lastRowHit = false;
        var lastRow;
        var result = [];
        var groupsSelectChildren = this.gridOptionsWrapper.isGroupSelectsChildren();
        this.forEachNodeAfterFilterAndSort(function (rowNode) {
            var lookingForLastRow = firstRowHit && !lastRowHit;
            // check if we need to flip the select switch
            if (!firstRowHit) {
                if (rowNode === lastInRange || rowNode === firstInRange) {
                    firstRowHit = true;
                }
            }
            var skipThisGroupNode = rowNode.group && groupsSelectChildren;
            if (!skipThisGroupNode) {
                var inRange = firstRowHit && !lastRowHit;
                var childOfLastRow = rowNode.isParentOfNode(lastRow);
                if (inRange || childOfLastRow) {
                    result.push(rowNode);
                }
            }
            if (lookingForLastRow) {
                if (rowNode === lastInRange || rowNode === firstInRange) {
                    lastRowHit = true;
                    if (rowNode === lastInRange) {
                        lastRow = lastInRange;
                    }
                    else {
                        lastRow = firstInRange;
                    }
                }
            }
        });
        return result;
    };
    ClientSideRowModel.prototype.setDatasource = function (datasource) {
        console.error('ag-Grid: should never call setDatasource on clientSideRowController');
    };
    ClientSideRowModel.prototype.getTopLevelNodes = function () {
        return this.rootNode ? this.rootNode.childrenAfterGroup : null;
    };
    ClientSideRowModel.prototype.getRootNode = function () {
        return this.rootNode;
    };
    ClientSideRowModel.prototype.getRow = function (index) {
        return this.rowsToDisplay[index];
    };
    ClientSideRowModel.prototype.isRowPresent = function (rowNode) {
        return this.rowsToDisplay.indexOf(rowNode) >= 0;
    };
    ClientSideRowModel.prototype.getRowIndexAtPixel = function (pixelToMatch) {
        if (this.isEmpty()) {
            return -1;
        }
        // do binary search of tree
        // http://oli.me.uk/2013/06/08/searching-javascript-arrays-with-a-binary-search/
        var bottomPointer = 0;
        var topPointer = this.rowsToDisplay.length - 1;
        // quick check, if the pixel is out of bounds, then return last row
        if (pixelToMatch <= 0) {
            // if pixel is less than or equal zero, it's always the first row
            return 0;
        }
        var lastNode = _.last(this.rowsToDisplay);
        if (lastNode.rowTop <= pixelToMatch) {
            return this.rowsToDisplay.length - 1;
        }
        while (true) {
            var midPointer = Math.floor((bottomPointer + topPointer) / 2);
            var currentRowNode = this.rowsToDisplay[midPointer];
            if (this.isRowInPixel(currentRowNode, pixelToMatch)) {
                return midPointer;
            }
            else if (currentRowNode.rowTop < pixelToMatch) {
                bottomPointer = midPointer + 1;
            }
            else if (currentRowNode.rowTop > pixelToMatch) {
                topPointer = midPointer - 1;
            }
        }
    };
    ClientSideRowModel.prototype.isRowInPixel = function (rowNode, pixelToMatch) {
        var topPixel = rowNode.rowTop;
        var bottomPixel = rowNode.rowTop + rowNode.rowHeight;
        var pixelInRow = topPixel <= pixelToMatch && bottomPixel > pixelToMatch;
        return pixelInRow;
    };
    ClientSideRowModel.prototype.getCurrentPageHeight = function () {
        if (this.rowsToDisplay && this.rowsToDisplay.length > 0) {
            var lastRow = _.last(this.rowsToDisplay);
            var lastPixel = lastRow.rowTop + lastRow.rowHeight;
            return lastPixel;
        }
        return 0;
    };
    ClientSideRowModel.prototype.forEachLeafNode = function (callback) {
        if (this.rootNode.allLeafChildren) {
            this.rootNode.allLeafChildren.forEach(function (rowNode, index) { return callback(rowNode, index); });
        }
    };
    ClientSideRowModel.prototype.forEachNode = function (callback) {
        this.recursivelyWalkNodesAndCallback(this.rootNode.childrenAfterGroup, callback, RecursionType.Normal, 0);
    };
    ClientSideRowModel.prototype.forEachNodeAfterFilter = function (callback) {
        this.recursivelyWalkNodesAndCallback(this.rootNode.childrenAfterFilter, callback, RecursionType.AfterFilter, 0);
    };
    ClientSideRowModel.prototype.forEachNodeAfterFilterAndSort = function (callback) {
        this.recursivelyWalkNodesAndCallback(this.rootNode.childrenAfterSort, callback, RecursionType.AfterFilterAndSort, 0);
    };
    ClientSideRowModel.prototype.forEachPivotNode = function (callback) {
        this.recursivelyWalkNodesAndCallback([this.rootNode], callback, RecursionType.PivotNodes, 0);
    };
    // iterates through each item in memory, and calls the callback function
    // nodes - the rowNodes to traverse
    // callback - the user provided callback
    // recursion type - need this to know what child nodes to recurse, eg if looking at all nodes, or filtered notes etc
    // index - works similar to the index in forEach in javascript's array function
    ClientSideRowModel.prototype.recursivelyWalkNodesAndCallback = function (nodes, callback, recursionType, index) {
        if (!nodes) {
            return index;
        }
        for (var i = 0; i < nodes.length; i++) {
            var node = nodes[i];
            callback(node, index++);
            // go to the next level if it is a group
            if (node.hasChildren()) {
                // depending on the recursion type, we pick a difference set of children
                var nodeChildren = null;
                switch (recursionType) {
                    case RecursionType.Normal:
                        nodeChildren = node.childrenAfterGroup;
                        break;
                    case RecursionType.AfterFilter:
                        nodeChildren = node.childrenAfterFilter;
                        break;
                    case RecursionType.AfterFilterAndSort:
                        nodeChildren = node.childrenAfterSort;
                        break;
                    case RecursionType.PivotNodes:
                        // for pivot, we don't go below leafGroup levels
                        nodeChildren = !node.leafGroup ? node.childrenAfterSort : null;
                        break;
                }
                if (nodeChildren) {
                    index = this.recursivelyWalkNodesAndCallback(nodeChildren, callback, recursionType, index);
                }
            }
        }
        return index;
    };
    // it's possible to recompute the aggregate without doing the other parts
    // + gridApi.recomputeAggregates()
    ClientSideRowModel.prototype.doAggregate = function (changedPath) {
        if (this.aggregationStage) {
            this.aggregationStage.execute({ rowNode: this.rootNode, changedPath: changedPath });
        }
    };
    // + gridApi.expandAll()
    // + gridApi.collapseAll()
    ClientSideRowModel.prototype.expandOrCollapseAll = function (expand) {
        var usingTreeData = this.gridOptionsWrapper.isTreeData();
        if (this.rootNode) {
            recursiveExpandOrCollapse(this.rootNode.childrenAfterGroup);
        }
        function recursiveExpandOrCollapse(rowNodes) {
            if (!rowNodes) {
                return;
            }
            rowNodes.forEach(function (rowNode) {
                var shouldExpandOrCollapse = usingTreeData ? _.exists(rowNode.childrenAfterGroup) : rowNode.group;
                if (shouldExpandOrCollapse) {
                    rowNode.expanded = expand;
                    recursiveExpandOrCollapse(rowNode.childrenAfterGroup);
                }
            });
        }
        this.refreshModel({ step: Constants.STEP_MAP });
        var eventSource = expand ? 'expandAll' : 'collapseAll';
        var event = {
            api: this.gridApi,
            columnApi: this.columnApi,
            type: Events.EVENT_EXPAND_COLLAPSE_ALL,
            source: eventSource
        };
        this.eventService.dispatchEvent(event);
    };
    ClientSideRowModel.prototype.doSort = function (rowNodeTransactions, changedPath) {
        this.sortStage.execute({
            rowNode: this.rootNode,
            rowNodeTransactions: rowNodeTransactions,
            changedPath: changedPath
        });
    };
    ClientSideRowModel.prototype.doRowGrouping = function (groupState, rowNodeTransactions, rowNodeOrder, changedPath, afterColumnsChanged) {
        // grouping is enterprise only, so if service missing, skip the step
        var doingLegacyTreeData = _.exists(this.gridOptionsWrapper.getNodeChildDetailsFunc());
        if (doingLegacyTreeData) {
            return;
        }
        if (this.groupStage) {
            if (rowNodeTransactions) {
                var merged_1 = {
                    add: [],
                    remove: [],
                    update: []
                };
                rowNodeTransactions.forEach(function (tran) {
                    _.pushAll(merged_1.add, tran.add);
                    _.pushAll(merged_1.remove, tran.remove);
                    _.pushAll(merged_1.update, tran.update);
                });
                this.groupStage.execute({
                    rowNode: this.rootNode,
                    rowNodeTransaction: merged_1,
                    rowNodeOrder: rowNodeOrder,
                    changedPath: changedPath
                });
            }
            else {
                // groups are about to get disposed, so need to deselect any that are selected
                this.selectionController.removeGroupsFromSelection();
                this.groupStage.execute({
                    rowNode: this.rootNode,
                    changedPath: changedPath,
                    afterColumnsChanged: afterColumnsChanged
                });
                // set open/closed state on groups
                this.restoreGroupState(groupState);
            }
            if (this.gridOptionsWrapper.isGroupSelectsChildren()) {
                this.selectionController.updateGroupsFromChildrenSelections(changedPath);
            }
        }
        else {
            this.rootNode.childrenAfterGroup = this.rootNode.allLeafChildren;
        }
    };
    ClientSideRowModel.prototype.restoreGroupState = function (groupState) {
        if (!groupState) {
            return;
        }
        _.traverseNodesWithKey(this.rootNode.childrenAfterGroup, function (node, key) {
            // if the group was open last time, then open it this time. however
            // if was not open last time, then don't touch the group, so the 'groupDefaultExpanded'
            // setting will take effect.
            if (typeof groupState[key] === 'boolean') {
                node.expanded = groupState[key];
            }
        });
    };
    ClientSideRowModel.prototype.doFilter = function (changedPath) {
        this.filterStage.execute({ rowNode: this.rootNode, changedPath: changedPath });
    };
    ClientSideRowModel.prototype.doPivot = function (changedPath) {
        if (this.pivotStage) {
            this.pivotStage.execute({ rowNode: this.rootNode, changedPath: changedPath });
        }
    };
    ClientSideRowModel.prototype.getGroupState = function () {
        if (!this.rootNode.childrenAfterGroup || !this.gridOptionsWrapper.isRememberGroupStateWhenNewData()) {
            return null;
        }
        var result = {};
        _.traverseNodesWithKey(this.rootNode.childrenAfterGroup, function (node, key) { return result[key] = node.expanded; });
        return result;
    };
    ClientSideRowModel.prototype.getCopyOfNodesMap = function () {
        return this.nodeManager.getCopyOfNodesMap();
    };
    ClientSideRowModel.prototype.getRowNode = function (id) {
        return this.nodeManager.getRowNode(id);
    };
    // rows: the rows to put into the model
    ClientSideRowModel.prototype.setRowData = function (rowData) {
        // no need to invalidate cache, as the cache is stored on the rowNode,
        // so new rowNodes means the cache is wiped anyway.
        // remember group state, so we can expand groups that should be expanded
        var groupState = this.getGroupState();
        this.nodeManager.setRowData(rowData);
        // this event kicks off:
        // - clears selection
        // - updates filters
        // - shows 'no rows' overlay if needed
        var rowDataChangedEvent = {
            type: Events.EVENT_ROW_DATA_CHANGED,
            api: this.gridApi,
            columnApi: this.columnApi
        };
        this.eventService.dispatchEvent(rowDataChangedEvent);
        this.refreshModel({
            step: Constants.STEP_EVERYTHING,
            groupState: groupState,
            newData: true
        });
    };
    ClientSideRowModel.prototype.batchUpdateRowData = function (rowDataTransaction, callback) {
        var _this = this;
        if (!this.rowDataTransactionBatch) {
            this.rowDataTransactionBatch = [];
            var waitMillis = this.gridOptionsWrapper.getBatchUpdateWaitMillis();
            window.setTimeout(function () {
                _this.executeBatchUpdateRowData();
                _this.rowDataTransactionBatch = null;
            }, waitMillis);
        }
        this.rowDataTransactionBatch.push({ rowDataTransaction: rowDataTransaction, callback: callback });
    };
    ClientSideRowModel.prototype.executeBatchUpdateRowData = function () {
        var _this = this;
        this.valueCache.onDataChanged();
        var callbackFuncsBound = [];
        var rowNodeTrans = [];
        if (this.rowDataTransactionBatch) {
            this.rowDataTransactionBatch.forEach(function (tranItem) {
                var rowNodeTran = _this.nodeManager.updateRowData(tranItem.rowDataTransaction, null);
                rowNodeTrans.push(rowNodeTran);
                if (tranItem.callback) {
                    callbackFuncsBound.push(tranItem.callback.bind(null, rowNodeTran));
                }
            });
        }
        this.commonUpdateRowData(rowNodeTrans);
        // do callbacks in next VM turn so it's async
        if (callbackFuncsBound.length > 0) {
            window.setTimeout(function () {
                callbackFuncsBound.forEach(function (func) { return func(); });
            }, 0);
        }
    };
    ClientSideRowModel.prototype.updateRowData = function (rowDataTran, rowNodeOrder) {
        this.valueCache.onDataChanged();
        var rowNodeTran = this.nodeManager.updateRowData(rowDataTran, rowNodeOrder);
        this.commonUpdateRowData([rowNodeTran], rowNodeOrder);
        return rowNodeTran;
    };
    // common to updateRowData and batchUpdateRowData
    ClientSideRowModel.prototype.commonUpdateRowData = function (rowNodeTrans, rowNodeOrder) {
        this.refreshModel({
            step: Constants.STEP_EVERYTHING,
            rowNodeTransactions: rowNodeTrans,
            rowNodeOrder: rowNodeOrder,
            keepRenderedRows: true,
            animate: true,
            keepEditingRows: true
        });
        var event = {
            type: Events.EVENT_ROW_DATA_UPDATED,
            api: this.gridApi,
            columnApi: this.columnApi
        };
        this.eventService.dispatchEvent(event);
    };
    ClientSideRowModel.prototype.doRowsToDisplay = function () {
        this.rowsToDisplay = this.flattenStage.execute({ rowNode: this.rootNode });
    };
    ClientSideRowModel.prototype.onRowHeightChanged = function () {
        this.refreshModel({ step: Constants.STEP_MAP, keepRenderedRows: true, keepEditingRows: true });
    };
    ClientSideRowModel.prototype.resetRowHeights = function () {
        this.forEachNode(function (rowNode) { return rowNode.setRowHeight(null); });
        this.onRowHeightChanged();
    };
    __decorate([
        Autowired('gridOptionsWrapper')
    ], ClientSideRowModel.prototype, "gridOptionsWrapper", void 0);
    __decorate([
        Autowired('columnController')
    ], ClientSideRowModel.prototype, "columnController", void 0);
    __decorate([
        Autowired('filterManager')
    ], ClientSideRowModel.prototype, "filterManager", void 0);
    __decorate([
        Autowired('$scope')
    ], ClientSideRowModel.prototype, "$scope", void 0);
    __decorate([
        Autowired('selectionController')
    ], ClientSideRowModel.prototype, "selectionController", void 0);
    __decorate([
        Autowired('eventService')
    ], ClientSideRowModel.prototype, "eventService", void 0);
    __decorate([
        Autowired('context')
    ], ClientSideRowModel.prototype, "context", void 0);
    __decorate([
        Autowired('valueService')
    ], ClientSideRowModel.prototype, "valueService", void 0);
    __decorate([
        Autowired('valueCache')
    ], ClientSideRowModel.prototype, "valueCache", void 0);
    __decorate([
        Autowired('columnApi')
    ], ClientSideRowModel.prototype, "columnApi", void 0);
    __decorate([
        Autowired('gridApi')
    ], ClientSideRowModel.prototype, "gridApi", void 0);
    __decorate([
        Autowired('filterStage')
    ], ClientSideRowModel.prototype, "filterStage", void 0);
    __decorate([
        Autowired('sortStage')
    ], ClientSideRowModel.prototype, "sortStage", void 0);
    __decorate([
        Autowired('flattenStage')
    ], ClientSideRowModel.prototype, "flattenStage", void 0);
    __decorate([
        Optional('groupStage')
    ], ClientSideRowModel.prototype, "groupStage", void 0);
    __decorate([
        Optional('aggregationStage')
    ], ClientSideRowModel.prototype, "aggregationStage", void 0);
    __decorate([
        Optional('pivotStage')
    ], ClientSideRowModel.prototype, "pivotStage", void 0);
    __decorate([
        PostConstruct
    ], ClientSideRowModel.prototype, "init", null);
    ClientSideRowModel = __decorate([
        Bean('rowModel')
    ], ClientSideRowModel);
    return ClientSideRowModel;
}());

var __decorate$1 = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var FilterStage = /** @class */ (function () {
    function FilterStage() {
    }
    FilterStage.prototype.execute = function (params) {
        var rowNode = params.rowNode, changedPath = params.changedPath;
        this.filterService.filter(changedPath);
        this.selectableService.updateSelectableAfterFiltering(rowNode);
    };
    __decorate$1([
        Autowired('gridOptionsWrapper')
    ], FilterStage.prototype, "gridOptionsWrapper", void 0);
    __decorate$1([
        Autowired('selectableService')
    ], FilterStage.prototype, "selectableService", void 0);
    __decorate$1([
        Autowired('filterService')
    ], FilterStage.prototype, "filterService", void 0);
    FilterStage = __decorate$1([
        Bean('filterStage')
    ], FilterStage);
    return FilterStage;
}());

var __decorate$2 = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var SortStage = /** @class */ (function () {
    function SortStage() {
    }
    SortStage.prototype.execute = function (params) {
        var sortOptions = this.sortController.getSortForRowController();
        var sortActive = _.exists(sortOptions) && sortOptions.length > 0;
        var deltaSort = sortActive
            && _.exists(params.rowNodeTransactions)
            // in time we can remove this check, so that delta sort is always
            // on if transactions are present. it's off for now so that we can
            // selectively turn it on and test it with some select users before
            // rolling out to everyone.
            && this.gridOptionsWrapper.isDeltaSort();
        // we only need dirty nodes if doing delta sort
        var dirtyLeafNodes = deltaSort ? this.calculateDirtyNodes(params.rowNodeTransactions) : null;
        var valueColumns = this.columnController.getValueColumns();
        var noAggregations = _.missingOrEmpty(valueColumns);
        this.sortService.sort(sortOptions, sortActive, deltaSort, dirtyLeafNodes, params.changedPath, noAggregations);
    };
    SortStage.prototype.calculateDirtyNodes = function (rowNodeTransactions) {
        var dirtyNodes = {};
        var addNodesFunc = function (rowNodes) {
            if (rowNodes) {
                rowNodes.forEach(function (rowNode) { return dirtyNodes[rowNode.id] = true; });
            }
        };
        // all leaf level nodes in the transaction were impacted
        rowNodeTransactions.forEach(function (tran) {
            addNodesFunc(tran.add);
            addNodesFunc(tran.update);
            addNodesFunc(tran.remove);
        });
        return dirtyNodes;
    };
    __decorate$2([
        Autowired('gridOptionsWrapper')
    ], SortStage.prototype, "gridOptionsWrapper", void 0);
    __decorate$2([
        Autowired('sortService')
    ], SortStage.prototype, "sortService", void 0);
    __decorate$2([
        Autowired('sortController')
    ], SortStage.prototype, "sortController", void 0);
    __decorate$2([
        Autowired('columnController')
    ], SortStage.prototype, "columnController", void 0);
    SortStage = __decorate$2([
        Bean('sortStage')
    ], SortStage);
    return SortStage;
}());

var __decorate$3 = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var FlattenStage = /** @class */ (function () {
    function FlattenStage() {
    }
    FlattenStage.prototype.execute = function (params) {
        var rootNode = params.rowNode;
        // even if not doing grouping, we do the mapping, as the client might
        // of passed in data that already has a grouping in it somewhere
        var result = [];
        // putting value into a wrapper so it's passed by reference
        var nextRowTop = { value: 0 };
        var skipLeafNodes = this.columnController.isPivotMode();
        // if we are reducing, and not grouping, then we want to show the root node, as that
        // is where the pivot values are
        var showRootNode = skipLeafNodes && rootNode.leafGroup;
        var topList = showRootNode ? [rootNode] : rootNode.childrenAfterSort;
        this.recursivelyAddToRowsToDisplay(topList, result, nextRowTop, skipLeafNodes, 0);
        // we do not want the footer total if the gris is empty
        var atLeastOneRowPresent = result.length > 0;
        var includeGroupTotalFooter = !showRootNode
            // don't show total footer when showRootNode is true (i.e. in pivot mode and no groups)
            && atLeastOneRowPresent
            && this.gridOptionsWrapper.isGroupIncludeTotalFooter();
        if (includeGroupTotalFooter) {
            this.ensureFooterNodeExists(rootNode);
            this.addRowNodeToRowsToDisplay(rootNode.sibling, result, nextRowTop, 0);
        }
        return result;
    };
    FlattenStage.prototype.recursivelyAddToRowsToDisplay = function (rowsToFlatten, result, nextRowTop, skipLeafNodes, uiLevel) {
        if (_.missingOrEmpty(rowsToFlatten)) {
            return;
        }
        var groupSuppressRow = this.gridOptionsWrapper.isGroupSuppressRow();
        var hideOpenParents = this.gridOptionsWrapper.isGroupHideOpenParents();
        // these two are mutually exclusive, so if first set, we don't set the second
        var groupRemoveSingleChildren = this.gridOptionsWrapper.isGroupRemoveSingleChildren();
        var groupRemoveLowestSingleChildren = !groupRemoveSingleChildren && this.gridOptionsWrapper.isGroupRemoveLowestSingleChildren();
        for (var i = 0; i < rowsToFlatten.length; i++) {
            var rowNode = rowsToFlatten[i];
            // check all these cases, for working out if this row should be included in the final mapped list
            var isParent = rowNode.hasChildren();
            var isGroupSuppressedNode = groupSuppressRow && isParent;
            var isSkippedLeafNode = skipLeafNodes && !isParent;
            var isRemovedSingleChildrenGroup = groupRemoveSingleChildren &&
                isParent &&
                rowNode.childrenAfterGroup.length === 1;
            var isRemovedLowestSingleChildrenGroup = groupRemoveLowestSingleChildren &&
                isParent &&
                rowNode.leafGroup &&
                rowNode.childrenAfterGroup.length === 1;
            // hide open parents means when group is open, we don't show it. we also need to make sure the
            // group is expandable in the first place (as leaf groups are not expandable if pivot mode is on).
            // the UI will never allow expanding leaf  groups, however the user might via the API (or menu option 'expand all')
            var neverAllowToExpand = skipLeafNodes && rowNode.leafGroup;
            var isHiddenOpenParent = hideOpenParents && rowNode.expanded && (!neverAllowToExpand);
            var thisRowShouldBeRendered = !isSkippedLeafNode &&
                !isGroupSuppressedNode &&
                !isHiddenOpenParent &&
                !isRemovedSingleChildrenGroup &&
                !isRemovedLowestSingleChildrenGroup;
            if (thisRowShouldBeRendered) {
                this.addRowNodeToRowsToDisplay(rowNode, result, nextRowTop, uiLevel);
            }
            // if we are pivoting, we never map below the leaf group
            if (skipLeafNodes && rowNode.leafGroup) {
                continue;
            }
            if (isParent) {
                var excludedParent = isRemovedSingleChildrenGroup || isRemovedLowestSingleChildrenGroup;
                // we traverse the group if it is expended, however we always traverse if the parent node
                // was removed (as the group will never be opened if it is not displayed, we show the children instead)
                if (rowNode.expanded || excludedParent) {
                    // if the parent was excluded, then ui level is that of the parent
                    var uiLevelForChildren = excludedParent ? uiLevel : uiLevel + 1;
                    this.recursivelyAddToRowsToDisplay(rowNode.childrenAfterSort, result, nextRowTop, skipLeafNodes, uiLevelForChildren);
                    // put a footer in if user is looking for it
                    if (this.gridOptionsWrapper.isGroupIncludeFooter()) {
                        this.ensureFooterNodeExists(rowNode);
                        this.addRowNodeToRowsToDisplay(rowNode.sibling, result, nextRowTop, uiLevel);
                    }
                }
            }
            else if (rowNode.master && rowNode.expanded) {
                var detailNode = this.createDetailNode(rowNode);
                this.addRowNodeToRowsToDisplay(detailNode, result, nextRowTop, uiLevel);
            }
        }
    };
    // duplicated method, it's also in floatingRowModel
    FlattenStage.prototype.addRowNodeToRowsToDisplay = function (rowNode, result, nextRowTop, uiLevel) {
        var isGroupMultiAutoColumn = this.gridOptionsWrapper.isGroupMultiAutoColumn();
        result.push(rowNode);
        rowNode.setUiLevel(isGroupMultiAutoColumn ? 0 : uiLevel);
    };
    FlattenStage.prototype.ensureFooterNodeExists = function (groupNode) {
        // only create footer node once, otherwise we have daemons and
        // the animate screws up with the daemons hanging around
        if (_.exists(groupNode.sibling)) {
            return;
        }
        var footerNode = new RowNode();
        this.context.wireBean(footerNode);
        Object.keys(groupNode).forEach(function (key) {
            footerNode[key] = groupNode[key];
        });
        footerNode.footer = true;
        footerNode.rowTop = null;
        footerNode.oldRowTop = null;
        if (_.exists(footerNode.id)) {
            footerNode.id = 'rowGroupFooter_' + footerNode.id;
        }
        // get both header and footer to reference each other as siblings. this is never undone,
        // only overwritten. so if a group is expanded, then contracted, it will have a ghost
        // sibling - but that's fine, as we can ignore this if the header is contracted.
        footerNode.sibling = groupNode;
        groupNode.sibling = footerNode;
    };
    FlattenStage.prototype.createDetailNode = function (masterNode) {
        if (_.exists(masterNode.detailNode)) {
            return masterNode.detailNode;
        }
        var detailNode = new RowNode();
        this.context.wireBean(detailNode);
        detailNode.detail = true;
        detailNode.selectable = false;
        // flower was renamed to 'detail', but keeping for backwards compatibility
        detailNode.flower = detailNode.detail;
        detailNode.parent = masterNode;
        if (_.exists(masterNode.id)) {
            detailNode.id = 'detail_' + masterNode.id;
        }
        detailNode.data = masterNode.data;
        detailNode.level = masterNode.level + 1;
        masterNode.detailNode = detailNode;
        masterNode.childFlower = masterNode.detailNode; // for backwards compatibility
        return detailNode;
    };
    __decorate$3([
        Autowired('gridOptionsWrapper')
    ], FlattenStage.prototype, "gridOptionsWrapper", void 0);
    __decorate$3([
        Autowired('selectionController')
    ], FlattenStage.prototype, "selectionController", void 0);
    __decorate$3([
        Autowired('eventService')
    ], FlattenStage.prototype, "eventService", void 0);
    __decorate$3([
        Autowired('context')
    ], FlattenStage.prototype, "context", void 0);
    __decorate$3([
        Autowired('columnController')
    ], FlattenStage.prototype, "columnController", void 0);
    FlattenStage = __decorate$3([
        Bean('flattenStage')
    ], FlattenStage);
    return FlattenStage;
}());

var __decorate$4 = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var SortService = /** @class */ (function () {
    function SortService() {
    }
    SortService.prototype.init = function () {
        this.postSortFunc = this.gridOptionsWrapper.getPostSortFunc();
    };
    SortService.prototype.sort = function (sortOptions, sortActive, deltaSort, dirtyLeafNodes, changedPath, noAggregations) {
        var _this = this;
        var callback = function (rowNode) {
            // we clear out the 'pull down open parents' first, as the values mix up the sorting
            _this.pullDownGroupDataForHideOpenParents(rowNode.childrenAfterFilter, true);
            // Javascript sort is non deterministic when all the array items are equals, ie Comparator always returns 0,
            // so to ensure the array keeps its order, add an additional sorting condition manually, in this case we
            // are going to inspect the original array position. This is what sortedRowNodes is for.
            if (sortActive) {
                var sortedRowNodes = deltaSort ?
                    _this.doDeltaSort(rowNode, sortOptions, dirtyLeafNodes, changedPath, noAggregations)
                    : _this.doFullSort(rowNode, sortOptions);
                rowNode.childrenAfterSort = sortedRowNodes.map(function (sorted) { return sorted.rowNode; });
            }
            else {
                rowNode.childrenAfterSort = rowNode.childrenAfterFilter.slice(0);
            }
            _this.updateChildIndexes(rowNode);
            if (_this.postSortFunc) {
                _this.postSortFunc(rowNode.childrenAfterSort);
            }
        };
        changedPath.forEachChangedNodeDepthFirst(callback);
        this.updateGroupDataForHiddenOpenParents(changedPath);
    };
    SortService.prototype.doFullSort = function (rowNode, sortOptions) {
        var sortedRowNodes = rowNode.childrenAfterFilter
            .map(this.mapNodeToSortedNode.bind(this));
        sortedRowNodes.sort(this.compareRowNodes.bind(this, sortOptions));
        return sortedRowNodes;
    };
    SortService.prototype.mapNodeToSortedNode = function (rowNode, pos) {
        return { currentPos: pos, rowNode: rowNode };
    };
    SortService.prototype.doDeltaSort = function (rowNode, sortOptions, dirtyLeafNodes, changedPath, noAggregations) {
        // clean nodes will be a list of all row nodes that remain in the set
        // and ordered. we start with the old sorted set and take out any nodes
        // that were removed or changed (but not added, added doesn't make sense,
        // if a node was added, there is no way it could be here from last time).
        var cleanNodes = rowNode.childrenAfterSort
            .filter(function (rowNode) {
            // take out all nodes that were changed as part of the current transaction.
            // a changed node could a) be in a different sort position or b) may
            // no longer be in this set as the changed node may not pass filtering,
            // or be in a different group.
            var passesDirtyNodesCheck = !dirtyLeafNodes[rowNode.id];
            // also remove group nodes in the changed path, as they can have different aggregate
            // values which could impact the sort order.
            // note: changed path is not active if a) no value columns or b) no transactions. it is never
            // (b) in deltaSort as we only do deltaSort for transactions. for (a) if no value columns, then
            // there is no value in the group that could of changed (ie no aggregate values)
            var passesChangedPathCheck = noAggregations || changedPath.canSkip(rowNode);
            return passesDirtyNodesCheck && passesChangedPathCheck;
        })
            .map(this.mapNodeToSortedNode.bind(this));
        // for fast access below, we map them
        var cleanNodesMapped = {};
        cleanNodes.forEach(function (sortedRowNode) { return cleanNodesMapped[sortedRowNode.rowNode.id] = sortedRowNode.rowNode; });
        // these are all nodes that need to be placed
        var changedNodes = rowNode.childrenAfterFilter
            // ignore nodes in the clean list
            .filter(function (rowNode) { return !cleanNodesMapped[rowNode.id]; })
            .map(this.mapNodeToSortedNode.bind(this));
        // sort changed nodes. note that we don't need to sort cleanNodes as they are
        // already sorted from last time.
        changedNodes.sort(this.compareRowNodes.bind(this, sortOptions));
        if (changedNodes.length === 0) {
            return cleanNodes;
        }
        else if (cleanNodes.length === 0) {
            return changedNodes;
        }
        else {
            return this.mergeSortedArrays(sortOptions, cleanNodes, changedNodes);
        }
    };
    // Merge two sorted arrays into each other
    SortService.prototype.mergeSortedArrays = function (sortOptions, arr1, arr2) {
        var res = [];
        var i = 0;
        var j = 0;
        // Traverse both array, adding them in order
        while (i < arr1.length && j < arr2.length) {
            // Check if current element of first
            // array is smaller than current element
            // of second array. If yes, store first
            // array element and increment first array
            // index. Otherwise do same with second array
            var compareResult = this.compareRowNodes(sortOptions, arr1[i], arr2[j]);
            if (compareResult < 0) {
                res.push(arr1[i++]);
            }
            else {
                res.push(arr2[j++]);
            }
        }
        // add remaining from arr1
        while (i < arr1.length) {
            res.push(arr1[i++]);
        }
        // add remaining from arr2
        while (j < arr2.length) {
            res.push(arr2[j++]);
        }
        return res;
    };
    SortService.prototype.compareRowNodes = function (sortOptions, sortedNodeA, sortedNodeB) {
        var nodeA = sortedNodeA.rowNode;
        var nodeB = sortedNodeB.rowNode;
        // Iterate columns, return the first that doesn't match
        for (var i = 0, len = sortOptions.length; i < len; i++) {
            var sortOption = sortOptions[i];
            // let compared = compare(nodeA, nodeB, sortOption.column, sortOption.inverter === -1);
            var isInverted = sortOption.inverter === -1;
            var valueA = this.getValue(nodeA, sortOption.column);
            var valueB = this.getValue(nodeB, sortOption.column);
            var comparatorResult = void 0;
            if (sortOption.column.getColDef().comparator) {
                //if comparator provided, use it
                comparatorResult = sortOption.column.getColDef().comparator(valueA, valueB, nodeA, nodeB, isInverted);
            }
            else {
                //otherwise do our own comparison
                comparatorResult = _.defaultComparator(valueA, valueB, this.gridOptionsWrapper.isAccentedSort());
            }
            if (comparatorResult !== 0) {
                return comparatorResult * sortOption.inverter;
            }
        }
        // All matched, we make is so that the original sort order is kept:
        return sortedNodeA.currentPos - sortedNodeB.currentPos;
    };
    SortService.prototype.getValue = function (nodeA, column) {
        return this.valueService.getValue(column, nodeA);
    };
    SortService.prototype.updateChildIndexes = function (rowNode) {
        if (_.missing(rowNode.childrenAfterSort)) {
            return;
        }
        var listToSort = rowNode.childrenAfterSort;
        for (var i = 0; i < listToSort.length; i++) {
            var child = listToSort[i];
            var firstChild = i === 0;
            var lastChild = i === rowNode.childrenAfterSort.length - 1;
            child.setFirstChild(firstChild);
            child.setLastChild(lastChild);
            child.setChildIndex(i);
        }
    };
    SortService.prototype.updateGroupDataForHiddenOpenParents = function (changedPath) {
        var _this = this;
        if (!this.gridOptionsWrapper.isGroupHideOpenParents()) {
            return;
        }
        // recurse breadth first over group nodes after sort to 'pull down' group data to child groups
        var callback = function (rowNode) {
            _this.pullDownGroupDataForHideOpenParents(rowNode.childrenAfterSort, false);
            rowNode.childrenAfterSort.forEach(function (child) {
                if (child.hasChildren()) {
                    callback(child);
                }
            });
        };
        changedPath.executeFromRootNode(function (rowNode) { return callback(rowNode); });
    };
    SortService.prototype.pullDownGroupDataForHideOpenParents = function (rowNodes, clearOperation) {
        var _this = this;
        if (_.missing(rowNodes)) {
            return;
        }
        if (!this.gridOptionsWrapper.isGroupHideOpenParents()) {
            return;
        }
        rowNodes.forEach(function (childRowNode) {
            var groupDisplayCols = _this.columnController.getGroupDisplayColumns();
            groupDisplayCols.forEach(function (groupDisplayCol) {
                var showRowGroup = groupDisplayCol.getColDef().showRowGroup;
                if (typeof showRowGroup !== 'string') {
                    console.error('ag-Grid: groupHideOpenParents only works when specifying specific columns for colDef.showRowGroup');
                    return;
                }
                var displayingGroupKey = showRowGroup;
                var rowGroupColumn = _this.columnController.getPrimaryColumn(displayingGroupKey);
                var thisRowNodeMatches = rowGroupColumn === childRowNode.rowGroupColumn;
                if (thisRowNodeMatches) {
                    return;
                }
                if (clearOperation) {
                    // if doing a clear operation, we clear down the value for every possible group column
                    childRowNode.setGroupValue(groupDisplayCol.getId(), null);
                }
                else {
                    // if doing a set operation, we set only where the pull down is to occur
                    var parentToStealFrom = childRowNode.getFirstChildOfFirstChild(rowGroupColumn);
                    if (parentToStealFrom) {
                        childRowNode.setGroupValue(groupDisplayCol.getId(), parentToStealFrom.key);
                    }
                }
            });
        });
    };
    __decorate$4([
        Autowired('sortController')
    ], SortService.prototype, "sortController", void 0);
    __decorate$4([
        Autowired('columnController')
    ], SortService.prototype, "columnController", void 0);
    __decorate$4([
        Autowired('valueService')
    ], SortService.prototype, "valueService", void 0);
    __decorate$4([
        Autowired('gridOptionsWrapper')
    ], SortService.prototype, "gridOptionsWrapper", void 0);
    __decorate$4([
        PostConstruct
    ], SortService.prototype, "init", null);
    SortService = __decorate$4([
        Bean('sortService')
    ], SortService);
    return SortService;
}());

var __decorate$5 = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var FilterService = /** @class */ (function () {
    function FilterService() {
    }
    FilterService.prototype.postConstruct = function () {
        this.doingTreeData = this.gridOptionsWrapper.isTreeData();
    };
    FilterService.prototype.filter = function (changedPath) {
        var filterActive = this.filterManager.isAnyFilterPresent();
        this.filterNodes(filterActive, changedPath);
    };
    FilterService.prototype.filterNodes = function (filterActive, changedPath) {
        var _this = this;
        var filterCallback = function (rowNode, includeChildNodes) {
            // recursively get all children that are groups to also filter
            if (rowNode.hasChildren()) {
                // result of filter for this node. when filtering tree data, includeChildNodes = true when parent passes
                if (filterActive && !includeChildNodes) {
                    rowNode.childrenAfterFilter = rowNode.childrenAfterGroup.filter(function (childNode) {
                        // a group is included in the result if it has any children of it's own.
                        // by this stage, the child groups are already filtered
                        var passBecauseChildren = childNode.childrenAfterFilter && childNode.childrenAfterFilter.length > 0;
                        // both leaf level nodes and tree data nodes have data. these get added if
                        // the data passes the filter
                        var passBecauseDataPasses = childNode.data && _this.filterManager.doesRowPassFilter(childNode);
                        // note - tree data nodes pass either if a) they pass themselves or b) any children of that node pass
                        return passBecauseChildren || passBecauseDataPasses;
                    });
                }
                else {
                    // if not filtering, the result is the original list
                    rowNode.childrenAfterFilter = rowNode.childrenAfterGroup;
                }
                _this.setAllChildrenCount(rowNode);
            }
            else {
                rowNode.childrenAfterFilter = rowNode.childrenAfterGroup;
                rowNode.setAllChildrenCount(null);
            }
        };
        if (this.doingTreeDataFiltering()) {
            var treeDataDepthFirstFilter_1 = function (rowNode, alreadyFoundInParent) {
                // tree data filter traverses the hierarchy depth first and includes child nodes if parent passes
                // filter, and parent nodes will be include if any children exist.
                if (rowNode.childrenAfterGroup) {
                    for (var i = 0; i < rowNode.childrenAfterGroup.length; i++) {
                        var childNode = rowNode.childrenAfterGroup[i];
                        // first check if current node passes filter before invoking child nodes
                        var foundInParent = alreadyFoundInParent || _this.filterManager.doesRowPassFilter(childNode);
                        if (childNode.childrenAfterGroup) {
                            treeDataDepthFirstFilter_1(rowNode.childrenAfterGroup[i], foundInParent);
                        }
                        else {
                            filterCallback(childNode, foundInParent);
                        }
                    }
                }
                filterCallback(rowNode, alreadyFoundInParent);
            };
            var treeDataFilterCallback = function (rowNode) { return treeDataDepthFirstFilter_1(rowNode, false); };
            changedPath.executeFromRootNode(treeDataFilterCallback);
        }
        else {
            var defaultFilterCallback = function (rowNode) { return filterCallback(rowNode, false); };
            changedPath.forEachChangedNodeDepthFirst(defaultFilterCallback, true);
        }
    };
    FilterService.prototype.setAllChildrenCountTreeData = function (rowNode) {
        // for tree data, we include all children, groups and leafs
        var allChildrenCount = 0;
        rowNode.childrenAfterFilter.forEach(function (child) {
            // include child itself
            allChildrenCount++;
            // include children of children
            allChildrenCount += child.allChildrenCount;
        });
        rowNode.setAllChildrenCount(allChildrenCount);
    };
    FilterService.prototype.setAllChildrenCountGridGrouping = function (rowNode) {
        // for grid data, we only count the leafs
        var allChildrenCount = 0;
        rowNode.childrenAfterFilter.forEach(function (child) {
            if (child.group) {
                allChildrenCount += child.allChildrenCount;
            }
            else {
                allChildrenCount++;
            }
        });
        rowNode.setAllChildrenCount(allChildrenCount);
    };
    FilterService.prototype.setAllChildrenCount = function (rowNode) {
        if (this.doingTreeData) {
            this.setAllChildrenCountTreeData(rowNode);
        }
        else {
            this.setAllChildrenCountGridGrouping(rowNode);
        }
    };
    FilterService.prototype.doingTreeDataFiltering = function () {
        return this.gridOptionsWrapper.isTreeData() && !this.gridOptionsWrapper.isExcludeChildrenWhenTreeDataFiltering();
    };
    __decorate$5([
        Autowired('filterManager')
    ], FilterService.prototype, "filterManager", void 0);
    __decorate$5([
        Autowired('gridOptionsWrapper')
    ], FilterService.prototype, "gridOptionsWrapper", void 0);
    __decorate$5([
        PostConstruct
    ], FilterService.prototype, "postConstruct", null);
    FilterService = __decorate$5([
        Bean("filterService")
    ], FilterService);
    return FilterService;
}());

var __decorate$6 = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var ImmutableService = /** @class */ (function () {
    function ImmutableService() {
    }
    ImmutableService.prototype.postConstruct = function () {
        if (this.rowModel.getType() === Constants.ROW_MODEL_TYPE_CLIENT_SIDE) {
            this.clientSideRowModel = this.rowModel;
        }
    };
    // converts the setRowData() command to a transaction
    ImmutableService.prototype.createTransactionForRowData = function (data) {
        if (_.missing(this.clientSideRowModel)) {
            console.error('ag-Grid: ImmutableService only works with ClientSideRowModel');
            return;
        }
        var getRowNodeIdFunc = this.gridOptionsWrapper.getRowNodeIdFunc();
        if (!getRowNodeIdFunc || _.missing(getRowNodeIdFunc)) {
            console.error('ag-Grid: ImmutableService requires getRowNodeId() callback to be implemented, your row data need IDs!');
            return;
        }
        // convert the data into a transaction object by working out adds, removes and updates
        var transaction = {
            remove: [],
            update: [],
            add: []
        };
        var existingNodesMap = this.clientSideRowModel.getCopyOfNodesMap();
        var suppressSortOrder = this.gridOptionsWrapper.isSuppressMaintainUnsortedOrder();
        var orderMap = suppressSortOrder ? null : {};
        if (_.exists(data)) {
            // split all the new data in the following:
            // if new, push to 'add'
            // if update, push to 'update'
            // if not changed, do not include in the transaction
            data.forEach(function (dataItem, index) {
                var id = getRowNodeIdFunc(dataItem);
                var existingNode = existingNodesMap[id];
                if (orderMap) {
                    orderMap[id] = index;
                }
                if (existingNode) {
                    var dataHasChanged = existingNode.data !== dataItem;
                    if (dataHasChanged) {
                        transaction.update.push(dataItem);
                    }
                    // otherwise, if data not changed, we just don't include it anywhere, as it's not a delta
                    // remove from list, so we know the item is not to be removed
                    existingNodesMap[id] = undefined;
                }
                else {
                    transaction.add.push(dataItem);
                }
            });
        }
        // at this point, all rows that are left, should be removed
        _.iterateObject(existingNodesMap, function (id, rowNode) {
            if (rowNode) {
                transaction.remove.push(rowNode.data);
            }
        });
        return [transaction, orderMap];
    };
    __decorate$6([
        Autowired('rowModel')
    ], ImmutableService.prototype, "rowModel", void 0);
    __decorate$6([
        Autowired('gridOptionsWrapper')
    ], ImmutableService.prototype, "gridOptionsWrapper", void 0);
    __decorate$6([
        PostConstruct
    ], ImmutableService.prototype, "postConstruct", null);
    ImmutableService = __decorate$6([
        Bean('immutableService')
    ], ImmutableService);
    return ImmutableService;
}());

var ClientSideRowModelModule = {
    moduleName: ModuleNames.ClientSideRowModelModule,
    beans: [FilterStage, SortStage, FlattenStage, SortService, FilterService, ImmutableService],
    rowModels: { clientSide: ClientSideRowModel }
};

var __extends = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate$7 = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var InfiniteBlock = /** @class */ (function (_super) {
    __extends(InfiniteBlock, _super);
    function InfiniteBlock(pageNumber, params) {
        var _this = _super.call(this, pageNumber, params) || this;
        _this.cacheParams = params;
        return _this;
    }
    InfiniteBlock.prototype.createBlankRowNode = function (rowIndex) {
        var rowNode = _super.prototype.createBlankRowNode.call(this, rowIndex);
        rowNode.uiLevel = 0;
        this.setIndexAndTopOnRowNode(rowNode, rowIndex);
        return rowNode;
    };
    InfiniteBlock.prototype.setDataAndId = function (rowNode, data, index) {
        if (_.exists(data)) {
            // this means if the user is not providing id's we just use the
            // index for the row. this will allow selection to work (that is based
            // on index) as long user is not inserting or deleting rows,
            // or wanting to keep selection between server side sorting or filtering
            rowNode.setDataAndId(data, index.toString());
        }
        else {
            rowNode.setDataAndId(undefined, undefined);
        }
    };
    InfiniteBlock.prototype.setRowNode = function (rowIndex, rowNode) {
        _super.prototype.setRowNode.call(this, rowIndex, rowNode);
        this.setIndexAndTopOnRowNode(rowNode, rowIndex);
    };
    InfiniteBlock.prototype.init = function () {
        _super.prototype.init.call(this, {
            context: this.getContext(),
            rowRenderer: this.rowRenderer
        });
    };
    InfiniteBlock.prototype.getNodeIdPrefix = function () {
        return null;
    };
    InfiniteBlock.prototype.getRow = function (displayIndex) {
        return this.getRowUsingLocalIndex(displayIndex);
    };
    InfiniteBlock.prototype.setIndexAndTopOnRowNode = function (rowNode, rowIndex) {
        rowNode.setRowIndex(rowIndex);
        rowNode.rowTop = this.cacheParams.rowHeight * rowIndex;
    };
    InfiniteBlock.prototype.loadFromDatasource = function () {
        var _this = this;
        // PROBLEM . . . . when the user sets sort via colDef.sort, then this code
        // is executing before the sort is set up, so server is not getting the sort
        // model. need to change with regards order - so the server side request is
        // AFTER thus it gets the right sort model.
        var params = {
            startRow: this.getStartRow(),
            endRow: this.getEndRow(),
            successCallback: this.pageLoaded.bind(this, this.getVersion()),
            failCallback: this.pageLoadFailed.bind(this),
            sortModel: this.cacheParams.sortModel,
            filterModel: this.cacheParams.filterModel,
            context: this.gridOptionsWrapper.getContext()
        };
        if (_.missing(this.cacheParams.datasource.getRows)) {
            console.warn("ag-Grid: datasource is missing getRows method");
            return;
        }
        // put in timeout, to force result to be async
        window.setTimeout(function () {
            _this.cacheParams.datasource.getRows(params);
        }, 0);
    };
    __decorate$7([
        Autowired('gridOptionsWrapper')
    ], InfiniteBlock.prototype, "gridOptionsWrapper", void 0);
    __decorate$7([
        Autowired('rowRenderer')
    ], InfiniteBlock.prototype, "rowRenderer", void 0);
    __decorate$7([
        PostConstruct
    ], InfiniteBlock.prototype, "init", null);
    return InfiniteBlock;
}(RowNodeBlock));

var __extends$1 = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate$8 = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __param = (undefined && undefined.__param) || function (paramIndex, decorator) {
    return function (target, key) { decorator(target, key, paramIndex); }
};
var InfiniteCache = /** @class */ (function (_super) {
    __extends$1(InfiniteCache, _super);
    function InfiniteCache(params) {
        return _super.call(this, params) || this;
    }
    InfiniteCache.prototype.setBeans = function (loggerFactory) {
        this.logger = loggerFactory.create('InfiniteCache');
    };
    InfiniteCache.prototype.init = function () {
        _super.prototype.init.call(this);
        // start load of data, as the virtualRowCount will remain at 0 otherwise,
        // so we need this to kick things off, otherwise grid would never call getRow()
        this.getRow(0);
    };
    InfiniteCache.prototype.moveItemsDown = function (block, moveFromIndex, moveCount) {
        var startRow = block.getStartRow();
        var endRow = block.getEndRow();
        var indexOfLastRowToMove = moveFromIndex + moveCount;
        // all rows need to be moved down below the insertion index
        for (var currentRowIndex = endRow - 1; currentRowIndex >= startRow; currentRowIndex--) {
            // don't move rows at or before the insertion index
            if (currentRowIndex < indexOfLastRowToMove) {
                continue;
            }
            var indexOfNodeWeWant = currentRowIndex - moveCount;
            var nodeForThisIndex = this.getRow(indexOfNodeWeWant, true);
            if (nodeForThisIndex) {
                block.setRowNode(currentRowIndex, nodeForThisIndex);
            }
            else {
                block.setBlankRowNode(currentRowIndex);
                block.setDirty();
            }
        }
    };
    InfiniteCache.prototype.insertItems = function (block, indexToInsert, items) {
        var pageStartRow = block.getStartRow();
        var pageEndRow = block.getEndRow();
        var newRowNodes = [];
        // next stage is insert the rows into this page, if applicable
        for (var index = 0; index < items.length; index++) {
            var rowIndex = indexToInsert + index;
            var currentRowInThisPage = rowIndex >= pageStartRow && rowIndex < pageEndRow;
            if (currentRowInThisPage) {
                var dataItem = items[index];
                var newRowNode = block.setNewData(rowIndex, dataItem);
                newRowNodes.push(newRowNode);
            }
        }
        return newRowNodes;
    };
    InfiniteCache.prototype.insertItemsAtIndex = function (indexToInsert, items) {
        // get all page id's as NUMBERS (not strings, as we need to sort as numbers) and in descending order
        var _this = this;
        var newNodes = [];
        this.forEachBlockInReverseOrder(function (block) {
            var pageEndRow = block.getEndRow();
            // if the insertion is after this page, then this page is not impacted
            if (pageEndRow <= indexToInsert) {
                return;
            }
            _this.moveItemsDown(block, indexToInsert, items.length);
            var newNodesThisPage = _this.insertItems(block, indexToInsert, items);
            newNodesThisPage.forEach(function (rowNode) { return newNodes.push(rowNode); });
        });
        if (this.isMaxRowFound()) {
            this.hack_setVirtualRowCount(this.getVirtualRowCount() + items.length);
        }
        this.onCacheUpdated();
        var event = {
            type: Events.EVENT_ROW_DATA_UPDATED,
            api: this.gridApi,
            columnApi: this.columnApi
        };
        this.eventService.dispatchEvent(event);
    };
    // the rowRenderer will not pass dontCreatePage, meaning when rendering the grid,
    // it will want new pages in the cache as it asks for rows. only when we are inserting /
    // removing rows via the api is dontCreatePage set, where we move rows between the pages.
    InfiniteCache.prototype.getRow = function (rowIndex, dontCreatePage) {
        if (dontCreatePage === void 0) { dontCreatePage = false; }
        var blockId = Math.floor(rowIndex / this.cacheParams.blockSize);
        var block = this.getBlock(blockId);
        if (!block) {
            if (dontCreatePage) {
                return null;
            }
            else {
                block = this.createBlock(blockId);
            }
        }
        return block.getRow(rowIndex);
    };
    InfiniteCache.prototype.createBlock = function (blockNumber) {
        var newBlock = new InfiniteBlock(blockNumber, this.cacheParams);
        this.getContext().wireBean(newBlock);
        this.postCreateBlock(newBlock);
        return newBlock;
    };
    // we have this on infinite row model only, not server side row model,
    // because for server side, it would leave the children in inconsistent
    // state - eg if a node had children, but after the refresh it had data
    // for a different row, then the children would be with the wrong row node.
    InfiniteCache.prototype.refreshCache = function () {
        this.forEachBlockInOrder(function (block) { return block.setDirty(); });
        this.checkBlockToLoad();
    };
    __decorate$8([
        Autowired('eventService')
    ], InfiniteCache.prototype, "eventService", void 0);
    __decorate$8([
        Autowired('columnApi')
    ], InfiniteCache.prototype, "columnApi", void 0);
    __decorate$8([
        Autowired('gridApi')
    ], InfiniteCache.prototype, "gridApi", void 0);
    __decorate$8([
        __param(0, Qualifier('loggerFactory'))
    ], InfiniteCache.prototype, "setBeans", null);
    __decorate$8([
        PostConstruct
    ], InfiniteCache.prototype, "init", null);
    return InfiniteCache;
}(RowNodeCache));

var __extends$2 = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate$9 = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var InfiniteRowModel = /** @class */ (function (_super) {
    __extends$2(InfiniteRowModel, _super);
    function InfiniteRowModel() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    InfiniteRowModel.prototype.getRowBounds = function (index) {
        return {
            rowHeight: this.rowHeight,
            rowTop: this.rowHeight * index
        };
    };
    // we don't implement as lazy row heights is not supported in this row model
    InfiniteRowModel.prototype.ensureRowHeightsValid = function (startPixel, endPixel, startLimitIndex, endLimitIndex) {
        return false;
    };
    InfiniteRowModel.prototype.init = function () {
        var _this = this;
        if (!this.gridOptionsWrapper.isRowModelInfinite()) {
            return;
        }
        this.rowHeight = this.gridOptionsWrapper.getRowHeightAsNumber();
        this.addEventListeners();
        this.addDestroyFunc(function () { return _this.destroyCache(); });
    };
    InfiniteRowModel.prototype.start = function () {
        this.setDatasource(this.gridOptionsWrapper.getDatasource());
    };
    InfiniteRowModel.prototype.destroyDatasource = function () {
        if (this.datasource) {
            if (this.datasource.destroy) {
                this.datasource.destroy();
            }
            this.rowRenderer.datasourceChanged();
            this.datasource = null;
        }
    };
    InfiniteRowModel.prototype.isLastRowFound = function () {
        return this.infiniteCache ? this.infiniteCache.isMaxRowFound() : false;
    };
    InfiniteRowModel.prototype.addEventListeners = function () {
        this.addDestroyableEventListener(this.eventService, Events.EVENT_FILTER_CHANGED, this.onFilterChanged.bind(this));
        this.addDestroyableEventListener(this.eventService, Events.EVENT_SORT_CHANGED, this.onSortChanged.bind(this));
        this.addDestroyableEventListener(this.eventService, Events.EVENT_COLUMN_EVERYTHING_CHANGED, this.onColumnEverything.bind(this));
    };
    InfiniteRowModel.prototype.onFilterChanged = function () {
        this.reset();
    };
    InfiniteRowModel.prototype.onSortChanged = function () {
        this.reset();
    };
    InfiniteRowModel.prototype.onColumnEverything = function () {
        var resetRequired;
        // if cache params, we require reset only if sort model has changed. we don't need to check
        // for filter model, as the filter manager will fire an event when columns change that result
        // in the filter changing.
        if (this.cacheParams) {
            resetRequired = this.isSortModelDifferent();
        }
        else {
            // if no cacheParams, means first time creating the cache, so always create one
            resetRequired = true;
        }
        if (resetRequired) {
            this.reset();
        }
    };
    InfiniteRowModel.prototype.isSortModelDifferent = function () {
        return !_.jsonEquals(this.cacheParams.sortModel, this.sortController.getSortModel());
    };
    InfiniteRowModel.prototype.getType = function () {
        return Constants.ROW_MODEL_TYPE_INFINITE;
    };
    InfiniteRowModel.prototype.setDatasource = function (datasource) {
        this.destroyDatasource();
        this.datasource = datasource;
        // only reset if we have a valid datasource to working with
        if (datasource) {
            this.checkForDeprecated();
            this.reset();
        }
    };
    InfiniteRowModel.prototype.checkForDeprecated = function () {
        var ds = this.datasource;
        // the number of concurrent loads we are allowed to the server
        if (_.exists(ds.maxConcurrentRequests)) {
            console.error('ag-Grid: since version 5.1.x, maxConcurrentRequests is replaced with grid property maxConcurrentDatasourceRequests');
        }
        if (_.exists(ds.maxPagesInCache)) {
            console.error('ag-Grid: since version 5.1.x, maxPagesInCache is replaced with grid property maxPagesInPaginationCache');
        }
        if (_.exists(ds.overflowSize)) {
            console.error('ag-Grid: since version 5.1.x, overflowSize is replaced with grid property paginationOverflowSize');
        }
        if (_.exists(ds.blockSize)) {
            console.error('ag-Grid: since version 5.1.x, pageSize/blockSize is replaced with grid property infinitePageSize');
        }
    };
    InfiniteRowModel.prototype.isEmpty = function () {
        return _.missing(this.infiniteCache);
    };
    InfiniteRowModel.prototype.isRowsToRender = function () {
        return _.exists(this.infiniteCache);
    };
    InfiniteRowModel.prototype.getNodesInRangeForSelection = function (firstInRange, lastInRange) {
        return this.infiniteCache ? this.infiniteCache.getRowNodesInRange(firstInRange, lastInRange) : [];
    };
    InfiniteRowModel.prototype.reset = function () {
        // important to return here, as the user could be setting filter or sort before
        // data-source is set
        if (_.missing(this.datasource)) {
            return;
        }
        // if user is providing id's, then this means we can keep the selection between datasource hits,
        // as the rows will keep their unique id's even if, for example, server side sorting or filtering
        // is done.
        var userGeneratingIds = _.exists(this.gridOptionsWrapper.getRowNodeIdFunc());
        if (!userGeneratingIds) {
            this.selectionController.reset();
        }
        this.resetCache();
        var event = this.createModelUpdatedEvent();
        this.eventService.dispatchEvent(event);
    };
    InfiniteRowModel.prototype.createModelUpdatedEvent = function () {
        return {
            type: Events.EVENT_MODEL_UPDATED,
            api: this.gridApi,
            columnApi: this.columnApi,
            // not sure if these should all be false - noticed if after implementing,
            // maybe they should be true?
            newPage: false,
            newData: false,
            keepRenderedRows: false,
            animate: false
        };
    };
    InfiniteRowModel.prototype.resetCache = function () {
        // if not first time creating a cache, need to destroy the old one
        this.destroyCache();
        var maxConcurrentRequests = this.gridOptionsWrapper.getMaxConcurrentDatasourceRequests();
        var blockLoadDebounceMillis = this.gridOptionsWrapper.getBlockLoadDebounceMillis();
        // there is a bi-directional dependency between the loader and the cache,
        // so we create loader here, and then pass dependencies in setDependencies() method later
        this.rowNodeBlockLoader = new RowNodeBlockLoader(maxConcurrentRequests, blockLoadDebounceMillis);
        this.getContext().wireBean(this.rowNodeBlockLoader);
        this.cacheParams = {
            // the user provided datasource
            datasource: this.datasource,
            // sort and filter model
            filterModel: this.filterManager.getFilterModel(),
            sortModel: this.sortController.getSortModel(),
            rowNodeBlockLoader: this.rowNodeBlockLoader,
            // properties - this way we take a snapshot of them, so if user changes any, they will be
            // used next time we create a new cache, which is generally after a filter or sort change,
            // or a new datasource is set
            maxConcurrentRequests: maxConcurrentRequests,
            overflowSize: this.gridOptionsWrapper.getCacheOverflowSize(),
            initialRowCount: this.gridOptionsWrapper.getInfiniteInitialRowCount(),
            maxBlocksInCache: this.gridOptionsWrapper.getMaxBlocksInCache(),
            blockSize: this.gridOptionsWrapper.getCacheBlockSize(),
            rowHeight: this.gridOptionsWrapper.getRowHeightAsNumber(),
            // the cache could create this, however it is also used by the pages, so handy to create it
            // here as the settings are also passed to the pages
            lastAccessedSequence: new NumberSequence()
        };
        // set defaults
        if (!this.cacheParams.maxConcurrentRequests || !(this.cacheParams.maxConcurrentRequests >= 1)) {
            this.cacheParams.maxConcurrentRequests = 2;
        }
        // page size needs to be 1 or greater. having it at 1 would be silly, as you would be hitting the
        // server for one page at a time. so the default if not specified is 100.
        if (!this.cacheParams.blockSize || !(this.cacheParams.blockSize >= 1)) {
            this.cacheParams.blockSize = 100;
        }
        // if user doesn't give initial rows to display, we assume zero
        if (!(this.cacheParams.initialRowCount >= 1)) {
            this.cacheParams.initialRowCount = 0;
        }
        // if user doesn't provide overflow, we use default overflow of 1, so user can scroll past
        // the current page and request first row of next page
        if (!(this.cacheParams.overflowSize >= 1)) {
            this.cacheParams.overflowSize = 1;
        }
        this.infiniteCache = new InfiniteCache(this.cacheParams);
        this.getContext().wireBean(this.infiniteCache);
        this.infiniteCache.addEventListener(RowNodeCache.EVENT_CACHE_UPDATED, this.onCacheUpdated.bind(this));
    };
    InfiniteRowModel.prototype.destroyCache = function () {
        if (this.infiniteCache) {
            this.infiniteCache.destroy();
            this.infiniteCache = null;
        }
        if (this.rowNodeBlockLoader) {
            this.rowNodeBlockLoader.destroy();
            this.rowNodeBlockLoader = null;
        }
    };
    InfiniteRowModel.prototype.onCacheUpdated = function () {
        var event = this.createModelUpdatedEvent();
        this.eventService.dispatchEvent(event);
    };
    InfiniteRowModel.prototype.getRow = function (rowIndex) {
        return this.infiniteCache ? this.infiniteCache.getRow(rowIndex) : null;
    };
    InfiniteRowModel.prototype.getRowNode = function (id) {
        var result = null;
        this.forEachNode(function (rowNode) {
            if (rowNode.id === id) {
                result = rowNode;
            }
        });
        return result;
    };
    InfiniteRowModel.prototype.forEachNode = function (callback) {
        if (this.infiniteCache) {
            this.infiniteCache.forEachNodeDeep(callback, new NumberSequence());
        }
    };
    InfiniteRowModel.prototype.getCurrentPageHeight = function () {
        return this.getRowCount() * this.rowHeight;
    };
    InfiniteRowModel.prototype.getTopLevelRowCount = function () {
        return this.getRowCount();
    };
    InfiniteRowModel.prototype.getTopLevelRowDisplayedIndex = function (topLevelIndex) {
        return topLevelIndex;
    };
    InfiniteRowModel.prototype.getRowIndexAtPixel = function (pixel) {
        if (this.rowHeight !== 0) { // avoid divide by zero error
            var rowIndexForPixel = Math.floor(pixel / this.rowHeight);
            var lastRowIndex = this.getRowCount() - 1;
            if (rowIndexForPixel > lastRowIndex) {
                return lastRowIndex;
            }
            else {
                return rowIndexForPixel;
            }
        }
        else {
            return 0;
        }
    };
    InfiniteRowModel.prototype.getRowCount = function () {
        return this.infiniteCache ? this.infiniteCache.getVirtualRowCount() : 0;
    };
    InfiniteRowModel.prototype.updateRowData = function (transaction) {
        if (_.exists(transaction.remove) || _.exists(transaction.update)) {
            console.warn('ag-Grid: updateRowData for InfiniteRowModel does not support remove or update, only add');
            return;
        }
        if (_.missing(transaction.addIndex)) {
            console.warn('ag-Grid: updateRowData for InfiniteRowModel requires add and addIndex to be set');
            return;
        }
        if (this.infiniteCache) {
            this.infiniteCache.insertItemsAtIndex(transaction.addIndex, transaction.add);
        }
    };
    InfiniteRowModel.prototype.isRowPresent = function (rowNode) {
        return false;
    };
    InfiniteRowModel.prototype.refreshCache = function () {
        if (this.infiniteCache) {
            this.infiniteCache.refreshCache();
        }
    };
    InfiniteRowModel.prototype.purgeCache = function () {
        if (this.infiniteCache) {
            this.infiniteCache.purgeCache();
        }
    };
    InfiniteRowModel.prototype.getVirtualRowCount = function () {
        if (this.infiniteCache) {
            return this.infiniteCache.getVirtualRowCount();
        }
        else {
            return null;
        }
    };
    InfiniteRowModel.prototype.isMaxRowFound = function () {
        if (this.infiniteCache) {
            return this.infiniteCache.isMaxRowFound();
        }
    };
    InfiniteRowModel.prototype.setVirtualRowCount = function (rowCount, maxRowFound) {
        if (this.infiniteCache) {
            this.infiniteCache.setVirtualRowCount(rowCount, maxRowFound);
        }
    };
    InfiniteRowModel.prototype.getBlockState = function () {
        if (this.rowNodeBlockLoader) {
            return this.rowNodeBlockLoader.getBlockState();
        }
        else {
            return null;
        }
    };
    __decorate$9([
        Autowired('gridOptionsWrapper')
    ], InfiniteRowModel.prototype, "gridOptionsWrapper", void 0);
    __decorate$9([
        Autowired('filterManager')
    ], InfiniteRowModel.prototype, "filterManager", void 0);
    __decorate$9([
        Autowired('sortController')
    ], InfiniteRowModel.prototype, "sortController", void 0);
    __decorate$9([
        Autowired('selectionController')
    ], InfiniteRowModel.prototype, "selectionController", void 0);
    __decorate$9([
        Autowired('eventService')
    ], InfiniteRowModel.prototype, "eventService", void 0);
    __decorate$9([
        Autowired('gridApi')
    ], InfiniteRowModel.prototype, "gridApi", void 0);
    __decorate$9([
        Autowired('columnApi')
    ], InfiniteRowModel.prototype, "columnApi", void 0);
    __decorate$9([
        Autowired('rowRenderer')
    ], InfiniteRowModel.prototype, "rowRenderer", void 0);
    __decorate$9([
        PostConstruct
    ], InfiniteRowModel.prototype, "init", null);
    __decorate$9([
        PreDestroy
    ], InfiniteRowModel.prototype, "destroyDatasource", null);
    InfiniteRowModel = __decorate$9([
        Bean('rowModel')
    ], InfiniteRowModel);
    return InfiniteRowModel;
}(BeanStub));

var InfiniteRowModelModule = {
    moduleName: ModuleNames.InfiniteRowModelModule,
    rowModels: { 'infinite': InfiniteRowModel }
};

var __decorate$a = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var BaseGridSerializingSession = /** @class */ (function () {
    function BaseGridSerializingSession(config) {
        var columnController = config.columnController, valueService = config.valueService, gridOptionsWrapper = config.gridOptionsWrapper, processCellCallback = config.processCellCallback, processHeaderCallback = config.processHeaderCallback, processGroupHeaderCallback = config.processGroupHeaderCallback, processRowGroupCallback = config.processRowGroupCallback;
        this.columnController = columnController;
        this.valueService = valueService;
        this.gridOptionsWrapper = gridOptionsWrapper;
        this.processCellCallback = processCellCallback;
        this.processHeaderCallback = processHeaderCallback;
        this.processGroupHeaderCallback = processGroupHeaderCallback;
        this.processRowGroupCallback = processRowGroupCallback;
    }
    BaseGridSerializingSession.prototype.prepare = function (columnsToExport) {
        this.firstGroupColumn = _.find(columnsToExport, function (col) { return !!col.getColDef().showRowGroup; });
    };
    BaseGridSerializingSession.prototype.extractHeaderValue = function (column) {
        var value = this.getHeaderName(this.processHeaderCallback, column);
        return value != null ? value : '';
    };
    BaseGridSerializingSession.prototype.extractRowCellValue = function (column, index, type, node) {
        // we render the group summary text e.g. "-> Parent -> Child"...
        var renderGroupSummaryCell = 
        // on group rows
        node && node.group
            && (
            // in the first group column if groups appear in regular grid cells
            column === this.firstGroupColumn
                // or the first cell in the row, if we're doing full width rows
                || (index === 0 && this.gridOptionsWrapper.isGroupUseEntireRow(this.columnController.isPivotMode())));
        var valueForCell;
        if (renderGroupSummaryCell) {
            valueForCell = this.createValueForGroupNode(node);
        }
        else {
            valueForCell = this.valueService.getValue(column, node);
        }
        var value = this.processCell(node, column, valueForCell, this.processCellCallback, type);
        return value != null ? value : '';
    };
    BaseGridSerializingSession.prototype.getHeaderName = function (callback, column) {
        if (callback) {
            return callback({
                column: column,
                api: this.gridOptionsWrapper.getApi(),
                columnApi: this.gridOptionsWrapper.getColumnApi(),
                context: this.gridOptionsWrapper.getContext()
            });
        }
        else {
            return this.columnController.getDisplayNameForColumn(column, 'csv', true);
        }
    };
    BaseGridSerializingSession.prototype.createValueForGroupNode = function (node) {
        if (this.processRowGroupCallback) {
            return this.processRowGroupCallback({
                node: node,
                api: this.gridOptionsWrapper.getApi(),
                columnApi: this.gridOptionsWrapper.getColumnApi(),
                context: this.gridOptionsWrapper.getContext(),
            });
        }
        var keys = [node.key];
        while (node.parent) {
            node = node.parent;
            keys.push(node.key);
        }
        return keys.reverse().join(' -> ');
    };
    BaseGridSerializingSession.prototype.processCell = function (rowNode, column, value, processCellCallback, type) {
        if (processCellCallback) {
            return processCellCallback({
                column: column,
                node: rowNode,
                value: value,
                api: this.gridOptionsWrapper.getApi(),
                columnApi: this.gridOptionsWrapper.getColumnApi(),
                context: this.gridOptionsWrapper.getContext(),
                type: type
            });
        }
        else {
            return value;
        }
    };
    return BaseGridSerializingSession;
}());
var GridSerializer = /** @class */ (function () {
    function GridSerializer() {
    }
    GridSerializer.prototype.serialize = function (gridSerializingSession, params) {
        if (params === void 0) { params = {}; }
        var rowSkipper = params.shouldRowBeSkipped || (function () { return false; });
        var api = this.gridOptionsWrapper.getApi();
        var columnApi = this.gridOptionsWrapper.getColumnApi();
        var skipSingleChildrenGroup = this.gridOptionsWrapper.isGroupRemoveSingleChildren();
        var skipLowestSingleChildrenGroup = this.gridOptionsWrapper.isGroupRemoveLowestSingleChildren();
        var context = this.gridOptionsWrapper.getContext();
        // when in pivot mode, we always render cols on screen, never 'all columns'
        var isPivotMode = this.columnController.isPivotMode();
        var rowModelNormal = this.rowModel.getType() === Constants.ROW_MODEL_TYPE_CLIENT_SIDE;
        var onlySelectedNonStandardModel = !rowModelNormal && params.onlySelected;
        var columnsToExport = [];
        if (_.existsAndNotEmpty(params.columnKeys)) {
            columnsToExport = this.columnController.getGridColumns(params.columnKeys);
        }
        else if (params.allColumns && !isPivotMode) {
            // add auto group column for tree data
            columnsToExport = this.gridOptionsWrapper.isTreeData() ?
                this.columnController.getGridColumns([Constants.GROUP_AUTO_COLUMN_ID]) : [];
            columnsToExport = columnsToExport.concat(this.columnController.getAllPrimaryColumns() || []);
        }
        else {
            columnsToExport = this.columnController.getAllDisplayedColumns();
        }
        if (params.customHeader) {
            gridSerializingSession.addCustomContent(params.customHeader);
        }
        gridSerializingSession.prepare(columnsToExport);
        // first pass, put in the header names of the cols
        if (params.columnGroups) {
            var groupInstanceIdCreator = new GroupInstanceIdCreator();
            var displayedGroups = this.displayedGroupCreator.createDisplayedGroups(columnsToExport, this.columnController.getGridBalancedTree(), groupInstanceIdCreator, null);
            this.recursivelyAddHeaderGroups(displayedGroups, gridSerializingSession, params.processGroupHeaderCallback);
        }
        if (!params.skipHeader) {
            var gridRowIterator_1 = gridSerializingSession.onNewHeaderRow();
            columnsToExport.forEach(function (column, index) {
                gridRowIterator_1.onColumn(column, index, undefined);
            });
        }
        this.pinnedRowModel.forEachPinnedTopRow(processRow);
        if (isPivotMode) {
            if (this.rowModel.forEachPivotNode) {
                this.rowModel.forEachPivotNode(processRow);
            }
            else {
                //Must be enterprise, so we can just loop through all the nodes
                this.rowModel.forEachNode(processRow);
            }
        }
        else {
            // onlySelectedAllPages: user doing pagination and wants selected items from
            // other pages, so cannot use the standard row model as it won't have rows from
            // other pages.
            // onlySelectedNonStandardModel: if user wants selected in non standard row model
            // (eg viewport) then again rowmodel cannot be used, so need to use selected instead.
            if (params.onlySelectedAllPages || onlySelectedNonStandardModel) {
                var selectedNodes = this.selectionController.getSelectedNodes();
                selectedNodes.forEach(function (node) {
                    processRow(node);
                });
            }
            else {
                // here is everything else - including standard row model and selected. we don't use
                // the selection model even when just using selected, so that the result is the order
                // of the rows appearing on the screen.
                if (rowModelNormal) {
                    this.rowModel.forEachNodeAfterFilterAndSort(processRow);
                }
                else {
                    this.rowModel.forEachNode(processRow);
                }
            }
        }
        this.pinnedRowModel.forEachPinnedBottomRow(processRow);
        if (params.customFooter) {
            gridSerializingSession.addCustomContent(params.customFooter);
        }
        function processRow(node) {
            var shouldSkipLowestGroup = skipLowestSingleChildrenGroup && node.leafGroup;
            var shouldSkipCurrentGroup = node.allChildrenCount === 1 && (skipSingleChildrenGroup || shouldSkipLowestGroup);
            if (node.group && (params.skipGroups || shouldSkipCurrentGroup)) {
                return;
            }
            if (params.skipFooters && node.footer) {
                return;
            }
            if (params.onlySelected && !node.isSelected()) {
                return;
            }
            if (params.skipPinnedTop && node.rowPinned === 'top') {
                return;
            }
            if (params.skipPinnedBottom && node.rowPinned === 'bottom') {
                return;
            }
            // if we are in pivotMode, then the grid will show the root node only
            // if it's not a leaf group
            var nodeIsRootNode = node.level === -1;
            if (nodeIsRootNode && !node.leafGroup) {
                return;
            }
            var shouldRowBeSkipped = rowSkipper({ node: node, api: api, context: context });
            if (shouldRowBeSkipped) {
                return;
            }
            var rowAccumulator = gridSerializingSession.onNewBodyRow();
            columnsToExport.forEach(function (column, index) {
                rowAccumulator.onColumn(column, index, node);
            });
            if (params.getCustomContentBelowRow) {
                var content = params.getCustomContentBelowRow({ node: node, api: api, columnApi: columnApi, context: context });
                if (content) {
                    gridSerializingSession.addCustomContent(content);
                }
            }
        }
        return gridSerializingSession.parse();
    };
    GridSerializer.prototype.recursivelyAddHeaderGroups = function (displayedGroups, gridSerializingSession, processGroupHeaderCallback) {
        var directChildrenHeaderGroups = [];
        displayedGroups.forEach(function (columnGroupChild) {
            var columnGroup = columnGroupChild;
            if (!columnGroup.getChildren) {
                return;
            }
            columnGroup.getChildren().forEach(function (it) { return directChildrenHeaderGroups.push(it); });
        });
        if (displayedGroups.length > 0 && displayedGroups[0] instanceof ColumnGroup) {
            this.doAddHeaderHeader(gridSerializingSession, displayedGroups, processGroupHeaderCallback);
        }
        if (directChildrenHeaderGroups && directChildrenHeaderGroups.length > 0) {
            this.recursivelyAddHeaderGroups(directChildrenHeaderGroups, gridSerializingSession, processGroupHeaderCallback);
        }
    };
    GridSerializer.prototype.doAddHeaderHeader = function (gridSerializingSession, displayedGroups, processGroupHeaderCallback) {
        var _this = this;
        var gridRowIterator = gridSerializingSession.onNewHeaderGroupingRow();
        var columnIndex = 0;
        displayedGroups.forEach(function (columnGroupChild) {
            var columnGroup = columnGroupChild;
            var name;
            if (processGroupHeaderCallback) {
                name = processGroupHeaderCallback({
                    columnGroup: columnGroup,
                    api: _this.gridOptionsWrapper.getApi(),
                    columnApi: _this.gridOptionsWrapper.getColumnApi(),
                    context: _this.gridOptionsWrapper.getContext()
                });
            }
            else {
                name = _this.columnController.getDisplayNameForColumnGroup(columnGroup, 'header');
            }
            gridRowIterator.onColumn(name || '', columnIndex++, columnGroup.getLeafColumns().length - 1);
        });
    };
    __decorate$a([
        Autowired('displayedGroupCreator')
    ], GridSerializer.prototype, "displayedGroupCreator", void 0);
    __decorate$a([
        Autowired('columnController')
    ], GridSerializer.prototype, "columnController", void 0);
    __decorate$a([
        Autowired('rowModel')
    ], GridSerializer.prototype, "rowModel", void 0);
    __decorate$a([
        Autowired('pinnedRowModel')
    ], GridSerializer.prototype, "pinnedRowModel", void 0);
    __decorate$a([
        Autowired('selectionController')
    ], GridSerializer.prototype, "selectionController", void 0);
    __decorate$a([
        Autowired('columnFactory')
    ], GridSerializer.prototype, "columnFactory", void 0);
    __decorate$a([
        Autowired('gridOptionsWrapper')
    ], GridSerializer.prototype, "gridOptionsWrapper", void 0);
    GridSerializer = __decorate$a([
        Bean("gridSerializer")
    ], GridSerializer);
    return GridSerializer;
}());
var RowType;
(function (RowType) {
    RowType[RowType["HEADER_GROUPING"] = 0] = "HEADER_GROUPING";
    RowType[RowType["HEADER"] = 1] = "HEADER";
    RowType[RowType["BODY"] = 2] = "BODY";
})(RowType || (RowType = {}));

var __extends$3 = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate$b = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var LINE_SEPARATOR = '\r\n';
var CsvSerializingSession = /** @class */ (function (_super) {
    __extends$3(CsvSerializingSession, _super);
    function CsvSerializingSession(config) {
        var _this = _super.call(this, config) || this;
        _this.isFirstLine = true;
        _this.result = '';
        var suppressQuotes = config.suppressQuotes, columnSeparator = config.columnSeparator;
        _this.suppressQuotes = suppressQuotes;
        _this.columnSeparator = columnSeparator;
        return _this;
    }
    CsvSerializingSession.prototype.addCustomContent = function (content) {
        var _this = this;
        if (!content) {
            return;
        }
        if (typeof content === 'string') {
            // we used to require the customFooter to be prefixed with a newline but no longer do,
            // so only add the newline if the user has not supplied one
            if (!/^\s*\n/.test(content)) {
                this.beginNewLine();
            }
            // replace whatever newlines are supplied with the style we're using
            content = content.replace(/\r?\n/g, LINE_SEPARATOR);
            this.result += content;
        }
        else {
            content.forEach(function (row) {
                _this.beginNewLine();
                row.forEach(function (cell, index) {
                    if (index !== 0) {
                        _this.result += _this.columnSeparator;
                    }
                    _this.result += _this.putInQuotes(cell.data.value || '');
                    if (cell.mergeAcross) {
                        _this.appendEmptyCells(cell.mergeAcross);
                    }
                });
            });
        }
    };
    CsvSerializingSession.prototype.onNewHeaderGroupingRow = function () {
        this.beginNewLine();
        return {
            onColumn: this.onNewHeaderGroupingRowColumn.bind(this)
        };
    };
    CsvSerializingSession.prototype.onNewHeaderGroupingRowColumn = function (header, index, span) {
        if (index != 0) {
            this.result += this.columnSeparator;
        }
        this.result += this.putInQuotes(header);
        this.appendEmptyCells(span);
    };
    CsvSerializingSession.prototype.appendEmptyCells = function (count) {
        for (var i = 1; i <= count; i++) {
            this.result += this.columnSeparator + this.putInQuotes("");
        }
    };
    CsvSerializingSession.prototype.onNewHeaderRow = function () {
        this.beginNewLine();
        return {
            onColumn: this.onNewHeaderRowColumn.bind(this)
        };
    };
    CsvSerializingSession.prototype.onNewHeaderRowColumn = function (column, index, node) {
        if (index != 0) {
            this.result += this.columnSeparator;
        }
        this.result += this.putInQuotes(this.extractHeaderValue(column));
    };
    CsvSerializingSession.prototype.onNewBodyRow = function () {
        this.beginNewLine();
        return {
            onColumn: this.onNewBodyRowColumn.bind(this)
        };
    };
    CsvSerializingSession.prototype.onNewBodyRowColumn = function (column, index, node) {
        if (index != 0) {
            this.result += this.columnSeparator;
        }
        this.result += this.putInQuotes(this.extractRowCellValue(column, index, Constants.EXPORT_TYPE_CSV, node));
    };
    CsvSerializingSession.prototype.putInQuotes = function (value) {
        if (this.suppressQuotes) {
            return value;
        }
        if (value === null || value === undefined) {
            return '""';
        }
        var stringValue;
        if (typeof value === 'string') {
            stringValue = value;
        }
        else if (typeof value.toString === 'function') {
            stringValue = value.toString();
        }
        else {
            console.warn('unknown value type during csv conversion');
            stringValue = '';
        }
        // replace each " with "" (ie two sets of double quotes is how to do double quotes in csv)
        var valueEscaped = stringValue.replace(/"/g, "\"\"");
        return '"' + valueEscaped + '"';
    };
    CsvSerializingSession.prototype.parse = function () {
        return this.result;
    };
    CsvSerializingSession.prototype.beginNewLine = function () {
        if (!this.isFirstLine) {
            this.result += LINE_SEPARATOR;
        }
        this.isFirstLine = false;
    };
    return CsvSerializingSession;
}(BaseGridSerializingSession));
var BaseCreator = /** @class */ (function () {
    function BaseCreator() {
    }
    BaseCreator.prototype.setBeans = function (beans) {
        this.beans = beans;
    };
    BaseCreator.prototype.export = function (userParams) {
        if (this.isExportSuppressed()) {
            console.warn("ag-grid: Export cancelled. Export is not allowed as per your configuration.");
            return '';
        }
        var _a = this.getMergedParamsAndData(userParams), mergedParams = _a.mergedParams, data = _a.data;
        var fileNamePresent = mergedParams && mergedParams.fileName && mergedParams.fileName.length !== 0;
        var fileName = fileNamePresent ? mergedParams.fileName : this.getDefaultFileName();
        if (fileName.indexOf(".") === -1) {
            fileName = fileName + "." + this.getDefaultFileExtension();
        }
        this.beans.downloader.download(fileName, this.packageFile(data));
        return data;
    };
    BaseCreator.prototype.getData = function (params) {
        return this.getMergedParamsAndData(params).data;
    };
    BaseCreator.prototype.getMergedParamsAndData = function (userParams) {
        var mergedParams = this.mergeDefaultParams(userParams);
        var data = this.beans.gridSerializer.serialize(this.createSerializingSession(mergedParams), mergedParams);
        return { mergedParams: mergedParams, data: data };
    };
    BaseCreator.prototype.mergeDefaultParams = function (userParams) {
        var baseParams = this.beans.gridOptionsWrapper.getDefaultExportParams();
        var params = {};
        _.assign(params, baseParams);
        _.assign(params, userParams);
        return params;
    };
    BaseCreator.prototype.packageFile = function (data) {
        return new Blob(["\ufeff", data], {
            type: window.navigator.msSaveOrOpenBlob ? this.getMimeType() : 'octet/stream'
        });
    };
    return BaseCreator;
}());
var CsvCreator = /** @class */ (function (_super) {
    __extends$3(CsvCreator, _super);
    function CsvCreator() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    CsvCreator.prototype.postConstruct = function () {
        this.setBeans({
            downloader: this.downloader,
            gridSerializer: this.gridSerializer,
            gridOptionsWrapper: this.gridOptionsWrapper
        });
    };
    CsvCreator.prototype.exportDataAsCsv = function (params) {
        return this.export(params);
    };
    CsvCreator.prototype.getDataAsCsv = function (params) {
        return this.getData(params);
    };
    CsvCreator.prototype.getMimeType = function () {
        return 'text/csv;charset=utf-8;';
    };
    CsvCreator.prototype.getDefaultFileName = function () {
        return 'export.csv';
    };
    CsvCreator.prototype.getDefaultFileExtension = function () {
        return 'csv';
    };
    CsvCreator.prototype.createSerializingSession = function (params) {
        var _a = this, columnController = _a.columnController, valueService = _a.valueService, gridOptionsWrapper = _a.gridOptionsWrapper;
        var processCellCallback = params.processCellCallback, processHeaderCallback = params.processHeaderCallback, processGroupHeaderCallback = params.processGroupHeaderCallback, processRowGroupCallback = params.processRowGroupCallback, suppressQuotes = params.suppressQuotes, columnSeparator = params.columnSeparator;
        return new CsvSerializingSession({
            columnController: columnController,
            valueService: valueService,
            gridOptionsWrapper: gridOptionsWrapper,
            processCellCallback: processCellCallback || undefined,
            processHeaderCallback: processHeaderCallback || undefined,
            processGroupHeaderCallback: processGroupHeaderCallback || undefined,
            processRowGroupCallback: processRowGroupCallback || undefined,
            suppressQuotes: suppressQuotes || false,
            columnSeparator: columnSeparator || ','
        });
    };
    CsvCreator.prototype.isExportSuppressed = function () {
        return this.gridOptionsWrapper.isSuppressCsvExport();
    };
    __decorate$b([
        Autowired('columnController')
    ], CsvCreator.prototype, "columnController", void 0);
    __decorate$b([
        Autowired('valueService')
    ], CsvCreator.prototype, "valueService", void 0);
    __decorate$b([
        Autowired('downloader')
    ], CsvCreator.prototype, "downloader", void 0);
    __decorate$b([
        Autowired('gridSerializer')
    ], CsvCreator.prototype, "gridSerializer", void 0);
    __decorate$b([
        Autowired('gridOptionsWrapper')
    ], CsvCreator.prototype, "gridOptionsWrapper", void 0);
    __decorate$b([
        PostConstruct
    ], CsvCreator.prototype, "postConstruct", null);
    CsvCreator = __decorate$b([
        Bean('csvCreator')
    ], CsvCreator);
    return CsvCreator;
}(BaseCreator));

var __decorate$c = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var Downloader = /** @class */ (function () {
    function Downloader() {
    }
    Downloader.prototype.download = function (fileName, content) {
        // Internet Explorer
        if (window.navigator.msSaveOrOpenBlob) {
            window.navigator.msSaveOrOpenBlob(content, fileName);
        }
        else {
            // Other Browsers
            var element = document.createElement("a");
            var url_1 = window.URL.createObjectURL(content);
            element.setAttribute("href", url_1);
            element.setAttribute("download", fileName);
            element.style.display = "none";
            document.body.appendChild(element);
            element.dispatchEvent(new MouseEvent('click', {
                bubbles: false,
                cancelable: true,
                view: window
            }));
            document.body.removeChild(element);
            window.setTimeout(function () {
                window.URL.revokeObjectURL(url_1);
            }, 0);
        }
    };
    Downloader = __decorate$c([
        Bean("downloader")
    ], Downloader);
    return Downloader;
}());

var __decorate$d = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var LINE_SEPARATOR$1 = '\r\n';
var XmlFactory = /** @class */ (function () {
    function XmlFactory() {
    }
    XmlFactory.prototype.createHeader = function (headerElement) {
        if (headerElement === void 0) { headerElement = {}; }
        var headerStart = '<?';
        var headerEnd = '?>';
        var keys = ['version'];
        if (!headerElement.version) {
            headerElement.version = "1.0";
        }
        if (headerElement.encoding) {
            keys.push('encoding');
        }
        if (headerElement.standalone) {
            keys.push('standalone');
        }
        var att = keys.map(function (key) { return key + "=\"" + headerElement[key] + "\""; }).join(' ');
        return headerStart + "xml " + att + " " + headerEnd;
    };
    XmlFactory.prototype.createXml = function (xmlElement, booleanTransformer) {
        var _this = this;
        var props = '';
        if (xmlElement.properties) {
            if (xmlElement.properties.prefixedAttributes) {
                xmlElement.properties.prefixedAttributes.forEach(function (prefixedSet) {
                    Object.keys(prefixedSet.map).forEach(function (key) {
                        props += _this.returnAttributeIfPopulated(prefixedSet.prefix + key, prefixedSet.map[key], booleanTransformer);
                    });
                });
            }
            if (xmlElement.properties.rawMap) {
                Object.keys(xmlElement.properties.rawMap).forEach(function (key) {
                    props += _this.returnAttributeIfPopulated(key, xmlElement.properties.rawMap[key], booleanTransformer);
                });
            }
        }
        var result = '<' + xmlElement.name + props;
        if (!xmlElement.children && xmlElement.textNode == null) {
            return result + '/>' + LINE_SEPARATOR$1;
        }
        if (xmlElement.textNode != null) {
            return result + '>' + xmlElement.textNode + '</' + xmlElement.name + '>' + LINE_SEPARATOR$1;
        }
        result += '>' + LINE_SEPARATOR$1;
        if (xmlElement.children) {
            xmlElement.children.forEach(function (it) {
                result += _this.createXml(it, booleanTransformer);
            });
        }
        return result + '</' + xmlElement.name + '>' + LINE_SEPARATOR$1;
    };
    XmlFactory.prototype.returnAttributeIfPopulated = function (key, value, booleanTransformer) {
        if (!value && value !== '' && value !== 0) {
            return '';
        }
        var xmlValue = value;
        if ((typeof (value) === 'boolean')) {
            if (booleanTransformer) {
                xmlValue = booleanTransformer(value);
            }
        }
        return " " + key + "=\"" + xmlValue + "\"";
    };
    XmlFactory = __decorate$d([
        Bean('xmlFactory')
    ], XmlFactory);
    return XmlFactory;
}());

var __decorate$e = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
// table for crc calculation
// from: https://referencesource.microsoft.com/#System/sys/System/IO/compression/Crc32Helper.cs,3b31978c7d7f7246,references
var crcTable = [
    0, 1996959894, -301047508, -1727442502, 124634137, 1886057615, -379345611, -1637575261, 249268274,
    2044508324, -522852066, -1747789432, 162941995, 2125561021, -407360249, -1866523247, 498536548,
    1789927666, -205950648, -2067906082, 450548861, 1843258603, -187386543, -2083289657, 325883990,
    1684777152, -43845254, -1973040660, 335633487, 1661365465, -99664541, -1928851979, 997073096,
    1281953886, -715111964, -1570279054, 1006888145, 1258607687, -770865667, -1526024853, 901097722,
    1119000684, -608450090, -1396901568, 853044451, 1172266101, -589951537, -1412350631, 651767980,
    1373503546, -925412992, -1076862698, 565507253, 1454621731, -809855591, -1195530993, 671266974,
    1594198024, -972236366, -1324619484, 795835527, 1483230225, -1050600021, -1234817731, 1994146192,
    31158534, -1731059524, -271249366, 1907459465, 112637215, -1614814043, -390540237, 2013776290,
    251722036, -1777751922, -519137256, 2137656763, 141376813, -1855689577, -429695999, 1802195444,
    476864866, -2056965928, -228458418, 1812370925, 453092731, -2113342271, -183516073, 1706088902,
    314042704, -1950435094, -54949764, 1658658271, 366619977, -1932296973, -69972891, 1303535960,
    984961486, -1547960204, -725929758, 1256170817, 1037604311, -1529756563, -740887301, 1131014506,
    879679996, -1385723834, -631195440, 1141124467, 855842277, -1442165665, -586318647, 1342533948,
    654459306, -1106571248, -921952122, 1466479909, 544179635, -1184443383, -832445281, 1591671054,
    702138776, -1328506846, -942167884, 1504918807, 783551873, -1212326853, -1061524307, -306674912,
    -1698712650, 62317068, 1957810842, -355121351, -1647151185, 81470997, 1943803523, -480048366,
    -1805370492, 225274430, 2053790376, -468791541, -1828061283, 167816743, 2097651377, -267414716,
    -2029476910, 503444072, 1762050814, -144550051, -2140837941, 426522225, 1852507879, -19653770,
    -1982649376, 282753626, 1742555852, -105259153, -1900089351, 397917763, 1622183637, -690576408,
    -1580100738, 953729732, 1340076626, -776247311, -1497606297, 1068828381, 1219638859, -670225446,
    -1358292148, 906185462, 1090812512, -547295293, -1469587627, 829329135, 1181335161, -882789492,
    -1134132454, 628085408, 1382605366, -871598187, -1156888829, 570562233, 1426400815, -977650754,
    -1296233688, 733239954, 1555261956, -1026031705, -1244606671, 752459403, 1541320221, -1687895376,
    -328994266, 1969922972, 40735498, -1677130071, -351390145, 1913087877, 83908371, -1782625662,
    -491226604, 2075208622, 213261112, -1831694693, -438977011, 2094854071, 198958881, -2032938284,
    -237706686, 1759359992, 534414190, -2118248755, -155638181, 1873836001, 414664567, -2012718362,
    -15766928, 1711684554, 285281116, -1889165569, -127750551, 1634467795, 376229701, -1609899400,
    -686959890, 1308918612, 956543938, -1486412191, -799009033, 1231636301, 1047427035, -1362007478,
    -640263460, 1088359270, 936918000, -1447252397, -558129467, 1202900863, 817233897, -1111625188,
    -893730166, 1404277552, 615818150, -1160759803, -841546093, 1423857449, 601450431, -1285129682,
    -1000256840, 1567103746, 711928724, -1274298825, -1022587231, 1510334235, 755167117
];
var ZipContainer = /** @class */ (function () {
    function ZipContainer() {
        var _this = this;
        this.folders = [];
        this.files = [];
        this.addFolder = function (path) {
            _this.folders.push({
                path: path,
                created: new Date()
            });
        };
    }
    ZipContainer.prototype.addFolders = function (paths) {
        paths.forEach(this.addFolder);
    };
    ZipContainer.prototype.addFile = function (path, content) {
        this.files.push({
            path: path,
            created: new Date(),
            content: content
        });
    };
    ZipContainer.prototype.clearStream = function () {
        this.folders = [];
        this.files = [];
    };
    ZipContainer.prototype.getContent = function (mimeType) {
        if (mimeType === void 0) { mimeType = 'application/zip'; }
        var textOutput = this.buildFileStream();
        var uInt8Output = this.buildUint8Array(textOutput);
        this.clearStream();
        return new Blob([uInt8Output], { type: mimeType });
    };
    ZipContainer.prototype.buildFileStream = function (fData) {
        if (fData === void 0) { fData = ''; }
        var totalFiles = this.folders.concat(this.files);
        var len = totalFiles.length;
        var foData = '';
        var lL = 0;
        var cL = 0;
        for (var _i = 0, totalFiles_1 = totalFiles; _i < totalFiles_1.length; _i++) {
            var currentFile = totalFiles_1[_i];
            var _a = this.getHeader(currentFile, lL), fileHeader = _a.fileHeader, folderHeader = _a.folderHeader, content = _a.content;
            lL += fileHeader.length + content.length;
            cL += folderHeader.length;
            fData += fileHeader + content;
            foData += folderHeader;
        }
        var foEnd = this.buildFolderEnd(len, cL, lL);
        return fData + foData + foEnd;
    };
    ZipContainer.prototype.getHeader = function (currentFile, offset) {
        var content = currentFile.content, path = currentFile.path, created = currentFile.created;
        var utf8_encode = _.utf8_encode, decToHex = _.decToHex;
        var utfPath = utf8_encode(path);
        var isUTF8 = utfPath !== path;
        var time = this.convertTime(created);
        var dt = this.convertDate(created);
        var extraFields = '';
        if (isUTF8) {
            var uExtraFieldPath = decToHex(1, 1) + decToHex(this.getFromCrc32Table(utfPath), 4) + utfPath;
            extraFields = "\x75\x70" + decToHex(uExtraFieldPath.length, 2) + uExtraFieldPath;
        }
        var header = '\x0A\x00' +
            (isUTF8 ? '\x00\x08' : '\x00\x00') +
            '\x00\x00' +
            decToHex(time, 2) + // last modified time
            decToHex(dt, 2) + // last modified date
            decToHex(content ? this.getFromCrc32Table(content) : 0, 4) +
            decToHex(content ? content.length : 0, 4) + // compressed size
            decToHex(content ? content.length : 0, 4) + // uncompressed size
            decToHex(utfPath.length, 2) + // file name length
            decToHex(extraFields.length, 2); // extra field length
        var fileHeader = 'PK\x03\x04' + header + utfPath + extraFields;
        var folderHeader = 'PK\x01\x02' + // central header
            '\x14\x00' +
            header + // file header
            '\x00\x00' +
            '\x00\x00' +
            '\x00\x00' +
            (content ? '\x00\x00\x00\x00' : '\x10\x00\x00\x00') + // external file attributes
            decToHex(offset, 4) + // relative offset of local header
            utfPath + // file name
            extraFields; // extra field
        return { fileHeader: fileHeader, folderHeader: folderHeader, content: content || '' };
    };
    ZipContainer.prototype.buildFolderEnd = function (tLen, cLen, lLen) {
        var decToHex = _.decToHex;
        return 'PK\x05\x06' + // central folder end
            '\x00\x00' +
            '\x00\x00' +
            decToHex(tLen, 2) + // total number of entries in the central folder
            decToHex(tLen, 2) + // total number of entries in the central folder
            decToHex(cLen, 4) + // size of the central folder
            decToHex(lLen, 4) + // central folder start offset
            '\x00\x00';
    };
    ZipContainer.prototype.buildUint8Array = function (content) {
        var uint8 = new Uint8Array(content.length);
        for (var i = 0; i < uint8.length; i++) {
            uint8[i] = content.charCodeAt(i);
        }
        return uint8;
    };
    ZipContainer.prototype.getFromCrc32Table = function (content, crc) {
        if (crc === void 0) { crc = 0; }
        if (!content.length) {
            return 0;
        }
        crc ^= (-1);
        var j = 0;
        var k = 0;
        var l = 0;
        for (var i = 0; i < content.length; i++) {
            j = content.charCodeAt(i);
            k = (crc ^ j) & 0xFF;
            l = crcTable[k];
            crc = (crc >>> 8) ^ l;
        }
        return crc ^ (-1);
    };
    ZipContainer.prototype.convertTime = function (date) {
        var time = date.getHours();
        time <<= 6;
        time = time | date.getMinutes();
        time <<= 5;
        time = time | date.getSeconds() / 2;
        return time;
    };
    ZipContainer.prototype.convertDate = function (date) {
        var dt = date.getFullYear() - 1980;
        dt <<= 4;
        dt = dt | (date.getMonth() + 1);
        dt <<= 5;
        dt = dt | date.getDate();
        return dt;
    };
    ZipContainer = __decorate$e([
        Bean('zipContainer')
    ], ZipContainer);
    return ZipContainer;
}());

var CsvExportModule = {
    moduleName: ModuleNames.CsvExportModule,
    beans: [CsvCreator, Downloader, XmlFactory, GridSerializer, ZipContainer]
};

var AllCommunityModules = [ClientSideRowModelModule, InfiniteRowModelModule, CsvExportModule];

var __extends$4 = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate$f = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var EXPAND_STATE;
(function (EXPAND_STATE) {
    EXPAND_STATE[EXPAND_STATE["EXPANDED"] = 0] = "EXPANDED";
    EXPAND_STATE[EXPAND_STATE["COLLAPSED"] = 1] = "COLLAPSED";
    EXPAND_STATE[EXPAND_STATE["INDETERMINATE"] = 2] = "INDETERMINATE";
})(EXPAND_STATE || (EXPAND_STATE = {}));
var PrimaryColsHeaderPanel = /** @class */ (function (_super) {
    __extends$4(PrimaryColsHeaderPanel, _super);
    function PrimaryColsHeaderPanel() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    PrimaryColsHeaderPanel.prototype.preConstruct = function () {
        this.setTemplate("<div class=\"ag-column-select-header\" role=\"presentation\">\n                <div ref=\"eExpand\" class=\"ag-column-select-header-icon\"></div>\n                <ag-checkbox ref=\"eSelect\" class=\"ag-column-select-header-checkbox\"></ag-checkbox>\n                <ag-input-text-field class=\"ag-column-select-header-filter-wrapper\" ref=\"eFilterTextField\"></ag-input-text-field>\n            </div>");
    };
    PrimaryColsHeaderPanel.prototype.postConstruct = function () {
        var _this = this;
        this.createExpandIcons();
        this.addDestroyableEventListener(this.eExpand, "click", this.onExpandClicked.bind(this));
        this.addDestroyableEventListener(this.eSelect.getInputElement(), 'click', this.onSelectClicked.bind(this));
        this.eFilterTextField.onValueChange(function () { return _this.onFilterTextChanged(); });
        this.addDestroyableEventListener(this.eFilterTextField.getInputElement(), "keypress", this.onMiniFilterKeyPress.bind(this));
        this.addDestroyableEventListener(this.eventService, Events.EVENT_NEW_COLUMNS_LOADED, this.showOrHideOptions.bind(this));
    };
    PrimaryColsHeaderPanel.prototype.init = function (params) {
        this.params = params;
        if (this.columnController.isReady()) {
            this.showOrHideOptions();
        }
    };
    PrimaryColsHeaderPanel.prototype.createExpandIcons = function () {
        this.eExpand.appendChild((this.eExpandChecked = _.createIconNoSpan("columnSelectOpen", this.gridOptionsWrapper)));
        this.eExpand.appendChild((this.eExpandUnchecked = _.createIconNoSpan("columnSelectClosed", this.gridOptionsWrapper)));
        this.eExpand.appendChild((this.eExpandIndeterminate = _.createIconNoSpan("columnSelectIndeterminate", this.gridOptionsWrapper)));
        this.setExpandState(EXPAND_STATE.EXPANDED);
    };
    // we only show expand / collapse if we are showing columns
    PrimaryColsHeaderPanel.prototype.showOrHideOptions = function () {
        var showFilter = !this.params.suppressColumnFilter;
        var showSelect = !this.params.suppressColumnSelectAll;
        var showExpand = !this.params.suppressColumnExpandAll;
        var groupsPresent = this.columnController.isPrimaryColumnGroupsPresent();
        var translate = this.gridOptionsWrapper.getLocaleTextFunc();
        this.eFilterTextField.setInputPlaceholder(translate('searchOoo', 'Search...'));
        _.setDisplayed(this.eFilterTextField.getGui(), showFilter);
        _.setDisplayed(this.eSelect.getGui(), showSelect);
        _.setDisplayed(this.eExpand, showExpand && groupsPresent);
    };
    PrimaryColsHeaderPanel.prototype.onFilterTextChanged = function () {
        var _this = this;
        if (!this.onFilterTextChangedDebounced) {
            this.onFilterTextChangedDebounced = _.debounce(function () {
                var filterText = _this.eFilterTextField.getValue();
                _this.dispatchEvent({ type: "filterChanged", filterText: filterText });
            }, 300);
        }
        this.onFilterTextChangedDebounced();
    };
    PrimaryColsHeaderPanel.prototype.onMiniFilterKeyPress = function (e) {
        if (_.isKeyPressed(e, Constants.KEY_ENTER)) {
            this.onSelectClicked();
        }
    };
    PrimaryColsHeaderPanel.prototype.onSelectClicked = function () {
        var eventType = this.selectState === true ? "unselectAll" : "selectAll";
        this.dispatchEvent({ type: eventType });
    };
    PrimaryColsHeaderPanel.prototype.onExpandClicked = function () {
        var eventType = this.expandState === EXPAND_STATE.EXPANDED ? "collapseAll" : "expandAll";
        this.dispatchEvent({ type: eventType });
    };
    PrimaryColsHeaderPanel.prototype.setExpandState = function (state) {
        this.expandState = state;
        _.setDisplayed(this.eExpandChecked, this.expandState === EXPAND_STATE.EXPANDED);
        _.setDisplayed(this.eExpandUnchecked, this.expandState === EXPAND_STATE.COLLAPSED);
        _.setDisplayed(this.eExpandIndeterminate, this.expandState === EXPAND_STATE.INDETERMINATE);
    };
    PrimaryColsHeaderPanel.prototype.setSelectionState = function (state) {
        this.selectState = state;
        this.eSelect.setValue(this.selectState);
    };
    __decorate$f([
        Autowired('gridOptionsWrapper')
    ], PrimaryColsHeaderPanel.prototype, "gridOptionsWrapper", void 0);
    __decorate$f([
        Autowired('columnController')
    ], PrimaryColsHeaderPanel.prototype, "columnController", void 0);
    __decorate$f([
        Autowired('eventService')
    ], PrimaryColsHeaderPanel.prototype, "eventService", void 0);
    __decorate$f([
        RefSelector('eExpand')
    ], PrimaryColsHeaderPanel.prototype, "eExpand", void 0);
    __decorate$f([
        RefSelector('eSelect')
    ], PrimaryColsHeaderPanel.prototype, "eSelect", void 0);
    __decorate$f([
        RefSelector('eFilterTextField')
    ], PrimaryColsHeaderPanel.prototype, "eFilterTextField", void 0);
    __decorate$f([
        PreConstruct
    ], PrimaryColsHeaderPanel.prototype, "preConstruct", null);
    __decorate$f([
        PostConstruct
    ], PrimaryColsHeaderPanel.prototype, "postConstruct", null);
    return PrimaryColsHeaderPanel;
}(Component));

var __extends$5 = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate$g = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var ToolPanelColumnGroupComp = /** @class */ (function (_super) {
    __extends$5(ToolPanelColumnGroupComp, _super);
    function ToolPanelColumnGroupComp(columnGroup, columnDept, allowDragging, expandByDefault, expandedCallback, getFilterResults) {
        var _this = _super.call(this) || this;
        _this.processingColumnStateChange = false;
        _this.columnGroup = columnGroup;
        _this.columnDept = columnDept;
        _this.allowDragging = allowDragging;
        _this.expanded = expandByDefault;
        _this.expandedCallback = expandedCallback;
        _this.getFilterResultsCallback = getFilterResults;
        return _this;
    }
    ToolPanelColumnGroupComp.prototype.init = function () {
        this.setTemplate(ToolPanelColumnGroupComp.TEMPLATE);
        this.eDragHandle = _.createIconNoSpan('columnDrag', this.gridOptionsWrapper);
        _.addCssClass(this.eDragHandle, 'ag-drag-handle');
        _.addCssClass(this.eDragHandle, 'ag-column-select-column-group-drag-handle');
        this.cbSelect.getGui().insertAdjacentElement('afterend', this.eDragHandle);
        this.displayName = this.columnController.getDisplayNameForOriginalColumnGroup(null, this.columnGroup, 'toolPanel');
        if (_.missing(this.displayName)) {
            this.displayName = '>>';
        }
        this.eLabel.innerHTML = this.displayName ? this.displayName : '';
        this.setupExpandContract();
        this.addCssClass('ag-column-select-indent-' + this.columnDept);
        this.addDestroyableEventListener(this.eventService, Events.EVENT_COLUMN_PIVOT_MODE_CHANGED, this.onColumnStateChanged.bind(this));
        this.addDestroyableEventListener(this.eLabel, 'click', this.onLabelClicked.bind(this));
        this.addDestroyableEventListener(this.cbSelect, AgCheckbox.EVENT_CHANGED, this.onCheckboxChanged.bind(this));
        this.setOpenClosedIcons();
        this.setupDragging();
        this.onColumnStateChanged();
        this.addVisibilityListenersToAllChildren();
        CssClassApplier.addToolPanelClassesFromColDef(this.columnGroup.getColGroupDef(), this.getGui(), this.gridOptionsWrapper, null, this.columnGroup);
    };
    ToolPanelColumnGroupComp.prototype.addVisibilityListenersToAllChildren = function () {
        var _this = this;
        this.columnGroup.getLeafColumns().forEach(function (column) {
            _this.addDestroyableEventListener(column, Column.EVENT_VISIBLE_CHANGED, _this.onColumnStateChanged.bind(_this));
            _this.addDestroyableEventListener(column, Column.EVENT_VALUE_CHANGED, _this.onColumnStateChanged.bind(_this));
            _this.addDestroyableEventListener(column, Column.EVENT_PIVOT_CHANGED, _this.onColumnStateChanged.bind(_this));
            _this.addDestroyableEventListener(column, Column.EVENT_ROW_GROUP_CHANGED, _this.onColumnStateChanged.bind(_this));
        });
    };
    ToolPanelColumnGroupComp.prototype.setupDragging = function () {
        var _this = this;
        if (!this.allowDragging) {
            _.setDisplayed(this.eDragHandle, false);
            return;
        }
        var dragSource = {
            type: DragSourceType.ToolPanel,
            eElement: this.eDragHandle,
            dragItemName: this.displayName,
            getDragItem: function () { return _this.createDragItem(); }
        };
        this.dragAndDropService.addDragSource(dragSource, true);
        this.addDestroyFunc(function () { return _this.dragAndDropService.removeDragSource(dragSource); });
    };
    ToolPanelColumnGroupComp.prototype.createDragItem = function () {
        var visibleState = {};
        this.columnGroup.getLeafColumns().forEach(function (col) {
            visibleState[col.getId()] = col.isVisible();
        });
        return {
            columns: this.columnGroup.getLeafColumns(),
            visibleState: visibleState
        };
    };
    ToolPanelColumnGroupComp.prototype.setupExpandContract = function () {
        this.eGroupClosedIcon.appendChild(_.createIcon('columnSelectClosed', this.gridOptionsWrapper, null));
        this.eGroupOpenedIcon.appendChild(_.createIcon('columnSelectOpen', this.gridOptionsWrapper, null));
        this.addDestroyableEventListener(this.eGroupClosedIcon, 'click', this.onExpandOrContractClicked.bind(this));
        this.addDestroyableEventListener(this.eGroupOpenedIcon, 'click', this.onExpandOrContractClicked.bind(this));
        var touchListener = new TouchListener(this.eColumnGroupIcons, true);
        this.addDestroyableEventListener(touchListener, TouchListener.EVENT_TAP, this.onExpandOrContractClicked.bind(this));
        this.addDestroyFunc(touchListener.destroy.bind(touchListener));
    };
    ToolPanelColumnGroupComp.prototype.onLabelClicked = function () {
        var nextState = !this.cbSelect.getValue();
        this.onChangeCommon(nextState);
    };
    ToolPanelColumnGroupComp.prototype.onCheckboxChanged = function (event) {
        this.onChangeCommon(event.selected);
    };
    ToolPanelColumnGroupComp.prototype.onChangeCommon = function (nextState) {
        if (this.processingColumnStateChange) {
            return;
        }
        var childColumns = this.columnGroup.getLeafColumns();
        if (this.columnController.isPivotMode()) {
            if (nextState) {
                this.actionCheckedReduce(childColumns);
            }
            else {
                this.actionUnCheckedReduce(childColumns);
            }
        }
        else {
            var isAllowedColumn = function (c) { return !c.getColDef().lockVisible && !c.getColDef().suppressColumnsToolPanel; };
            var allowedColumns = childColumns.filter(isAllowedColumn);
            var filterResults_1 = this.getFilterResultsCallback();
            var passesFilter = function (c) { return !filterResults_1 || filterResults_1[c.getColId()]; };
            var visibleColumns = allowedColumns.filter(passesFilter);
            // only columns that are 'allowed' and pass filter should be visible
            this.columnController.setColumnsVisible(visibleColumns, nextState, "toolPanelUi");
        }
    };
    ToolPanelColumnGroupComp.prototype.actionUnCheckedReduce = function (columns) {
        var columnsToUnPivot = [];
        var columnsToUnValue = [];
        var columnsToUnGroup = [];
        columns.forEach(function (column) {
            if (column.isPivotActive()) {
                columnsToUnPivot.push(column);
            }
            if (column.isRowGroupActive()) {
                columnsToUnGroup.push(column);
            }
            if (column.isValueActive()) {
                columnsToUnValue.push(column);
            }
        });
        if (columnsToUnPivot.length > 0) {
            this.columnController.removePivotColumns(columnsToUnPivot, "toolPanelUi");
        }
        if (columnsToUnGroup.length > 0) {
            this.columnController.removeRowGroupColumns(columnsToUnGroup, "toolPanelUi");
        }
        if (columnsToUnValue.length > 0) {
            this.columnController.removeValueColumns(columnsToUnValue, "toolPanelUi");
        }
    };
    ToolPanelColumnGroupComp.prototype.actionCheckedReduce = function (columns) {
        var columnsToAggregate = [];
        var columnsToGroup = [];
        var columnsToPivot = [];
        columns.forEach(function (column) {
            // don't change any column that's already got a function active
            if (column.isAnyFunctionActive()) {
                return;
            }
            if (column.isAllowValue()) {
                columnsToAggregate.push(column);
            }
            else if (column.isAllowRowGroup()) {
                columnsToGroup.push(column);
            }
            else if (column.isAllowRowGroup()) {
                columnsToPivot.push(column);
            }
        });
        if (columnsToAggregate.length > 0) {
            this.columnController.addValueColumns(columnsToAggregate, "toolPanelUi");
        }
        if (columnsToGroup.length > 0) {
            this.columnController.addRowGroupColumns(columnsToGroup, "toolPanelUi");
        }
        if (columnsToPivot.length > 0) {
            this.columnController.addPivotColumns(columnsToPivot, "toolPanelUi");
        }
    };
    ToolPanelColumnGroupComp.prototype.onColumnStateChanged = function () {
        var selectedValue = this.workOutSelectedValue();
        var readOnlyValue = this.workOutReadOnlyValue();
        this.processingColumnStateChange = true;
        this.cbSelect.setValue(selectedValue);
        this.cbSelect.setReadOnly(readOnlyValue);
        _.addOrRemoveCssClass(this.getGui(), 'ag-column-select-column-group-readonly', readOnlyValue);
        this.processingColumnStateChange = false;
    };
    ToolPanelColumnGroupComp.prototype.workOutSelectedValue = function () {
        var pivotMode = this.columnController.isPivotMode();
        var leafColumns = this.columnGroup.getLeafColumns();
        var filterResults = this.getFilterResultsCallback();
        var len = leafColumns.length;
        var count = { visible: 0, hidden: 0 };
        var ignoredChildCount = { visible: 0, hidden: 0 };
        for (var i = 0; i < len; i++) {
            var column = leafColumns[i];
            // ignore lock visible columns and columns set to 'suppressColumnsToolPanel'
            var ignore = column.getColDef().lockVisible || column.getColDef().suppressColumnsToolPanel;
            var type = this.isColumnVisible(column, pivotMode) ? 'visible' : 'hidden';
            count[type]++;
            // also ignore columns that have been removed by the filter
            if (filterResults) {
                var columnPassesFilter = filterResults[column.getColId()];
                if (!columnPassesFilter) {
                    ignore = true;
                }
            }
            if (!ignore) {
                continue;
            }
            ignoredChildCount[type]++;
        }
        // if all columns are ignored we use the regular count, if not
        // we only consider the columns that were not ignored
        if (ignoredChildCount.visible + ignoredChildCount.hidden !== len) {
            count.visible -= ignoredChildCount.visible;
            count.hidden -= ignoredChildCount.hidden;
        }
        var selectedValue;
        if (count.visible > 0 && count.hidden > 0) {
            selectedValue = null;
        }
        else {
            selectedValue = count.visible > 0;
        }
        return selectedValue == null ? undefined : selectedValue;
    };
    ToolPanelColumnGroupComp.prototype.workOutReadOnlyValue = function () {
        var pivotMode = this.columnController.isPivotMode();
        var colsThatCanAction = 0;
        this.columnGroup.getLeafColumns().forEach(function (col) {
            if (pivotMode) {
                if (col.isAnyFunctionAllowed()) {
                    colsThatCanAction++;
                }
            }
            else {
                if (!col.getColDef().lockVisible) {
                    colsThatCanAction++;
                }
            }
        });
        return colsThatCanAction === 0;
    };
    ToolPanelColumnGroupComp.prototype.isColumnVisible = function (column, pivotMode) {
        if (pivotMode) {
            var pivoted = column.isPivotActive();
            var grouped = column.isRowGroupActive();
            var aggregated = column.isValueActive();
            return pivoted || grouped || aggregated;
        }
        else {
            return column.isVisible();
        }
    };
    ToolPanelColumnGroupComp.prototype.onExpandOrContractClicked = function () {
        this.expanded = !this.expanded;
        this.setOpenClosedIcons();
        this.expandedCallback();
    };
    ToolPanelColumnGroupComp.prototype.setOpenClosedIcons = function () {
        var folderOpen = this.expanded;
        _.setDisplayed(this.eGroupClosedIcon, !folderOpen);
        _.setDisplayed(this.eGroupOpenedIcon, folderOpen);
    };
    ToolPanelColumnGroupComp.prototype.isExpanded = function () {
        return this.expanded;
    };
    ToolPanelColumnGroupComp.prototype.getDisplayName = function () {
        return this.displayName;
    };
    ToolPanelColumnGroupComp.prototype.onSelectAllChanged = function (value) {
        if ((value && !this.cbSelect.getValue()) ||
            (!value && this.cbSelect.getValue())) {
            if (!this.cbSelect.isReadOnly()) {
                this.cbSelect.toggle();
            }
        }
    };
    ToolPanelColumnGroupComp.prototype.isSelected = function () {
        return this.cbSelect.getValue();
    };
    ToolPanelColumnGroupComp.prototype.isSelectable = function () {
        return !this.cbSelect.isReadOnly();
    };
    ToolPanelColumnGroupComp.prototype.isExpandable = function () {
        return true;
    };
    ToolPanelColumnGroupComp.prototype.setExpanded = function (value) {
        if (this.expanded !== value) {
            this.onExpandOrContractClicked();
        }
    };
    ToolPanelColumnGroupComp.prototype.setSelected = function (selected) {
        this.cbSelect.setValue(selected, true);
    };
    ToolPanelColumnGroupComp.TEMPLATE = "<div class=\"ag-column-select-column-group\">\n            <span class=\"ag-column-group-icons\" ref=\"eColumnGroupIcons\" >\n                <span class=\"ag-column-group-closed-icon\" ref=\"eGroupClosedIcon\"></span>\n                <span class=\"ag-column-group-opened-icon\" ref=\"eGroupOpenedIcon\"></span>\n            </span>\n            <ag-checkbox ref=\"cbSelect\" class=\"ag-column-select-checkbox\"></ag-checkbox>\n            <span class=\"ag-column-select-column-label\" ref=\"eLabel\"></span>\n        </div>";
    __decorate$g([
        Autowired('eventService')
    ], ToolPanelColumnGroupComp.prototype, "eventService", void 0);
    __decorate$g([
        Autowired('columnController')
    ], ToolPanelColumnGroupComp.prototype, "columnController", void 0);
    __decorate$g([
        Autowired('dragAndDropService')
    ], ToolPanelColumnGroupComp.prototype, "dragAndDropService", void 0);
    __decorate$g([
        Autowired('gridOptionsWrapper')
    ], ToolPanelColumnGroupComp.prototype, "gridOptionsWrapper", void 0);
    __decorate$g([
        RefSelector('cbSelect')
    ], ToolPanelColumnGroupComp.prototype, "cbSelect", void 0);
    __decorate$g([
        RefSelector('eLabel')
    ], ToolPanelColumnGroupComp.prototype, "eLabel", void 0);
    __decorate$g([
        RefSelector('eGroupOpenedIcon')
    ], ToolPanelColumnGroupComp.prototype, "eGroupOpenedIcon", void 0);
    __decorate$g([
        RefSelector('eGroupClosedIcon')
    ], ToolPanelColumnGroupComp.prototype, "eGroupClosedIcon", void 0);
    __decorate$g([
        RefSelector('eColumnGroupIcons')
    ], ToolPanelColumnGroupComp.prototype, "eColumnGroupIcons", void 0);
    __decorate$g([
        PostConstruct
    ], ToolPanelColumnGroupComp.prototype, "init", null);
    return ToolPanelColumnGroupComp;
}(Component));

var __extends$6 = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate$h = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var ToolPanelColumnComp = /** @class */ (function (_super) {
    __extends$6(ToolPanelColumnComp, _super);
    function ToolPanelColumnComp(column, columnDept, allowDragging, groupsExist) {
        var _this = _super.call(this) || this;
        _this.processingColumnStateChange = false;
        _this.column = column;
        _this.columnDept = columnDept;
        _this.allowDragging = allowDragging;
        _this.groupsExist = groupsExist;
        return _this;
    }
    ToolPanelColumnComp.prototype.init = function () {
        this.setTemplate(ToolPanelColumnComp.TEMPLATE);
        this.eDragHandle = _.createIconNoSpan('columnDrag', this.gridOptionsWrapper);
        _.addCssClass(this.eDragHandle, 'ag-drag-handle');
        _.addCssClass(this.eDragHandle, 'ag-column-select-column-drag-handle');
        this.cbSelect.getGui().insertAdjacentElement('afterend', this.eDragHandle);
        this.displayName = this.columnController.getDisplayNameForColumn(this.column, 'toolPanel');
        var displayNameSanitised = _.escape(this.displayName);
        this.eLabel.innerHTML = displayNameSanitised;
        // if grouping, we add an extra level of indent, to cater for expand/contract icons we need to indent for
        var indent = this.columnDept;
        if (this.groupsExist) {
            this.addCssClass('ag-column-select-add-group-indent');
        }
        this.addCssClass("ag-column-select-indent-" + indent);
        this.setupDragging();
        this.addDestroyableEventListener(this.eventService, Events.EVENT_COLUMN_PIVOT_MODE_CHANGED, this.onColumnStateChanged.bind(this));
        this.addDestroyableEventListener(this.column, Column.EVENT_VALUE_CHANGED, this.onColumnStateChanged.bind(this));
        this.addDestroyableEventListener(this.column, Column.EVENT_PIVOT_CHANGED, this.onColumnStateChanged.bind(this));
        this.addDestroyableEventListener(this.column, Column.EVENT_ROW_GROUP_CHANGED, this.onColumnStateChanged.bind(this));
        this.addDestroyableEventListener(this.column, Column.EVENT_VISIBLE_CHANGED, this.onColumnStateChanged.bind(this));
        this.addDestroyableEventListener(this.gridOptionsWrapper, 'functionsReadOnly', this.onColumnStateChanged.bind(this));
        this.addDestroyableEventListener(this.cbSelect, AgCheckbox.EVENT_CHANGED, this.onCheckboxChanged.bind(this));
        this.addDestroyableEventListener(this.eLabel, 'click', this.onLabelClicked.bind(this));
        this.onColumnStateChanged();
        CssClassApplier.addToolPanelClassesFromColDef(this.column.getColDef(), this.getGui(), this.gridOptionsWrapper, this.column, null);
    };
    ToolPanelColumnComp.prototype.onLabelClicked = function () {
        if (this.gridOptionsWrapper.isFunctionsReadOnly()) {
            return;
        }
        var nextState = !this.cbSelect.getValue();
        this.onChangeCommon(nextState);
    };
    ToolPanelColumnComp.prototype.onCheckboxChanged = function (event) {
        this.onChangeCommon(event.selected);
    };
    ToolPanelColumnComp.prototype.onChangeCommon = function (nextState) {
        // ignore lock visible columns
        if (this.column.getColDef().lockVisible) {
            return;
        }
        // only want to action if the user clicked the checkbox, not is we are setting the checkbox because
        // of a change in the model
        if (this.processingColumnStateChange) {
            return;
        }
        // action in a timeout, as the action takes some time, we want to update the icons first
        // so the user gets nice feedback when they click. otherwise there would be a lag and the
        // user would think the checkboxes were clunky
        if (this.columnController.isPivotMode()) {
            if (nextState) {
                this.actionCheckedPivotMode();
            }
            else {
                this.actionUnCheckedPivotMode();
            }
        }
        else {
            this.columnController.setColumnVisible(this.column, nextState, "columnMenu");
        }
    };
    ToolPanelColumnComp.prototype.actionUnCheckedPivotMode = function () {
        var functionPassive = this.gridOptionsWrapper.isFunctionsPassive();
        var column = this.column;
        var columnController = this.columnController;
        // remove pivot if column is pivoted
        if (column.isPivotActive()) {
            if (functionPassive) {
                var copyOfPivotColumns = this.columnController.getPivotColumns().slice();
                copyOfPivotColumns.push(column);
                var event_1 = {
                    type: Events.EVENT_COLUMN_PIVOT_CHANGE_REQUEST,
                    columns: copyOfPivotColumns,
                    api: this.gridApi,
                    columnApi: this.columnApi
                };
                this.eventService.dispatchEvent(event_1);
            }
            else {
                columnController.removePivotColumn(column, "columnMenu");
            }
        }
        // remove value if column is value
        if (column.isValueActive()) {
            if (functionPassive) {
                var copyOfValueColumns = this.columnController.getValueColumns().slice();
                copyOfValueColumns.push(column);
                var event_2 = {
                    type: Events.EVENT_COLUMN_VALUE_CHANGE_REQUEST,
                    columns: copyOfValueColumns,
                    api: this.gridApi,
                    columnApi: this.columnApi
                };
                this.eventService.dispatchEvent(event_2);
            }
            else {
                columnController.removeValueColumn(column, "columnMenu");
            }
        }
        // remove group if column is grouped
        if (column.isRowGroupActive()) {
            if (functionPassive) {
                var copyOfRowGroupColumns = this.columnController.getRowGroupColumns().slice();
                copyOfRowGroupColumns.push(column);
                var event_3 = {
                    type: Events.EVENT_COLUMN_ROW_GROUP_CHANGE_REQUEST,
                    columns: copyOfRowGroupColumns,
                    api: this.gridApi,
                    columnApi: this.columnApi
                };
                this.eventService.dispatchEvent(event_3);
            }
            else {
                columnController.removeRowGroupColumn(column, "columnMenu");
            }
        }
    };
    ToolPanelColumnComp.prototype.actionCheckedPivotMode = function () {
        var column = this.column;
        // function already active, so do nothing
        if (column.isValueActive() || column.isPivotActive() || column.isRowGroupActive()) {
            return;
        }
        var functionPassive = this.gridOptionsWrapper.isFunctionsPassive();
        if (column.isAllowValue()) {
            if (functionPassive) {
                var copyOfValueColumns = this.columnController.getValueColumns().slice();
                _.removeFromArray(copyOfValueColumns, column);
                var event_4 = {
                    type: Events.EVENT_COLUMN_VALUE_CHANGE_REQUEST,
                    api: this.gridApi,
                    columnApi: this.columnApi,
                    columns: copyOfValueColumns
                };
                this.eventService.dispatchEvent(event_4);
            }
            else {
                this.columnController.addValueColumn(column, "columnMenu");
            }
        }
        else if (column.isAllowRowGroup()) {
            if (functionPassive) {
                var copyOfRowGroupColumns = this.columnController.getRowGroupColumns().slice();
                _.removeFromArray(copyOfRowGroupColumns, column);
                var event_5 = {
                    type: Events.EVENT_COLUMN_ROW_GROUP_CHANGE_REQUEST,
                    api: this.gridApi,
                    columnApi: this.columnApi,
                    columns: copyOfRowGroupColumns
                };
                this.eventService.dispatchEvent(event_5);
            }
            else {
                this.columnController.addRowGroupColumn(column, "columnMenu");
            }
        }
        else if (column.isAllowPivot()) {
            if (functionPassive) {
                var copyOfPivotColumns = this.columnController.getPivotColumns().slice();
                _.removeFromArray(copyOfPivotColumns, column);
                var event_6 = {
                    type: Events.EVENT_COLUMN_PIVOT_CHANGE_REQUEST,
                    api: this.gridApi,
                    columnApi: this.columnApi,
                    columns: copyOfPivotColumns
                };
                this.eventService.dispatchEvent(event_6);
            }
            else {
                this.columnController.addPivotColumn(column, "columnMenu");
            }
        }
    };
    ToolPanelColumnComp.prototype.setupDragging = function () {
        var _this = this;
        if (!this.allowDragging) {
            _.setDisplayed(this.eDragHandle, false);
            return;
        }
        var dragSource = {
            type: DragSourceType.ToolPanel,
            eElement: this.eDragHandle,
            dragItemName: this.displayName,
            getDragItem: function () { return _this.createDragItem(); }
        };
        this.dragAndDropService.addDragSource(dragSource, true);
        this.addDestroyFunc(function () { return _this.dragAndDropService.removeDragSource(dragSource); });
    };
    ToolPanelColumnComp.prototype.createDragItem = function () {
        var visibleState = {};
        visibleState[this.column.getId()] = this.column.isVisible();
        return {
            columns: [this.column],
            visibleState: visibleState
        };
    };
    ToolPanelColumnComp.prototype.onColumnStateChanged = function () {
        this.processingColumnStateChange = true;
        var isPivotMode = this.columnController.isPivotMode();
        if (isPivotMode) {
            // if reducing, checkbox means column is one of pivot, value or group
            var anyFunctionActive = this.column.isAnyFunctionActive();
            this.cbSelect.setValue(anyFunctionActive);
        }
        else {
            // if not reducing, the checkbox tells us if column is visible or not
            this.cbSelect.setValue(this.column.isVisible());
        }
        var checkboxReadOnly;
        if (isPivotMode) {
            // when in pivot mode, the item should be read only if:
            //  a) gui is not allowed make any changes
            var functionsReadOnly = this.gridOptionsWrapper.isFunctionsReadOnly();
            //  b) column is not allow any functions on it
            var noFunctionsAllowed = !this.column.isAnyFunctionAllowed();
            checkboxReadOnly = functionsReadOnly || noFunctionsAllowed;
        }
        else {
            // when in normal mode, the checkbox is read only if visibility is locked
            checkboxReadOnly = !!this.column.getColDef().lockVisible;
        }
        this.cbSelect.setReadOnly(checkboxReadOnly);
        _.addOrRemoveCssClass(this.getGui(), 'ag-column-select-column-readonly', checkboxReadOnly);
        var checkboxPassive = isPivotMode && this.gridOptionsWrapper.isFunctionsPassive();
        this.cbSelect.setPassive(checkboxPassive);
        this.processingColumnStateChange = false;
    };
    ToolPanelColumnComp.prototype.getDisplayName = function () {
        return this.displayName;
    };
    ToolPanelColumnComp.prototype.onSelectAllChanged = function (value) {
        if (value !== this.cbSelect.getValue()) {
            if (!this.cbSelect.isReadOnly()) {
                this.cbSelect.toggle();
            }
        }
    };
    ToolPanelColumnComp.prototype.isSelected = function () {
        return this.cbSelect.getValue();
    };
    ToolPanelColumnComp.prototype.isSelectable = function () {
        return !this.cbSelect.isReadOnly();
    };
    ToolPanelColumnComp.prototype.isExpandable = function () {
        return false;
    };
    ToolPanelColumnComp.prototype.setExpanded = function (value) {
        console.warn('ag-grid: can not expand a column item that does not represent a column group header');
    };
    ToolPanelColumnComp.TEMPLATE = "<div class=\"ag-column-select-column\">\n            <ag-checkbox ref=\"cbSelect\" class=\"ag-column-select-checkbox\"></ag-checkbox>\n            <span class=\"ag-column-select-column-label\" ref=\"eLabel\"></span>\n        </div>";
    __decorate$h([
        Autowired('gridOptionsWrapper')
    ], ToolPanelColumnComp.prototype, "gridOptionsWrapper", void 0);
    __decorate$h([
        Autowired('columnController')
    ], ToolPanelColumnComp.prototype, "columnController", void 0);
    __decorate$h([
        Autowired('eventService')
    ], ToolPanelColumnComp.prototype, "eventService", void 0);
    __decorate$h([
        Autowired('dragAndDropService')
    ], ToolPanelColumnComp.prototype, "dragAndDropService", void 0);
    __decorate$h([
        Autowired('columnApi')
    ], ToolPanelColumnComp.prototype, "columnApi", void 0);
    __decorate$h([
        Autowired('gridApi')
    ], ToolPanelColumnComp.prototype, "gridApi", void 0);
    __decorate$h([
        RefSelector('eLabel')
    ], ToolPanelColumnComp.prototype, "eLabel", void 0);
    __decorate$h([
        RefSelector('cbSelect')
    ], ToolPanelColumnComp.prototype, "cbSelect", void 0);
    __decorate$h([
        PostConstruct
    ], ToolPanelColumnComp.prototype, "init", null);
    return ToolPanelColumnComp;
}(Component));

var __extends$7 = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate$i = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var PrimaryColsListPanel = /** @class */ (function (_super) {
    __extends$7(PrimaryColsListPanel, _super);
    function PrimaryColsListPanel() {
        var _this = _super.call(this, PrimaryColsListPanel.TEMPLATE) || this;
        _this.selectAllChecked = true;
        _this.getColumnCompId = function (columnGroupChild) {
            if (columnGroupChild instanceof OriginalColumnGroup) {
                // group comps are stored using a custom key (groupId + child colIds concatenated) as we need
                // to distinguish individual column groups after they have been split by user
                var childIds = columnGroupChild.getLeafColumns().map(function (child) { return child.getId(); }).join('-');
                return columnGroupChild.getId() + '-' + childIds;
            }
            else {
                return columnGroupChild.getId();
            }
        };
        return _this;
    }
    PrimaryColsListPanel.prototype.init = function (params, allowDragging) {
        var _this = this;
        this.params = params;
        this.allowDragging = allowDragging;
        if (!this.params.suppressSyncLayoutWithGrid) {
            this.addDestroyableEventListener(this.eventService, Events.EVENT_COLUMN_MOVED, this.onColumnsChanged.bind(this));
        }
        this.addDestroyableEventListener(this.eventService, Events.EVENT_COLUMN_EVERYTHING_CHANGED, this.onColumnsChanged.bind(this));
        var eventsImpactingCheckedState = [
            Events.EVENT_COLUMN_EVERYTHING_CHANGED,
            Events.EVENT_COLUMN_PIVOT_CHANGED,
            Events.EVENT_COLUMN_PIVOT_MODE_CHANGED,
            Events.EVENT_COLUMN_ROW_GROUP_CHANGED,
            Events.EVENT_COLUMN_VALUE_CHANGED,
            Events.EVENT_COLUMN_VISIBLE,
            Events.EVENT_NEW_COLUMNS_LOADED
        ];
        eventsImpactingCheckedState.forEach(function (event) {
            // update header select all checkbox with current selection state
            _this.addDestroyableEventListener(_this.eventService, event, _this.fireSelectionChangedEvent.bind(_this));
        });
        this.expandGroupsByDefault = !this.params.contractColumnSelection;
        if (this.columnController.isReady()) {
            this.onColumnsChanged();
        }
    };
    PrimaryColsListPanel.prototype.onColumnsChanged = function () {
        var pivotModeActive = this.columnController.isPivotMode();
        var shouldSyncColumnLayoutWithGrid = !this.params.suppressSyncLayoutWithGrid && !pivotModeActive;
        shouldSyncColumnLayoutWithGrid ? this.syncColumnLayout() : this.buildTreeFromProvidedColumnDefs();
        this.setFilterText(this.filterText);
    };
    PrimaryColsListPanel.prototype.syncColumnLayout = function () {
        this.colDefService.syncLayoutWithGrid(this.setColumnLayout.bind(this));
    };
    PrimaryColsListPanel.prototype.setColumnLayout = function (colDefs) {
        this.destroyColumnComps();
        // create column tree using supplied colDef's
        this.columnTree = this.colDefService.createColumnTree(colDefs);
        // using col defs to check if groups exist as it could be a custom layout
        var groupsExist = colDefs.some(function (colDef) {
            return colDef && typeof colDef.children !== 'undefined';
        });
        this.recursivelyAddComps(this.columnTree, 0, groupsExist);
        this.recursivelySetVisibility(this.columnTree, true);
        // notify header
        this.notifyListeners();
    };
    PrimaryColsListPanel.prototype.buildTreeFromProvidedColumnDefs = function () {
        this.destroyColumnComps();
        // add column / group comps to tool panel
        this.columnTree = this.columnController.getPrimaryColumnTree();
        var groupsExist = this.columnController.isPrimaryColumnGroupsPresent();
        this.recursivelyAddComps(this.columnTree, 0, groupsExist);
        this.recursivelySetVisibility(this.columnTree, true);
        // notify header
        this.notifyListeners();
    };
    PrimaryColsListPanel.prototype.recursivelyAddComps = function (tree, dept, groupsExist) {
        var _this = this;
        tree.forEach(function (child) {
            if (child instanceof OriginalColumnGroup) {
                _this.recursivelyAddGroupComps(child, dept, groupsExist);
            }
            else {
                _this.addColumnComps(child, dept, groupsExist);
            }
        });
    };
    PrimaryColsListPanel.prototype.recursivelyAddGroupComps = function (columnGroup, dept, groupsExist) {
        var _this = this;
        // only render group if user provided the definition
        var newDept;
        if (columnGroup.getColGroupDef() && columnGroup.getColGroupDef().suppressColumnsToolPanel) {
            return;
        }
        if (!columnGroup.isPadding()) {
            var renderedGroup = new ToolPanelColumnGroupComp(columnGroup, dept, this.allowDragging, this.expandGroupsByDefault, this.onGroupExpanded.bind(this), function () { return _this.filterResults; });
            this.getContext().wireBean(renderedGroup);
            this.getGui().appendChild(renderedGroup.getGui());
            // we want to indent on the gui for the children
            newDept = dept + 1;
            // group comps are stored using a custom key (groupId + child colIds concatenated) as we need
            // to distinguish individual column groups after they have been split by user
            var key = this.getColumnCompId(columnGroup);
            this.columnComps[key] = renderedGroup;
        }
        else {
            // no children, so no indent
            newDept = dept;
        }
        this.recursivelyAddComps(columnGroup.getChildren(), newDept, groupsExist);
    };
    PrimaryColsListPanel.prototype.addColumnComps = function (column, dept, groupsExist) {
        if (column.getColDef() && column.getColDef().suppressColumnsToolPanel) {
            return;
        }
        var columnComp = new ToolPanelColumnComp(column, dept, this.allowDragging, groupsExist);
        this.getContext().wireBean(columnComp);
        this.getGui().appendChild(columnComp.getGui());
        this.columnComps[column.getId()] = columnComp;
    };
    PrimaryColsListPanel.prototype.onGroupExpanded = function () {
        this.recursivelySetVisibility(this.columnTree, true);
        this.fireGroupExpandedEvent();
    };
    PrimaryColsListPanel.prototype.doSetExpandedAll = function (value) {
        _.iterateObject(this.columnComps, function (key, renderedItem) {
            if (renderedItem.isExpandable()) {
                renderedItem.setExpanded(value);
            }
        });
    };
    PrimaryColsListPanel.prototype.setGroupsExpanded = function (expand, groupIds) {
        var _this = this;
        var expandedGroupIds = [];
        if (!groupIds) {
            this.doSetExpandedAll(expand);
            return;
        }
        groupIds.forEach(function (suppliedGroupId) {
            // we need to search through all comps to handle the case when groups are split
            _.iterateObject(_this.columnComps, function (key, comp) {
                // check if group comp starts with supplied group id as the tool panel keys contain
                // groupId + childIds concatenated
                var foundMatchingGroupComp = key.indexOf(suppliedGroupId) === 0;
                if (foundMatchingGroupComp) {
                    comp.setExpanded(expand);
                    expandedGroupIds.push(suppliedGroupId);
                }
            });
        });
        var unrecognisedGroupIds = groupIds.filter(function (groupId) { return !_.includes(expandedGroupIds, groupId); });
        if (unrecognisedGroupIds.length > 0) {
            console.warn('ag-Grid: unable to find group(s) for supplied groupIds:', unrecognisedGroupIds);
        }
    };
    PrimaryColsListPanel.prototype.getExpandState = function () {
        var _this = this;
        var expandedCount = 0;
        var notExpandedCount = 0;
        var recursiveFunc = function (items) {
            items.forEach(function (item) {
                // only interested in groups
                if (item instanceof OriginalColumnGroup) {
                    var compId = _this.getColumnCompId(item);
                    var comp = _this.columnComps[compId];
                    if (comp) {
                        if (comp.isExpanded()) {
                            expandedCount++;
                        }
                        else {
                            notExpandedCount++;
                        }
                    }
                    var columnGroup = item;
                    var groupChildren = columnGroup.getChildren();
                    recursiveFunc(groupChildren);
                }
            });
        };
        recursiveFunc(this.columnTree);
        if (expandedCount > 0 && notExpandedCount > 0) {
            return EXPAND_STATE.INDETERMINATE;
        }
        else if (notExpandedCount > 0) {
            return EXPAND_STATE.COLLAPSED;
        }
        else {
            return EXPAND_STATE.EXPANDED;
        }
    };
    PrimaryColsListPanel.prototype.doSetSelectedAll = function (selectAllChecked) {
        this.selectAllChecked = selectAllChecked;
        this.updateSelections();
    };
    PrimaryColsListPanel.prototype.updateSelections = function () {
        var _this = this;
        if (this.columnApi.isPivotMode()) {
            // if pivot mode is on, then selecting columns has special meaning (eg group, aggregate, pivot etc),
            // so there is no bulk operation we can do.
            _.iterateObject(this.columnComps, function (key, column) {
                column.onSelectAllChanged(_this.selectAllChecked);
            });
        }
        else {
            // we don't want to change visibility on lock visible columns
            var primaryCols = this.columnApi.getPrimaryColumns();
            var colsToChange = primaryCols.filter(function (col) { return !col.getColDef().lockVisible; });
            // however if pivot mode is off, then it's all about column visibility so we can do a bulk
            // operation directly with the column controller. we could column.onSelectAllChanged(checked)
            // as above, however this would work on each column independently and take longer.
            if (!_.exists(this.filterText)) {
                this.columnController.setColumnsVisible(colsToChange, this.selectAllChecked, 'columnMenu');
                return;
            }
            // obtain list of columns currently filtered
            var filteredCols_1 = [];
            _.iterateObject(this.filterResults, function (key, passesFilter) {
                if (passesFilter)
                    filteredCols_1.push(key);
            });
            if (filteredCols_1.length > 0) {
                var filteredColsToChange = colsToChange.filter(function (col) { return _.includes(filteredCols_1, col.getColId()); });
                // update visibility of columns currently filtered
                this.columnController.setColumnsVisible(filteredColsToChange, this.selectAllChecked, 'columnMenu');
                // update select all header with new state
                var selectionState = this.selectAllChecked ? true : false;
                this.dispatchEvent({ type: 'selectionChanged', state: selectionState });
            }
        }
    };
    PrimaryColsListPanel.prototype.getSelectionState = function () {
        var _this = this;
        var allPrimaryColumns = this.columnController.getAllPrimaryColumns();
        var columns = [];
        if (allPrimaryColumns !== null) {
            columns = allPrimaryColumns.filter(function (col) { return !col.getColDef().lockVisible; });
        }
        var pivotMode = this.columnController.isPivotMode();
        var checkedCount = 0;
        var uncheckedCount = 0;
        columns.forEach(function (col) {
            // ignore lock visible columns
            if (col.getColDef().lockVisible) {
                return;
            }
            // not not count columns not in tool panel
            var colDef = col.getColDef();
            if (colDef && colDef.suppressColumnsToolPanel) {
                return;
            }
            // ignore columns that have been removed from panel by filter
            if (_this.filterResults && !_this.filterResults[col.getColId()])
                return;
            var checked;
            if (pivotMode) {
                var noPivotModeOptionsAllowed = !col.isAllowPivot() && !col.isAllowRowGroup() && !col.isAllowValue();
                if (noPivotModeOptionsAllowed) {
                    return;
                }
                checked = col.isValueActive() || col.isPivotActive() || col.isRowGroupActive();
            }
            else {
                checked = col.isVisible();
            }
            if (checked) {
                checkedCount++;
            }
            else {
                uncheckedCount++;
            }
        });
        if (checkedCount > 0 && uncheckedCount > 0) {
            return undefined;
        }
        else if (checkedCount === 0 || uncheckedCount > 0) {
            return false;
        }
        return true;
    };
    PrimaryColsListPanel.prototype.setFilterText = function (filterText) {
        this.filterText = _.exists(filterText) ? filterText.toLowerCase() : null;
        this.filterColumns();
        this.recursivelySetVisibility(this.columnTree, true);
        // groups selection state may need to be updated when filter is present
        _.iterateObject(this.columnComps, function (key, columnComp) {
            if (columnComp instanceof ToolPanelColumnGroupComp) {
                columnComp.onColumnStateChanged();
            }
        });
        // update header panel with current selection and expanded state
        this.fireSelectionChangedEvent();
        this.fireGroupExpandedEvent();
    };
    PrimaryColsListPanel.prototype.filterColumns = function () {
        var _this = this;
        var filterResults = {};
        var passesFilter = function (item) {
            if (!_.exists(_this.filterText))
                return true;
            var columnCompId = _this.getColumnCompId(item);
            var comp = _this.columnComps[columnCompId];
            if (!comp)
                return false;
            var isPaddingGroup = item instanceof OriginalColumnGroup && item.isPadding();
            if (isPaddingGroup)
                return false;
            var displayName = comp.getDisplayName();
            return displayName !== null ? displayName.toLowerCase().indexOf(_this.filterText) >= 0 : true;
        };
        var recursivelyCheckFilter = function (item, parentPasses) {
            var atLeastOneChildPassed = false;
            if (item instanceof OriginalColumnGroup) {
                var groupPasses_1 = passesFilter(item);
                item.getChildren().forEach(function (child) {
                    var childPasses = recursivelyCheckFilter(child, groupPasses_1 || parentPasses);
                    if (childPasses) {
                        atLeastOneChildPassed = childPasses;
                    }
                });
            }
            var filterPasses = (parentPasses || atLeastOneChildPassed) ? true : passesFilter(item);
            var columnCompId = _this.getColumnCompId(item);
            filterResults[columnCompId] = filterPasses;
            return filterPasses;
        };
        this.columnTree.forEach(function (item) { return recursivelyCheckFilter(item, false); });
        this.filterResults = filterResults;
    };
    PrimaryColsListPanel.prototype.recursivelySetVisibility = function (columnTree, parentGroupsOpen) {
        var _this = this;
        columnTree.forEach(function (child) {
            var compId = _this.getColumnCompId(child);
            var comp = _this.columnComps[compId];
            if (comp) {
                var filterResultExists = _this.filterResults && _.exists(_this.filterResults[compId]);
                var passesFilter = filterResultExists ? _this.filterResults[compId] : true;
                comp.setDisplayed(parentGroupsOpen && passesFilter);
            }
            if (child instanceof OriginalColumnGroup) {
                var columnGroup = child;
                var childrenOpen = void 0;
                if (comp) {
                    var expanded = comp.isExpanded();
                    childrenOpen = parentGroupsOpen ? expanded : false;
                }
                else {
                    childrenOpen = parentGroupsOpen;
                }
                var children = columnGroup.getChildren();
                _this.recursivelySetVisibility(children, childrenOpen);
            }
        });
    };
    PrimaryColsListPanel.prototype.notifyListeners = function () {
        this.fireGroupExpandedEvent();
        this.fireSelectionChangedEvent();
    };
    PrimaryColsListPanel.prototype.fireGroupExpandedEvent = function () {
        var expandState = this.getExpandState();
        this.dispatchEvent({ type: 'groupExpanded', state: expandState });
    };
    PrimaryColsListPanel.prototype.fireSelectionChangedEvent = function () {
        var selectionState = this.getSelectionState();
        this.dispatchEvent({ type: 'selectionChanged', state: selectionState });
    };
    PrimaryColsListPanel.prototype.destroyColumnComps = function () {
        _.clearElement(this.getGui());
        if (this.columnComps) {
            _.iterateObject(this.columnComps, function (key, renderedItem) { return renderedItem.destroy(); });
        }
        this.columnComps = {};
    };
    PrimaryColsListPanel.prototype.destroy = function () {
        _super.prototype.destroy.call(this);
        this.destroyColumnComps();
    };
    PrimaryColsListPanel.TEMPLATE = "<div class=\"ag-column-select-list\"></div>";
    __decorate$i([
        Autowired('columnController')
    ], PrimaryColsListPanel.prototype, "columnController", void 0);
    __decorate$i([
        Autowired('toolPanelColDefService')
    ], PrimaryColsListPanel.prototype, "colDefService", void 0);
    __decorate$i([
        Autowired('eventService')
    ], PrimaryColsListPanel.prototype, "eventService", void 0);
    __decorate$i([
        Autowired('columnApi')
    ], PrimaryColsListPanel.prototype, "columnApi", void 0);
    return PrimaryColsListPanel;
}(Component));

var __extends$8 = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate$j = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var PivotModePanel = /** @class */ (function (_super) {
    __extends$8(PivotModePanel, _super);
    function PivotModePanel() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    PivotModePanel.prototype.createTemplate = function () {
        return "<div class=\"ag-pivot-mode-panel\">\n                <ag-toggle-button ref=\"cbPivotMode\" class=\"ag-pivot-mode-select\"></ag-checkbox>\n            </div>";
    };
    PivotModePanel.prototype.init = function () {
        this.setTemplate(this.createTemplate());
        this.cbPivotMode.setValue(this.columnController.isPivotMode());
        var localeTextFunc = this.gridOptionsWrapper.getLocaleTextFunc();
        this.cbPivotMode.setLabel(localeTextFunc('pivotMode', 'Pivot Mode'));
        this.addDestroyableEventListener(this.cbPivotMode, AgCheckbox.EVENT_CHANGED, this.onBtPivotMode.bind(this));
        this.addDestroyableEventListener(this.eventService, Events.EVENT_COLUMN_EVERYTHING_CHANGED, this.onPivotModeChanged.bind(this));
        this.addDestroyableEventListener(this.eventService, Events.EVENT_COLUMN_PIVOT_MODE_CHANGED, this.onPivotModeChanged.bind(this));
    };
    PivotModePanel.prototype.onBtPivotMode = function () {
        var newValue = this.cbPivotMode.getValue();
        if (newValue !== this.columnController.isPivotMode()) {
            this.columnController.setPivotMode(newValue, "toolPanelUi");
            var api = this.gridOptionsWrapper.getApi();
            if (api) {
                api.refreshHeader();
            }
        }
    };
    PivotModePanel.prototype.onPivotModeChanged = function () {
        var pivotModeActive = this.columnController.isPivotMode();
        this.cbPivotMode.setValue(pivotModeActive);
    };
    __decorate$j([
        Autowired('columnController')
    ], PivotModePanel.prototype, "columnController", void 0);
    __decorate$j([
        Autowired('eventService')
    ], PivotModePanel.prototype, "eventService", void 0);
    __decorate$j([
        Autowired('gridOptionsWrapper')
    ], PivotModePanel.prototype, "gridOptionsWrapper", void 0);
    __decorate$j([
        RefSelector('cbPivotMode')
    ], PivotModePanel.prototype, "cbPivotMode", void 0);
    __decorate$j([
        PreConstruct
    ], PivotModePanel.prototype, "init", null);
    return PivotModePanel;
}(Component));

var __decorate$k = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var AggregationStage = /** @class */ (function () {
    function AggregationStage() {
    }
    // it's possible to recompute the aggregate without doing the other parts
    // + gridApi.recomputeAggregates()
    AggregationStage.prototype.execute = function (params) {
        // we don't do aggregation if doing legacy tree good
        var doingLegacyTreeData = _.exists(this.gridOptionsWrapper.getNodeChildDetailsFunc());
        if (doingLegacyTreeData) {
            return null;
        }
        // if changed path is active, it means we came from a) change detection or b) transaction update.
        // for both of these, if no value columns are present, it means there is nothing to aggregate now
        // and there is no cleanup to be done (as value columns don't change between transactions or change
        // detections). if no value columns and no changed path, means we have to go through all nodes in
        // case we need to clean up agg data from before.
        var noValueColumns = _.missingOrEmpty(this.columnController.getValueColumns());
        var noUserAgg = !this.gridOptionsWrapper.getGroupRowAggNodesFunc();
        var changedPathActive = params.changedPath && params.changedPath.isActive();
        if (noValueColumns && noUserAgg && changedPathActive) {
            return;
        }
        var aggDetails = this.createAggDetails(params);
        this.recursivelyCreateAggData(aggDetails);
    };
    AggregationStage.prototype.createAggDetails = function (params) {
        var pivotActive = this.columnController.isPivotActive();
        var measureColumns = this.columnController.getValueColumns();
        var pivotColumns = pivotActive ? this.columnController.getPivotColumns() : [];
        var aggDetails = {
            changedPath: params.changedPath,
            valueColumns: measureColumns,
            pivotColumns: pivotColumns
        };
        return aggDetails;
    };
    AggregationStage.prototype.recursivelyCreateAggData = function (aggDetails) {
        var _this = this;
        var callback = function (rowNode) {
            var hasNoChildren = !rowNode.hasChildren();
            if (hasNoChildren) {
                // this check is needed for TreeData, in case the node is no longer a child,
                // but it was a child previously.
                if (rowNode.aggData) {
                    rowNode.setAggData(null);
                }
                // never agg data for leaf nodes
                return;
            }
            //Optionally prevent the aggregation at the root Node
            //https://ag-grid.atlassian.net/browse/AG-388
            var isRootNode = rowNode.level === -1;
            if (isRootNode) {
                var notPivoting = !_this.columnController.isPivotMode();
                var suppressAggAtRootLevel = _this.gridOptionsWrapper.isSuppressAggAtRootLevel();
                if (suppressAggAtRootLevel && notPivoting) {
                    return;
                }
            }
            _this.aggregateRowNode(rowNode, aggDetails);
        };
        aggDetails.changedPath.forEachChangedNodeDepthFirst(callback, true);
    };
    AggregationStage.prototype.aggregateRowNode = function (rowNode, aggDetails) {
        var measureColumnsMissing = aggDetails.valueColumns.length === 0;
        var pivotColumnsMissing = aggDetails.pivotColumns.length === 0;
        var userFunc = this.gridOptionsWrapper.getGroupRowAggNodesFunc();
        var aggResult;
        if (userFunc) {
            aggResult = userFunc(rowNode.childrenAfterFilter);
        }
        else if (measureColumnsMissing) {
            aggResult = null;
        }
        else if (pivotColumnsMissing) {
            aggResult = this.aggregateRowNodeUsingValuesOnly(rowNode, aggDetails);
        }
        else {
            aggResult = this.aggregateRowNodeUsingValuesAndPivot(rowNode);
        }
        rowNode.setAggData(aggResult);
        // if we are grouping, then it's possible there is a sibling footer
        // to the group, so update the data here also if there is one
        if (rowNode.sibling) {
            rowNode.sibling.setAggData(aggResult);
        }
    };
    AggregationStage.prototype.aggregateRowNodeUsingValuesAndPivot = function (rowNode) {
        var _this = this;
        var result = {};
        var pivotColumnDefs = this.pivotStage.getPivotColumnDefs();
        // Step 1: process value columns
        pivotColumnDefs
            .filter(function (v) { return !_.exists(v.pivotTotalColumnIds); }) // only process pivot value columns
            .forEach(function (valueColDef) {
            var keys = valueColDef.pivotKeys || [];
            var values;
            var valueColumn = valueColDef.pivotValueColumn;
            var colId = valueColDef.colId;
            if (rowNode.leafGroup) {
                // lowest level group, get the values from the mapped set
                values = _this.getValuesFromMappedSet(rowNode.childrenMapped, keys, valueColumn);
            }
            else {
                // value columns and pivot columns, non-leaf group
                values = _this.getValuesPivotNonLeaf(rowNode, colId);
            }
            result[colId] = _this.aggregateValues(values, valueColumn.getAggFunc());
        });
        // Step 2: process total columns
        pivotColumnDefs
            .filter(function (v) { return _.exists(v.pivotTotalColumnIds); }) // only process pivot total columns
            .forEach(function (totalColDef) {
            var aggResults = [];
            var pivotValueColumn = totalColDef.pivotValueColumn, pivotTotalColumnIds = totalColDef.pivotTotalColumnIds, colId = totalColDef.colId;
            //retrieve results for colIds associated with this pivot total column
            if (!pivotTotalColumnIds || !pivotTotalColumnIds.length) {
                return;
            }
            pivotTotalColumnIds.forEach(function (colId) {
                aggResults.push(result[colId]);
            });
            result[colId] = _this.aggregateValues(aggResults, pivotValueColumn.getAggFunc());
        });
        return result;
    };
    AggregationStage.prototype.aggregateRowNodeUsingValuesOnly = function (rowNode, aggDetails) {
        var _this = this;
        var result = {};
        var changedValueColumns = aggDetails.changedPath.isActive() ?
            aggDetails.changedPath.getValueColumnsForNode(rowNode, aggDetails.valueColumns)
            : aggDetails.valueColumns;
        var notChangedValueColumns = aggDetails.changedPath.isActive() ?
            aggDetails.changedPath.getNotValueColumnsForNode(rowNode, aggDetails.valueColumns)
            : null;
        var values2d = this.getValuesNormal(rowNode, changedValueColumns);
        var oldValues = rowNode.aggData;
        changedValueColumns.forEach(function (valueColumn, index) {
            result[valueColumn.getId()] = _this.aggregateValues(values2d[index], valueColumn.getAggFunc());
        });
        if (notChangedValueColumns && oldValues) {
            notChangedValueColumns.forEach(function (valueColumn) {
                result[valueColumn.getId()] = oldValues[valueColumn.getId()];
            });
        }
        return result;
    };
    AggregationStage.prototype.getValuesPivotNonLeaf = function (rowNode, colId) {
        var values = [];
        rowNode.childrenAfterFilter.forEach(function (node) {
            var value = node.aggData[colId];
            values.push(value);
        });
        return values;
    };
    AggregationStage.prototype.getValuesFromMappedSet = function (mappedSet, keys, valueColumn) {
        var _this = this;
        var mapPointer = mappedSet;
        keys.forEach(function (key) { return (mapPointer = mapPointer ? mapPointer[key] : null); });
        if (!mapPointer) {
            return [];
        }
        var values = [];
        mapPointer.forEach(function (rowNode) {
            var value = _this.valueService.getValue(valueColumn, rowNode);
            values.push(value);
        });
        return values;
    };
    AggregationStage.prototype.getValuesNormal = function (rowNode, valueColumns) {
        // create 2d array, of all values for all valueColumns
        var values = [];
        valueColumns.forEach(function () { return values.push([]); });
        var valueColumnCount = valueColumns.length;
        var rowCount = rowNode.childrenAfterFilter.length;
        for (var i = 0; i < rowCount; i++) {
            var childNode = rowNode.childrenAfterFilter[i];
            for (var j = 0; j < valueColumnCount; j++) {
                var valueColumn = valueColumns[j];
                // if the row is a group, then it will only have an agg result value,
                // which means valueGetter is never used.
                var value = this.valueService.getValue(valueColumn, childNode);
                values[j].push(value);
            }
        }
        return values;
    };
    AggregationStage.prototype.aggregateValues = function (values, aggFuncOrString) {
        var aggFunction = typeof aggFuncOrString === 'string' ?
            this.aggFuncService.getAggFunc(aggFuncOrString) :
            aggFuncOrString;
        if (typeof aggFunction !== 'function') {
            console.error("ag-Grid: unrecognised aggregation function " + aggFuncOrString);
            return null;
        }
        return aggFunction(values);
    };
    __decorate$k([
        Autowired('gridOptionsWrapper')
    ], AggregationStage.prototype, "gridOptionsWrapper", void 0);
    __decorate$k([
        Autowired('columnController')
    ], AggregationStage.prototype, "columnController", void 0);
    __decorate$k([
        Autowired('valueService')
    ], AggregationStage.prototype, "valueService", void 0);
    __decorate$k([
        Autowired('pivotStage')
    ], AggregationStage.prototype, "pivotStage", void 0);
    __decorate$k([
        Autowired('aggFuncService')
    ], AggregationStage.prototype, "aggFuncService", void 0);
    AggregationStage = __decorate$k([
        Bean('aggregationStage')
    ], AggregationStage);
    return AggregationStage;
}());

var __decorate$l = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var GroupStage = /** @class */ (function () {
    function GroupStage() {
        // we use a sequence variable so that each time we do a grouping, we don't
        // reuse the ids - otherwise the rowRenderer will confuse rowNodes between redraws
        // when it tries to animate between rows. we set to -1 as others row id 0 will be shared
        // with the other rows.
        this.groupIdSequence = new NumberSequence(1);
    }
    GroupStage.prototype.postConstruct = function () {
        this.usingTreeData = this.gridOptionsWrapper.isTreeData();
        if (this.usingTreeData) {
            this.getDataPath = this.gridOptionsWrapper.getDataPathFunc();
        }
    };
    GroupStage.prototype.execute = function (params) {
        var details = this.createGroupingDetails(params);
        if (details.transaction) {
            this.handleTransaction(details);
        }
        else {
            var afterColsChanged = params.afterColumnsChanged === true;
            this.shotgunResetEverything(details, afterColsChanged);
        }
        this.sortGroupsWithComparator(details.rootNode);
        this.selectableService.updateSelectableAfterGrouping(details.rootNode);
    };
    GroupStage.prototype.createGroupingDetails = function (params) {
        var rowNode = params.rowNode, changedPath = params.changedPath, rowNodeTransaction = params.rowNodeTransaction, rowNodeOrder = params.rowNodeOrder;
        var groupedCols = this.usingTreeData ? null : this.columnController.getRowGroupColumns();
        var isGrouping = this.usingTreeData || (groupedCols && groupedCols.length > 0);
        var usingTransaction = isGrouping && _.exists(rowNodeTransaction);
        var details = {
            // someone complained that the parent attribute was causing some change detection
            // to break is some angular add-on - which i never used. taking the parent out breaks
            // a cyclic dependency, hence this flag got introduced.
            includeParents: !this.gridOptionsWrapper.isSuppressParentsInRowNodes(),
            expandByDefault: this.gridOptionsWrapper.isGroupSuppressRow() ?
                -1 : this.gridOptionsWrapper.getGroupDefaultExpanded(),
            groupedCols: groupedCols,
            rootNode: rowNode,
            pivotMode: this.columnController.isPivotMode(),
            groupedColCount: this.usingTreeData || !groupedCols ? 0 : groupedCols.length,
            rowNodeOrder: rowNodeOrder,
            // important not to do transaction if we are not grouping, as otherwise the 'insert index' is ignored.
            // ie, if not grouping, then we just want to shotgun so the rootNode.allLeafChildren gets copied
            // to rootNode.childrenAfterGroup and maintaining order (as delta transaction misses the order).
            transaction: usingTransaction ? rowNodeTransaction : null,
            // if no transaction, then it's shotgun, changed path would be 'not active' at this point anyway
            changedPath: changedPath
        };
        return details;
    };
    GroupStage.prototype.handleTransaction = function (details) {
        var tran = details.transaction;
        // remove nodes first in case a node is removed and re-added in the same transaction
        if (tran.remove) {
            this.removeNodes(tran.remove, details);
        }
        if (tran.add) {
            this.insertNodes(tran.add, details, false);
        }
        if (tran.update) {
            this.moveNodesInWrongPath(tran.update, details);
        }
        if (details.rowNodeOrder) {
            this.sortChildren(details);
        }
    };
    // this is used when doing delta updates, eg Redux, keeps nodes in right order
    GroupStage.prototype.sortChildren = function (details) {
        details.changedPath.forEachChangedNodeDepthFirst(function (rowNode) {
            _.sortRowNodesByOrder(rowNode.childrenAfterGroup, details.rowNodeOrder);
        });
    };
    GroupStage.prototype.sortGroupsWithComparator = function (rootNode) {
        // we don't do group sorting for tree data
        if (this.usingTreeData) {
            return;
        }
        var comparator = this.gridOptionsWrapper.getDefaultGroupSortComparator();
        if (_.exists(comparator)) {
            recursiveSort(rootNode);
        }
        function recursiveSort(rowNode) {
            var doSort = _.exists(rowNode.childrenAfterGroup) &&
                // we only want to sort groups, so we do not sort leafs (a leaf group has leafs as children)
                !rowNode.leafGroup;
            if (doSort) {
                rowNode.childrenAfterGroup.sort(comparator);
                rowNode.childrenAfterGroup.forEach(function (childNode) { return recursiveSort(childNode); });
            }
        }
    };
    GroupStage.prototype.getExistingPathForNode = function (node, details) {
        var res = [];
        // when doing tree data, the node is part of the path,
        // but when doing grid grouping, the node is not part of the path so we start with the parent.
        var pointer = this.usingTreeData ? node : node.parent;
        while (pointer && pointer !== details.rootNode) {
            res.push({
                key: pointer.key,
                rowGroupColumn: pointer.rowGroupColumn,
                field: pointer.field
            });
            pointer = pointer.parent;
        }
        res.reverse();
        return res;
    };
    GroupStage.prototype.moveNodesInWrongPath = function (childNodes, details) {
        var _this = this;
        childNodes.forEach(function (childNode) {
            // we add node, even if parent has not changed, as the data could have
            // changed, hence aggregations will be wrong
            if (details.changedPath.isActive()) {
                details.changedPath.addParentNode(childNode.parent);
            }
            var infoToKeyMapper = function (item) { return item.key; };
            var oldPath = _this.getExistingPathForNode(childNode, details).map(infoToKeyMapper);
            var newPath = _this.getGroupInfo(childNode, details).map(infoToKeyMapper);
            var nodeInCorrectPath = _.compareArrays(oldPath, newPath);
            if (!nodeInCorrectPath) {
                _this.moveNode(childNode, details);
            }
        });
    };
    GroupStage.prototype.moveNode = function (childNode, details) {
        this.removeNodesInStages([childNode], details);
        this.insertOneNode(childNode, details, true);
        // hack - if we didn't do this, then renaming a tree item (ie changing rowNode.key) wouldn't get
        // refreshed into the gui.
        // this is needed to kick off the event that rowComp listens to for refresh. this in turn
        // then will get each cell in the row to refresh - which is what we need as we don't know which
        // columns will be displaying the rowNode.key info.
        childNode.setData(childNode.data);
        // we add both old and new parents to changed path, as both will need to be refreshed.
        // we already added the old parent (in calling method), so just add the new parent here
        if (details.changedPath.isActive()) {
            var newParent = childNode.parent;
            details.changedPath.addParentNode(newParent);
        }
    };
    GroupStage.prototype.removeNodes = function (leafRowNodes, details) {
        this.removeNodesInStages(leafRowNodes, details);
        if (details.changedPath.isActive()) {
            leafRowNodes.forEach(function (rowNode) { return details.changedPath.addParentNode(rowNode.parent); });
        }
    };
    GroupStage.prototype.removeNodesInStages = function (leafRowNodes, details) {
        this.removeNodesFromParents(leafRowNodes, details);
        this.postRemoveCreateFillerNodes(leafRowNodes, details);
        this.postRemoveRemoveEmptyGroups(leafRowNodes, details);
    };
    GroupStage.prototype.forEachParentGroup = function (details, child, callback) {
        var pointer = child.parent;
        while (pointer && pointer !== details.rootNode) {
            callback(pointer);
            pointer = pointer.parent;
        }
    };
    GroupStage.prototype.removeNodesFromParents = function (nodesToRemove, details) {
        var _this = this;
        var batchRemover = new BatchRemover();
        nodesToRemove.forEach(function (nodeToRemove) {
            _this.removeFromParent(nodeToRemove, batchRemover);
            // remove from allLeafChildren. we clear down all parents EXCEPT the Root Node, as
            // the ClientSideNodeManager is responsible for the Root Node.
            _this.forEachParentGroup(details, nodeToRemove, function (parentNode) {
                batchRemover.removeFromAllLeafChildren(parentNode, nodeToRemove);
            });
        });
        batchRemover.flush();
    };
    GroupStage.prototype.postRemoveCreateFillerNodes = function (nodesToRemove, details) {
        var _this = this;
        nodesToRemove.forEach(function (nodeToRemove) {
            // if not group, and children are present, need to move children to a group.
            // otherwise if no children, we can just remove without replacing.
            var replaceWithGroup = nodeToRemove.hasChildren();
            if (replaceWithGroup) {
                var oldPath = _this.getExistingPathForNode(nodeToRemove, details);
                // because we just removed the userGroup, this will always return new support group
                var newGroupNode_1 = _this.findParentForNode(nodeToRemove, oldPath, details);
                // these properties are the ones that will be incorrect in the newly created group,
                // so copy them from the old childNode
                newGroupNode_1.expanded = nodeToRemove.expanded;
                newGroupNode_1.allLeafChildren = nodeToRemove.allLeafChildren;
                newGroupNode_1.childrenAfterGroup = nodeToRemove.childrenAfterGroup;
                newGroupNode_1.childrenMapped = nodeToRemove.childrenMapped;
                newGroupNode_1.childrenAfterGroup.forEach(function (rowNode) { return rowNode.parent = newGroupNode_1; });
            }
        });
    };
    GroupStage.prototype.postRemoveRemoveEmptyGroups = function (nodesToRemove, details) {
        var _this = this;
        // we do this multiple times, as when we remove groups, that means the parent of just removed
        // group can then be empty. to get around this, if we remove, then we check everything again for
        // newly emptied groups. the max number of times this will execute is the depth of the group tree.
        var checkAgain = true;
        var groupShouldBeRemoved = function (rowNode) {
            // because of the while loop below, it's possible we already moved the node,
            // so double check before trying to remove again.
            var mapKey = _this.getChildrenMappedKey(rowNode.key, rowNode.rowGroupColumn);
            var parentRowNode = rowNode.parent;
            var groupAlreadyRemoved = (parentRowNode && parentRowNode.childrenMapped) ?
                !parentRowNode.childrenMapped[mapKey] : true;
            if (groupAlreadyRemoved) {
                // if not linked, then group was already removed
                return false;
            }
            else {
                // if still not removed, then we remove if this group is empty
                return rowNode.isEmptyRowGroupNode();
            }
        };
        var _loop_1 = function () {
            checkAgain = false;
            var batchRemover = new BatchRemover();
            nodesToRemove.forEach(function (nodeToRemove) {
                // remove empty groups
                _this.forEachParentGroup(details, nodeToRemove, function (rowNode) {
                    if (groupShouldBeRemoved(rowNode)) {
                        checkAgain = true;
                        _this.removeFromParent(rowNode, batchRemover);
                        // we remove selection on filler nodes here, as the selection would not be removed
                        // from the RowNodeManager, as filler nodes don't exist on the RowNodeManager
                        rowNode.setSelected(false);
                    }
                });
            });
            batchRemover.flush();
        };
        while (checkAgain) {
            _loop_1();
        }
    };
    // removes the node from the parent by:
    // a) removing from childrenAfterGroup (using batchRemover if present, otherwise immediately)
    // b) removing from childrenMapped (immediately)
    // c) setRowTop(null) - as the rowRenderer uses this to know the RowNode is no longer needed
    GroupStage.prototype.removeFromParent = function (child, batchRemover) {
        if (child.parent) {
            if (batchRemover) {
                batchRemover.removeFromChildrenAfterGroup(child.parent, child);
            }
            else {
                _.removeFromArray(child.parent.childrenAfterGroup, child);
            }
        }
        var mapKey = this.getChildrenMappedKey(child.key, child.rowGroupColumn);
        if (child.parent && child.parent.childrenMapped) {
            child.parent.childrenMapped[mapKey] = undefined;
        }
        // this is important for transition, see rowComp removeFirstPassFuncs. when doing animation and
        // remove, if rowTop is still present, the rowComp thinks it's just moved position.
        child.setRowTop(null);
    };
    GroupStage.prototype.addToParent = function (child, parent) {
        var mapKey = this.getChildrenMappedKey(child.key, child.rowGroupColumn);
        if (parent) {
            if (parent.childrenMapped) {
                parent.childrenMapped[mapKey] = child;
            }
            parent.childrenAfterGroup.push(child);
        }
    };
    GroupStage.prototype.areGroupColsEqual = function (d1, d2) {
        if (d1 == null || d2 == null) {
            return false;
        }
        if (d1.pivotMode !== d2.pivotMode) {
            return false;
        }
        if (!_.compareArrays(d1.groupedCols, d2.groupedCols)) {
            return false;
        }
        return true;
    };
    GroupStage.prototype.shotgunResetEverything = function (details, afterColumnsChanged) {
        var skipStage = afterColumnsChanged ?
            this.usingTreeData || this.areGroupColsEqual(details, this.oldGroupingDetails)
            : false;
        this.oldGroupingDetails = details;
        if (skipStage) {
            return;
        }
        // because we are not creating the root node each time, we have the logic
        // here to change leafGroup once.
        // we set .leafGroup to false for tree data, as .leafGroup is only used when pivoting, and pivoting
        // isn't allowed with treeData, so the grid never actually use .leafGroup when doing treeData.
        details.rootNode.leafGroup = this.usingTreeData ? false : details.groupedCols.length === 0;
        // we are doing everything from scratch, so reset childrenAfterGroup and childrenMapped from the rootNode
        details.rootNode.childrenAfterGroup = [];
        details.rootNode.childrenMapped = {};
        this.insertNodes(details.rootNode.allLeafChildren, details, false);
    };
    GroupStage.prototype.insertNodes = function (newRowNodes, details, isMove) {
        var _this = this;
        newRowNodes.forEach(function (rowNode) {
            _this.insertOneNode(rowNode, details, isMove);
            if (details.changedPath.isActive()) {
                details.changedPath.addParentNode(rowNode.parent);
            }
        });
    };
    GroupStage.prototype.insertOneNode = function (childNode, details, isMove) {
        var path = this.getGroupInfo(childNode, details);
        var parentGroup = this.findParentForNode(childNode, path, details);
        if (!parentGroup.group) {
            console.warn("ag-Grid: duplicate group keys for row data, keys should be unique", [parentGroup.data, childNode.data]);
        }
        if (this.usingTreeData) {
            this.swapGroupWithUserNode(parentGroup, childNode, isMove);
        }
        else {
            childNode.parent = parentGroup;
            childNode.level = path.length;
            parentGroup.childrenAfterGroup.push(childNode);
        }
    };
    GroupStage.prototype.findParentForNode = function (childNode, path, details) {
        var _this = this;
        var nextNode = details.rootNode;
        path.forEach(function (groupInfo, level) {
            nextNode = _this.getOrCreateNextNode(nextNode, groupInfo, level, details);
            // node gets added to all group nodes.
            // note: we do not add to rootNode here, as the rootNode is the master list of rowNodes
            nextNode.allLeafChildren.push(childNode);
        });
        return nextNode;
    };
    GroupStage.prototype.swapGroupWithUserNode = function (fillerGroup, userGroup, isMove) {
        userGroup.parent = fillerGroup.parent;
        userGroup.key = fillerGroup.key;
        userGroup.field = fillerGroup.field;
        userGroup.groupData = fillerGroup.groupData;
        userGroup.level = fillerGroup.level;
        // AG-3441 - preserve the existing expanded status of the node if we're moving it, so that
        // you can drag a sub tree from one parent to another without changing its expansion
        if (!isMove) {
            userGroup.expanded = fillerGroup.expanded;
        }
        // we set .leafGroup to false for tree data, as .leafGroup is only used when pivoting, and pivoting
        // isn't allowed with treeData, so the grid never actually use .leafGroup when doing treeData.
        userGroup.leafGroup = fillerGroup.leafGroup;
        // always null for userGroups, as row grouping is not allowed when doing tree data
        userGroup.rowGroupIndex = fillerGroup.rowGroupIndex;
        userGroup.allLeafChildren = fillerGroup.allLeafChildren;
        userGroup.childrenAfterGroup = fillerGroup.childrenAfterGroup;
        userGroup.childrenMapped = fillerGroup.childrenMapped;
        this.removeFromParent(fillerGroup);
        userGroup.childrenAfterGroup.forEach(function (rowNode) { return rowNode.parent = userGroup; });
        this.addToParent(userGroup, fillerGroup.parent);
    };
    GroupStage.prototype.getOrCreateNextNode = function (parentGroup, groupInfo, level, details) {
        var mapKey = this.getChildrenMappedKey(groupInfo.key, groupInfo.rowGroupColumn);
        var nextNode = parentGroup.childrenMapped ? parentGroup.childrenMapped[mapKey] : undefined;
        if (!nextNode) {
            nextNode = this.createGroup(groupInfo, parentGroup, level, details);
            // attach the new group to the parent
            this.addToParent(nextNode, parentGroup);
        }
        return nextNode;
    };
    GroupStage.prototype.createGroup = function (groupInfo, parent, level, details) {
        var _this = this;
        var groupNode = new RowNode();
        this.context.wireBean(groupNode);
        groupNode.group = true;
        groupNode.field = groupInfo.field;
        groupNode.rowGroupColumn = groupInfo.rowGroupColumn;
        groupNode.groupData = {};
        var groupDisplayCols = this.columnController.getGroupDisplayColumns();
        groupDisplayCols.forEach(function (col) {
            // newGroup.rowGroupColumn=null when working off GroupInfo, and we always display the group in the group column
            // if rowGroupColumn is present, then it's grid row grouping and we only include if configuration says so
            var displayGroupForCol = _this.usingTreeData || (groupNode.rowGroupColumn ? col.isRowGroupDisplayed(groupNode.rowGroupColumn.getId()) : false);
            if (displayGroupForCol) {
                groupNode.groupData[col.getColId()] = groupInfo.key;
            }
        });
        // we use negative number for the ids of the groups, this makes sure we don't clash with the
        // id's of the leaf nodes.
        groupNode.id = (this.groupIdSequence.next() * -1).toString();
        groupNode.key = groupInfo.key;
        groupNode.level = level;
        groupNode.leafGroup = this.usingTreeData ? false : level === (details.groupedColCount - 1);
        // if doing pivoting, then the leaf group is never expanded,
        // as we do not show leaf rows
        if (details.pivotMode && groupNode.leafGroup) {
            groupNode.expanded = false;
        }
        else {
            groupNode.expanded = this.isExpanded(details.expandByDefault, level);
        }
        groupNode.allLeafChildren = [];
        // why is this done here? we are not updating the children could as we go,
        // i suspect this is updated in the filter stage
        groupNode.setAllChildrenCount(0);
        groupNode.rowGroupIndex = this.usingTreeData ? null : level;
        groupNode.childrenAfterGroup = [];
        groupNode.childrenMapped = {};
        groupNode.parent = details.includeParents ? parent : null;
        return groupNode;
    };
    GroupStage.prototype.getChildrenMappedKey = function (key, rowGroupColumn) {
        if (rowGroupColumn) {
            // grouping by columns
            return rowGroupColumn.getId() + '-' + key;
        }
        else {
            // tree data - we don't have rowGroupColumns
            return key;
        }
    };
    GroupStage.prototype.isExpanded = function (expandByDefault, level) {
        if (expandByDefault === -1) {
            return true;
        }
        else {
            return level < expandByDefault;
        }
    };
    GroupStage.prototype.getGroupInfo = function (rowNode, details) {
        if (this.usingTreeData) {
            return this.getGroupInfoFromCallback(rowNode);
        }
        else {
            return this.getGroupInfoFromGroupColumns(rowNode, details);
        }
    };
    GroupStage.prototype.getGroupInfoFromCallback = function (rowNode) {
        var keys = this.getDataPath ? this.getDataPath(rowNode.data) : null;
        if (keys === null || keys === undefined || keys.length === 0) {
            _.doOnce(function () { return console.warn("getDataPath() should not return an empty path for data", rowNode.data); }, 'groupStage.getGroupInfoFromCallback');
        }
        var groupInfoMapper = function (key) { return ({ key: key, field: null, rowGroupColumn: null }); };
        return keys ? keys.map(groupInfoMapper) : [];
    };
    GroupStage.prototype.getGroupInfoFromGroupColumns = function (rowNode, details) {
        var _this = this;
        var res = [];
        details.groupedCols.forEach(function (groupCol) {
            var key = _this.valueService.getKeyForNode(groupCol, rowNode);
            var keyExists = key !== null && key !== undefined;
            // unbalanced tree and pivot mode don't work together - not because of the grid, it doesn't make
            // mathematical sense as you are building up a cube. so if pivot mode, we put in a blank key where missing.
            // this keeps the tree balanced and hence can be represented as a group.
            if (details.pivotMode && !keyExists) {
                key = ' ';
                keyExists = true;
            }
            if (keyExists) {
                var item = {
                    key: key,
                    field: groupCol.getColDef().field,
                    rowGroupColumn: groupCol
                };
                res.push(item);
            }
        });
        return res;
    };
    __decorate$l([
        Autowired('selectionController')
    ], GroupStage.prototype, "selectionController", void 0);
    __decorate$l([
        Autowired('gridOptionsWrapper')
    ], GroupStage.prototype, "gridOptionsWrapper", void 0);
    __decorate$l([
        Autowired('columnController')
    ], GroupStage.prototype, "columnController", void 0);
    __decorate$l([
        Autowired('selectableService')
    ], GroupStage.prototype, "selectableService", void 0);
    __decorate$l([
        Autowired('valueService')
    ], GroupStage.prototype, "valueService", void 0);
    __decorate$l([
        Autowired('eventService')
    ], GroupStage.prototype, "eventService", void 0);
    __decorate$l([
        Autowired('context')
    ], GroupStage.prototype, "context", void 0);
    __decorate$l([
        PostConstruct
    ], GroupStage.prototype, "postConstruct", null);
    GroupStage = __decorate$l([
        Bean('groupStage')
    ], GroupStage);
    return GroupStage;
}());
// doing _.removeFromArray() multiple times on a large list can be a bottleneck.
// when doing large deletes (eg removing 1,000 rows) then we would be calling _.removeFromArray()
// a thousands of times, in particular RootNode.allGroupChildren could be a large list, and
// 1,000 removes is time consuming as each one requires traversing the full list.
// to get around this, we do all the removes in a batch. this class manages the batch.
//
// This problem was brought to light by a client (AG-2879), with dataset of 20,000
// in 10,000 groups (2 items per group), then deleting all rows with transaction,
// it took about 20 seconds to delete. with the BathRemoved, the reduced to less than 1 second.
var BatchRemover = /** @class */ (function () {
    function BatchRemover() {
        this.allSets = {};
        this.allParents = [];
    }
    BatchRemover.prototype.removeFromChildrenAfterGroup = function (parent, child) {
        var set = this.getSet(parent);
        set.removeFromChildrenAfterGroup[child.id] = true;
    };
    BatchRemover.prototype.removeFromAllLeafChildren = function (parent, child) {
        var set = this.getSet(parent);
        set.removeFromAllLeafChildren[child.id] = true;
    };
    BatchRemover.prototype.getSet = function (parent) {
        if (!this.allSets[parent.id]) {
            this.allSets[parent.id] = {
                removeFromAllLeafChildren: {},
                removeFromChildrenAfterGroup: {}
            };
            this.allParents.push(parent);
        }
        return this.allSets[parent.id];
    };
    BatchRemover.prototype.flush = function () {
        var _this = this;
        this.allParents.forEach(function (parent) {
            var nodeDetails = _this.allSets[parent.id];
            parent.childrenAfterGroup = parent.childrenAfterGroup.filter(function (child) {
                var res = !nodeDetails.removeFromChildrenAfterGroup[child.id];
                return res;
            });
            parent.allLeafChildren = parent.allLeafChildren.filter(function (child) { return !nodeDetails.removeFromAllLeafChildren[child.id]; });
        });
        this.allSets = {};
        this.allParents.length = 0;
    };
    return BatchRemover;
}());

var __decorate$m = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var PivotColDefService = /** @class */ (function () {
    function PivotColDefService() {
    }
    PivotColDefService.prototype.createPivotColumnDefs = function (uniqueValues) {
        // this is passed to the columnController, to configure the columns and groups we show
        var pivotColumnGroupDefs = [];
        // this is used by the aggregation stage, to do the aggregation based on the pivot columns
        var pivotColumnDefs = [];
        var pivotColumns = this.columnController.getPivotColumns();
        var valueColumns = this.columnController.getValueColumns();
        var levelsDeep = pivotColumns.length;
        var columnIdSequence = new NumberSequence();
        this.recursivelyAddGroup(pivotColumnGroupDefs, pivotColumnDefs, 1, uniqueValues, [], columnIdSequence, levelsDeep, pivotColumns);
        this.addRowGroupTotals(pivotColumnGroupDefs, pivotColumnDefs, valueColumns, pivotColumns, columnIdSequence);
        this.addPivotTotalsToGroups(pivotColumnGroupDefs, pivotColumnDefs, columnIdSequence);
        // we clone, so the colDefs in pivotColumnsGroupDefs and pivotColumnDefs are not shared. this is so that
        // any changes the user makes (via processSecondaryColumnDefinitions) don't impact the internal aggregations,
        // as these use the col defs also
        var pivotColumnDefsClone = pivotColumnDefs.map(function (colDef) { return _.cloneObject(colDef); });
        return {
            pivotColumnGroupDefs: pivotColumnGroupDefs,
            pivotColumnDefs: pivotColumnDefsClone
        };
    };
    // parentChildren - the list of colDefs we are adding to
    // @index - how far the column is from the top (also same as pivotKeys.length)
    // @uniqueValues - the values for which we should create a col for
    // @pivotKeys - the keys for the pivot, eg if pivoting on {Language,Country} then could be {English,Ireland}
    PivotColDefService.prototype.recursivelyAddGroup = function (parentChildren, pivotColumnDefs, index, uniqueValues, pivotKeys, columnIdSequence, levelsDeep, primaryPivotColumns) {
        var _this = this;
        _.iterateObject(uniqueValues, function (key, value) {
            var newPivotKeys = pivotKeys.slice(0);
            newPivotKeys.push(key);
            var createGroup = index !== levelsDeep;
            if (createGroup) {
                var groupDef = {
                    children: [],
                    headerName: key,
                    pivotKeys: newPivotKeys,
                    columnGroupShow: 'open',
                    groupId: 'pivot' + columnIdSequence.next()
                };
                parentChildren.push(groupDef);
                _this.recursivelyAddGroup(groupDef.children, pivotColumnDefs, index + 1, value, newPivotKeys, columnIdSequence, levelsDeep, primaryPivotColumns);
            }
            else {
                var measureColumns = _this.columnController.getValueColumns();
                var valueGroup_1 = {
                    children: [],
                    headerName: key,
                    pivotKeys: newPivotKeys,
                    columnGroupShow: 'open',
                    groupId: 'pivot' + columnIdSequence.next()
                };
                // if no value columns selected, then we insert one blank column, so the user at least sees columns
                // rendered. otherwise the grid would render with no columns (just empty groups) which would give the
                // impression that the grid is broken
                if (measureColumns.length === 0) {
                    // this is the blank column, for when no value columns enabled.
                    var colDef_1 = _this.createColDef(null, '-', newPivotKeys, columnIdSequence);
                    valueGroup_1.children.push(colDef_1);
                    pivotColumnDefs.push(colDef_1);
                }
                else {
                    measureColumns.forEach(function (measureColumn) {
                        var columnName = _this.columnController.getDisplayNameForColumn(measureColumn, 'header');
                        var colDef = _this.createColDef(measureColumn, columnName, newPivotKeys, columnIdSequence);
                        colDef.columnGroupShow = 'open';
                        valueGroup_1.children.push(colDef);
                        pivotColumnDefs.push(colDef);
                    });
                }
                parentChildren.push(valueGroup_1);
            }
        });
        // sort by either user provided comparator, or our own one
        var colDef = primaryPivotColumns[index - 1].getColDef();
        var userComparator = colDef.pivotComparator;
        var comparator = this.headerNameComparator.bind(this, userComparator);
        parentChildren.sort(comparator);
    };
    PivotColDefService.prototype.addPivotTotalsToGroups = function (pivotColumnGroupDefs, pivotColumnDefs, columnIdSequence) {
        var _this = this;
        if (!this.gridOptionsWrapper.getPivotColumnGroupTotals()) {
            return;
        }
        var insertAfter = this.gridOptionsWrapper.getPivotColumnGroupTotals() === 'after';
        var valueCols = this.columnController.getValueColumns();
        var aggFuncs = valueCols.map(function (valueCol) { return valueCol.getAggFunc(); });
        // don't add pivot totals if there is less than 1 aggFunc or they are not all the same
        if (!aggFuncs || aggFuncs.length < 1 || !this.sameAggFuncs(aggFuncs)) {
            // console.warn('ag-Grid: aborting adding pivot total columns - value columns require same aggFunc');
            return;
        }
        // arbitrarily select a value column to use as a template for pivot columns
        var valueColumn = valueCols[0];
        pivotColumnGroupDefs.forEach(function (groupDef) {
            _this.recursivelyAddPivotTotal(groupDef, pivotColumnDefs, columnIdSequence, valueColumn, insertAfter);
        });
    };
    PivotColDefService.prototype.recursivelyAddPivotTotal = function (groupDef, pivotColumnDefs, columnIdSequence, valueColumn, insertAfter) {
        var _this = this;
        var group = groupDef;
        if (!group.children) {
            var def = groupDef;
            return def.colId ? [def.colId] : null;
        }
        var colIds = [];
        // need to recurse children first to obtain colIds used in the aggregation stage
        group.children
            .forEach(function (grp) {
            var childColIds = _this.recursivelyAddPivotTotal(grp, pivotColumnDefs, columnIdSequence, valueColumn, insertAfter);
            if (childColIds) {
                colIds = colIds.concat(childColIds);
            }
        });
        // only add total colDef if there is more than 1 child node
        if (group.children.length > 1) {
            //create total colDef using an arbitrary value column as a template
            var totalColDef = this.createColDef(valueColumn, 'Total', groupDef.pivotKeys, columnIdSequence);
            totalColDef.pivotTotalColumnIds = colIds;
            totalColDef.aggFunc = valueColumn.getAggFunc();
            // add total colDef to group and pivot colDefs array
            var children = groupDef.children;
            insertAfter ? children.push(totalColDef) : children.unshift(totalColDef);
            pivotColumnDefs.push(totalColDef);
        }
        return colIds;
    };
    PivotColDefService.prototype.addRowGroupTotals = function (pivotColumnGroupDefs, pivotColumnDefs, valueColumns, pivotColumns, columnIdSequence) {
        var _this = this;
        if (!this.gridOptionsWrapper.getPivotRowTotals()) {
            return;
        }
        var insertAfter = this.gridOptionsWrapper.getPivotRowTotals() === 'after';
        // order of row group totals depends on position
        var valueCols = insertAfter ? valueColumns.slice() : valueColumns.slice().reverse();
        var _loop_1 = function (i) {
            var valueCol = valueCols[i];
            var colIds = [];
            pivotColumnGroupDefs.forEach(function (groupDef) {
                colIds = colIds.concat(_this.extractColIdsForValueColumn(groupDef, valueCol));
            });
            var levelsDeep = pivotColumns.length;
            this_1.createRowGroupTotal(pivotColumnGroupDefs, pivotColumnDefs, 1, [], columnIdSequence, levelsDeep, pivotColumns, valueCol, colIds, insertAfter);
        };
        var this_1 = this;
        for (var i = 0; i < valueCols.length; i++) {
            _loop_1(i);
        }
    };
    PivotColDefService.prototype.extractColIdsForValueColumn = function (groupDef, valueColumn) {
        var _this = this;
        var group = groupDef;
        if (!group.children) {
            var colDef = group;
            return colDef.pivotValueColumn === valueColumn && colDef.colId ? [colDef.colId] : [];
        }
        var colIds = [];
        group.children
            .forEach(function (grp) {
            _this.extractColIdsForValueColumn(grp, valueColumn);
            var childColIds = _this.extractColIdsForValueColumn(grp, valueColumn);
            colIds = colIds.concat(childColIds);
        });
        return colIds;
    };
    PivotColDefService.prototype.createRowGroupTotal = function (parentChildren, pivotColumnDefs, index, pivotKeys, columnIdSequence, levelsDeep, primaryPivotColumns, valueColumn, colIds, insertAfter) {
        var newPivotKeys = pivotKeys.slice(0);
        var createGroup = index !== levelsDeep;
        if (createGroup) {
            var groupDef = {
                children: [],
                pivotKeys: newPivotKeys,
                groupId: 'pivot' + columnIdSequence.next()
            };
            insertAfter ? parentChildren.push(groupDef) : parentChildren.unshift(groupDef);
            this.createRowGroupTotal(groupDef.children, pivotColumnDefs, index + 1, newPivotKeys, columnIdSequence, levelsDeep, primaryPivotColumns, valueColumn, colIds, insertAfter);
        }
        else {
            var measureColumns = this.columnController.getValueColumns();
            var valueGroup = {
                children: [],
                pivotKeys: newPivotKeys,
                groupId: 'pivot' + columnIdSequence.next()
            };
            if (measureColumns.length === 0) {
                var colDef = this.createColDef(null, '-', newPivotKeys, columnIdSequence);
                valueGroup.children.push(colDef);
                pivotColumnDefs.push(colDef);
            }
            else {
                var columnName = this.columnController.getDisplayNameForColumn(valueColumn, 'header');
                var colDef = this.createColDef(valueColumn, columnName, newPivotKeys, columnIdSequence);
                colDef.pivotTotalColumnIds = colIds;
                valueGroup.children.push(colDef);
                pivotColumnDefs.push(colDef);
            }
            insertAfter ? parentChildren.push(valueGroup) : parentChildren.unshift(valueGroup);
        }
    };
    PivotColDefService.prototype.createColDef = function (valueColumn, headerName, pivotKeys, columnIdSequence) {
        var colDef = {};
        if (valueColumn) {
            var colDefToCopy = valueColumn.getColDef();
            _.assign(colDef, colDefToCopy);
            // even if original column was hidden, we always show the pivot value column, otherwise it would be
            // very confusing for people thinking the pivot is broken
            colDef.hide = false;
        }
        colDef.headerName = headerName;
        colDef.colId = 'pivot_' + columnIdSequence.next();
        // pivot columns repeat over field, so it makes sense to use the unique id instead. For example if you want to
        // assign values to pinned bottom rows using setPinnedBottomRowData the value service will use this colId.
        colDef.field = colDef.colId;
        colDef.pivotKeys = pivotKeys;
        colDef.pivotValueColumn = valueColumn;
        colDef.filter = false;
        return colDef;
    };
    PivotColDefService.prototype.sameAggFuncs = function (aggFuncs) {
        if (aggFuncs.length == 1) {
            return true;
        }
        //check if all aggFunc's match
        for (var i = 1; i < aggFuncs.length; i++) {
            if (aggFuncs[i] !== aggFuncs[0]) {
                return false;
            }
        }
        return true;
    };
    PivotColDefService.prototype.headerNameComparator = function (userComparator, a, b) {
        if (userComparator) {
            return userComparator(a.headerName, b.headerName);
        }
        else {
            if (a.headerName && !b.headerName) {
                return 1;
            }
            else if (!a.headerName && b.headerName) {
                return -1;
            }
            // slightly naff here - just to satify typescript
            // really should be &&, but if so ts complains
            // the above if/else checks would deal with either being falsy, so at this stage if either are falsy, both are
            // ..still naff though
            if (!a.headerName || !b.headerName) {
                return 0;
            }
            if (a.headerName < b.headerName) {
                return -1;
            }
            else if (a.headerName > b.headerName) {
                return 1;
            }
            else {
                return 0;
            }
        }
    };
    __decorate$m([
        Autowired('columnController')
    ], PivotColDefService.prototype, "columnController", void 0);
    __decorate$m([
        Autowired('gridOptionsWrapper')
    ], PivotColDefService.prototype, "gridOptionsWrapper", void 0);
    PivotColDefService = __decorate$m([
        Bean('pivotColDefService')
    ], PivotColDefService);
    return PivotColDefService;
}());

var __decorate$n = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var PivotStage = /** @class */ (function () {
    function PivotStage() {
        this.uniqueValues = {};
    }
    PivotStage.prototype.execute = function (params) {
        var rootNode = params.rowNode;
        var changedPath = params.changedPath;
        if (this.columnController.isPivotActive()) {
            this.executePivotOn(rootNode, changedPath);
        }
        else {
            this.executePivotOff(changedPath);
        }
    };
    PivotStage.prototype.executePivotOff = function (changedPath) {
        this.aggregationColumnsHashLastTime = null;
        this.uniqueValues = {};
        if (this.columnController.isSecondaryColumnsPresent()) {
            this.columnController.setSecondaryColumns(null, "rowModelUpdated");
            if (changedPath) {
                changedPath.setInactive();
            }
        }
    };
    PivotStage.prototype.executePivotOn = function (rootNode, changedPath) {
        var uniqueValues = this.bucketUpRowNodes(rootNode);
        var uniqueValuesChanged = this.setUniqueValues(uniqueValues);
        var aggregationColumns = this.columnController.getValueColumns();
        var aggregationColumnsHash = aggregationColumns.map(function (column) { return column.getId(); }).join('#');
        var aggregationFuncsHash = aggregationColumns.map(function (column) { return column.getAggFunc().toString(); }).join('#');
        var aggregationColumnsChanged = this.aggregationColumnsHashLastTime !== aggregationColumnsHash;
        var aggregationFuncsChanged = this.aggregationFuncsHashLastTime !== aggregationFuncsHash;
        this.aggregationColumnsHashLastTime = aggregationColumnsHash;
        this.aggregationFuncsHashLastTime = aggregationFuncsHash;
        if (uniqueValuesChanged || aggregationColumnsChanged || aggregationFuncsChanged) {
            var result = this.pivotColDefService.createPivotColumnDefs(this.uniqueValues);
            this.pivotColumnGroupDefs = result.pivotColumnGroupDefs;
            this.pivotColumnDefs = result.pivotColumnDefs;
            this.columnController.setSecondaryColumns(this.pivotColumnGroupDefs, "rowModelUpdated");
            // because the secondary columns have changed, then the aggregation needs to visit the whole
            // tree again, so we make the changedPath not active, to force aggregation to visit all paths.
            if (changedPath) {
                changedPath.setInactive();
            }
        }
    };
    PivotStage.prototype.setUniqueValues = function (newValues) {
        var json1 = JSON.stringify(newValues);
        var json2 = JSON.stringify(this.uniqueValues);
        var uniqueValuesChanged = json1 !== json2;
        // we only continue the below if the unique values are different, as otherwise
        // the result will be the same as the last time we did it
        if (uniqueValuesChanged) {
            this.uniqueValues = newValues;
            return true;
        }
        else {
            return false;
        }
    };
    // returns true if values were different
    PivotStage.prototype.bucketUpRowNodes = function (rootNode) {
        var _this = this;
        // accessed from inside inner function
        var uniqueValues = {};
        // finds all leaf groups and calls mapRowNode with it
        var recursivelySearchForLeafNodes = function (rowNode) {
            if (rowNode.leafGroup) {
                _this.bucketRowNode(rowNode, uniqueValues);
            }
            else {
                rowNode.childrenAfterFilter.forEach(function (child) {
                    recursivelySearchForLeafNodes(child);
                });
            }
        };
        recursivelySearchForLeafNodes(rootNode);
        return uniqueValues;
    };
    PivotStage.prototype.bucketRowNode = function (rowNode, uniqueValues) {
        var pivotColumns = this.columnController.getPivotColumns();
        if (pivotColumns.length === 0) {
            rowNode.childrenMapped = null;
            return;
        }
        rowNode.childrenMapped = this.bucketChildren(rowNode.childrenAfterFilter, pivotColumns, 0, uniqueValues);
    };
    PivotStage.prototype.bucketChildren = function (children, pivotColumns, pivotIndex, uniqueValues) {
        var _this = this;
        var mappedChildren = {};
        var pivotColumn = pivotColumns[pivotIndex];
        // map the children out based on the pivot column
        children.forEach(function (child) {
            var key = _this.valueService.getKeyForNode(pivotColumn, child);
            if (_.missing(key)) {
                key = '';
            }
            if (!uniqueValues[key]) {
                uniqueValues[key] = {};
            }
            if (!mappedChildren[key]) {
                mappedChildren[key] = [];
            }
            mappedChildren[key].push(child);
        });
        // if it's the last pivot column, return as is, otherwise go one level further in the map
        if (pivotIndex === pivotColumns.length - 1) {
            return mappedChildren;
        }
        else {
            var result_1 = {};
            _.iterateObject(mappedChildren, function (key, value) {
                result_1[key] = _this.bucketChildren(value, pivotColumns, pivotIndex + 1, uniqueValues[key]);
            });
            return result_1;
        }
    };
    PivotStage.prototype.getPivotColumnDefs = function () {
        return this.pivotColumnDefs;
    };
    __decorate$n([
        Autowired('rowModel')
    ], PivotStage.prototype, "rowModel", void 0);
    __decorate$n([
        Autowired('valueService')
    ], PivotStage.prototype, "valueService", void 0);
    __decorate$n([
        Autowired('columnController')
    ], PivotStage.prototype, "columnController", void 0);
    __decorate$n([
        Autowired('eventService')
    ], PivotStage.prototype, "eventService", void 0);
    __decorate$n([
        Autowired('pivotColDefService')
    ], PivotStage.prototype, "pivotColDefService", void 0);
    PivotStage = __decorate$n([
        Bean('pivotStage')
    ], PivotStage);
    return PivotStage;
}());

var __decorate$o = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var AggFuncService = /** @class */ (function () {
    function AggFuncService() {
        this.aggFuncsMap = {};
        this.initialised = false;
    }
    AggFuncService_1 = AggFuncService;
    AggFuncService.prototype.init = function () {
        if (this.initialised) {
            return;
        }
        this.initialiseWithDefaultAggregations();
        this.addAggFuncs(this.gridOptionsWrapper.getAggFuncs());
    };
    AggFuncService.prototype.initialiseWithDefaultAggregations = function () {
        this.aggFuncsMap[AggFuncService_1.AGG_SUM] = aggSum;
        this.aggFuncsMap[AggFuncService_1.AGG_FIRST] = aggFirst;
        this.aggFuncsMap[AggFuncService_1.AGG_LAST] = aggLast;
        this.aggFuncsMap[AggFuncService_1.AGG_MIN] = aggMin;
        this.aggFuncsMap[AggFuncService_1.AGG_MAX] = aggMax;
        this.aggFuncsMap[AggFuncService_1.AGG_COUNT] = aggCount;
        this.aggFuncsMap[AggFuncService_1.AGG_AVG] = aggAvg;
        this.initialised = true;
    };
    AggFuncService.prototype.getDefaultAggFunc = function (column) {
        var allKeys = this.getFuncNames(column);
        // use 'sum' if it's a) allowed for the column and b) still registered
        // (ie not removed by user)
        var sumInKeysList = _.includes(allKeys, AggFuncService_1.AGG_SUM);
        var sumInFuncs = _.exists(this.aggFuncsMap[AggFuncService_1.AGG_SUM]);
        if (sumInKeysList && sumInFuncs) {
            return AggFuncService_1.AGG_SUM;
        }
        else {
            return _.existsAndNotEmpty(allKeys) ? allKeys[0] : null;
        }
    };
    AggFuncService.prototype.addAggFuncs = function (aggFuncs) {
        _.iterateObject(aggFuncs, this.addAggFunc.bind(this));
    };
    AggFuncService.prototype.addAggFunc = function (key, aggFunc) {
        this.init();
        this.aggFuncsMap[key] = aggFunc;
    };
    AggFuncService.prototype.getAggFunc = function (name) {
        this.init();
        return this.aggFuncsMap[name];
    };
    AggFuncService.prototype.getFuncNames = function (column) {
        var userAllowedFuncs = column.getColDef().allowedAggFuncs;
        if (_.exists(userAllowedFuncs) && userAllowedFuncs) {
            return userAllowedFuncs;
        }
        else {
            return Object.keys(this.aggFuncsMap).sort();
        }
    };
    AggFuncService.prototype.clear = function () {
        this.aggFuncsMap = {};
    };
    var AggFuncService_1;
    AggFuncService.AGG_SUM = 'sum';
    AggFuncService.AGG_FIRST = 'first';
    AggFuncService.AGG_LAST = 'last';
    AggFuncService.AGG_MIN = 'min';
    AggFuncService.AGG_MAX = 'max';
    AggFuncService.AGG_COUNT = 'count';
    AggFuncService.AGG_AVG = 'avg';
    __decorate$o([
        Autowired('gridOptionsWrapper')
    ], AggFuncService.prototype, "gridOptionsWrapper", void 0);
    __decorate$o([
        PostConstruct
    ], AggFuncService.prototype, "init", null);
    AggFuncService = AggFuncService_1 = __decorate$o([
        Bean('aggFuncService')
    ], AggFuncService);
    return AggFuncService;
}());
function aggSum(input) {
    return input
        .filter(function (value) { return typeof value === 'number'; })
        .reduce(function (sum, value) { return sum === null ? value : sum + value; }, null);
}
function aggFirst(input) {
    return input.length > 0 ? input[0] : null;
}
function aggLast(input) {
    return input.length > 0 ? _.last(input) : null;
}
function aggMin(input) {
    return input
        .filter(function (value) { return typeof value === 'number'; })
        .reduce(function (min, value) { return min === null || value < min ? value : min; }, null);
}
function aggMax(input) {
    return input
        .filter(function (value) { return typeof value === 'number'; })
        .reduce(function (max, value) { return max === null || value > max ? value : max; }, null);
}
function aggCount(input) {
    var value = input.reduce(function (count, item) {
        var isGroupAgg = _.exists(item) && typeof item.value === 'number';
        return count + (isGroupAgg ? item.value : 1);
    }, 0);
    return {
        value: value,
        toString: function () {
            return this.value.toString();
        },
        // used for sorting
        toNumber: function () {
            return this.value;
        }
    };
}
// the average function is tricky as the multiple levels require weighted averages
// for the non-leaf node aggregations.
function aggAvg(input) {
    // the average will be the sum / count
    var _a = input.reduce(function (_a, item) {
        var sum = _a.sum, count = _a.count;
        var itemIsGroupResult = _.exists(item) &&
            typeof item.value === 'number' &&
            typeof item.count === 'number';
        if (typeof item === 'number') {
            return { sum: sum + item, count: count + 1 };
        }
        if (itemIsGroupResult) {
            // we are aggregating groups, so we take the
            // aggregated values to calculated a weighted average
            return {
                sum: sum + item.value * item.count,
                count: count + item.count
            };
        }
        return { sum: sum, count: count };
    }, { sum: 0, count: 0 }), sum = _a.sum, count = _a.count;
    // avoid divide by zero error
    var value = count > 0 ? sum / count : null;
    // the result will be an object. when this cell is rendered, only the avg is shown.
    // however when this cell is part of another aggregation, the count is also needed
    // to create a weighted average for the next level.
    return {
        count: count,
        value: value,
        // the grid by default uses toString to render values for an object, so this
        // is a trick to get the default cellRenderer to display the avg value
        toString: function () {
            if (typeof this.value === 'number') {
                return this.value.toString();
            }
            else {
                return '';
            }
        },
        // used for sorting
        toNumber: function () {
            return this.value;
        }
    };
}

var __extends$9 = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate$p = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var DropZoneColumnComp = /** @class */ (function (_super) {
    __extends$9(DropZoneColumnComp, _super);
    function DropZoneColumnComp(column, dragSourceDropTarget, ghost, valueColumn, horizontal) {
        var _this = _super.call(this) || this;
        _this.column = column;
        _this.dragSourceDropTarget = dragSourceDropTarget;
        _this.ghost = ghost;
        _this.valueColumn = valueColumn;
        _this.horizontal = horizontal;
        _this.popupShowing = false;
        return _this;
    }
    DropZoneColumnComp.prototype.init = function () {
        this.setTemplate(DropZoneColumnComp.TEMPLATE);
        this.addElementClasses(this.getGui());
        this.addElementClasses(this.eDragHandle, 'drag-handle');
        this.addElementClasses(this.eText, 'text');
        this.addElementClasses(this.eButton, 'button');
        this.eDragHandle.appendChild(_.createIconNoSpan('columnDrag', this.gridOptionsWrapper));
        this.eButton.appendChild(_.createIconNoSpan('cancel', this.gridOptionsWrapper));
        this.displayName = this.columnController.getDisplayNameForColumn(this.column, 'columnDrop');
        this.setupComponents();
        if (!this.ghost && !this.gridOptionsWrapper.isFunctionsReadOnly()) {
            this.addDragSource();
        }
    };
    DropZoneColumnComp.prototype.addDragSource = function () {
        var _this = this;
        var dragSource = {
            type: DragSourceType.ToolPanel,
            eElement: this.eDragHandle,
            defaultIconName: DragAndDropService.ICON_HIDE,
            getDragItem: function () { return _this.createDragItem(); },
            dragItemName: this.displayName,
            dragSourceDropTarget: this.dragSourceDropTarget
        };
        this.dragAndDropService.addDragSource(dragSource, true);
        this.addDestroyFunc(function () { return _this.dragAndDropService.removeDragSource(dragSource); });
    };
    DropZoneColumnComp.prototype.createDragItem = function () {
        var visibleState = {};
        visibleState[this.column.getId()] = this.column.isVisible();
        return {
            columns: [this.column],
            visibleState: visibleState
        };
    };
    DropZoneColumnComp.prototype.setupComponents = function () {
        this.setTextValue();
        this.setupRemove();
        if (this.ghost) {
            _.addCssClass(this.getGui(), 'ag-column-drop-cell-ghost');
        }
        if (this.valueColumn && !this.gridOptionsWrapper.isFunctionsReadOnly()) {
            this.addGuiEventListener('click', this.onShowAggFuncSelection.bind(this));
        }
    };
    DropZoneColumnComp.prototype.setupRemove = function () {
        var _this = this;
        _.setDisplayed(this.eButton, !this.gridOptionsWrapper.isFunctionsReadOnly());
        this.addDestroyableEventListener(this.eButton, 'click', function (mouseEvent) {
            var agEvent = { type: DropZoneColumnComp.EVENT_COLUMN_REMOVE };
            _this.dispatchEvent(agEvent);
            mouseEvent.stopPropagation();
        });
        var touchListener = new TouchListener(this.eButton);
        this.addDestroyableEventListener(touchListener, TouchListener.EVENT_TAP, function (event) {
            var agEvent = { type: DropZoneColumnComp.EVENT_COLUMN_REMOVE };
            _this.dispatchEvent(agEvent);
        });
        this.addDestroyFunc(touchListener.destroy.bind(touchListener));
    };
    DropZoneColumnComp.prototype.setTextValue = function () {
        var displayValue;
        if (this.valueColumn) {
            var aggFunc = this.column.getAggFunc();
            // if aggFunc is a string, we can use it, but if it's a function, then we swap with 'func'
            var aggFuncString = (typeof aggFunc === 'string') ? aggFunc : 'agg';
            var localeTextFunc = this.gridOptionsWrapper.getLocaleTextFunc();
            var aggFuncStringTranslated = localeTextFunc(aggFuncString, aggFuncString);
            displayValue = aggFuncStringTranslated + "(" + this.displayName + ")";
        }
        else {
            displayValue = this.displayName;
        }
        var displayValueSanitised = _.escape(displayValue);
        this.eText.innerHTML = displayValueSanitised;
    };
    DropZoneColumnComp.prototype.onShowAggFuncSelection = function () {
        var _this = this;
        if (this.popupShowing) {
            return;
        }
        this.popupShowing = true;
        var virtualList = new VirtualList('select-agg-func');
        var rows = this.aggFuncService.getFuncNames(this.column);
        virtualList.setModel({
            getRow: function (index) { return rows[index]; },
            getRowCount: function () { return rows.length; }
        });
        this.getContext().wireBean(virtualList);
        var ePopup = _.loadTemplate('<div class="ag-select-agg-func-popup"></div>');
        ePopup.style.top = '0px';
        ePopup.style.left = '0px';
        ePopup.appendChild(virtualList.getGui());
        // ePopup.style.height = this.gridOptionsWrapper.getAggFuncPopupHeight() + 'px';
        ePopup.style.width = this.getGui().clientWidth + 'px';
        var popupHiddenFunc = function () {
            virtualList.destroy();
            _this.popupShowing = false;
        };
        var hidePopup = this.popupService.addAsModalPopup(ePopup, true, popupHiddenFunc);
        virtualList.setComponentCreator(this.createAggSelect.bind(this, hidePopup));
        this.popupService.positionPopupUnderComponent({
            type: 'aggFuncSelect',
            eventSource: this.getGui(),
            ePopup: ePopup,
            keepWithinBounds: true,
            column: this.column
        });
        virtualList.refresh();
    };
    DropZoneColumnComp.prototype.createAggSelect = function (hidePopup, value) {
        var _this = this;
        var itemSelected = function () {
            hidePopup();
            if (_this.gridOptionsWrapper.isFunctionsPassive()) {
                var event_1 = {
                    type: Events.EVENT_COLUMN_AGG_FUNC_CHANGE_REQUEST,
                    columns: [_this.column],
                    aggFunc: value,
                    api: _this.gridApi,
                    columnApi: _this.columnApi
                };
                _this.eventService.dispatchEvent(event_1);
            }
            else {
                _this.columnController.setColumnAggFunc(_this.column, value, "toolPanelDragAndDrop");
            }
        };
        var localeTextFunc = this.gridOptionsWrapper.getLocaleTextFunc();
        var aggFuncString = value.toString();
        var aggFuncStringTranslated = localeTextFunc(aggFuncString, aggFuncString);
        var comp = new AggItemComp(itemSelected, aggFuncStringTranslated);
        return comp;
    };
    DropZoneColumnComp.prototype.addElementClasses = function (el, suffix) {
        suffix = suffix ? "-" + suffix : '';
        _.addCssClass(el, "ag-column-drop-cell" + suffix);
        var direction = this.horizontal ? 'horizontal' : 'vertical';
        _.addCssClass(el, "ag-column-drop-" + direction + "-cell" + suffix);
    };
    DropZoneColumnComp.EVENT_COLUMN_REMOVE = 'columnRemove';
    DropZoneColumnComp.TEMPLATE = "<span>\n          <span ref=\"eDragHandle\" class=\"ag-drag-handle ag-column-drop-cell-drag-handle\"></span>\n          <span ref=\"eText\" class=\"ag-column-drop-cell-text\"></span>\n          <span ref=\"eButton\" class=\"ag-column-drop-cell-button\"></span>\n        </span>";
    __decorate$p([
        Autowired('dragAndDropService')
    ], DropZoneColumnComp.prototype, "dragAndDropService", void 0);
    __decorate$p([
        Autowired('columnController')
    ], DropZoneColumnComp.prototype, "columnController", void 0);
    __decorate$p([
        Autowired('popupService')
    ], DropZoneColumnComp.prototype, "popupService", void 0);
    __decorate$p([
        Optional('aggFuncService')
    ], DropZoneColumnComp.prototype, "aggFuncService", void 0);
    __decorate$p([
        Autowired('gridOptionsWrapper')
    ], DropZoneColumnComp.prototype, "gridOptionsWrapper", void 0);
    __decorate$p([
        Autowired('eventService')
    ], DropZoneColumnComp.prototype, "eventService", void 0);
    __decorate$p([
        Autowired('columnApi')
    ], DropZoneColumnComp.prototype, "columnApi", void 0);
    __decorate$p([
        Autowired('gridApi')
    ], DropZoneColumnComp.prototype, "gridApi", void 0);
    __decorate$p([
        RefSelector('eText')
    ], DropZoneColumnComp.prototype, "eText", void 0);
    __decorate$p([
        RefSelector('eDragHandle')
    ], DropZoneColumnComp.prototype, "eDragHandle", void 0);
    __decorate$p([
        RefSelector('eButton')
    ], DropZoneColumnComp.prototype, "eButton", void 0);
    __decorate$p([
        PostConstruct
    ], DropZoneColumnComp.prototype, "init", null);
    return DropZoneColumnComp;
}(Component));
var AggItemComp = /** @class */ (function (_super) {
    __extends$9(AggItemComp, _super);
    function AggItemComp(itemSelected, value) {
        var _this = _super.call(this, '<div class="ag-select-agg-func-item"/>') || this;
        _this.getGui().innerText = value;
        _this.value = value;
        _this.addGuiEventListener('click', itemSelected);
        return _this;
    }
    return AggItemComp;
}(Component));

var __extends$a = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var BaseDropZonePanel = /** @class */ (function (_super) {
    __extends$a(BaseDropZonePanel, _super);
    function BaseDropZonePanel(horizontal, valueColumn) {
        var _this = _super.call(this, "<div class=\"ag-unselectable\"></div>") || this;
        _this.horizontal = horizontal;
        _this.valueColumn = valueColumn;
        _this.state = BaseDropZonePanel.STATE_NOT_DRAGGING;
        _this.guiDestroyFunctions = [];
        _this.childColumnComponents = [];
        _this.addElementClasses(_this.getGui());
        _this.eColumnDropList = document.createElement('div');
        _this.addElementClasses(_this.eColumnDropList, 'list');
        return _this;
    }
    BaseDropZonePanel.prototype.isHorizontal = function () {
        return this.horizontal;
    };
    BaseDropZonePanel.prototype.setBeans = function (beans) {
        this.beans = beans;
    };
    BaseDropZonePanel.prototype.destroy = function () {
        this.destroyGui();
        _super.prototype.destroy.call(this);
    };
    BaseDropZonePanel.prototype.destroyGui = function () {
        this.guiDestroyFunctions.forEach(function (func) { return func(); });
        this.guiDestroyFunctions.length = 0;
        this.childColumnComponents.length = 0;
        _.clearElement(this.getGui());
        _.clearElement(this.eColumnDropList);
    };
    BaseDropZonePanel.prototype.init = function (params) {
        this.params = params;
        this.beans.eventService.addEventListener(Events.EVENT_COLUMN_EVERYTHING_CHANGED, this.refreshGui.bind(this));
        this.addDestroyableEventListener(this.beans.gridOptionsWrapper, 'functionsReadOnly', this.refreshGui.bind(this));
        this.setupDropTarget();
        // we don't know if this bean will be initialised before columnController.
        // if columnController first, then below will work
        // if columnController second, then below will put blank in, and then above event gets first when columnController is set up
        this.refreshGui();
    };
    BaseDropZonePanel.prototype.addElementClasses = function (el, suffix) {
        suffix = suffix ? "-" + suffix : '';
        _.addCssClass(el, "ag-column-drop" + suffix);
        var direction = this.horizontal ? 'horizontal' : 'vertical';
        _.addCssClass(el, "ag-column-drop-" + direction + suffix);
    };
    BaseDropZonePanel.prototype.setupDropTarget = function () {
        this.dropTarget = {
            getContainer: this.getGui.bind(this),
            getIconName: this.getIconName.bind(this),
            onDragging: this.onDragging.bind(this),
            onDragEnter: this.onDragEnter.bind(this),
            onDragLeave: this.onDragLeave.bind(this),
            onDragStop: this.onDragStop.bind(this),
            isInterestedIn: this.isInterestedIn.bind(this)
        };
        this.beans.dragAndDropService.addDropTarget(this.dropTarget);
    };
    BaseDropZonePanel.prototype.isInterestedIn = function (type) {
        // not interested in row drags
        return type === DragSourceType.HeaderCell || type === DragSourceType.ToolPanel;
    };
    BaseDropZonePanel.prototype.checkInsertIndex = function (draggingEvent) {
        var newIndex = this.horizontal ? this.getNewHorizontalInsertIndex(draggingEvent) : this.getNewVerticalInsertIndex(draggingEvent);
        // <0 happens when drag is no a direction we are interested in, eg drag is up/down but in horizontal panel
        if (newIndex < 0) {
            return false;
        }
        var changed = newIndex !== this.insertIndex;
        if (changed) {
            this.insertIndex = newIndex;
        }
        return changed;
    };
    BaseDropZonePanel.prototype.getNewHorizontalInsertIndex = function (draggingEvent) {
        if (_.missing(draggingEvent.hDirection)) {
            return -1;
        }
        var newIndex = 0;
        var mouseEvent = draggingEvent.event;
        var enableRtl = this.beans.gridOptionsWrapper.isEnableRtl();
        var goingLeft = draggingEvent.hDirection === HorizontalDirection.Left;
        var mouseX = mouseEvent.clientX;
        this.childColumnComponents.forEach(function (childColumn) {
            var rect = childColumn.getGui().getBoundingClientRect();
            var rectX = goingLeft ? rect.right : rect.left;
            var horizontalFit = enableRtl ? mouseX <= rectX : mouseX >= rectX;
            if (horizontalFit) {
                newIndex++;
            }
        });
        return newIndex;
    };
    BaseDropZonePanel.prototype.getNewVerticalInsertIndex = function (draggingEvent) {
        if (_.missing(draggingEvent.vDirection)) {
            return -1;
        }
        var newIndex = 0;
        var mouseEvent = draggingEvent.event;
        this.childColumnComponents.forEach(function (childColumn) {
            var rect = childColumn.getGui().getBoundingClientRect();
            var verticalFit = mouseEvent.clientY >= (draggingEvent.vDirection === VerticalDirection.Down ? rect.top : rect.bottom);
            if (verticalFit) {
                newIndex++;
            }
        });
        return newIndex;
    };
    BaseDropZonePanel.prototype.checkDragStartedBySelf = function (draggingEvent) {
        if (this.state !== BaseDropZonePanel.STATE_NOT_DRAGGING) {
            return;
        }
        this.state = BaseDropZonePanel.STATE_REARRANGE_COLUMNS;
        this.potentialDndColumns = draggingEvent.dragSource.getDragItem().columns || [];
        this.refreshGui();
        this.checkInsertIndex(draggingEvent);
        this.refreshGui();
    };
    BaseDropZonePanel.prototype.onDragging = function (draggingEvent) {
        this.checkDragStartedBySelf(draggingEvent);
        if (this.checkInsertIndex(draggingEvent)) {
            this.refreshGui();
        }
    };
    BaseDropZonePanel.prototype.onDragEnter = function (draggingEvent) {
        // this will contain all columns that are potential drops
        var dragColumns = draggingEvent.dragSource.getDragItem().columns || [];
        this.state = BaseDropZonePanel.STATE_NEW_COLUMNS_IN;
        // take out columns that are not droppable
        var goodDragColumns = dragColumns.filter(this.isColumnDroppable.bind(this));
        if (goodDragColumns.length > 0) {
            this.potentialDndColumns = goodDragColumns;
            this.checkInsertIndex(draggingEvent);
            this.refreshGui();
        }
    };
    BaseDropZonePanel.prototype.isPotentialDndColumns = function () {
        return _.existsAndNotEmpty(this.potentialDndColumns);
    };
    BaseDropZonePanel.prototype.onDragLeave = function (draggingEvent) {
        // if the dragging started from us, we remove the group, however if it started
        // someplace else, then we don't, as it was only 'asking'
        if (this.state === BaseDropZonePanel.STATE_REARRANGE_COLUMNS) {
            var columns = draggingEvent.dragSource.getDragItem().columns || [];
            this.removeColumns(columns);
        }
        if (this.isPotentialDndColumns()) {
            this.potentialDndColumns = [];
            this.refreshGui();
        }
        this.state = BaseDropZonePanel.STATE_NOT_DRAGGING;
    };
    BaseDropZonePanel.prototype.onDragStop = function () {
        if (this.isPotentialDndColumns()) {
            var success = false;
            if (this.state === BaseDropZonePanel.STATE_NEW_COLUMNS_IN) {
                this.addColumns(this.potentialDndColumns);
                success = true;
            }
            else {
                success = this.rearrangeColumns(this.potentialDndColumns);
            }
            this.potentialDndColumns = [];
            // If the function is passive, then we don't refresh, as we assume the client application
            // is going to call setRowGroups / setPivots / setValues at a later point which will then
            // cause a refresh. This gives a nice GUI where the ghost stays until the app has caught
            // up with the changes. However, if there was no change in the order, then we do need to
            // refresh to reset the columns
            if (!this.beans.gridOptionsWrapper.isFunctionsPassive() || !success) {
                this.refreshGui();
            }
        }
        this.state = BaseDropZonePanel.STATE_NOT_DRAGGING;
    };
    BaseDropZonePanel.prototype.removeColumns = function (columnsToRemove) {
        var newColumnList = this.getExistingColumns().filter(function (col) { return !_.includes(columnsToRemove, col); });
        this.updateColumns(newColumnList);
    };
    BaseDropZonePanel.prototype.addColumns = function (columnsToAdd) {
        var newColumnList = this.getExistingColumns().slice();
        _.insertArrayIntoArray(newColumnList, columnsToAdd, this.insertIndex);
        this.updateColumns(newColumnList);
    };
    BaseDropZonePanel.prototype.rearrangeColumns = function (columnsToAdd) {
        var newColumnList = this.getNonGhostColumns().slice();
        _.insertArrayIntoArray(newColumnList, columnsToAdd, this.insertIndex);
        if (_.shallowCompare(newColumnList, this.getExistingColumns())) {
            return false;
        }
        else {
            this.updateColumns(newColumnList);
            return true;
        }
    };
    BaseDropZonePanel.prototype.refreshGui = function () {
        // we reset the scroll position after the refresh.
        // if we don't do this, then the list will always scroll to the top
        // each time we refresh it. this is because part of the refresh empties
        // out the list which sets scroll to zero. so the user could be just
        // reordering the list - we want to prevent the resetting of the scroll.
        // this is relevant for vertical display only (as horizontal has no scroll)
        var scrollTop = this.eColumnDropList.scrollTop;
        this.destroyGui();
        this.addIconAndTitleToGui();
        this.addEmptyMessageToGui();
        this.addColumnsToGui();
        if (!this.isHorizontal()) {
            this.eColumnDropList.scrollTop = scrollTop;
        }
    };
    BaseDropZonePanel.prototype.getNonGhostColumns = function () {
        var _this = this;
        var existingColumns = this.getExistingColumns();
        if (this.isPotentialDndColumns()) {
            return existingColumns.filter(function (column) { return !_.includes(_this.potentialDndColumns, column); });
        }
        else {
            return existingColumns;
        }
    };
    BaseDropZonePanel.prototype.addColumnsToGui = function () {
        var _this = this;
        var nonGhostColumns = this.getNonGhostColumns();
        var addingGhosts = this.isPotentialDndColumns();
        var itemsToAddToGui = [];
        nonGhostColumns.forEach(function (column, index) {
            if (addingGhosts && index >= _this.insertIndex) {
                return;
            }
            var columnComponent = _this.createColumnComponent(column, false);
            itemsToAddToGui.push(columnComponent);
        });
        if (this.isPotentialDndColumns()) {
            this.potentialDndColumns.forEach(function (column) {
                var columnComponent = _this.createColumnComponent(column, true);
                itemsToAddToGui.push(columnComponent);
            });
            nonGhostColumns.forEach(function (column, index) {
                if (index < _this.insertIndex) {
                    return;
                }
                var columnComponent = _this.createColumnComponent(column, false);
                itemsToAddToGui.push(columnComponent);
            });
        }
        this.getGui().appendChild(this.eColumnDropList);
        itemsToAddToGui.forEach(function (columnComponent, index) {
            if (index > 0) {
                _this.addArrow(_this.eColumnDropList);
            }
            _this.eColumnDropList.appendChild(columnComponent.getGui());
        });
    };
    BaseDropZonePanel.prototype.createColumnComponent = function (column, ghost) {
        var columnComponent = new DropZoneColumnComp(column, this.dropTarget, ghost, this.valueColumn, this.horizontal);
        columnComponent.addEventListener(DropZoneColumnComp.EVENT_COLUMN_REMOVE, this.removeColumns.bind(this, [column]));
        this.beans.context.wireBean(columnComponent);
        this.guiDestroyFunctions.push(function () { return columnComponent.destroy(); });
        if (!ghost) {
            this.childColumnComponents.push(columnComponent);
        }
        return columnComponent;
    };
    BaseDropZonePanel.prototype.addIconAndTitleToGui = function () {
        var eGroupIcon = this.params.icon;
        var eTitleBar = document.createElement('div');
        this.addElementClasses(eTitleBar, 'title-bar');
        this.addElementClasses(eGroupIcon, 'icon');
        _.addOrRemoveCssClass(this.getGui(), 'ag-column-drop-empty', this.isExistingColumnsEmpty());
        eTitleBar.appendChild(eGroupIcon);
        if (!this.horizontal) {
            var eTitle = document.createElement('span');
            this.addElementClasses(eTitle, 'title');
            eTitle.innerHTML = this.params.title;
            eTitleBar.appendChild(eTitle);
        }
        this.getGui().appendChild(eTitleBar);
    };
    BaseDropZonePanel.prototype.isExistingColumnsEmpty = function () {
        return this.getExistingColumns().length === 0;
    };
    BaseDropZonePanel.prototype.addEmptyMessageToGui = function () {
        if (!this.isExistingColumnsEmpty() || this.isPotentialDndColumns()) {
            return;
        }
        var eMessage = document.createElement('span');
        eMessage.innerHTML = this.params.emptyMessage;
        this.addElementClasses(eMessage, 'empty-message');
        this.eColumnDropList.appendChild(eMessage);
    };
    BaseDropZonePanel.prototype.addArrow = function (eParent) {
        // only add the arrows if the layout is horizontal
        if (this.horizontal) {
            // for RTL it's a left arrow, otherwise it's a right arrow
            var enableRtl = this.beans.gridOptionsWrapper.isEnableRtl();
            var icon = _.createIconNoSpan(enableRtl ? 'smallLeft' : 'smallRight', this.beans.gridOptionsWrapper);
            this.addElementClasses(icon, 'cell-separator');
            eParent.appendChild(icon);
        }
    };
    BaseDropZonePanel.STATE_NOT_DRAGGING = 'notDragging';
    BaseDropZonePanel.STATE_NEW_COLUMNS_IN = 'newColumnsIn';
    BaseDropZonePanel.STATE_REARRANGE_COLUMNS = 'rearrangeColumns';
    return BaseDropZonePanel;
}(Component));

var __extends$b = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate$q = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var RowGroupDropZonePanel = /** @class */ (function (_super) {
    __extends$b(RowGroupDropZonePanel, _super);
    function RowGroupDropZonePanel(horizontal) {
        return _super.call(this, horizontal, false) || this;
    }
    RowGroupDropZonePanel.prototype.passBeansUp = function () {
        _super.prototype.setBeans.call(this, {
            gridOptionsWrapper: this.gridOptionsWrapper,
            eventService: this.eventService,
            context: this.getContext(),
            loggerFactory: this.loggerFactory,
            dragAndDropService: this.dragAndDropService
        });
        var localeTextFunc = this.gridOptionsWrapper.getLocaleTextFunc();
        var emptyMessage = localeTextFunc('rowGroupColumnsEmptyMessage', 'Drag here to set row groups');
        var title = localeTextFunc('groups', 'Row Groups');
        _super.prototype.init.call(this, {
            dragAndDropIcon: DragAndDropService.ICON_GROUP,
            icon: _.createIconNoSpan('rowGroupPanel', this.gridOptionsWrapper, null),
            emptyMessage: emptyMessage,
            title: title
        });
        this.addDestroyableEventListener(this.eventService, Events.EVENT_COLUMN_ROW_GROUP_CHANGED, this.refreshGui.bind(this));
    };
    RowGroupDropZonePanel.prototype.isColumnDroppable = function (column) {
        // we never allow grouping of secondary columns
        if (this.gridOptionsWrapper.isFunctionsReadOnly() || !column.isPrimary()) {
            return false;
        }
        return column.isAllowRowGroup() && !column.isRowGroupActive();
    };
    RowGroupDropZonePanel.prototype.updateColumns = function (columns) {
        if (this.gridOptionsWrapper.isFunctionsPassive()) {
            var event_1 = {
                type: Events.EVENT_COLUMN_ROW_GROUP_CHANGE_REQUEST,
                columns: columns,
                api: this.gridApi,
                columnApi: this.columnApi
            };
            this.eventService.dispatchEvent(event_1);
        }
        else {
            this.columnController.setRowGroupColumns(columns, "toolPanelUi");
        }
    };
    RowGroupDropZonePanel.prototype.getIconName = function () {
        return this.isPotentialDndColumns() ? DragAndDropService.ICON_GROUP : DragAndDropService.ICON_NOT_ALLOWED;
    };
    RowGroupDropZonePanel.prototype.getExistingColumns = function () {
        return this.columnController.getRowGroupColumns();
    };
    __decorate$q([
        Autowired('columnController')
    ], RowGroupDropZonePanel.prototype, "columnController", void 0);
    __decorate$q([
        Autowired('eventService')
    ], RowGroupDropZonePanel.prototype, "eventService", void 0);
    __decorate$q([
        Autowired('gridOptionsWrapper')
    ], RowGroupDropZonePanel.prototype, "gridOptionsWrapper", void 0);
    __decorate$q([
        Autowired('loggerFactory')
    ], RowGroupDropZonePanel.prototype, "loggerFactory", void 0);
    __decorate$q([
        Autowired('dragAndDropService')
    ], RowGroupDropZonePanel.prototype, "dragAndDropService", void 0);
    __decorate$q([
        Autowired('columnApi')
    ], RowGroupDropZonePanel.prototype, "columnApi", void 0);
    __decorate$q([
        Autowired('gridApi')
    ], RowGroupDropZonePanel.prototype, "gridApi", void 0);
    __decorate$q([
        PostConstruct
    ], RowGroupDropZonePanel.prototype, "passBeansUp", null);
    return RowGroupDropZonePanel;
}(BaseDropZonePanel));

var __extends$c = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate$r = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var PivotDropZonePanel = /** @class */ (function (_super) {
    __extends$c(PivotDropZonePanel, _super);
    function PivotDropZonePanel(horizontal) {
        return _super.call(this, horizontal, false) || this;
    }
    PivotDropZonePanel.prototype.passBeansUp = function () {
        _super.prototype.setBeans.call(this, {
            gridOptionsWrapper: this.gridOptionsWrapper,
            eventService: this.eventService,
            context: this.getContext(),
            loggerFactory: this.loggerFactory,
            dragAndDropService: this.dragAndDropService
        });
        var localeTextFunc = this.gridOptionsWrapper.getLocaleTextFunc();
        var emptyMessage = localeTextFunc('pivotColumnsEmptyMessage', 'Drag here to set column labels');
        var title = localeTextFunc('pivots', 'Column Labels');
        _super.prototype.init.call(this, {
            dragAndDropIcon: DragAndDropService.ICON_GROUP,
            icon: _.createIconNoSpan('pivotPanel', this.gridOptionsWrapper, null),
            emptyMessage: emptyMessage,
            title: title
        });
        this.addDestroyableEventListener(this.eventService, Events.EVENT_COLUMN_EVERYTHING_CHANGED, this.refresh.bind(this));
        this.addDestroyableEventListener(this.eventService, Events.EVENT_COLUMN_PIVOT_CHANGED, this.refresh.bind(this));
        this.addDestroyableEventListener(this.eventService, Events.EVENT_COLUMN_PIVOT_MODE_CHANGED, this.checkVisibility.bind(this));
        this.refresh();
    };
    PivotDropZonePanel.prototype.refresh = function () {
        this.checkVisibility();
        this.refreshGui();
    };
    PivotDropZonePanel.prototype.checkVisibility = function () {
        var pivotMode = this.columnController.isPivotMode();
        if (this.isHorizontal()) {
            // what we do for horizontal (ie the pivot panel at the top) depends
            // on the user property as well as pivotMode.
            switch (this.gridOptionsWrapper.getPivotPanelShow()) {
                case 'always':
                    this.setDisplayed(pivotMode);
                    break;
                case 'onlyWhenPivoting':
                    var pivotActive = this.columnController.isPivotActive();
                    this.setDisplayed(pivotMode && pivotActive);
                    break;
                default:
                    // never show it
                    this.setDisplayed(false);
                    break;
            }
        }
        else {
            // in toolPanel, the pivot panel is always shown when pivot mode is on
            this.setDisplayed(pivotMode);
        }
    };
    PivotDropZonePanel.prototype.isColumnDroppable = function (column) {
        // we never allow grouping of secondary columns
        if (this.gridOptionsWrapper.isFunctionsReadOnly() || !column.isPrimary()) {
            return false;
        }
        return column.isAllowPivot() && !column.isPivotActive();
    };
    PivotDropZonePanel.prototype.updateColumns = function (columns) {
        if (this.gridOptionsWrapper.isFunctionsPassive()) {
            var event_1 = {
                type: Events.EVENT_COLUMN_PIVOT_CHANGE_REQUEST,
                columns: columns,
                api: this.gridApi,
                columnApi: this.columnApi
            };
            this.eventService.dispatchEvent(event_1);
        }
        else {
            this.columnController.setPivotColumns(columns, "toolPanelUi");
        }
    };
    PivotDropZonePanel.prototype.getIconName = function () {
        return this.isPotentialDndColumns() ? DragAndDropService.ICON_PIVOT : DragAndDropService.ICON_NOT_ALLOWED;
    };
    PivotDropZonePanel.prototype.getExistingColumns = function () {
        return this.columnController.getPivotColumns();
    };
    __decorate$r([
        Autowired('columnController')
    ], PivotDropZonePanel.prototype, "columnController", void 0);
    __decorate$r([
        Autowired('eventService')
    ], PivotDropZonePanel.prototype, "eventService", void 0);
    __decorate$r([
        Autowired('gridOptionsWrapper')
    ], PivotDropZonePanel.prototype, "gridOptionsWrapper", void 0);
    __decorate$r([
        Autowired('loggerFactory')
    ], PivotDropZonePanel.prototype, "loggerFactory", void 0);
    __decorate$r([
        Autowired('dragAndDropService')
    ], PivotDropZonePanel.prototype, "dragAndDropService", void 0);
    __decorate$r([
        Autowired('columnApi')
    ], PivotDropZonePanel.prototype, "columnApi", void 0);
    __decorate$r([
        Autowired('gridApi')
    ], PivotDropZonePanel.prototype, "gridApi", void 0);
    __decorate$r([
        PostConstruct
    ], PivotDropZonePanel.prototype, "passBeansUp", null);
    return PivotDropZonePanel;
}(BaseDropZonePanel));

var __extends$d = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate$s = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var GridHeaderDropZones = /** @class */ (function (_super) {
    __extends$d(GridHeaderDropZones, _super);
    function GridHeaderDropZones() {
        return _super.call(this) || this;
    }
    GridHeaderDropZones.prototype.postConstruct = function () {
        this.setGui(this.createNorthPanel());
        this.eventService.addEventListener(Events.EVENT_COLUMN_ROW_GROUP_CHANGED, this.onRowGroupChanged.bind(this));
        this.eventService.addEventListener(Events.EVENT_COLUMN_EVERYTHING_CHANGED, this.onRowGroupChanged.bind(this));
        this.onRowGroupChanged();
    };
    GridHeaderDropZones.prototype.createNorthPanel = function () {
        var _this = this;
        var topPanelGui = document.createElement('div');
        var dropPanelVisibleListener = this.onDropPanelVisible.bind(this);
        _.addCssClass(topPanelGui, 'ag-column-drop-wrapper');
        this.rowGroupComp = new RowGroupDropZonePanel(true);
        this.getContext().wireBean(this.rowGroupComp);
        this.addDestroyFunc(function () { return _this.rowGroupComp.destroy(); });
        this.pivotComp = new PivotDropZonePanel(true);
        this.getContext().wireBean(this.pivotComp);
        this.addDestroyFunc(function () { return _this.pivotComp.destroy(); });
        topPanelGui.appendChild(this.rowGroupComp.getGui());
        topPanelGui.appendChild(this.pivotComp.getGui());
        this.rowGroupComp.addEventListener(Component.EVENT_DISPLAYED_CHANGED, dropPanelVisibleListener);
        this.pivotComp.addEventListener(Component.EVENT_DISPLAYED_CHANGED, dropPanelVisibleListener);
        this.addDestroyFunc(function () {
            _this.rowGroupComp.removeEventListener(Component.EVENT_DISPLAYED_CHANGED, dropPanelVisibleListener);
            _this.pivotComp.removeEventListener(Component.EVENT_DISPLAYED_CHANGED, dropPanelVisibleListener);
        });
        this.onDropPanelVisible();
        return topPanelGui;
    };
    GridHeaderDropZones.prototype.onDropPanelVisible = function () {
        var bothDisplayed = this.rowGroupComp.isDisplayed() && this.pivotComp.isDisplayed();
        this.rowGroupComp.addOrRemoveCssClass('ag-column-drop-horizontal-half-width', bothDisplayed);
        this.pivotComp.addOrRemoveCssClass('ag-column-drop-horizontal-half-width', bothDisplayed);
    };
    GridHeaderDropZones.prototype.onRowGroupChanged = function () {
        if (!this.rowGroupComp) {
            return;
        }
        var rowGroupPanelShow = this.gridOptionsWrapper.getRowGroupPanelShow();
        if (rowGroupPanelShow === Constants.ALWAYS) {
            this.rowGroupComp.setDisplayed(true);
        }
        else if (rowGroupPanelShow === Constants.ONLY_WHEN_GROUPING) {
            var grouping = !this.columnController.isRowGroupEmpty();
            this.rowGroupComp.setDisplayed(grouping);
        }
        else {
            this.rowGroupComp.setDisplayed(false);
        }
    };
    __decorate$s([
        Autowired('gridOptionsWrapper')
    ], GridHeaderDropZones.prototype, "gridOptionsWrapper", void 0);
    __decorate$s([
        Autowired('columnController')
    ], GridHeaderDropZones.prototype, "columnController", void 0);
    __decorate$s([
        Autowired('eventService')
    ], GridHeaderDropZones.prototype, "eventService", void 0);
    __decorate$s([
        PostConstruct
    ], GridHeaderDropZones.prototype, "postConstruct", null);
    return GridHeaderDropZones;
}(Component));

var RowGroupingModule = {
    moduleName: ModuleNames.RowGroupingModule,
    beans: [AggregationStage, GroupStage, PivotColDefService, PivotStage, AggFuncService],
    agStackComponents: [
        { componentName: 'AgGridHeaderDropZones', componentClass: GridHeaderDropZones }
    ],
    dependantModules: [
    //EnterpriseCoreModule
    ]
};

var __extends$e = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate$t = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var ValuesDropZonePanel = /** @class */ (function (_super) {
    __extends$e(ValuesDropZonePanel, _super);
    function ValuesDropZonePanel(horizontal) {
        return _super.call(this, horizontal, true) || this;
    }
    ValuesDropZonePanel.prototype.passBeansUp = function () {
        _super.prototype.setBeans.call(this, {
            gridOptionsWrapper: this.gridOptionsWrapper,
            eventService: this.eventService,
            context: this.getContext(),
            loggerFactory: this.loggerFactory,
            dragAndDropService: this.dragAndDropService
        });
        var localeTextFunc = this.gridOptionsWrapper.getLocaleTextFunc();
        var emptyMessage = localeTextFunc('valueColumnsEmptyMessage', 'Drag here to aggregate');
        var title = localeTextFunc('values', 'Values');
        _super.prototype.init.call(this, {
            dragAndDropIcon: DragAndDropService.ICON_AGGREGATE,
            icon: _.createIconNoSpan('valuePanel', this.gridOptionsWrapper, null),
            emptyMessage: emptyMessage,
            title: title
        });
        this.addDestroyableEventListener(this.eventService, Events.EVENT_COLUMN_VALUE_CHANGED, this.refreshGui.bind(this));
    };
    ValuesDropZonePanel.prototype.getIconName = function () {
        return this.isPotentialDndColumns() ? DragAndDropService.ICON_AGGREGATE : DragAndDropService.ICON_NOT_ALLOWED;
    };
    ValuesDropZonePanel.prototype.isColumnDroppable = function (column) {
        // we never allow grouping of secondary columns
        if (this.gridOptionsWrapper.isFunctionsReadOnly() || !column.isPrimary()) {
            return false;
        }
        return column.isAllowValue() && !column.isValueActive();
    };
    ValuesDropZonePanel.prototype.updateColumns = function (columns) {
        if (this.gridOptionsWrapper.isFunctionsPassive()) {
            var event_1 = {
                type: Events.EVENT_COLUMN_VALUE_CHANGE_REQUEST,
                columns: columns,
                api: this.gridApi,
                columnApi: this.columnApi
            };
            this.eventService.dispatchEvent(event_1);
        }
        else {
            this.columnController.setValueColumns(columns, "toolPanelUi");
        }
    };
    ValuesDropZonePanel.prototype.getExistingColumns = function () {
        return this.columnController.getValueColumns();
    };
    __decorate$t([
        Autowired('columnController')
    ], ValuesDropZonePanel.prototype, "columnController", void 0);
    __decorate$t([
        Autowired('eventService')
    ], ValuesDropZonePanel.prototype, "eventService", void 0);
    __decorate$t([
        Autowired('gridOptionsWrapper')
    ], ValuesDropZonePanel.prototype, "gridOptionsWrapper", void 0);
    __decorate$t([
        Autowired('loggerFactory')
    ], ValuesDropZonePanel.prototype, "loggerFactory", void 0);
    __decorate$t([
        Autowired('dragAndDropService')
    ], ValuesDropZonePanel.prototype, "dragAndDropService", void 0);
    __decorate$t([
        Autowired('columnApi')
    ], ValuesDropZonePanel.prototype, "columnApi", void 0);
    __decorate$t([
        Autowired('gridApi')
    ], ValuesDropZonePanel.prototype, "gridApi", void 0);
    __decorate$t([
        PostConstruct
    ], ValuesDropZonePanel.prototype, "passBeansUp", null);
    return ValuesDropZonePanel;
}(BaseDropZonePanel));

var __extends$f = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate$u = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var FunctionsReadOnlyModePanel = /** @class */ (function (_super) {
    __extends$f(FunctionsReadOnlyModePanel, _super);
    function FunctionsReadOnlyModePanel() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    FunctionsReadOnlyModePanel.prototype.createTemplate = function () {
        return "<div class=\"ag-pivot-mode-panel\">\n                <ag-toggle-button ref=\"cbFunctionMode\" class=\"ag-pivot-mode-select\"></ag-checkbox>\n            </div>";
    };
    FunctionsReadOnlyModePanel.prototype.init = function () {
        this.setTemplate(this.createTemplate());
        this.gridOptionsWrapper.getApi();
        this.cbFunctionMode.setValue(this.gridOptionsWrapper.isFunctionsReadOnly());
        var localeTextFunc = this.gridOptionsWrapper.getLocaleTextFunc();
        this.cbFunctionMode.setLabel(localeTextFunc('functionReadOnlyMode', 'FunctionsReadOnly Mode'));
        this.addDestroyableEventListener(this.cbFunctionMode, AgCheckbox.EVENT_CHANGED, this.onBtFunctionMode.bind(this));
        this.addDestroyableEventListener(this.eventService, Events.EVENT_COLUMN_EVERYTHING_CHANGED, this.onFunctionModeChanged.bind(this));
        // this.addDestroyableEventListener(this.eventService, Events.EVENT_COLUMN_PIVOT_MODE_CHANGED, this.onPivotModeChanged.bind(this));
    };
    FunctionsReadOnlyModePanel.prototype.onBtFunctionMode = function () {
        var newValue = this.cbFunctionMode.getValue();
        if (newValue !== this.gridOptionsWrapper.isFunctionsReadOnly()) {
            this.gridOptionsWrapper.getApi().setFunctionsReadOnly(newValue);
        }
    };
    FunctionsReadOnlyModePanel.prototype.onFunctionModeChanged = function () {
        var functionModeActive = this.gridOptionsWrapper.isFunctionsReadOnly();
        this.cbFunctionMode.setValue(functionModeActive);
    };
    __decorate$u([
        Autowired('columnController')
    ], FunctionsReadOnlyModePanel.prototype, "columnController", void 0);
    __decorate$u([
        Autowired('eventService')
    ], FunctionsReadOnlyModePanel.prototype, "eventService", void 0);
    __decorate$u([
        Autowired('gridOptionsWrapper')
    ], FunctionsReadOnlyModePanel.prototype, "gridOptionsWrapper", void 0);
    __decorate$u([
        RefSelector('cbFunctionMode')
    ], FunctionsReadOnlyModePanel.prototype, "cbFunctionMode", void 0);
    __decorate$u([
        PreConstruct
    ], FunctionsReadOnlyModePanel.prototype, "init", null);
    return FunctionsReadOnlyModePanel;
}(Component));

var __extends$g = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate$v = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var ColumnToolPanel = /** @class */ (function (_super) {
    __extends$g(ColumnToolPanel, _super);
    function ColumnToolPanel() {
        var _this = _super.call(this, ColumnToolPanel.TEMPLATE) || this;
        _this.initialised = false;
        _this.childDestroyFuncs = [];
        return _this;
    }
    // lazy initialise the panel
    ColumnToolPanel.prototype.setVisible = function (visible) {
        _super.prototype.setDisplayed.call(this, visible);
        if (visible && !this.initialised) {
            this.init(this.params);
        }
    };
    ColumnToolPanel.prototype.init = function (params) {
        var defaultParams = {
            suppressSideButtons: false,
            suppressColumnSelectAll: false,
            suppressColumnFilter: false,
            suppressColumnExpandAll: false,
            contractColumnSelection: false,
            suppressPivotMode: false,
            suppressRowGroups: false,
            suppressValues: false,
            suppressPivots: false,
            suppressSyncLayoutWithGrid: false,
            api: this.gridApi
        };
        _.mergeDeep(defaultParams, params);
        this.params = defaultParams;
        this.functionModelPanel = new FunctionsReadOnlyModePanel();
        this.addComponent(this.functionModelPanel);
        if (this.isRowGroupingModuleLoaded() && !this.params.suppressPivotMode) {
            this.pivotModePanel = new PivotModePanel();
            this.addComponent(this.pivotModePanel);
        }
        this.primaryColsPanel = this.getContext().createComponent('AG-PRIMARY-COLS');
        this.primaryColsPanel.init(true, this.params);
        _.addCssClass(this.primaryColsPanel.getGui(), 'ag-column-panel-column-select');
        this.addComponent(this.primaryColsPanel);
        if (this.isRowGroupingModuleLoaded()) {
            if (!this.params.suppressRowGroups) {
                this.rowGroupDropZonePanel = new RowGroupDropZonePanel(false);
                this.addComponent(this.rowGroupDropZonePanel);
            }
            if (!this.params.suppressValues) {
                this.valuesDropZonePanel = new ValuesDropZonePanel(false);
                this.addComponent(this.valuesDropZonePanel);
            }
            if (!this.params.suppressPivots) {
                this.pivotDropZonePanel = new PivotDropZonePanel(false);
                this.addComponent(this.pivotDropZonePanel);
            }
            this.setLastVisible();
            this.addDestroyableEventListener(this.eventService, Events.EVENT_COLUMN_PIVOT_MODE_CHANGED, this.setLastVisible.bind(this));
        }
        this.initialised = true;
    };
    ColumnToolPanel.prototype.setPivotModeSectionVisible = function (visible) {
        if (!this.isRowGroupingModuleLoaded()) {
            return;
        }
        if (this.pivotModePanel) {
            this.pivotModePanel.setDisplayed(visible);
        }
        else if (visible) {
            this.pivotModePanel = new PivotModePanel();
            this.getContext().wireBean(this.pivotModePanel);
            // ensure pivot mode panel is positioned at the top of the columns tool panel
            this.getGui().insertBefore(this.pivotModePanel.getGui(), this.getGui().firstChild);
            this.childDestroyFuncs.push(this.pivotModePanel.destroy.bind(this.pivotModePanel));
        }
        this.setLastVisible();
    };
    ColumnToolPanel.prototype.setRowGroupsSectionVisible = function (visible) {
        if (!this.isRowGroupingModuleLoaded()) {
            return;
        }
        if (this.rowGroupDropZonePanel) {
            this.rowGroupDropZonePanel.setDisplayed(visible);
        }
        else if (visible) {
            this.rowGroupDropZonePanel = new RowGroupDropZonePanel(false);
            this.addComponent(new RowGroupDropZonePanel(false));
        }
        this.setLastVisible();
    };
    ColumnToolPanel.prototype.setValuesSectionVisible = function (visible) {
        if (!this.isRowGroupingModuleLoaded()) {
            return;
        }
        if (this.valuesDropZonePanel) {
            this.valuesDropZonePanel.setDisplayed(visible);
        }
        else if (visible) {
            this.valuesDropZonePanel = new ValuesDropZonePanel(false);
            this.addComponent(this.valuesDropZonePanel);
        }
        this.setLastVisible();
    };
    ColumnToolPanel.prototype.setPivotSectionVisible = function (visible) {
        if (!this.isRowGroupingModuleLoaded()) {
            return;
        }
        if (this.pivotDropZonePanel) {
            this.pivotDropZonePanel.setDisplayed(visible);
        }
        else if (visible) {
            this.pivotDropZonePanel = new PivotDropZonePanel(false);
            this.addComponent(this.pivotDropZonePanel);
            this.pivotDropZonePanel.setDisplayed(visible);
        }
        this.setLastVisible();
    };
    ColumnToolPanel.prototype.setLastVisible = function () {
        var eGui = this.getGui();
        var columnDrops = Array.prototype.slice.call(eGui.querySelectorAll('.ag-column-drop'));
        columnDrops.forEach(function (columnDrop) { return _.removeCssClass(columnDrop, 'ag-last-column-drop'); });
        var lastVisible = _.last(eGui.querySelectorAll('.ag-column-drop:not(.ag-hidden)'));
        if (lastVisible) {
            _.addCssClass(lastVisible, 'ag-last-column-drop');
        }
    };
    ColumnToolPanel.prototype.isRowGroupingModuleLoaded = function () {
        return ModuleRegistry.assertRegistered(ModuleNames.RowGroupingModule, 'Row Grouping');
    };
    ColumnToolPanel.prototype.expandColumnGroups = function (groupIds) {
        this.primaryColsPanel.expandGroups(groupIds);
    };
    ColumnToolPanel.prototype.collapseColumnGroups = function (groupIds) {
        this.primaryColsPanel.collapseGroups(groupIds);
    };
    ColumnToolPanel.prototype.setColumnLayout = function (colDefs) {
        this.primaryColsPanel.setColumnLayout(colDefs);
    };
    ColumnToolPanel.prototype.syncLayoutWithGrid = function () {
        this.primaryColsPanel.syncLayoutWithGrid();
    };
    ColumnToolPanel.prototype.addComponent = function (component) {
        this.getContext().wireBean(component);
        this.getGui().appendChild(component.getGui());
        this.childDestroyFuncs.push(component.destroy.bind(component));
    };
    ColumnToolPanel.prototype.destroyChildren = function () {
        this.childDestroyFuncs.forEach(function (func) { return func(); });
        this.childDestroyFuncs.length = 0;
        _.clearElement(this.getGui());
    };
    ColumnToolPanel.prototype.refresh = function () {
        this.destroyChildren();
        this.init(this.params);
    };
    ColumnToolPanel.prototype.destroy = function () {
        this.destroyChildren();
        _super.prototype.destroy.call(this);
    };
    ColumnToolPanel.TEMPLATE = "<div class=\"ag-column-panel\"></div>";
    __decorate$v([
        Autowired("gridOptionsWrapper")
    ], ColumnToolPanel.prototype, "gridOptionsWrapper", void 0);
    __decorate$v([
        Autowired("gridApi")
    ], ColumnToolPanel.prototype, "gridApi", void 0);
    __decorate$v([
        Autowired("eventService")
    ], ColumnToolPanel.prototype, "eventService", void 0);
    return ColumnToolPanel;
}(Component));

var __extends$h = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate$w = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var PrimaryColsPanel = /** @class */ (function (_super) {
    __extends$h(PrimaryColsPanel, _super);
    function PrimaryColsPanel() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    // we allow dragging in the toolPanel, but not when this component appears in the column menu
    PrimaryColsPanel.prototype.init = function (allowDragging, params) {
        this.setTemplate(PrimaryColsPanel.TEMPLATE);
        this.allowDragging = allowDragging;
        this.params = params;
        this.primaryColsHeaderPanel.init(this.params);
        var hideFilter = this.params.suppressColumnFilter;
        var hideSelect = this.params.suppressColumnSelectAll;
        var hideExpand = this.params.suppressColumnExpandAll;
        if (hideExpand && hideFilter && hideSelect) {
            this.primaryColsHeaderPanel.setDisplayed(false);
        }
        this.addDestroyableEventListener(this.primaryColsListPanel, 'groupExpanded', this.onGroupExpanded.bind(this));
        this.addDestroyableEventListener(this.primaryColsListPanel, 'selectionChanged', this.onSelectionChange.bind(this));
        this.primaryColsListPanel.init(this.params, this.allowDragging);
        this.addDestroyableEventListener(this.primaryColsHeaderPanel, 'expandAll', this.onExpandAll.bind(this));
        this.addDestroyableEventListener(this.primaryColsHeaderPanel, 'collapseAll', this.onCollapseAll.bind(this));
        this.addDestroyableEventListener(this.primaryColsHeaderPanel, 'selectAll', this.onSelectAll.bind(this));
        this.addDestroyableEventListener(this.primaryColsHeaderPanel, 'unselectAll', this.onUnselectAll.bind(this));
        this.addDestroyableEventListener(this.primaryColsHeaderPanel, 'filterChanged', this.onFilterChanged.bind(this));
    };
    PrimaryColsPanel.prototype.onExpandAll = function () {
        this.primaryColsListPanel.doSetExpandedAll(true);
    };
    PrimaryColsPanel.prototype.onCollapseAll = function () {
        this.primaryColsListPanel.doSetExpandedAll(false);
    };
    PrimaryColsPanel.prototype.expandGroups = function (groupIds) {
        this.primaryColsListPanel.setGroupsExpanded(true, groupIds);
    };
    PrimaryColsPanel.prototype.collapseGroups = function (groupIds) {
        this.primaryColsListPanel.setGroupsExpanded(false, groupIds);
    };
    PrimaryColsPanel.prototype.setColumnLayout = function (colDefs) {
        this.primaryColsListPanel.setColumnLayout(colDefs);
    };
    PrimaryColsPanel.prototype.onFilterChanged = function (event) {
        this.primaryColsListPanel.setFilterText(event.filterText);
    };
    PrimaryColsPanel.prototype.syncLayoutWithGrid = function () {
        this.primaryColsListPanel.syncColumnLayout();
    };
    PrimaryColsPanel.prototype.onSelectAll = function () {
        this.primaryColsListPanel.doSetSelectedAll(true);
    };
    PrimaryColsPanel.prototype.onUnselectAll = function () {
        this.primaryColsListPanel.doSetSelectedAll(false);
    };
    PrimaryColsPanel.prototype.onGroupExpanded = function (event) {
        this.primaryColsHeaderPanel.setExpandState(event.state);
    };
    PrimaryColsPanel.prototype.onSelectionChange = function (event) {
        this.primaryColsHeaderPanel.setSelectionState(event.state);
    };
    PrimaryColsPanel.TEMPLATE = "<div class=\"ag-column-select\">\n            <ag-primary-cols-header ref=\"primaryColsHeaderPanel\"></ag-primary-cols-header>\n            <ag-primary-cols-list ref=\"primaryColsListPanel\"></ag-primary-cols-list>\n        </div>";
    __decorate$w([
        RefSelector('primaryColsHeaderPanel')
    ], PrimaryColsPanel.prototype, "primaryColsHeaderPanel", void 0);
    __decorate$w([
        RefSelector('primaryColsListPanel')
    ], PrimaryColsPanel.prototype, "primaryColsListPanel", void 0);
    return PrimaryColsPanel;
}(Component));

var __extends$i = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate$x = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var HorizontalResizeComp = /** @class */ (function (_super) {
    __extends$i(HorizontalResizeComp, _super);
    function HorizontalResizeComp() {
        return _super.call(this, "<div class=\"ag-tool-panel-horizontal-resize\"></div>") || this;
    }
    HorizontalResizeComp.prototype.setElementToResize = function (elementToResize) {
        this.elementToResize = elementToResize;
    };
    HorizontalResizeComp.prototype.postConstruct = function () {
        var finishedWithResizeFunc = this.horizontalResizeService.addResizeBar({
            eResizeBar: this.getGui(),
            dragStartPixels: 1,
            onResizeStart: this.onResizeStart.bind(this),
            onResizing: this.onResizing.bind(this),
            onResizeEnd: this.onResizing.bind(this)
        });
        this.addDestroyFunc(finishedWithResizeFunc);
        this.setInverted(this.gridOptionsWrapper.isEnableRtl());
    };
    HorizontalResizeComp.prototype.onResizeStart = function () {
        this.startingWidth = this.elementToResize.offsetWidth;
    };
    HorizontalResizeComp.prototype.onResizing = function (delta) {
        var direction = this.inverted ? -1 : 1;
        var newWidth = Math.max(100, Math.floor(this.startingWidth - (delta * direction)));
        this.elementToResize.style.width = newWidth + "px";
    };
    HorizontalResizeComp.prototype.setInverted = function (inverted) {
        this.inverted = inverted;
    };
    __decorate$x([
        Autowired('horizontalResizeService')
    ], HorizontalResizeComp.prototype, "horizontalResizeService", void 0);
    __decorate$x([
        Autowired('gridOptionsWrapper')
    ], HorizontalResizeComp.prototype, "gridOptionsWrapper", void 0);
    __decorate$x([
        Autowired('eventService')
    ], HorizontalResizeComp.prototype, "eventService", void 0);
    __decorate$x([
        PostConstruct
    ], HorizontalResizeComp.prototype, "postConstruct", null);
    return HorizontalResizeComp;
}(Component));

var __extends$j = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate$y = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var SideBarButtonsComp = /** @class */ (function (_super) {
    __extends$j(SideBarButtonsComp, _super);
    function SideBarButtonsComp() {
        var _this = _super.call(this, SideBarButtonsComp.TEMPLATE) || this;
        _this.buttonComps = [];
        return _this;
    }
    SideBarButtonsComp.prototype.setToolPanelDefs = function (toolPanelDefs) {
        toolPanelDefs.forEach(this.addButtonComp.bind(this));
    };
    SideBarButtonsComp.prototype.setActiveButton = function (id) {
        this.buttonComps.forEach(function (comp) {
            comp.setSelected(id === comp.getToolPanelId());
        });
    };
    SideBarButtonsComp.prototype.addButtonComp = function (def) {
        var _this = this;
        var buttonComp = new SideBarButtonComp(def);
        this.getContext().wireBean(buttonComp);
        this.buttonComps.push(buttonComp);
        this.getGui().appendChild(buttonComp.getGui());
        buttonComp.addEventListener(SideBarButtonComp.EVENT_TOGGLE_BUTTON_CLICKED, function () {
            _this.dispatchEvent({
                type: SideBarButtonsComp.EVENT_SIDE_BAR_BUTTON_CLICKED,
                toolPanelId: def.id
            });
        });
    };
    SideBarButtonsComp.prototype.clearButtons = function () {
        if (this.buttonComps) {
            this.buttonComps.forEach(function (comp) { return comp.destroy(); });
        }
        _.clearElement(this.getGui());
        this.buttonComps.length = 0;
    };
    SideBarButtonsComp.prototype.destroy = function () {
        this.clearButtons();
        _super.prototype.destroy.call(this);
    };
    SideBarButtonsComp.EVENT_SIDE_BAR_BUTTON_CLICKED = 'sideBarButtonClicked';
    SideBarButtonsComp.TEMPLATE = "<div class=\"ag-side-buttons\"></div>";
    __decorate$y([
        Autowired("gridOptionsWrapper")
    ], SideBarButtonsComp.prototype, "gridOptionsWrapper", void 0);
    return SideBarButtonsComp;
}(Component));
var SideBarButtonComp = /** @class */ (function (_super) {
    __extends$j(SideBarButtonComp, _super);
    function SideBarButtonComp(toolPanelDef) {
        var _this = _super.call(this) || this;
        _this.toolPanelDef = toolPanelDef;
        return _this;
    }
    SideBarButtonComp.prototype.getToolPanelId = function () {
        return this.toolPanelDef.id;
    };
    SideBarButtonComp.prototype.postConstruct = function () {
        var template = this.createTemplate();
        this.setTemplate(template);
        this.eIconWrapper.insertAdjacentElement('afterbegin', _.createIconNoSpan(this.toolPanelDef.iconKey, this.gridOptionsWrapper));
        this.addDestroyableEventListener(this.eToggleButton, 'click', this.onButtonPressed.bind(this));
    };
    SideBarButtonComp.prototype.createTemplate = function () {
        var translate = this.gridOptionsWrapper.getLocaleTextFunc();
        var def = this.toolPanelDef;
        var label = translate(def.labelKey, def.labelDefault);
        var res = "<div class=\"ag-side-button\">\n                <button type=\"button\" ref=\"eToggleButton\" class=\"ag-side-button-button\">\n                    <div ref=\"eIconWrapper\" class=\"ag-side-button-icon-wrapper\"></div>\n                    <span class=\"ag-side-button-label\">" + label + "</span>\n                </button>\n            </div>";
        return res;
    };
    SideBarButtonComp.prototype.onButtonPressed = function () {
        this.dispatchEvent({ type: SideBarButtonComp.EVENT_TOGGLE_BUTTON_CLICKED });
    };
    SideBarButtonComp.prototype.setSelected = function (selected) {
        this.addOrRemoveCssClass('ag-selected', selected);
    };
    SideBarButtonComp.EVENT_TOGGLE_BUTTON_CLICKED = 'toggleButtonClicked';
    __decorate$y([
        Autowired("gridOptionsWrapper")
    ], SideBarButtonComp.prototype, "gridOptionsWrapper", void 0);
    __decorate$y([
        RefSelector('eToggleButton')
    ], SideBarButtonComp.prototype, "eToggleButton", void 0);
    __decorate$y([
        RefSelector('eIconWrapper')
    ], SideBarButtonComp.prototype, "eIconWrapper", void 0);
    __decorate$y([
        PostConstruct
    ], SideBarButtonComp.prototype, "postConstruct", null);
    return SideBarButtonComp;
}(Component));

var __extends$k = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate$z = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var ToolPanelWrapper = /** @class */ (function (_super) {
    __extends$k(ToolPanelWrapper, _super);
    function ToolPanelWrapper() {
        return _super.call(this, ToolPanelWrapper.TEMPLATE) || this;
    }
    ToolPanelWrapper.prototype.getToolPanelId = function () {
        return this.toolPanelId;
    };
    ToolPanelWrapper.prototype.setToolPanelDef = function (toolPanelDef) {
        this.toolPanelId = toolPanelDef.id;
        var params = {
            api: this.gridOptionsWrapper.getApi()
        };
        var componentPromise = this.userComponentFactory.newToolPanelComponent(toolPanelDef, params);
        if (componentPromise == null) {
            console.warn("ag-grid: error processing tool panel component " + toolPanelDef.id + ". You need to specify either 'toolPanel' or 'toolPanelFramework'");
            return;
        }
        componentPromise.then(this.setToolPanelComponent.bind(this));
    };
    ToolPanelWrapper.prototype.setupResize = function () {
        var resizeBar = this.resizeBar = new HorizontalResizeComp();
        this.getContext().wireBean(resizeBar);
        resizeBar.setElementToResize(this.getGui());
        this.appendChild(resizeBar);
    };
    ToolPanelWrapper.prototype.setToolPanelComponent = function (compInstance) {
        this.toolPanelCompInstance = compInstance;
        this.appendChild(compInstance);
    };
    ToolPanelWrapper.prototype.getToolPanelInstance = function () {
        return this.toolPanelCompInstance;
    };
    ToolPanelWrapper.prototype.setResizerSizerSide = function (side) {
        var isRtl = this.gridOptionsWrapper.isEnableRtl();
        var isLeft = side === 'left';
        var inverted = isRtl ? isLeft : !isLeft;
        this.resizeBar.setInverted(inverted);
    };
    ToolPanelWrapper.prototype.refresh = function () {
        this.toolPanelCompInstance.refresh();
    };
    ToolPanelWrapper.TEMPLATE = "<div class=\"ag-tool-panel-wrapper\"/>";
    __decorate$z([
        Autowired("userComponentFactory")
    ], ToolPanelWrapper.prototype, "userComponentFactory", void 0);
    __decorate$z([
        Autowired("gridOptionsWrapper")
    ], ToolPanelWrapper.prototype, "gridOptionsWrapper", void 0);
    __decorate$z([
        PostConstruct
    ], ToolPanelWrapper.prototype, "setupResize", null);
    return ToolPanelWrapper;
}(Component));

var __extends$l = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate$A = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var SideBarComp = /** @class */ (function (_super) {
    __extends$l(SideBarComp, _super);
    function SideBarComp() {
        var _this = _super.call(this, SideBarComp.TEMPLATE) || this;
        _this.toolPanelWrappers = [];
        return _this;
    }
    SideBarComp.prototype.postConstruct = function () {
        this.sideBarButtonsComp.addEventListener(SideBarButtonsComp.EVENT_SIDE_BAR_BUTTON_CLICKED, this.onToolPanelButtonClicked.bind(this));
        this.setSideBarDef();
    };
    SideBarComp.prototype.onToolPanelButtonClicked = function (event) {
        var id = event.toolPanelId;
        var openedItem = this.openedItem();
        // if item was already open, we close it
        if (openedItem === id) {
            this.openToolPanel(undefined); // passing undefined closes
        }
        else {
            this.openToolPanel(id);
        }
    };
    SideBarComp.prototype.clearDownUi = function () {
        this.sideBarButtonsComp.clearButtons();
        this.destroyToolPanelWrappers();
    };
    SideBarComp.prototype.setSideBarDef = function () {
        // initially hide side bar
        this.setDisplayed(false);
        var sideBar = this.gridOptionsWrapper.getSideBar();
        var sideBarExists = !!sideBar && !!sideBar.toolPanels;
        if (sideBarExists) {
            var shouldDisplaySideBar = sideBarExists && !sideBar.hiddenByDefault;
            this.setDisplayed(shouldDisplaySideBar);
            var toolPanelDefs = sideBar.toolPanels;
            this.sideBarButtonsComp.setToolPanelDefs(toolPanelDefs);
            this.setupToolPanels(toolPanelDefs);
            this.setSideBarPosition(sideBar.position);
            if (!sideBar.hiddenByDefault) {
                this.openToolPanel(sideBar.defaultToolPanel);
            }
        }
    };
    SideBarComp.prototype.setSideBarPosition = function (position) {
        if (!position) {
            position = 'right';
        }
        var isLeft = position === 'left';
        var resizerSide = isLeft ? 'right' : 'left';
        var eGui = this.getGui();
        _.addOrRemoveCssClass(eGui, 'ag-side-bar-left', isLeft);
        _.addOrRemoveCssClass(eGui, 'ag-side-bar-right', !isLeft);
        this.toolPanelWrappers.forEach(function (wrapper) {
            wrapper.setResizerSizerSide(resizerSide);
        });
        return this;
    };
    SideBarComp.prototype.setupToolPanels = function (defs) {
        var _this = this;
        defs.forEach(function (def) {
            if (def.id == null) {
                console.warn("ag-grid: please review all your toolPanel components, it seems like at least one of them doesn't have an id");
                return;
            }
            // helpers, in case user doesn't have the right module loaded
            if (def.toolPanel === 'agColumnsToolPanel') {
                var moduleMissing = !ModuleRegistry.assertRegistered(ModuleNames.ColumnToolPanelModule, 'Column Tool Panel');
                if (moduleMissing) {
                    return;
                }
            }
            if (def.toolPanel === 'agFiltersToolPanel') {
                var moduleMissing = !ModuleRegistry.assertRegistered(ModuleNames.FiltersToolPanelModule, 'Filters Tool Panel');
                if (moduleMissing) {
                    return;
                }
            }
            var wrapper = new ToolPanelWrapper();
            _this.getContext().wireBean(wrapper);
            wrapper.setToolPanelDef(def);
            wrapper.setDisplayed(false);
            _this.getGui().appendChild(wrapper.getGui());
            _this.toolPanelWrappers.push(wrapper);
        });
    };
    SideBarComp.prototype.refresh = function () {
        this.toolPanelWrappers.forEach(function (wrapper) { return wrapper.refresh(); });
    };
    SideBarComp.prototype.openToolPanel = function (key) {
        var currentlyOpenedKey = this.openedItem();
        if (currentlyOpenedKey === key) {
            return;
        }
        this.toolPanelWrappers.forEach(function (wrapper) {
            var show = key === wrapper.getToolPanelId();
            wrapper.setDisplayed(show);
        });
        var newlyOpenedKey = this.openedItem();
        var openToolPanelChanged = currentlyOpenedKey !== newlyOpenedKey;
        if (openToolPanelChanged) {
            this.sideBarButtonsComp.setActiveButton(key);
            this.raiseToolPanelVisibleEvent(key);
        }
    };
    SideBarComp.prototype.getToolPanelInstance = function (key) {
        var toolPanelWrapper = this.toolPanelWrappers.filter(function (toolPanel) { return toolPanel.getToolPanelId() === key; })[0];
        if (!toolPanelWrapper) {
            console.warn("ag-grid: unable to lookup Tool Panel as invalid key supplied: " + key);
            return;
        }
        return toolPanelWrapper.getToolPanelInstance();
    };
    SideBarComp.prototype.raiseToolPanelVisibleEvent = function (key) {
        var event = {
            type: Events.EVENT_TOOL_PANEL_VISIBLE_CHANGED,
            source: key,
            api: this.gridOptionsWrapper.getApi(),
            columnApi: this.gridOptionsWrapper.getColumnApi()
        };
        this.eventService.dispatchEvent(event);
    };
    SideBarComp.prototype.close = function () {
        this.openToolPanel(undefined);
    };
    SideBarComp.prototype.isToolPanelShowing = function () {
        return !!this.openedItem();
    };
    SideBarComp.prototype.openedItem = function () {
        var activeToolPanel = null;
        this.toolPanelWrappers.forEach(function (wrapper) {
            if (wrapper.isDisplayed()) {
                activeToolPanel = wrapper.getToolPanelId();
            }
        });
        return activeToolPanel;
    };
    // get called after user sets new sideBarDef via the API
    SideBarComp.prototype.reset = function () {
        this.clearDownUi();
        this.setSideBarDef();
    };
    SideBarComp.prototype.destroyToolPanelWrappers = function () {
        this.toolPanelWrappers.forEach(function (wrapper) {
            _.removeFromParent(wrapper.getGui());
            wrapper.destroy();
        });
        this.toolPanelWrappers.length = 0;
    };
    SideBarComp.prototype.destroy = function () {
        this.destroyToolPanelWrappers();
        _super.prototype.destroy.call(this);
    };
    SideBarComp.TEMPLATE = "<div class=\"ag-side-bar ag-unselectable\">\n              <ag-side-bar-buttons ref=\"sideBarButtons\">\n          </div>";
    __decorate$A([
        Autowired("eventService")
    ], SideBarComp.prototype, "eventService", void 0);
    __decorate$A([
        Autowired("gridOptionsWrapper")
    ], SideBarComp.prototype, "gridOptionsWrapper", void 0);
    __decorate$A([
        RefSelector('sideBarButtons')
    ], SideBarComp.prototype, "sideBarButtonsComp", void 0);
    __decorate$A([
        PostConstruct
    ], SideBarComp.prototype, "postConstruct", null);
    return SideBarComp;
}(Component));

var __decorate$B = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var ToolPanelColDefService = /** @class */ (function () {
    function ToolPanelColDefService() {
        var _this = this;
        this.isColGroupDef = function (colDef) { return colDef && typeof colDef.children !== 'undefined'; };
        this.getId = function (colDef) {
            return _this.isColGroupDef(colDef) ? colDef.groupId : colDef.colId;
        };
    }
    ToolPanelColDefService.prototype.createColumnTree = function (colDefs) {
        var _this = this;
        var invalidColIds = [];
        var createDummyColGroup = function (abstractColDef, depth) {
            if (_this.isColGroupDef(abstractColDef)) {
                // creating 'dummy' group which is not associated with grid column group
                var groupDef = abstractColDef;
                var groupId = (typeof groupDef.groupId !== 'undefined') ? groupDef.groupId : groupDef.headerName;
                var group = new OriginalColumnGroup(groupDef, groupId, false, depth);
                var children_1 = [];
                groupDef.children.forEach(function (def) {
                    var child = createDummyColGroup(def, depth + 1);
                    // check column exists in case invalid colDef is supplied for primary column
                    if (child) {
                        children_1.push(child);
                    }
                });
                group.setChildren(children_1);
                return group;
            }
            else {
                var colDef = abstractColDef;
                var key = colDef.colId ? colDef.colId : colDef.field;
                var column = _this.columnController.getPrimaryColumn(key);
                if (!column) {
                    invalidColIds.push(colDef);
                }
                return column;
            }
        };
        var mappedResults = [];
        colDefs.forEach(function (colDef) {
            var result = createDummyColGroup(colDef, 0);
            if (result) {
                // only return correctly mapped colDef results
                mappedResults.push(result);
            }
        });
        if (invalidColIds.length > 0) {
            console.warn('ag-Grid: unable to find grid columns for the supplied colDef(s):', invalidColIds);
        }
        return mappedResults;
    };
    ToolPanelColDefService.prototype.syncLayoutWithGrid = function (syncLayoutCallback) {
        // extract ordered list of leaf path trees (column group hierarchy for each individual leaf column)
        var leafPathTrees = this.getLeafPathTrees();
        // merge leaf path tree taking split column groups into account
        var mergedColumnTrees = this.mergeLeafPathTrees(leafPathTrees);
        // sync layout with merged column trees
        syncLayoutCallback(mergedColumnTrees);
    };
    ToolPanelColDefService.prototype.getLeafPathTrees = function () {
        // leaf tree paths are obtained by walking up the tree starting at a column until we reach the top level group.
        var getLeafPathTree = function (node, childDef) {
            var leafPathTree;
            // build up tree in reverse order
            if (node instanceof OriginalColumnGroup) {
                if (node.isPadding()) {
                    // skip over padding groups
                    leafPathTree = childDef;
                }
                else {
                    var groupDef = _.assign({}, node.getColGroupDef());
                    // ensure group contains groupId
                    groupDef.groupId = node.getGroupId();
                    groupDef.children = [childDef];
                    leafPathTree = groupDef;
                }
            }
            else {
                var colDef = _.assign({}, node.getColDef());
                // ensure col contains colId
                colDef.colId = node.getColId();
                leafPathTree = colDef;
            }
            // walk tree
            var parent = node.getOriginalParent();
            if (parent) {
                // keep walking up the tree until we reach the root
                return getLeafPathTree(parent, leafPathTree);
            }
            else {
                // we have reached the root - exit with resulting leaf path tree
                return leafPathTree;
            }
        };
        // obtain a sorted list of all grid columns
        var allGridColumns = this.columnController.getAllGridColumns();
        // only primary columns and non row group columns should appear in the tool panel
        var allPrimaryGridColumns = allGridColumns.filter(function (column) {
            var colDef = column.getColDef();
            return column.isPrimary() && !colDef.showRowGroup;
        });
        // construct a leaf path tree for each column
        return allPrimaryGridColumns.map(function (col) { return getLeafPathTree(col, col.getColDef()); });
    };
    ToolPanelColDefService.prototype.mergeLeafPathTrees = function (leafPathTrees) {
        var _this = this;
        var matchingRootGroupIds = function (pathA, pathB) {
            var bothPathsAreGroups = _this.isColGroupDef(pathA) && _this.isColGroupDef(pathB);
            return bothPathsAreGroups && _this.getId(pathA) === _this.getId(pathB);
        };
        var mergeTrees = function (treeA, treeB) {
            if (!_this.isColGroupDef(treeB))
                return treeA;
            var mergeResult = treeA;
            var groupToMerge = treeB;
            if (groupToMerge.children && groupToMerge.groupId) {
                var added = _this.addChildrenToGroup(mergeResult, groupToMerge.groupId, groupToMerge.children[0]);
                if (added)
                    return mergeResult;
            }
            groupToMerge.children.forEach(function (child) { return mergeTrees(mergeResult, child); });
            return mergeResult;
        };
        // we can't just merge the leaf path trees as groups can be split apart - instead only merge if leaf
        // path groups with the same root group id are contiguous.
        var mergeColDefs = [];
        for (var i = 1; i <= leafPathTrees.length; i++) {
            var first = leafPathTrees[i - 1];
            var second = leafPathTrees[i];
            if (matchingRootGroupIds(first, second)) {
                leafPathTrees[i] = mergeTrees(first, second);
            }
            else {
                mergeColDefs.push(first);
            }
        }
        return mergeColDefs;
    };
    ToolPanelColDefService.prototype.addChildrenToGroup = function (tree, groupId, colDef) {
        var _this = this;
        var subGroupIsSplit = function (currentGroup, groupToAdd) {
            var existingChildIds = currentGroup.children.map(_this.getId);
            var childGroupAlreadyExists = _.includes(existingChildIds, _this.getId(groupToAdd));
            var lastChild = _.last(currentGroup.children);
            var lastChildIsDifferent = lastChild && _this.getId(lastChild) !== _this.getId(groupToAdd);
            return childGroupAlreadyExists && lastChildIsDifferent;
        };
        if (!this.isColGroupDef(tree))
            return true;
        var currentGroup = tree;
        var groupToAdd = colDef;
        if (subGroupIsSplit(currentGroup, groupToAdd)) {
            currentGroup.children.push(groupToAdd);
            return true;
        }
        if (currentGroup.groupId === groupId) {
            // add children that don't already exist to group
            var existingChildIds = currentGroup.children.map(this.getId);
            var colDefAlreadyPresent = _.includes(existingChildIds, this.getId(groupToAdd));
            if (!colDefAlreadyPresent) {
                currentGroup.children.push(groupToAdd);
                return true;
            }
        }
        // recurse until correct group is found to add children
        currentGroup.children.forEach(function (subGroup) { return _this.addChildrenToGroup(subGroup, groupId, colDef); });
        return false;
    };
    __decorate$B([
        Autowired('columnController')
    ], ToolPanelColDefService.prototype, "columnController", void 0);
    ToolPanelColDefService = __decorate$B([
        Bean('toolPanelColDefService')
    ], ToolPanelColDefService);
    return ToolPanelColDefService;
}());

var SideBarModule = {
    moduleName: ModuleNames.SideBarModule,
    beans: [ToolPanelColDefService],
    agStackComponents: [
        { componentName: 'AgHorizontalResize', componentClass: HorizontalResizeComp },
        { componentName: 'AgSideBar', componentClass: SideBarComp },
        { componentName: 'AgSideBarButtons', componentClass: SideBarButtonsComp },
    ],
    dependantModules: [
    // EnterpriseCoreModule
    ]
};

var ColumnsToolPanelModule = {
    moduleName: ModuleNames.ColumnToolPanelModule,
    beans: [],
    agStackComponents: [
        { componentName: 'AgPrimaryColsHeader', componentClass: PrimaryColsHeaderPanel },
        { componentName: 'AgPrimaryColsList', componentClass: PrimaryColsListPanel },
        { componentName: 'AgPrimaryCols', componentClass: PrimaryColsPanel }
    ],
    userComponents: [
        { componentName: 'agColumnsToolPanel', componentClass: ColumnToolPanel },
    ],
    dependantModules: [
        //EnterpriseCoreModule,
        RowGroupingModule,
        SideBarModule
    ]
};

var __extends$m = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate$C = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var EXPAND_STATE$1;
(function (EXPAND_STATE) {
    EXPAND_STATE[EXPAND_STATE["EXPANDED"] = 0] = "EXPANDED";
    EXPAND_STATE[EXPAND_STATE["COLLAPSED"] = 1] = "COLLAPSED";
    EXPAND_STATE[EXPAND_STATE["INDETERMINATE"] = 2] = "INDETERMINATE";
})(EXPAND_STATE$1 || (EXPAND_STATE$1 = {}));
var FiltersToolPanelHeaderPanel = /** @class */ (function (_super) {
    __extends$m(FiltersToolPanelHeaderPanel, _super);
    function FiltersToolPanelHeaderPanel() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    FiltersToolPanelHeaderPanel.prototype.preConstruct = function () {
        this.setTemplate("<div class=\"ag-filter-toolpanel-search\" role=\"presentation\">\n            <div ref=\"eExpand\" class=\"ag-filter-toolpanel-expand\"></div>\n            <ag-input-text-field ref=\"eFilterTextField\" class=\"ag-filter-toolpanel-search-input\"></ag-input-text-field>\n        </div>");
    };
    FiltersToolPanelHeaderPanel.prototype.postConstruct = function () {
        this.eSearchTextField.onValueChange(this.onSearchTextChanged.bind(this));
        this.createExpandIcons();
        this.setExpandState(EXPAND_STATE$1.EXPANDED);
        this.addDestroyableEventListener(this.eExpand, 'click', this.onExpandClicked.bind(this));
        this.addDestroyableEventListener(this.eventService, Events.EVENT_NEW_COLUMNS_LOADED, this.showOrHideOptions.bind(this));
    };
    FiltersToolPanelHeaderPanel.prototype.init = function (params) {
        this.params = params;
        if (this.columnController.isReady()) {
            this.showOrHideOptions();
        }
    };
    FiltersToolPanelHeaderPanel.prototype.createExpandIcons = function () {
        this.eExpand.appendChild(this.eExpandChecked = _.createIconNoSpan('columnSelectOpen', this.gridOptionsWrapper));
        this.eExpand.appendChild(this.eExpandUnchecked = _.createIconNoSpan('columnSelectClosed', this.gridOptionsWrapper));
        this.eExpand.appendChild(this.eExpandIndeterminate = _.createIconNoSpan('columnSelectIndeterminate', this.gridOptionsWrapper));
    };
    // we only show expand / collapse if we are showing filters
    FiltersToolPanelHeaderPanel.prototype.showOrHideOptions = function () {
        var showFilterSearch = !this.params.suppressFilterSearch;
        var showExpand = !this.params.suppressExpandAll;
        var translate = this.gridOptionsWrapper.getLocaleTextFunc();
        this.eSearchTextField.setInputPlaceholder(translate('searchOoo', 'Search...'));
        var isFilterGroupPresent = function (col) { return col.getOriginalParent() && col.isFilterAllowed(); };
        var filterGroupsPresent = this.columnController.getAllGridColumns().some(isFilterGroupPresent);
        _.setDisplayed(this.eSearchTextField.getGui(), showFilterSearch);
        _.setDisplayed(this.eExpand, showExpand && filterGroupsPresent);
    };
    FiltersToolPanelHeaderPanel.prototype.onSearchTextChanged = function () {
        var _this = this;
        if (!this.onSearchTextChangedDebounced) {
            this.onSearchTextChangedDebounced = _.debounce(function () {
                _this.dispatchEvent({ type: 'searchChanged', searchText: _this.eSearchTextField.getValue() });
            }, 300);
        }
        this.onSearchTextChangedDebounced();
    };
    FiltersToolPanelHeaderPanel.prototype.onExpandClicked = function () {
        var event = this.currentExpandState === EXPAND_STATE$1.EXPANDED ? { type: 'collapseAll' } : { type: 'expandAll' };
        this.dispatchEvent(event);
    };
    FiltersToolPanelHeaderPanel.prototype.setExpandState = function (state) {
        this.currentExpandState = state;
        _.setDisplayed(this.eExpandChecked, this.currentExpandState === EXPAND_STATE$1.EXPANDED);
        _.setDisplayed(this.eExpandUnchecked, this.currentExpandState === EXPAND_STATE$1.COLLAPSED);
        _.setDisplayed(this.eExpandIndeterminate, this.currentExpandState === EXPAND_STATE$1.INDETERMINATE);
    };
    __decorate$C([
        Autowired('gridOptionsWrapper')
    ], FiltersToolPanelHeaderPanel.prototype, "gridOptionsWrapper", void 0);
    __decorate$C([
        Autowired('columnController')
    ], FiltersToolPanelHeaderPanel.prototype, "columnController", void 0);
    __decorate$C([
        Autowired('eventService')
    ], FiltersToolPanelHeaderPanel.prototype, "eventService", void 0);
    __decorate$C([
        RefSelector('eExpand')
    ], FiltersToolPanelHeaderPanel.prototype, "eExpand", void 0);
    __decorate$C([
        RefSelector('eFilterTextField')
    ], FiltersToolPanelHeaderPanel.prototype, "eSearchTextField", void 0);
    __decorate$C([
        PreConstruct
    ], FiltersToolPanelHeaderPanel.prototype, "preConstruct", null);
    __decorate$C([
        PostConstruct
    ], FiltersToolPanelHeaderPanel.prototype, "postConstruct", null);
    return FiltersToolPanelHeaderPanel;
}(Component));

var __extends$n = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate$D = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var ToolPanelFilterComp = /** @class */ (function (_super) {
    __extends$n(ToolPanelFilterComp, _super);
    function ToolPanelFilterComp(hideHeader) {
        if (hideHeader === void 0) { hideHeader = false; }
        var _this = _super.call(this, ToolPanelFilterComp.TEMPLATE) || this;
        _this.expanded = false;
        _this.hideHeader = hideHeader;
        return _this;
    }
    ToolPanelFilterComp.prototype.postConstruct = function () {
        this.eExpandChecked = _.createIconNoSpan('columnSelectOpen', this.gridOptionsWrapper);
        this.eExpandUnchecked = _.createIconNoSpan('columnSelectClosed', this.gridOptionsWrapper);
        this.eExpand.appendChild(this.eExpandChecked);
        this.eExpand.appendChild(this.eExpandUnchecked);
    };
    ToolPanelFilterComp.prototype.setColumn = function (column) {
        this.column = column;
        this.eFilterName.innerText = this.columnController.getDisplayNameForColumn(this.column, 'header', false);
        this.addDestroyableEventListener(this.eFilterToolPanelHeader, 'click', this.toggleExpanded.bind(this));
        this.addDestroyableEventListener(this.eventService, Events.EVENT_FILTER_OPENED, this.onFilterOpened.bind(this));
        this.addInIcon('filter', this.eFilterIcon, this.column);
        _.addOrRemoveCssClass(this.eFilterIcon, 'ag-hidden', !this.isFilterActive());
        _.addCssClass(this.eExpandChecked, 'ag-hidden');
        if (this.hideHeader) {
            _.addOrRemoveCssClass(this.eFilterToolPanelHeader, 'ag-hidden', true);
        }
        this.addDestroyableEventListener(this.column, Column.EVENT_FILTER_CHANGED, this.onFilterChanged.bind(this));
    };
    ToolPanelFilterComp.prototype.getColumn = function () {
        return this.column;
    };
    ToolPanelFilterComp.prototype.getColumnFilterName = function () {
        return this.columnController.getDisplayNameForColumn(this.column, 'header', false);
    };
    ToolPanelFilterComp.prototype.addCssClassToTitleBar = function (cssClass) {
        _.addCssClass(this.eFilterToolPanelHeader, cssClass);
    };
    ToolPanelFilterComp.prototype.addInIcon = function (iconName, eParent, column) {
        if (eParent == null) {
            return;
        }
        var eIcon = _.createIconNoSpan(iconName, this.gridOptionsWrapper, column);
        eParent.appendChild(eIcon);
    };
    ToolPanelFilterComp.prototype.isFilterActive = function () {
        return this.filterManager.isFilterActive(this.column);
    };
    ToolPanelFilterComp.prototype.onFilterChanged = function () {
        _.addOrRemoveCssClass(this.eFilterIcon, 'ag-hidden', !this.isFilterActive());
        this.dispatchEvent({ type: Column.EVENT_FILTER_CHANGED });
    };
    ToolPanelFilterComp.prototype.toggleExpanded = function () {
        this.expanded ? this.collapse() : this.expand();
    };
    ToolPanelFilterComp.prototype.expand = function () {
        var _this = this;
        if (this.expanded)
            return;
        this.expanded = true;
        var container = _.loadTemplate("<div class=\"ag-filter-toolpanel-instance-filter\" />");
        var filterPromise = this.filterManager.getOrCreateFilterWrapper(this.column, 'TOOLBAR').filterPromise;
        if (filterPromise) {
            filterPromise.then(function (filter) {
                _this.underlyingFilter = filter;
                container.appendChild(filter.getGui());
                _this.agFilterToolPanelBody.appendChild(container);
                if (filter.afterGuiAttached) {
                    filter.afterGuiAttached({});
                }
            });
        }
        _.setDisplayed(this.eExpandChecked, true);
        _.setDisplayed(this.eExpandUnchecked, false);
    };
    ToolPanelFilterComp.prototype.collapse = function () {
        if (!this.expanded)
            return;
        this.expanded = false;
        this.agFilterToolPanelBody.removeChild(this.agFilterToolPanelBody.children[0]);
        _.setDisplayed(this.eExpandChecked, false);
        _.setDisplayed(this.eExpandUnchecked, true);
    };
    ToolPanelFilterComp.prototype.refreshFilter = function () {
        var filter = this.underlyingFilter;
        if (!filter)
            return;
        // set filters should be updated when the filter has been changed elsewhere, i.e. via api. Note that we can't
        // use 'afterGuiAttached' to refresh the virtual list as it also focuses on the mini filter which changes the
        // scroll position in the filter list panel
        var isSetFilter = filter.refreshVirtualList;
        if (isSetFilter) {
            filter.refreshVirtualList();
        }
    };
    ToolPanelFilterComp.prototype.onFilterOpened = function (event) {
        if (event.source !== 'COLUMN_MENU') {
            return;
        }
        if (event.column !== this.column) {
            return;
        }
        if (!this.expanded) {
            return;
        }
        this.collapse();
    };
    ToolPanelFilterComp.TEMPLATE = "<div class=\"ag-filter-toolpanel-instance\">\n            <div class=\"ag-filter-toolpanel-header ag-filter-toolpanel-instance-header\" ref=\"eFilterToolPanelHeader\">\n                <div ref=\"eExpand\" class=\"ag-filter-toolpanel-expand\"></div>\n                <span ref=\"eFilterName\" class=\"ag-header-cell-text\"></span>\n                <span ref=\"eFilterIcon\" class=\"ag-header-icon ag-filter-icon ag-filter-toolpanel-instance-header-icon\" aria-hidden=\"true\"></span>\n            </div>\n            <div class=\"ag-filter-toolpanel-instance-body ag-filter\" ref=\"agFilterToolPanelBody\"/></div>";
    __decorate$D([
        RefSelector('eFilterToolPanelHeader')
    ], ToolPanelFilterComp.prototype, "eFilterToolPanelHeader", void 0);
    __decorate$D([
        RefSelector('eFilterName')
    ], ToolPanelFilterComp.prototype, "eFilterName", void 0);
    __decorate$D([
        RefSelector('agFilterToolPanelBody')
    ], ToolPanelFilterComp.prototype, "agFilterToolPanelBody", void 0);
    __decorate$D([
        RefSelector('eFilterIcon')
    ], ToolPanelFilterComp.prototype, "eFilterIcon", void 0);
    __decorate$D([
        RefSelector('eExpand')
    ], ToolPanelFilterComp.prototype, "eExpand", void 0);
    __decorate$D([
        Autowired('gridApi')
    ], ToolPanelFilterComp.prototype, "gridApi", void 0);
    __decorate$D([
        Autowired('filterManager')
    ], ToolPanelFilterComp.prototype, "filterManager", void 0);
    __decorate$D([
        Autowired('eventService')
    ], ToolPanelFilterComp.prototype, "eventService", void 0);
    __decorate$D([
        Autowired('gridOptionsWrapper')
    ], ToolPanelFilterComp.prototype, "gridOptionsWrapper", void 0);
    __decorate$D([
        Autowired('columnController')
    ], ToolPanelFilterComp.prototype, "columnController", void 0);
    __decorate$D([
        PostConstruct
    ], ToolPanelFilterComp.prototype, "postConstruct", null);
    return ToolPanelFilterComp;
}(Component));

var __extends$o = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate$E = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var ToolPanelFilterGroupComp = /** @class */ (function (_super) {
    __extends$o(ToolPanelFilterGroupComp, _super);
    function ToolPanelFilterGroupComp(columnGroup, childFilterComps, expandedCallback, depth) {
        var _this = _super.call(this) || this;
        _this.columnGroup = columnGroup;
        _this.childFilterComps = childFilterComps;
        _this.depth = depth;
        _this.expandedCallback = expandedCallback;
        return _this;
    }
    ToolPanelFilterGroupComp.prototype.preConstruct = function () {
        var groupParams = {
            cssIdentifier: 'filter-toolpanel',
            direction: 'vertical'
        };
        this.setTemplate(ToolPanelFilterGroupComp.TEMPLATE, { filterGroupComp: groupParams });
    };
    ToolPanelFilterGroupComp.prototype.init = function () {
        var _this = this;
        this.setGroupTitle();
        this.filterGroupComp.setAlignItems('stretch');
        _.addCssClass(this.filterGroupComp.getGui(), "ag-filter-toolpanel-group-level-" + this.depth);
        this.filterGroupComp.addCssClassToTitleBar("ag-filter-toolpanel-group-level-" + this.depth + "-header");
        this.childFilterComps.forEach(function (filterComp) {
            _this.filterGroupComp.addItem(filterComp);
            filterComp.addCssClassToTitleBar("ag-filter-toolpanel-group-level-" + (_this.depth + 1) + "-header");
        });
        if (!this.isColumnGroup()) {
            this.addTopLevelColumnGroupExpandListener();
        }
        else {
            this.addDestroyableEventListener(this.filterGroupComp, 'expanded', function () {
                _this.expandedCallback();
            });
            this.addDestroyableEventListener(this.filterGroupComp, 'collapsed', function () {
                _this.expandedCallback();
            });
        }
        this.addFilterChangedListeners();
    };
    ToolPanelFilterGroupComp.prototype.addCssClassToTitleBar = function (cssClass) {
        this.filterGroupComp.addCssClassToTitleBar(cssClass);
    };
    ToolPanelFilterGroupComp.prototype.refreshFilters = function () {
        this.childFilterComps.forEach(function (filterComp) {
            if (filterComp instanceof ToolPanelFilterGroupComp) {
                filterComp.refreshFilters();
            }
            else {
                filterComp.refreshFilter();
            }
        });
    };
    ToolPanelFilterGroupComp.prototype.isColumnGroup = function () {
        return this.columnGroup instanceof OriginalColumnGroup;
    };
    ToolPanelFilterGroupComp.prototype.isExpanded = function () {
        return this.filterGroupComp.isExpanded();
    };
    ToolPanelFilterGroupComp.prototype.getChildren = function () {
        return this.childFilterComps;
    };
    ToolPanelFilterGroupComp.prototype.getFilterGroupName = function () {
        return this.filterGroupName ? this.filterGroupName : '';
    };
    ToolPanelFilterGroupComp.prototype.getFilterGroupId = function () {
        return this.columnGroup.getId();
    };
    ToolPanelFilterGroupComp.prototype.hideGroupItem = function (hide, index) {
        this.filterGroupComp.hideItem(hide, index);
    };
    ToolPanelFilterGroupComp.prototype.hideGroup = function (hide) {
        _.addOrRemoveCssClass(this.getGui(), 'ag-hidden', hide);
    };
    ToolPanelFilterGroupComp.prototype.addTopLevelColumnGroupExpandListener = function () {
        var _this = this;
        this.addDestroyableEventListener(this.filterGroupComp, 'expanded', function () {
            _this.childFilterComps.forEach(function (filterComp) {
                // also need to refresh the virtual list on set filters as the filter may have been updated elsewhere
                if (filterComp instanceof ToolPanelFilterComp) {
                    filterComp.expand();
                    filterComp.refreshFilter();
                }
                else {
                    filterComp.refreshFilters();
                }
            });
        });
    };
    ToolPanelFilterGroupComp.prototype.addFilterChangedListeners = function () {
        var _this = this;
        if (this.columnGroup instanceof OriginalColumnGroup) {
            var group_1 = this.columnGroup;
            var anyChildFiltersActive_1 = function () { return group_1.getLeafColumns().some(function (col) { return col.isFilterActive(); }); };
            group_1.getLeafColumns().forEach(function (column) {
                _this.addDestroyableEventListener(column, Column.EVENT_FILTER_CHANGED, function () {
                    _.addOrRemoveCssClass(_this.filterGroupComp.getGui(), 'ag-has-filter', anyChildFiltersActive_1());
                });
            });
        }
        else {
            var column_1 = this.columnGroup;
            this.addDestroyableEventListener(this.eventService, Events.EVENT_FILTER_OPENED, this.onFilterOpened.bind(this));
            this.addDestroyableEventListener(column_1, Column.EVENT_FILTER_CHANGED, function () {
                _.addOrRemoveCssClass(_this.filterGroupComp.getGui(), 'ag-has-filter', column_1.isFilterActive());
            });
        }
    };
    ToolPanelFilterGroupComp.prototype.onFilterOpened = function (event) {
        // when a filter is opened elsewhere, i.e. column menu we close the filter comp so we also need to collapse
        // the column group. This approach means we don't need to try and sync filter models on the same column.
        if (event.source !== 'COLUMN_MENU') {
            return;
        }
        if (event.column !== this.columnGroup) {
            return;
        }
        if (!this.isExpanded()) {
            return;
        }
        this.collapse();
    };
    ToolPanelFilterGroupComp.prototype.expand = function () {
        this.filterGroupComp.toggleGroupExpand(true);
    };
    ToolPanelFilterGroupComp.prototype.collapse = function () {
        this.filterGroupComp.toggleGroupExpand(false);
    };
    ToolPanelFilterGroupComp.prototype.setGroupTitle = function () {
        this.filterGroupName = (this.columnGroup instanceof OriginalColumnGroup) ?
            this.getColumnGroupName(this.columnGroup) : this.getColumnName(this.columnGroup);
        this.filterGroupComp.setTitle(this.filterGroupName);
    };
    ToolPanelFilterGroupComp.prototype.getColumnGroupName = function (columnGroup) {
        return this.columnController.getDisplayNameForOriginalColumnGroup(null, columnGroup, 'toolPanel');
    };
    ToolPanelFilterGroupComp.prototype.getColumnName = function (column) {
        return this.columnController.getDisplayNameForColumn(column, 'header', false);
    };
    ToolPanelFilterGroupComp.prototype.destroyFilters = function () {
        this.childFilterComps.forEach(function (filterComp) { return filterComp.destroy(); });
        this.childFilterComps.length = 0;
        _.clearElement(this.getGui());
    };
    ToolPanelFilterGroupComp.prototype.destroy = function () {
        this.destroyFilters();
        _super.prototype.destroy.call(this);
    };
    ToolPanelFilterGroupComp.TEMPLATE = "<div class=\"ag-filter-toolpanel-group-wrapper\">\n            <ag-group-component ref=\"filterGroupComp\"></ag-group-component>\n         </div>";
    __decorate$E([
        RefSelector('filterGroupComp')
    ], ToolPanelFilterGroupComp.prototype, "filterGroupComp", void 0);
    __decorate$E([
        Autowired('eventService')
    ], ToolPanelFilterGroupComp.prototype, "eventService", void 0);
    __decorate$E([
        Autowired('columnController')
    ], ToolPanelFilterGroupComp.prototype, "columnController", void 0);
    __decorate$E([
        PreConstruct
    ], ToolPanelFilterGroupComp.prototype, "preConstruct", null);
    __decorate$E([
        PostConstruct
    ], ToolPanelFilterGroupComp.prototype, "init", null);
    return ToolPanelFilterGroupComp;
}(Component));

var __extends$p = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate$F = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var FiltersToolPanelListPanel = /** @class */ (function (_super) {
    __extends$p(FiltersToolPanelListPanel, _super);
    function FiltersToolPanelListPanel() {
        var _this = _super.call(this, FiltersToolPanelListPanel.TEMPLATE) || this;
        _this.initialised = false;
        _this.filterGroupComps = [];
        return _this;
    }
    FiltersToolPanelListPanel.prototype.init = function (params) {
        var _this = this;
        this.initialised = true;
        var defaultParams = {
            suppressExpandAll: false,
            suppressFilterSearch: false,
            suppressSyncLayoutWithGrid: false,
            api: this.gridApi
        };
        _.mergeDeep(defaultParams, params);
        this.params = defaultParams;
        if (!this.params.suppressSyncLayoutWithGrid) {
            this.addDestroyableEventListener(this.eventService, Events.EVENT_COLUMN_MOVED, function () { return _this.onColumnsChanged(); });
        }
        this.addDestroyableEventListener(this.eventService, Events.EVENT_NEW_COLUMNS_LOADED, function () { return _this.onColumnsChanged(); });
        this.addDestroyableEventListener(this.eventService, Events.EVENT_TOOL_PANEL_VISIBLE_CHANGED, function (event) {
            // when re-entering the filters tool panel we need to refresh the virtual lists in the set filters in case
            // filters have been changed elsewhere, i.e. via an api call.
            if (event.source === 'filters') {
                _this.refreshFilters();
            }
        });
        if (this.columnController.isReady()) {
            this.onColumnsChanged();
        }
    };
    FiltersToolPanelListPanel.prototype.onColumnsChanged = function () {
        var pivotModeActive = this.columnController.isPivotMode();
        var shouldSyncColumnLayoutWithGrid = !this.params.suppressSyncLayoutWithGrid && !pivotModeActive;
        shouldSyncColumnLayoutWithGrid ? this.syncFilterLayout() : this.buildTreeFromProvidedColumnDefs();
    };
    FiltersToolPanelListPanel.prototype.syncFilterLayout = function () {
        this.toolPanelColDefService.syncLayoutWithGrid(this.setFiltersLayout.bind(this));
    };
    FiltersToolPanelListPanel.prototype.buildTreeFromProvidedColumnDefs = function () {
        var _this = this;
        this.destroyFilters();
        var columnTree = this.columnController.getPrimaryColumnTree();
        this.filterGroupComps = this.recursivelyAddComps(columnTree, 0);
        var len = this.filterGroupComps.length;
        if (len) {
            this.filterGroupComps.forEach(function (comp) { return _this.appendChild(comp); });
            this.setFirstAndLastVisible(0, len - 1);
        }
        // perform search if searchFilterText exists
        if (_.exists(this.searchFilterText)) {
            this.searchFilters(this.searchFilterText);
        }
        // notify header of expand
        this.fireExpandedEvent();
    };
    FiltersToolPanelListPanel.prototype.setFiltersLayout = function (colDefs) {
        var _this = this;
        this.destroyFilters();
        var columnTree = this.toolPanelColDefService.createColumnTree(colDefs);
        this.filterGroupComps = this.recursivelyAddComps(columnTree, 0);
        var len = this.filterGroupComps.length;
        if (len) {
            this.filterGroupComps.forEach(function (comp) { return _this.appendChild(comp); });
            this.setFirstAndLastVisible(0, len - 1);
        }
        // perform search if searchFilterText exists
        if (_.exists(this.searchFilterText)) {
            this.searchFilters(this.searchFilterText);
        }
        // notify header of expand
        this.fireExpandedEvent();
    };
    FiltersToolPanelListPanel.prototype.recursivelyAddComps = function (tree, depth) {
        var _this = this;
        return _.flatten(tree.map(function (child) {
            if (child instanceof OriginalColumnGroup) {
                return _.flatten(_this.recursivelyAddFilterGroupComps(child, depth));
            }
            var column = child;
            if (!_this.shouldDisplayFilter(column)) {
                return [];
            }
            var hideFilterCompHeader = depth === 0;
            var filterComp = new ToolPanelFilterComp(hideFilterCompHeader);
            _this.getContext().wireBean(filterComp);
            filterComp.setColumn(column);
            if (depth > 0) {
                return filterComp;
            }
            var filterGroupComp = new ToolPanelFilterGroupComp(column, [filterComp], _this.onGroupExpanded.bind(_this), depth);
            _this.getContext().wireBean(filterGroupComp);
            filterGroupComp.addCssClassToTitleBar('ag-filter-toolpanel-header');
            filterGroupComp.collapse();
            return filterGroupComp;
        }));
    };
    FiltersToolPanelListPanel.prototype.recursivelyAddFilterGroupComps = function (columnGroup, depth) {
        if (!this.filtersExistInChildren(columnGroup.getChildren()))
            return;
        if (columnGroup.getColGroupDef() && columnGroup.getColGroupDef().suppressFiltersToolPanel) {
            return [];
        }
        var newDepth = columnGroup.isPadding() ? depth : depth + 1;
        var childFilterComps = _.flatten(this.recursivelyAddComps(columnGroup.getChildren(), newDepth));
        if (columnGroup.isPadding())
            return childFilterComps;
        var filterGroupComp = new ToolPanelFilterGroupComp(columnGroup, childFilterComps, this.onGroupExpanded.bind(this), depth);
        this.getContext().wireBean(filterGroupComp);
        filterGroupComp.addCssClassToTitleBar('ag-filter-toolpanel-header');
        return [filterGroupComp];
    };
    FiltersToolPanelListPanel.prototype.filtersExistInChildren = function (tree) {
        var _this = this;
        return tree.some(function (child) {
            if (child instanceof OriginalColumnGroup) {
                return _this.filtersExistInChildren(child.getChildren());
            }
            return _this.shouldDisplayFilter(child);
        });
    };
    FiltersToolPanelListPanel.prototype.shouldDisplayFilter = function (column) {
        var suppressFiltersToolPanel = column.getColDef() && column.getColDef().suppressFiltersToolPanel;
        return column.isFilterAllowed() && !suppressFiltersToolPanel;
    };
    // we don't support refreshing, but must implement because it's on the tool panel interface
    FiltersToolPanelListPanel.prototype.refresh = function () { };
    // lazy initialise the panel
    FiltersToolPanelListPanel.prototype.setVisible = function (visible) {
        _super.prototype.setDisplayed.call(this, visible);
        if (visible && !this.initialised) {
            this.init(this.params);
        }
    };
    FiltersToolPanelListPanel.prototype.expandFilterGroups = function (expand, groupIds) {
        var updatedGroupIds = [];
        var updateGroupExpandState = function (filterGroup) {
            var groupId = filterGroup.getFilterGroupId();
            var shouldExpandOrCollapse = !groupIds || _.includes(groupIds, groupId);
            if (shouldExpandOrCollapse) {
                // don't expand 'column groups', i.e. top level columns wrapped in a group
                if (expand && filterGroup.isColumnGroup()) {
                    filterGroup.expand();
                }
                else {
                    filterGroup.collapse();
                }
                updatedGroupIds.push(groupId);
            }
            // recursively look for more groups to expand / collapse
            filterGroup.getChildren().forEach(function (child) {
                if (child instanceof ToolPanelFilterGroupComp) {
                    updateGroupExpandState(child);
                }
            });
        };
        this.filterGroupComps.forEach(updateGroupExpandState);
        // update header expand / collapse icon
        this.onGroupExpanded();
        if (groupIds) {
            var unrecognisedGroupIds = groupIds.filter(function (groupId) { return updatedGroupIds.indexOf(groupId) < 0; });
            if (unrecognisedGroupIds.length > 0) {
                console.warn('ag-Grid: unable to find groups for these supplied groupIds:', unrecognisedGroupIds);
            }
        }
    };
    FiltersToolPanelListPanel.prototype.expandFilters = function (expand, colIds) {
        var updatedColIds = [];
        var updateGroupExpandState = function (filterComp) {
            if (filterComp instanceof ToolPanelFilterGroupComp) {
                var anyChildrenChanged_1 = false;
                filterComp.getChildren().forEach(function (child) {
                    var childUpdated = updateGroupExpandState(child);
                    if (childUpdated) {
                        if (expand) {
                            filterComp.expand();
                            anyChildrenChanged_1 = true;
                        }
                        else if (!filterComp.isColumnGroup()) {
                            // we only collapse columns wrapped in groups
                            filterComp.collapse();
                        }
                    }
                });
                return anyChildrenChanged_1;
            }
            var colId = filterComp.getColumn().getColId();
            var updateFilterExpandState = !colIds || _.includes(colIds, colId);
            if (updateFilterExpandState) {
                expand ? filterComp.expand() : filterComp.collapse();
                updatedColIds.push(colId);
            }
            return updateFilterExpandState;
        };
        this.filterGroupComps.forEach(updateGroupExpandState);
        // update header expand / collapse icon
        this.onGroupExpanded();
        if (colIds) {
            var unrecognisedColIds = colIds.filter(function (colId) { return updatedColIds.indexOf(colId) < 0; });
            if (unrecognisedColIds.length > 0) {
                console.warn('ag-Grid: unable to find columns for these supplied colIds:', unrecognisedColIds);
            }
        }
    };
    FiltersToolPanelListPanel.prototype.onGroupExpanded = function () {
        this.fireExpandedEvent();
    };
    FiltersToolPanelListPanel.prototype.fireExpandedEvent = function () {
        var expandedCount = 0;
        var notExpandedCount = 0;
        var updateExpandCounts = function (filterGroup) {
            if (!filterGroup.isColumnGroup())
                return;
            filterGroup.isExpanded() ? expandedCount++ : notExpandedCount++;
            filterGroup.getChildren().forEach(function (child) {
                if (child instanceof ToolPanelFilterGroupComp) {
                    updateExpandCounts(child);
                }
            });
        };
        this.filterGroupComps.forEach(updateExpandCounts);
        var state;
        if (expandedCount > 0 && notExpandedCount > 0) {
            state = EXPAND_STATE$1.INDETERMINATE;
        }
        else if (notExpandedCount > 0) {
            state = EXPAND_STATE$1.COLLAPSED;
        }
        else {
            state = EXPAND_STATE$1.EXPANDED;
        }
        this.dispatchEvent({ type: 'groupExpanded', state: state });
    };
    FiltersToolPanelListPanel.prototype.performFilterSearch = function (searchText) {
        this.searchFilterText = _.exists(searchText) ? searchText.toLowerCase() : null;
        this.searchFilters(this.searchFilterText);
    };
    FiltersToolPanelListPanel.prototype.searchFilters = function (searchFilter) {
        var passesFilter = function (groupName) {
            return !_.exists(searchFilter) || groupName.toLowerCase().indexOf(searchFilter) !== -1;
        };
        var recursivelySearch = function (filterItem, parentPasses) {
            if (!(filterItem instanceof ToolPanelFilterGroupComp)) {
                return passesFilter(filterItem.getColumnFilterName());
            }
            var children = filterItem.getChildren();
            var groupNamePasses = passesFilter(filterItem.getFilterGroupName());
            // if group or parent already passed - ensure this group and all children are visible
            var alreadyPassed = parentPasses || groupNamePasses;
            if (alreadyPassed) {
                // ensure group visible
                filterItem.hideGroup(false);
                // ensure all children are visible
                for (var i = 0; i < children.length; i++) {
                    recursivelySearch(children[i], alreadyPassed);
                    filterItem.hideGroupItem(false, i);
                }
                return true;
            }
            // hide group item filters
            var anyChildPasses = false;
            children.forEach(function (child, index) {
                var childPasses = recursivelySearch(child, parentPasses);
                filterItem.hideGroupItem(!childPasses, index);
                if (childPasses)
                    anyChildPasses = true;
            });
            // hide group if no children pass
            filterItem.hideGroup(!anyChildPasses);
            return anyChildPasses;
        };
        var firstVisible;
        var lastVisible;
        this.filterGroupComps.forEach(function (filterGroup, idx) {
            recursivelySearch(filterGroup, false);
            if (firstVisible === undefined) {
                if (!_.containsClass(filterGroup.getGui(), 'ag-hidden')) {
                    firstVisible = idx;
                    lastVisible = idx;
                }
            }
            else if (!_.containsClass(filterGroup.getGui(), 'ag-hidden') && lastVisible !== idx) {
                lastVisible = idx;
            }
        });
        this.setFirstAndLastVisible(firstVisible, lastVisible);
    };
    FiltersToolPanelListPanel.prototype.setFirstAndLastVisible = function (firstIdx, lastIdx) {
        this.filterGroupComps.forEach(function (filterGroup, idx) {
            _.removeCssClass(filterGroup.getGui(), 'ag-first-group-visible');
            _.removeCssClass(filterGroup.getGui(), 'ag-last-group-visible');
            if (idx === firstIdx) {
                _.addCssClass(filterGroup.getGui(), 'ag-first-group-visible');
            }
            if (idx === lastIdx) {
                _.addCssClass(filterGroup.getGui(), 'ag-last-group-visible');
            }
        });
    };
    FiltersToolPanelListPanel.prototype.refreshFilters = function () {
        this.filterGroupComps.forEach(function (filterGroupComp) { return filterGroupComp.refreshFilters(); });
    };
    FiltersToolPanelListPanel.prototype.destroyFilters = function () {
        this.filterGroupComps.forEach(function (filterComp) { return filterComp.destroy(); });
        this.filterGroupComps.length = 0;
        _.clearElement(this.getGui());
    };
    FiltersToolPanelListPanel.prototype.destroy = function () {
        this.destroyFilters();
        _super.prototype.destroy.call(this);
    };
    FiltersToolPanelListPanel.TEMPLATE = "<div class=\"ag-filter-list-panel\"></div>";
    __decorate$F([
        Autowired("gridApi")
    ], FiltersToolPanelListPanel.prototype, "gridApi", void 0);
    __decorate$F([
        Autowired("eventService")
    ], FiltersToolPanelListPanel.prototype, "eventService", void 0);
    __decorate$F([
        Autowired('toolPanelColDefService')
    ], FiltersToolPanelListPanel.prototype, "toolPanelColDefService", void 0);
    __decorate$F([
        Autowired('columnController')
    ], FiltersToolPanelListPanel.prototype, "columnController", void 0);
    return FiltersToolPanelListPanel;
}(Component));

var __extends$q = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate$G = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var FiltersToolPanel = /** @class */ (function (_super) {
    __extends$q(FiltersToolPanel, _super);
    function FiltersToolPanel() {
        var _this = _super.call(this, FiltersToolPanel.TEMPLATE) || this;
        _this.initialised = false;
        return _this;
    }
    FiltersToolPanel.prototype.init = function (params) {
        this.initialised = true;
        var defaultParams = {
            suppressExpandAll: false,
            suppressFilterSearch: false,
            suppressSyncLayoutWithGrid: false,
            api: this.gridApi
        };
        _.mergeDeep(defaultParams, params);
        this.params = defaultParams;
        this.filtersToolPanelHeaderPanel.init(this.params);
        this.filtersToolPanelListPanel.init(this.params);
        var hideExpand = this.params.suppressExpandAll;
        var hideSearch = this.params.suppressFilterSearch;
        if (hideExpand && hideSearch) {
            this.filtersToolPanelHeaderPanel.setDisplayed(false);
        }
        this.addDestroyableEventListener(this.filtersToolPanelHeaderPanel, 'expandAll', this.onExpandAll.bind(this));
        this.addDestroyableEventListener(this.filtersToolPanelHeaderPanel, 'collapseAll', this.onCollapseAll.bind(this));
        this.addDestroyableEventListener(this.filtersToolPanelHeaderPanel, 'searchChanged', this.onSearchChanged.bind(this));
        this.addDestroyableEventListener(this.filtersToolPanelListPanel, 'groupExpanded', this.onGroupExpanded.bind(this));
    };
    // lazy initialise the panel
    FiltersToolPanel.prototype.setVisible = function (visible) {
        _super.prototype.setDisplayed.call(this, visible);
        if (visible && !this.initialised) {
            this.init(this.params);
        }
    };
    FiltersToolPanel.prototype.onExpandAll = function () {
        this.filtersToolPanelListPanel.expandFilterGroups(true);
    };
    FiltersToolPanel.prototype.onCollapseAll = function () {
        this.filtersToolPanelListPanel.expandFilterGroups(false);
    };
    FiltersToolPanel.prototype.onSearchChanged = function (event) {
        this.filtersToolPanelListPanel.performFilterSearch(event.searchText);
    };
    FiltersToolPanel.prototype.setFilterLayout = function (colDefs) {
        this.filtersToolPanelListPanel.setFiltersLayout(colDefs);
    };
    FiltersToolPanel.prototype.onGroupExpanded = function (event) {
        this.filtersToolPanelHeaderPanel.setExpandState(event.state);
    };
    FiltersToolPanel.prototype.expandFilterGroups = function (groupIds) {
        this.filtersToolPanelListPanel.expandFilterGroups(true, groupIds);
    };
    FiltersToolPanel.prototype.collapseFilterGroups = function (groupIds) {
        this.filtersToolPanelListPanel.expandFilterGroups(false, groupIds);
    };
    FiltersToolPanel.prototype.expandFilters = function (colIds) {
        this.filtersToolPanelListPanel.expandFilters(true, colIds);
    };
    FiltersToolPanel.prototype.collapseFilters = function (colIds) {
        this.filtersToolPanelListPanel.expandFilters(false, colIds);
    };
    FiltersToolPanel.prototype.syncLayoutWithGrid = function () {
        this.filtersToolPanelListPanel.syncFilterLayout();
    };
    FiltersToolPanel.prototype.refresh = function () {
        this.init(this.params);
    };
    FiltersToolPanel.prototype.destroy = function () {
        _super.prototype.destroy.call(this);
    };
    FiltersToolPanel.TEMPLATE = "<div class=\"ag-filter-toolpanel\">\n            <ag-filters-tool-panel-header ref=\"filtersToolPanelHeaderPanel\"></ag-filters-tool-panel-header>\n            <ag-filters-tool-panel-list ref=\"filtersToolPanelListPanel\"></ag-filters-tool-panel-list> \n         </div>";
    __decorate$G([
        RefSelector('filtersToolPanelHeaderPanel')
    ], FiltersToolPanel.prototype, "filtersToolPanelHeaderPanel", void 0);
    __decorate$G([
        RefSelector('filtersToolPanelListPanel')
    ], FiltersToolPanel.prototype, "filtersToolPanelListPanel", void 0);
    __decorate$G([
        Autowired("gridApi")
    ], FiltersToolPanel.prototype, "gridApi", void 0);
    __decorate$G([
        Autowired("eventService")
    ], FiltersToolPanel.prototype, "eventService", void 0);
    __decorate$G([
        Autowired('columnController')
    ], FiltersToolPanel.prototype, "columnController", void 0);
    return FiltersToolPanel;
}(Component));

var FiltersToolPanelModule = {
    moduleName: ModuleNames.FiltersToolPanelModule,
    beans: [],
    agStackComponents: [
        { componentName: 'AgFiltersToolPanelHeader', componentClass: FiltersToolPanelHeaderPanel },
        { componentName: 'AgFiltersToolPanelList', componentClass: FiltersToolPanelListPanel }
    ],
    userComponents: [
        { componentName: 'agFiltersToolPanel', componentClass: FiltersToolPanel },
    ],
    dependantModules: [
        SideBarModule,
    ]
};

// import {StatusBarModule} from "@ag-grid-enterprise/status-bar";
// import {ViewportRowModelModule} from "@ag-grid-enterprise/viewport-row-model";
//export * from "@ag-grid-community/all-modules";
// export * from "@ag-grid-enterprise/clipboard";
// export * from "@ag-grid-enterprise/column-tool-panel";
// export * from "@ag-grid-enterprise/excel-export";
// export * from "@ag-grid-enterprise/filter-tool-panel";
// export * from "@ag-grid-enterprise/charts";
// export * from "@ag-grid-enterprise/master-detail";
// export * from "@ag-grid-enterprise/menu";
// export * from "@ag-grid-enterprise/range-selection";
// export * from "@ag-grid-enterprise/rich-select";
// export * from "@ag-grid-enterprise/row-grouping";
// export * from "@ag-grid-enterprise/server-side-row-model";
// export * from "@ag-grid-enterprise/set-filter";
// export * from "@ag-grid-enterprise/side-bar";
// export * from "@ag-grid-enterprise/status-bar";
// export * from "@ag-grid-enterprise/viewport-row-model";
// export * from "@ag-grid-enterprise/core";
var AllEnterpriseModules = [
    // ClipboardModule,
    ColumnsToolPanelModule,
    // ExcelExportModule,
    FiltersToolPanelModule,
    // GridChartsModule,
    // MasterDetailModule,
    // MenuModule,
    // RangeSelectionModule,
    // RichSelectModule,
    RowGroupingModule,
    // ServerSideRowModelModule,
    // SetFilterModule,
    SideBarModule,
];
//export const AllModules: Module[] = AllEnterpriseModules;
var AllModules = AllCommunityModules.concat(AllEnterpriseModules);

export { AllEnterpriseModules, AllModules };
