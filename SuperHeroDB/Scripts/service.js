function getAllHeros(showAll) {
    if (showAll)
        $("#searchText").val("");

    const search = $("#searchText").val() || "";
    $.ajax({
        url: 'Service/SuperHeroService.svc/GetAllHeros' + "?search=" + search,
        type: 'GET',
        dataType: 'json',
        success: function (result) {
            heroes = result;
            drawHeroTable(result);
        }
    });
}

function addHero() {
    var newHero = {
        FirstName: $("#addFirstname").val(),
        LastName: $("#addLastname").val(),
        HeroName: $("#addHeroname").val(),
        PlaceOfBirth: $("#addPlaceOfBirth").val(),
        Combat: $("#addCombatPoints").val()
    };

    $.ajax({
        url: 'Service/SuperHeroService.svc/AddHero',
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify(newHero),
        success: function () {
            getAllHeros();
            showOverview();
        }
    });
}

function UpdatingHeroDetails() {
    updateHero.FirstName = $("#updateFirstname").val();
    updateHero.LastName = $("#updateLastname").val();
    updateHero.HeroName = $("#updateHeroname").val();
    updateHero.PlaceOfBirth = $("#updatePlaceOfBirth").val();
    updateHero.Combat = $("#updateCombatPoints").val();

    $.ajax({
        url: 'Service/SuperHeroService.svc/UpdateHero/' + updateHero.Id,
        type: 'PATCH',
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify(updateHero),
        success: function () {
            getAllHeros();
            showOverview();
        }
    });
}

function deleteHero(heroId) {
    $.ajax({
        url: 'Service/SuperHeroService.svc/DeleteHero/' + heroId,
        type: 'DELETE',
        success: function () {
            getAllHeros();
        }
    });
}