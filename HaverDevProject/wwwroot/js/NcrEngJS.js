$(document).ready(function () {
    GetNcrs();
});

function GetNcrs() {
    $.ajax({
        url: '/NcrOperation/GetNcrs',
        type: 'GET',
        dataType: 'json',
        contentType: 'application/json;charset=utf-8',
        success: function (response) {
            if (response.length === 0) {
                $('#tblBody').html('<tr><td colspan="3">No New NCRs available</td></tr>');
            } else {
                $('#tblBody').empty(); // Clear existing rows
                for (let i = 0; i < response.length; i++) {
                    const ncrEng = response[i];
                    const $row = $('<tr>');
                    $row.append(`<td>${ncrEng.ncrNumber}</td>`);
                    $row.append(`<td>${ncrEng.ncrEngDispositionDescription}</td>`);
                    $row.append(`<td><button class="btn btn-success" onclick="startNcrOperation('${ncrEng.ncrNumber}')"><i class="bi bi-play-fill"></i> Start</button></td>`);
                    $('#tblBody').append($row);
                }
            }
        },
        error: function () {
            alert('Error fetching data.'); // Provide more specific error message if possible
        }
    });
}

function startNcrOperation(ncrNumber) {
    // You can now use the ncrNumber to create the new NcrOperation
    // For example, redirect to the Create action with the NcrNumber parameter
    window.location.href = '/NcrOperation/Create?ncrNumber=' + ncrNumber;
}