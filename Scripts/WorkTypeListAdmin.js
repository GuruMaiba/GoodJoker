$(document).ready(function () {
    // Поиск, создание и редактирование типов
    $("input[name='AddType']").keyup(function () {
        var val = $(this).val();
        var id = $("input[name='TypeId']").val();
        if (id == "0") {
            if (val.length > 0) {
                $(".list .item").each(function () {
                    if ($(this).children(".name").text().indexOf(val) == -1 && !$(this).hasClass("hidden")) {
                        $(this).addClass("hidden");
                    } else if ($(this).children(".name").text().indexOf(val) > -1 && $(this).hasClass("hidden")) {
                        $(this).removeClass("hidden");
                    }
                });
                $(".list .item.new").removeClass("hidden");
            } else {
                $(".list .item").removeClass("hidden");
                $(".list .item.new").addClass("hidden");
            }
        } else {
            $(".list .item#" + id).children(".name").text(val);
        }
    });

    // Выбор элемента
    $("body").on("click", ".list .item .name", function () {
        var parent = $(this).parents(".item");
        var placesId = "";

        if (!parent.hasClass("new")) {
            if (parent.hasClass("active")) {
                parent.removeClass("active");
            } else {
                parent.addClass("active");
            }

            var list = $(".list .item").each(function () {
                if ($(this).hasClass("active")) {
                    placesId += (placesId != "") ? ";" + $(this).attr("id") : $(this).attr("id");
                }
            });

            $("input[name='TypesId']").val(placesId);
        }
    });

    // Создание нового элемента
    $(".list .item.new").click(function () {
        var name = $("input[name='AddType']").val();
        if (name.length > 0) {
            $.ajax({
                url: linkAjax,
                type: "POST",
                data: {
                    __RequestVerificationToken: token,
                    Id: 0,
                    Name: name
                },
                success: function (id) {
                    if (id != "0") {
                        $(".list").append(`<div id="${id}" class="item"><div class="name">${name}</div> <div class="actions"><i class="icon-pencil-1 editItem"></i> <i class="icon-cancel delItem"></i></div></div>`);
                    } else {
                        alert("Проблема с именем!");
                    }
                    $("input[name='AddType']").val("");
                    $(".list .item").removeClass("hidden");
                    $(".list .item.new").addClass("hidden");
                },
                error: function () {
                    globalError();
                }
            });
        }
    });

    // Изменение элемента
    $("body").on("click", ".list .item .editItem", function () {
        var parent = $(this).parents(".item");
        $(".list .item").each(function () {
            if (!$(this).hasClass("hidden")) {
                $(this).addClass("hidden");
            }
        });
        parent.removeClass("hidden");
        $("input[name='TypeId']").val(parent.attr("id"));
        $("input[name='TypeName']").val(parent.children(".name").text());
        $("input[name='AddType']").val(parent.children(".name").text());
        parent.addClass("confirm");
        $(this).attr("class", "icon-check-1 confirmItem");
    });

    // Подтверждение изменения элемента
    $("body").on("click", ".list .item .confirmItem", function () {
        var parent = $(this).parents(".item");
        var id = parent.attr("id");
        var name = parent.children(".name").text();
        delConfirm(parent);
        if (name.length > 0) {
            $.ajax({
                url: linkAjax,
                type: "POST",
                data: {
                    __RequestVerificationToken: token,
                    Id: id,
                    Name: name
                },
                error: function () {
                    globalError();
                }
            });
        }
    });

    // Удаление элемента
    $("body").on("click", ".list .item .delItem", function () {
        var parent = $(this).parents(".item");
        var id = parent.attr("id");
        var name = $("input[name='TypeName']").val();
        if (parent.hasClass("confirm")) {
            parent.children(".name").text(name);
            delConfirm(parent);
        } else {
            $.ajax({
                url: linkAjax,
                type: "POST",
                data: {
                    __RequestVerificationToken: token,
                    Id: id,
                    Name: ""
                },
                success: function (data) {
                    if (data != "0") {
                        parent.remove();
                    }
                },
                error: function () {
                    globalError();
                }
            });
        }
    });
});