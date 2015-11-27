var Common = Common || {};
Common.putCall = function (url, data, success, error) {
    $.ajax({
        type: 'PUT',
        url: url,
        dataType: 'json',
        data: JSON.stringify(data),
        contentType: 'application/json; charset=utf-8',
        success: success,
        error: error
    });
};

Common.getCall = function (url, data, success, error) {
    $.ajax({
        type: 'GET',
        url: url,
        dataType: 'json',
        data: (data == null) ? null : JSON.stringify(data),
        contentType: 'application/json; charset=utf-8',
        success: success,
        error: error
    });
};

Common.postCall = function (url, data, success, error) {
    $.ajax({
        type: 'POST',
        url: url,
        dataType: 'json',
        data: JSON.stringify(data),
        contentType: 'application/json; charset=utf-8',
        success: success,
        error: error
    });
};


Array.prototype.contains = function (needle) {
    for (i in this) {
        if (this[i] == needle) return true;
    }
    return false;
}

var callerParent
Common.CallerParentObject = function (parent) {
    callerParent = parent;
};

Common.AjaxCall = function (url, type, crossDomain, cache, paramsToService, dataType, contentType, processDataFlag, successCallBack, failureCallBack, errorCallBack, loaderShowCalBack, loaderHideCallBack) {
    if (loaderShowCalBack !== null && typeof loaderShowCalBack !== 'undefined') {
        loaderShowCalBack.call();
    }
    else {
        //AxactHandler.ShowLoadingImage.call();
    }
    if (contentType)
        contentType = contentType.indexOf('application/json') == "-1" ? 'application/json; charset=utf-8' : contentType;
    else
        contentType = false;
    if (dataType)
        dataType = dataType.indexOf('json') == "-1" ? 'json' : dataType;
    else
        dataType = false;
    jQuery.ajax({
        type: type,
        url: url,
        crossDomain: crossDomain,
        data: paramsToService,
        dataType: dataType,
        contentType: contentType,
        processData: processDataFlag,
        success: function (result) {
            if (successCallBack !== null && typeof successCallBack !== 'undefined') {
                successCallBack.call(undefined, result, callerParent);
            }
            if (loaderHideCallBack !== null && typeof loaderHideCallBack !== 'undefined') {
                loaderHideCallBack.call();
            }
            else {
                //AxactHandler.HideLoadingImage.call();
            }
        },
        failure: function (xhr) {
            if (failureCallBack !== null && typeof failureCallBack !== 'undefined') {
                failureCallBack.call(xhr);
            }
        },
        error: function (xhr) {
            if (errorCallBack !== null && typeof errorCallBack !== 'undefined') {
                errorCallBack.call(xhr);
            }
        },
        complete: function () {
        }
    });
};
