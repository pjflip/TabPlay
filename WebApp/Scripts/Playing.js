var cardSelectedNumber = -1;
var cardSelectedString = "";
var allowPlay = false;
var claimMade = false;
var openingLeadMade = false;
var claimTricksNumber = -1;
var handExposed = [true, false, false, false];

document.getElementById("dir" + model.DummyDirectionNumber.toString() + "body").style.backgroundColor = "Grey";
if (model.DummyDirectionNumber == 0) document.getElementById("claimButton").className = "btn-invisible";
if (model.PlayCounter >= 0) {
    makeInitialPlays();
    exposeCards(model.DummyDirectionNumber);
}
startPlay();

var request = new XMLHttpRequest();
var pollPlay = new XMLHttpRequest();
pollPlay.onload = pollPlayListener;
setTimeout(function () {
    pollPlay.open('get', pollPlayUrl + "&playCounter=" + model.PlayCounter.toString(), true);
    pollPlay.send();
}, model.PollInterval);

function pollPlayListener() {
    var returnPlay = JSON.parse(this.responseText);
    if (returnPlay.Status == "None") {
        setTimeout(function () {
            pollPlay.open('get', pollPlayUrl + "&playCounter=" + model.PlayCounter.toString(), true);
            pollPlay.send();
        }, model.PollInterval);
    }
    else if (returnPlay.Status == "PlayComplete") {
        location.href = travellerUrl;
    }
    else if (returnPlay.Status == "Play" && returnPlay.PlayCounter == -1) {
        if (!openingLeadMade) {
            var directionNumber = dirNumber(returnPlay.PlayDirection);
            if (handExposed[directionNumber] || directionNumber == 2) {
                document.getElementById("h" + directionNumber.toString() + "c" + returnPlay.CardNumber.toString()).className = "btn btn-invisible";
            }
            else {
                document.getElementById("h" + directionNumber.toString() + "r3c2").className = "btn btn-invisible";
            }
            document.getElementById("playCard" + directionNumber.toString()).className = "btn btn-cardLarge btn-cardBack";
            openingLeadMade = true;
        }
        setTimeout(function () {
            pollPlay.open('get', pollPlayUrl + "&playCounter=" + model.PlayCounter.toString(), true);
            pollPlay.send();
        }, model.PollInterval);
    }
    else if (returnPlay.Status == "ClaimExpose") {
        if (!claimMade) {
            var claimDirectionNumber = dirNumber(returnPlay.ClaimDirection);
            for (var i = 0; i < 4; i++) {
                if (model.Direction[0] == "North") {
                    document.getElementById("claimButton").className = "btn btn-warning mr-0 ml-2 float-right";
                }
                else {
                    document.getElementById("claimButton").className = "btn-invisible";
                }
            }
            document.getElementById("claimDirection").innerHTML = returnPlay.ClaimDirection;
            document.getElementById("claimBox").className = "card bg-danger text-white";
            if (claimDirectionNumber != 0) {
                exposeCards(claimDirectionNumber);
            }
            claimMade = true;
        }
        setTimeout(function () {
            pollPlay.open('get', pollPlayUrl + "&playCounter=" + model.PlayCounter.toString(), true);
            pollPlay.send();
        }, model.PollInterval);
    }
    else {
        model.PlayCounter = returnPlay.PlayCounter;
        var directionNumber = dirNumber(returnPlay.PlayDirection);
        model.CardPlayed[13 * directionNumber + returnPlay.CardNumber] = true;
        model.TrickSuit[directionNumber] = returnPlay.CardString.substr(1, 1);
        if (model.PlayCounter % 4 == 0) {
            model.TrickLeadSuit = model.TrickSuit[directionNumber];
        }
        var rankString = returnPlay.CardString.substr(0, 1);
        if (rankString == "A") {
            model.TrickRank[directionNumber] = 14;
        }
        else if (rankString == "K") {
            model.TrickRank[directionNumber] = 13;
        }
        else if (rankString == "Q") {
            model.TrickRank[directionNumber] = 12;
        }
        else if (rankString == "J") {
            model.TrickRank[directionNumber] = 11;
        }
        else if (rankString == "T") {
            model.TrickRank[directionNumber] = 10;
        }
        else {
            model.TrickRank[directionNumber] = parseInt(rankString);
        }

        if (handExposed[directionNumber] || directionNumber == 2) {
            document.getElementById("h" + directionNumber.toString() + "c" + returnPlay.CardNumber.toString()).className = "btn btn-invisible";
        }
        else {
            var iCard;
            var iRow = Math.floor((12 - model.TrickNumber) / 3);
            if (iRow < 0) iRow = 0;
            if (model.TrickNumber < 10) {
                iCard = (12 - model.TrickNumber) % 3;
            }
            else {
                iCard = (13 - model.TrickNumber) % 4;
            }
            document.getElementById("h" + directionNumber.toString() + "r" + iRow.toString() + "c" + iCard.toString()).className = "btn btn-invisible";
        }
        document.getElementById("playCard" + directionNumber.toString() + "Rank").innerHTML = model.DisplayRank[13 * directionNumber + returnPlay.CardNumber];
        document.getElementById("playCard" + directionNumber.toString() + "Suit").innerHTML = model.DisplaySuit[13 * directionNumber + returnPlay.CardNumber];
        document.getElementById("playCard" + directionNumber.toString()).className = "btn btn-cardLarge";

        if (model.PlayCounter == 0) {
            // Opening lead
            exposeCards(model.DummyDirectionNumber);
        }

        if (model.PlayCounter % 4 == 3) {
            // End of trick
            var winningDirectionNumber = -1;
            var winningSuit = model.TrickLeadSuit;
            var winningRank = 0;
            for (var i = 0; i < 4; i++) {
                if ((winningSuit == model.TrickLeadSuit && model.TrickSuit[i] == model.TrickLeadSuit && model.TrickRank[i] > winningRank) || (winningSuit == model.ContractSuit && model.TrickSuit[i] == model.ContractSuit && model.TrickRank[i] > winningRank)) {
                    winningDirectionNumber = i;
                    winningRank = model.TrickRank[i];
                }
                else if (model.TrickLeadSuit != model.ContractSuit && winningSuit == model.TrickLeadSuit && model.TrickSuit[i] == model.ContractSuit) {
                    winningDirectionNumber = i;
                    winningRank = model.TrickRank[i];
                    winningSuit = model.ContractSuit;
                }
            }
            if (model.Direction[winningDirectionNumber] == "North" || model.Direction[winningDirectionNumber] == "South") {
                model.TricksNS++;
                document.getElementById("tricksNS").innerHTML = model.TricksNS.toString();
            }
            else {
                model.TricksEW++;
                document.getElementById("tricksEW").innerHTML = model.TricksEW.toString();
            }
            model.TrickNumber++;
            model.PlayDirectionNumber = winningDirectionNumber;
            model.TrickLeadSuit = "";
            if (model.PlayCounter == 51 && model.Direction[0] == "North") {
                var tricks = tricksEW;
                if (model.Declarer == "North" || model.Declarer == "South") tricks = tricksNS;
                request.open('get', sendResultUrl + "&tricks=" + tricks.toString(), true);
                request.send();
                setTimeout(function () {
                    pollPlay.open('get', pollPlayUrl + "&playCounter=" + model.PlayCounter.toString(), true);
                    pollPlay.send();
                }, model.PollInterval);
            }
            else {
                setTimeout(function () {
                    document.getElementById("playCard0").className = "btn btn-invisible";
                    document.getElementById("playCard1").className = "btn btn-invisible";
                    document.getElementById("playCard2").className = "btn btn-invisible";
                    document.getElementById("playCard3").className = "btn btn-invisible";
                    startPlay();
                    pollPlay.open('get', pollPlayUrl + "&playCounter=" + model.PlayCounter.toString(), true);
                    pollPlay.send();
                }, 4000);
            }
        }
        else {
            // Not end of trick
            model.PlayDirectionNumber = (model.PlayDirectionNumber + 1) % 4;
            startPlay();
            setTimeout(function () {
                pollPlay.open('get', pollPlayUrl + "&playCounter=" + model.PlayCounter.toString(), true);
                pollPlay.send();
            }, model.PollInterval);
        }
    }
}

