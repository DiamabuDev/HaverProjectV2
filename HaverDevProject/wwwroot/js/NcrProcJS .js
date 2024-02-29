$(document).ready(function () {
    GetNcrs();
});

function GetNcrs() {
    $.ajax({
        url: '/NcrReInspect/GetNcrs',
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
                    $row.append(`<td>${ncrEng.supplierName}</td>`);//Supplier
                    $row.append(`<td><button class="btn btn-success" onclick="startNcrProc('${ncrEng.ncrNumber}')"><i class="bi bi-play-fill"></i> Start</button></td>`);
                    $('#tblBody').append($row);
                }
            }
        },
        error: function () {
            alert('Error fetching data.'); // Provide more specific error message if possible
        }
    });
}

function startNcrProc(ncrNumber) {
    window.location.href = '/NcrReInspect/Create?ncrNumber=' + ncrNumber;
}