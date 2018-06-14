var application = new Vue({
    el: "#page-root",
    data: {
        user: {
            id: model.id,
            name: model.name,
            email: model.email,
            password: null,
            passwordConfirmation: null,
            sex: model.sex,
            educationProgress: model.educationProgress,
            educationType: model.educationType,
            language: model.language,
            occupation: model.occupation,
            location: model.location,
            image: ""
        },
        enumerations: model.enumerations
    },
    created: function () {
        $("#model").detach();
    },
    mounted: function () {
        var self = this;

        var data = {
            Url: `accounts/${this.user.id}/image`,
            Method: "GET",
            Data: null
        };

        $.ajax({
            url: "/api/call",
            method: "POST",
            data: JSON.stringify(data),
            contentType: "application/json",
            success: function (response) {
                if (response.isSuccessfull) {
                    self.user.image = response.content.image;
                }
            }
        });
    },
    methods: {
        OnAccountIconOver: function () {
            events.$emit("ShowAccountPanel");
        },
        OnAccountIconOut: function () {
            events.$emit("HideAccountPanel");
        },
        OnChangeImageClicked: function () {
            $("#image-input").click();
        },
        OnImageOver: function () {
            $("#image .popup").removeClass("hidden");
        },
        OnImageOut: function () {
            $("#image .popup").addClass("hidden");
        },
        OnSubmitFormClicked: async function () {
            if (this.$validator.errors.any()) {
                return;
            } else {
                this.$validator.errors.clear();
                this.$validator.validate();
            }

            if (this.user.password != "" && this.user.password != this.user.passwordConfirmation) {
                this.$validator.errors.add("password", "Пароль та його підтвердження не співпадають.", "validation");
                return;
            }

            var data = {
                Url: `accounts/${this.user.id}`,
                Method: "PUT",
                Data: {
                    name: this.user.name,
                    email: this.user.email,
                    password: this.user.password == "" ? null : this.user.password,
                    sex: this.user.sex,
                    educationProgressId: this.user.educationProgress,
                    educationTypeId: this.user.educationType,
                    languageId: this.user.language,
                    occupationId: this.user.occupation,
                    locationId: this.user.location,
                }
            };

            var response = await $.ajax({
                url: "/api/call",
                method: "POST",
                data: JSON.stringify(data),
                contentType: "application/json"
            });

            if (response.isSuccessfull) {
                var response2 = await $.ajax({
                    url: "/account/authorize",
                    method: "POST",
                    data: JSON.stringify(response.content),
                    contentType: "application/json"
                });

                if (response2.isSuccessfull) {
                    location.reload();
                }

            } else if (response.exception) {
                document.write(response.exception);
            } else {
                for (var i = 0; i < response.errors.length; i++) {
                    this.$validator.errors.add(response.errors[i].field, response.errors[i].msg, "server");
                }
            }
        },
        OnImageInputChanged: async function () {
            var self = this;
            var input = $("#image-input")[0];
            if (input.files.length > 0) {
                var reader = new ImageInputReader("image-input", true, 500, 500);
                var image = await reader.Run();

                var id = self.user.id;
                var data = {
                    Url: `accounts/${id}/image`,
                    Method: "PUT",
                    Data: {
                        Image: `${image}`
                    }
                };

                $.ajax({
                    url: "/api/call",
                    method: "POST",
                    data: JSON.stringify(data),
                    contentType: "application/json",
                    success: function (data) {
                        self.user.image = image;
                    }
                });
            }
        }
    }
});