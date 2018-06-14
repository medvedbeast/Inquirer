class Question {
    constructor(key, id, title, typeId, maximum, options) {
        this.key = key;
        this.id = id;
        this.title = title;
        this.typeId = typeId;
        this.maximum = maximum;
        this.options = options;
    }
}

var application = new Vue({
    el: "#page-root",
    data: {
        survey: model.survey,
        user: model.user,
        statistics: model.statistics,
        questions: []
    },
    created: function () {
        for (var i = 0; i < this.statistics.questions.length; i++) {
            var q = this.statistics.questions[i];
            this.questions.push(new Question(i, q.id, q.title, q.typeId, q.maximum, q.options));
        }
        $("#model").detach();
    },
    methods: {
        Format: function (date, format) {
            return $.format.date(date, format);
        }
    }
});

