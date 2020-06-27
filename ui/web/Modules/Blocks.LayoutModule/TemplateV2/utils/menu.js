import { ajaxRequest } from "./ajax";
import { Loading } from 'element-ui'
import $ from 'jquery'
import store from './../store'

export function getCollect() {
    let bilinUserID = localStorage.getItem("bilinUserID");
    let key = 'menuCollect_' + bilinUserID;
    let isFirst = store.getters.first_getmenu;
    if (isFirst) {
        return getRemoteCollect();
    }
    else {
        if (localStorage.getItem(key) != null) {
            let j = JSON.parse(localStorage.getItem(key));
            return j.filter((value, index, arr) => {
                let newTitle = getTitle(value.meta.uId)
                if (newTitle != '') {
                    value.meta.title = newTitle
                }
                return true;
            });
        }
        else {
            return getRemoteCollect();
        }
    }
}

export function setCollect(value) {
    let bilinUserID = localStorage.getItem("bilinUserID");
    let key = 'menuCollect_' + bilinUserID;
    let l = Loading.service({ target: $(".global-loading")[0], fullscreen: false, lock: true, spinner: 'el-icon-loading', background: "rgba(255,255,255,0)" });
    ajaxRequest({
        url: "/api/services/authentionModule/collect/add",
        type: "post",
        params: {
            UserAccount: bilinUserID,
            CollectJson: JSON.stringify(value)
        },
        success: function (res) {
        }
    });
    l.close();
    let j = JSON.stringify(value)
    localStorage.setItem(key, j)
}

export function removeCollect() {
    let bilinUserID = localStorage.getItem("bilinUserID");
    let key = 'menuCollect_' + bilinUserID;
    let l = Loading.service({ target: $(".global-loading")[0], fullscreen: false, lock: true, spinner: 'el-icon-loading', background: "rgba(255,255,255,0)" });
    ajaxRequest({
        url: "/api/services/authentionModule/collect/delete",
        type: "post",
        params: {
            UserAccount: bilinUserID,
            CollectJson: ''
        },
        success: function (res) {
        }
    });
    l.close();
    localStorage.removeItem(key)
}

function getRemoteCollect() {
    let bilinUserID = localStorage.getItem("bilinUserID");
    let key = 'menuCollect_' + bilinUserID;
    let collect = '';
    let l = Loading.service({ target: $(".global-loading")[0], fullscreen: false, lock: true, spinner: 'el-icon-loading', background: "rgba(255,255,255,0)" });
    ajaxRequest({
        url: "/api/services/authentionModule/collect/get",
        type: "post",
        params: {
            UserAccount: bilinUserID,
            CollectJson: ''
        },
        success: function (res) {
            collect = res.content;
        }
    });
    l.close();
    store.dispatch('setFristGetMenu');
    if (collect != "" && collect != null) {
        let j = JSON.parse(collect);
        localStorage.setItem(key, collect)
        return j.filter((value, index, arr) => {
            let newTitle = getTitle(value.meta.uId)
            if (newTitle != '') {
                value.meta.title = newTitle
            }
            return true;
        });
    }
    else {
        localStorage.setItem(key, "[]")
        return [];
    }
}

function getTitle(uId) {
    let menus = store.getters.sys_menus;
    for (let i of menus) {
        let child = i.Items.find((value, index, arr) => {
            if (value.uId == uId) {
                return true;
            }
        })
        if (!(typeof (child) == "undefined")) {
            return child.DisplayName;
        }
    }
    return '';
}

export function getVisit() {
    let bilinUserID = localStorage.getItem("bilinUserID");
    let visitKey = 'menuVisit_' + bilinUserID;
    if (localStorage.getItem(visitKey) != null) {
        let v = JSON.parse(localStorage.getItem(visitKey));
        return v.filter((value, index, arr) => {
            let newTitle = getTitle(value.meta.uId)
            if (newTitle != '') {
                value.meta.title = newTitle
            }
            return true;
        });
        //return v;
    }
    else {
        return [];
    }
}

export function setVisit(value) {
    let bilinUserID = localStorage.getItem("bilinUserID");
    let visitKey = 'menuVisit_' + bilinUserID;
    let v = JSON.stringify(value)
    return localStorage.setItem(visitKey, v)
}

export function removeVisit() {
    let bilinUserID = localStorage.getItem("bilinUserID");
    let visitKey = 'menuVisit_' + bilinUserID;
    localStorage.removeItem(visitKey)
}