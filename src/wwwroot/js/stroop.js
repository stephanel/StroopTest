var KeyCodes = {
    SPACE: 32,
    LEFT: 37,
    RIGHT: 39,
    // A: 97,
    // E: 101,
};

var startTime = new Date();

document.onkeydown = function (e) {
    console.log('keydown');
    e = e || window.event;
    var charCode = (typeof e.which == "number") ? e.which : e.keyCode
    if (Object.values(KeyCodes).indexOf(charCode) > -1) {

        var elapsedTime = (new Date()) - startTime;

        if (currentPageIsTestPhaseStarter()) {
            if (charCode == KeyCodes.SPACE) {
                start();
            }
        }
        else
        {

            switch (charCode) {

                case KeyCodes.LEFT:
                    goNextStep(true, elapsedTime);
                    break;

                case KeyCodes.RIGHT:
                    goNextStep(false, elapsedTime);
                    break;
            }

        }
    }
}


function start() {
    document.onkeydown = null;
    window.location = startUrl;
}

function goNextStep(sameColor, elapsedTime) {
    document.onkeydown = null;
    $('#sameColorField').val(sameColor);
    $('#elapsedTimeField').val(elapsedTime);
    $('#globalForm').submit();
}

function currentPageIsTestPhaseStarter() {
    return typeof isTestPhaseStarter !== 'undefined';
}