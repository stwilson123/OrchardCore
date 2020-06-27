import $ from 'jquery'

export function ajaxRequest(options){
    $.ajax({
        type: options.type ? options.type : "get",
        timeout: 50000,
        async: options.async ? options.async : false,
        cache: false,
        dataType: "json",
        url: options.url,
        data: options.params,
        success: options.success,
        error: options.error
    });
}
