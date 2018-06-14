class Question {

    constructor(list, key, type, isRequired, data) {
        this.list = list;
        this.key = key;
        this.type = type;
        this.data = data;
        this.isRequired = isRequired;
    }

    get index() {
        return this.list.indexOf(this);
    }
}

class Choice {

    constructor(list, key, label, value, image = { data: null, filename: "" }, isCustom = false) {
        this.list = list;
        this.key = key;
        this.label = label;
        this.value = value;
        if (image == null) {
            this.image = {
                data: null,
                filename: ""
            };
        } else {
            this.image = image;
        }
        this.isCustom = isCustom;
    }

    get index() {
        return this.list.indexOf(this);
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
            questions: []
        },
        message: "",
        questionTypes: {
            "q-text": {
                id: 1,
                component: null,
                label: "Текстова відповідь"
            },
            "q-choice": {
                id: 2,
                component: "q-choice",
                label: "Варіант відповіді"
            },
            "q-multi-choice": {
                id: 3,
                component: "q-choice",
                label: "Кілька варіантів відповіді"
            },
            "q-grid": {
                id: 4,
                component: "q-grid",
                label: "Сітка відповідей"
            },
            "q-multi-grid": {
                id: 5,
                component: "q-grid",
                label: "Сітка відповідей (кілька відповідей)"
            },
            "q-date": {
                id: 6,
                component: null,
                label: "Вибір дати"
            },
            "q-time": {
                id: 7,
                component: null,
                label: "Вибір часу"
            },
            "q-select": {
                id: 8,
                component: "q-choice",
                label: "Випадаючий список"
            },
            "q-range": {
                id: 9,
                component: "q-range",
                label: "Шкала значень"
            }
        }
    },
    created: function () {
        if (model.questions) {
            for (var i = 0; i < model.questions.length; i++) {
                var q = model.questions[i];

                var type = "";
                var j = 0;
                for (var t in this.questionTypes) {
                    if (this.questionTypes[t].id == q.typeId) {
                        type = Object.keys(this.questionTypes)[j];
                        break;
                    }
                    j++;
                }

                this.survey.questions.push(new Question(this.survey.questions, this.counter++, type, q.isRequired, {
                    id: q.id,
                    title: q.title,
                    image: q.image,
                    options: q.options
                }));
            }
        }

        $("#model").detach();
    },
    methods: {
        OnCreateQuestionClicked: function (type) {
            this.survey.questions.push(new Question(this.survey.questions, this.counter++, type, false, null));
        },
        OnDeleteQuestionClicked: function (i) {
            this.survey.questions.splice(i, 1);
        },
        OnCopyQuestionClicked: function (i) {
            var q = this.questions[i];
            var copy = new Question(this.survey.questions, this.counter++, q.type, q.isRequired, q.data);

            this.survey.questions.splice(i + 1, 0, copy);
        },
        OnSaveClicked: async function () {

            await form.validate();

            if (new Date(this.survey.startDate) >= new Date(this.survey.endDate)) {
                this.$validator.errors.add("startDate", "Дата початку повинна бути раніше за дату закінчення.", "asd");
            }

            if (!form.valid) {
                return;
            }

            var survey = {
                id: this.survey.id,
                title: this.survey.title,
                description: this.survey.description,
                startDate: this.survey.startDate,
                endDate: this.survey.endDate,
                isOpen: this.survey.isOpen,
                isAuthenticationRequired: this.survey.isAuthenticationRequired
            };

            var q = [];
            for (var i = 0; i < this.survey.questions.length; i++) {
                var key = this.survey.questions[i].key;
                var value = this.$refs[`question[${key}]`][0].result;
                q.push(value);
            }
            survey.questions = q;
            console.log(survey);

            var data = {
                Url: `surveys/${this.survey.id}`,
                Method: "PUT",
                Data: survey
            };

            var self = this;

            var response = await $.ajax({
                url: "/api/call",
                method: "POST",
                data: JSON.stringify(data),
                contentType: "application/json",
                error: function (data) {
                    console.log(data);
                    self.message = "Сталась помилка";
                }
            });

            console.log(response);

            if (response.isSuccessfull) {
                var now = new Date();
                this.message = `Збережно о ${now.getHours()}:${now.getMinutes()}:${now.getSeconds()}`;
                events.$emit("InformationDialog", {
                    title: "Запит виконано",
                    message: "Опитування успішно збережено!",
                    isError: false
                });
            }
        },
        OnDeleteSurveyClicked: function () {

        }
    }
});

