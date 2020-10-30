var isSubmitted = false;
var playerNumberString = "";
var allRegistered = false;
var bidLevel = 0;
var bidSuit = "";
var alert = false;
var lastBidByOpps = false;
var bid = "";

if (model.Direction == model.ToBidDirection) {
    resetBB();
    document.getElementById("bb").style.display = "inline";
    document.getElementById("dir0").className = "card-header bg-warning py-1";
}
else {
    document.getElementById("bb").style.display = "none";
    document.getElementById("dir0").className = "card-header py-1";
}

var request = new XMLHttpRequest();
var sendBid = new XMLHttpRequest();
sendBid.onload = sendBidListener;
var pollBid = new XMLHttpRequest();
pollBid.onload = pollBidListener;
setTimeout(function () {
    pollBid.open('get', pollBidUrl + "&bidCounter=" + model.BidCounter.toString(), true);
    pollBid.send();
}, model.PollInterval);

function pollBidListener() {
    var returnBid = JSON.parse(this.responseText);
    if (returnBid.Status == "None") {
        setTimeout(function () {
            pollBid.open('get', pollBidUrl + "&bidCounter=" + model.BidCounter.toString(), true);
            pollBid.send();
        }, model.PollInterval);
    }
    else if (returnBid.Status == "BiddingComplete") {
        setTimeout(function () {
            if (returnBid.LastBidLevel > 0) {
                location.href = playingUrl;
            }
            else if (returnBid.LastBidLevel == 0) {
                location.href = travellerUrl;
            }
            else {
                location.href = registerPlayersUrl;
            }
        }, 3000);
    }
    else {
        if (model.Direction == "North") document.getElementById("skipButton").className = "btn-invisible";
        model.BidCounter = returnBid.BidCounter;
        model.LastBidLevel = returnBid.LastBidLevel;
        model.LastBidSuit = returnBid.LastBidSuit;
        model.LastBidX = returnBid.LastBidX;
        model.LastBidDirection = returnBid.LastBidDirection;
        model.PassCount = returnBid.PassCount;
        var tableEntry = "bid" + Math.floor(model.BidCounter / 4).toString() + (model.BidCounter % 4).toString();
        document.getElementById(tableEntry).innerHTML = returnBid.DisplayBid;
        if (returnBid.Alert) {
            if (!((model.Direction == "North" && returnBid.LastCallDirection == "South") || (model.Direction == "South" && returnBid.LastCallDirection == "North") || (model.Direction == "East" && returnBid.LastCallDirection == "West") || (model.LastDirection == "West" && returnBid.CallDirection == "East"))) {
                document.getElementById(tableEntry).className = "alertBid";
            }
        }
        if (model.Direction == returnBid.ToBidDirection) {
            bid = "";
            bidLevel = 0;
            bidSuit = "";
            alert = false;
            resetBB();
            document.getElementById("dir0").className = "card-header bg-warning py-1";
            document.getElementById("bbAlert").className = "btn btn-dark btn-lg btn-bb-lg";
            document.getElementById("bb").style.display = "inline";
        }
        else {
            document.getElementById("bb").style.display = "none";
            document.getElementById("dir0").className = "card-header py-1";
        }
        setTimeout(function () {
            pollBid.open('get', pollBidUrl + "&bidCounter=" + model.BidCounter.toString(), true);
            pollBid.send();
        }, model.PollInterval);
    }
}

function sendBidListener() {
    var returnBid = JSON.parse(this.responseText);
    model.BidCounter = returnBid.BidCounter;
    if (model.Direction == "North") document.getElementById("skipButton").className = "btn-invisible";
    model.LastBidLevel = returnBid.LastBidLevel;
    model.LastBidSuit = returnBid.LastBidSuit;
    model.LastBidX = returnBid.LastBidX;
    model.LastBidDirection = returnBid.LastBidDirection;
    model.PassCount = returnBid.PassCount;
    var tableEntry = "bid" + Math.floor(model.BidCounter / 4).toString() + (model.BidCounter % 4).toString();
    document.getElementById(tableEntry).innerHTML = returnBid.DisplayBid;
    if (returnBid.Alert) {
        if (!((model.Direction == "North" && returnBid.LastCallDirection == "South") || (model.Direction == "South" && returnBid.LastCallDirection == "North") || (model.Direction == "East" && returnBid.LastCallDirection == "West") || (model.LastDirection == "West" && returnBid.CallDirection == "East"))) {
            document.getElementById(tableEntry).className = "alertBid";
        }
    }
    document.getElementById("bb").style.display = "none";
    document.getElementById("dir0").className = "card-header py-1";
}

