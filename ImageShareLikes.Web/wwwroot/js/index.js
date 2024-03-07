$(() => {
    GetImages();
    function GetImages() {
        $(".bg-light").remove();
        $.get("/home/getimages", function (images) {
            images.forEach(function (image) {
                $(".col-md-6").append(`<div class="p-4 shadow bg-light mb-3" style="margin-bottom:20px; text-align:center">
                    <a href="/home/viewimage?id=${image.id}">
                        <h3>${image.title}</h3>
                        <h5>${image.uploadDate}</h5>
                        <img style="width:400px;" src="/uploads/${image.fileName}">
                    </a>
                </div>`)
            })
        })
    }
})