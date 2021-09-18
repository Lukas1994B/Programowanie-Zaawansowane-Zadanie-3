const uri = "https://localhost:44354/api/People";
let people = null;
function getCount(data) {
    const el = $("#counter");
    let name = "osoba";
    if (data) {
        if (data > 1) {
            name = "osoby";
        }
        el.text(data + " " + name);
    } else {
        el.text("Brak osób");
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
            const tBody = $("#people");
            $(tBody).empty();
            getCount(data.length);
            let counter = 1;
            $.each(data, function (key, item) {
                const tr = $("<tr></tr>")
                    .append($(`<td>Pozycja ${counter}: ${item.firstName} ${item.lastName} </td>`))
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
            people = data;
        }
    });
}
function addItem() {
    const item = {
        firstName: $("#add-firstname").val(),
        lastName: $("#add-lastname").val()
    };
    $.ajax({
        type: "POST",
        accepts: "application/json",
        url: uri + '/CreatePersonItem',
        contentType: "application/json",
        data: JSON.stringify(item),
        error: function (jqXHR, textStatus, errorThrown) {
            alert("Something went wrong!");
        },
        success: function (result) {
            getData();
            $("#add-firstname").val("");
            $("#add-lastname").val("");
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
    $.each(people, function (key, item) {
        if (item.id === id) {
            $("#edit-firstname").val(item.firstName);
            $("#edit-lastname").val(item.lastName);
            $("#edit-id").val(item.id);
        }
    });
    $("#spoiler").css({ display: "block" });
}

function updateItem() {
    var id = parseInt($("#edit-id").val(), 10);
    const item = {
        id: id,
        firstName: $("#edit-firstname").val(),
        lastName: $("#edit-lastname").val(),
    };
    $.ajax({
        type: "POST",
        accepts: "application/json",
        url: uri + '/UpdatePersonItem',
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