function dirNumber(dirString) {
    for (var i = 0; i < 4; i++) {
        if (model.Direction[i] == dirString) return i;
    }
    return -1;
}

function startPlay() {
    document.getElementById("dir0header").className = "card-header py-1";
    document.getElementById("dir1header").className = "card-header py-1";
    document.getElementById("dir2header").className = "card-header py-1";
    document.getElementById("dir3header").className = "card-header py-1";
    document.getElementById("dir" + model.PlayDirectionNumber.toString() + "header").className = "card-header bg-warning py-1";
    allowPlay = ((model.PlayDirectionNumber == 0 && model.DummyDirectionNumber != 0) || (model.PlayDirectionNumber == 2 && model.DummyDirectionNumber == 2));
    if (allowPlay) {
        document.getElementById("buttonMakePlay").disabled = true;
        document.getElementById("buttonMakePlay").className = "btn btn-info btn-lg";
    }
}

function makeInitialPlays() {
    var hand1CountPlayed = 0;
    var hand3CountPlayed = 0;
    for (var iCard = 0; iCard < 13; iCard++) {
        if (model.CardPlayed[iCard]) {
            document.getElementById("h0c" + iCard.toString()).className = "btn btn-invisible";
        }
        if (model.DummyDirectionNumber != 2 && model.CardPlayed[26 + iCard]) {
            document.getElementById("h2c" + iCard.toString()).className = "btn btn-invisible";
        }
        if (model.CardPlayed[13 + iCard]) hand1CountPlayed++;
        if (model.CardPlayed[39 + iCard]) hand3CountPlayed++;
    }
    if (model.DummyDirectionNumber != 1) {
        if (hand1CountPlayed > 0) document.getElementById("h1r3c2").className = "btn btn-invisible";
        if (hand1CountPlayed > 1) document.getElementById("h1r3c1").className = "btn btn-invisible";
        if (hand1CountPlayed > 2) document.getElementById("h1r3c0").className = "btn btn-invisible";
        if (hand1CountPlayed > 3) document.getElementById("h1r2c2").className = "btn btn-invisible";
        if (hand1CountPlayed > 4) document.getElementById("h1r2c1").className = "btn btn-invisible";
        if (hand1CountPlayed > 5) document.getElementById("h1r2c0").className = "btn btn-invisible";
        if (hand1CountPlayed > 6) document.getElementById("h1r1c2").className = "btn btn-invisible";
        if (hand1CountPlayed > 7) document.getElementById("h1r1c1").className = "btn btn-invisible";
        if (hand1CountPlayed > 8) document.getElementById("h1r1c0").className = "btn btn-invisible";
        if (hand1CountPlayed > 9) document.getElementById("h1r0c3").className = "btn btn-invisible";
        if (hand1CountPlayed > 10) document.getElementById("h1r0c2").className = "btn btn-invisible";
        if (hand1CountPlayed > 11) document.getElementById("h1r0c1").className = "btn btn-invisible";
        if (hand1CountPlayed > 12) document.getElementById("h1r0c0").className = "btn btn-invisible";
    }
    if (model.DummyDirectionNumber != 3) {
        if (hand3CountPlayed > 0) document.getElementById("h3r3c2").className = "btn btn-invisible";
        if (hand3CountPlayed > 1) document.getElementById("h3r3c1").className = "btn btn-invisible";
        if (hand3CountPlayed > 2) document.getElementById("h3r3c0").className = "btn btn-invisible";
        if (hand3CountPlayed > 3) document.getElementById("h3r2c2").className = "btn btn-invisible";
        if (hand3CountPlayed > 4) document.getElementById("h3r2c1").className = "btn btn-invisible";
        if (hand3CountPlayed > 5) document.getElementById("h3r2c0").className = "btn btn-invisible";
        if (hand3CountPlayed > 6) document.getElementById("h3r1c2").className = "btn btn-invisible";
        if (hand3CountPlayed > 7) document.getElementById("h3r1c1").className = "btn btn-invisible";
        if (hand3CountPlayed > 8) document.getElementById("h3r1c0").className = "btn btn-invisible";
        if (hand3CountPlayed > 9) document.getElementById("h3r0c3").className = "btn btn-invisible";
        if (hand3CountPlayed > 10) document.getElementById("h3r0c2").className = "btn btn-invisible";
        if (hand3CountPlayed > 11) document.getElementById("h3r0c1").className = "btn btn-invisible";
        if (hand3CountPlayed > 12) document.getElementById("h3r0c0").className = "btn btn-invisible";
    }
    for (var i = 0; i < 4; i++) {
        if (model.TrickCardString[i] != "") {
            document.getElementById("playCard" + i.toString() + "Rank").innerHTML = model.TrickDisplayRank[i];
            document.getElementById("playCard" + i.toString() + "Suit").innerHTML = model.TrickDisplaySuit[i];
            document.getElementById("playCard" + i.toString()).className = "btn btn-cardLarge";
        }
    }
}

