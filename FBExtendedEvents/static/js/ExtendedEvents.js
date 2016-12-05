// FBExtendedEvents@goit.io plugin JavaScript code

$(document).ready(function () {
  $('input[name="btnRequestToken"]').on('click', function () {
    console.log('Requesting token...');

    var username = $('input[name="username"]').val();
    var password = $('input[name="password"]').val();

    var data = {
      "cmd": "logon",
      "email": username,
      "password": password
    };

    $.ajax({
      type: "GET",
      url: "api.asp",
      cache: false,
      data: data,
      dataType: "xml",
      success: function (xml) {
        $(xml).find('token').each(function () {
          var token = $(this).text()
          
          $('#lblToken').text(token);
        });
      }
    });

  });
});
