var namespace = namespace || {};
namespace.Account = namespace.Account || {};

namespace.Account.Form = (function (){

    var onFormLoad = function (exContext) {
        var formType = exContext.getFormContext();
    };

    return {
        onFormLoad: onFormLoad,
    }
})();