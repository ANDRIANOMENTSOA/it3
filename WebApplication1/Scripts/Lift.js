//var symphony = "/symphony";
var symphony = "";
function setdate1(id,id1,id2) {
    $("#" + id).datepicker({
        changeYear: true,
        yearRange: "2010:2035",
        dateFormat: "dd-M-yy"
    }).datepicker("show");
    $("#"+id1).prop("checked", true);
    $("#"+id2).prop("checked", true);
}
function setCriteriaFlownRevenue() {
    let Period = $("#Period").val();
    let url = symphony + "/Lift/LoadFlownRevenue"
    $.ajax({
       
            type: "POST",
            url: url,
            data: { Period: Period},

            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {
                $("#flownData").html(data);
                console.log(data);

            },
            complete: function () {
                $('.ajax-loader').css("visibility", "hidden");
                var cmpt = $('#compt').val();
               

            },
            error: function () {
                $('#errorModal').modal('show');
            }
        });
}

function setFlN() {
    let dateFrom = $("#dateFromTfcflown").val();
    let dateTo = $("#dateToTfcflown").val();
    let AgCode = $("#AgCode").val();
    let url = symphony + "/Lift/getflightNum";
    if ($("#testfln").val() != dateFrom) {
        $("#testfln").val(dateFrom);
        $.ajax({
            type: 'POST',
            url: url,
            data: { dateFrom: dateFrom, dateTo: dateTo, AgCode: AgCode },
            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {
                $("#flightNum").html(data);
                console.log(data);
            },
            complete: function () {
                $('.ajax-loader').css("visibility", "hidden");
            },
            error: function () {
                $('#errorModal').modal('show');
            }
        });
    }

}

function chercherefnLift(event) {
    var $form = $(event).closest('form');
    let parentId = $form.attr('id');

    let myvar = $("#" + parentId + " #AgCTrans").val();
    let url = symphony + "/Sales/AgentRefund";
    $.ajax({
        type: "POST",
        url: url,
        data: { value: myvar },
        // dataType: "text",
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (msg) {
            $('.autocomplete').html(msg);
        }, error: function (msg) {
            $('#errorModal').modal('show');
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
        }
    });

}

function setAgcodeLift(event) {

    var $form2 = $(event).closest('markelement');
    let grandParentId = $form2.attr('id');

    let dateFrom = $("#" + grandParentId + " [name=dateFrom]").val();
    let dateTo = $("#" + grandParentId + " [name=dateTo]").val();

    let flightNum = $("#" + grandParentId + " #flightNum").val();
    let url = symphony + "/Lift/getAgcode";

    if ($("#" + grandParentId + " #testTrans").val() != dateFrom) {
        $("#" + grandParentId + " #testTrans").val(dateFrom);
        flightNum
        $.ajax({
            type: 'POST',
            url: url,
            data: { dateFrom: dateFrom, dateTo: dateTo, flightNum: flightNum },
            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {
                console.log(data);
                $("#" + grandParentId + " #AgCode").html(data);

            },
            complete: function () {
                $('.ajax-loader').css("visibility", "hidden");
            },
            error: function () {
                $('#errorModal').modal('show');
            }
        });
    }
}

