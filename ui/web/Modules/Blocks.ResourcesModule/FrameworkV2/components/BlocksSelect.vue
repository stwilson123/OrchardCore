<template>
  <el-select
    ref="theselect"
    v-model="vmodel"
    :class="newClass"
    :popper-class="popperClass"
    :multiple="multiple"
    :disabled="disabled"
    :size="size"
    :clearable="clearable"
    :collapse-tags="collapseTags"
    :multiple-limit="multipleLimit"
    :name="name"
    :autocomplete="autocomplete"
    :placeholder="placeholder"
    :filterable="filterable"
    :allow-create="allowCreate"
    :filter-method="filterMethod"
    :remote="remote"
    :loading="blocksLocalLoading"
    :remote-method="blocksRemoteMethod"
    :loading-text="loadingText"
    :no-match-text="noMatchText"
    :no-data-text="noDataText"
    :reserve-keyword="reserveKeyword"
    :popper-append-to-body="popperAppendToBody"
    @change="changeEvent"
    @visible-change="visibleChangeEvent"
    @remove-tag="removeTagEvent"
    @clear="clearEvent"
    @blur="blurEvent"
    @focus="focusEvent"
  >
    <fragement ref="theScrollContainer" v-if="remote">
      <el-option
        :key="item.id"
        :label="item.text"
        :value="item.id"
        infinite-scroll-immediate="false"
        v-for="item in dataSource"
        v-infinite-scroll="loadMoreMethod"
      ></el-option>
      <template v-if="isShowLoadingMore">
        <slot name="empty" v-if="$slots.empty"></slot>
        <p class="el-select-dropdown__empty" v-else>{{ emptyText }}</p>
      </template>
    </fragement>
    <fragement v-else>
      <el-option
        :key="item[itemValue]"
        :label="item[itemText]"
        :value="item[itemValue]"
        v-for="item in dataSource"
      ></el-option>
    </fragement>
  </el-select>
