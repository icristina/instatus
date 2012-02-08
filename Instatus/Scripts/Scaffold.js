var uriField;
var previewField;

$('button[name=openFilePicker]').click(function () {
    var button = $(this);

    uriField = button.siblings('input');
    previewField = button.siblings('img.thumbnail');

    window.open('/editor/file?command=picker', '_blank');
});

$('a[rel=select]').click(function (e) {
    e.preventDefault();

    var button = $(this);
    var preview = button.closest('tr').find('.thumbnail');

    window.opener.selectedFile(button.attr('href'), preview.attr('src'));
    window.close();
});

function selectedFile(uri, picture) {
    uriField.val(uri);

    if (picture) {
        previewField.attr('src', picture);
        previewField.show();
    } else {
        previewField.hide();
    }
}