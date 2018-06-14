class Answer {

    constructor(id, content, option, question, respondent) {
        this.id = id;
        this.content = content;
        this.optionId = option;
        this.questionId = question;
        this.respondentId = respondent;
    }

}

class Question {

    constructor(id, key, type, title, image, isRequired, options) {
        this.id = id;
        this.key = key;
        this.type = type;
        this.title = title;
        this.image = image;
        this.isRequired = isRequired;
        this.options = options;
    }

}

class Choice {

    constructor(key, id, label, value, image) {
        this.key = key;
        this.id = id;
        this.label = label;
        this.value = value;
        this.image = image;
    }
}


var application = new Vue({
    el: "#page-root",
    data: {
        user: model.user,
        counter: 0,
        survey: {
            id: model.id,
            title: model.title,
            description: model.description,
            startDate: model.startDate,
            endDate: model.endDate,
            isOpen: model.isOpen,
            isAuthenticationRequired: model.isAuthenticationRequired,
            creatorId: model.creatorId,
            questions: []
        },
        message: {
            status: "",
            content: ""
        },
        questionTypes: {
            1: "q-text",
            2: "q-choice",
            3: "q-multi-choice",
            4: "q-grid",
            5: "q-multi-grid",
            6: "q-date",
            7: "q-time",
            8: "q-select",
            9: "q-range"
        }
    },
    created: function () {
        if (model.questions) {
            for (var i = 0; i < model.questions.length; i++) {
                var q = model.questions[i];

                this.survey.questions.push(new Question(
                    q.id,
                    this.counter++,
                    this.questionTypes[q.typeId],
                    q.title,
                    q.image,
                    q.isRequired,
                    q.options
                ));
            }
        }

        $("#model").detach();
    },
    methods: {
        Format: function (date, format) {
            return $.format.date(date, format);
        },
        OnDeleteSurveyClicked: function () {

        },
        OnSendAnswersClicked: async function () {
            await form.validate();
            if (!form.valid) {
                this.message.status = "error";
                this.message.content = "Перед збереженням відповідей виправте всі помилки!";
                return;
            }

            if (this.user.id == null && this.survey.isAuthenticationRequired == true) {
                events.$emit("Authorize", {
                    message: "Для відправки відповідей необхідно авторизуватися.",
                    callback: this.OnSendAnswersClicked
                });
                return;
            }

            var result = [];
            for (var key in this.$refs) {
                var q = this.$refs[key][0];
                var r = q.result;
                if (r) {
                    result = result.concat(r);
                }
            }

            var data = {
                Url: `answers?surveyId=${this.survey.id }&userId=${this.user.id || -1}`,
                Method: "POST",
                Data: result
            };

            var self = this;

            var response = await $.ajax({
                url: "/api/call",
                method: "POST",
                data: JSON.stringify(data),
                contentType: "application/json"
            });

            console.log(response);

            if (response.isSuccessfull) {
                location.reload();
            } else {
                events.$emit("InformationDialog", {
                    title: "Помилка при виконанні запиту",
                    message: response.errors[0].msg,
                    isError: true
                });
            }
        }
    }
});