</template>
<script>
import { value, computed, watch } from "vue-function-api";
export default {
  name: "BlocksSelect",
  // data() {
  //   return {
  //     vmodel: this.value
  //     //params: {}
  //   };
  // },
  computed: {
    emptyText: function() {
      if (this.blocksRemoteLoading === true || !this.$refs.theselect) {
        return this.loadingText || this.$t("el.select.loading"); //this.t("el.select.loading");
      } else {
        let query = this.$refs.theselect.$el.querySelector("input").value;
        if (
          this.blocksRemoteLoading === true &&
          query === "" &&
          this.dataSource.length === 0
        )
          return false;
        // if (
        //   props.filterable &&
        //   query &&
        //   dataSource.length > 0 &&
        //   this.filteredOptionsCount === 0
        // ) {
        //   return props.noMatchText || "el.select.noMatch"//this.t("el.select.noMatch");
        // }
        if (this.dataSource.length === 0) {
          return this.noDataText || this.$t("el.select.noData"); //this.t("el.select.noData");
        }
      }
      return this.loadingText || this.$t("el.select.loading");
    }
  },
  setup(props, context) {
    let dataSource = props.optionsData ? value(props.optionsData) : value([]);
    let newDisabled = value(props.disabled);
    let newClass = value(props.selectClass);
    let blocksLocalLoading = props.remoteMethod
      ? value(props.loading)
      : value(false);
    let blocksRemoteLoading = value(false);
    let pageSize = value(10);
    let currentPage = value(0);
    let api = {};
    let vmodel = value(props.value);
    let isShowLoadingMore = value(false);
    let hasLoadingMoreData = true;
    // let setDisabled = b => {
    //   newDisabled.value = b;
    // };
    let setClass = name => {
      newClass.value = name;
    };
    let setData = data => {
      if (blocksRemoteLoading.value === true) {
        if (data && Array.isArray(data)) {
          dataSource.value = dataSource.value.concat(data);
        }
        setDataLoadMoreDataEvent(data);
      } else {
        dataSource.value = data;
        setDataEvent(data);
      }
    };
    let setDataLoadMoreDataEvent = data => {
      if (
        typeof data === undefined ||
        data === null ||
        (Array.isArray(data) && data.length < 1)
      ) {
        hasLoadingMoreData = false;
        isShowLoadingMore.value = false;
      }
    };
    let hasScrolled = (el, direction = "vertical") => {
      if (direction === "vertical") {
        return el.scrollHeight > el.clientHeight;
      } else if (direction === "horizontal") {
        return el.scrollWidth > el.clientWidth;
      }
    };
    let setDataEvent = data => {
      if (props.remote === true) {
        setTimeout(() => {
          if (
            hasScrolled(
              context.refs.theScrollContainer.parentElement.parentElement
            )
          )
            isShowLoadingMore.value = true;
        }, 10);
      }
    };
    let changeEvent = e => {
      context.emit("blocks-change", e);
    };
    let visibleChangeEvent = e => {
      context.emit("blocks-visible", e);
    };
    let removeTagEvent = e => {
      context.emit("blocks-remove", e);
    };
    let clearEvent = () => {
      context.emit("blocks-clear");
    };
    let blurEvent = e => {
      context.emit("blocks-blur", e);
    };
    let focusEvent = e => {
      context.emit("blocks-focus", e);
    };
    let focus = () => {
      context.refs.theselect.focus();
    };
    let blur = () => {
      context.refs.theselect.blur();
    };
    let blocksRemoteMethod = (args, loadMoreParams) => {
      if (!loadMoreParams) {
        blocksLocalLoading.value = true;
        currentPage.value = 0;
        let queryParams = {
          url: props.remoteUrl,
          api
        };
        let newParams = Object.assign({}, props.remoteParams, {
          page: {
            pageSize: pageSize.value,
            page: currentPage.value + 1,
            filters: {
              groupOp: "AND",
              rules: [{ field: "Text", op: "cn", data: args }]
            }
          }
        });
        queryParams.data = newParams;
        props.remoteMethod(args, queryParams, () => {
          blocksLocalLoading.value = false;
        });
        return;
      }
      if (loadMoreParams && loadMoreParams.isLoadMore) {
        let queryParams = {
          url: props.remoteUrl,
          api
        };
        let newParams = Object.assign({}, props.remoteParams, {
          page: {
            pageSize: pageSize.value,
            page: currentPage.value + 1,
            filters: {
              groupOp: "AND",
              rules: [{ field: "Text", op: "cn", data: args }]
            }
          }
        });
        queryParams.data = newParams;
        props.remoteMethod(args, queryParams, () => {
          loadMoreParams.asyncCallback();
        });
        return;
      }
      if (
        loadMoreParams &&
        (loadMoreParams.isInitializeSelectData ||
          loadMoreParams.isInitializeData)
      ) {
        let queryParams = {
          url: props.remoteUrl,
          api
        };
        let newParams = Object.assign({}, props.remoteParams, {
          page: {
            page: 1,
            filters: {
              groupOp: "AND",
              rules:
                loadMoreParams.isInitializeSelectData === true && props.remote
                  ? [{ field: "Id", op: "eq", data: loadMoreParams.params }]
                  : []
            }
          }
        });
        queryParams.data = newParams;
        let callback;
        let callbackPromise = new Promise(resolve => {
          callback = resolve;
        });
        props.remoteMethod(args, queryParams, () => {
          if (loadMoreParams.isInitializeSelectData === true) {
            vmodel.value = props.multiple
              ? [loadMoreParams.params]
              : loadMoreParams.params;
          }

          callback();
        });
        return callbackPromise;
      }
    };
    let loadMoreMethod = () => {
      if (props.remote !== true || blocksRemoteLoading.value === true) return;
      blocksRemoteLoading.value = true;
      let scrollEl =
        context.refs.theScrollContainer.parentElement.parentElement;

      if (
        scrollEl.scrollHeight - scrollEl.scrollTop <=
        scrollEl.clientHeight + 10
      ) {
        let query = context.refs.theselect.$el.querySelector("input").value;

        blocksRemoteMethod(query, {
          isLoadMore: true,
          asyncCallback: () => {
            blocksRemoteLoading.value = false;
            currentPage.value++;
            context.parent.$nextTick(() => {});
          }
        });
      } else {
        blocksRemoteLoading.value = false;
      }
    };
    let initializeData = async params => {
      let options = Object.assign(
        {
          isInitializeSelectData: false
        },
        params
      );
      return await blocksRemoteMethod(undefined, {
        isInitializeSelectData: options.isInitializeSelectData,
        isInitializeData: true,
        params: options.data
      });
    };
    api = {
      vmodel,
      dataSource,
      setData,
      setClass,
      //newDisabled,
      newClass,
      //setDisabled,
      changeEvent,
      visibleChangeEvent,
      removeTagEvent,
      clearEvent,
      blurEvent,
      focusEvent,
      focus,
      blur,
      loadMoreMethod,
      blocksRemoteMethod,
      blocksLocalLoading,
      blocksRemoteLoading,
      initializeData,
      isShowLoadingMore
      // emptyText
    };
    return api;
  },
  props: {
    value: {
      type: Object
    },
    multiple: {
      type: Boolean,
      default: () => false
    },
    disabled: {
      type: Boolean,
      default: () => false
    },
    size: {
      type: String
    },
    clearable: {
      type: Boolean,
      default: () => false
    },
    collapseTags: {
      type: Boolean,
      default: () => false
    },
    multipleLimit: {
      type: Number,
      default: () => 0
    },
    name: {
      type: String
    },
    autocomplete: {
      type: String,
      default: () => "off"
    },
    placeholder: {
      type: String
    },
    filterable: {
      type: Boolean,
      default: () => false
    },
    allowCreate: {
      type: Boolean,
      default: () => false
    },
    filterMethod: {
      type: Function
    },
    remote: {
      type: Boolean,
      default: () => false
    },
    remoteMethod: {
      type: Function
    },
    loading: {
      type: Boolean,
      default: () => false
    },
    loadingText: {
      type: String,
      default: () => "加载中"
      // default: undefined
    },
    noMatchText: {
      type: String,
      default: () => "无匹配数据"
      //default: undefined
    },
    noDataText: {
      type: String,
      default: () => "无数据"
      //default: undefined
    },
    popperClass: {
      type: String
    },
    selectClass: {
      type: String
    },
    reserveKeyword: {
      type: Boolean,
      default: () => false
    },
    popperAppendToBody: {
      type: Boolean,
      default: () => true
    },
    optionsData: {
      type: Array
    },
    itemValue: {
      type: String,
      default: "id"
    },
    itemText: {
      type: String,
      default: "text"
    },
    remoteUrl: {
      type: String
    },
    remoteParams: {
      type: Object,
      default: () => {}
    }
  },
  watch: {
    value(newVal) {
      this.vmodel = newVal;
    },
    vmodel(newVal) {
      this.$emit("input", newVal);
    },
    optionsData(newVal) {
      this.dataSource = newVal;
    }
  },
  methods: {
    getData() {
      return this.dataSource;
    }
  }
  // methods: {
  //   getValue() {
  //     return this.vmodel;
  //   }
  // },
  // created() {
  //   if (this.params.value) {
  //     this.vmodel = this.params.value;
  //     this.setClass("blocks-select-grid");
  //   }
  //   if (this.params.optionsData) {
  //     this.setData(this.params.optionsData);
  //   }
  //   if (this.params.multiple) {
  //     this.multiple = this.params.multiple;
  //   }
  // }
};
</script>