define([], (function () {

    var definedExemplar = function () {
    }

    definedExemplar.prototype.inherits = function (subExemplar, superExemplar) {
        var subPrototype = Object.create(superExemplar.prototype);

        // At the very least, we keep the "constructor" property
        // At most, we keep additions that have already been made
        $.extend(subPrototype, subExemplar.prototype);
        subExemplar.prototype = subPrototype;

        subExemplar._super = superExemplar.prototype;
    };

    return definedExemplar;
}));