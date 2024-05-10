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




// ? Controller: Responsible for handeling the events raised in the Gigs view and updating the view
var GigsController = function (attendanceService) {
    //! Private attributes:
    var button;//Keeps track of the state of the Gigs view, so if a button is clicked, we can keep a reference to it

    //! Public methods:
    var init = function (container) //container is a selector that represents the container for all the gigs
    {

       /*
            Register a handler for the click event for elements of the class js-toggle-attendance:

            The jQuery 'on' method we achieve two advantages:
               1. It allows us to implement a 'load more gigs' feature that allows the user to load more gigs (add
               more elements to the DOM after the page is loaded) as he scrolls down. 
               2. In terms of memory management, we only have have one handler for all the elements belonging to
               the "js-toggle-attendance"
        */
        $(container) //Stores all the js-toggle-attendance elements that may be load to the DOM now or in the future 
                     .on("click", //Type of event we want to handle
                        ".js-toggle-attendance", //Selector
                         toggleAttendance //Handler for the event
                     );
    };


    //!Private methods:
    var toggleAttendance = function (event) {
        //Store a reference of the button that raises the click event
        button = $(event.target);

        //Get the Gig ID
        var gigId = button.attr("data-gig-id");

        if (button.hasClass("btn-secondary"))
            attendanceService.createAttendance(gigId, done, fail);
        else
            attendanceService.deleteAttendance(gigId, done, fail);
    }

    var done = function () {

        var text = (button.text() == "Going") ? "Going?" : "Going";

        /*
            To swap the btn-info with the btn-default class we use toggleClass
            The toggleClass method toggles between adding and removing class names for the selected element.
            In this example:
                If the btn-info exist it will be removed, otherwise it will be added
                If the btn-default exist it will be removed, otherwise it will be added
         */

        button.toggleClass("btn-success").toggleClass("btn-secondary").text(text);
    };

    //TODO: Change this implementation with a toast or a bootbox
    var fail = function () {
        alert("Something failed")
    };

    //!Revealing part
    return {
        init: init
    }

}(AttendanceService);//!<- IIFE pattern: When we invoke the function we must pass an argument as a value for attendanceService

