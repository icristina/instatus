function chain() {
    var a = arguments;
    return function () {
        for (var i in a) {
            if ($.isFunction(a[i])) a[i]();
        }
    };
}

function contains(array, item) {
    return array.indexOf(item) != -1;
}

function remove(array, item) {
    var index = array.indexOf(item);

    if (index != -1) {
        array.splice(index, 1);
    }
}