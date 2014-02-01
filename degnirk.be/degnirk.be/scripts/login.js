window.fbAsyncInit = function () {
    FB.init({
        appId: '442171809217325', // App ID
        status: true, // check login status
        cookie: true, // enable cookies to allow the server to access the session
        xfbml: true  // parse XFBML
    });

    // Additional initialization code here
    FB.Event.subscribe('auth.authResponseChange', function (response) {
        if (response.status === 'connected') {
            // the user is logged in and has authenticated your
            // app, and response.authResponse supplies
            // the user's ID, a valid access token, a signed
            // request, and the time th e access token
            // and signed request each expire
            var uid = response.authResponse.userID;
            var accessToken = response.authResponse.accessToken;

            //Change the login button when a user is logged in
            $('.fb-login-button').hide();

            //Call a function through AJAX for handling the user information when they logged in
            $.post('/umbraco/Surface/Login/UserInfo',
                { 'accessToken': accessToken },
                function (data, statusText) {
                    var name = data.name;
                    var id = data.id;
                    $('#fb-login-button').text(name).show();

                });
        } else if (response.status === 'not_authorized') {
            // the user is logged in to Facebook,
            // but has not authenticated your app
        } else {
            // the user isn't logged in to Facebook.
        }
    });

    FB.Event.subscribe('auth.login', function (response) {
        if (response.status === 'connected') {
            // the user is has logged on, make a new Umbraco member if needed
        }
    });
};

// Load the SDK Asynchronously
(function (d) {
    var js, id = 'facebook-jssdk', ref = d.getElementsByTagName('script')[0];
    if (d.getElementById(id)) { return; }
    js = d.createElement('script'); js.id = id; js.async = true;
    js.src = "//connect.facebook.net/en_US/all.js";
    ref.parentNode.insertBefore(js, ref);
}(document));