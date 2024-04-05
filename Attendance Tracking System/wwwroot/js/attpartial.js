// Initialize DataTable
$('#listTable').DataTable();

// Arrival form submission
$('.ArrForm').submit(function (event) {
    event.preventDefault(); // Prevent default form submission

    var formData = $(this).serialize(); // Serialize form data
    var form = $(this);

    $.post($(this).attr('action'), formData, function (response) {
        if (response.success) {
            // Show success message
            Swal.fire({
                icon: 'success',
                title: 'Success',
                text: 'Arrival Time Recorded Successfully'
            });
            // Hide Arrival and Absent buttons, show Leave button
            var row = form.closest('tr');
            row.find('.ArrBtn').hide();
            row.find('.AbsentBtn').hide();
            row.find('.LeaveBtn').show();
        } else {
            // Show error message
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Some Error Occured'
            });
        }
    });
});

// Leave form submission
$('.LeaveForm').submit(function (event) {
    event.preventDefault(); // Prevent default form submission

    var formData = $(this).serialize(); // Serialize form data
    var form = $(this);

    $.post($(this).attr('action'), formData, function (response) {
        if (response.success) {
            // Show success message
            Swal.fire({
                icon: 'success',
                title: 'Success',
                text: 'Leave Time Recorded Successfully'
            });
            // Hide the row containing the form after successful submission
            form.closest('tr').hide();
        } else {
            // Show error message
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Some Error Occured'
            });
        }
    });
});

// Absent form submission
$('.AbsentForm').submit(function (event) {
    event.preventDefault(); // Prevent default form submission

    var formData = $(this).serialize(); // Serialize form data
    var form = $(this);

    $.post($(this).attr('action'), formData, function (response) {
        if (response.success) {
            // Show success message
            Swal.fire({
                icon: 'success',
                title: 'Success',
                text: 'This Student Has Been Marked As Absent For Today'
            });
            // Hide the row containing the form after successful submission
            form.closest('tr').hide();
        } else {
            // Show error message
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Some Error Occurred'
            });
        }
    });
});
