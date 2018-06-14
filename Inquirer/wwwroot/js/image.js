class ImageInputReader {

    constructor(input, resize = true, maxWidth = 800, maxHeight = 600) {
        this.input = input;
        this.resize = resize;
        this.maxWidth = maxWidth;
        this.maxHeight = maxHeight;
    }

    async Run(cleanup = false) {
        this.image = await this.Read();
        if (this.resize === true) {
            this.resizedImage = await this.Resize();
        }
        if (cleanup === true) {
            $(`#${this.input}`).parent().find("canvas").detach();
            $(`#${this.input}`).unwrap();
        }
        return this.resizedImage || this.image;
    }

    Read() {
        return new Promise((resolve, reject) => {
            var self = this;

            var file = $(`#${self.input}`)[0].files[0];
            var reader = new FileReader();

            reader.onload = () => {
                resolve(reader.result);
            };

            reader.readAsDataURL(file);
        });
    }

    Resize() {
        return new Promise(async (resolve, reject) => {
            var self = this;

            var image = await this.CreateImage(this.image);

            var imageUrl = image.src;

            var form = $("<form></form>");
            $(`#${self.input}`).wrap(form);

            if (image.width > self.maxWidth || image.height > self.maxHeight) {

                var canvas = $("<canvas></canvas>");
                canvas.insertBefore(`#${self.input}`);

                var context = canvas[0].getContext("2d");
                context.clearRect(0, 0, canvas.width, canvas.height);
                context.drawImage(image, 0, 0);

                var width = image.width;
                var height = image.height;
                if (width > height) {
                    if (width > self.maxWidth) {
                        height *= self.maxWidth / width;
                        width = self.maxWidth;
                    }
                } else {
                    if (height > self.maxHeight) {
                        width *= self.maxHeight / height;
                        height = self.maxHeight;
                    }
                }
                canvas.width = width;
                canvas.height = height;

                context = canvas[0].getContext("2d");
                context.drawImage(image, 0, 0, width, height);

                resolve(canvas[0].toDataURL("image/png"));

            }

            form[0].reset();

            resolve(image.src);
        });
    }

    CreateImage(source) {
        return new Promise((resolve, reject) => {
            var image = new Image();

            image.onload = () => {
                resolve(image);
            };

            image.src = source;
        });
    }
}