function setCriteriaTfcsFlown() {
    let dateFrom = $("#dateFromTfcflown").val();
    let dateTo = $("#dateToTfcflown").val();
    let flightNum = $("#flightNum").val();
    let AgCode = $("#AgCode").val();
    let url = symphony + "/Lift/LoadTfcsFlown";
    $.ajax({
        type: 'POST',
        url: url,
        data: { dateFrom: dateFrom, dateTo: dateTo, flightNum: flightNum, AgCode: AgCode },
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#Tfcsdata").html(data);
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}

function ClearFlownRevenue(){
    let url = symphony + "/Lift/FlownRevenue";
    $.ajax({
        type: "POST",
        url: url,
        success: function (msg) {
            $('.tab-cancellation').html(msg);
        }, error: function (msg) {
            $('#errorModal').modal('show');
        }
    });
}
/***************Compare***************************/
function setCompFln() {
    let dateFrom = $("#dateFromCompare").val();
    let dateTo = $("#dateToCompare").val();
    let url = symphony + "/Lift/getCompFln";
    if ($("#testfln1").val() != dateFrom || $("#testfln2").val()!=dateTo) {
        $("#testfln1").val(dateFrom);
        $("#testfln2").val(dateTo)

        $.ajax({
            type: 'POST',
            url: url,
            data:{dateFrom:dateFrom, dateTo:dateTo},
            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {
                $("#CompFln").html(data);
                console.log(data);
            },
            complete: function () {
                $('.ajax-loader').css("visibility", "hidden");
            },
            error: function () {
                $('#errorModal').modal('show');
            }
        });
    }
}

function setCriteriaCompare() {
    let dateFrom = $("#dateFromCompare").val();
    let dateTo = $("#dateToCompare").val();
    console.log(dateFrom)
    let flnNum = $("#CompFln").val();
    let url = symphony + "/Lift/loadCompare";
    $.ajax({
        type: 'POST',
        url: url,
        data:{dateFrom: dateFrom, dateTo: dateTo, flnNum: flnNum },
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#loadFlownR").html(data);
            console.log(data);
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}
function ClearCompare() {
    let url = symphony + "/Lift/Compare";
    $.ajax({
        type: "POST",
        url: url,
        success: function (msg) {
            $('.tab-cancellation').html(msg);
        }, error: function (msg) {
            $('#errorModal').modal('show');
        }
    });
}


/*******************************endCompare***********************************/
/*****************************begingPassenger********************************/


function LiftsetNumericode() {
    let c =$('.nav-tabs .active').text().slice(14);
    let a = $('#agent-code-label').text();
    let b = $("#agent-name-label").text();
    var adress;
    var agcode;
    var name;
    var loc;
    if (c == "Lift For Transportation") {
        adress = "resultAdrTransP";
        agcode = "AgCTransP";
        name = "AgNTransP";
        loc = "locTransP";
    }
    else if (c == "Lift For Exchange") {
        adress = "resultAdrExc";
        agcode = "AgCExc"
        name = "AgNExc";
        loc = "locEx";
    }
    else if (c == "Lift For Refund") {
        adress = "resultAdrRfn";
        agcode = "AgCrfn";
        name = "AgNrfn";
        loc = "locrfn";
    }
    else if (c == "Passenger Lift Reports") {
        adress = "resultAdr";
        agcode = "AgCPass";
        name = "AgNPass";
        loc = "locPass";
    }
    //$('#AgCTrans option[value="1"]').text(a);
    $('#'+agcode+' option[value="1"]').text(a);
    //$("#AgCTrans").val(1);
    $("#" + agcode).val(1);
    $("#AgNTrans").val(b);
    let url = symphony + "/Lift/loadAgAdresse";
    $.ajax({
        type: 'POST',
        url: url,
        data: { agcode: a, id2:name, adresse: loc },
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("." + adress).html(data);
           // $("#"+agcode).val(a)
            console.log(data);
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}
function LiftsetNumericode1() {
    let a = $('#agent-code-label').text();
    let b = $("#agent-name-label").text();
    $('#AgCTrans option[value="1"]').text(a);
    $("#AgCTrans").val(1);
    $("#AgNTrans").val(b);
    let url = symphony + "/Lift/loadAgAdresse1";
    $.ajax({
        type: 'POST',
        url: url,
        data: { agcode: a },
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $(".resultAdr").html(data);
            console.log(data);
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}
function getFlightNoSectP(myId, Bdate, Edate, sector, idTest, sectTest, id2, test1, test2) {
    let dateFrom = $("#" + Bdate).val();
    let dateTo = $("#" + Edate).val();
    var myId = myId;
    var sector = sector;
    var test = idTest;

    let url = symphony + "/Lift/LoadSector"
   
    if ($("#" + test1).val() != dateFrom || $("#" + test2).val() != dateTo) {
        //$("#" + test1).val(dateFrom);
        oups = $("#" + test1).val();

        $("#" + test2).val(dateTo);

        $.ajax({
            type: 'POST',
            url: url,
            data: { dateFrom: dateFrom, dateTo: dateTo, myId: myId, sector: sector, sectTest: sectTest, test: test, Bdate: Bdate, Edate: Edate, id: id2, test1: test1, test2: test2 },
            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {
                $("#" + id2).html(data);

            },
            complete: function () {
                $('.ajax-loader').css("visibility", "hidden");
                if ($("#" + id2).val() != "-All-") {
                    var nameSector = $("#" + myId).val();
                    var pos = nameSector.indexOf('-');
                    var inputSector = nameSector.slice(pos + 1);

                }
                
            },
            error: function () {
                $('#errorModal').modal('show');
            }
        });
    }
    
    if ($("#" + myId).val() != "-All-") {
       
        var check;
        if ($("#ownTransP").is(':checked') && !$("#oalTransP").is(':checked')) {
            check = 'Y';

        }
        else if (!$("#ownTransP").is(':checked') && $("#oalTransP").is(':checked')) {
            check = 'N';
        }
        else {
            check = '%';
        }

        $("#" + sectTest).val($("#" + myId).val());
      
        var nameSector = $("#" + myId).val();
        var pos = nameSector.indexOf('-');
        var inputSector = nameSector.slice(pos + 1);
        
        $("#" + sector).val(inputSector);

        let url = symphony + "/Lift/loadTransportation";
        let agc = $("#AgCTransP").val();
        let fln = ($("#" + myId)).val();

        $.ajax({
            type: 'POST',
            url: url,
            data: { dateFrom: dateFrom, dateTo: dateTo, agc: agc, check: check, fln: fln },
            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {
                $("#display-transP").html(data);
                $("#countTransP").val($("#transcountP").val());
                $("#netTransP").val($("#transTotP").val());
                console.log(data);

            },
            complete: function () {
                $('.ajax-loader').css("visibility", "hidden");
            },
            error: function () {
                $('#errorModal').modal('show');
            }
        });




    }
   
}
function setAgcodeLift1(myId,Bdate,Edate, test, adresse, myid2,test1,statut, dispres ) {
    var dateFrom = $("#"+Bdate).val();
    var dateTo = $("#"+Edate).val();
    var id = myId;
    var id2 = myid2
  
    //var AgAdr = adresse;
    var myTest = test;
    var myTest1 = test1
    let url = symphony + "/Lift/getAgcodePass"
    if ($("#" + myTest).val() != dateFrom || $("#" + myTest1).val() != dateTo) {
        $("#" + myTest).val(dateFrom);
        $("#" + myTest1).val(dateTo)
        $.ajax({
            type: 'POST',
            url: url,
            data: { dateFrom: dateFrom, dateTo: dateTo, statut: statut },
            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {
                $("#"+id).html(data);
                console.log(data);
            },
            complete: function () {
                $('.ajax-loader').css("visibility", "hidden");
            },
            error: function () {
                $('#errorModal').modal('show');
            }
        });
    }
    if ($("#" + id).val() != "-All-") {
        let AgAdr = $("#" + id).val();
        $.ajax({
            type: 'POST',
            url: symphony + "/Lift/loadAgAdresse",
            data: { agcode: AgAdr, adresse: adresse, id2: id2 },
            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {
                $("." + dispres).html(data);
                console.log(data);
            },
            complete: function () {
                $('.ajax-loader').css("visibility", "hidden");
            },
            error: function () {
                $('#errorModal').modal('show');
            }
        });
        $("#" + id).val(AgAdr)
    }

}
function setAgcodeLift2(myId, Bdate, Edate, test, adresse,statut) {
    var dateFrom = $("#" + Bdate).val();
    var dateTo = $("#" + Edate).val();
    var id = myId;
    //var id2 = myid2
    //var AgAdr = adresse;
    var myTest = test;
    let url = symphony + "/Lift/getAgcodePass"
    if ($("#testTrans").val() != dateFrom || $("#testTrans1").val() != dateTo) {
        $("#testTrans").val(dateFrom);
        $("#testTrans1").val(dateTo);
        $.ajax({
            type: 'POST',
            url: url,
            data: { dateFrom: dateFrom, dateTo: dateTo, statut: statut },
            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {
                $("#" + id).html(data);

            },
            complete: function () {
                $('.ajax-loader').css("visibility", "hidden");
            },
            error: function () {
                $('#errorModal').modal('show');
            }
        });
    }
    if ($("#" + id).val() != "-All-") {
        let AgAdr = $("#" + id).val();
        $.ajax({
            type: 'POST',
            url: symphony + "/Lift/loadAgAdresse1",
            data: { agcode: AgAdr },
            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {
                $(".resultAdr").html(data);
                console.log(data);
            },
            complete: function () {
                $('.ajax-loader').css("visibility", "hidden");
            },
            error: function () {
                $('#errorModal').modal('show');
            }
        });
    }
}
function setCriteriaTransportationP() {
    var check;
    if ($("#ownTransP").is(':checked') && !$("#oalTransP").is(':checked')) {
        check = 'Y';

    }
    else if (!$("#ownTransP").is(':checked') && $("#oalTransP").is(':checked')) {
        check = 'N';
    }
    else {
        check = '%';
    }
    let dateFrom = $("#dateFromP").val();
    let dateTo = $("#dateToP").val();
    let agc = $("#AgCTransP").val();
    let fln = $("#FlightTransP").val()
   
    let url = symphony + "/Lift/loadTransportation";
    $.ajax({
        type: 'POST',
        url: url,
        data: { dateFrom: dateFrom, dateTo: dateTo, agc: agc, check: check, fln: fln },
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#display-transP").html(data);
            $("#countTransP").val($("#transcountP").val());
            $("#netTransP").val($("#transTotP").val());
            if ($("#countTransP").val() == 0) {
                $('#alertModal').modal('show');
                if (check == 'N') {
                    $('#msgalert').text('No Other Airline Involved (OAL) in this Flight');
                }
                else {
                    $('#msgalert').text('No data available');
                }

            }
           
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });

}
function setCriteriaTransportation(event) {

    var $form = $(event).closest('form');
    let parentId = $form.attr('id');

    var $form2 = $(event).closest('markelement');
    let grandParentId = $form2.attr('id');

    var check;
    if ($("#" + parentId + " #ownTrans").is(':checked') && !$("#" + parentId + " #oalTrans").is(':checked')) {
        check = 'Y';

    }
    else if (!$("#" + parentId + " #ownTrans").is(':checked') && $("#" + parentId + " #oalTrans").is(':checked')) {
        check = 'N';
    }
    else {
        check = '%';
    }

    console.log(check);

    let dateFrom = $("#" + parentId + " [name=dateFrom]").val();
    let dateTo = $("#" + parentId + " [name=dateTo]").val();

    let agc = $("#" + parentId + " #AgCTrans").val();
    let fln = $("#" + parentId + " #FlightTrans").val()
    let url = symphony + "/Lift/loadTransportation";
    $.ajax({
        type: 'POST',
        url: url,
        data: { dateFrom: dateFrom, dateTo: dateTo, agc: agc, check: check, fln: fln },
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $('#' + grandParentId + ' #display-trans').html(data);
            $('#' + grandParentId + ' #countTrans').val($('#' + grandParentId + ' #transcountP').val());
            $('#' + grandParentId + ' #netTrans').val($('#' + grandParentId + ' #transTotP').val());

            if ($('#' + grandParentId + ' #countTrans').val() == 0) {
                $('#alertModal').modal('show');
                if (check == 'N') {
                    $('#msgalert').text('No Other Airline Involved (OAL) in this Flight');
                }
                else {
                    $('#msgalert').text('No data available');
                }


            }
           
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}

function setCriteriaExchanges() {
    let dateFrom = $("#dateFromExc").val();
    let dateTo = $("#dateToExc").val();
    let agc = $("#AgCExc").val();
    let fln = $("#flnEx").val();
    var check;
    if ($("#ownExc").is(':checked') && !$("#oalExc").is(':checked')) {
        check = 'Y';

    }
    else if (!$("#ownExc").is(':checked') && $("#oalExc").is(':checked')) {
        check = 'N';
    }
    else {
        check = '%';
    }
    let url = symphony + "/Lift/loadExchange";
    $.ajax({
        type: 'POST',
        url: url,
        data: { dateFrom: dateFrom, dateTo: dateTo, agc: agc, check: check, fln:fln },
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#display-exc").html(data);
            $("#countExc").val($("#exccount").val());
            $("#netExc").val($("#excTot").val());
            if ($("#countExc").val() == 0) {
                $('#alertModal').modal('show');
                $('#msgalert').text('No data available');
                
            }
            console.log(data);
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });

}

function setCriteriaRefundLift() {
    var check;
    if ($("#ownrfn").is(':checked') && !$("#oalrfn").is(':checked')) {
        check = 'Y';

    }
    else if (!$("#ownrfn").is(':checked') && $("#oalrfn").is(':checked')) {
        check = 'N';
    }
    else {
        check = '%';
    }
    let dateFrom = $("#dateFromrfn").val();
    let dateTo = $("#dateTorfn").val();
    let agc = $("#AgCrfn").val();
    let url = symphony + "/Lift/loadRefundLift";
    let fln = $("#flnrfn").val();
   
    $.ajax({
        type: 'POST',
        url: url,
        data: { dateFrom: dateFrom, dateTo: dateTo, agc: agc, check: check,fln:fln },
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#display-rfn").html(data);
            $("#countrfn").val($("#rfncount").val());
            $("#netrfn").val($("#rfnTot").val());
            if ($("#countrfn").val() == 0) {
                $('#alertModal').modal('show');
                $('#msgalert').text('No data available');
            }
            console.log(data);
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });

}
function showreports(id, classe) {
    let idCkeck = "";
    if ($("#" + id).is(':checked')) {

        if (id == "allsumm" || id == "allfaire" || id == "allkey" || id == "allkeyFR" || id == "allpasse" || id == "allkeySpec" || id == "allkeyDisc") {
            $('ul.list-group li').each(function (idx, li) {
                idCkeck = $(li).find('input:checkbox').attr('id');
                $("#" + idCkeck).prop("checked", true);

            });
            for (i = 0; i < 68; i++) {
                $('.' + classe + '-' + i).show();
            }
        } else {
            $('.' + classe).show();
        }

    } else {
        if (id == "allsumm" || id == "allfaire" || id == "allkey" || id == "allkeyFR" || id == "allpasse" || id == "allkeySpec" || id == "allkeyDisc") {
            $('ul.list-group li').each(function (idx, li) {
                idCkeck = $(li).find('input:checkbox').attr('id');
                $("#" + idCkeck).prop("checked", false);

            });
            for (i = 0; i < 68; i++) {
                $('.' + classe + '-' + i).hide();
            }
        } else {
            $('.' + classe).hide();
        }

    }
}

function showreportsdatasals(id, classe) {
    let idCkeck = "";
    if ($("#" + id).is(':checked')) {

        if (id == "allsumm" || id == "allfaire" || id == "allkey" || id == "allkeyFR" || id == "allpasse" || id == "allkeySpec" || id == "allkeyDisc") {
            $('ul.list-group li').each(function (idx, li) {
                idCkeck = $(li).find('input:checkbox').attr('id');
                $("#" + idCkeck).prop("checked", true);

            });
            for (i = 0; i < 68; i++) {
                $('.' + classe + '-' + i).show();
            }
        } else {
            $('.' + classe).show();
        }

    } else {
        if (id == "allsumm" || id == "allfaire" || id == "allkey" || id == "allkeyFR" || id == "allpasse" || id == "allkeySpec" || id == "allkeyDisc") {
            $('ul.list-group li').each(function (idx, li) {
                idCkeck = $(li).find('input:checkbox').attr('id');
                $("#" + idCkeck).prop("checked", false);

            });
            for (i = 0; i < 68; i++) {
                $('.' + classe + '-' + i).hide();
            }
        } else {
            $('.' + classe).hide();
        }

    }
}

function showreportspricing(id, classe) {
    let idCkeck = "";
    if ($("#" + id).is(':checked')) {

        if (id == "allsumm" || id == "allfaire" || id == "allkey" || id == "allkeyFR" || id == "allpasse" || id == "allkeySpec" || id == "allkeyDisc") {
            $('ul.list-group li').each(function (idx, li) {
                idCkeck = $(li).find('input:checkbox').attr('id');
                $("#" + idCkeck).prop("checked", true);

            });
            for (i = 0; i < 68; i++) {
                $('.' + classe + '-' + i).show();
            }
        } else {
            $('.' + classe).show();
        }

    } else {
        if (id == "allsumm" || id == "allfaire" || id == "allkey" || id == "allkeyFR" || id == "allpasse" || id == "allkeySpec" || id == "allkeyDisc") {
            $('ul.list-group li').each(function (idx, li) {
                idCkeck = $(li).find('input:checkbox').attr('id');
                $("#" + idCkeck).prop("checked", false);

            });
            for (i = 0; i < 68; i++) {
                $('.' + classe + '-' + i).hide();
            }
        } else {
            $('.' + classe).hide();
        }

    }
}

function showPassenger(myid) {
    var etat = document.getElementById(myid).style.display;
    if (etat == 'none') {
        document.getElementById(myid).style.display = 'block';
    }
    else {
        document.getElementById(myid).style.display = 'none';
    }
}

function getFlightNo(myId,Bdate,Edate) {
    let dateFrom = $("#" + Bdate).val();
    let dateTo = $("#"+Edate).val();
    var myId = myId;
    console.log(myId);
    let url = symphony + "/Lift/getFlightNo"
    if ($("#testAnalysis").val() != dateFrom) {
        $("#testAnalysis").val(dateFrom);
        $.ajax({
            type: 'POST',
            url: url,
            data: { dateFrom: dateFrom, dateTo: dateTo },
            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {
                $("#" + myId).html(data);
                console.log(data);
            },
            complete: function () {
                $('.ajax-loader').css("visibility", "hidden");
            },
            error: function () {
                $('#errorModal').modal('show');
            }
        });
    }
}

function getFlightNoSect(event,myId, Bdate, Edate, sector, idTest, sectTest, id2, test1, test2) {
    var $form = $(event).closest('form');
    let parentId = $form.attr('id');

    var $form2 = $(event).closest('markelement');
    let grandParentId = $form2.attr('id');

    let dateFrom = $("#" + parentId + " #"+ Bdate).val();
    let dateTo = $("#" + parentId + " #"+ Edate).val();

    let url = symphony + "/Lift/LoadSector"
    if ($("#" + parentId + " #" + test1).val() != dateFrom || $("#" + parentId + " #" + test2).val() != dateTo) {
        //$("#" + test1).val(dateFrom);
        oups = $("#" + parentId + " #" + test1).val();
        
        $("#" + parentId + " #" + test2).val(dateTo);
        
        $.ajax({
            type: 'POST',
            url: url,
            data: { dateFrom: dateFrom, dateTo: dateTo, myId: myId, sector: sector, sectTest: sectTest, test: idTest, Bdate: Bdate, Edate: Edate, id: id2, test1: test1, test2: test2 },
            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {
                $("#" + parentId + " #" + id2).html(data);
                
            },
            complete: function () {
                $('.ajax-loader').css("visibility", "hidden");
                if ($("#" + parentId + " #" + id2).val() != "-All-") {
                    var nameSector = $("#" + parentId + " #" + myId).val();
                    var pos = nameSector.indexOf('-');
                    var inputSector = nameSector.slice(pos + 1);
               
                }
            },
            error: function () {
                $('#errorModal').modal('show');
            }
        });
    }
    /*$("#" + sectTest).val($("#" + myId).val());
    console.log(myId + ":" + $("#" + myId).val());
    if ($("#" + sectTest).val() != "-All-") {
        var nameSector = $("#" + myId).val();
        var pos = nameSector.indexOf('-');
        var inputSector = nameSector.slice(pos + 1);
        console.log(inputSector);
        $("#" + sector).val(inputSector);
    
    }*/
    if ($("#" + parentId + " #" + myId).val() != "-All-") {
        var check;
        if ($("#" + parentId + " #ownTrans").is(':checked') && !$("#" + parentId + " #oalTrans").is(':checked')) {
            check = 'Y';

        }
        else if (!$("#" + parentId + " #ownTrans").is(':checked') && $("#" + parentId + " #oalTrans").is(':checked')) {
            check = 'N';
        }
        else {
            check = '%';
        }

        $("#" + parentId + " #" + sectTest).val($("#" + parentId + " #" + myId).val());

        var nameSector = $("#" + parentId + " #" + myId).val();
        var pos = nameSector.indexOf('-');
        var inputSector = nameSector.slice(pos + 1);
        $("#" + parentId + " #" + sector).val(inputSector);

        let url = symphony + "/Lift/loadTransportation";
        let agc = $("#" + parentId + " #AgCTrans").val();
        let fln = ($("#" + parentId + " #" + myId)).val();

        $.ajax({
            type: 'POST',
            url: url,
            data: { dateFrom: dateFrom, dateTo: dateTo, agc: agc, check: check, fln: fln },
            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {
                $("#" + parentId + " #display-trans").html(data);
                $("#" + parentId + " #countTrans").val($("#" + parentId + " #transcount").val());
                $("#" + parentId + " #netTrans").val($("#" + parentId + " #transTot").val());
                console.log(data);
            },
            complete: function () {
                $('.ajax-loader').css("visibility", "hidden");
            },
            error: function () {
                $('#errorModal').modal('show');
            }
        });
    }
}

function getFlightNoSect1(event, myId, Bdate, Edate, sector, idTest, sectTest, id2, test1, test2) {
    var $form = $(event).closest('form');
    let parentId = $form.attr('id');

    var $form2 = $(event).closest('markelement');
    let grandParentId = $form2.attr('id');

    let dateFrom = $("#" + parentId + " #" + Bdate).val();
    let dateTo = $("#" + parentId + " #" + Edate).val();

    let url = symphony + "/Lift/LoadSectorOwn"

    if ($("#" + parentId + " #" + test1).val() != dateFrom || $("#" + parentId + " #" + test2).val() != dateTo) {

        oups = $("#" + parentId + " #" + test1).val();

        $("#" + parentId + " #" + test2).val(dateTo);

        $.ajax({
            type: 'POST',
            url: url,
            data: { dateFrom: dateFrom, dateTo: dateTo, myId: myId, sector: sector, sectTest: sectTest, test: idTest, Bdate: Bdate, Edate: Edate, id: id2, test1: test1, test2: test2 },
            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {
                $("#" + parentId + " #" + id2).html(data);
            },
            complete: function () {
                $('.ajax-loader').css("visibility", "hidden");
                if ($("#" + parentId + " #" + id2).val() != "-All-") {
                    var nameSector = $("#" + parentId + " #" + myId).val();
                    var pos = nameSector.indexOf('-');
                    var inputSector = nameSector.slice(pos + 1);
                }
            },
            error: function () {
                $('#errorModal').modal('show');
            }
        });
    }

    if ($("#" + parentId + " #" + myId).val() != "-All-") {
        var valno = $("#" + parentId + " #" + myId).val();

        var check;
        if ($("#" + parentId + " #ownTrans").is(':checked') && !$("#" + parentId + " #oalTrans").is(':checked')) {
            check = 'Y';

        }
        else if (!$("#" + parentId + " #ownTrans").is(':checked') && $("#" + parentId + " #oalTrans").is(':checked')) {
            check = 'N';
        }
        else {
            check = '%';
        }
        $("#" + parentId + " #" + sectTest).val($("#" + parentId + " #" + myId).val());
        var nameSector = $("#" + parentId + " #" + myId).val();
        var pos = nameSector.indexOf('-');
        var inputSector = nameSector.slice(pos + 1);
        $("#" + parentId + " #" + sector).val(inputSector);
        let url = symphony + "/Lift/loadTransportation";
        let agc = $("#" + parentId + " #AgCTrans").val();
        let fln = ($("#" + parentId + " #" + myId)).val();
        if ($("#" + parentId + " #testtrans").val() != $("#" + parentId + " #" + myId).val()) {
            $.ajax({
                type: 'POST',
                url: url,
                data: { dateFrom: dateFrom, dateTo: dateTo, agc: agc, check: check, fln: fln },
                beforeSend: function () {
                    $('.ajax-loader').css("visibility", "visible");
                },
                success: function (data) {
                    $("#" + parentId + " #display-trans").html(data);
                    $("#" + parentId + " #countTrans").val($("#transcount").val());
                    $("#" + parentId + " #netTrans").val($("#transTot").val());
                    if ($("#" + parentId + " #countTrans").val() == 0) {

                        $('#alertModal').modal('show');
                        $('#msgalert').text('No data for this selected period');
                    }
                    $("#" + parentId + " #testtrans").val($("#" + myId).val());
                },
                complete: function () {
                    $('.ajax-loader').css("visibility", "hidden");

                },
                error: function () {
                    $('#errorModal').modal('show');
                }
            });
        }
    }

}

function getFlightNoSectExc(myId, Bdate, Edate, sector, idTest, sectTest, id2, test1, test2) {
    let dateFrom = $("#" + Bdate).val();
    let dateTo = $("#" + Edate).val();
    var myId = myId;
    var sector = sector;
    var test = idTest;

    if ($("#" + test1).val() != dateFrom || $("#" + test2).val() != dateTo) {
        $("#" + test1).val(dateFrom);
        $("#" + test2).val(dateTo);
        //if ($("#" + sectTest).val() == "-All-") {
        // $("#" + sectTest).val("-All-");
        let url = symphony + "/Lift/LoadSectorExc"
        console.log($("#" + test).val(), dateFrom)

        $.ajax({
            type: 'POST',
            url: url,
            data: { dateFrom: dateFrom, dateTo: dateTo, myId: myId, sector: sector, sectTest: sectTest, test: test, Bdate: Bdate, Edate: Edate, id: id2, test1: test1, test2: test2 },
            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {
                $("#" + id2).html(data);

            },
            complete: function () {
                $('.ajax-loader').css("visibility", "hidden");
                if ($("#" + id2).val() != "-All-") {
                    var nameSector = $("#" + myId).val();
                    var pos = nameSector.indexOf('-');
                    var inputSector = nameSector.slice(pos + 1);

                }
                console.log($("#" + myId).val());
            },
            error: function () {
                $('#errorModal').modal('show');
            }
        });

    }
    if ($("#" + myId).val() != "-All-") {

        var check;
        if ($("#ownrfn").is(':checked') && !$("#oalrfn").is(':checked')) {
            check = 'Y';

        }
        else if (!$("#ownrfn").is(':checked') && $("#oalrfn").is(':checked')) {
            check = 'N';
        }
        else {
            check = '%';
        }

        $("#" + sectTest).val($("#" + myId).val());
        console.log(myId + ":" + $("#" + myId).val());

        var nameSector = $("#" + myId).val();
        var pos = nameSector.indexOf('-');
        var inputSector = nameSector.slice(pos + 1);
        console.log(inputSector);
        $("#" + sector).val(inputSector);

        console.log(check);
        let url = symphony + "/Lift/loadExchange";
        let agc = $("#AgCrfn").val();
        let fln = ($("#" + myId)).val();

        $.ajax({
            type: 'POST',
            url: url,
            data: { dateFrom: dateFrom, dateTo: dateTo, agc: agc, check: check, fln: fln },
            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {
                $("#display-exc").html(data);
                $("#countExc").val($("#exccount").val());
                $("#netExc").val($("#excTot").val());
                if ($("#countExc").val() == 0) {
                    $('#alertModal').modal('show');
                    $('#msgalert').text('No data for this selected period');
                }
                console.log(data);

            },
            complete: function () {
                $('.ajax-loader').css("visibility", "hidden");
            },
            error: function () {
                $('#errorModal').modal('show');
            }
        });




    }
}

function getFlightNoSectRfn(myId, Bdate, Edate, sector, idTest, sectTest, id2, test1, test2) {
    let dateFrom = $("#" + Bdate).val();
    let dateTo = $("#" + Edate).val();
    var myId = myId;
    var sector = sector;
    var test = idTest;

    if ($("#" + test1).val() != dateFrom || $("#" + test2).val() != dateTo) {
        $("#" + test1).val(dateFrom);
        $("#" + test2).val(dateTo);
        //if ($("#" + sectTest).val() == "-All-") {
        // $("#" + sectTest).val("-All-");
        let url = symphony + "/Lift/LoadSectorRfn"
        console.log($("#" + test).val(), dateFrom)

        $.ajax({
            type: 'POST',
            url: url,
            data: { dateFrom: dateFrom, dateTo: dateTo, myId: myId, sector: sector, sectTest: sectTest, test: test, Bdate: Bdate, Edate: Edate, id: id2, test1: test1, test2: test2 },
            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {
                $("#" + id2).html(data);

            },
            complete: function () {
                $('.ajax-loader').css("visibility", "hidden");
                if ($("#" + id2).val() != "-All-") {
                    var nameSector = $("#" + myId).val();
                    var pos = nameSector.indexOf('-');
                    var inputSector = nameSector.slice(pos + 1);

                }
                console.log($("#" + myId).val());
            },
            error: function () {
                $('#errorModal').modal('show');
            }
        });

        if ($("#" + myId).val() != "-All-") {

            var check;
            if ($("#ownrfn").is(':checked') && !$("#oalrfn").is(':checked')) {
                check = 'Y';

            }
            else if (!$("#ownrfn").is(':checked') && $("#oalrfn").is(':checked')) {
                check = 'N';
            }
            else {
                check = '%';
            }

            $("#" + sectTest).val($("#" + myId).val());
            console.log(myId + ":" + $("#" + myId).val());

            var nameSector = $("#" + myId).val();
            var pos = nameSector.indexOf('-');
            var inputSector = nameSector.slice(pos + 1);
            console.log(inputSector);
            $("#" + sector).val(inputSector);

            console.log(check);
            let url = symphony + "/Lift/loadRefundLift";
            let agc = $("#AgCrfn").val();
            let fln = ($("#" + myId)).val();

            $.ajax({
                type: 'POST',
                url: url,
                data: { dateFrom: dateFrom, dateTo: dateTo, agc: agc, check: check, fln: fln },
                beforeSend: function () {
                    $('.ajax-loader').css("visibility", "visible");
                },
                success: function (data) {
                    $("#display-rfn").html(data);
                    $("#countrfn").val($("#rfncount").val());
                    $("#netrfn").val($("#rfnTot").val());
                    console.log(data);

                },
                complete: function () {
                    $('.ajax-loader').css("visibility", "hidden");
                },
                error: function () {
                    $('#errorModal').modal('show');
                }
            });


        }

    }
    //} else {
    if ($("#" + myId).val() != "-All-") {
        
        var check;
        if ($("#ownrfn").is(':checked') && !$("#oalrfn").is(':checked')) {
            check = 'Y';

        }
        else if (!$("#ownrfn").is(':checked') && $("#oalrfn").is(':checked')) {
            check = 'N';
        }
        else {
            check = '%';
        }

        $("#" + sectTest).val($("#" + myId).val());
        console.log(myId + ":" + $("#" + myId).val());

        var nameSector = $("#" + myId).val();
        var pos = nameSector.indexOf('-');
        var inputSector = nameSector.slice(pos + 1);
        console.log(inputSector);
        $("#" + sector).val(inputSector);

        console.log(check);
        let url = symphony + "/Lift/loadRefundLift";
        let agc = $("#AgCrfn").val();
        let fln = ($("#" + myId)).val();
       
        $.ajax({
            type: 'POST',
            url: url,
            data: { dateFrom: dateFrom, dateTo: dateTo, agc: agc, check: check, fln: fln },
            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {
                $("#display-rfn").html(data);
                $("#countrfn").val($("#rfncount").val());
                $("#netrfn").val($("#rfnTot").val());
                console.log(data);
                
               

            },
            complete: function () {
                $('.ajax-loader').css("visibility", "hidden");
            },
            error: function () {
                $('#errorModal').modal('show');
            }
        });




    }
}
function setCriteriaAnalysis() {
    var check;
    if ($("#ownAnalysis").is(':checked') && !$("#oalAnalysis").is(':checked')) {
        check = 'Y';

    }
    else if (!$("#ownAnalysis").is(':checked') && $("#oalAnalysis").is(':checked')) {
        check = 'N';
    }
    else {
        check = '%';
    }
    let dateFrom = $("#dateFromAnalysis").val();
    let dateTo = $("#dateToAnalysis").val();
    let fln = $("#FlightAnalysis").val();
    let url = symphony + "/Lift/loadAnalysis";
    $.ajax({
        type: 'POST',
        url: url,
        data: { dateFrom: dateFrom, dateTo: dateTo, fln: fln, check: check },
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#data-analysis").html(data);
            $("#countAnalysis").val($("#anacount").val());
            $("#netAnalysis").val($("#anaTot").val());
            if ($("#countAnalysis").val() == 0) {
                $('#alertModal').modal('show');
                $('#msgalert').text('No data available');
            }
            console.log(data);
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });

}

function ClearTransportation() {
    var today = new Date().toLocaleDateString('en-GB', {
        day: 'numeric',
        month: 'short',
        year: 'numeric'
    }).split(' ').join('-');
    
   
    let d = new Date();
    let madate = d.getDate('dd') + '-' + d.getMonth() - 1 + '-' + d.getFullYear();
    let mydate = d.toISOString();
    
    var da = new Date();
    var month = new Array();
    month[0] = "Jan";
    month[1] = "Feb";
    month[2] = "Mar";
    month[3] = "Apr";
    month[4] = "May";
    month[5] = "Jun";
    month[6] = "Jul";
    month[7] = "Aug";
    month[8] = "Sep";
    month[9] = "Oct";
    month[10] = "Nov";
    month[11] = "Dec";
    var n = month[da.getMonth() -1];
   // alert(n);
    var dateB = d.getDate() + '-' + n + '-' + d.getFullYear();
    //alert(dateB);
    $("#dateFromP").val(dateB);
    $("#dateToP").val(today);
    $("#FlightTransP").val('');
    $("#AgCTransP").val('');
    $("#AgNTransP").val('');
    $("#locTransP").val('');
    $("#countTransP").val('');
    $("#netTransP").val('');
    $("#sectorTransP").val('');
    $("#idTransP").html('');
}

function ClearExchange() {
    var today = new Date().toLocaleDateString('en-GB', {
        day: 'numeric',
        month: 'short',
        year: 'numeric'
    }).split(' ').join('-');
    
   
    let d = new Date();
    let madate = d.getDate('dd') + '-' + d.getMonth() - 1 + '-' + d.getFullYear();
    let mydate = d.toISOString();
    
    var da = new Date();
    var month = new Array();
    month[0] = "Jan";
    month[1] = "Feb";
    month[2] = "Mar";
    month[3] = "Apr";
    month[4] = "May";
    month[5] = "Jun";
    month[6] = "Jul";
    month[7] = "Aug";
    month[8] = "Sep";
    month[9] = "Oct";
    month[10] = "Nov";
    month[11] = "Dec";
    var n = month[da.getMonth() -1];
    // alert(n);
    var dateB = d.getDate() + '-' + n + '-' + d.getFullYear();
    $("#dateFromExc").val(dateB);
    $("#dateToExc").val(today);
    $("#flnEx").val('');
    $("#AgCExc").val('');
    $("#AgNExc").val('');
    $("#locEx").val('');
    $("#countExc").val('');
    $("#netExc").val('');
    $("#sectorExc").val('');
    $("#idEx").html('');
}

function ClearRefundLift() {
    var today = new Date().toLocaleDateString('en-GB', {
        day: 'numeric',
        month: 'short',
        year: 'numeric'
    }).split(' ').join('-');


    let d = new Date();
    let madate = d.getDate('dd') + '-' + d.getMonth() - 1 + '-' + d.getFullYear();
    let mydate = d.toISOString();

    var da = new Date();
    var month = new Array();
    month[0] = "Jan";
    month[1] = "Feb";
    month[2] = "Mar";
    month[3] = "Apr";
    month[4] = "May";
    month[5] = "Jun";
    month[6] = "Jul";
    month[7] = "Aug";
    month[8] = "Sep";
    month[9] = "Oct";
    month[10] = "Nov";
    month[11] = "Dec";
    var n = month[da.getMonth() - 1];
    // alert(n);
    var dateB = d.getDate() + '-' + n + '-' + d.getFullYear();
    $("#dateFromrfn").val(dateB);
    $("#dateTorfn").val(today);
    $("#AgCrfn").val('');
    $("#AgNrfn").val('');
    $("#locrfn").val('');
    $("#testsectrfn").val('');
    $("#flnrfn").val('');
    $("#countrfn").val('');
    $("#netrfn").val('');
    $("#sectorRfn").val('');
    $("#rfnList").html('');
}

function ClearPassenger() {
    var today = new Date().toLocaleDateString('en-GB', {
        day: 'numeric',
        month: 'short',
        year: 'numeric'
    }).split(' ').join('-');


    let d = new Date();
    let madate = d.getDate('dd') + '-' + d.getMonth() - 1 + '-' + d.getFullYear();
    let mydate = d.toISOString();

    var da = new Date();
    var month = new Array();
    month[0] = "Jan";
    month[1] = "Feb";
    month[2] = "Mar";
    month[3] = "Apr";
    month[4] = "May";
    month[5] = "Jun";
    month[6] = "Jul";
    month[7] = "Aug";
    month[8] = "Sep";
    month[9] = "Oct";
    month[10] = "Nov";
    month[11] = "Dec";
    var n = month[da.getMonth() - 1];
    // alert(n);
    var dateB = d.getDate() + '-' + n + '-' + d.getFullYear();
    $("#dateFromProcess").val(dateB);
    $("#dateToProcess").val(today);
    $("#dateFromUsage").val(dateB);
    $("#dateToUsage").val(today);
    $("#AgCPass").val('');
    $("#AgNPass").val('');
    $("#locPass").val('');
    $("#usageType").val('');
    $("#usageOrig").val('');
    $("#usageDest").val('');
    $("#dataSource").val('');
   // $("#rfnList").html('');
}

function ClearAnalysis() {
    var today = new Date().toLocaleDateString('en-GB', {
        day: 'numeric',
        month: 'short',
        year: 'numeric'
    }).split(' ').join('-');


    let d = new Date();
    let madate = d.getDate('dd') + '-' + d.getMonth() - 1 + '-' + d.getFullYear();
    let mydate = d.toISOString();

    var da = new Date();
    var month = new Array();
    month[0] = "Jan";
    month[1] = "Feb";
    month[2] = "Mar";
    month[3] = "Apr";
    month[4] = "May";
    month[5] = "Jun";
    month[6] = "Jul";
    month[7] = "Aug";
    month[8] = "Sep";
    month[9] = "Oct";
    month[10] = "Nov";
    month[11] = "Dec";
    var n = month[da.getMonth() - 1];
    // alert(n);
    var dateB = d.getDate() + '-' + n + '-' + d.getFullYear();
    $("#anacount").val('');
    $("#anaTot").val('');
    $("#FlightAnalysis").val('');
    $("#netAnalysis").val('');
    $("#countAnalysis").val('');
    $("#dateToAnalysis").val(today);
    $("#dateFromAnalysis").val(dateB);
    $("#anacount").val('');
    $("#anaTot").val('');
    //$("#usageDest").val('');
    //$("#dataSource").val('');
    $("#idAnalysis").html('');
}


/******************************endPassenger**********************************/

/****************SalesMatching******************************/
function setCriteriaSalesMatching() {
    let dateFrom = $("#dateFromSalesMatching").val();
    let dateTo = $("#dateToSalesMatching").val();
    let url = symphony + "/Lift/LoadSalesMatching";
    $.ajax({
        type: "POST",
        url: url,
        data: { dateFrom: dateFrom, dateTo: dateTo },

        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#loadSalesMatching").html(data);
            //console.log(data);
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");

        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}
function clearSalesMatching() {
    let url = symphony + "/Lift/SalesMatching";
   
    $.ajax({
        type: "POST",
        url: url,
        success: function (msg) {
            $('.tab-cancellation').html(msg);
        }, error: function (msg) {
            $('#errorModal').modal('show');
        }
    });
    
}


/*******************end********************************/
/****************FlownWithoutSales******************************/
function setCriteriaFlownNoSales() {
    let dateFrom = $("#dateFromFlowWithoutSales").val();
    let dateTo = $("#dateToFlowWithoutSales").val();
    let agNo = $("#agNo").val();
    let chkOAL = $("#FlownClass input[name=chkOAL]:radio:checked").val();
    let chkOWN = $("#FlownClass input[name=chkOAL]:radio:checked").val();
    let url = symphony + "/Lift/LoadFlownWithoutSales";
    //$('.tFlownNo').html('<tr><td></td></tr>')
    $.ajax({
        type: "POST",
        url: url,
        data: { agNo: agNo, dateFrom: dateFrom, dateTo: dateTo, chkOAL: chkOAL, chkOWN: chkOWN },

        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#loadFlownNoSales").html(data);
            //console.log(data);
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
            $("#txtPCount").val($("#txtCount").val());
            $("#txtFAmt").val($("#txtAmt").val());
            var cmpt = $('#compt').val();
            if (cmpt != "") {
                $('#alertModal').modal('show');
                $('#msgalert').text(cmpt);
            }
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}
function clearFlownNoSales() {
    let url = symphony + "/Lift/FlownWithoutSales";
    $.ajax({
        type: "POST",
        url: url,
        success: function (msg) {
            $('.tab-cancellation').html(msg);
        }, error: function (msg) {
            $('#errorModal').modal('show');
        }
    });

}
function cherche3(myvar) {
    let url = symphony + "/Lift/Cherche3ByCode";
    $.ajax({
        type: "POST",
        url: url,
        data: { value: myvar },
        // dataType: "text",
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (msg) {
            $('.autocomplete').html(msg);
        },
        error: function (msg) {
            $('#errorModal').modal('show');
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
        },
    });

}
function putCodeAgent2(code, name) {

    $('#agent-name-label').html(name);
    $('#agent-code-label').html(code);
    $('#tempscode').val(code);
}
/*******************end********************************/
/****************Surcharges Lift******************************/
function setCriteriaSurchargeL() {
    let dateFrom = $("#dateFromSurcharges").val();
    let dateTo = $("#dateToSurcharges").val();
    let cbAgt = $("#cbAgtSurcharge").val();
    let cbS = $("#cbS").val();
    let cbF = $("#cbFSurcharge").val();
    let chkYes = $("#SurchargeClass input[name=chkYes]:radio:checked").val();
    let chkOAL = $("#SurchargeClass input[name=chkOAL]:radio:checked").val();
    let chkOWN = $("#SurchargeClass input[name=chkOAL]:radio:checked").val();
    let url = symphony + "/Lift/LoadSurcharges";
    $.ajax({
        type: "POST",
        url: url,
        data: { dateFrom: dateFrom, dateTo: dateTo, chkYes: chkYes, chkOAL: chkOAL, chkOWN: chkOWN, cbS: cbS, cbAgt: cbAgt, cbF: cbF },

        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#loadSurcharge").html(data);
            //console.log(data);
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
            $("#txtPCount").val($("#txtCount").val());
            $("#txtSurcharge").val($("#txtVat").val());
            $("#txtFare").val($("#txtFares").val());
            var cmpt = $('#compt').val();
            $('#alertModal').modal('show');
            $('#msgalert').text(cmpt);

        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}

/*******************end********************************/
/****************Surcharges Lift******************************/
function setCriteriaVAT() {
    let dateFrom = $("#dateFromVat").val();
    let dateTo = $("#dateToVat").val();
    let cbAgt = $("#cbAgt").val();

    let cbF = $("#cbF").val();
    let chkYes = $("#SurchargeClass input[name=chkYes]:radio:checked").val();
    let chkOAL = $("#SurchargeClass input[name=chkOAL]:radio:checked").val();
    let chkOWN = $("#SurchargeClass input[name=chkOAL]:radio:checked").val();
    let url = symphony + "/Lift/LoadVAT";
    $.ajax({
        type: "POST",
        url: url,
        data: { dateFrom: dateFrom, dateTo: dateTo, chkYes: chkYes, chkOAL: chkOAL, chkOWN: chkOWN, cbAgt: cbAgt, cbF: cbF },

        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#loadVAT").html(data);
            //console.log(data);
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
            $("#txtPCount").val($("#txtCount").val());
            $("#txtVAT").val($("#txtVat").val());
            $("#txtFare").val($("#txtFares").val());
            var cmpt = $('#compt').val();
            $('#alertModal').modal('show');
            $('#msgalert').text(cmpt);
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}
function recupeAgtVAT() {
    let dateFrom = $("#dateFromVat").val();
    let dateTo = $("#dateToVat").val();
    //let dropdownType = $("#dropdownType").val();
    console.log(dateFrom);
    let url = symphony + "/Lift/LoadAgtVAT";
    if ($("#testTrans").val() != dateFrom || $("#testTrans1").val() != dateTo) {
        $("#testTrans").val(dateFrom);
        $("#testTrans1").val(dateTo);
        $.ajax({
            type: "POST",
            url: url,
            data: { dateFrom: dateFrom, dateTo: dateTo },

            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {
                $("#cbAgt").html(data);

            },
            complete: function () {
                $('.ajax-loader').css("visibility", "hidden");

            },
            error: function () {
                $('#errorModal').modal('show');
            }
        });
    }
}
function recupeAgtSurcharge() {
    let dateFrom = $("#dateFromSurcharges").val();
    let dateTo = $("#dateToSurcharges").val();
    //let dropdownType = $("#dropdownType").val();
   
    let url = symphony + "/Lift/LoadAgtVAT";
    if ($("#testTransSurcharge").val() != dateFrom || $("#testTrans1Surcharge").val() != dateTo) {
        $("#testTransSurcharge").val(dateFrom);
        $("#testTrans1Surcharge").val(dateTo);
        $.ajax({
            type: "POST",
            url: url,
            data: { dateFrom: dateFrom, dateTo: dateTo },

            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {
                $("#cbAgtSurcharge").html(data);

            },
            complete: function () {
                $('.ajax-loader').css("visibility", "hidden");

            },
            error: function () {
                $('#errorModal').modal('show');
            }
        });
    }
}
function recupeFligthVAT1() {
    let dateFrom = $("#dateFromSurcharges").val();
    let dateTo = $("#dateToSurcharges").val();
    //let dropdownType = $("#dropdownType").val();

    let url = symphony + "/Lift/LoadcbFVAT";
    if ($("#testTrans2").val() != dateFrom || $("#testTrans3").val() != dateTo) {
        $("#testTrans2").val(dateFrom);
        $("#testTrans3").val(dateTo);
        $.ajax({
            type: "POST",
            url: url,
            data: { dateFrom: dateFrom, dateTo: dateTo },

            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {
                $("#cbFSurcharge").html(data);

            },
            complete: function () {
                $('.ajax-loader').css("visibility", "hidden");

            },
            error: function () {
                $('#errorModal').modal('show');
            }
        });
    }
}
function recupeFligthVAT() {
    let dateFrom = $("#dateFromVat").val();
    let dateTo = $("#dateToVat").val();
    //let dropdownType = $("#dropdownType").val();

    let url = symphony + "/Lift/LoadcbFVAT";
    if ($("#testTrans2").val() != dateFrom || $("#testTrans3").val() != dateTo) {
        $("#testTrans2").val(dateFrom);
        $("#testTrans3").val(dateTo);
        $.ajax({
            type: "POST",
            url: url,
            data: { dateFrom: dateFrom, dateTo: dateTo },

            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {
                $("#cbF").html(data);

            },
            complete: function () {
                $('.ajax-loader').css("visibility", "hidden");

            },
            error: function () {
                $('#errorModal').modal('show');
            }
        });
    }
}

function clearOAL() {
    let url = symphony + "/Lift/LiftForTransportOwn";
    $.ajax({
        type: "POST",
        url: url,
        success: function (msg) {
            $('.tab-cancellation').html(msg);
        }, error: function (msg) {
            $('#errorModal').modal('show');
        }
    });
}


/*******************end********************************/

/* lift fano */
function OBDetails() {

}


function getBillingPeriod() {

    //let tmp = $(".billingPeriod").val();
    let tmp = $("#airlineCode").val();
    if ($("#tempBilling").val() == "" && tmp == "All") {
        $("#tempBilling").val(tmp);
        tmp = "%";
        let url = symphony + "/Lift/getBillingPeriod";
        Ajaxs(tmp, '', 'billingPeriod', url);
        console.log(tmp)
    }
    if ($("#tempBilling").val() != tmp) {
        $("#tempBilling").val(tmp);
        let value = $("#airlineCode").val() == "All" ? "%" : $("#airlineCode").val();
        let url = symphony + "/Lift/getBillingPeriod";
        console.log(value)
        Ajaxs(value, '', 'billingPeriod', url);
    }


}

function searchOBView() {
    let air = $("#airlineCode").val() == "All" ? "%" : $("#airlineCode").val();
    let billing = $(".billingPeriod").val() == "All" ? "%" : $(".billingPeriod").val();
    let tab = [air, billing];
    let url = symphony + "/Lift/LoadPreviousBilling";
    Ajaxs(tab, '', 'loadOBView', url);
    var netAmount = 0.000;
    var totalTax = 0.000;
    var totalIsc = 0.000;
    var grossAmount = 0.000;
    $(".loadOBView tr").each(function () {

        netAmount = netAmount + parseFloat($(this).find('td:eq(10)').text());
        totalTax = totalTax + parseFloat($(this).find('td:eq(9)').text());
        totalIsc = totalIsc + parseFloat($(this).find('td:eq(7)').text());
        grossAmount = grossAmount + parseFloat($(this).find('td:eq(6)').text());
    });
    $("#resumePreviousNilling tr").each(function () {
        $(this).find('td:eq(3)').text(netAmount.toFixed(3));
        $(this).find('td:eq(2)').text(totalTax.toFixed(3));
        $(this).find('td:eq(1)').text(totalIsc.toFixed(3));
        $(this).find('td:eq(0)').text(grossAmount.toFixed(3));
    })
}

function DetailsPreviousBilling(alc, period) {

    let url = symphony + "/Lift/DetailsPreviousBilling";
    $("#OBDetails").modal('show');
    $("#DetailAirlineCode").text("  " + alc);
    $("#DetailBilling").text("  " + period);
    Ajaxs(period, alc, 'load-OBDetails', url);
    let i = 0;
    $(".load-OBDetails tr").each(function () {
        i++
    })
    $("#totalNb").text(i);
}

function goToPax(event,data, IdModal) {
    $('#' + IdModal).modal('hide');
    let value = data.split("-");
    let original = value[0];
    let transacode = value[1];
    ajout(this,'PAXTKTs', 'Sales', 'PAX TKTs');
    clickLigneTransaction(null,original, transacode);
    /*.modal-backdrop.in opacity = 0
    .modal-backdrop = position relative*/
}

function loadValidatePrime() {
    let validated = $("#checkValidated input:radio[name=Validated]:checked").val();
    let oalCarrier = $("#oalCarrier").val();
    let dateFrom = $("#dateFrom").val();
    let dateTo = $("#dateTo").val();
    tab = [dateFrom, dateTo, oalCarrier, validated];
    let url = symphony + "/Lift/GetValidatePrime";
    Ajaxs(tab, '', 'loadValidatePrime', url);
    $("#nbRow").val($("#nbLigne").val());
    if ($("#msgValidate").val() != "") {
        $('#alertModal').modal('show');
        $("#msgalert").html($("#msgValidate").val());
    }
    if ($("#ErrorMsgValidate").val() != "") {
        $('#errorModal').modal('show');
        $("#errorMessage").text($("#ErrorMsgValidate").val());
    }

}

function updateValidatePrime() {
    var tt = false;
    $(".loadValidatePrime tr").each(function () {
        if ($(this).find("td").eq(22).html() == false) {
            tt = true;
        }
    })
    if (tt) {

        $('#alertModal').modal('show');
        $("#msgalert").html("All coupons haven't been validated yet. Do you wish to continue?");
        $('div.modal-confirmation').html('<button type="button" class="btn btn-default" style="position: relative;top: 12px;" onclick="confirmeUpdateValidate()">OK</button>' +
                '<button type="button" class="Close btn btn-secondary btn-default" style="background-color:#e8ae72;" data-dismiss="modal">Close</button>');
    } else {
        confirmeUpdateValidate();
    }
}

function confirmeUpdateValidate() {
    let url = symphony + "/Lift/UpdateValidatePrime";
    let tab = [];
    let concat = "";
    var id = "";
    var tmp = "";
    $(".loadValidatePrime tr").each(function () {
        for (i = 0; i < 28; i++) {
            if (i == 13) {
                concat = concat + ',' + $(this).find("td").eq(i).html() + ',';
            }
            else if (i != 21) {
                concat = concat + $(this).find("td").eq(i).html() + ',';
            }

            if (i == 21) {
                id = $(this).find("td").eq(i).attr('id');
                id = id.replace("ici", "check");
                var val = $('#' + id).is(':checked');
                concat = concat + val + ',';
            }
        }
        tmp = $(this).find("td").eq(28).attr('id');
        tmp = id1 = tmp.replace("ici", "editUser")
        tab.push(concat + $('#' + tmp).val());
        concat = "";
    });
    Ajaxs(tab, '', 'success', url);
}

function setMenuValidatePrime(id, original) {

    $("#ExchangeTicket").removeClass();
    $("#ExchangeDetails").removeClass();
    $("#ExchangeTicket").addClass(original);
    $("#ExchangeDetails").addClass(original + '-' + 'news' + '-' + 'TKTT');
    let tops = $('#' + id).offset().top + 95;
    $("div.menu-dispo").css({ left: $('#' + id).offset().left + 'px', top: tops + 'px' });

    let url = symphony + "/Lift/ValidatePrimeDetail";
    $.ajax({
        type: 'POST',
        url: url,
        data: { dataValue: original },
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#issueDate").val(data[0]);
            $("#pnr").val(data[1]);
            $("#endosRestriction").val(data[2]);
        },
        error: function () {
            $('#alertModal').modal('show');
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
        },
    });
    $("#selectDispo").show();
}

function reprorate() {
    $("#proration").modal('show');
}

function OalCarrier() {
    if ($("#date1").val() != $("#dateFrom").val() && $("#date2").val() != $("#dateTo").val()) {
        $("#date1").val($("#dateFrom").val());
        $("#date2").val($("#dateTo").val());
        let url = symphony + "/Lift/OalCarrier";
        let tab = [$("#dateFrom").val(), $("#dateTo").val()]
        Ajaxs(tab, '', 'oalCarrier', url);
    }
}

function showOBExclude() {
    $("#showOBExclude").modal('show');
    let tab = [];
    var concat = "";
    var tmp = "";
    $(".loadValidatePrime tr").each(function () {
        for (i = 0; i < 28; i++) {
            if (i == 13) {
                concat = concat + ',' + $(this).find("td").eq(i).html() + ',';
            }
            else if (i != 21) {
                concat = concat + $(this).find("td").eq(i).html() + ',';
            }

            if (i == 21) {
                id = $(this).find("td").eq(i).attr('id');
                id = id.replace("ici", "check");
                var val = $('#' + id).is(':checked');
                concat = concat + val + ',';
            }
        }
        tmp = $(this).find("td").eq(28).attr('id');
        tmp = id1 = tmp.replace("ici", "editUser")
        tab.push(concat + $('#' + tmp).val());
        concat = "";
    });
    let url = symphony + "/Lift/ClickOBExclusion";
    Ajaxs(tab, '', 'oalCarrierExclude', url);

    let url1 = symphony + "/Lift/LoadOBExclusion";
    Ajaxs('%', '', 'obExclusionList', url1);
    if ($("#msgExclude").val() != "") {
        $('#errorModal').modal('show');
        $("#errorMessage").text($("#msgExclude").val());
    }

}

function OalCarrierExclude() {
    if ($("#oalCarrierExclude").val() != $(".oalCarrierExclude").val()) {
        $("#oalCarrierExclude").val($(".oalCarrierExclude").val());
        let tab = $(".oalCarrierExclude").val() == "All" ? "%" : $(".oalCarrierExclude").val();
        let url1 = symphony + "/Lift/LoadOBExclusion";
        Ajaxs(tab, '', 'obExclusionList', url1);
    }
}

function clickLigneOBExclusion(ID, sectorForm, sectorTo, startDate, endDate, validated) {
    $("#airlineId").val(ID);
    $("#sectorFrom").val(sectorForm);
    $("#sectorTo").val(sectorTo);
    $("#dateFrom1").val(startDate);
    $("#dateTo1").val(endDate);
    validated == "1" ? $("#validated").prop("checked", true) : $("#validated").prop("checked", false);
}

function savaOBExclude() {
    let tmp = true;
    if ($("#airlineId").val() == "") {
        $('#errorModal').modal('show');
        $("#errorMessage").text("Airline ID is compulsory");
        tmp = false;
    }
    if ($("#sectorFrom").val() == "") {
        $('#errorModal').modal('show');
        $("#errorMessage").text("Sector From is compulsory");
        tmp = false;
    }
    if ($("#sectorTo").val() == "") {
        $('#errorModal').modal('show');
        $("#errorMessage").text("Sector To is compulsory");
        tmp = false;
    }
    if ($("#airlineId").val().length < 3) {
        $('#errorModal').modal('show');
        $("#errorMessage").text("Sector From length can not be less than 3 characters");
        tmp = false;
    }
    let url = symphony + "/Lift/checkifexist";
    let validate = $("#getCkeckedValidated input[name=validate]:checkbox:checked").val() == undefined ? 0 : 1;

    let tab = [$("#airlineId").val(), $("#sectorFrom").val(), $("#sectorTo").val(), $("#dateFrom1").val(), $("#dateTo1").val(), validate];
    if (tmp) {
        Ajaxs(tab, '', 'success', url);
    }

    showOBExclude();
}

function addNewOBExclude() {
    clearBillingExclusion()
    editBillingExclusion('airlineId', 'sectorFrom', 'sectorTo', 'validated');
}

function editBillingExclusion(airlineId, sectorFrom, sectorTo, validated) {
    document.getElementById(airlineId).removeAttribute('readonly');
    document.getElementById(sectorFrom).removeAttribute('readonly');
    document.getElementById(sectorTo).removeAttribute('readonly');
    document.getElementById(validated).removeAttribute('readonly');
}
function clearBillingExclusion() {
    $("#airlineId").val("");
    $("#sectorFrom").val("");
    $("#sectorTo").val("");
    $("#dateFrom1").val("");
    $("#dateTo1").val("");
    $("#validated").prop("checked", false);
    $(".obExclusionList").html('');

}


function showTaxInfo() {

    $('#alertModal').modal('show');
    $("#msgalert").html("Please Note that only Validated Documents will be checked. Do you wish to continue?");
    $('div.modal-confirmation').html('<button type="button" class="btn btn-default" style="position: relative;top: 12px;" onclick="confirmTaxInfo()">Yes</button>' +
            '<button type="button" class="Close btn btn-secondary btn-default" style="background-color:#e8ae72;" data-dismiss="modal">Close</button>');
}

function confirmTaxInfo() {
    $('#alertModal').modal('hide');
    $("#showTaxInfo").modal('show');
    let url = symphony + "/Lift/TaxCarrier";
    Ajaxs('', '', 'taxCarrier', url);
}

function taxSectors() {
    let url = symphony + "/Lift/TaxSector";
    let tab = "%";
    if ($("#tmptaxSectors").val() != $(".taxCarrier").val() && $(".taxCarrier").val() != "All") {
        $("#tmptaxSectors").val($(".taxCarrier").val());
        Ajaxs(tab, '', 'taxSector', url);
    }
}

function LoadTaxSector() {
    if ($("#tmptaxSector").val() != $(".taxSector").val()) {
        $("#tmptaxSector").val($(".taxSector").val());
        let tab = $(".taxSector").val();
        let tab1 = $(".taxCarrier").val() == "All" ? "%" : $(".taxCarrier").val();
        let url = symphony + "/Lift/LoadTaxVerification";
        Ajaxs(tab, tab1, 'body-taxVerification', url);
    }
}

function BtnTransitCheckClick() {
    let tab = $(".taxCarrier").val();
    let url = symphony + "/Lift/BtnTransitCheckClick";
    Ajaxs(tab, '', 'success', url);
}

function userMethod(text) {
    var id = text.getAttribute('id');

    if (text.value.length >= 2 && text.value.toUpperCase() == "CS") {
        $("#" + id).val(text.value.toUpperCase())
    } else if (text.value.length == 3) {
        if (text.value.toUpperCase() == "MPA" || text.value.toUpperCase() == "SPA") {
            $("#" + id).val(text.value.toUpperCase())
        } else {
            $("#" + id).val("");
            $('#alertModal').modal('show');
            $("#msgalert").html("Invalid Entries. Only SPA,MPA and CS are valid Entries");

        }
    }
}

function taxEditing() {

    $("#viewRecord").modal('hide');
    $("#showTaxInfo").modal('hide');
    $("#taxEditing").modal('show');
    let url = symphony + "/Lift/TaxEditingAirport";
    Ajaxs('', '', 'taxEditing', url);
}

function getDetailTaxEditing() {
    let tab = $(".taxEditing").val();
    if (tab != "") {
        let url = symphony + "/Lift/DetailTaxEditingAirport";
        Ajaxs(tab, '', 'body-taxEditing', url);
    }
}

function clickLigneTaxInfo(zero, un, deux, trois, quatre, cinq, six, sept, huit, neuf, dix, onze, douze) {
    $("#zero").val(zero);
    $("#un").val(un);
    $("#deux").val(deux);
    $("#trois").val(trois);
    $("#quatre").val(quatre);
    $("#cinq").val(cinq);
    $("#DateFrom2").val(six);
    $("#DateTo2").val(sept);
    $("#huit").val(huit);
    $("#neuf").val(neuf);
    $("#dix").val(dix);
    $("#onze").val(onze);
    $("#taxRefId").val(douze);
}
function editTaxEditing() {
    document.getElementById('zero').removeAttribute('readonly');
    document.getElementById('un').removeAttribute('readonly');
    document.getElementById('deux').removeAttribute('readonly');
    document.getElementById('trois').removeAttribute('readonly');
    document.getElementById('quatre').removeAttribute('readonly');
    document.getElementById('cinq').removeAttribute('readonly');
    document.getElementById('huit').removeAttribute('readonly');
    document.getElementById('neuf').removeAttribute('readonly');
    document.getElementById('dix').removeAttribute('readonly');
    document.getElementById('onze').removeAttribute('readonly');
}
function addTaxEditing() {
    let url = symphony + "/Lift/AddDetailTaxEditing";
    let checkDateFrom = $("#checkDateFrom input[name=dtpDateFrom]:checkbox:checked").val() == undefined ? "No" : "On";
    let checkDateTo = $("#checkDateTo input[name=dtpDateTo]:checkbox:checked").val() == undefined ? "No" : "On";
    if ($("#huit").val() == "") {
        $('#alertModal').modal('show');
        $("#msgalert").html("Tax Code Can Not Null");
    } else {
        let tab = [$("#zero").val(), $("#un").val(), $("#deux").val(), $("#trois").val(), $("#quatre").val(), $("#cinq").val(), $("#DateFrom2").val(), $("#DateTo2").val(), $("#huit").val(), $("#neuf").val(), $("#dix").val(), $("#onze").val(), checkDateFrom, checkDateTo];
        Ajaxs(tab, '', 'success', url);
    }


}
function updateTaxEditing() {
    let url = symphony + "/Lift/UpdateRefTax";
    let checkDateFrom = $("#checkDateFrom input[name=dtpDateFrom]:checkbox:checked").val() == undefined ? "No" : "On";
    let checkDateTo = $("#checkDateTo input[name=dtpDateTo]:checkbox:checked").val() == undefined ? "No" : "On";

    let tab = [$("#zero").val(), $("#un").val(), $("#deux").val(), $("#trois").val(), $("#quatre").val(), $("#cinq").val(), $("#DateFrom2").val(), $("#DateTo2").val(), $("#huit").val(), $("#neuf").val(), $("#dix").val(), $("#onze").val(), checkDateFrom, checkDateTo, $("#taxRefId").val()];
    Ajaxs(tab, '', 'success', url);
    $('#getDetailTaxEditing').trigger('click');

}
function deleteTaxEditing() {
    $('#errorModal').modal('show');
    $('div.confirm-delete').html('<button type="button" class="btn btn-default" style="position: relative;top: 12px;" onclick="confimDeleteTaxEditing()">OK</button>' +
            '<button type="button" class="Close btn btn-secondary btn-default" style="background-color:#e8ae72;" data-dismiss="modal">Close</button>');
    $("#errorMessage").text("Are you sure to delete this tax entry?", "Taxes Reference");

}
function confimDeleteTaxEditing() {
    let url = symphony + "/Lift/DeleteDetailTaxEditing";
    let tab = $("#taxRefId").val();
    if (tab != "") {
        $("#taxRefId").val("");
        Ajaxs(tab, '', 'success', url);
        $('#getDetailTaxEditing').trigger('click');
        clearTaxEditing();
    }

}
function clearTaxEditingAll() {
    clearTaxEditing();
    $(".body-taxEditing").html('<tr></tr>')
}
function clearTaxEditing() {
    $("#zero").val("");
    $("#un").val("");
    $("#deux").val("");
    $("#trois").val("");
    $("#quatre").val("");
    $("#cinq").val("");
    $("#DateFrom2").val("");
    $("#DateTo2").val("");
    $("#huit").val("");
    $("#neuf").val("");
    $("#dix").val("");
    $("#onze").val("");
    $("#taxRefId").val("");
}

function couponBilling() {
    let url = symphony + "/Lift/BtnGeneratePrimeCoupon";
    let tab = [];
    $("#loadValidatePrime tr").each(function () {
        /*for (i = 0; i < 29; i++) {
            concat = concat + $(this).find("td").eq(i).html() + ',';
        }*/
        tab.push($(this).find("td").eq(1).html());
    });
    if (tab.length == 0) {
        tab = [""]
    }
    console.log(tab);
    $.ajax({
        type: 'POST',
        url: url,
        data: { dataValue: tab },
        success: function (data) {
            console.log(data);
            if (data.includes("Airline Info missing in Ref.Airline.Please Input Airline Info for")) {
                $('#alertModal').modal('show');
                $("#msgalert").html(data);
                $('div.modal-confirmation').html('<button type="button" class="btn btn-default" style="position: relative;top: 12px;" onclick="yesCouponBilling()">Yes</button>' +
                        '<button type="button" class="Close btn btn-secondary btn-default" style="background-color:#e8ae72;" data-dismiss="modal">Close</button>');
            }
            if (data.includes("Folder C:\SISinv does not exist on your drive")) {
                $('#alertModal').modal('show');
                $("#msgalert").html(data);
                $('div.modal-confirmation').html('<button type="button" class="btn btn-default" style="position: relative;top: 12px;" onclick="confimCouponBilling()">Yes</button>' +
                        '<button type="button" class="Close btn btn-secondary btn-default" style="background-color:#e8ae72;" data-dismiss="modal">Close</button>');
            }
            if (data == "") {
                $('#alertModal').modal('show');
                $("#msgalert").html("Do you want to generate invoices as per the date filter?");
                $('div.modal-confirmation').html('<button type="button" class="btn btn-default" style="position: relative;top: 12px;" onclick="yesGenerateCouponBilling()">Yes</button>' +
                                    '<button type="button" class="Close btn btn-secondary btn-default" style="background-color:#e8ae72;" data-dismiss="modal">Close</button>');
            }
        },
        error: function () {
            $('#alertModal').modal('show');
        },
    });
}

function confimCouponBilling() {

    $('#alertModal').modal('hide');
    $('#alertModal').modal('show');
    $("#msgalert").html("Do you want to generate invoices as per the date filter?");
    $('div.modal-confirmation').html('<button type="button" class="btn btn-default" style="position: relative;top: 12px;" onclick="yesConfimCouponBilling("yes")">Yes</button>' +
                        '<button type="button" class="Close btn btn-secondary btn-default" style="background-color:#e8ae72;" onclick="nonGenerateCouponBilling()">No</button>');

}

function yesConfimCouponBilling() {
    $('#alertModal').modal('hide');
    let url = symphony + "/Lift/yesConfimCouponBilling";
    let tab = "";
    $.ajax({
        type: 'POST',
        url: url,
        data: { dataValue: tab },
        success: function (data) {
            if (data == "Do you want to generate invoices as per the date filter?") {
                $('#alertModal').modal('show');
                $("#msgalert").html(data);
                $('div.modal-confirmation').html('<button type="button" class="btn btn-default" style="position: relative;top: 12px;" onclick="yesGenerateCouponBilling()">Yes</button>' +
                        '<button type="button" class="Close btn btn-secondary btn-default" style="background-color:#e8ae72;" onclick="nonGenerateCouponBilling()">No</button>');
            }
        },
        error: function () {
            $('#alertModal').modal('show');
        },
    });


}

function yesGenerateCouponBilling() {
    let url = symphony + "/Lift/yesGenerateCouponBilling";
    let tab = [$("#dateFrom").val(), $("#dateTo").val(), $("#oalCarrier").val()];
    Ajaxs(tab, '', 'head-sisEngine', url);
    $('#alertModal').modal('hide');
    $("#couponBilling").modal('show');
}

function nonGenerateCouponBilling() {
    let url = symphony + "/Lift/yesGenerateCouponBilling";
    let tab = ["01-Jan-2013", $("#dateTo").val(), $("#oalCarrier").val()];
    Ajaxs(tab, '', 'head-sisEngine', url);
    $('#alertModal').modal('hide');
    $("#couponBilling").modal('show');
}



function btnGenerateInvoices() {
    let carr = $("#oalCarrier").val();
    carr == "All" ? "%" : carr;
    let tab = [$("#YY").val(), $("#MM").val(), $("#PP").val(), $("#dateFrom").val(), $("#dateTo").val(), carr];
    let url = symphony + "/Lift/btnGenerateInvoices";
    Ajaxs(tab, '', 'body-taxVerification', url);
}

function btnDeleteSisBilling(alc, period) {

    let tab = [alc, period];
    let url = symphony + "/Lift/DeleteSisBilling";
    Ajaxs(tab, '', 'success', url);
    btnGenerateInvoices()
}

function deleteSisBilling() {

}


function btnProrateOb() {
    let alc = $("#oalCarrier").val() == "All" ? "%" : $("#oalCarrier").val();
    let tab = [$("#YY").val(), $("#MM").val(), $("#dateFrom").val(), $("#dateTo").val(), alc];
    let url = symphony + "/Lift/btnProrateClick";
    Ajaxs(tab, '', 'success', url);
}

function CheckBillingPeriod() {
    let tab = $("#YY").val() + $("#MM").val() + $("#PP").val();
    if (tab.length == 6) {
        let url = symphony + "/Lift/CheckBillingPeriod";
        $.ajax({
            type: 'POST',
            url: url,
            data: { dataValue: tab },
            success: function (data) {
                if (data == "False") {
                    $("#couponBilling").modal('hide');
                    $('#alertModal').modal('show');
                    $("#msgalert").html("Please check the Billing Period. Do you wish to continue with selected billing period?");
                    $('div.modal-confirmation').html('<button type="button" class="btn btn-default" style="position: relative;top: 12px;" onclick="yesCheckBillingPeriod()">Yes</button>' +
                            '<button type="button" class="Close btn btn-secondary btn-default" style="background-color:#e8ae72;" data-dismiss="modal">Close</button>');
                }
            },
            error: function () {
                $('#alertModal').modal('show');
            },
        });

    }
}

function yesCheckBillingPeriod() {
    document.getElementById("prorateOb").style.borderColor = "#0d84ca";
    $('#alertModal').modal('hide');
    $("#couponBilling").modal('show');
}

function btnViewRecords() {
    let tab = $("#YY").val() + $("#MM").val() + $("#PP").val();
    let url = symphony + "/Lift/OBViewer";
    $("#couponBilling").modal('hide');
    $("#viewRecord").modal('show');
    Ajaxs(tab, '', 'body-viewRecord', url);
}

function getObViewDetail(zero, un, deux, trois, quatre, seze) {
    $("#alc").text(un);
    $("#chk").text(quatre);
    $("#doc").text(deux);
    $("#BillingPeriod").text(zero);
    $("#cpn").text(trois);
    $("#taxCode").val(seze.substring(0, 2)),
    $("#taxAmount").val(seze.substring(2, seze.length));
    $(".body-obViewr").html('<tr><td>' + seze.substring(0, 2) + '</td><td>' + seze.substring(2, seze.length) + '</td><tr>')
}

function saveOBViewer() {
    $("#couponBilling").modal('hide');
    $('#alertModal').modal('show');
    $("#msgalert").html("Are you sure to update this entry?");
    $('div.modal-confirmation').html('<button type="button" class="btn btn-default" style="position: relative;top: 12px;" onclick="yessaveOBViewer()">Yes</button>' +
            '<button type="button" class="Close btn btn-secondary btn-default" style="background-color:#e8ae72;" data-dismiss="modal">Close</button>');
}

function yessaveOBViewer() {
    $('#alertModal').modal('hide');
    $(".body-obViewr").html('<tr><td>' + $("#taxCode").val() + '</td><td>' + $("#taxAmount").val() + '</td><tr>')
    let tab = [$("#BillingPeriod").text(), $("#alc").text(), $("#doc").text(), $("#cpn").text(), $("#chk").text(), $("#taxCode").val(), $("#taxAmount").val()];
    let url = symphony + "/Lift/UpdateOBViewer";
    Ajaxs(tab, '', 'success', url);
    btnViewRecords();
}

function deleteOBViewer() {
    $("#couponBilling").modal('hide');
    $('#alertModal').modal('show');
    $("#msgalert").html("Are you sure you want to delete the Tax Code: " + $("#taxCode").val() + " ?");
    $('div.modal-confirmation').html('<button type="button" class="btn btn-default" style="position: relative;top: 12px;" onclick="yesdeleteOBViewer()">Yes</button>' +
            '<button type="button" class="Close btn btn-secondary btn-default" style="background-color:#e8ae72;" data-dismiss="modal">Close</button>');

}

function yesdeleteOBViewer() {
    $('#alertModal').modal('hide');
    $(".body-obViewr").html('<tr><td></td><td></td><tr>')
    let tab = [$("#doc").text(), $("#cpn").text(), $("#taxCode").val()];
    $("#taxCode").val('');
    $("#taxAmount").val('');
    let url = symphony + "/Lift/DeleteOBViewer";
    Ajaxs(tab, '', 'success', url);
    btnViewRecords();
}

function reGenerate() {
    $("#reGenerate").modal('show');
    let url = symphony + "/Lift/ReGenerate";
    Ajaxs('', '', 'regenerateBillPeriod', url);
}

function getRegenerateAirline() {
    let url = symphony + "/Lift/ReGenerate";
    if ($("#tmpregeneratePeriod").val() != $(".regenerateBillPeriod").val()) {
        $("#tmpregeneratePeriod").val($(".regenerateBillPeriod").val());
        let tab = $("#tmpregeneratePeriod").val();
        Ajaxs(tab, "", 'regenerateAirlineCode', url);
    } else {
        if ($("#tmpregenerate").val() != $(".regenerateAirlineCode").val()) {
            $("#tmpregenerate").val($(".regenerateAirlineCode").val())
            let tab1 = $("#tmpregenerate").val();
            let tab = $("#tmpregeneratePeriod").val();
            if ($(".regenerateAirlineCode").val() == "All") {
                $(".aboutAirline").text("All Airlines Selected");
            } else {
                Ajaxs(tab, tab1, 'aboutAirline', url);
            }
        }

    }
}

function procedReprorate() {
    $("#reGenerate").modal('hide');
    $('#errorModal').modal('show');
    if ($(".regenerateAirlineCode").val() != "All")
        $("#errorMessage").text("Are you sure to Delete the Outward Billing Entries For Airline " + $(".regenerateAirlineCode").val() + "  and for the Billing Period of " + $(".regenerateBillPeriod").val());
    else
        $("#errorMessage").text("Are you sure to Delete the Outward Billing Entries For All Airlines and for the Billing Period of " + $(".regenerateBillPeriod").val());
    $('div.confirm-delete').html('<button type="button" class="btn btn-default" style="position: relative;top: 12px;" onclick="confimDeleteReGenerate()">Yes</button>' +
            '<button type="button" class="Close btn btn-secondary btn-default" style="background-color:#e8ae72;" data-dismiss="modal">Non</button>');
}

function confimDeleteReGenerate() {
    $('#errorModal').modal('hide');
    let alc = $(".regenerateAirlineCode").val() == "All" ? "%" : $(".regenerateAirlineCode").val();
    let tab = [$(".regenerateBillPeriod").val(), alc];
    let url = symphony + "/Lift/DeleteOutWardBilling";
    Ajaxs(tab, '', 'success', url);
    $("#reGenerate").modal('show');
}

/**********************No show*******************/
function SearchNoShow() {

    /*/if ($("#datedtpIssueDateFrom").val() == "" || $("#datedtpIssueDateTo").val() == "" || $("#datedtpIssueDateFrom1").val() == "" || $("#datedtpIssueDateTo1").val() == "") {
        $('#alertModal').modal('show');
        $("#msgalert").html("Please Enter Date");
    }*/
    //else {
    let datedtpIssueDateFrom = $("#datedtpIssueDateFromNoshow").val();
    let datedtpIssueDateTo = $("#datedtpIssueDateToNoshow").val();

    let AgentName = $("#AgentName").val();
    let PassengerName = $("#PassengerName").val();
    let DocumentNumber = $("#DocumentNumber").val();

    let UsageDateFrom = $("#datedtpIssueDateFrom1Noshow").val();
    let UsageDateTo = $("#datedtpIssueDateTo1Nowshow").val();

    let Remarks = $("#Remarks").val();

    let chekeddatedtpIssueDateFrom = $("#sqlCriteria input[name=dtpIssueDateFromNoshow]:checkbox:checked").val();
    let checkeddatedtpIssueDateTo = $("#sqlCriteria1 input[name=dtpIssueDateFrom1Noshow]:checkbox:checked").val();
    tab = [datedtpIssueDateFrom, datedtpIssueDateTo, AgentName, DocumentNumber, UsageDateFrom, UsageDateTo, Remarks, chekeddatedtpIssueDateFrom, checkeddatedtpIssueDateTo, PassengerName];
    let url = symphony + "/Lift/SearchNoShow";
    $.ajax({
        type: 'POST',
        url: url,
        data: { dataValue: tab },
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            console.log(data);
            $(".showresult").html(data);
            if ($("#TotalNumberOfDocuments").val() == 0) {
                $('#alertModal').modal('show');
                $('#msgalert').text('No data available for the selected criteria');
            }
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
        },
        error: function () {
            $('#alertModal').modal('show');
            $("#msgalert").html("Error");
        }
    });
}
//}

function gotoPax(numcode, transaction) {
    ajout(this, 'PAXTKTs', 'Sales', 'PAX TKTs');
    clickLigneTransaction(numcode, transaction);
}

function Remarksselect() {
    if ($("#datedtpIssueDateFromNoshow").val() == "" && $("#datedtpIssueDateToNoshow").val() == "" && $("#datedtpIssueDateFrom1Noshow").val() == "" && $("#datedtpIssueDateTo1Noshow").val() == "") {
        $('#alertModal').modal('show');
        $("#msgalert").html("Please Enter Date");
    }
    let datedtpIssueDateFrom = $("#datedtpIssueDateFromNoshow").val();
    let datedtpIssueDateTo = $("#datedtpIssueDateToNoshow").val();
    let UsageDateFrom = $("#datedtpIssueDateFrom1Noshow").val();
    let UsageDateTo = $("#datedtpIssueDateTo1Nowshow").val();
    let AgentName = $("#AgentName").val();
    let chekeddatedtpIssueDateFrom = $("#sqlCriteria input[name=dtpIssueDateFromNoshow]:checkbox:checked").val();
    let checkeddatedtpIssueDateTo = $("#sqlCriteria1 input[name=dtpIssueDateFrom1Noshow]:checkbox:checked").val();
    tab = [datedtpIssueDateFrom, datedtpIssueDateTo, UsageDateFrom, UsageDateTo, chekeddatedtpIssueDateFrom, checkeddatedtpIssueDateTo, AgentName];
    let url = symphony + "/Lift/Remarksselect";
    if ($("#test").val() != datedtpIssueDateFrom && $("#stock").val() != datedtpIssueDateTo) {
        $("#test").val(datedtpIssueDateFrom);
        $("#stock").val(datedtpIssueDateTo);
        $.ajax({
            type: 'POST',
            url: url,
            data: { dataValue: tab },
            success: function (msg) {
                $("#selectshow").html(msg);
            },
            complete: function () {
                $('.ajax-loader').css("visibility", "hidden");
                /*var mes = $('#message').val();
                if (mes !== "") {
                    $('#alertModal').modal('show');
                    $("#msgalert").html(mes);
                }*/
            },
            error: function () {
                $('#errorModal').modal('show');
            }
        });

    }

}
function clearNoShow() {
    let url = symphony + "/Lift/NoShow";
    $.ajax({
        type: "POST",
        url: url,
        success: function (msg) {
            $('.tab-cancellation').html(msg);
        }, error: function (msg) {
            $('#errorModal').modal('show');
        }
    });
}
////////////////////////////////SLS////////////////////////////////////////

function Fightselect() {
    if ($("#datedtpIssueDateFromSls").val() == "" && $("#datedtpIssueDateToSls").val() == "") {
        $('#alertModal').modal('show');
        $("#msgalert").html("Please Enter Date");
    }

    let datedtpIssueDateFrom = $("#datedtpIssueDateFromSls").val();
    let datedtpIssueDateTo = $("#datedtpIssueDateToSls").val();
    let Fight = $("#Fight").val();
    tab = [datedtpIssueDateFrom, datedtpIssueDateTo, Fight];
    let url = symphony + "/Lift/Fightselect";

    if ($("#test").val() != datedtpIssueDateFrom || $("#stock").val() != datedtpIssueDateTo) {
        $("#test").val(datedtpIssueDateFrom);
        $("#stock").val(datedtpIssueDateTo);
        $.ajax({
            type: 'POST',
            url: url,
            data: { dataValue: tab },
            success: function (msg) {
                $("#selectSLS").html(msg);
            },
            complete: function () {
                $('.ajax-loader').css("visibility", "hidden");
                var mes = $('#message').val();
                if (mes !== "") {
                    $('#alertModal').modal('show');
                    $("#msgalert").html(mes);
                }
            },
            error: function () {
                $('#errorModal').modal('show');
            }
        });
    }
}

function FightshowTable() {
    // Fightselect();
    let datedtpIssueDateFrom = $("#datedtpIssueDateFromSls").val();
    let datedtpIssueDateTo = $("#datedtpIssueDateToSls").val();
    let Fight = $("#Fight").val();
    tab = [datedtpIssueDateFrom, datedtpIssueDateTo, Fight];
    let url = symphony + "/Lift/SLSShowTable";
    $.ajax({
        type: 'POST',
        url: url,
        data: { dataValue: tab },

        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $(".showresultSLS").html(data);
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
        },
        error: function () {
            $('#alertModal').modal('show');
            $("#msgalert").html("");
        }

    });
}

///////////////////////////////LDM/////////////////////////////////////

function FightselectLDM() {
    if ($("#datedtpIssueDateFromLdm").val() == "" && $("#datedtpIssueDateToLdm").val() == "") {
        $('#alertModal').modal('show');
        $("#msgalert").html("Please Enter Date");
    }

    let datedtpIssueDateFrom = $("#datedtpIssueDateFromLdm").val();
    let datedtpIssueDateTo = $("#datedtpIssueDateToLdm").val();
    let Fight = $("#Fight").val();
    tab = [datedtpIssueDateFrom, datedtpIssueDateTo, Fight];
    let url = symphony + "/Lift/FightselectLDM";

    if ($("#test").val() != datedtpIssueDateFrom || $("#stock").val() != datedtpIssueDateTo) {
        $("#test").val(datedtpIssueDateFrom);
        $("#stock").val(datedtpIssueDateTo);
        $.ajax({
            type: 'POST',
            url: url,
            data: { dataValue: tab },
            success: function (msg) {
                $("#selectLDM").html(msg);
            },
            complete: function () {
                $('.ajax-loader').css("visibility", "hidden");
                var mes = $('#message').val();
                if (mes !== "") {
                    $('#alertModal').modal('show');
                    $("#msgalert").html(mes);
                }
            },
            error: function () {
                $('#errorModal').modal('show');
            }
        });
    }
}

function FightshowTableLDM() {
    FightselectLDM();
    let datedtpIssueDateFrom = $("#datedtpIssueDateFromLdm").val();
    let datedtpIssueDateTo = $("#datedtpIssueDateToLdm").val();
    let Fight = $("#Fight").val();
    tab = [datedtpIssueDateFrom, datedtpIssueDateTo, Fight];
    let url = symphony + "/Lift/SLSShowTableLDM";
    $.ajax({
        type: 'POST',
        url: url,
        data: { dataValue: tab },
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $(".showresultLDM").html(data);
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
        },
        error: function () {
            $('#alertModal').modal('show');
            $("#msgalert").html("error");
        }
    });
}

function activetabIntBill(id, radioId) {
    if (radioId == null) {
        $('#tabIntBilling li a').each(function (idx, li) {
            if ($(this).attr('href') == "#" + id) {
                $(this).trigger('click');
            }
        });
    } else {
        $("#" + radioId).prop("checked", true);
    }

}

/**************end********/
/********************JS Tolotra******************************/
function HideShow(iddiv, idbtn) {
    var x = document.getElementById(iddiv);
    var y = document.getElementById(idbtn);
    var out = "Hide Add-on Details";
    if (x.style.display === "block") {
        x.style.display = "none";
        y.innerHTML = "Show Add-on Details";
    } else {
        x.style.display = "block";
        y.innerHTML = out;
    }
}
/********************JS end******************************/


function HideShowLift(iddiv, idbtn) {
    var x = document.getElementById(iddiv);
    var y = document.getElementById(idbtn);
    var out = "Hide Add-on Details";
    if (x.style.display === "block") {
        x.style.display = "none";
        y.innerHTML = "Show Add-on Details";
    } else {
        x.style.display = "block";
        y.innerHTML = out;
    }
}

/*Lift Tab */

function showreportsLift(id, classe) {
    let idCkeck = "";
    if ($("#" + id).is(':checked')) {

        if (id == "allsumm" || id == "allfaire" || id == "allkey" || id == "allkeyFR2" || id == "allpasse" || id == "allkeySpec" || id == "allkeyDisc") {
            $('ul.list-group li').each(function (idx, li) {
                idCkeck = $(li).find('input:checkbox').attr('id');
                $("#" + idCkeck).prop("checked", true);

            });
            for (i = 0; i < 68; i++) {
                $('.' + classe + '-' + i).show();
            }
        } else {
            $('.' + classe).show();
        }

    } else {
        if (id == "allsumm" || id == "allfaire" || id == "allkey" || id == "allkeyFR2" || id == "allpasse" || id == "allkeySpec" || id == "allkeyDisc") {
            $('ul.list-group li').each(function (idx, li) {
                idCkeck = $(li).find('input:checkbox').attr('id');
                $("#" + idCkeck).prop("checked", false);

            });
            for (i = 0; i < 68; i++) {
                $('.' + classe + '-' + i).hide();
            }
        } else {
            $('.' + classe).hide();
        }

    }
}
