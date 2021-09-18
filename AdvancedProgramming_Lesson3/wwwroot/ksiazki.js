const uri = "https://localhost:44354/api/Books";
let books = null;
function getCount(data) {
    const el = $("#counter");
    let name = "książka";
    if (data) {
        if (data > 1) {
            name = "książki";
        }
        el.text(data + " " + name);
    } else {
        el.text("Brak książek");
    }
}
$(document).ready(function () {
    getData();
});
function getData() {
    $.ajax({
        type: "GET",
        url: uri,
        cache: false,
        success: function (data) {
            const tBody = $("#books");
            $(tBody).empty();
            getCount(data.length);
            let counter = 1;
            $.each(data, function (key, item) {
                const tr = $("<tr></tr>")
                    .append($(`<td>Pozycja ${counter}: ${item.title} - ${item.author} | ${item.releaseCity}  </td>`))
                    .append(
                        $("<td></td>").append(
                            $("<button class=\"btn btn-warning\">Edycja</button>").on("click", function () {
                                editItem(item.id);
                            })
                        )
                    )
                    .append(
                        $("<td></td>").append(
                            $("<button class=\"btn btn-danger\">Usuń</button>").on("click", function () {
                                deleteItem(item.id);
                            })
                        )
                    );
                tr.appendTo(tBody);
                counter++;
            });
            books = data;
        }
    });
}
function addItem() {
    const item = {
        title: $("#add-title").val(),
        author: $("#add-author").val(),
        releaseCity: $("#add-releasecity").val()
    };
    $.ajax({
        type: "POST",
        accepts: "application/json",
        url: uri + '/CreateBookItem',
        contentType: "application/json",
        data: JSON.stringify(item),
        error: function (jqXHR, textStatus, errorThrown) {
            alert("Something went wrong!");
        },
        success: function (result) {
            getData();
            $("#add-title").val("");
            $("#add-author").val("");
            $("#add-releasecity").val("");
        }
    });
}

function deleteItem(id) {
    $.ajax({
        url: uri + "/" + id,
        type: "DELETE",
        success: function (result) {
            getData();
        }
    });
}

function editItem(id) {
    $.each(books, function (key, item) {
        if (item.id === id) {
            $("#edit-title").val(item.title);
            $("#edit-author").val(item.author);
            $("#edit-releasecity").val(item.releaseCity);
            $("#edit-id").val(item.id);
        }
    });
    $("#spoiler").css({ display: "block" });
}

function updateItem() {
    var id = parseInt($("#edit-id").val(), 10);
    const item = {
        id: id,
        title: $("#edit-title").val(),
        author: $("#edit-author").val(),
        releaseCity: $("#edit-releasecity").val()
    };
    $.ajax({
        type: "POST",
        accepts: "application/json",
        url: uri + '/UpdateBookItem',
        contentType: "application/json",
        data: JSON.stringify(item),
        error: function (jqXHR, textStatus, errorThrown) {
            alert("Something went wrong!");
        },
        success: function (result) {
            getData();
            closeInput();
        }
    });
}

function closeInput() {
    $("#spoiler").css({ display: "none" });
}