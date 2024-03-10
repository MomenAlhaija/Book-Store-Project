$(document).ready(function () {
    function initializeRatingPopup(bookId) {
        Swal.fire({
            title: 'Please rate this book',
            html:
                '<div class="rating">' +
                '<i class="fas fa-star" data-value="1"></i>' +
                '<i class="fas fa-star" data-value="2"></i>' +
                '<i class="fas fa-star" data-value="3"></i>' +
                '<i class="fas fa-star" data-value="4"></i>' +
                '<i class="fas fa-star" data-value="5"></i>' +
                '</div>',
            showCancelButton: true,
            confirmButtonText: 'Rate',
            cancelButtonText: 'Cancel',
            showLoaderOnConfirm: true,
            preConfirm: () => {
                var rating = $('.rating .fas.fa-star.clicked').length;
                if (rating < 1 || rating > 5) {
                    Swal.showValidationMessage('Please select a rating between 1 and 5 stars');
                } else {
                    return rating;
                }
            }
        }).then((result) => {
            if (result.isConfirmed) {
                var rating = result.value;
                $.ajax({
                    url: '/Book/RateBook',
                    type: 'POST',
                    data: { bookId: bookId, rate: rating },
                    success: function (response) {
                        Swal.fire({
                            icon: 'success',
                            title: 'Thank you for your rating!',
                            text: 'Your rating has been successfully submitted.',
                        });
                    },
                    error: function (xhr, status, error) {
                    }
                });
            }
        });
    }

    $(".like").click(function () {
        var bookId = $(this).closest('.offer').find('.cartbtn').data('bookid');
        $.ajax({
            url: '/Book/CheckPurchase',
            type: 'POST',
            data: { bookId: bookId },
            success: function (response) {
                if (response.purchased) {
                    initializeRatingPopup(bookId);
                }
                else {
                    Swal.fire({
                        icon: 'info',
                        title: 'You must purchase the book to vote on it',
                    });
                }
            },
            error: function (xhr, status, error) {
                Swal.fire({
                    icon: 'error',
                    title: response.error,
                });
            }
        });
    });

    $(document).on('click', '.rating .fas.fa-star', function () {
        var value = $(this).data('value');
        $('.rating .fas.fa-star').removeClass('clicked');
        $(this).addClass('clicked').prevAll().addClass('clicked');
        $(this).nextAll().removeClass('clicked');
    });

    $(document).on('mouseenter', '.rating .fas.fa-star', function () {
        $(this).addClass('hover');
        $(this).prevAll().addClass('hover');
    });

    $(document).on('mouseleave', '.rating .fas.fa-star', function () {
        $(this).removeClass('hover');
        $(this).prevAll().removeClass('hover');
    });
});