function exposeCards(directionNumber) {
    handExposed[directionNumber] = true;
    if (directionNumber == 1) {
        var iCard = 0;
        for (var iSuit = 0; iSuit < 4; iSuit++) {
            for (var i = 0; i < model.SuitLengths[4 + iSuit]; i++) {
                if (model.CardPlayed[13 + iCard]) {
                    document.getElementById("h1r" + iSuit.toString() + "c" + i.toString()).className = "btn btn-invisible";
                }
                else {
                    var hr = "h1r" + iSuit.toString() + "c" + i.toString();
                    document.getElementById(hr + "Rank").innerHTML = model.DisplayRank[13 + iCard];
                    document.getElementById(hr + "Suit").innerHTML = model.DisplaySuit[13 + iCard];
                    document.getElementById(hr).className = "btn btn-cardSmall";
                    document.getElementById(hr + "Rank").id = "h1c" + iCard.toString() + "Rank";
                    document.getElementById(hr + "Suit").id = "h1c" + iCard.toString() + "Suit";
                    document.getElementById(hr).id = "h1c" + iCard.toString();
                }
                iCard++;
            }
            for (var i = model.SuitLengths[4 + iSuit]; i < 5; i++) {
                document.getElementById("h1r" + iSuit.toString() + "c" + i.toString()).className = "btn btn-invisible";
            }
        }
    }
    else if (directionNumber == 2) {
        for (var iCard = 0; iCard < 13; iCard++) {
            var hr = "h2c" + iCard.toString();
            if (model.CardPlayed[26 + iCard]) {
                document.getElementById(hr).className = "btn btn-invisible";
            }
            else {
                document.getElementById(hr + "Rank").innerHTML = model.DisplayRank[26 + iCard];
                document.getElementById(hr + "Suit").innerHTML = model.DisplaySuit[26 + iCard];
                if (model.DummyDirectionNumber == 2) {
                    document.getElementById(hr).className = "btn btn-cardLarge";
                }
                else {
                    document.getElementById(hr).className = "btn btn-cardSmall";
                }
            }
        }
    }
    else if (directionNumber == 3) {
        var iCard = 0;
        for (var iSuit = 0; iSuit < 4; iSuit++) {
            for (var i = 0; i < model.SuitLengths[12 + iSuit]; i++) {
                if (model.CardPlayed[39 + iCard]) {
                    document.getElementById("h3r" + iSuit.toString() + "c" + i.toString()).className = "btn btn-invisible";
                }
                else {
                    var hr = "h3r" + iSuit.toString() + "c" + i.toString();
                    document.getElementById(hr + "Rank").innerHTML = model.DisplayRank[39 + iCard];
                    document.getElementById(hr + "Suit").innerHTML = model.DisplaySuit[39 + iCard];
                    document.getElementById(hr).className = "btn btn-cardSmall";
                    document.getElementById(hr + "Rank").id = "h3c" + iCard.toString() + "Rank";
                    document.getElementById(hr + "Suit").id = "h3c" + iCard.toString() + "Suit";
                    document.getElementById(hr).id = "h3c" + iCard.toString();
                }
                iCard++;
            }
            for (var i = model.SuitLengths[12 + iSuit]; i < 5; i++) {
                document.getElementById("h3r" + iSuit.toString() + "c" + i.toString()).className = "btn btn-invisible";
            }
        }
    }
}

