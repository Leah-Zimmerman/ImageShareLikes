$(() => {

    const id = $(".row").data('id');
    viewImage();
    setInterval(viewImage, 1000);
    function viewImage() {
        $.get("/home/displayimage", { id }, function (vm) {
            $(".bg-light").remove();
            $(".row").append(` <div class="col-md-8 offset-md-2 bg-light p-4 shadow">
                <h3>${vm.image.title}</h3>
        <img src="/uploads/${vm.image.fileName}" class="w-100" />
        <div>  <form method="post" action="/home/addtosession?id=${vm.image.id}">
                    <button data-image-id="${vm.image.id}" class="btn btn-primary likebutton" id="likebutton">Like</button>
                </form>            
            <h5>Current Likes: ${vm.image.views}</h5>
        </div>
        </div>`)
             var button = document.getElementById("likebutton");
            button.disabled = vm.disable;
        })
    }

})