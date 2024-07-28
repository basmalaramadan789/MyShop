$(document).ready(function () {
    loaddata();
});
function loaddata() {
    dtble = $("#mytable").DataTable({
        "ajax": {
            "url": "/Admin/Order/GetData"

        },
        "columns": [
            { "data": "id" },
            { "data": "name" },
            { "data": "phonenumber" },
            { "data": "applicationuser.email" },
            { "data": "orderstatus" },
            { "data": "ordertotal" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                    <a href="/Admin/Order/Details?orderid=${data}" class="btn btn-success">Details</a>
                    
                    `
                }
            }

        ]

    });

}