function cardClick(cardNumber, directionNumber) {
    if (allowPlay && (model.PlayDirectionNumber == directionNumber)) {
        if (cardNumber == cardSelectedNumber) {
            cardSelectedNumber = -1;
            document.getElementById("h" + directionNumber.toString() + "c" + cardNumber.toString()).className = "btn btn-cardLarge";
            document.getElementById("buttonMakePlay").disabled = true;
        }
        else {
            var hasCardInLeadSuit = false;
            for (var i = 0; i < 13; i++) {
                if (!model.CardPlayed[13 * directionNumber + i] && model.CardString[13 * directionNumber + i].substr(1, 1) == model.TrickLeadSuit) {
                    hasCardInLeadSuit = true;
                    break;
                }
            }
            if (!hasCardInLeadSuit || model.CardString[13 * directionNumber + cardNumber].substr(1, 1) == model.TrickLeadSuit) {
                if (cardSelectedNumber != -1) document.getElementById("h" + directionNumber.toString() + "c" + cardSelectedNumber.toString()).className = "btn btn-cardLarge";
                cardSelectedNumber = cardNumber;
                cardSelectedString = model.CardString[13 * directionNumber + cardSelectedNumber];
                document.getElementById("h" + directionNumber.toString() + "c" + cardSelectedNumber.toString()).className = "btn btn-cardLarge btn-selected";
                document.getElementById("buttonMakePlay").disabled = false;
            }
        }
    }
}

