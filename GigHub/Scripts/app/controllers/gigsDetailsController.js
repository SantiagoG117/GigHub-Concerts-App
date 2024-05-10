/*
 
    4. Remove the Follow button in the Gigs view
*/

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

var GigsDetailsController = function (service) {

    //!Private Methods:
    var button;

    //!Public methods:
    var init = function () {
        $(".js-toggle-follow").click(toggleFollowing);
    };

    //!Private methods:
    var toggleFollowing = function (event) {
        //Store a reference of the button that raises the click event
        button = $(event.target);

        //Get the artist ID:
        var artistID = button.attr("data-artist-id");

       
        //If the user is not following the artist...
        if (button.hasClass("btn-primary"))
            service.createFollowing(artistID, button);
        //If the user is following the artist...
        else
            service.deleteFollowing(artistID, button);

    }

    //! Revealing part:
    return {
        init: init
    }

}(FollowingService);//!<- IIFE pattern: When we invoke the function we must pass an argument as a value for service
