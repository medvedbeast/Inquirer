var application = new Vue({
    el: "#page-root",
    data: {
        returnUrl: model.returnUrl,
        name: "",
        email: "",
        password: ""
    },
    created: function () {
        $("#model").detach();
    },
    methods: {
        OnRegisterClicked: async function () {
            await form.validate();
            if (!form.valid) {
                return;
            }

            var data = {
                email: this.email,
                password: this.password,
                name: this.name
            };

            var response = await $.ajax({
                url: `/account/register`,
                method: "POST",
                data: JSON.stringify(data),
                contentType: "application/json"
            });

            if (response.isSuccessfull) {
                location.href = this.returnUrl || "/";
            } else {
                for (var i = 0; i < response.errors.length; i++) {
                    this.$validator.errors.add(response.errors[i].field, response.errors[i].msg, "server");
                }
            }

        }
    }
});