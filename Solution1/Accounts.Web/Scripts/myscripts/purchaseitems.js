$(function () {
    $("a[data-modal=purchaseitem]").on("click", function () {
        $("#purchaseitemModalContent").load(this.href, function () {
            $("#purchaseitemModal").modal({ keyboard: true }, "show");

            $("#purchaseitem").submit(function () {
                if ($("#purchaseitem").valid()) {
                    $.ajax({
                        url: this.action,
                        type: this.method,
                        data: $(this).serialize(),
                        success: function (result) {
                            if (result.success) {
                                $("#purchaseitemModal").modal("hide");
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

            $("#Code").autocomplete({
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
                    $("#Quantity").val(ui.item.Quantity);
                    $("#UnitPrice").val(ui.item.UnitPrice);
                    $("#UnitId").val(ui.item.UnitId);
                    recalculateExtendedPrice();
                    $("#Quantity").select();

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


function recalculateExtendedPrice() {
    var quantity = parseFloat(document.getElementById("Quantity").value).toFixed(2);
    var unitPrice = parseFloat(document.getElementById("UnitPrice").value).toFixed(2);

    if (isNaN(quantity)) {
        quantity = 0;
    }

    if (isNaN(unitPrice)) {
        unitPrice = 0;
    }

    document.getElementById("Quantity").value = quantity;
    document.getElementById("UnitPrice").value = unitPrice;

    document.getElementById("ExtendedPrice").value = numberWithCommas((quantity * unitPrice).toFixed(2));
}


function numberWithCommas(n) {
    return n.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}

jQuery.fn.exists = function () {
    return this.length > 0;
}