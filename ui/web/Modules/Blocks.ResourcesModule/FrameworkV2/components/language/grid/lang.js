export function getLang(key) {
    let langsText = [{
        key: "zh-CN",
        localeText: {
            // for filter panel
            page: "第",
            more: "更多",
            to: "到",
            of: "共",
            next: "向后",
            last: "最后",
            first: "第一",
            previous: "向前",
            loadingOoo: "加载中...",

            // for set filter
            selectAll: "全选",
            searchOoo: "搜索",
            blanks: "空白",

            // for number filter and text filter
            filterOoo: "关键字",
            applyFilter: "确定",
            clearFilter: '清空',
            equals: "等于",
            notEqual: "不等于",

            // for number filter
            lessThan: "小于",
            greaterThan: "大于",
            lessThanOrEqual: "小于等于",
            greaterThanOrEqual: "大于等于",
            inRange: "范围内",

            // for text filter
            contains: "包含",
            notContains: "不包含",
            startsWith: "开始于",
            endsWith: "结束于",

            // filter conditions
            andCondition: "且",
            orCondition: "或",

            // the header of the default group column
            group: "组",

            // tool panel
            columns: "列控制",
            filters: "筛选器",
            rowGroupColumns: "laPivot Cols",
            rowGroupColumnsEmptyMessage: "拖动列到分组上",
            valueColumns: "值所在的列",
            pivotMode: "数据透视模式",
            groups: "分组",
            values: "值",
            pivots: "转换",
            valueColumnsEmptyMessage: "拖动列进行统计",
            pivotColumnsEmptyMessage: "拖动列在此进行转换",
            toolPanelButton: "la tool panel",

            // other
            noRowsToShow: "暂无数据",

            // enterprise menu
            pinColumn: "laPin Column",
            valueAggregation: "laValue Agg",
            autosizeThiscolumn: "laAutosize Diz",
            autosizeAllColumns: "laAutsoie em All",
            groupBy: "laGroup by",
            ungroupBy: "laUnGroup by",
            resetColumns: "laReset Those Cols",
            expandAll: "laOpen-em-up",
            collapseAll: "laClose-em-up",
            toolPanel: "laTool Panelo",
            export: "laExporto",
            csvExport: "laCSV Exportp",
            excelExport: "laExcel Exporto (.xlsx)",
            excelXmlExport: "laExcel Exporto (.xml)",

            // enterprise menu (charts)
            pivotChartAndPivotMode: "laPivot Chart & Pivot Mode",
            pivotChart: "laPivot Chart",
            chartRange: "laChart Range",

            columnChart: "laColumn",
            groupedColumn: "laGrouped",
            stackedColumn: "laStacked",
            normalizedColumn: "la100% Stacked",

            barChart: "laBar",
            groupedBar: "laGrouped",
            stackedBar: "laStacked",
            normalizedBar: "la100% Stacked",

            pieChart: "laPie",
            pie: "laPie",
            doughnut: "laDoughnut",

            line: "laLine",

            xyChart: "laX Y (Scatter)",
            scatter: "laScatter",
            bubble: "laBubble",

            areaChart: "laArea",
            area: "laArea",
            stackedArea: "laStacked",
            normalizedArea: "la100% Stacked",

            // enterprise menu pinning
            pinLeft: "laPin <<",
            pinRight: "laPin >>",
            noPin: "laDontPin <>",

            // enterprise menu aggregation and status bar
            sum: "求和",
            min: "最小值",
            max: "最大值",
            none: "laNone",
            count: "计数",
            averagae: "平均值",
            filteredRows: "laFiltered",
            selectedRows: "laSelected",
            totalRows: "laTotal Rows",
            totalAndFilteredRows: "laRows",

            // standard menu
            copy: "laCopy",
            copyWithHeaders: "laCopy Wit hHeaders",
            ctrlC: "ctrl n C",
            paste: "laPaste",
            ctrlV: "ctrl n V",

            // charts
            pivotChartTitle: "laPivot Chart",
            rangeChartTitle: "laRange Chart",
            settings: "laSettings",
            data: "laData",
            format: "laFormat",
            categories: "laCategories",
            series: "laSeries",
            axis: "laAxis",
            color: "laColor",
            thickness: "laThickness",
            xRotation: "laX Rotation",
            yRotation: "laY Rotation",
            ticks: "laTicks",
            width: "laWidth",
            length: "laLength",
            padding: "laPadding",
            chart: "laChart",
            title: "laTitle",
            font: "laFont",
            top: "laTop",
            right: "laRight",
            bottom: "laBottom",
            left: "laLeft",
            labels: "laLabels",
            size: "laSize",
            legend: "laLegend",
            position: "laPosition",
            markerSize: "laMarker Size",
            markerStroke: "laMarker Stroke",
            markerPadding: "laMarker Padding",
            itemPaddingX: "laItem Padding X",
            itemPaddingY: "laItem Padding Y",
            strokeWidth: "laStroke Width",
            offset: "laOffset",
            tooltips: "laTooltips",
            offsets: "laOffsets",
            callout: "laCallout",
            markers: "laMarkers",
            shadow: "laShadow",
            blur: "laBlur",
            xOffset: "laX Offset",
            yOffset: "laY Offset",
            lineWidth: "laLine Width",
            normal: "laNormal",
            bold: "laBold",
            italic: "laItalic",
            boldItalic: "laBold Italic",
            fillOpacity: "laFill Opacity",
            strokeOpacity: "laLine Opacity",
            columnGroup: "Column",
            barGroup: "Bar",
            pieGroup: "Pie",
            lineGroup: "Line",
            scatterGroup: "Scatter",
            areaGroup: "Area",
            groupedColumnTooltip: "laGrouped",
            stackedColumnTooltip: "laStacked",
            normalizedColumnTooltip: "la100% Stacked",
            groupedBarTooltip: "laGrouped",
            stackedBarTooltip: "laStacked",
            normalizedBarTooltip: "la100% Stacked",
            pieTooltip: "laPie",
            doughnutTooltip: "laDoughnut",
            lineTooltip: "laLine",
            groupedAreaTooltip: "laGrouped",
            stackedAreaTooltip: "laStacked",
            normalizedAreaTooltip: "la100% Stacked",
            scatterTooltip: "laScatter",
            bubbleTooltip: "laBubble",
            noDataToChart: "laNo data available to be charted.",
            pivotChartRequiresPivotMode:
                "laPivot Chart requires Pivot Mode enabled.",
            functionReadOnlyMode: "只读模式"
        }
    }, {
        key: "en",
        localeText: {
            // for filter panel
            page: "page",
            more: "more",
            to: "to",
            of: "of",
            next: "next",
            last: "last",
            first: "first",
            previous: "previous",
            loadingOoo: "loading...",
            // for set filter
            selectAll: "selectAll",
            searchOoo: "search",
            blanks: "blanks",
            // for number filter and text filter
            filterOoo: "filter",
            applyFilter: "applyFilter",
            clearFilter: 'clearFilter',
            equals: "equals",
            notEqual: "notEqual",

            // for number filter
            lessThan: "lessThan",
            greaterThan: "greaterThan",
            lessThanOrEqual: "lessThanOrEqual",
            greaterThanOrEqual: "greaterThanOrEqual",
            inRange: "inRange",

            // for text filter
            contains: "contains",
            notContains: "notContains",
            startsWith: "startsWith",
            endsWith: "endsWith",

            // filter conditions
            andCondition: "and",
            orCondition: "or",

            // the header of the default group column
            group: "group",

            // tool panel
            columns: "columns",
            filters: "filters",
            rowGroupColumns: "rowGroupColumns",
            rowGroupColumnsEmptyMessage: "rowGroupColumnsEmptyMessage",
            valueColumns: "valueColumns",
            pivotMode: "pivotMode",
            groups: "groups",
            values: "values",
            pivots: "pivots",
            valueColumnsEmptyMessage: "valueColumnsEmptyMessage",
            pivotColumnsEmptyMessage: "pivotColumnsEmptyMessage",
            toolPanelButton: "toolPanelButton",

            // other
            noRowsToShow: "noRowsToShow",

            // enterprise menu
            pinColumn: "pinColumn",
            valueAggregation: "valueAggregation",
            autosizeThiscolumn: "autosizeThiscolumn",
            autosizeAllColumns: "autosizeAllColumns",
            groupBy: "groupBy",
            ungroupBy: "ungroupBy",
            resetColumns: "resetColumns",
            expandAll: "expandAll",
            collapseAll: "collapseAll",
            toolPanel: "toolPanel",
            export: "export",
            csvExport: "csvExport",
            excelExport: "excelExport(.xlsx)",
            excelXmlExport: "excelXmlExport(.xml)",

            // enterprise menu (charts)
            pivotChartAndPivotMode: "laPivot Chart & Pivot Mode",
            pivotChart: "laPivot Chart",
            chartRange: "laChart Range",

            columnChart: "laColumn",
            groupedColumn: "laGrouped",
            stackedColumn: "laStacked",
            normalizedColumn: "la100% Stacked",

            barChart: "laBar",
            groupedBar: "laGrouped",
            stackedBar: "laStacked",
            normalizedBar: "la100% Stacked",

            pieChart: "laPie",
            pie: "laPie",
            doughnut: "laDoughnut",

            line: "laLine",

            xyChart: "laX Y (Scatter)",
            scatter: "laScatter",
            bubble: "laBubble",

            areaChart: "laArea",
            area: "laArea",
            stackedArea: "laStacked",
            normalizedArea: "la100% Stacked",

            // enterprise menu pinning
            pinLeft: "laPin <<",
            pinRight: "laPin >>",
            noPin: "laDontPin <>",

            // enterprise menu aggregation and status bar
            sum: "laSum",
            min: "laMin",
            max: "laMax",
            none: "laNone",
            count: "laCount",
            average: "laAverage",
            filteredRows: "laFiltered",
            selectedRows: "laSelected",
            totalRows: "laTotal Rows",
            totalAndFilteredRows: "laRows",

            // standard menu
            copy: "copy",
            copyWithHeaders: "copyWithHeaders",
            ctrlC: "ctrl n C",
            paste: "paste",
            ctrlV: "ctrl n V",

            // charts
            pivotChartTitle: "laPivot Chart",
            rangeChartTitle: "laRange Chart",
            settings: "laSettings",
            data: "laData",
            format: "laFormat",
            categories: "laCategories",
            series: "laSeries",
            axis: "laAxis",
            color: "laColor",
            thickness: "laThickness",
            xRotation: "laX Rotation",
            yRotation: "laY Rotation",
            ticks: "laTicks",
            width: "laWidth",
            length: "laLength",
            padding: "laPadding",
            chart: "laChart",
            title: "laTitle",
            font: "laFont",
            top: "laTop",
            right: "laRight",
            bottom: "laBottom",
            left: "laLeft",
            labels: "laLabels",
            size: "laSize",
            legend: "laLegend",
            position: "laPosition",
            markerSize: "laMarker Size",
            markerStroke: "laMarker Stroke",
            markerPadding: "laMarker Padding",
            itemPaddingX: "laItem Padding X",
            itemPaddingY: "laItem Padding Y",
            strokeWidth: "laStroke Width",
            offset: "laOffset",
            tooltips: "laTooltips",
            offsets: "laOffsets",
            callout: "laCallout",
            markers: "laMarkers",
            shadow: "laShadow",
            blur: "laBlur",
            xOffset: "laX Offset",
            yOffset: "laY Offset",
            lineWidth: "laLine Width",
            normal: "laNormal",
            bold: "laBold",
            italic: "laItalic",
            boldItalic: "laBold Italic",
            fillOpacity: "laFill Opacity",
            strokeOpacity: "laLine Opacity",
            columnGroup: "Column",
            barGroup: "Bar",
            pieGroup: "Pie",
            lineGroup: "Line",
            scatterGroup: "Scatter",
            areaGroup: "Area",
            groupedColumnTooltip: "laGrouped",
            stackedColumnTooltip: "laStacked",
            normalizedColumnTooltip: "la100% Stacked",
            groupedBarTooltip: "laGrouped",
            stackedBarTooltip: "laStacked",
            normalizedBarTooltip: "la100% Stacked",
            pieTooltip: "laPie",
            doughnutTooltip: "laDoughnut",
            lineTooltip: "laLine",
            groupedAreaTooltip: "laGrouped",
            stackedAreaTooltip: "laStacked",
            normalizedAreaTooltip: "la100% Stacked",
            scatterTooltip: "laScatter",
            bubbleTooltip: "laBubble",
            noDataToChart: "laNo data available to be charted.",
            pivotChartRequiresPivotMode:
                "laPivot Chart requires Pivot Mode enabled.",
            functionReadOnlyMode: "functionsReadOnlyMode"
        }
    }];
    let newlangText = langsText.filter(n => n.key == key);
    return newlangText[0].localeText;
}