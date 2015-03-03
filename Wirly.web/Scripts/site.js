var wirly = (function () {
    function deleteDoc(id) {
        var data = JSON.stringify({
            id: id
        });

        console.log(data);
        $.ajax({
            url: "/project/deletedocument",
            data: data,
            contentType: "application/json; charset=utf-8",
            type: "POST"
        }).success(function () {
            window.location.reload();
        });
    }

    return {
        DeleteDoc: deleteDoc
    };

})();