var allRegistered = false;
var allNames = false;
var playerNumberString = "";

updateRegistration();

var request = new XMLHttpRequest();
var enterPlayerNumber = new XMLHttpRequest();
enterPlayerNumber.onload = enterPlayerNumberListener;
var pollRegister = new XMLHttpRequest();
pollRegister.onload = pollRegisterListener;
setTimeout(function () {
    pollRegister.open('get', pollRegisterUrl, true);
    pollRegister.send();
}, model.PollInterval);

$(document).on('touchstart', '#OKButton:enabled', function () {
    request.open('get', OKButtonClickUrl, true);
    request.send();
});

function enterPlayerNumberListener() {
    model.PlayerName[0] = JSON.parse(this.responseText);
    document.getElementById("Name0").innerHTML = model.PlayerName[0];
    document.getElementById("NameBlock0").className = "card bg-light text-dark";
    updateRegistration();
}

function pollRegisterListener() {
    var playerUpdate = JSON.parse(this.responseText);
    if (playerUpdate.Status == "None") {
        setTimeout(function () {
            pollRegister.open('get', pollRegisterUrl, true);
            pollRegister.send();
        }, model.PollInterval);
    }
    else if (playerUpdate.Status == "BiddingStarted") {
        if (sitOut) {
            location.href = sitOutUrl;
        }
        else {
            location.href = biddingUrl;
        }
    }
    else if (playerUpdate.Status == "PlayerUpdate") {
        model.Registered = playerUpdate.Registered;
        model.PlayerName = playerUpdate.PlayerName;
        updateRegistration();
        setTimeout(function () {
            pollRegister.open('get', pollRegisterUrl, true);
            pollRegister.send();
        }, model.PollInterval);
    }
}

function updateRegistration() {
    for (var i = 0; i < 4; i++) {
        if (model.Registered[i] && model.PlayerName[i] != "") {
            document.getElementById("NameBlock" + i.toString()).className = "card bg-light text-dark";
            document.getElementById("Name" + i.toString()).innerHTML = model.PlayerName[i];
        }
    }
    if (model.PairNumber[1] == 0) {
        allRegistered = model.Registered[0] && model.Registered[2];
        allNames = (model.PlayerName[0] != "") && (model.PlayerName[2] != "");
    }
    else {
        allRegistered = model.Registered[0] && model.Registered[1] && model.Registered[2] && model.Registered[3];
        allNames = (model.PlayerName[0] != "") && (model.PlayerName[1] != "") && (model.PlayerName[2] != "") && (model.PlayerName[3] != "");
    }
    if (model.Direction[0] == "North") {
        document.getElementById("Message").innerHTML = "Please wait until everyone is registered, and then press OK";
        if (allRegistered && allNames) document.getElementById("OKButton").disabled = false;
    }
    else {
        document.getElementById("Message").innerHTML = "Please wait for North to take everyone to the next screen";
    }
}

function addNumber(e) {
    if (playerNumberString == "Unknown") {
        playerNumberString = "";
    }
    playerNumberString = playerNumberString + e;
    document.getElementById('playerNumberBox').value = playerNumberString;
    document.getElementById("enterButton").disabled = false;
}

function unknown() {
    playerNumberString = "Unknown";
    document.getElementById('playerNumberBox').value = playerNumberString;
    document.getElementById("enterButton").disabled = false;
}

function clearplayerNumber() {
    playerNumberString = "";
    document.getElementById('playerNumberBox').value = "";
    document.getElementById("enterButton").disabled = true;
}

function clearLastEntry() {
    if (playerNumberString == "Unknown") {
        playerNumberString = "";
    }
    else {
        if (playerNumberString.length > 0) {
            playerNumberString = playerNumberString.substr(0, playerNumberString.length - 1);
        }
    }
    document.getElementById('playerNumberBox').value = playerNumberString;
    if (playerNumberString == "") document.getElementById("enterButton").disabled = true;
}

function enterNumber() {
    if (playerNumberString == "Unknown") {
        enterPlayerNumber.open('get', enterPlayerNumberUrl + "0", true);
    }
    else {
        enterPlayerNumber.open('get', enterPlayerNumberUrl + playerNumberString, true);
    }
    enterPlayerNumber.send();
    clearplayerNumber();
}
