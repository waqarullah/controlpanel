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
