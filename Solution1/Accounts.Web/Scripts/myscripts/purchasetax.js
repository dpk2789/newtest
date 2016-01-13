$(function () {
    $("a[data-modal=purchasetax]").on("click", function () {
        $("#purchasetaxModalContent").load(this.href, function () {
            $("#purchasetaxModal").modal({ keyboard: true }, "show");

            $("#purchasetax").submit(function () {
                if ($("#purchasetax").valid()) {
                    $.ajax({
                        url: this.action,
                        type: this.method,
                        data: $(this).serialize(),
                        success: function (result) {
                            if (result.success) {
                                $("#purchasetaxModal").modal("hide");
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

            $("#TaxCode").autocomplete({
                minLength: 1,
                source: function (request, response) {
                    var url = $(this.element).data("url");
                    $.getJSON(url, { term: request.term }, function (data) {
                        response(data);
                    });
                },
                appendTo: $(".modal-body"),
                select: function (event, ui) {
                    $("#Name").val(ui.item.Name);
                    $("#Percent").val(ui.item.Percent);
                },
                change: function (event, ui) {
                    if (!ui.item) {
                        $(event.target).val("");
                    }
                }
            });

        });
        return false;
    });
});
