var isSubmitted = false;

var pollRanking = new XMLHttpRequest();
pollRanking.onload = pollRankingListener;
setTimeout(function () {
    pollRanking.open('get', pollRankingUrl, true);
    pollRanking.send();
}, 10000);

function pollRankingListener() {
   rankingList = JSON.parse(this.responseText);
    if (twoWinners) {
        var new_tbodyNS = document.createElement("tbodyNS");
        var new_tbodyEW = document.createElement("tbodyEW");
        for (var i = 0; i < rankingList.length; i++) {
            var row = null;
            if (rankingList[i].Orientation = "E") {
                row = new_tbodyEW.insertRow(i);
                if ((direction == "East" || direction == "West") && rankingList[i].PairNumber == pairNumber) row.className = "table-warning";
            }
            else {
                row = new_tbodyNS.insertRow(i);
                if ((direction == "North" || direction == "South") && rankingList[i].PairNumber == pairNumber) row.className = "table-warning";
            }
            var cellRank = row.insertCell(0);
            var cellPairNumber = row.insertCell(1);
            var cellScore = row.insertCell(2);
            cellRank.innerHTML = rankingList[i].Rank;
            cellPairNumber.innerHTML = rankingList[i].PairNumber;
            cellScore.innerHTML = rankingList[i].Score;
        }
        var old_tbodyNS = document.getElementById("tableBodyNS");
        old_tbodyNS.parentNode.replaceChild(new_tbodyNS, old_tbodyNS);
        new_tbodyNS.id = "tableBodyNS";
        var old_tbodyEW = document.getElementById("tableBodyEW");
        old_tbodyEW.parentNode.replaceChild(new_tbodyEW, old_tbodyEW);
        new_tbodyEW.id = "tableBodyEW";
    }
    else {
        var new_tbody = document.createElement("tbody");
        for (var i = 0; i < rankingList.length; i++) {
            var row = new_tbody.insertRow(i);
            if (rankingList[i].PairNumber == pairNumber) row.className = "table-warning";
            var cellRank = row.insertCell(0);
            var cellPairNumber = row.insertCell(1);
            var cellScore = row.insertCell(2);
            cellRank.innerHTML = rankingList[i].Rank;
            cellPairNumber.innerHTML = rankingList[i].PairNumber;
            cellScore.innerHTML = rankingList[i].Score;
        }
        var old_tbody = document.getElementById("tableBody");
        old_tbody.parentNode.replaceChild(new_tbody, old_tbody);
        new_tbody.id = "tableBody";
    }
    setTimeout(function () {
        pollRanking.open('get', pollRankingUrl, true);
        pollRanking.send();
    }, 10000);
}

function OKButtonClick() {
    if (!isSubmitted) {
        isSubmitted = true;
        if (finalRankingList) {
            location.href = endScreenUrl;
        }
        else {
            location.href = moveUrl;
        }
    }
}