function selectPass() {
    if (bid == "Pass") { 
        bid = "";
    }
    else {
        bid = "Pass";
        bidLevel = 0;
        bidSuit = "";
    }
    resetBB();
}

function selectX() {
    if (bid == "x") {
        bid = "";
    }
    else {
        bid = "x";
        bidLevel = 0;
        bidSuit = "";
    }
    resetBB();
}

function selectXX() {
    if (bid == "xx") {
        bid = "";
    }
    else {
        bid = "xx";
        bidLevel = 0;
        bidSuit = "";
    }
    resetBB();
}

function selectLevel(level) {
    if (bidLevel == level) {
        bidLevel = 0;
    }
    else {
        bidLevel = level;
    }
    bid = "";
    bidSuit = "";
    resetBB();
}

function selectSuit(suit) {
    if (suit == bidSuit) {
        bidSuit = "";
    }
    else {
        bidSuit = suit;
    }
    resetBB();
}

function selectAlert() {
    alert = !alert;
    if (alert) {
        document.getElementById("bbAlert").className = "btn btn-warning btn-lg btn-bb-lg";
    }
    else {
        document.getElementById("bbAlert").className = "btn btn-dark btn-lg btn-bb-lg";
    }
}

function makeBid() {
    if (bid == "Pass") {
        model.PassCount++;
        sendBid.open('get', sendBidUrl + "&lastBidLevel=" + model.LastBidLevel.toString() + "&lastBidSuit=" + model.LastBidSuit + "&lastBidX=" + model.LastBidX + "&alert=" + alert + "&lastBidDirection=" + model.LastBidDirection + "&passCount=" + model.PassCount.toString() + "&bidCounter=" + (model.BidCounter + 1).toString(), true);
    }
    else if (bid == "x") {
        sendBid.open('get', sendBidUrl + "&lastBidLevel=" + model.LastBidLevel.toString() + "&lastBidSuit=" + model.LastBidSuit + "&lastBidX=x&alert=" + alert + "&lastBidDirection=" + model.Direction + "&passCount=0&bidCounter=" + (model.BidCounter + 1).toString(), true);
    }
    else if (bid == "xx") {
        sendBid.open('get', sendBidUrl + "&lastBidLevel=" + model.LastBidLevel.toString() + "&lastBidSuit=" + model.LastBidSuit + "&lastBidX=xx&alert=" + alert + "&lastBidDirection=" + model.Direction + "&passCount=0&bidCounter=" + (model.BidCounter + 1).toString(), true);
    }
    else {
        sendBid.open('get', sendBidUrl + "&lastBidLevel=" + bidLevel.toString() + "&lastBidSuit=" + bidSuit + "&lastBidX=&alert=" + alert + "&lastBidDirection=" + model.Direction + "&passCount=0&bidCounter=" + (model.BidCounter + 1).toString(), true);
    }
    sendBid.send();
}

