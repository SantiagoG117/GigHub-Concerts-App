/*
    Revealing Module Pattern: A way to modularize our Script files and create a structure simmilar to the concept of
    classes.

    var Person = function () {
        var firstName = "Santiago"; <- Private variable accessible only from inside the function's scoope

        var sayHello = function(){
            console.log(firstName)
        }

        //Revealing part:
        return {
            sayHello : sayHello <- By returning a variable as a key value pair we make it public and accessible from outside
                                   of the Person function 
        }

    }();<- () represent the Immediately Invoke Function Expression (IIFE) Allows us to create privacy within a function
*/

var FollowingService = function () {

    //!Public methods:
    var createFollowing = function (artistID, button) {
        //Ajax POST request:
        $.post(
            //URL of the request
            "/api/followings/",
            //Data to be send in the request body
            { artistId: artistID }
        )
            //Promise of the Post request
            .done(function () {

                

                var text = (button.text() == "Following") ? "Follow" : "Following";

                /*
                    To swap the btn-primary with thebtn-success class we use toggleClass.
                    The toggleClass method toggles between adding and removing class names for the selected element.
                    In this example:
                        If the btn-success exist it will be removed, otherwise it will be added
                        If the btn-primary exist it will be removed, otherwise it will be added
                */

                button.toggleClass("btn-success").toggleClass("btn-primary").text(text);
            })
            .fail(function () {
                alert("Something failed");
            });
    };

    var deleteFollowing = function (artistID, button) {
        $.ajax({

            url: "/api/followings/unfollow/?id=" + artistID,
            method: "DELETE"
        })
            .done(function () {

            

                var text = (button.text() == "Follow") ? "Following" : "Follow";

                /*
                    To swap the btn-primary with thebtn-success class we use toggleClass.
                    The toggleClass method toggles between adding and removing class names for the selected element.
                    In this example:
                        If the btn-success exist it will be removed, otherwise it will be added
                        If the btn-primary exist it will be removed, otherwise it will be added
                */

                button.toggleClass("btn-success").toggleClass("btn-primary").text(text);
            })
            .fail(function () {
                alert("Something failed");
            });
    }

    //! Revealing part:

    return {
        createFollowing: createFollowing,
        deleteFollowing: deleteFollowing
    }

}(); //! <- IIFE pattern