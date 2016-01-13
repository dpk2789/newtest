$(function () {
    $("a[data-modal=issueitems]").on("click", function () {
        $("#issueitemsModalContent").load(this.href, function () {
            $("#issueitemsModal").modal({ keyboard: true }, "show");

            $("#issueitems").submit(function () {
                if ($("#issueitems").valid()) {
                    $.ajax({
                        url: this.action,
                        type: this.method,
                        data: $(this).serialize(),
                        success: function (result) {
                            if (result.success) {
                                $("#issueitemsModal").modal("hide");
                                location.reload();
                            } else {
                                $("#MessageToClient").text("The data was not updated.");
                            }
                        },
                        error: function () {
                            $("#MessageToClient").text("The web server had an error.");
                        }
                    });
                    return false;
                }
            });


        });
        return false;
    });
});