function resetBB() {
    document.getElementById("bbMakeBid").disabled = true;
    if (bid == "Pass") {
        document.getElementById("bbMakeBid").disabled = false;
        document.getElementById("bbPass").className = "btn btn-warning btn-lg btn-bbLarge";
    }
    else {
        document.getElementById("bbPass").className = "btn btn-success btn-lg btn-bbLarge";
    }
    if (bid == "x") {
        document.getElementById("bbMakeBid").disabled = false;
        document.getElementById("bbX").className = "btn btn-warning btn-lg btn-bbLarge";
    }
    else {
        document.getElementById("bbX").className = "btn btn-danger btn-lg btn-bbLarge";
    }
    if (bid == "xx") {
        document.getElementById("bbMakeBid").disabled = false;
        document.getElementById("bbXX").className = "btn btn-warning btn-lg btn-bbLarge";
    }
    else {
        document.getElementById("bbXX").className = "btn btn-primary btn-lg btn-bbLarge";
    }

    if (bidLevel > 0 && bidSuit != "") {
        document.getElementById("bbMakeBid").disabled = false;
    }
    document.getElementById("bbNT").className = "btn btn-light  btn-lg btn-bb";
    document.getElementById("bbS").className = "btn btn-light  btn-lg btn-bb";
    document.getElementById("bbH").className = "btn btn-light  btn-lg btn-bb";
    document.getElementById("bbD").className = "btn btn-light  btn-lg btn-bb";
    document.getElementById("bbC").className = "btn btn-light  btn-lg btn-bb";
    if (bidSuit != "") {
        document.getElementById("bb" + bidSuit).className = "btn btn-warning btn-lg btn-bb";
    }

    for (var i = 1; i <= 7; i++) {
        if (i < model.LastBidLevel || (i == model.LastBidLevel && model.LastBidSuit == "N")) {
            document.getElementById("bb" + i.toString()).className = "btn btn-light btn-lg btn-bb";
            document.getElementById("bb" + i.toString()).disabled = true;
        }
        else {
            if (i == bidLevel) {
                document.getElementById("bb" + i.toString()).className = "btn btn-warning btn-lg btn-bb";
            }
            else {
                document.getElementById("bb" + i.toString()).className = "btn btn-light btn-lg btn-bb";
            }
            document.getElementById("bb" + i.toString()).disabled = false;
        }
    }
    if (bidLevel == 0) {
        document.getElementById("bbC").disabled = true;
        document.getElementById("bbD").disabled = true;
        document.getElementById("bbH").disabled = true;
        document.getElementById("bbS").disabled = true;
        document.getElementById("bbNT").disabled = true;
    }
    else if (bidLevel > model.LastBidLevel) {
        document.getElementById("bbC").disabled = false;
        document.getElementById("bbD").disabled = false;
        document.getElementById("bbH").disabled = false;
        document.getElementById("bbS").disabled = false;
        document.getElementById("bbNT").disabled = false;
    }
    else {
        if (model.LastBidSuit == "S") {
            document.getElementById("bbC").disabled = true;
            document.getElementById("bbD").disabled = true;
            document.getElementById("bbH").disabled = true;
            document.getElementById("bbS").disabled = true;
            document.getElementById("bbNT").disabled = false;
        }
        else if (model.LastBidSuit == "H") {
            document.getElementById("bbC").disabled = true;
            document.getElementById("bbD").disabled = true;
            document.getElementById("bbH").disabled = true;
            document.getElementById("bbS").disabled = false;
            document.getElementById("bbNT").disabled = false;
        }
        else if (model.LastBidSuit == "D") {
            document.getElementById("bbC").disabled = true;
            document.getElementById("bbD").disabled = true;
            document.getElementById("bbH").disabled = false;
            document.getElementById("bbS").disabled = false;
            document.getElementById("bbNT").disabled = false;
        }
        else if (model.LastBidSuit == "C") {
            document.getElementById("bbC").disabled = true;
            document.getElementById("bbD").disabled = false;
            document.getElementById("bbH").disabled = false;
            document.getElementById("bbS").disabled = false;
            document.getElementById("bbNT").disabled = false;
        }
        else {
            document.getElementById("bbC").disabled = false;
            document.getElementById("bbD").disabled = false;
            document.getElementById("bbH").disabled = false;
            document.getElementById("bbS").disabled = false;
            document.getElementById("bbNT").disabled = false;
        }
    }
    lastBidByOpps = ((model.Direction == "North" || model.Direction == "South") && (model.LastBidDirection == "East" || model.LastBidDirection == "West")) || ((model.Direction == "East" || model.Direction == "West") && (model.LastBidDirection == "North" || model.LastBidDirection == "South"));
    if (model.LastBidLevel > 0 && lastBidByOpps && model.LastBidX == "") {
        document.getElementById("bbX").disabled = false;
    }
    else {
        document.getElementById("bbX").disabled = true;
    }
    if (model.LastBidLevel > 0 && lastBidByOpps && model.LastBidX == "x") {
        document.getElementById("bbXX").disabled = false;
    }
    else {
        document.getElementById("bbXX").disabled = true;
    }
}

function skipYes() {
    request.open('get', skipUrl, true);
    request.send();
}

function SkipButtonClick() {
    $("#skipModal").modal("show");
}
