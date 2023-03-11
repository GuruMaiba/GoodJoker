$(document).ready(function () {
    // Поиск населенного пункта
    $("input[name='CitySearch']").keyup(function () {
        if ($(this).val().length > 3) {
            $.ajax({
                url: '/Lottery/_LocalLimiter',
                type: 'POST',
                data: {
                    __RequestVerificationToken: token,
                    Search: $(this).val(),
                    CitiesID: $("input[name='CitiesId']").val(),
                    RegionsID: $("input[name='RegionsId']").val()
                },
                success: function (data) {
                    if (data != "") {
                        $(".listCity").html(data).slideDown();
                    }
                }
            });
        } else {
            $(".listCity").slideUp();
        }
    });

    // Закрытие списка
    $("input[name=CitySearch]").focusout(function () {
        setTimeout(function () {
            $(".listCity").slideUp();
        }, 200);
    });

    // Нажатие по городу
    $("body").on("click", ".listCity .cityItem", function () {
        $("input[name=CitySearch]").val("");
        if (!$(this).hasClass("addCity")) {
            var CityId = $(this).attr("cityId");
            var RegId = $(this).attr("regId");

            if (CityId > 0) {
                if ($("input[name=CitiesId]").val() != "") {
                    CityId = ";" + CityId;
                }
                $("input[name=CitiesId]").val($("input[name=CitiesId]").val() + CityId);
            } else {
                var allId = "";
                $(".listAddCity .cityItem").each(function () {
                    if (!$(this).hasClass("addCity")) {
                        var cID = $(this).attr("cityId");
                        var rID = $(this).attr("regId");
                        if (rID == RegId) {
                            $(this).remove();
                        } else {
                            allId += (allId == "") ? cID : ";" + cID;
                        }
                    }
                });
                if ($("input[name=RegionsId]").val() != "" && CityId) {
                    RegId = ";" + RegId;
                }
                $("input[name=CitiesId]").val(allId);
                $("input[name=RegionsId]").val($("input[name=RegionsId]").val() + RegId);
            }

            var clone = $(this).clone();
            clone.children(".reg").html(clone.children(".reg").text() + `<i class="icon-cancel delCity"></i>`);
            clone.appendTo(".listAddCity");
            $(this).parent().slideUp();
        }
    });

    // Удаление города
    $("body").on("click", ".delCity", function () {
        $(this).parents(".cityItem").remove();
        var citiesId = "";
        var regionsId = "";
        $(".listAddCity .cityItem").each(function () {
            if (!$(this).hasClass("addCity")) {
                var cID = $(this).attr("cityId");
                var rID = $(this).attr("regId");
                if (cID > 0) {
                    if (citiesId != "") {
                        citiesId += ";";
                    }
                    citiesId += cID;
                } else {
                    if (regionsId != "") {
                        regionsId += ";";
                    }
                    regionsId += rID;
                }
            }
        });
        $("input[name=CitiesId]").val(citiesId);
        $("input[name=RegionsId]").val(regionsId);
    });
});