function makePlay() {
    if (model.PlayCounter == -999) {
        request.open('get', sendOpeningLeadUrl + "&direction=" + model.Direction[model.PlayDirectionNumber] + "&cardNumber=" + cardSelectedNumber.toString(), true);
        request.send();
        model.PlayCounter = -1;
        document.getElementById("h0c" + cardSelectedNumber.toString()).className = "btn btn-invisible";
        document.getElementById("playCard0").className = "btn btn-cardLarge btn-cardBack";
    }
    else {
        allowPlay = false;
        model.PlayCounter++;
        request.open('get', sendPlayUrl + "&direction=" + model.Direction[model.PlayDirectionNumber] + "&cardNumber=" + cardSelectedNumber.toString() + "&cardString=" + cardSelectedString + "&playCounter=" + model.PlayCounter.toString(), true);
        request.send();
        document.getElementById("buttonMakePlay").className = "btn btn-invisible";
        model.CardPlayed[13 * model.PlayDirectionNumber + cardSelectedNumber] = true;
        model.TrickSuit[model.PlayDirectionNumber] = cardSelectedString.substr(1, 1);
        if (model.PlayCounter % 4 == 0) {
            model.TrickLeadSuit = model.TrickSuit[model.PlayDirectionNumber];
        }
        var rankString = cardSelectedString.substr(0, 1);
        if (rankString == "A") {
            model.TrickRank[model.PlayDirectionNumber] = 14;
        }
        else if (rankString == "K") {
            model.TrickRank[model.PlayDirectionNumber] = 13;
        }
        else if (rankString == "Q") {
            model.TrickRank[model.PlayDirectionNumber] = 12;
        }
        else if (rankString == "J") {
            model.TrickRank[model.PlayDirectionNumber] = 11;
        }
        else if (rankString == "T") {
            model.TrickRank[model.PlayDirectionNumber] = 10;
        }
        else {
            model.TrickRank[model.PlayDirectionNumber] = parseInt(rankString);
        }
        document.getElementById("h" + model.PlayDirectionNumber.toString() + "c" + cardSelectedNumber.toString()).className = "btn btn-invisible";

        document.getElementById("playCard" + model.PlayDirectionNumber.toString() + "Rank").innerHTML = model.DisplayRank[13 * model.PlayDirectionNumber + cardSelectedNumber];
        document.getElementById("playCard" + model.PlayDirectionNumber.toString() + "Suit").innerHTML = model.DisplaySuit[13 * model.PlayDirectionNumber + cardSelectedNumber];
        document.getElementById("playCard" + model.PlayDirectionNumber.toString()).className = "btn btn-cardLarge";

        if (model.PlayCounter == 0) {
            // Opening lead
            exposeCards(model.DummyDirectionNumber);
        }

        if (model.PlayCounter % 4 == 3) {
            // End of trick
            var winningDirectionNumber = -1;
            var winningSuit = model.TrickLeadSuit;
            var winningRank = 0;
            for (var i = 0; i < 4; i++) {
                if ((winningSuit == model.TrickLeadSuit && model.TrickSuit[i] == model.TrickLeadSuit && model.TrickRank[i] > winningRank) || (winningSuit == model.ContractSuit && model.TrickSuit[i] == model.ContractSuit && model.TrickRank[i] > winningRank)) {
                    winningDirectionNumber = i;
                    winningRank = model.TrickRank[i];
                }
                else if (model.TrickLeadSuit != model.ContractSuit && winningSuit == model.TrickLeadSuit && model.TrickSuit[i] == model.ContractSuit) {
                    winningDirectionNumber = i;
                    winningRank = model.TrickRank[i];
                    winningSuit = model.ContractSuit;
                }
            }
            if (model.Direction[winningDirectionNumber] == "North" || model.Direction[winningDirectionNumber] == "South") {
                model.TricksNS++;
                document.getElementById("tricksNS").innerHTML = model.TricksNS.toString();
            }
            else {
                model.TricksEW++;
                document.getElementById("tricksEW").innerHTML = model.TricksEW.toString();
            }
            model.TrickNumber++;
            model.PlayDirectionNumber = winningDirectionNumber;
            model.TrickLeadSuit = "";
            if (model.PlayCounter == 51 && model.Direction[0] == "North") {
                var tricks = tricksEW;
                if (model.Declarer == "North" || model.Declarer == "South") tricks = tricksNS;
                request.open('get', sendResultUrl + "&tricks=" + tricks.toString(), true);
                request.send();
            }
            else {
                setTimeout(function () {
                    document.getElementById("playCard0").className = "btn btn-invisible";
                    document.getElementById("playCard1").className = "btn btn-invisible";
                    document.getElementById("playCard2").className = "btn btn-invisible";
                    document.getElementById("playCard3").className = "btn btn-invisible";
                    startPlay();
                }, 4000);
            }
        }
        else {
            // Not end of trick
            model.PlayDirectionNumber = (model.PlayDirectionNumber + 1) % 4;
            startPlay();
        }
        cardSelectedNumber = -1;
    }
}

