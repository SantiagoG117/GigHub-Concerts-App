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


//? Service: Responsible for holding data access methods that call the server. The Service should be independent of any
//? UI and can be reusable accross multiple controllers. 

var AttendanceService = function () {

    //!Public methods

    /*
        done and fail: It is the responsibility of the controller to determine what should happen after the call to the
        server, not of the service. The controller should pass the private references holding the done and fail
        anonymous functions as arguments.
    */
    var createAttendance = function (gigId, done, fail) {
        $.post(
            //URL of the request
            "/api/attendances",
            //Data to be sent in the request body 
            { gigId: gigId }
        )
            //Promise of the post request
            .done(done)
            .fail(fail);
    };

    /*
        done and fail: It is the responsibility of the controller to determine what should happen after the call to the
        server, not of the service. The controller should pass the private references holding the done and fail
        anonymous functions as arguments.
    */
    var deleteAttendance = function (gigId, done, fail) {
        $.ajax({
            /*
                Add an ID that uniquely identify an attendance for the currently logged-in user.
                The keys for attendance are a combination of the User ID and the Gig ID, however we
                should't expose the UserID in the API, so we should only send the Gig ID. 
                */
            url: "/api/attendances/CancelAttend/?id=" + gigId,
            method: "DELETE"
        })
            .done(done)
            .fail(fail);
    };

    //! Revealing part
    return {
        createAttendance: createAttendance,
        deleteAttendance: deleteAttendance
    };
}(); //! <- IIFE pattern