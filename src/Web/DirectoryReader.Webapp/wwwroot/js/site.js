// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

/*
var app = {
    index: {
        uploadFile: function () {
            
            $("#flUpload").change(function () {
                //var input = document.getElementById(inputId);
                var files = this.files;
                var formData = new FormData();
                
                for (var i = 0; i !== files.length; i++) {
                    formData.append("files", files[i]);
                }

                $(".content").each(function (x, y) {
                    formData.append($(y).attr("name"), $(y).val());
                    console.log($(y).val());
                });

                console.log(formData);

                $.ajax(
                    {
                        url: "/api/File",
                        data: formData,
                        processData: false,
                        contentType: false,
                        type: "POST",
                        success: function (data) {
                            console.log(data);
                            alert("File Uploaded!");
                        },
                        error: function (data) {
                            console.log(data);
                            alert("File Upload Failed!");
                        }
                    }
                );
            });
        }
    }
};



$(document).ready(function () {
    app.index.uploadFile();
});

*/