Vue.use(VeeValidate);

const dictionary = {
    uk: {
        messages: {
            required: () => "Поле не може бути порожнім.",
            email: () => "Поле має містити валідну email-адресу.",
            between: (field, data) => `Значення має бути у проміжку [${data}]`
        }
    }
}

VeeValidate.Validator.localize(dictionary);
VeeValidate.Validator.localize("uk");

VeeValidate.Validator.extend("question_required", {
    validate: function (value) {
        if (value === "") {
            value = null;
        }
        if (Array.isArray(value)) {
            return Array.flatten(value).length > 0;
        } else {
            return value != null;
        }
    },
    getMessage: () => "Питання є обов'язковим для заповнення."
});

class Form {

    get valid() {
        if (application.$validator.errors.any()) {
            return false;
        }
        return true;
    }

    async validate() {
        application.$validator.errors.clear();
        await application.$validator.validateAll();
    }
}

var form = new Form();

Array.flatten = function (arr) {
    return arr.reduce(function (flat, toFlatten) {
        return flat.concat(Array.isArray(toFlatten) ? Array.flatten(toFlatten) : toFlatten);
    }, []);
}