// *** Claims section ***
$('#claimTricksModal').on('show.bs.modal', function () {
    for (var i = 0; i <= 13; i++) {
        if ((model.Declarer == "North" || model.Declarer == "South") && i < model.TricksNS) document.getElementById("tricks" + i.toString()).className = "btn btn-invisible"
        if ((model.Declarer == "East" || model.Declarer == "West") && i < model.TricksEW) document.getElementById("tricks" + i.toString()).className = "btn btn-invisible"
        if ((model.Declarer == "North" || model.Declarer == "South") && i > 13 - model.TricksEW) document.getElementById("tricks" + i.toString()).className = "btn btn-invisible"
        if ((model.Declarer == "East" || model.Declarer == "West") && i > 13 - model.TricksNS) document.getElementById("tricks" + i.toString()).className = "btn btn-invisible"
    }
});

function claimButtonClick() {
    if (claimMade) {
        claimTricksNumber = -1;
        $("#claimTricksModal").modal("show");
    }
    else {
        $("#claimExposeModal").modal("show");
    }
} 

function claimExpose() {
    $("#claimExposeModal").modal("hide");
    request.open('get', sendClaimExposeUrl, true);
    request.send();
}

function claimTricks(iTricks) {
    if (iTricks == claimTricksNumber) {
        document.getElementById("tricks" + claimTricksNumber.toString()).className = "btn btn-lg btn-primary pb-2";
        claimTricksNumber = -1;
        document.getElementById("claimTricksYes").disabled = true;
    }
    else {
        if (claimTricksNumber != -1) document.getElementById("tricks" + claimTricksNumber.toString()).className = "btn btn-lg btn-primary pb-2";
        claimTricksNumber = iTricks;
        document.getElementById("tricks" + claimTricksNumber.toString()).className = "btn btn-lg btn-warning pb-2";
        document.getElementById("claimTricksYes").disabled = false;
    }
}

function confirmClaimTricks() {
    $("#claimTricksModal").modal("hide");
    request.open('get', sendResultUrl + "&tricks=" + claimTricksNumber.toString(), true);
    request.send();
}
