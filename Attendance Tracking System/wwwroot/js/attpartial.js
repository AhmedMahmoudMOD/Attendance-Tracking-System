
            $('#listTable').DataTable();
            $('#ArrForm').submit(function (event) {
                event.preventDefault(); // Prevent default form submission

                var formData = $(this).serialize(); // Serialize form data

                $.post($(this).attr('action'), formData, function (response) {
                    if (response.success) {
                        Swal.fire({
                            icon: 'success',
                            title: 'Success',
                            text: 'Arrival Time Recorded Successfully'
                        });
                        $('#ArrBtn').hide();// Hide the button on successful operation
                        $('#LeaveBtn').show();// Show the button on successful operation
                    } else {
                        // show swal error message
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: 'Attendance Already Recorded'
                        });
                    }
                });
            });
             // Leave form submission
            $('#LeaveForm').submit(function (event) {
                event.preventDefault(); // Prevent default form submission

                var formData = $(this).serialize(); // Serialize form data
                var form = $(this);
                $.post($(this).attr('action'), formData, function (response) {
                    if (response.success) {
                        Swal.fire({
                            icon: 'success',
                            title: 'Success',
                            text: 'Leave Time Recorded Successfully'
                        });
                        form.closest('tr').hide();
                    } else {
                        // show swal error message
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: 'Leave Time Already Recorded'
                        });
                    }
                });
            });
  