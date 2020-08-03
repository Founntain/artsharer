import * as $ from 'jquery';

var user: string = new URLSearchParams(window.location.search).get("user");
var expireDate: Date;

document.title = "Artwork for " + user;

$(document).ready(() =>{

    $("#footer").click(() =>{
        window.open("https://www.furaffinity.net/user/founntain/");
    })

    LoadExpireDate(user).done((result) => {
        expireDate = new Date(result);

        $("#expireDate").html(expireDate.toLocaleDateString());

        if(new Date() > expireDate){
            $("#headerExpired").removeClass("hiddenDiv");
            $("#headerDownload").addClass("hiddenDiv");

            $("#headerExpired").click(() => {
                window.open("https://www.furaffinity.net/user/mohregregs")
            });
        }
        else{
            $("#headerExpired").addClass("hiddenDiv");
            $("#headerDownload").removeClass("hiddenDiv");
            LoadImagesFromUser(user);
        }
    });
})

function LoadExpireDate(user: string){
    return ApiCallWithParameters("Main", "getExpireDateOfUser", {
        "username": user
    });
}

function LoadImagesFromUser(user: string){
    ApiCallWithParameters("Main", "getImagesFromCustomer", {
        "username": user
    }).done((result) => {
        let url = "YOUR_URL THAT POINTS TO THE IMAGE FOLDER ON YOUR SERVER WHERE THE API IS RUNNING";

        for(const obj of result){
            if((<string> obj).slice(-".zip".length) == ".zip"){
                $("#headerDownload").click(() => {
                    window.open(url+name+"/"+obj);
                });

                continue;
            }

            var img = $(document.createElement("img"));
            img.attr("src", url+name+"/"+ obj)

            img.addClass("image")

            $("#images").prepend(img);

            img.click(() => {
                window.open(url+name+"/"+ obj);
            });
        }
    });
}

function ApiCallWithParameters(controller: string, action: string, object: any): JQuery.jqXHR {
    // var url = "http://localhost:5000/api/";
    var url = "YOUR_API_URL";
    
    return $.ajax({
        url: url + controller + "/" + action,
        data: object,
        dataType: "json",
        success: (result) => { return result }
    })
}