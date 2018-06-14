var application = new Vue({
    el: "#page-root",
    data: {
        user: model.user,
        statistics: model.statistics,
        surveys: model.surveys
    },
    created: function () {
        $("#model").detach();
    },
    methods: {
        Format: function (date, format) {
            return $.format.date(date, format);
        },
        OnCreateSurveyClicked: async function () {

            var data = {
                Url: `surveys`,
                Method: "POST",
                Data: {
                    CreatorId: this.user.id
                }
            }

            var response = await $.ajax({
                url: "/api/call",
                method: "POST",
                data: JSON.stringify(data),
                contentType: "application/json"
            });

            console.log(response);
            location.href = `/survey/${response.content}/edit`;

        }
    }
});