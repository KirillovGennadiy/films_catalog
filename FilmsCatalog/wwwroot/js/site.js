$(function () {
    $('[data-type=clickable-row]').on('click', ({ target }) => {
        let tr = $(target).closest('tr');
        let url = tr.data('href');
        window.location = url;
    })
})
