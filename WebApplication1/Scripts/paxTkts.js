/// <reference path="paxTkts.js" />
//var symphony = "/symphony";
var symphony = "";
$("#firstDocNumber :input").prop("disabled", true);

//// modal dans sales
$('.ticket-payment').on('click', '#modal-ancillary', function () {
    let url = symphony + "/Sales/PayementTFCs";
    $.get(url, function (data) {
        $('.modal-body').html(data);
    });
})

/*****fait par christian********************/
function getlist() {
    let url = symphony + "/Process/TFC";
    $.ajax({
        type: "POST",
        url: url,
        success: function (data) {
            $("#FlightTrans").html(data);
            console.log(data);

        }
    });
}
function valoption() {

    let FlightTrans = $("#FlightTrans").val();

    let url = symphony + "/Process/TFC2";
    $.ajax({
        type: 'POST',
        url: url,
        data: { FlightTrans: FlightTrans },

        success: function (data) {
            $("#taxx").html(data);
            $("#newwtaxx").html(data);
        },
    });


}
function recherche() {

    let FlightTrans = $("#FlightTrans").val();

    let FlightTrans1 = $("#FlightTrans1").val();

    let url = symphony + "/Process/recherche";

    $.ajax({
        async: true,
        type: "POST",
        url: url,
        data: { FlightTrans: FlightTrans, FlightTrans1: FlightTrans1 },

        success: function (data) {
            $("#coGENERALTFCINFO").html($(data).filter('#newcoGENERALTFCINFO')); 
            $("#coAPPLICABLETFC").html($(data).filter('#newcoAPPLICABLETFC'));
            $("#coINTERLININGTFC").html($(data).filter('#newcoINTERLININGTFC'));
        },
        complete: function () {
        },
    });
}
function gettable1(effective, expiry, percentage, amount, currency, detail, sale, travel) {
    $("#effectivedate").val(effective);
    $("#expirydate").val(expiry);
    $("#percentage").val(percentage);
    $("#amount").val(amount);
    $("#currency").val(currency);
    $("#detail").text(detail);
    $("#saledate").val(sale);
    $("#traveldate").val(travel);
}
function gettable2(exemptionscode, exemptiondetails, dom) {
    $("#exemptionscode").val(exemptionscode);
    $("#exemptiondetails").text(exemptiondetails);
    $("#dom").val(dom);
}
function gettable3(from, to, prime, passenger, domestic, duration, valid, validto, taxcode, taxcurrency, taxamount, percentage, reftaxid) {
    $('#reftaxid').val(reftaxid);
    $("#from").val(from);
    $("#to").val(to);
    $("#prime").val(prime);
    $("#passenger").val(passenger);
    $("#domestic").val(domestic);
    $("#validfrom").val(valid);
    $("#validto").val(validto);
    $("#taxcode1").val(taxcode);
    $("#taxcurrency").val(taxcurrency);
    $("#duration").val(duration);
    $("#taxamount").val(taxamount);
    $("#percentage").val(percentage);

}
function recherchetfc() {
    let dateFrom = $("#dateFromTFC").val();
    let dateTo = $("#dateToTFC").val();
    let DocNo = $("#DocNo").val();
    let AgentNumericCode = $("#AgentNumericCode").val();
    let url = symphony + "/Process/recherchetfc";
    $.ajax({
        type: 'POST',
        url: url,
        data: { dateFrom: dateFrom, dateTo: dateTo, DocNo: DocNo, AgentNumericCode: AgentNumericCode },
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#tableautfcgenerale").html($(data).filter('#newtableautfcgenerale'));
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
            tableExchangedDocs();
            getcsvtfcgeneral();
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}
function missingtaxes() {
    let url = symphony + "/Process/viewmissingsector";
    $.ajax({
        async: true,
        type: "POST",
        url: url,
        data: {},
        success: function (data) {
            $("#tb1").html($(data).filter('#newtb1'));
            getcsvmissingtaxes();
        }
    });
}
function getcsvmissingtaxes() {
    $(document).ready(function () {
        $('#newtb1').DataTable({
            "pageLength": 100,
            dom: 'Bfrtip',
            buttons: [
				 'csv'
            ]
        });
    });
}
function sectornotaxes() {
    let url = symphony + "/Process/sectornotaxes";
    $.ajax({
        async: true,
        type: "POST",
        url: url,
        data: {},
        success: function (data) {
            $("#tb2").html($(data).filter('#newtb2'));
            getcsvsectornotaxes();
        }
    });
}
function getcsvsectornotaxes() {
    $(document).ready(function () {
        $('#newtb2').DataTable({
            "pageLength": 100,
            dom: 'Bfrtip',
            buttons: [
				 'csv'
            ]
        });
    });
}
function addtfc() {
    if ($('#temps').val() == 1) {

        $('#flag1').val("true");
        $('#secondflag').val("true");
    }
    let url = symphony + "/Process/addtfc";
    $.ajax({
        async: true,
        type: "POST",
        url: url,
        data: {},

        success: function (data) {
            $("#divFlightTrans").html($(data).filter('#adddivFlightTrans'));
            $("#taxx").html($(data).filter('#addtaxx'));
            $("#coGENERALTFCINFO").html($(data).filter('#addtfccoGENERALTFCINFO'));
            $("#coAPPLICABLETFC").html($(data).filter('#addtfccoAPPLICABLETFC'));
            $("#coINTERLININGTFC").html($(data).filter('#addtfccoINTERLININGTFC'));
        }
    });
}
function clear1() {
    let url = symphony + "/Process/clear1";
    $.ajax({
        async: true,
        type: "POST",
        url: url,
        success: function (data) {
            $("#divFlightTrans").html($(data).filter('#cleardivFlightTrans'));
            $("#taxx").html($(data).filter('#cleartaxx'));
            $("#coGENERALTFCINFO").html($(data).filter('#clearcoGENERALTFCINFO'));
            $("#coAPPLICABLETFC").html($(data).filter('#clearcoAPPLICABLETFC'));
            $("#coINTERLININGTFC").html($(data).filter('#clearcoINTERLININGTFC'));
        }
    });

}
function save1() {
    let sectorfrom = $("#sectorfrom ").val();
    let sectorto = $("#sectorto").val();
    let taxcode = $("#taxcode").val();
    let taxname = $("#taxname").val(); 
    let effectivedate = $("#effectivedate").val(); 
    let expirydate = $("#expirydate").val(); 
    let percentage = $("#percentage0").val(); 
    let amount = $("#amount").val();
    let currency = $("#currency").val(); 
    let detail = $("#detail").val(); 
    let newFromSector = $("#newFromSector").val(); 
    let newtoSector = $("#newtoSector").val(); 
    let newaptaxcode = $("#newaptaxcode").val(); 
    let newapname = $("#newapname").val(); 
    let saledate = $("#saledate").val(); 
    let traveldate = $("#traveldate").val();
    let passengersdomestic = document.getElementById("passengersdomestic").checked;
    let passengersinternational = document.getElementById("passengersinternational").checked;
    let airlinesdomestic = document.getElementById("airlinesdomestic").checked;
    let airlinesinternational = document.getElementById("airlinesinternational").checked;
    let APPLICABLETO = document.getElementById("APPLICABLETO").checked;
    let INTERNATIONAL = document.getElementById("INTERNATIONAL").checked;
    let DOMESTIC = document.getElementById("DOMESTIC").checked;
    let salecollection = document.getElementById("salecollection").checked;
    let defaultGroupExample1 = document.getElementById("defaultGroupExample1").checked;
    let ptamco = document.getElementById("ptamco").checked;

    let cboSector = sectorfrom + " - " + sectorto;
    let cbotaxcode = taxcode;

    let temps = $('#temps').val();
    let flag1 = $('#flag1').val();
    let secondflag = $('#secondflag').val();

    let percentage0 = $('#percentage0').val();

    let urlg = symphony + "/Process/SaveSecond";
    let urlu = symphony + "/Process/updateAppTFC";

    let FlightTrans = cboSector;
    let FlightTrans1 = cbotaxcode;
    let url54 = symphony + "/Process/recherche";

    let urlSaveRefTax = symphony + "/Process/SaveRefTax";
    
    let from = $('#from').val(); 
    let to = $('#to').val(); 
    let prime = $('#prime').val(); 
    let passenger = $('#passenger').val(); 
    let domestic = $('#domestic').val(); 
    let duration = $('#duration').val(); 
    let validfrom = $('#validfrom').val(); 
    let validto = $('#validto').val(); 
    let taxcode1 = $('#taxcode1').val(); 
    let taxcurrency = $('#taxcurrency').val(); 
    let taxamount = $('#taxamount').val(); 
    let percentage12 = $('#percentage12').val();

    if (temps == 0) {

        let url = symphony + "/Process/delete";
        $.ajax({
            async: false,
            type: "POST",
            url: url,
            data: { sectorfrom: sectorfrom, sectorto: sectorto, taxcode: taxcode },
            beforeSend: function () {
            },
            success: function (data) {
            },
            complete: function () {
            },
            error: function () {
            }
        });
        if ($("#sectorfrom").val() == "") {
            $('#alertModal').modal('show');
            $('#msgalert').text('Sector From Code is compulsory');
        }
        else if ($("#sectorto").val() == "") {
            $('#alertModal').modal('show');
            $('#msgalert').text('Sector To is compulsory');
        }
        else if ($("#taxname").val() == "") {
            $('#alertModal').modal('show');
            $('#msgalert').text('Tax Name is compulsory');
        }
        else if ($("#taxcode").val() == "") {
            $('#alertModal').modal('show');
            $('#msgalert').text('Tax Code is compulsory');
        }
        else if ($("#taxdefinition").val() == "") {
            $('#alertModal').modal('show');
            $('#msgalert').text('Tax Definition is compulsory');
        }
        else if (APPLICABLETO == false && INTERNATIONAL == false && DOMESTIC == false) {
            $('#alertModal').modal('show');
            $('#msgalert').text('Only One type of Remittance can be check');
        }
        else {
            let url1 = symphony + "/Process/saveoperation";
            $.ajax({
                async: false,
                type: "POST",
                url: url1,
                data: { cbotaxcode: cbotaxcode, cboSector: cboSector, taxname: taxname, sectorfrom: sectorfrom, sectorto: sectorto, taxcode: taxcode, passengersdomestic: passengersdomestic, passengersinternational: passengersinternational, airlinesdomestic: airlinesdomestic, airlinesinternational: airlinesinternational, APPLICABLETO: APPLICABLETO, INTERNATIONAL: INTERNATIONAL, DOMESTIC: DOMESTIC, salecollection: salecollection, defaultGroupExample1: defaultGroupExample1, ptamco: ptamco },
                beforeSend: function () {
                },
                success: function (data) {
                    $('#alertModal').modal('show');
                    $('#msgalert').text('Record Save Sucessfully');
                    $("#divFlightTrans").html($(data).filter('#newwdivFlightTrans'));
                    $("#taxx").html($(data).filter('#newwtaxx'));
                },
                complete: function () {
                },
                error: function () {
                    $('#alertModal').modal('show');
                    $('#msgalert').text('Record didnt save Successfully');
                }
            });

            let FlightTrans = cboSector;
            let FlightTrans1 = cbotaxcode;
            let url54 = symphony + "/Process/recherche";
            $.ajax({
                async: true,
                type: "POST",
                url: url54,
                data: { FlightTrans: FlightTrans, FlightTrans1: FlightTrans1 },
                success: function (data) {
                    $("#coGENERALTFCINFO").html($(data).filter('#newcoGENERALTFCINFO'));
                    $("#coAPPLICABLETFC").html($(data).filter('#newcoAPPLICABLETFC'));
                    $("#coINTERLININGTFC").html($(data).filter('#newcoINTERLININGTFC'));
                },
                complete: function () {
                },
            });
        }

    }

    if (temps == 1)
    {
        if (flag1 == "true" && secondflag == "true")
        {
                /*SaveSecond*/
                $.ajax({
                    type: "POST",
                    url: urlg,
                    data: { sectorfrom: sectorfrom, sectorto: sectorto, taxcode: taxcode, effectivedate: effectivedate, expirydate: expirydate, percentage: percentage, amount: amount, currency: currency, detail: detail, newFromSector: newFromSector, newtoSector: newtoSector, newaptaxcode: newaptaxcode, newapname: newapname, FlightTrans: cboSector, FlightTrans1: cbotaxcode },
                    success: function (data) {
                        $('#alertModal').modal('show');
                        $('#msgalert').text('Record Save Sucessfully');
                        alert('vita marina');
                    },
                    error: function () {
                        $('#alertModal').modal('show');
                        $('#msgalert').text('Record didnt Save');
                    }
                });
                /*fin SaveSecond*/
                $('#secondflag').val("false");
                $.ajax({
                    async: true,
                    type: "POST",
                    url: url54,
                    data: { FlightTrans: FlightTrans, FlightTrans1: FlightTrans1 },
                    success: function (data) {
                        $("#coGENERALTFCINFO").html($(data).filter('#newcoGENERALTFCINFO'));
                        $("#coAPPLICABLETFC").html($(data).filter('#newcoAPPLICABLETFC'));
                        $("#coINTERLININGTFC").html($(data).filter('#newcoINTERLININGTFC'));
                    },
                    complete: function () {
                    },
                });
        }
        else if (flag1 == "false")
        {
            /*updateAppTFC*/
            $.ajax({
                type: "POST",
                url: urlu,
                data: { sectorfrom: sectorfrom, sectorto: sectorto, taxcode: taxcode, taxname: taxname, effectivedate: effectivedate, expirydate: expirydate, percentage0: percentage0, amount: amount, currency: currency, saledate: saledate, detail: detail, traveldate: traveldate },
                success: function (data) {
                    $('#alertModal').modal('show');
                    $('#msgalert').text('Record Save Sucessfully');

                    $.ajax({
                        async: true,
                        type: "POST",
                        url: url54,
                        data: { FlightTrans: FlightTrans, FlightTrans1: FlightTrans1 },
                        success: function (data) {
                            $("#coGENERALTFCINFO").html($(data).filter('#newcoGENERALTFCINFO'));
                            $("#coAPPLICABLETFC").html($(data).filter('#newcoAPPLICABLETFC'));
                            $("#coINTERLININGTFC").html($(data).filter('#newcoINTERLININGTFC'));
                        },
                        complete: function () {
                        },
                    });
                },
                error: function () {
                    $('#alertModal').modal('show');
                    $('#msgalert').text('Record didnt Save');
                }
            });
            /*fin updateAppTFC*/
        }

    }

    if (temps == 2) {
        /*SaveRefTax*/
        $.ajax({
            type: "POST",
            url: urlSaveRefTax,
            data: { from: from, to: to, prime: prime, passenger: passenger, domestic: domestic, duration: duration, validfrom: validfrom, validto: validto, taxcode1: taxcode1, taxcurrency: taxcurrency, taxamount: taxamount, percentage12: percentage12 },
            success: function (data) {
                $('#alertModal').modal('show');
                $('#msgalert').text('Record Save Sucessfully');

                $.ajax({
                    async: true,
                    type: "POST",
                    url: url54,
                    data: { FlightTrans: FlightTrans, FlightTrans1: FlightTrans1 },
                    success: function (data) {
                        $("#coGENERALTFCINFO").html($(data).filter('#newcoGENERALTFCINFO'));
                        $("#coAPPLICABLETFC").html($(data).filter('#newcoAPPLICABLETFC'));
                        $("#coINTERLININGTFC").html($(data).filter('#newcoINTERLININGTFC'));
                    },
                    complete: function () {
                    },
                });
            },
            error: function () {
                $('#alertModal').modal('show');
                $('#msgalert').text('Record didnt Save');
            }
        });
        /*fin SaveRefTax*/
    }
}
function addNew() {
    if ($("#add").hasClass("btn1")) {
        if ($("#plbrange").val() == "") {
            $('#alertModal').modal('show');
            $('#msgalert').text('PLBRange is compulsory');
        }
        else if ($("#plbper").val() == "") {
            $('#alertModal').modal('show');
            $('#msgalert').text('PLBPercentage is compulsory');
        }
        if ($("#plbrange").val() != "") {
            let agCode = $("#bonusAc").val();
            let finYear = $("#headFinancial").val();
            let plbrange = $("#plbrange").val();
            let plbper = $("#plbper").val();
            let url = symphony + "/Sales/saveOperation2";
            let action = $("#add1").text();
            $.ajax({
                type: 'POST',
                url: url,
                data: { agCode: agCode, finYear: finYear, plbrange: plbrange, plbper: plbper, action: action },
                beforeSend: function () {
                    $('.ajax-loader').css("visibility", "visible");
                },
                success: function (data) {
                    $("#details").html(data);

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
    else {
        $('#alertModal').modal('show');
        $('#msgalert').text('Record didin\'t save succesfully');
    }

}
function edit() {
    let FlightTrans = $("#FlightTrans").val();
    let FlightTrans1 = $("#FlightTrans1").val();
    let reftaxid = $("#reftaxid").val();
    let temps = $('#temps').val();
    let from = $("#from").val();
    let duration = $("#duration").val();
    let taxamount = $("#taxamount").val();
    let to = $("#to").val();
    let validfrom = $("#validfrom").val();
    let percentage = $("#percentage").val();
    let prime = $("#prime").val();
    let validto = $("#validto").val();
    let passenger = $("#passenger").val();
    let taxcode = $("#taxcode").val();
    let domestic = $("#domestic").val();
    let taxcurrency = $("#taxcurrency").val();
    
    let percentage0 = $("#percentage0").val();
    let currency = $("#currency").val();
    let amount = $("#amount").val();
    let effectivedate = $("#effectivedate").val();
    let expirydate = $("#expirydate").val();
    let saledate = $("#saledate").val();
    let traveldate = $("#traveldate").val();
    let detail = $("#detail").val();

    let exemptionscode = $("#exemptionscode").val();
    let exemptiondetails = $("#exemptiondetails").val();
    let dom = $("#dom").val();



    let url = symphony + "/Process/edit";
    $.ajax({
        type: "POST",
        url: url,
        data: { FlightTrans: FlightTrans, FlightTrans1: FlightTrans1 },
        success: function (data) {
            $("#divFlightTrans").html($(data).filter('#eddivFlightTrans'));
            $("#taxx").html($(data).filter('#edtaxx'));
                $("#coGENERALTFCINFO").html($(data).filter('#editcoGENERALTFCINFO'));
                $("#coAPPLICABLETFC").html($(data).filter('#editcoAPPLICABLETFC'));
                $("#coINTERLININGTFC").html($(data).filter('#editcoINTERLININGTFC'));
                if (temps == 1) {
                    $('#flag1').val("false");

                    $("#percentage0").val(percentage);
                    $("#currency").val(currency);
                    $("#amount").val(amount);
                    $("#effectivedate").val(effectivedate);
                    $("#expirydate").val(expirydate);
                    $("#saledate").val(saledate);
                    $("#traveldate").val(traveldate);
                    $("#detail").val(detail);
                    $("#exemptionscode").val(exemptionscode);
                    $("#exemptiondetails").val(exemptiondetails);
                    $("#dom").val(dom);
                }
                if (temps == 2) {
                    let urldelReftax = symphony + "/Process/delReftax";
                    $.ajax({
                        type: "POST",
                        url: urldelReftax,
                        data: { reftaxid: reftaxid },
                        success: function (data) {
                            $("#from").val(from);
                            $("#duration").val(duration);
                            $("#taxamount").val(taxamount);
                            $("#to").val(to);
                            $("#validfrom").val(validfrom);
                            $("#percentage0").val(percentage);
                            $("#prime").val(prime);
                            $("#validto").val(validto);
                            $("#passenger").val(passenger);
                            $("#taxcode").val(taxcode);
                            $("#domestic").val(domestic);
                            $("#taxcurrency").val(taxcurrency);
                        }
                    });
                }
            }
    });


    

    
}
function deletedonne() {

    let sectorfrom = $("#sectorfrom").val();
    let sectorto = $("#sectorto").val();
    let taxcode = $("#taxcode").val();
    let FlightTrans = $("#FlightTrans").val();
    let FlightTrans1 = $("#FlightTrans1").val();


    let temps = $('#temps').val();
    let url = symphony + "/Process/deletedonne";
    let url1 = symphony + "/Process/recherche";
    let urlApptaxdelete = symphony + "/Process/Apptaxdelete";

    
    let txtsectorfrom = $("#sectorfrom").val();
    let txtsectorto = $("#sectorto").val();
    let TxtTaxCode = $("#taxcode").val();
    let effectiveDate = $("#effectivedate").val();
    let expirydate = $("#expirydate").val();
    let Percentage = $("#percentage").val();
    let RADTAmount = $("#amount").val();
    let RADTCurrency = $("#currency").val();
    let RATDDetails = $("#detail").val();

    let urldelReftax = symphony + "/Process/delReftax";
    let reftaxid = $("#reftaxid").val();


    if (temps == 0) {
        if (window.confirm("Are you sure to delete this tax entry?")) {
            /*delete*/
            $.ajax({
                type: "POST",
                url: url,
                data: { sectorfrom: sectorfrom, sectorto: sectorto, taxcode: taxcode },
                success: function (data) {
                    alert('Record Delet Sucessfully');
                },
                complete: function () {
                },
            });
            /*fin delete*/
            $.ajax({
                async: true,
                type: "POST",
                url: url1,
                data: { FlightTrans: FlightTrans, FlightTrans1: FlightTrans1 },
                success: function (data) {
                    $("#coGENERALTFCINFO").html($(data).filter('#newcoGENERALTFCINFO'));
                    $("#coAPPLICABLETFC").html($(data).filter('#newcoAPPLICABLETFC'));
                    $("#coINTERLININGTFC").html($(data).filter('#newcoINTERLININGTFC'));
                },
                complete: function () {
                },
            });
        }
    }
    if (temps == 1) {
        if (window.confirm("Are you sure to delete this tax entry?")) {
            /*Apptaxdelete*/
            $.ajax({
                type: "POST",
                url: urlApptaxdelete,
                data: { txtsectorfrom: txtsectorfrom, txtsectorto: txtsectorto, TxtTaxCode: TxtTaxCode, effectiveDate: effectiveDate, expirydate: expirydate, Percentage: Percentage, RADTAmount: RADTAmount, RADTCurrency: RADTCurrency, RATDDetails: RATDDetails },
                success: function (data) {
                    $.ajax({
                        async: true,
                        type: "POST",
                        url: url1,
                        data: { FlightTrans: FlightTrans, FlightTrans1: FlightTrans1 },
                        success: function (data) {
                            $("#coGENERALTFCINFO").html($(data).filter('#newcoGENERALTFCINFO'));
                            $("#coAPPLICABLETFC").html($(data).filter('#newcoAPPLICABLETFC'));
                            $("#coINTERLININGTFC").html($(data).filter('#newcoINTERLININGTFC'));
                        },
                        complete: function () {
                        },
                    });
                },
                complete: function () {
                },
            });
            /*fin Apptaxdelete*/
        }
    }
    if (temps == 2) {
        if (window.confirm("Are you sure to delete this tax entry?")) {
            /*delReftax*/
            $.ajax({
                type: "POST",
                url: urldelReftax,
                data: { reftaxid: reftaxid },
                success: function (data) {
                    $.ajax({
                        async: true,
                        type: "POST",
                        url: url1,
                        data: { FlightTrans: FlightTrans, FlightTrans1: FlightTrans1 },
                        success: function (data) {
                            $("#coGENERALTFCINFO").html($(data).filter('#newcoGENERALTFCINFO'));
                            $("#coAPPLICABLETFC").html($(data).filter('#newcoAPPLICABLETFC'));
                            $("#coINTERLININGTFC").html($(data).filter('#newcoINTERLININGTFC'));
                        },
                        complete: function () {
                        },
                    });
                },
                complete: function () {
                },
            });
            /*fin delReftax*/
        }
    }
}
function save2() {
    alert('Record didnt Save');
}
function searchrefundaudit() {

    let dateFrom = $("#refundAuditFrom").val();
    let dateTo = $("#refundAuditTo").val();

    let url = symphony + "/Process/searchrefundaudit";
    $.ajax({
        type: 'POST',
        url: url,
        data: { dateFrom: dateFrom, dateTo: dateTo },
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#refundautidtbody").html($(data).filter('#newrefundautidtbody'));
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
            tableExchangedDocs();
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}
function page0() {
    $('#temps').val(0);
}
function page1() {
    $('#temps').val(1);
}
function page2() {
    $('#temps').val(2);
}
function getcsvtfcgeneral() {
    $(document).ready(function () {
        $('#tabgen').DataTable({
            "pageLength": 100,
            dom: 'Bfrtip',
            buttons: [
				 'csv'
            ]
        });
    });
}
function agentRBD() {
    let dateFromTFC = $('#dateFromTFC').val();
    let dateToTFC = $('#dateToTFC').val();
    let url = symphony + "/Process/agentRBD";
    $.ajax({
        async: true,
        type: "POST",
        url: url,
        data: { dateFromTFC: dateFromTFC, dateToTFC: dateToTFC },
        success: function (data) {
            $("#agentRBD").html(data);
        }
    });
}
function param() {
    let selectagentRBD = $('#selectagentRBD').val();

    if (selectagentRBD == "-All-") {
        $('#ag').val("%");
    }
    else {
        $('#ag').val(selectagentRBD);
    }
}
function btnSearch_Clickdiscount() {
    let dateFromTFC = $('#dateFromTFC').val();
    let dateToTFC = $('#dateToTFC').val();
    let ag = $('#ag').val();
    let url = symphony + "/Process/btnSearch_Click";
    $.ajax({
        async: true,
        type: "POST",
        url: url,
        data: { dateFromTFC: dateFromTFC, dateToTFC: dateToTFC, ag: ag },
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#tableagent").html($(data).filter('#newtableagent'));
            $("#divtxtagtname").html($(data).filter('#newdivtxtagtname'));
            $("#divtxtagtadd").html($(data).filter('#newdivtxtagtadd'));
            getpagination();
        },
        complete: function () {
            let tempsnb = $('#tempsnb').val();
            $('.ajax-loader').css("visibility", "hidden");
            if (tempsnb == "0") {
                $('#alertModal').modal('show');
                $('#msgalert').text('No data available for the selected criteria.');
            }
        },
    });
}
function getpagination() {
    $(document).ready(function () {
        $('#tablecontenuagent').DataTable({
            "pageLength": 150,
        });
    });
}
function CellContentDoubleClick(event, docNumber) {
    var $form = $(event).closest('markelement');
    let parentId = $form.attr('id')
    let url = symphony + "/Sales/Transaction2";
    if (event == null) {
        parentId = 'GrandParent_PAXTKTs';
    }
    $.ajax({
        type: 'POST',
        data: { docNumber: docNumber},
        url: url,
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {

            /* $("li").removeClass("active");
             $("#" + parentId + " #search").removeClass("active");
             $("#" + parentId + " #testtrans").addClass("active");
             $("#" + parentId + " #transaction").addClass("active in");
             $("#" + parentId + " .data-transaction").html(data);
             $("#" + parentId + " .data-transaction").html(data);*/

            $("#search").html(data);
            $("#transaction").html(data);
        },
        complete: function (date) {
            $('.ajax-loader').css("visibility", "hidden");
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}
function btnReset_Click() {
    let url = symphony + "/Process/btnReset_Click";
    $.ajax({
        async: true,
        type: "POST",
        url: url,
        data: {  },
        success: function (data) {
            $("#tableagent").html($(data).filter('#newtableagent'));
            $("#divtxtagtname").html($(data).filter('#newdivtxtagtname'));
            $("#divtxtagtadd").html($(data).filter('#newdivtxtagtadd'));
            $("#agentRBD").html($(data).filter('#newagentRBD')); 
            $("#fieldsetdiscount").html($(data).filter('#newfieldsetdiscount'));
        },
    });
}
function searchfinalsharevalidation() {
    let finalSareFrom = $('#finalSareFrom').val();
    let finalSareTo = $('#finalSareTo').val();
    let residual = $('#residual').val();
    let url = symphony + "/Process/Query";

    
    if (residual == "") {
        $('#alertModal').modal('show');
        $('#msgalert').text('Please Enter a Threshold Value Residue.');
    }
    else {
        /*Query*/
        $.ajax({
            async: true,
            type: "POST",
            url: url,
            data: { finalSareFrom: finalSareFrom, finalSareTo: finalSareTo, residual: residual },
            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {
                $("#display-datashare").html(data);
                paginationfinalsharevalidation();
            },
            complete: function () {
                let temps = $('#temps').val();
                $('.ajax-loader').css("visibility", "hidden");
                if (temps == "0") {
                    $('#alertModal').modal('show');
                    $('#msgalert').text('No data available for the selected criteria.');
                }
            },
        });
        /*fin Query*/ 
        if (dgfinal.Rows.Count < 1) {
            MessageBox.Show("No data available for the selected criteria.", "Symphony",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
function paginationfinalsharevalidation() {
    $(document).ready(function () {
        $('#tablefinalsharevalidation').DataTable({
            "pageLength": 150,
        });
    });
}
function clearfinalsharevalidation() {
    let url = symphony + "/Process/clearfinalsharevalidation";
    $.ajax({
        async: true,
        type: "POST",
        url: url,
        data: {},
        success: function (data) {
            $('#residual').val(""); 
            $('#pageShare').val(""); 

            $("#divfinalSareFrom").html($(data).filter('#newdivfinalSareFrom'));
            $("#divfinalSareTo").html($(data).filter('#newdivfinalSareTo'));
            $("#display-datashare").html($(data).filter('#newdisplay-datashare'));
        },
    });
}
function doubleclicktablefinal(event, doc) {
    var $form = $(event).closest('markelement');
    let parentId = $form.attr('id')
    if (event == null) {
        parentId = 'GrandParent_PAXTKTs';
    }
    let document = doc;
    let url = symphony + "/Process/SearchFinalSharevsAmount";
    let urlaz = symphony + "/Process/ShowtableFinalSharevsAmount";
    tab = [document];
    $.ajax({
        type: 'POST',
        url: url,
        data: { dataValue: tab },
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#search").html(data);
            $('#DocumentNoFS').val(document);
            console.log(data);
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
    /*ShowTableFinalSharevAmount*/
    $.ajax({
        type: 'POST',
        url: urlaz,
        data: { dataValue: tab },
        success: function (data) {
            console.log(data);
            $(".TableFSA").html(data);
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
        },
        error: function () {
            $('#alertModal').modal('show');
            $("#msgalert").html("Error");
        }
    });
    /*fin ShowTableFinalSharevAmount*/
}
function onchangedtpToValue() {
    let dtpFromValue = $('#dtpFromValue').val();
    let dtpToValue = $('#dtpToValue').val();
    let url = symphony + "/Process/ErrorMSG";
    $.ajax({
        type: "POST",
        url: url,
        data: { dtpFromValue: dtpFromValue, dtpToValue: dtpToValue },
        success: function (data) {
            $("#selectpev").html(data);
        },
    });
}
function onchangeselectpev() {
    let ag = $('#ag').val();
    let selectpev = $('#selectpev').val();
    /*Param*/
    if (selectpev == "-All-") {
        $('#ag').val("%");
    }
    else {
        $('#ag').val(selectpev);
    }
    /*fin Param*/
}
function onchangedocpev() {
    let docpev = $('#docpev').val();
    let doc = $('#doc').val();
    /*Param*/
    if (docpev != "")
    {
        $('#doc').val(docpev);
        $('#selectpev').val("-All-");
        $('#ag').val("%");
    }
    else
    {
        $('#doc').val("%");
    }
    /*fin Param*/
}
function SEARCHpev() {
    $('#txtDocCount').val("");
    let dtpFromValue = $('#dtpFromValue').val();
    let dtpToValue = $('#dtpToValue').val();
    let dtpIssueDateFromValue = $('#dtpIssueDateFromValue').val();
    let dtpIssueDateToValue = $('#dtpIssueDateToValue').val();

    let dtpFromChecked = document.getElementById("dtpFromChecked").checked;
    let dtpToChecked = document.getElementById("dtpToChecked").checked;
    let dtpIssueDateFromChecked = document.getElementById("dtpIssueDateFromChecked").checked;
    let dtpIssueDateToChecked = document.getElementById("dtpIssueDateToChecked").checked;

    let ag = $('#ag').val();
    let doc = $('#doc').val();
    let docpev = $('#docpev').val();
    let selectpev = $('#selectpev').val();
    /*Querypev*/
    let url = symphony + "/Process/Querypev";
    $.ajax({
        async: true,
        type: "POST",
        url: url,
        data: { dtpFromValue: dtpFromValue, dtpToValue: dtpToValue, dtpIssueDateFromValue: dtpIssueDateFromValue, dtpIssueDateToValue: dtpIssueDateToValue, dtpFromChecked: dtpFromChecked, dtpToChecked: dtpToChecked, dtpIssueDateFromChecked: dtpIssueDateFromChecked, dtpIssueDateToChecked: dtpIssueDateToChecked, ag: ag, doc: doc, docpev: docpev, selectpev: selectpev },
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            //$("#divtablepev").html($(data).filter('#newdivtablepev'));
            $("#divtablepev").html(data);
        },
        complete: function () {
            let temps = $('#temps').val();
            $('#txtDocCount').val(temps);
            $('.ajax-loader').css("visibility", "hidden");
        },
    });
    /*fin Querypev*/
    $('#label').text("");
    $('#label1').text("");

}
function doublecliquetablepev(fca) {
    $('#label').text(fca);
    /*label1*/
    let dtpFromValue = $('#dtpFromValue').val();
    let dtpToValue = $('#dtpToValue').val();
    let dtpIssueDateFromValue = $('#dtpIssueDateFromValue').val();
    let dtpIssueDateToValue = $('#dtpIssueDateToValue').val();

    let dtpFromChecked = document.getElementById("dtpFromChecked").checked;
    let dtpToChecked = document.getElementById("dtpToChecked").checked;
    let dtpIssueDateFromChecked = document.getElementById("dtpIssueDateFromChecked").checked;
    let dtpIssueDateToChecked = document.getElementById("dtpIssueDateToChecked").checked;

    let ag = $('#ag').val();
    let doc = $('#doc').val();
    let docpev = $('#docpev').val();
    let selectpev = $('#selectpev').val();

    /*fcadoc*/
    let url = symphony + "/Process/fcadoc";
    $.ajax({
        type: "POST",
        url: url,
        data: { fca: fca, dtpFromValue: dtpFromValue, dtpToValue: dtpToValue, dtpIssueDateFromValue: dtpIssueDateFromValue, dtpIssueDateToValue: dtpIssueDateToValue, dtpFromChecked: dtpFromChecked, dtpToChecked: dtpToChecked, dtpIssueDateFromChecked: dtpIssueDateFromChecked, dtpIssueDateToChecked: dtpIssueDateToChecked, ag: ag, doc: doc, docpev: docpev, selectpev: selectpev },
        success: function (data) {
            $("#divfca").html(data);
        },
        complete: function () {
            let fcaa = $('#fca').val();
            $('#label1').text(fcaa);
        },
    });
    /*fin fcadoc*/
    /*fin label1*/
}
function clearpev() {
    $('#docpev').val("");
    $('#selectpev').val("-All-");
    $('#label').text("");
    $('#label1').text("");
    $('#txtDocCount').val("");
    let url = symphony + "/Process/clearpev";
    $.ajax({
        async: true,
        type: "POST",
        url: url,
        data: {},
        success: function (data) {
            $("#divprocessdatefrom").html($(data).filter('#newdivprocessdatefrom'));
            $("#divprocessdateto").html($(data).filter('#newdivprocessdateto'));
            $("#divissuedatefrom").html($(data).filter('#newdivissuedatefrom'));
            $("#divissuedateto").html($(data).filter('#newdivissuedateto'));
            $("#divtablepev").html($(data).filter('#newdivtablepev'));
        },
    });
}
function AgentNum() {
    let pricingFrom = $('#pricingFrom').val();
    let pricingTo = $('#pricingTo').val();
    let url = symphony + "/Process/AgentNum";
    $.ajax({
        async: true,
        type: "POST",
        url: url,
        data: { pricingFrom: pricingFrom, pricingTo: pricingTo },
        success: function (data) {
            $("#divselectpricing-Agc").html(data);
        },
    });
}
function frmcode() {
    let pagepricing = $('#pagepricing').val();
    if(pagepricing == "0"){
        let pricingFrom = $('#pricingFrom').val();
        let pricingTo = $('#pricingTo').val();
        let code = $('#tempscode').val();
        let url = symphony + "/Process/frmcode";
        $.ajax({
            async: true,
            type: "POST",
            url: url,
            data: { pricingFrom: pricingFrom, pricingTo: pricingTo, code: code },
            success: function (data) {
                $("#divselectpricing-Agc").html($(data).filter("#newdivselectpricing-Agc"));
                $("#divpricing-AgnName").html($(data).filter("#newdivpricing-AgnName"));
                $("#divpricing-AgcCode").html($(data).filter("#newdivpricing-AgcCode"));
            },
        });
    }
    if (pagepricing == "1") {
        let basisFrom = $('#basisFrom').val();
        let basisTo = $('#basisTo').val();
        let code = $('#tempscode').val();
        let url = symphony + "/Process/frmcode1";
        $.ajax({
            async: true,
            type: "POST",
            url: url,
            data: { basisFrom: basisFrom, basisTo: basisTo, code: code },
            success: function (data) {
                $("#divselectbasicpricing").html($(data).filter("#newdivselectbasicpricing"));
                $("#divbasis-Agn").html($(data).filter("#newdivbasis-Agn"));
                $("#divbasis-Agc").html($(data).filter("#newdivbasis-Agc"));
            },
        });
    }
    if (pagepricing == "2") {
        let rbdFrom = $('#rbdFrom').val();
        let rbdTo = $('#rbdTo').val();
        let code = $('#tempscode').val();
        let url = symphony + "/Process/frmcode2";
        $.ajax({
            async: true,
            type: "POST",
            url: url,
            data: { rbdFrom: rbdFrom, rbdTo: rbdTo, code: code },
            success: function (data) {
                $("#divcboagt3").html($(data).filter("#newdivcboagt3"));
                $("#divrbd-Agn").html($(data).filter("#newdivrbd-Agn"));
                $("#divrbd-Agc").html($(data).filter("#newdivrbd-Agc"));
            },
        });
    }
    if (pagepricing == "4") {
        let fareCompFrom = $('#fareCompFrom').val();
        let fareCompTo = $('#fareCompTo').val();
        let code = $('#tempscode').val();
        let url = symphony + "/Process/frmcode4";
        $.ajax({
            async: true,
            type: "POST",
            url: url,
            data: { fareCompFrom: fareCompFrom, fareCompTo: fareCompTo, code: code },
            success: function (data) {
                $("#divcboagt5farecomppricing").html($(data).filter("#newdivcboagt5farecomppricing"));
                $("#divfare-Agn").html($(data).filter("#newdivfare-Agn"));
                $("#divfare-Agc").html($(data).filter("#newdivfare-Agc"));
            },
        });
    }
}
function pagepricing(value) {
    $('#pagepricing').val(value);
}
function Testagcode(value) {
    let cboAgt = $('#pricing-Agc').val;
    let url = symphony + "/Process/Testagcode";
    $.ajax({
        async: true,
        type: "POST",
        url: url,
        data: { cboAgt: value },
        success: function (data) {
            $("#divpricing-AgnName").html($(data).filter('#newdivpricing-AgnName'));
            $("#divpricing-AgcCode").html($(data).filter('#newdivpricing-AgcCode'));
        },
    });
}
function cbSelection_SelectedIndexChanged_1(cbSelection) {

    if (cbSelection == "Sector") {

        let url = symphony + "/Process/cbSelectionSector";
        $.ajax({
            async: true,
            type: "POST",
            url: url,
            data: {  },
            success: function (data) {
                $("#content-pricing").html($(data).filter('#newcontent-pricing'));
                $("#bouttonhideShowPric").html($(data).filter('#newbouttonhideShowPric'));
            },
        });
    }
    if (cbSelection == "Fare Component") {

        let url = symphony + "/Process/cbSelectionFareComponent";
        $.ajax({
            async: true,
            type: "POST",
            url: url,
            data: {},
            success: function (data) {
                $("#content-pricing").html($(data).filter('#newcontent-pricing'));
                $("#bouttonhideShowPric").html($(data).filter('#newbouttonhideShowPric'));
            },
        });
    }
    if (cbSelection == "Journey") {

        let url = symphony + "/Process/cbSelectionJourney";
        $.ajax({
            async: true,
            type: "POST",
            url: url,
            data: {},
            success: function (data) {
                $("#content-pricing").html($(data).filter('#newcontent-pricing'));
                $("#bouttonhideShowPric").html($(data).filter('#newbouttonhideShowPric'));
            },
        });
    }
}
function btnSearch_Clickpricing() {

    let cbSelection = $('#cbSelection').val();
    let urlQUERY = symphony + "/Process/QUERYpricing";
    let cboAgt = $('#pricing-Agc').val(); 
    let cmbdow = $('#cmbdow').val(); cbSelection
    let pricingFrom = $('#pricingFrom').val();
    let pricingTo = $('#pricingTo').val();

    if (cbSelection != "") {
        /*QUERY*/
        $.ajax({
            async: true,
            type: "POST",
            url: urlQUERY,
            data: { cbSelection: cbSelection, cboAgt: cboAgt, cmbdow: cmbdow, pricingFrom: pricingFrom, pricingTo: pricingTo },
            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {
                $("#content-pricing").html($(data).filter('#newcontent-pricing'));
            },
            complete: function () {
                let count = $('#count').val();
                $('.ajax-loader').css("visibility", "hidden");
                if (count == "0") {
                    $('#alertModal').modal('show');
                    $('#msgalert').text('No data available for the selected criteria.');
                }
                getcsvpricing();
            },
        });
        /*fin QUERY*/
    }
    else {
        $('#alertModal').modal('show');
        $('#msgalert').text('Please Select an Option from the Selection DropBox');
    }
}
function getcsvpricing() {
    $(document).ready(function () {
        $('#tablepricing').DataTable({
            "pageLength": 150,
            dom: 'Bfrtip',
            buttons: [
				 'csv'
            ]
        });
    });
}
function chercheprincing(myvar) {
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
function cboagt1_SelectedIndexChanged(value) {
    /*Test1*/
    let url = symphony + "/Process/Test1basicpricing";
    $.ajax({
        async: true,
        type: "POST",
        url: url,
        data: { cboagt1: value },
        success: function (data) {
            $("#divbasis-Agn").html($(data).filter('#newdivbasis-Agn'));
            $("#divbasis-Agc").html($(data).filter('#newdivbasis-Agc'));
        },
    });
    /*fin Test1*/
}
function AgentNum1pricing() {
    let basisFrom = $('#basisFrom').val();
    let basisTo = $('#basisTo').val();
    let url = symphony + "/Process/AgentNum1pricing";
    $.ajax({
        async: true,
        type: "POST",
        url: url,
        data: { basisFrom: basisFrom, basisTo: basisTo },
        success: function (data) {
            $("#divselectbasicpricing").html(data);
        },
    });
}
function FareBasispricing() {
    let basisFrom = $('#basisFrom').val();
    let basisTo = $('#basisTo').val();
    let url = symphony + "/Process/FareBasispricing";
    $.ajax({
        async: true,
        type: "POST",
        url: url,
        data: { basisFrom: basisFrom, basisTo: basisTo },
        success: function (data) {
            $("#divcbofarbasis").html(data);
        },
    });
}
function btnSearch1_Clickbasicpricing() {
    let cboagt1 = $('#cboagt1').val();
    let cboFBdow = $('#cboFBdow').val();
    let cbofarbasis = $('#cbofarbasis').val();
    let basisFrom = $('#basisFrom').val();
    let basisTo = $('#basisTo').val();
    /*QueryFarebasis*/
    let url = symphony + "/Process/btnSearch1_Clickbasicpricing";
    $.ajax({
        async: true,
        type: "POST",
        url: url,
        data: { cboagt1: cboagt1, cboFBdow: cboFBdow, cbofarbasis: cbofarbasis, basisFrom: basisFrom, basisTo: basisTo },
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#basis-content").html(data);
        },
        complete: function (date) {
            let tempsbasicpricing = $('#tempsbasicpricing').val();
            $('#basisNumRec').val(tempsbasicpricing);
            $('.ajax-loader').css("visibility", "hidden");
            if (tempsbasicpricing == "0") {
                $('#alertModal').modal('show');
                $('#msgalert').text('No data available for the selected criteria.');
            }
        },
    });
    /*fin QueryFarebasis*/
}
function onchangecboagt3_SelectedIndexChanged(value) {
    /*Test2*/
    let url = symphony + "/Process/onchangecboagt3_SelectedIndexChanged";
    $.ajax({
        async: true,
        type: "POST",
        url: url,
        data: { cboagt3: value },
        success: function (data) {
            $("#divrbd-Agn").html($(data).filter('#newdivrbd-Agn'));
            $("#divrbd-Agc").html($(data).filter('#newdivrbd-Agc'));
        },
    });
    /*fin Test2*/
}
function onchangeAgentNum2() {
    let rbdFrom = $('#rbdFrom').val();
    let rbdTo = $('#rbdTo').val();
    let url = symphony + "/Process/onchangeAgentNum2";
    $.ajax({
        async: true,
        type: "POST",
        url: url,
        data: { rbdFrom: rbdFrom, rbdTo: rbdTo },
        success: function (data) {
            $("#divcboagt3").html(data);
        },
    });
}
function onchangeResrBD() {
    let rbdFrom = $('#rbdFrom').val();
    let rbdTo = $('#rbdTo').val();
    let url = symphony + "/Process/onchangeResrBD";
    $.ajax({
        async: true,
        type: "POST",
        url: url,
        data: { rbdFrom: rbdFrom, rbdTo: rbdTo },
        success: function (data) {
            $("#divcboRBD").html(data);
        },
    });
}
function btnSearch3_Clickrbdpricing() {
    let cboagt3 = $('#cboagt3').val();
    let cboRBD = $('#cboRBD').val();
    let cboRBDwe = $('#cboRBDwe').val();
    let rbdFrom = $('#rbdFrom').val();
    let rbdTo = $('#rbdTo').val();
    /*QueryRBD*/
    let url = symphony + "/Process/btnSearch3_Clickrbdpricing";
    $.ajax({
        async: true,
        type: "POST",
        url: url,
        data: { cboagt3: cboagt3, cboRBD: cboRBD, cboRBDwe: cboRBDwe, rbdFrom: rbdFrom, rbdTo: rbdTo },
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#rbd-content").html(data);
        },
        complete: function (date) {
            let tempsrbdpricing = $('#tempsrbdpricing').val();
            $('#rbdNumRec').val(tempsrbdpricing);
            
            $('.ajax-loader').css("visibility", "hidden");

            if (tempsrbdpricing == "0") {
                $('#alertModal').modal('show');
                $('#msgalert').text('No data available for the selected criteria.');
            }
        },
    });
    /*fin QueryRBD*/
}
function onchangedatemarquepricing() {
    let marketFrom = $('#marketFrom').val();
    let marketTo = $('#marketTo').val();
    let url = symphony + "/Process/onchangedatemarquepricing";
    $.ajax({
        async: true,
        type: "POST",
        url: url,
        data: { marketFrom: marketFrom, marketTo: marketTo },
        success: function (data) {
            $("#divcboPoSmarket").html($(data).filter('#newdivcboPoSmarket'));
            $("#divcboFCMImarketpricing").html($(data).filter('#newdivcboFCMImarketpricing'));
            $("#divcboFOPFarebasismarketpricing").html($(data).filter('#newdivcboFOPFarebasismarketpricing')); 
            $("#divFOPmarketpricing").html($(data).filter('#newdivFOPmarketpricing'));

        },
    });
}
function btnSearch4_Clickmarketpricing() {
    let marketFrom = $('#marketFrom').val();
    let marketTo = $('#marketTo').val();
    let cboPoS = $('#cboPoSmarket').val();
    let cboFCMI = $('#cboFCMImarketpricing').val();
    let cboFOPFarebasis = $('#cboFOPFarebasismarketpricing').val();
    let FOP = $('#FOPmarketpricing').val();
    /*QueryFOP*/
    let url = symphony + "/Process/btnSearch4_Clickmarketpricing";
    $.ajax({
        async: true,
        type: "POST",
        url: url,
        data: { marketFrom: marketFrom, marketTo: marketTo, cboPoS: cboPoS, cboFCMI: cboFCMI, cboFOPFarebasis: cboFOPFarebasis, FOP: FOP },
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#content-market").html(data);
        },
        complete: function (date) {
            let tempsmarket = $('#tempsmarket').val();
            $('#basisNumRecmarket').val(tempsmarket);
            $('.ajax-loader').css("visibility", "hidden");
            if (tempsmarket == "0") {
                $('#alertModal').modal('show');
                $('#msgalert').text('No data available for the selected criteria.');
            }
        },
    });
    /*fin QueryFOP*/
}
function onchangedatefarecomp() {
    let fareCompFrom = $('#fareCompFrom').val();
    let fareCompTo = $('#fareCompTo').val();
    let url = symphony + "/Process/AgentNum5pricing";
    $.ajax({
        async: true,
        type: "POST",
        url: url,
        data: { fareCompFrom: fareCompFrom, fareCompTo: fareCompTo },
        success: function (data) {
            $("#divcboagt5farecomppricing").html(data);
        },
    });
}
function onchangecboagt5farecomppricing(value) {
    let url = symphony + "/Process/Test5farecomp";
    $.ajax({
        async: true,
        type: "POST",
        url: url,
        data: { cboagt5: value },
        success: function (data) {
            $("#divfare-Agn").html($(data).filter('#newdivfare-Agn'));
            $("#divfare-Agc").html($(data).filter('#newdivfare-Agc'));
        },
    });
}
function btnseach_Clickmfarecomppricing() {
    let cboagt5 = $('#cboagt5farecomppricing').val();
    let fareCompFrom = $('#fareCompFrom').val();
    let fareCompTo = $('#fareCompTo').val();
    /*Query5*/
    let url = symphony + "/Process/btnseach_Clickmfarecomppricing";
    $.ajax({
        async: true,
        type: "POST",
        url: url,
        data: { cboagt5: cboagt5, fareCompFrom: fareCompFrom, fareCompTo: fareCompTo },
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#fare-content").html(data);
        },
        complete: function (date) {
            let tempsfarecompt = $('#tempsfarecompt').val();
            $('#fareNumRec').val(tempsfarecompt);
            $('.ajax-loader').css("visibility", "hidden");
            if (tempsfarecompt == "0") {
                $('#alertModal').modal('show');
                $('#msgalert').text('No data available for the selected criteria.');
            }
        },
    });
    /*fin Query5*/
}
function btnSearch_Clickcoupon() {
    let couponUsageFrom = $('#couponUsageFrom').val();
    let couponUsageTo = $('#couponUsageTo').val();
    let dtpFromChecked = document.getElementById("dtpFromChecked").checked;
    let dtpToChecked = document.getElementById("dtpToChecked").checked;

    /*QueryFOP*/
    let url = symphony + "/Process/btnSearch_Clickcoupon";
    $.ajax({
        async: true,
        type: "POST",
        url: url,
        data: { couponUsageFrom: couponUsageFrom, couponUsageTo: couponUsageTo, dtpFromChecked: dtpFromChecked, dtpToChecked: dtpToChecked },
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#coupon-content").html(data);
        },
        complete: function (date) {
             let tempscoupona = $('#tempscoupona').val();
             $('#cpnNb').val(tempscoupona);
             getcsvcoupona();
             $('.ajax-loader').css("visibility", "hidden");
             if (tempscoupona == "0") {
                $('#alertModal').modal('show');
                $('#msgalert').text('No data available.');
             }
        },
    });
    /*fin QueryFOP*/
}
function getcsvcoupona() {
    $(document).ready(function () {
        $('#tablecoupon').DataTable({
            "pageLength": 100,
            dom: 'Bfrtip',
            buttons: [
				 'csv'
            ]
        });
    });
}
function btnCCMSearch_Click() {
    let creditCardFrom = $('#creditCardFrom').val();
    let creditCardTo = $('#creditCardTo').val();

    /*CCM*/
    let url = symphony + "/Process/btnCCMSearch_Click";
    $.ajax({
        async: true,
        type: "POST",
        url: url,
        data: { creditCardFrom: creditCardFrom, creditCardTo: creditCardTo },
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#content-creditManagement").html(data);
        },
        complete: function (date) {
            let tempscreditManagement = $('#tempscreditManagement').val();
           // getcsvcreditmanagementaz();
            $('.ajax-loader').css("visibility", "hidden");
            if (tempscreditManagement == "0") {
                $('#alertModal').modal('show');
                $('#msgalert').text('No data available..');
            }
        },
    });
    /*CCM*/
}
function getcsvcreditmanagementaz() {
    $(document).ready(function () {
        $('#tablecreditManagementaz').DataTable({
            dom: 'Bfrtip',
            buttons: [
				 'csv'
            ]
        });
    });
}
function CharacterCasing(value) {
    var temp = value.toUpperCase();
    $('#txtFareComponent').val(temp);
}
function CharacterCasing1(value) {
    var temp = value.toUpperCase();
    $('#txtEndorsementArea').val(temp);
}
function CharacterCasing2(value) {
    var temp = value.toUpperCase();
    $('#txtTourCode').val(temp);
}
function CharacterCasing3(value) {
    var temp = value.toUpperCase();
    $('#txtFrom').val(temp);
}
function CharacterCasing4(value) {
    var temp = value.toUpperCase();
    $('#txtTo').val(temp);
}
function CharacterCasing5(value) {
    var temp = value.toUpperCase();
    $('#txtCarrier').val(temp);
}
function CharacterCasing6(value) {
    var temp = value.toUpperCase();
    $('#txtClass').val(temp);
}
function CharacterCasing7(value) {
    var temp = value.toUpperCase();
    $('#txtStatus').val(temp);
}
function CharacterCasing8(value) {
    var temp = value.toUpperCase();
    $('#txtFareBasis').val(temp);
}
function CharacterCasing9(value) {
    var temp = value.toUpperCase();
    $('#txtBagAllowance').val(temp);
}
function CharacterCasing10(value) {
    var temp = value.toUpperCase();
    $('#txtBookingStatus').val(temp);
}
function CharacterCasing11(value) {
    var temp = value.toUpperCase();
    $('#txtRBD').val(temp);
}
function CharacterCasing12(value) {
    var temp = value.toUpperCase();
    $('#txtFFP').val(temp);
}
function CharacterCasing13(value) {
    var temp = value.toUpperCase();
    $('#txtUsageAirline').val(temp);
}
function CharacterCasing14(value) {
    var temp = value.toUpperCase();
    $('#txtUsageFrom').val(temp);
}
function CharacterCasing15(value) {
    var temp = value.toUpperCase();
    $('#txtUsageTo').val(temp);
}
function CharacterCasing16(value) {
    var temp = value.toUpperCase();
    $('#txtPNR2').val(temp);
}
function CharacterCasing17(value) {
    var temp = value.toUpperCase();
    $('#txtBookingAgentID').val(temp);
}
function CharacterCasing18(value) {
    var temp = value.toUpperCase();
    $('#txtVendorIdentifier').val(temp);
    var txtVendorIdentifier = $('#txtVendorIdentifier').val();
    $('#txtVendorIdentifier').val(txtVendorIdentifier.toUpperCase());
    var txtOrgDest = $('#txtOrgDest').val();
    $('#txtOrgDest').val(txtOrgDest.toUpperCase());
    var txtCPUI = $('#txtCPUI').val();
    $('#txtCPUI').val(txtCPUI.toUpperCase());
    var txttransactioncode = $('#txttransactioncode').val();
    $('#txttransactioncode').val(txttransactioncode.toUpperCase());
    var txtPassengerNam = $('#txtPassengerNam').val();
    $('#txtPassengerNam').val(txtPassengerNam.toUpperCase());
    var txtTotalamtcur = $('#txtTotalamtcur').val();
    $('#txtTotalamtcur').val(txtTotalamtcur.toUpperCase());
    var txtFareCurrency = $('#txtFareCurrency').val();
    $('#txtFareCurrency').val(txtFareCurrency.toUpperCase());
    var txtFarePaidCur = $('#txtFarePaidCur').val();
    $('#txtFarePaidCur').val(txtFarePaidCur.toUpperCase());
}
function btnSearch_ClickManualTicketEntry() {
    let _xtcdoc = $('#_xtcdoc').val();
    let txtIssuedInExchangeFor = $('#txtIssuedInExchangeFor').val();
    let url = symphony + "/Process/btnSearch_ClickManualTicketEntry";
    $.ajax({
        async: true,
        type: "POST",
        url: url,
        data: { _xtcdoc: _xtcdoc, txtIssuedInExchangeFor: txtIssuedInExchangeFor },
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#part").html(data);
        },
        complete: function (date) {
            let txtDocumentCarrier = $('#txtDocumentCarrier').val();
            let txtDocumentNumber = $('#txtDocumentNumber').val();
            if (txtDocumentCarrier != "" && txtDocumentNumber != "") {
                document.getElementById("txtTotalamtcur").readOnly = true;
                document.getElementById("txtDocumentCarrier").readOnly = true;
                document.getElementById("txtDocumentNumber").readOnly = true;
                document.getElementById("txtChkDgt").readOnly = true;
                document.getElementById("txtOrgDest").readOnly = true;
                document.getElementById("txtPNR2").readOnly = true;
                document.getElementById("txtVendorIdentifier").readOnly = true;
                document.getElementById("txtCPUI").readOnly = true;
                document.getElementById("txtReportingSystemIdentifier").readOnly = true;
                document.getElementById("txtTourCode").readOnly = true;
                document.getElementById("txtPassengerNam").readOnly = true;
                document.getElementById("txtCustomerCode").readOnly = true;
                document.getElementById("txtBookingAgentID").readOnly = true;
                document.getElementById("txtFareAmount").readOnly = true;
                document.getElementById("txtFareCurrency").readOnly = true;
                document.getElementById("txtFarePaidAmt").readOnly = true;
                document.getElementById("txtFarePaidCur").readOnly = true;
                document.getElementById("txtFareComponent").readOnly = true;
                document.getElementById("txtEndorsementArea").readOnly = true;
                document.getElementById("txtFcmi").readOnly = true;
                document.getElementById("txtIssuedInExchangeFor").readOnly = true;
                $('#txttransactioncode').val("TKTM");
                document.getElementById("FromManuelTicketScreen").disabled = false;
                document.getElementById("dtpIssDoc").disabled = false;
                document.getElementById("dbgCPNlist").readOnly = true;
                document.getElementById("dgother").readOnly = true;
                document.getElementById("dgpayment").readOnly = true;
                document.getElementById("dgOriDoc").readOnly = true;
                document.getElementById("btnEdit").disabled = false;
                document.getElementById("btnSave").disabled = false;
                document.getElementById("btnnew").disabled = false;
                document.getElementById("btnClear").disabled = false;
            }
            else {
                document.getElementById("btnEdit").disabled = true;
                document.getElementById("btnSave").disabled = true;
            }
            $('.ajax-loader').css("visibility", "hidden");
        },
    });
}
function btnAdd_Clickticketentry() {
    let txtBagAllowance = $('#txtBagAllowance').val();
    if (txtBagAllowance.Length > 3) {
        $('#alertModal').modal('show');
        $('#msgalert').text('Text should limited to three characters only. E.g 30K or 1PC');
    }
    var rw = $('#dbgCPNlist tr').length;
    /*addRow*/
    let elmt = document.getElementById('dbgCPNlist');
    let cboXO = document.getElementById('cboXO').value;
    let txtFrom = document.getElementById('txtFrom').value;
    let txtTo = document.getElementById('txtTo').value; 
    let txtCarrier = document.getElementById('txtCarrier').value; 
    let txtFltNo = document.getElementById('txtFltNo').value; 
    let txtClass = document.getElementById('txtClass').value; 
    let dtpdate = document.getElementById('dtpdate').value; 
    let txtTime = document.getElementById('txtTime').value; 
    let txtStatus = document.getElementById('txtStatus').value; 
    let txtFareBasis = document.getElementById('txtFareBasis').value; 
    let dateNotValidBefore = document.getElementById('dateNotValidBefore').value;
    let NotValidAfterdate = document.getElementById('NotValidAfterdate').value;
    let NotValidBefordate = document.getElementById("NotValidBefordate").checked;
    let dtpIssueDateTo1Noshow = document.getElementById("dtpIssueDateTo1Noshow").checked;
    let UsageDateManuel = document.getElementById("UsageDateManuel").checked;
    let txtBookingStatus = document.getElementById('txtBookingStatus').value;
    let txtRBD = document.getElementById('txtRBD').value;
    let txtUsageFrom = document.getElementById('txtUsageFrom').value;
    let txtUsageTo = document.getElementById('txtUsageTo').value; 
    let txtUsageAirline = document.getElementById('txtUsageAirline').value; 
    let txtUsageFlightNo = document.getElementById('txtUsageFlightNo').value;
    let UsageDateManuel1 = document.getElementById('UsageDateManuel1').value; 
    let txtFFP = document.getElementById('txtFFP').value;


    //var value = document.getElementById('_xtcdoc').value;

    var tr = document.createElement('tr');
    elmt.appendChild(tr);

    var td0 = document.createElement('td');
    tr.appendChild(td0);
    var tdText0 = document.createTextNode(rw);
    td0.appendChild(tdText0);

    var td1 = document.createElement('td');
    tr.appendChild(td1);
    var tdText1 = document.createTextNode(cboXO );
    td1.appendChild(tdText1);

    var td2 = document.createElement('td');
    tr.appendChild(td2);
    if (txtFrom != "") {
        let Value = txtFrom.trim() + "-" + txtTo.trim();
        var tdText2 = document.createTextNode(Value);
    }
    td2.appendChild(tdText2);

    var td3 = document.createElement('td');
    tr.appendChild(td3);
    var tdText3 = document.createTextNode(txtCarrier);
    td3.appendChild(tdText3);

    var td4 = document.createElement('td');
    tr.appendChild(td4);
    var tdText4 = document.createTextNode(txtFltNo);
    td4.appendChild(tdText4);

    var td5 = document.createElement('td');
    tr.appendChild(td5);
    var tdText5 = document.createTextNode(txtClass);
    td5.appendChild(tdText5);

    var td6 = document.createElement('td');
    tr.appendChild(td6);
    var tdText6 = document.createTextNode(dtpdate);
    td6.appendChild(tdText6);

    var td7 = document.createElement('td');
    tr.appendChild(td7);
    var tdText7 = document.createTextNode(txtTime);
    td7.appendChild(tdText7);

    var td8 = document.createElement('td');
    tr.appendChild(td8);
    var tdText8 = document.createTextNode(txtStatus);
    td8.appendChild(tdText8);

    var td9 = document.createElement('td');
    tr.appendChild(td9);
    var tdText9 = document.createTextNode(txtFareBasis);
    td9.appendChild(tdText9);

    var td10 = document.createElement('td');
    tr.appendChild(td10);
    if (NotValidBefordate == true) {
        var tdText10 = document.createTextNode(dateNotValidBefore);
    }
    td10.appendChild(tdText10);

    var td11 = document.createElement('td');
    tr.appendChild(td11);
    if (dtpIssueDateTo1Noshow == true)
    {
        var tdText11 = document.createTextNode(NotValidAfterdate);
    }
    td11.appendChild(tdText11);

    var td12 = document.createElement('td');
    tr.appendChild(td12);
    var tdText12 = document.createTextNode(txtBagAllowance);
    td12.appendChild(tdText12);

    var td13 = document.createElement('td');
    tr.appendChild(td13);
    var tdText13 = document.createTextNode(txtBookingStatus);
    td13.appendChild(tdText13);

    var td14 = document.createElement('td');
    tr.appendChild(td14);
    var tdText14 = document.createTextNode(txtRBD);
    td14.appendChild(tdText14);

    var td15 = document.createElement('td');
    tr.appendChild(td15);
    if (txtUsageFrom.Text != "")
    {
        let Valueza = txtUsageFrom.trim() + "-" + txtUsageTo.trim();
        var tdText15 = document.createTextNode(Valueza);
    }
    td15.appendChild(tdText15);

    var td16 = document.createElement('td');
    tr.appendChild(td16);
    var tdText16 = document.createTextNode(txtUsageAirline);
    td16.appendChild(tdText16);

    var td17 = document.createElement('td');
    tr.appendChild(td17);
    var tdText17 = document.createTextNode(txtUsageFlightNo);
    td17.appendChild(tdText17);

    
    var td18 = document.createElement('td');
    tr.appendChild(td18);
    if (UsageDateManuel == true){
        var tdText18 = document.createTextNode(UsageDateManuel1);
    }
    td18.appendChild(tdText18);

    var td19 = document.createElement('td');
    tr.appendChild(td19);
    var tdText19 = document.createTextNode(txtFFP);
    td19.appendChild(tdText19);
    /*fin addRow*/
    clearCpn();
}
/*clearCpn*/
function clearCpn() {
    var ladate = new Date();
    let now = ladate.getDate() + "/" + (ladate.getMonth()+1) + "/" + ladate.getFullYear(); 
    $('#cboXO').val("");
    $('#txtFrom').val("");
    $('#txtTo').val("");
    $('#txtCarrier').val("");
    $('#txtFltNo').val("");
    $('#txtClass').val("");
    $('#dtpdate').val(now);
    $('#txtTime').val("");
    $('#txtStatus').val("");
    $('#txtFareBasis').val("");
    $('#dateNotValidBefore').val(now);
    document.getElementById('NotValidBefordate').checked = false;
    $('#NotValidAfterdate').val(now);
    document.getElementById('dtpIssueDateTo1Noshow').checked = false;
    $('#txtStatus').val("");
    $('#txtBookingStatus').val("");
    $('#txtRBD').val("");
    $('#txtUsageAirline').val("");
    $('#txtUsageFlightNo').val("");
    $('#txtUsageFrom').val("");
    $('#txtUsageTo').val(""); 
    $('#txtBagAllowance').val("");
    $('#UsageDateManuel1').val(now);
    document.getElementById('UsageDateManuel').checked = false;
    $('#txtFFP').val("");
}
/* fin clearCpn*/
function newFrmTicketDiscrepancy() {
    document.getElementById("txtDocumentCarrier").readOnly = true;
    document.getElementById("txtDocumentNumber").readOnly = false;
    document.getElementById("txtChkDgt").readOnly = false;
    document.getElementById("txtOrgDest").readOnly = false;
    document.getElementById("txtPNR2").readOnly = false;
    document.getElementById("txtVendorIdentifier").readOnly = false;
    document.getElementById("txtCPUI").readOnly = false;
    document.getElementById("txtReportingSystemIdentifier").readOnly = false;
    document.getElementById("txtTourCode").readOnly = false;
    document.getElementById("txtPassengerNam").readOnly = false;
    document.getElementById("txtCustomerCode").readOnly = false;
    document.getElementById("txtBookingAgentID").readOnly = false;
    document.getElementById("txtFareAmount").readOnly = false;
    document.getElementById("txtFareCurrency").readOnly = false;
    document.getElementById("txtFarePaidAmt").readOnly = false;
    document.getElementById("txtFarePaidCur").readOnly = false;
    document.getElementById("txtFareComponent").readOnly = false;
    document.getElementById("txtEndorsementArea").readOnly = false;
    document.getElementById("txtFcmi").readOnly = false;
    document.getElementById("txtIssuedInExchangeFor").readOnly = false;
    $('#txttransactioncode').val("TKTM");
    document.getElementById("txtTotalamtcur").readOnly = false;
    document.getElementById("dbgCPNlist").readOnly = false;
    document.getElementById("dgother").readOnly = false;
    document.getElementById("dgpayment").readOnly = false;
    document.getElementById("dgOriDoc").readOnly = false;
    clearall();
    clearCpn();
    /*TRUNCATE*/
    let url = symphony + "/Process/TRUNCATE";
    $.ajax({
        type: "POST",
        url: url,
        data: {},
        success: function (data) {
        },
    });
    /*fin TRUNCATE*/
    document.getElementById("btnEdit").disabled = true;
    document.getElementById("btnSave").disabled = false;
    document.getElementById("btnnew").disabled = true;
    document.getElementById("btnClear").disabled = false;
}
function clearall() {
    var ladate = new Date();
    let now = ladate.getDate() + "/" + (ladate.getMonth() + 1) + "/" + ladate.getFullYear();
    $('#FromManuelTicketScreen').val(now);
    $('#txtDocumentCarrier').val("");
    $('#txtDocumentNumber').val("");
    $('#txtChkDgt').val("");
    $('#txtOrgDest').val("");
    $('#txtPNR2').val("");
    $('#txtIssuedInExchangeFor').val("");
    $('#txtVendorIdentifier').val("");
    $('#txtCPUI').val("");
    $('#txtReportingSystemIdentifier').val("");
    $('#txtTourCode').val("");
    $('#txtPassengerNam').val("");
    $('#txtCustomerCode').val("");
    $('#txtBookingAgentID').val("");
    $('#txtEndorsementArea').val("");
    $('#txtTicketingAgentId').val("");
    $('#txtFareAmount').val("");
    $('#txtFareCurrency').val("");
    $('#txtFarePaidAmt').val("");
    $('#txtFarePaidCur').val("");
    $('#txtFareComponent').val("");
    $('#txttransactioncode').val("TKTM");
    $('#txtFcmi').val("");
    $('#txtTotalamt').val("");
    $('#txtTotalamtcur').val("");
    $('#_xtcdoc').val("");
    let url = symphony + "/Process/clearall";
    $.ajax({
        type: "POST",
        url: url,
        data: {  },
        success: function (data) {
            $("#divdgother").html($(data).filter('#newdivdgother'));
            $("#divdgpayment").html($(data).filter('#newdivdgpayment'));
            $("#divdbgCPNlist").html($(data).filter('#newdivdbgCPNlist')); 
            $("#divdgOriDoc").html($(data).filter('#newdivdgOriDoc'));
        },
    });
    document.getElementById("btnnew").disabled = false;
    document.getElementById("btnEdit").disabled = true;
    document.getElementById("btnClear").disabled = false;
    document.getElementById("btnSave").disabled = true;
}
function EditFrmTicketDiscrepancy() {
    let _xtcdoc = $('#_xtcdoc').val();
    document.getElementById("txtDocumentCarrier").readOnly = true;
    document.getElementById("txtDocumentNumber").readOnly = false;
    document.getElementById("txtChkDgt").readOnly = false;
    document.getElementById("txtOrgDest").readOnly = false;
    document.getElementById("txtPNR2").readOnly = false;
    document.getElementById("txtVendorIdentifier").readOnly = false;
    document.getElementById("txtCPUI").readOnly = false;
    document.getElementById("txtReportingSystemIdentifier").readOnly = false;
    document.getElementById("txtTourCode").readOnly = false;
    document.getElementById("txtPassengerNam").readOnly = false;
    document.getElementById("txtCustomerCode").readOnly = false;
    document.getElementById("txtBookingAgentID").readOnly = false;
    document.getElementById("txtFareAmount").readOnly = false;
    document.getElementById("txtFareCurrency").readOnly = false;
    document.getElementById("txtFarePaidAmt").readOnly = false;
    document.getElementById("txtFarePaidCur").readOnly = false;
    document.getElementById("txtFareComponent").readOnly = false;
    document.getElementById("txtEndorsementArea").readOnly = false;
    document.getElementById("txtFcmi").readOnly = false;
    document.getElementById("txtIssuedInExchangeFor").readOnly = false;
    $('#txttransactioncode').val("TKTM");
    document.getElementById("txtTotalamtcur").readOnly = false;
    document.getElementById("dbgCPNlist").readOnly = false;
    document.getElementById("dgother").readOnly = false;
    document.getElementById("dgpayment").readOnly = false;
    document.getElementById("dgOriDoc").readOnly = false;
    /*DELETEditFrmTicketDiscrepancy*/
    let url = symphony + "/Process/DELETEditFrmTicketDiscrepancy";
    $.ajax({
        type: "POST",
        url: url,
        data: { _xtcdoc: _xtcdoc },
        success: function (data) {
        },
    });
    /*fin DELETEditFrmTicketDiscrepancy*/
    document.getElementById("btnnew").disabled = true;
    document.getElementById("btnClear").disabled = true;
    document.getElementById("btnEdit").disabled = true;
    document.getElementById("btnSave").disabled = false;
}
function btnClear_ClickFrmTicketDiscrepancy() {
    clearall();
}
function SaveFrmTicketDiscrepancy() {
    let txtDocumentCarrier = $('#txtDocumentCarrier').val();
    let txtDocumentNumber = $('#txtDocumentNumber').val();
    let txtOrgDest = $('#txtOrgDest').val();
    let txtFareComponent = $('#txtFareComponent').val();
    let txtFareCurrency = $('#txtFareCurrency').val();
    let txtFareAmount = $('#txtFareAmount').val();
    let txtPassengerNam = $('#txtPassengerNam').val();
    let txtTotalamtcur = $('#txtTotalamtcur').val();
    let txtTotalamt = $('#txtTotalamt').val();
    let txtFcmi = $('#txtFcmi').val();
    try
    {
        if (txtDocumentCarrier == "" && txtDocumentNumber == "")
        {
            $('#alertModal').modal('show');
            $('#msgalert').text('Carrier and Document Number is compulsory');
        }
        if (txtOrgDest == "")
        {
            $('#alertModal').modal('show');
            $('#msgalert').text('True Origin Destination City Codes is compulsory');
        }
        if (txtFareComponent == "")
        {
            $('#alertModal').modal('show');
            $('#msgalert').text('Fare Calculation Area is compulsory');
        }
        if (txtFareCurrency == "")
        {
            $('#alertModal').modal('show');
            $('#msgalert').text('Fare Currency is compulsory');
        }
        if (txtFareAmount == "")
        {
            $('#alertModal').modal('show');
            $('#msgalert').text('Fare Amount is compulsory');
        }
        if (txtPassengerNam == "")
        {
            $('#alertModal').modal('show');
            $('#msgalert').text('Passenger Name is compulsory');
        }
        if (txtTotalamtcur == "")
        {
            $('#alertModal').modal('show');
            $('#msgalert').text('Total Currency is compulsory');
        }
        if (txtTotalamt == "")
        {
            $('#alertModal').modal('show');
            $('#msgalert').text('Total Amount is compulsory');
        }
        if (txtFcmi == "")
        {
            $('#alertModal').modal('show');
            $('#msgalert').text('FCMI is compulsory');
        }
        var dgpaymentRowCount = $('#dgpayment tr').length;
        if (dgpaymentRowCount > 1) {
            var xi;
            var dgpayment = document.getElementById('dgpayment');
            for (xi = 1; xi < dgpaymentRowCount; xi++) {
                if (dgpayment.rows[xi].cells[1].innerHTML == "") {
                    $('#alertModal').modal('show');
                    $('#msgalert').text('FOP is compulsory');
                }
            }
        }
        let _Cur = $('#txtTotalamtcur').val();
        ManualHeader();//+ManualRelatedDocInf
        var dbgCPNlistRowCount = $('#dbgCPNlist tr').length;
        var dbgCPNlist = document.getElementById('dbgCPNlist');
        for (var ii = 1; ii < dbgCPNlistRowCount; ii++)
        {
            var a;
            if (dbgCPNlist.rows[ii].cells[0].innerHTML != null && !isNullOrWhitespace(dbgCPNlist.rows[ii].cells[0].innerHTML) && dbgCPNlist.rows[ii].cells[0].innerHTML != "") 
            {
                a = dbgCPNlist.rows[ii].cells[0].innerHTML;
            }
            var b;
            if (dbgCPNlist.rows[ii].cells[1].innerHTML != null && !isNullOrWhitespace(dbgCPNlist.rows[ii].cells[1].innerHTML) && dbgCPNlist.rows[ii].cells[1].innerHTML != "") 
            {
                b = dbgCPNlist.rows[ii].cells[1].innerHTML;
            }
            var c;
            if (dbgCPNlist.rows[ii].cells[2].innerHTML != null && !isNullOrWhitespace(dbgCPNlist.rows[ii].cells[2].innerHTML) && dbgCPNlist.rows[ii].cells[2].innerHTML != "") 
            {
                c = dbgCPNlist.rows[ii].cells[2].innerHTML;
            }
            var d;
            if (dbgCPNlist.rows[ii].cells[3].innerHTML != null && !isNullOrWhitespace(dbgCPNlist.rows[ii].cells[3].innerHTML) && dbgCPNlist.rows[ii].cells[3].innerHTML != "") 
            {
                d = dbgCPNlist.rows[ii].cells[3].innerHTML;
            }
            var ee;
            if (dbgCPNlist.rows[ii].cells[4].innerHTML != null && !isNullOrWhitespace(dbgCPNlist.rows[ii].cells[4].innerHTML) && dbgCPNlist.rows[ii].cells[4].innerHTML != "") 
            {
                ee= dbgCPNlist.rows[ii].cells[4].innerHTML;
            }
            var f;
            if (dbgCPNlist.rows[ii].cells[5].innerHTML != null && !isNullOrWhitespace(dbgCPNlist.rows[ii].cells[5].innerHTML) && dbgCPNlist.rows[ii].cells[5].innerHTML != "") 
            {
                f= dbgCPNlist.rows[ii].cells[5].innerHTML;
            }
            var g;
            if (dbgCPNlist.rows[ii].cells[6].innerHTML != null && !isNullOrWhitespace(dbgCPNlist.rows[ii].cells[6].innerHTML) && dbgCPNlist.rows[ii].cells[6].innerHTML != "") 
            {
                g= dbgCPNlist.rows[ii].cells[6].innerHTML;
            }
            var h;
            if (dbgCPNlist.rows[ii].cells[7].innerHTML != null && !isNullOrWhitespace(dbgCPNlist.rows[ii].cells[7].innerHTML) && dbgCPNlist.rows[ii].cells[7].innerHTML != "") 
            {
                h= dbgCPNlist.rows[ii].cells[7].innerHTML;
            }
            var i;
            if (dbgCPNlist.rows[ii].cells[8].innerHTML != null && !isNullOrWhitespace(dbgCPNlist.rows[ii].cells[8].innerHTML) && dbgCPNlist.rows[ii].cells[8].innerHTML != "") 
            {
                i= dbgCPNlist.rows[ii].cells[8].innerHTML;
            }
            var j;
            if (dbgCPNlist.rows[ii].cells[9].innerHTML != null && !isNullOrWhitespace(dbgCPNlist.rows[ii].cells[9].innerHTML) && dbgCPNlist.rows[ii].cells[9].innerHTML != "") 
            {
                j= dbgCPNlist.rows[ii].cells[9].innerHTML;
            }
            var k;
            if (dbgCPNlist.rows[ii].cells[10].innerHTML != null && !isNullOrWhitespace(dbgCPNlist.rows[ii].cells[10].innerHTML) && dbgCPNlist.rows[ii].cells[10].innerHTML != "") 
            {
                k= dbgCPNlist.rows[ii].cells[10].innerHTML;
            }
            var l;
            if (dbgCPNlist.rows[ii].cells[11].innerHTML != null && !isNullOrWhitespace(dbgCPNlist.rows[ii].cells[11].innerHTML) && dbgCPNlist.rows[ii].cells[11].innerHTML != "") 
            {
                l= dbgCPNlist.rows[ii].cells[11].innerHTML;
            }
            var m;
            if (dbgCPNlist.rows[ii].cells[12].innerHTML != null && !isNullOrWhitespace(dbgCPNlist.rows[ii].cells[12].innerHTML) && dbgCPNlist.rows[ii].cells[12].innerHTML != "") 
            {
                m= dbgCPNlist.rows[ii].cells[12].innerHTML;
            }
            var n;
            if (dbgCPNlist.rows[ii].cells[13].innerHTML != null && !isNullOrWhitespace(dbgCPNlist.rows[ii].cells[13].innerHTML) && dbgCPNlist.rows[ii].cells[13].innerHTML != "") 
            {
                n= dbgCPNlist.rows[ii].cells[13].innerHTML;
            }
            var o;
            if (dbgCPNlist.rows[ii].cells[14].innerHTML != null && !isNullOrWhitespace(dbgCPNlist.rows[ii].cells[14].innerHTML) && dbgCPNlist.rows[ii].cells[14].innerHTML != "") 
            {
                o= dbgCPNlist.rows[ii].cells[14].innerHTML;
            }
            var p;
            if (dbgCPNlist.rows[ii].cells[15].innerHTML != null && !isNullOrWhitespace(dbgCPNlist.rows[ii].cells[15].innerHTML) && dbgCPNlist.rows[ii].cells[15].innerHTML != "") 
            {
                p= dbgCPNlist.rows[ii].cells[15].innerHTML;
            }
            var q;
            if (dbgCPNlist.rows[ii].cells[16].innerHTML != null && !isNullOrWhitespace(dbgCPNlist.rows[ii].cells[16].innerHTML) && dbgCPNlist.rows[ii].cells[16].innerHTML != "") 
            {
                q= dbgCPNlist.rows[ii].cells[16].innerHTML;
            }
            var r;
            if (dbgCPNlist.rows[ii].cells[17].innerHTML != null && !isNullOrWhitespace(dbgCPNlist.rows[ii].cells[17].innerHTML) && dbgCPNlist.rows[ii].cells[17].innerHTML != "") 
            {
                r= dbgCPNlist.rows[ii].cells[17].innerHTML;
            }
            var s;
            if (dbgCPNlist.rows[ii].cells[18].innerHTML != null && !isNullOrWhitespace(dbgCPNlist.rows[ii].cells[18].innerHTML) && dbgCPNlist.rows[ii].cells[18].innerHTML != "") 
            {
                s= dbgCPNlist.rows[ii].cells[18].innerHTML;
            }
            var t;
            if (dbgCPNlist.rows[ii].cells[19].innerHTML != null && !isNullOrWhitespace(dbgCPNlist.rows[ii].cells[19].innerHTML) && dbgCPNlist.rows[ii].cells[19].innerHTML != "") 
            {
                t= dbgCPNlist.rows[ii].cells[19].innerHTML;
            }
            var u = "";
            var _Dox = "";
            _Dox = txtDocumentCarrier.concat(txtDocumentNumber);
            u = _Dox;
            /*cpnSave(a, b, c, d, ee, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v);*/
            let url = symphony + "/Process/cpnSave";
            $.ajax({
                type: "POST",
                url: url,
                data: { a:a,b:b,c:c,d:d,ee:ee,f:f,g:g,h:h,i:i,j:j,k:k,l:l,m:m,n:n,o:o,p:p,q:q,r:r,s:s,t:t,u:u },
                beforeSend: function () {
                },
                success: function (data) {
                },
                complete: function (data) {
                },
                error: function () {
                },
            });
            /*fin cpnSave*/
        }
    	/*Resqeuencing*/
        var dgotherRowCount = $('#dgother tr').length;
        var dgpaymentRowCount = $('#dgpayment tr').length;
        var dgpaymentRowCount = $('#dgpayment tr').length;
        var dgother = document.getElementById('dgother');
        var seqtaxsurcharge = 0;
        var seqcomm3 = 0;
        var seqtaxcomm1 = 0;
        var taxsurcharge = 100;
        var comm3 = 200;
        var taxcomm1 = 300;
        for (var i = 1; i < dgotherRowCount; i++) {
        	var item = dgother.rows[i].cells[0].innerHTML;
        	switch (item) {
        		case "Tax,Surcharges":
        			dgother.rows[i].cells[4].innerHTML = taxsurcharge + seqtaxsurcharge;
        			seqtaxsurcharge++;
        			break;
        		case "Commission3":
        			dgother.rows[i].cells[4].innerHTML = comm3 + seqcomm3;
        			seqcomm3++;
        			break;
        		case "TaxCommission1":
        			dgother.rows[i].cells[4].innerHTML = taxcomm1 + seqtaxcomm1;
        			seqtaxcomm1++;
        			break;
        	}
        }
    	/*fin Resqeuencing*/
    	/*ambany Resqeuencing*/
        let url = symphony + "/Process/ResqeuencingcheckifexistOA";
        $.ajax({
        	type: "POST",
        	url: url,
        	data: { dgotherRowCount: dgotherRowCount, dgpaymentRowCount: dgpaymentRowCount },
        	beforeSend: function () {
        	},
        	success: function (data) {
        	},
        	complete: function (data) {
        	},
        	error: function () {
        	},
        });
    	/*Saveotheramt*/
        if (dgotherRowCount > 1) {
        	for (var vi = 1; vi < dgotherRowCount; vi++) {
        		var dgother = document.getElementById('dgother');

        		let t0 = dgother.rows[vi].cells[0].innerHTML;
        		let t1 = dgother.rows[vi].cells[1].innerHTML;
        		let t2 = dgother.rows[vi].cells[2].innerHTML;
        		let t3 = dgother.rows[vi].cells[3].innerHTML;
        		//let t4 = dgother.rows[vi].cells[4].innerHTML;
        		let url = symphony + "/Process/Saveotheramt";
        		$.ajax({
        			type: "POST",
        			url: url,
        			//data: { txtDocumentCarrier: txtDocumentCarrier, txtDocumentNumber: txtDocumentNumber, t0: t0, t1: t1, t2: t2, t3: t3, t4: t4 },
        			data: { txtDocumentCarrier: txtDocumentCarrier, txtDocumentNumber: txtDocumentNumber, t0: t0, t1: t1, t2: t2, t3: t3 },
        			beforeSend: function () {
        			},
        			success: function (data) {
        			},
        			complete: function (data) {
        			},
        			error: function () {
        			},
        		});
        	}
        }
    	/*fin Saveotheramt*/
    	/*SavePayment*/
        if (dgpaymentRowCount > 1) {
        	var seqaccseq =0;
        	var dgOriDoc = document.getElementById('dgOriDoc');
        	var dgOriDocRowCount = $('#dgOriDoc tr').length;

        	var dgpayment = document.getElementById('dgpayment');
        	var dgpaymentRowCount = $('#dgpayment tr').length;

        	for (var ii = 1; ii < dgOriDocRowCount; ii++) {
        	    let dgOriDoc0 = dgOriDoc.rows[ii].cells[0].innerHTML;
        	    if(dgOriDoc0 != ""){
        		    let url = symphony + "/Process/SavePayment";
        		    $.ajax({
        			    type: "POST",
        			    url: url,
        			    data: { txtDocumentCarrier: txtDocumentCarrier, txtDocumentNumber: txtDocumentNumber, dgOriDocRowCount: dgOriDocRowCount, seqaccseq: seqaccseq, dgOriDoc0: dgOriDoc0 },
        			    beforeSend: function () {
        			    },
        			    success: function (data) {
        			    },
        			    error: function () {
        			    },
        		    });
        		    seqaccseq++;
        	    }
        	}
        	for (var ii = 1; ii < dgpaymentRowCount; ii++) {
        		let dgpayment0 = dgpayment.rows[ii].cells[0].innerHTML;
        		let dgpayment1 = dgpayment.rows[ii].cells[1].innerHTML;
        		let dgpayment2 = dgpayment.rows[ii].cells[2].innerHTML;
        		let url = symphony + "/Process/SavePaymentdgpayment";
        		$.ajax({
        			type: "POST",
        			url: url,
        			data: { txtDocumentCarrier: txtDocumentCarrier, txtDocumentNumber: txtDocumentNumber, dgpayment0: dgpayment0, dgpayment1: dgpayment1, dgpayment2: dgpayment2 },
        			success: function (data) {
        			},
        			error: function (data) {
        			},
        		});
        	}
        }
    	/*fin SavePayment*/
    	/*fin ambany Resqeuencing*/
    	try
    	{
    		/*SP_ManualEntryUpdate*/
    		let url = symphony + "/Process/SP_ManualEntryUpdate";
    		$.ajax({
    			type: "POST",
    			url: url,
    			data: {  },
    			success: function (data) {
    			},
    			error: function () {
    			},
    		});
    		/*SP_ManualEntryUpdate*/
    		try
    		{
    			/*ManualEntryLog*/
    			let url = symphony + "/Process/ManualEntryLog";
    			$.ajax({
    				type: "POST",
    				url: url,
    				data: {  },
    				success: function (data) {
    				},
    				error: function () {
    				},
    			});
    			/*fin ManualEntryLog*/
    		}
        	catch (err) {
        		$('#alertModal').modal('show');
        		$('#msgalert').text(err.message);
        	}
			$('#alertModal').modal('show');
			$('#msgalert').text("Record Save Sucessfully.");
		}
		catch (ex) {
			$('#alertModal').modal('show');
			$('#msgalert').text("Error:" + ex.Message);
		}

	}
	catch (error) {
		$('#alertModal').modal('show');
		$('#msgalert').text("Error:" + error.Message);
	}
	document.getElementById("btnnew").disabled = true;
	document.getElementById("btnEdit").disabled = true;
	document.getElementById("btnClear").disabled = false;
	document.getElementById("btnSave").disabled = false;

}
function OrgDetails(value) {
    let url = symphony + "/Process/OrgDetails";
    $.ajax({
        async: true,
        type: "POST",
        url: url,
        data: { DocNum: value },
        success: function (data) {
            $("#OrgDetails").html(data);
        },
        error: function () {
        }
    });
}
function ManualHeader()
{
    let txtIssuedInExchangeFor = $('#txtIssuedInExchangeFor').val();
    let txtDocumentCarrier = $('#txtDocumentCarrier').val();
    let txtDocumentNumber = $('#txtDocumentNumber').val(); 
    let txtChkDgt = $('#txtChkDgt').val(); 
    let txtOrgDest = $('#txtOrgDest').val(); 
    let txtFareComponent = $('#txtFareComponent').val(); 
    let txtFareCurrency = $('#txtFareCurrency').val();
    let txtFareAmount = $('#txtFareAmount').val();
    let txtEndorsementArea = $('#txtEndorsementArea').val();
    let txtPNR2 = $('#txtPNR2').val();
    let txtPassengerNam = $('#txtPassengerNam').val();
    let dtpIssDoc = $('#FromManuelTicketScreen').val();
    let txtBookingAgentID = $('#txtBookingAgentID').val();
    let txtTourCode = $('#txtTourCode').val();
    let txtFcmi = $('#txtFcmi').val();
    let txtTotalamtcur = $('#txtTotalamtcur').val();
    let txtFarePaidAmt = $('#txtFarePaidAmt').val();
    let txtTotalamt = $('#txtTotalamt').val();
    let txtVendorIdentifier = $('#txtVendorIdentifier').val();
    let txttransactioncode = $('#txttransactioncode').val();
    let txtReportingSystemIdentifier = $('#txtReportingSystemIdentifier').val();
    let txtTicketingAgentId = $('#txtTicketingAgentId').val(); 
    let txtCPUI = $('#txtCPUI').val();

    var list = [];
    list[0] = txtDocumentCarrier;
    list[1] = txtDocumentNumber;

    let _Doc = list.join('');
    if (txtDocumentCarrier == "" && txtDocumentNumber == "") {
        $('#alertModal').modal('show');
        $('#msgalert').text('Carrier and Document Number is compulsory');
    }
    if (txtOrgDest == "") {
        $('#alertModal').modal('show');
        $('#msgalert').text('True Origin Destination City Codes is compulsory');
    }
    if (txtFareComponent == "") {
        $('#alertModal').modal('show');
        $('#msgalert').text('Fare Calculation Area is compulsory');
    }
    if (txtFareCurrency == "") {
        $('#alertModal').modal('show');
        $('#msgalert').text('Fare Currency is compulsory');
    }
    if (txtFareAmount == "") {
        $('#alertModal').modal('show');
        $('#msgalert').text('Fare Amount is compulsory');
    }
    if (txtPassengerNam == "") {
        $('#alertModal').modal('show');
        $('#msgalert').text('Passenger Name is compulsory');
    }
    if (txtTotalamtcur == "") {
        $('#alertModal').modal('show');
        $('#msgalert').text('Total Currency is compulsory');
    }
    if (txtTotalamt == "") {
        $('#alertModal').modal('show');
        $('#msgalert').text('Total Amount is compulsory');
    }
    if (txttransactioncode == "") {
        $('#alertModal').modal('show');
        $('#msgalert').text('Transaction Code is compulsory');
    }
    let url = symphony + "/Process/ManualHeader";
    $.ajax({
        type: "POST",
        url: url,
        data: { txtDocumentCarrier: txtDocumentCarrier, txtDocumentNumber: txtDocumentNumber, _Doc: _Doc, txtChkDgt: txtChkDgt, txtOrgDest: txtOrgDest, txtFareComponent: txtFareComponent, txtFareCurrency: txtFareCurrency, txtFareAmount: txtFareAmount, txtEndorsementArea: txtEndorsementArea, txtPNR2: txtPNR2, txtPassengerNam: txtPassengerNam, dtpIssDoc: dtpIssDoc, txtBookingAgentID: txtBookingAgentID, txtTourCode: txtTourCode, txtFcmi: txtFcmi, txtTotalamtcur: txtTotalamtcur, txtFarePaidAmt: txtFarePaidAmt, txtTotalamt: txtTotalamt, txtVendorIdentifier: txtVendorIdentifier, txttransactioncode: txttransactioncode, txtReportingSystemIdentifier: txtReportingSystemIdentifier, txtTicketingAgentId: txtTicketingAgentId, txtIssuedInExchangeFor: txtIssuedInExchangeFor },
        success: function (data) {
        },
        error: function () {
        },
    });
    let url1 = symphony + "/Process/ManualRelatedDocInf";
    $.ajax({
        type: "POST",
        url: url1,
        data: { txtDocumentCarrier: txtDocumentCarrier, txtDocumentNumber: txtDocumentNumber, txtChkDgt: txtChkDgt, txtCPUI: txtCPUI, txttransactioncode: txttransactioncode },
        success: function (data) {
        },
        error: function () {
        },
    });
}
/*fiftedcoupons*/
function btnClear_Clickfiftedcoupons() {
    $('#txtFlightNo').val("");
    var ladate = new Date();
    let now = ladate.getDate() + "/" + (ladate.getMonth() + 1) + "/" + ladate.getFullYear();
    $('#dateFromlifted').val(now);
    $('#dateTolifted').val(now);

    let url = symphony + "/Process/btnClear_Clickfiftedcoupons";
    $.ajax({
        type: "POST",
        url: url,
        data: {},
        success: function (data) {
            $("#divdgtype").html($(data).filter('#newdivdgtype'));
            $("#divdgOAL").html($(data).filter('#newdivdgOAL'));
            $("#divdgOWN").html($(data).filter('#newdivdgOWN'));
            $("#divdgdetails").html($(data).filter('#newdivdgdetails'));
            $("#divdgtotal").html($(data).filter('#newdivdgtotal'));
            $("#divdtpFrom").html($(data).filter('#newdivdtpFrom'));
            $("#divdtpTo").html($(data).filter('#newdivdtpTo'));
        },
    });
    $('#txtFinalTotalLift').val("");
    $('#txtFlightNo').focus();
}
function txtFlightNo_TextChanged() {
    document.getElementById("txtFlightNo").style.textTransform = "capitalize";
}
function btnSearch_Clickfiftedcoupons() {
    let txtFlightNo = $('#txtFlightNo').val();
    let dateFromlifted = $('#dateFromlifted').val();
    let dateTolifted = $('#dateTolifted').val();

    let url = symphony + "/Process/btnSearch_Clickfiftedcoupons";
    $.ajax({
        type: "POST",
        url: url,
        data: { txtFlightNo: txtFlightNo, dateFromlifted: dateFromlifted, dateTolifted: dateTolifted },
        beforeSend: function () {
            //btnClear_Clickfiftedcoupons();
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#divdgOAL").html($(data).filter('#newdivdgOAL'));
            $("#divdgOWN").html($(data).filter('#newdivdgOWN'));
            $("#divdgtype").html($(data).filter('#newdivdgtype'));
            $("#divdgtotal").html($(data).filter('#newdivdgtotal'));
        },
        complete: function () {
            let tempstxtFinalTotalLift = $('#tempstxtFinalTotalLift').val();
            $('#txtFinalTotalLift').val(tempstxtFinalTotalLift);
            $('.ajax-loader').css("visibility", "hidden");
        },
        error: function () {
        }
    });
}
function dgOWN_CellContentDoubleClick(Airline, UsageType) {
    let dateFromlifted = $('#dateFromlifted').val();
    let dateTolifted = $('#dateTolifted').val();
    let url = symphony + "/Process/dgOWN_CellContentDoubleClick";
    $.ajax({
        type: "POST",
        url: url,
        data: { Airline: Airline, UsageType: UsageType, dateFromlifted: dateFromlifted, dateTolifted: dateTolifted },
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            //$(".country").toggle();
            $("#divdgdetails").html($(data).filter('#newdivdgdetails'));
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
        },
        error: function () {
        }
    });
}
function dgOAL_CellContentDoubleClick(Airline, UsageType) {
    let dateFromlifted = $('#dateFromlifted').val();
    let dateTolifted = $('#dateTolifted').val();
    let url = symphony + "/Process/dgOAL_CellContentDoubleClick";
    $.ajax({
        type: "POST",
        url: url,
        data: { Airline: Airline, UsageType: UsageType, dateFromlifted: dateFromlifted, dateTolifted: dateTolifted },
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            //$(".country").toggle();
            $("#divdgdetails").html($(data).filter('#newdivdgdetails'));
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
        },
        error: function () {
        }
    });
}
function button2_Clickchehconnection() {
    let url = symphony + "/Process/button2_Clickchehconnection";
    $.ajax({
        type: "POST",
        url: url,
        data: {},
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#lblLastUpdated").html($(data).filter('#newlblLastUpdated'));
            $("#lblCONNECTIONSTATUS").html($(data).filter('#newlblCONNECTIONSTATUS'));
            $("#divdataGridView1").html($(data).filter('#newdivdataGridView1'));
        },
        complete: function () {
            getcsvbutton2_Clickchehconnection();
            $('.ajax-loader').css("visibility", "hidden");
            /*if (identifianttest == "0") {
                alert('0');
                $("#lblCONNECTIONSTATUS").html($(data).filter('#newlblCONNECTIONSTATUS'));
            }*/
        },
        error: function () {
        }
    });
}
function getcsvbutton2_Clickchehconnection() {
    $(document).ready(function () {
        $('#dataGridView1').DataTable({
            "pageLength": 50,
            dom: 'Bfrtip',
            buttons: [
				 'csv'
            ]
        });
    });
}
/*fin fiftedcoupons*/   
function button1_Clickchehconnection() {
    let url = symphony + "/Process/button1_Clickchehconnection";
    $.ajax({
        type: "POST",
        url: url,
        data: {},
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#lblLastUpdated").html($(data).filter('#newlblLastUpdated'));
            $("#lblCONNECTIONSTATUS").html($(data).filter('#newlblCONNECTIONSTATUS'));
            $("#divdataGridView1").html($(data).filter('#newdivdataGridView1'));

            $('#alertModal').modal('show');
            $('#msgalert').text('XML Bankers rate imported successfully ", "DB Update');

        },
        complete: function () {
            getcsvbutton2_Clickchehconnection();
            $('.ajax-loader').css("visibility", "hidden");
        },
        error: function () {
            $('#alertModal').modal('show');
            $('#msgalert').text('La balise de début \'base\' sur la ligne 51 à la position 2 ne correspond pas à la balise de fin de \'head\'. Ligne 53, position 3.');
        }
    });
}
function isNullOrWhitespace( input ) {

    if (typeof input === 'undefined' || input == null) return true;

    return input.replace(/\s/g, '').length < 1;
}
function button3_Clickinterlincorrespondence() {
    let comboBox3SelectedIndex = document.getElementById("comboBox3").selectedIndex;
    if (comboBox3SelectedIndex != 0)
    {
        /*Getdata*/
        var comboBox3 = $('#comboBox3').val();
        let url = symphony + "/Process/button3_Clickinterlincorrespondence";
        $.ajax({
            type: "POST",
            url: url,
            data: { comboBox3index: comboBox3SelectedIndex, comboBox3: comboBox3 },
            beforeSend: function () {
            },
            success: function (data) {
                $("#CorEntri").html($(data).filter('#newCorEntri'));
            },
            complete: function () {
            },
            error: function () {
            }
        });
        /*fin Getdata*/
    }
}
function dataGridView1_CellClickcorrespondenceinterline(RowIndex, dataGridView1RowsCount, value0, value1, value2, value3, value4, value5) {
    if (dataGridView1RowsCount != 0)
    {
        let url = symphony + "/Process/dataGridView1_CellClickcorrespondenceinterline";
        $.ajax({
            type: "POST",
            url: url,
            data: { RowIndex: RowIndex, dataGridView1RowsCount: dataGridView1RowsCount, value0: value0, value1: value1, value2: value2, value3: value3, value4: value4, value5: value5 },
            beforeSend: function () {
            },
            success: function (data) {
                document.getElementById("label20").innerHTML = value1;
                document.getElementById("label19").innerHTML = value2;
                /*Getcarrier()*/
                let url1 = symphony + "/Process/Getcarrier";
                $.ajax({
                    type: "POST",
                    url: url1,
                    data: { value1: value1 },
                    success: function (data) {
                        document.getElementById("label18").innerHTML = data;
                    },
                });
                /*fin Getcarrier()*/
                $("#temps").html($(data).filter('#newtemps'));
            },
            complete: function () {
                $('#author').val($('#txtAuthor').val());
                $('#SubmissionDatedCorrespondance').val($('#dateTimePicker1').val());
                $('#LimDate').val($('#txtDateLimit').val());
                $('#ref').val($('#txtDocRef').val());
                $('#invoice').val($('#txtInvoiceNO').val());
                $('#DocAmt').val($('#txtDocAmt').val());
                $('#AmtAcc').val($('#txtAccepted').val());
                $('#AmtDisp').val($('#txtDisputed').val());
                $('#ResolvedDateCorrespondance').val($('#dateTimePicker2').val());
                $('#msgContent').val($('#richTextBox1').val());
                document.getElementById("comboBox1").selectedIndex = $('#comboBox1SelectedIndex').val();
                document.getElementById("comboBox2").selectedIndex = $('#comboBox2SelectedIndex').val();
            },
            error: function () {
            }
        });
    }
}
function button4_Clickwebinterfacecorrespondence() {
    var windowObjectReference;
    windowObjectReference = window.open(
    '',
    "DescriptiveWindowName",
    "resizable,scrollbars,status"
  );
}
function button1_Clickinterlinecorrespondence() {
    var DocumentNumber = $('#DocumentNumber').val();
    var CouponNo = $('#CouponNo').val();
    var ChargeCode = $('#ChargeCode').val();
    var CorrespondenceNo = $('#CorrespondenceNo').val();
    
    var txtDocAmt = document.getElementById("DocAmt").value;
    var txtAccepted = document.getElementById("AmtAcc").value;
    var txtDisputed = document.getElementById("AmtDisp").value;


    var txtAuthor = $('#author').val();
    var comboBox1 = $('#comboBox1').val();
    var txtAuthor = $('#author').val();
    var dateTimePicker1 = $('#SubmissionDatedCorrespondance').val();
    var txtDateLimit = $('#LimDate').val();
    var txtDocRef = $('#ref').val();
    var txtInvoiceNO = $('#invoice').val();
    var richTextBox1 = $('#msgContent').val();
    var dateTimePicker2 = $('#ResolvedDateCorrespondance').val();
    var comboBox2SelectedIndex = document.getElementById("comboBox2").selectedIndex;
    var comboBox3index = $("select[name='comboBox3'] option:selected").index();
    var comboBox3 = $('#comboBox3').val();
    if (txtDocAmt == "" || txtAccepted == "" || txtDisputed == "")
    {
        alert("Invaild Amount,Confirm  amounts for new correspondence entry");
    } 
    if (txtAuthor == "Author" || txtAuthor.trim() == "")
    {
        alert("Invaild Author name,Confirm  Author name for new correspondence entry");
        document.getElementById("author").focus();
    }
    else
    {
        let url = symphony + "/Process/button1_Clickinterlinecorrespondence";
        $.ajax({
            type: "POST",
            url: url,
            data: { CorrespondenceNo:CorrespondenceNo,ChargeCode: ChargeCode, CouponNo: CouponNo, DocumentNumber: DocumentNumber, comboBox1: comboBox1, txtAuthor: txtAuthor, dateTimePicker1: dateTimePicker1, txtDateLimit: txtDateLimit, txtDocRef: txtDocRef, txtInvoiceNO: txtInvoiceNO, txtDocAmt: txtDocAmt, txtAccepted: txtAccepted, txtDisputed: txtDisputed, richTextBox1: richTextBox1, dateTimePicker2: dateTimePicker2, comboBox2SelectedIndex: comboBox2SelectedIndex },
            beforeSend: function () {
            },
            success: function (data) {
                let url0 = symphony + "/Process/divselectcorrespondance";
                $.ajax({
                    type: "POST",
                    url: url0,
                    data: {},
                    success: function (data) {
                        $("#divselectcorrespondance").html($(data).filter('#newdivselectcorrespondance'));
                        /*Getdata*/
                        var comboBox3index = document.getElementById("comboBox3").selectedIndex;
                        var comboBox3 = $('#comboBox3').val();
                        if (comboBox3index != 0) {
                            let url1 = symphony + "/Process/button3_Clickinterlincorrespondence";
                            $.ajax({
                                type: "POST",
                                url: url1,
                                data: { comboBox3index: comboBox3index, comboBox3: comboBox3 },
                                beforeSend: function () {
                                },
                                success: function (data) {
                                    $("#CorEntri").html($(data).filter('#newCorEntri'));
                                },
                                complete: function () {
                                },
                                error: function () {
                                }
                            });
                        }
                        /*fin Getdata*/
                    },
                });
            },
        });
    }
}
function onchangecorrespondance(value) {
    let url0 = symphony + "/Process/onchangecorrespondance";
    $.ajax({
        type: "POST",
        url: url0,
        data: { value: value },
        success: function (data) {
            if (data == "false") {
                if (window.confirm("Do you want add a new correspondence entry?" + value + "th Correspondence")) {
                    $('#author').val('author');
                    $('#DocAmt').val('');
                    $('#AmtAcc').val('');
                    $('#AmtDisp').val('');
                    $('#msgContent').val('');
                    document.getElementById("ref").readOnly = true;
                    document.getElementById("invoice").readOnly = true;
                }
                else {
                    let temps = value - 1;
                    $('#comboBox1').val(temps);
                }
            }
        },
    });
}
function setdateinterlinecorrespond(id) {
    $("#" + id).datepicker({
        changeYear: true,
        yearRange: "2010:2035",
        dateFormat: "dd-M-yy",
        onSelect: function (dateText, instance) {
            date = $.datepicker.parseDate(instance.settings.dateFormat, dateText, instance.settings);
            date.setMonth(date.getMonth() + 2);
            $("#LimDate").datepicker("setDate", date);
        }
    }).datepicker("show");
    $("#LimDate").datepicker({
        dateFormat: "dd-M-yy"
    });
}
/*interline*/
function Rejectionanalyticsload() {
    biduleRejectionanalyticsload(1);
}
function biduleRejectionanalyticsload(x) {
    switch (x) {
        case 1:
            /*RejectionList*/
            let url = symphony + "/Interline/RejectionList";
            let cboRAChargeCode = $('#cboRAChargeCode').val();
            let cboRAInvoiceNumberSelectedIndex = document.getElementById('cboRAInvoiceNumber').selectedIndex;
            let cboRAChargeCodeSelectedIndex = document.getElementById('cboRAChargeCode').selectedIndex;
            let cboRAInvoiceNumber = $('#cboRAInvoiceNumber').val();
            let Param = 0;//*******jerene tsara*************/
            $.ajax({
                type: "POST",
                url: url,
                data: { Param: Param, cboRAChargeCode: cboRAChargeCode, cboRAInvoiceNumberSelectedIndex: cboRAInvoiceNumberSelectedIndex, cboRAInvoiceNumber: cboRAInvoiceNumber, cboRAChargeCodeSelectedIndex: cboRAChargeCodeSelectedIndex },
                beforeSend: function () {
                },
                success: function (data) {
                    $("#divinvisible").html($(data).filter('#newdivinvisible'));
                    $("#divdgvRejection").html($(data).filter('#newdivdgvRejection'));
                },
                complete: function () {
                    if ($('#newlblNextStage').val() != "") {
                        document.getElementById('lblNextStage').innerHTML = $('#newlblNextStage').val();
                    }
                    document.getElementById('pnlAccept').style.visibility = $('#newpnlAcceptVisible').val();
                    document.getElementById('pnlRejection').style.visibility = $('#newpnlRejectionVisible').val();
                    biduleRejectionanalyticsload(2);
                },
                error: function () {
                }
            });
            /*fin RejectionList*/
            break;
        case 2:
            /*LoadInvoiceNumber*/
            let url1 = symphony + "/Interline/LoadInvoiceNumber";
            $.ajax({
                type: "POST",
                url: url1,
                data: {  },
                beforeSend: function () {
                },
                success: function (data) {
                    $("#divcboRAInvoiceNumber").html($(data).filter('#newdivcboRAInvoiceNumber'));
                },
                complete: function () {
                    document.getElementById('cboRAInvoiceNumber').selectedIndex = -1;
                },
                error: function () {
                }
            });
            /*fin LoadInvoiceNumber*/
            break;
    }
}
function comboBox9_SelectionChangeCommittedrejectionanalytic() {
    bidulecomboBox9_SelectionChangeCommittedrejectionanalytic(1);
}
function bidulecomboBox9_SelectionChangeCommittedrejectionanalytic(x) {
    switch (x) {
        case 1:
            /*RejectionList*/
            let urla1 = symphony + "/Interline/RejectionList";
            let cboRAChargeCode = $('#cboRAChargeCode').val();
            let cboRAInvoiceNumberSelectedIndex = document.getElementById('cboRAInvoiceNumber').selectedIndex;
            let cboRAChargeCodeSelectedIndex = document.getElementById('cboRAChargeCode').selectedIndex;
            let cboRAInvoiceNumber = $('#cboRAInvoiceNumber').val();
            let X = document.getElementById('comboBox9').selectedIndex;
            if (X == 0) { X = 0; }
            if (X == 1) { X = -1; }
            if (X == 2) { X = 1;}
            if (X == 3) { X = 2; }
            if (X == 4) { X = 3;}
            if (X == 5) { X = 4; }
            if (X == 6) { X = 5; }
            if (X == 3 || X == 1) {  }
            let Param = X;//*******jerene tsara*************/
            $.ajax({
                type: "POST",
                url: urla1,
                data: { Param: Param, cboRAChargeCode: cboRAChargeCode, cboRAInvoiceNumberSelectedIndex: cboRAInvoiceNumberSelectedIndex, cboRAInvoiceNumber: cboRAInvoiceNumber, cboRAChargeCodeSelectedIndex: cboRAChargeCodeSelectedIndex },
                beforeSend: function () {
                },
                success: function (data) {
                    $("#divinvisible").html($(data).filter('#newdivinvisible'));
                    $("#divdgvRejection").html($(data).filter('#newdivdgvRejection'));
                },
                complete: function () {
                    if ($('#newlblNextStage').val() != "") {
                        document.getElementById('lblNextStage').innerHTML = $('#newlblNextStage').val();
                    }
                    document.getElementById('pnlAccept').style.visibility = $('#newpnlAcceptVisible').val();
                    document.getElementById('pnlRejection').style.visibility = $('#newpnlRejectionVisible').val();
                },
                error: function () {
                }
            });
            /*fin RejectionList*/
            break;
    }
}
function cboRAInvoiceNumberchangeindex() {
    gotocboRAInvoiceNumberchangeindex(1);
}
function gotocboRAInvoiceNumberchangeindex(x) {
    switch (x) {
        case 1:
            if (x == 1) {
                /*if (cboRAInvoiceNumber.SelectedIndex != 0)*/
                let cboRAInvoiceNumberSelectedIndex = document.getElementById('cboRAInvoiceNumber').selectedIndex;
                let cboRAInvoiceNumber = $('#cboRAInvoiceNumber').val();
                if (cboRAInvoiceNumberSelectedIndex != 0) {
                    document.getElementById('lblArchiveInvNo').innerHTML = cboRAInvoiceNumber.trim();
                    /*GetInvoiceCount*/
                    let url = symphony + "/Interline/GetInvoiceCountrejectionanalytic";
                    let x = document.getElementById('cboRAInvoiceNumber').selectedIndex;
                    let lblArchiveInvNo = document.getElementById('lblArchiveInvNo').innerHTML;
                    $.ajax({
                        type: "POST",
                        url: url,
                        data: { x: x, lblArchiveInvNo: lblArchiveInvNo },
                        beforeSend: function () {
                        },
                        success: function (data) {
                            document.getElementById('bntArchived').disabled = true;
                            $("#divdvgarchive").html($(data).filter('#newdivdvgarchive'));
                            document.getElementById('lblarcmsg').innerHTML = "";

                        },
                        complete: function () {
                            if ($('#newlblarcmsg').val() != "") {
                                document.getElementById('lblarcmsg').innerHTML = $('#newlblarcmsg').val();
                            }
                            if ($('#bntArchivedEnabled').val() == "true") {
                                document.getElementById('bntArchived').disabled = false;
                            }
                            if ($('#bntArchivedEnabled').val() == "false") {
                                document.getElementById('bntArchived').disabled = true;
                            }
                        },
                        error: function () {
                        }
                    });
                    /*fin GetInvoiceCount*/
                }
                gotocboRAInvoiceNumberchangeindex(2);
                /*fin if (cboRAInvoiceNumber.SelectedIndex != 0)*/
            }
            break;
        case 2:
            if (x == 2) {
                /*RALoadChargeCode*/
                let urlq = symphony + "/Interline/RALoadChargeCoderejectionanalytic";
                let cboRAInvoiceNumberSelectedIndexq = document.getElementById('cboRAInvoiceNumber').selectedIndex;
                let cboRAInvoiceNumberq = $('#cboRAInvoiceNumber').val();
                $.ajax({
                    type: "POST",
                    url: urlq,
                    data: { cboRAInvoiceNumberSelectedIndexq: cboRAInvoiceNumberSelectedIndexq, cboRAInvoiceNumberq: cboRAInvoiceNumberq },
                    beforeSend: function () {
                    },
                    success: function (data) {
                        document.getElementById('cboRAChargeCode').selectedIndex = -1;
                        $("#divcboRAChargeCode").html($(data).filter('#newdivcboRAChargeCode'));
                    },
                    complete: function () {
                        if ($('#cboRAChargeCodeSelectedIndex').val() != "") {
                            document.getElementById('cboRAChargeCode').selectedIndex = $('#cboRAChargeCodeSelectedIndex').val();
                        }
                        gotocboRAInvoiceNumberchangeindex(3);
                    },
                    error: function () {
                    }
                });
                /*finRALoadChargeCode*/
            }
            break;
        case 3:
            if (x == 3) {
                /*RejectionList*/
                let url = symphony + "/Interline/RejectionList";
                let cboRAChargeCode = $('#cboRAChargeCode').val();
                let cboRAInvoiceNumberSelectedIndexza = document.getElementById('cboRAInvoiceNumber').selectedIndex;
                let cboRAChargeCodeSelectedIndex = document.getElementById('cboRAChargeCode').selectedIndex;
                let cboRAInvoiceNumberza = $('#cboRAInvoiceNumber').val();
                let X = document.getElementById('comboBox9').selectedIndex;
                if (X == 0) { X = 0; }
                if (X == 1) { X = -1; }
                if (X == 2) { X = 1; }
                if (X == 3) { X = 2; }
                if (X == 4) { X = 3; }
                if (X == 5) { X = 4; }
                if (X == 6) { X = 5; }
                let Param = X;//*******jerene tsara*************/
                $.ajax({
                    type: "POST",
                    url: url,
                    data: { Param: Param, cboRAChargeCode: cboRAChargeCode, cboRAInvoiceNumberSelectedIndex: cboRAInvoiceNumberSelectedIndexza, cboRAInvoiceNumber: cboRAInvoiceNumberza, cboRAChargeCodeSelectedIndex: cboRAChargeCodeSelectedIndex },
                    beforeSend: function () {
                    },
                    success: function (data) {
                        $("#divinvisible").html($(data).filter('#newdivinvisible'));
                        $("#divdgvRejection").html($(data).filter('#newdivdgvRejection'));
                    },
                    complete: function () {
                        if ($('#newlblNextStage').val() != "") {
                            document.getElementById('lblNextStage').innerHTML = $('#newlblNextStage').val();
                        }
                        document.getElementById('pnlAccept').style.visibility = $('#newpnlAcceptVisible').val();
                        document.getElementById('pnlRejection').style.visibility = $('#newpnlRejectionVisible').val();
                        gotocboRAInvoiceNumberchangeindex(4);
                    },
                    error: function () {
                    }
                });
                /*fin RejectionList*/
            }
            break;
        case 4:
            if (x == 4) {
                /*LoadInvoiceSummary*/
                let urlsa = symphony + "/Interline/LoadInvoiceSummaryrejectionanaltityc";
                let cboRAInvoiceNumber = $('#cboRAInvoiceNumber').val();
                $.ajax({
                    type: "POST",
                    url: urlsa,
                    data: { cboRAInvoiceNumber: cboRAInvoiceNumber },
                    beforeSend: function () {
                    },
                    success: function (data) {
                        $("#divdataGridView20").html($(data).filter('#newdivdataGridView20'));
                    },
                    complete: function () {
                    },
                    error: function () {
                    }
                });
                /*fin LoadInvoiceSummary*/
            }
            break;
    }
}
function button17_Click_1rejectionanalytic() {
    /*RejectionList*/
    let url = symphony + "/Interline/RejectionList";
    let cboRAChargeCode = $('#cboRAChargeCode').val();
    let cboRAInvoiceNumberSelectedIndex = document.getElementById('cboRAInvoiceNumber').selectedIndex;
    let cboRAChargeCodeSelectedIndex = document.getElementById('cboRAChargeCode').selectedIndex;
    let cboRAInvoiceNumber = $('#cboRAInvoiceNumber').val();
    let X = document.getElementById('comboBox9').selectedIndex;
    if (X == 0) { X = 0; }
    if (X == 1) { X = -1; }
    if (X == 2) { X = 1; }
    if (X == 3) { X = 2; }
    if (X == 4) { X = 3; }
    if (X == 5) { X = 4; }

    let Param = X;//*******jerene tsara*************/
    $.ajax({
        type: "POST",
        url: url,
        data: { Param: Param, cboRAChargeCode: cboRAChargeCode, cboRAInvoiceNumberSelectedIndex: cboRAInvoiceNumberSelectedIndex, cboRAInvoiceNumber: cboRAInvoiceNumber, cboRAChargeCodeSelectedIndex: cboRAChargeCodeSelectedIndex },
        beforeSend: function () {
        },
        success: function (data) {
            $("#divinvisible").html($(data).filter('#newdivinvisible'));
            $("#divdgvRejection").html($(data).filter('#newdivdgvRejection'));
        },
        complete: function () {
            if ($('#newlblNextStage').val() != "") {
                document.getElementById('lblNextStage').innerHTML = $('#newlblNextStage').val();
            }
            document.getElementById('pnlAccept').style.visibility = $('#newpnlAcceptVisible').val();
            document.getElementById('pnlRejection').style.visibility = $('#newpnlRejectionVisible').val();
        },
        error: function () {
        }
    });
    /*fin RejectionList*/
}
function dgvRejection_CellClickrejectionanalytic(dgvRejectionCurrentRowIndex, value0, value1, value2, value3, value4, value5, value6, value7, value22, value26) {
    bidulebutton17_Click_1rejectionanalytic(1, dgvRejectionCurrentRowIndex, value0, value1, value2, value3, value4, value5, value6, value7, value22, value26);
}
function bidulebutton17_Click_1rejectionanalytic(x, dgvRejectionCurrentRowIndex, value0, value1, value2, value3, value4, value5, value6, value7, value22, value26) {
    switch (x) {
        case 1:
            /*dgvRejection_CellClick*/
            let pvChargeCode = $('#pvChargeCode').val();
            let txtReasonCode = $('#txtReasonCode').val();
            let url = symphony + "/Interline/dgvRejection_CellClickrejectionanalytic";
            $.ajax({
                type: "POST",
                url: url,
                data: { txtReasonCode:txtReasonCode,pvChargeCode: pvChargeCode, dgvRejectionCurrentRowIndex: dgvRejectionCurrentRowIndex, value0: value0, value1: value1, value2: value2, value3: value3, value4: value4, value5: value5, value6: value6, value7: value7, value22: value22, value26: value26 },
                beforeSend: function () {
                },
                success: function (data) {
                    document.getElementById('cboRFA').selectedIndex = -1;
                    document.getElementById('cboReasonCode').selectedIndex = -1;
                    $('#txtReasonCode').val("");
                    $('#txtReasonDescription').val("");
                    $("#dataGridView25").html($(data).filter('#new0dataGridView25'));
                    document.getElementById('R2C1').innerHTML = "0.000";
                    document.getElementById('R2C2').innerHTML = "0.000";
                    document.getElementById('R2C3').innerHTML = "0.000";
                    document.getElementById('R2C4').innerHTML = "0.000";
                    document.getElementById('R2C5').innerHTML = "0.000";
                    document.getElementById('R2C6').innerHTML = "0.000";
                    document.getElementById('R2C7').innerHTML = "0.000";
                    document.getElementById('R2C8').innerHTML = "0.000";
                    document.getElementById('R2C9').innerHTML = "0.000";
                    document.getElementById('R2C10').innerHTML = "0.000";
                    document.getElementById('R2C11').innerHTML = "0.000";
                    document.getElementById('R2C12').innerHTML = "0.000";
                    document.getElementById('R2C13').innerHTML = "0.000";
                    document.getElementById('R2C11').innerHTML = "0.000";
                    document.getElementById('R2C9').innerHTML = "0.000";
                    document.getElementById('R2C7').innerHTML = "0.000";
                    document.getElementById('R2C5').innerHTML = "0.000";

                    document.getElementById('R2C1').innerHTML = "0.000";
                    document.getElementById('R2C2').innerHTML = "0.000";
                    document.getElementById('R2C3').innerHTML = "0.000";
                    document.getElementById('R2C4').innerHTML = "0.000";
                    document.getElementById('R2C5').innerHTML = "0.000";
                    document.getElementById('R2C6').innerHTML = "0.000";
                    document.getElementById('R2C7').innerHTML = "0.000";
                    document.getElementById('R2C8').innerHTML = "0.000";
                    document.getElementById('R2C9').innerHTML = "0.000";
                    document.getElementById('R2C10').innerHTML = "0.000";
                    document.getElementById('R2C11').innerHTML = "0.000";
                    document.getElementById('R2C12').innerHTML = "0.000";
                    document.getElementById('R2C13').innerHTML = "0.000";


                    document.getElementById('R1C1').innerHTML = "0.000";
                    document.getElementById('R1C2').innerHTML = "0.000";
                    document.getElementById('R1C3').innerHTML = "0.000";
                    document.getElementById('R1C4').innerHTML = "0.000";
                    document.getElementById('R1C5').innerHTML = "0.000";
                    document.getElementById('R1C6').innerHTML = "0.000";
                    document.getElementById('R1C7').innerHTML = "0.000";
                    document.getElementById('R1C8').innerHTML = "0.000";
                    document.getElementById('R1C9').innerHTML = "0.000";
                    document.getElementById('R1C10').innerHTML = "0.000";
                    document.getElementById('R1C11').innerHTML = "0.000";
                    document.getElementById('R1C12').innerHTML = "0.000";
                    document.getElementById('R1C13').innerHTML = "0.000";
                    
                    $("#dataGridView25").html($(data).filter('#newdataGridView25'));
                    $("#divdgvAttachment").html($(data).filter('#newdivdgvAttachment'));
                    $("#divdataGridView21").html($(data).filter('#new001divdataGridView21'));

                },
                complete: function () {
                    $("#pvChargeCode").val($('#newpvChargeCode001').val());
                    $("#PreviousBillingPeriod").val($('#newPreviousBillingPeriod001').val());
                    $("#FullDocNo").val($('#newFullDocNo001').val());
                    document.getElementById('pnlAccept').style.visibility = $('#newpnlAcceptVisible001').val();
                    bidulebutton17_Click_1rejectionanalytic(2, '', '', '', '', '', '', '', '', '','','');
                    /*OBData*/
                    document.getElementById('lblRMN1').innerHTML = $('#newlblRMN1001').val(); 
                    document.getElementById('lblRMNStage1').innerHTML = $('#newlblRMNStage1001').val(); 
                    document.getElementById('lblRMNCode1').innerHTML = $('#newlblRMNCode1001').val();
                    document.getElementById('txtRMNRDesc1').value = $('#newtxtRMNRDesc1001').val(); 
                    document.getElementById('lblRMNInvoiceNumber1').innerHTML = $('#newlblRMNInvoiceNumber1001').val(); 
                    document.getElementById('lblRMNBillingDate1').innerHTML = $('#newlblRMNBillingDate1001').val();
                    document.getElementById('lblAttCount').innerHTML = $('#newlblAttCount').val(); 
                    document.getElementById('lblRejMsg').innerHTML = $('#newlblRejMsg001').val();
                    document.getElementById('lblNextStage').innerHTML = $('#newlblNextStage001').val();
                    document.getElementById('R1C1').innerHTML = $('#newR1C1').val();
                    document.getElementById('R1C2').innerHTML = $('#newR1C2').val();
                    document.getElementById('R1C3').innerHTML = $('#newR1C3').val();
                    document.getElementById('R1C4').innerHTML = $('#newR1C4').val();
                    document.getElementById('R1C5').innerHTML = $('#newR1C5').val();
                    document.getElementById('R1C6').innerHTML = $('#newR1C6').val();
                    document.getElementById('R1C7').innerHTML = $('#newR1C7').val();
                    document.getElementById('R1C8').innerHTML = $('#newR1C8').val();
                    document.getElementById('R1C9').innerHTML = $('#newR1C9').val();
                    document.getElementById('R1C10').innerHTML = $('#newR1C10').val();
                    document.getElementById('R1C11').innerHTML = $('#newR1C11').val();
                    document.getElementById('R1C12').innerHTML = $('#newR1C12').val();
                    document.getElementById('R1C13').innerHTML = $('#newR1C13').val();
                    document.getElementById('R2C1').innerHTML = $('#newR2C1').val();
                    document.getElementById('R2C2').innerHTML = $('#newR2C2').val();
                    document.getElementById('R2C3').innerHTML = $('#newR2C3').val();
                    document.getElementById('R2C4').innerHTML = $('#newR2C4').val();
                    document.getElementById('R2C5').innerHTML = $('#newR2C5').val();
                    document.getElementById('R2C6').innerHTML = $('#newR2C6').val();
                    document.getElementById('R2C7').innerHTML = $('#newR2C7').val();
                    document.getElementById('R2C8').innerHTML = $('#newR2C8').val();
                    document.getElementById('R2C9').innerHTML = $('#newR2C9').val();
                    document.getElementById('R2C10').innerHTML = $('#newR2C10').val();
                    document.getElementById('R2C11').innerHTML = $('#newR2C11').val();
                    document.getElementById('R2C12').innerHTML = $('#newR2C12').val();
                    document.getElementById('R2C13').innerHTML = $('#newR2C13').val();
                    document.getElementById('R3C1').innerHTML = $('#newR3C1').val();
                    document.getElementById('R3C2').innerHTML = $('#newR3C2').val();
                    document.getElementById('R3C3').innerHTML = $('#newR3C3').val();
                    document.getElementById('R3C4').innerHTML = $('#newR3C4').val();
                    document.getElementById('R3C5').innerHTML = $('#newR3C5').val();
                    document.getElementById('R3C6').innerHTML = $('#newR3C6').val();
                    document.getElementById('R3C7').innerHTML = $('#newR3C7').val();
                    document.getElementById('R3C8').innerHTML = $('#newR3C8').val();
                    document.getElementById('R3C9').innerHTML = $('#newR3C9').val();
                    document.getElementById('R3C10').innerHTML = $('#newR3C10').val();
                    document.getElementById('R3C11').innerHTML = $('#newR3C11').val();
                    document.getElementById('R3C12').innerHTML = $('#newR3C12').val();
                    document.getElementById('R3C13').innerHTML = $('#newR3C13').val();
                    $("#txtReasonCodeX").val($('#newtxtReasonCodeX001').val());
                    $("#txtReasonDescriptionX").val($('#newtxtReasonDescriptionX001').val());
                    $("#NotinSystem").val($('#newNotinSystem001').val());
                    bidulebutton17_Click_1rejectionanalytic(3, '', '', '', '', '', '', '', '', '', '', '');
                    if ( $('#button19Enabled001').val() == "true"){
                        document.querySelector('button19').disabled = false;
                    }
                    if ($('#button19Enabled001').val() == "false") {
                        document.querySelector('button19').disabled = true;
                    }
                    document.getElementById('lblRejTotalTax').innerText = $('#newlblRejTotalTax001').val();
                    $("#invoiceno").val($('#newinvoiceno001').val());
                    $("#TicketNo").val($('#newTicketNo001').val());
                    $("#CouponNumber").val($('#newCouponNumber001').val());



                    /* fin OBData*/
                },
                error: function () {
                }
            });
            /* fin dgvRejection_CellClick*/
            break;
        case 2:
            if (x == 2) {
                let pvChargeCode = $('#pvChargeCode').val();
                if (pvChargeCode.trim().length > 0) {
                    /*LoadRefReasonCode()*/
                    let url10 = symphony + "/Interline/LoadRefReasonCode";
                    $.ajax({
                        type: "POST",
                        url: url10,
                        data: { pvChargeCode: pvChargeCode },
                        beforeSend: function () {
                        },
                        success: function (data) {
                            $("#divcboReasonCode").html($(data).filter('#newdivcboReasonCode'));
                        },
                        complete: function () {
                            document.getElementById('cboReasonCode').selectedIndex = -1;
                        },
                        error: function () {
                        }
                    });
                    /*fin LoadRefReasonCode()*/
                }
            }
            break;
        case 3:
            if (x == 3) {
                let R3C2 = document.getElementById('R3C2').innerHTML;
                let url1 = symphony + "/Interline/testR3C2";
                $.ajax({
                    type: "POST",
                    url: url1,
                    data: { R3C2: R3C2 },
                    beforeSend: function () {
                    },
                    success: function (data) {
                        $("#testR3C2").html($(data).filter('#new001testR3C2'));
                    },
                    complete: function () {
                    },
                    error: function () {
                    }
                });
            }
            break;
    }
}
/*function RejectionList(param) {
    bidule(1);
}
function bidule(x) {
    switch (x) {
        case 1:
            alert('01');
            bidule(2);
            break;
        case 2:
            alert('02');
            bidule(3);
            break;
        case 3:
            alert('03');
            break;
    }
}*/
/*interline*/
/*********************fin fait par christian***********/








function  setdate(id, idcheck1, idcheck2) {
    $("#" + id).datepicker({
        changeYear: true,
        yearRange: "2010:2035",
        dateFormat: "dd-M-yy"
    }).datepicker("show");
    $("#" + idcheck1).prop("checked", true);
    $("#" + idcheck2).prop("checked", true);
}
function uncheked(id, id1, input, input1) {
    if ($("#" + id).is(':checked')) {
        $("#" + id).prop("checked", true);
        $("#" + id1).prop("checked", true);
    } else {
        $("#" + id).prop("checked", false);
        $("#" + id1).prop("checked", false);
        //$("#" + input).val('');
        //$("#" + input1).val('');
    }
}
function uncheked1(id, id1, input, input1) {
    if ($("#" + id).is(':checked')) {
        $("#" + id).prop("checked", true);
        $("#" + id1).prop("checked", true);
    } else {
        $("#" + id).prop("checked", false);
        $("#" + id1).prop("checked", false);
        //$("#" + input).val('');
        //$("#" + input1).val('');
    }
}

function reduire(myid) {
    //var i= myid;//console.log(i);
    var etat = document.getElementById(myid).style.display;
    if (etat == 'none') {
        document.getElementById(myid).style.display = 'block';
    }
    else {
        document.getElementById(myid).style.display = 'none';
    }
}

function Ajaxs(array, array1, classSuccess, url) {
    $.ajax({
        type: 'POST',
        data: { dataValue: array, dataValue1: array1 },
        url: url,
        async: false,
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            if (classSuccess == "success") {
                $('#successModal').modal('show');
                $("#msgsuccess").html(data);
            } else {
                $("." + classSuccess).html(data);
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

function clearPax() {
    let url = symphony + "/Sales/PAXTKTs";
    $.ajax({
        type: "POST",
        url: url,
        success: function (msg) {
            $('.container-pax1').html(msg);
        }, error: function (msg) {
            $('#errorModal').modal('show');
        }
    });
}

function postRequeteCriter(parentId, nextOrPreview, type, tab) { /// type = transaction code, tab = condition click group transaction code "little table"

    let agentCode = $("#" + parentId + " [name=agentCode]").val();
    let docummentNo = $("#" + parentId + " [name=docummentNo]").val();
    let pnr = $("#" + parentId + " [name=pnr]").val();
    let passengerName = $("#" + parentId + " [name=passengerName]").val();
    let datedtpIssueDateFrom = $("#" + parentId + " [name=datedtpIssueDateFrom]").val();
    let dtpIssueDateFrom = $("#" + parentId + "  input[name=dtpIssueDateFrom]:checkbox:checked").val();
    let dtpIssueDateTo = $("#" + parentId + "  input[name=dtpIssueDateTo]:checkbox:checked").val();
    let datedtpIssueDateTo = $("#" + parentId + "  [name=datedtpIssueDateTo]").val();
    let radContains = $("#" + parentId + "  input:radio[name=radContains]:checked").val();
    let domeInt = $("#" + parentId + "  input:radio[name=domeInt]:checked").val();
    let ownOal = $("#" + parentId + "  input:radio[name=ownOal]:checked").val();



    let lastRow = $("#" + parentId + " #infoPagination #lastRow").val();
    let nbResult = $("#" + parentId + " #infoPagination #nbResult").val();
    let next = $("#" + parentId + " #infoPagination #plus").val();
    let preview = $("#" + parentId + " #infoPagination #moin").val();
    let totalPage = $("#" + parentId + " #infoPagination #total-page").val();

    let allerAjax = false;
    let url = symphony + "/Sales/GetSearchCriteria";
    if (nextOrPreview == "next") {
        if (next <= totalPage) {
            allerAjax = true;
        }
    }
    if (nextOrPreview == "preview") {
        if (preview != "0" && preview >= 1) {
            allerAjax = true;
        }
    }

    if (nextOrPreview == "last") {
        if (next <= totalPage) {
            allerAjax = true;
        }
    }
    if (nextOrPreview == "first") {
        if (preview != "0" && preview >= 1) {
            allerAjax = true;
        }
    }

    if (tab) {
        allerAjax = true;
        next = "";
        preview = "";
    }

    if (allerAjax == true) {
        $.ajax({
            type: 'POST',
            data: {
                agentCode: agentCode, docummentNo: docummentNo, pnr: pnr, passengerName: passengerName,
                datedtpIssueDateFrom: datedtpIssueDateFrom, dtpIssueDateFrom: dtpIssueDateFrom, dtpIssueDateTo: dtpIssueDateTo,
                datedtpIssueDateTo: datedtpIssueDateTo, radContains: radContains, ownOal: ownOal, domeInt: domeInt, type: type,
                lastRow: lastRow, nbResult: nbResult, nextOrPreview: nextOrPreview, next: next, preview: preview, totalPage: totalPage
            },
            url: url,
            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {
                //console.log($(data).filter("#newTable-pax"))
                console.log(parentId);
                $("#" + parentId + " #table-pax").html($(data).filter("#newTable-pax"));
                $("#" + parentId + " .transaction-code-record").html($(data).filter("#newCountTransaction"))
                $("#" + parentId + " .Data-pagination").html($(data).filter("#infoPagination"));
                $("#" + parentId + " #input-next").val($("#" + parentId + " #total-page").val());
                $("#" + parentId + " #input-preview").val($("#" + parentId + " #moin").val());
                let from = 1
                if ($("#" + parentId + " #lastRow").val() > 1000) {
                    from = $("#" + parentId + " #moin").val() * 1000 - 1000;
                }
                let to = $("#" + parentId + " #plus").val() * 1000 < $("#" + parentId + " #nbResult").val() ? $("#" + parentId + " #plus").val() * 1000 : $("#" + parentId + " #nbResult").val();
                $("#" + parentId + " #input-nb-page").val(from + ' to ' + to + ' of ' + $("#" + parentId + " #nbResult").val());

                if ($("#" + parentId + " #input-next").val() == $("#" + parentId + " #input-preview").val()) {
                    $("#" + parentId + " #displayRecord").text("Completed");
                } else {
                    $("#" + parentId + " #displayRecord").text("Displaying record(s)");
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

}
function postedSqlCriteria(event, nameController, nameAction, type) {
    var $form = $(event).closest('form');
    var $form2 = $(event).closest('markelement');
    let parentId = $form.attr('id');
    let grandParentId = $form2.attr('id');
    let url = symphony + "/Sales/GetSearchCriteria";
    let agentCode = $("#" + parentId + " [name=agentCode]").val();
    let docummentNo = $("#" + parentId + " [name=docummentNo]").val();
    let pnr = $("#" + parentId + " [name=pnr]").val();
    let passengerName = $("#" + parentId + " [name=passengerName]").val();
    let datedtpIssueDateFrom = $("#" + parentId + " [name=datedtpIssueDateFrom]").val();
    let dtpIssueDateFrom = $("#" + parentId + " input[name=dtpIssueDateFrom]:checkbox:checked").val();
    let dtpIssueDateTo = $("#" + parentId + " input[name=dtpIssueDateTo]:checkbox:checked").val()
    let datedtpIssueDateTo = $("#" + parentId + " [name=datedtpIssueDateTo]").val();
    let radContains = $("#" + parentId + " input:radio[name=radContains]:checked").val();
    let domeInt = $("#" + parentId + " input:radio[name=domeInt]:checked").val();
    let ownOal = $("#" + parentId + " input:radio[name=ownOal]:checked").val();
    let lastRow = $("#" + grandParentId + " #nbResult").val();
    let nbResult = $("#" + grandParentId + " #nbResult").val();
    let vide = false;
    if (passengerName == '' && pnr == '' && docummentNo == '' && agentCode == '') { vide = true; }
    if (vide == true && dtpIssueDateFrom == "on" && (datedtpIssueDateFrom == "" || datedtpIssueDateTo == "")) {
        $('#' + grandParentId + ' #alertModal').modal('show');
        $("#" + grandParentId + " #msgalert").html("Select a date range");
    } else if (vide == true && datedtpIssueDateFrom == "" && datedtpIssueDateTo == "") {
        $("#" + grandParentId + " #alertModal").modal('show');
        $("#" + grandParentId + " #msgalert").html("Please enter the criteria");
    } else if (vide == true && dtpIssueDateFrom == undefined) {
        $("#" + grandParentId + " #alertModal").modal('show');
        $("#" + grandParentId + " #msgalert").html("Select the check box in the date range");
    } else {

        $.ajax({
            type: 'POST',
            data: {
                agentCode: agentCode, docummentNo: docummentNo, pnr: pnr, passengerName: passengerName, datedtpIssueDateFrom: datedtpIssueDateFrom,
                dtpIssueDateFrom: dtpIssueDateFrom, dtpIssueDateTo: dtpIssueDateTo, datedtpIssueDateTo: datedtpIssueDateTo, radContains: radContains,
                ownOal: ownOal, domeInt: domeInt, type: type, lastRow: lastRow
            },
            url: url,
            //async: false,
            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {

                $("#" + grandParentId + " #table-pax").html($(data).filter('#newTable-pax'));
                //$("#countTransaction").html($(data).filter('#newCountTransaction'));
                $("#" + grandParentId + " .transaction-code-record").html($(data).filter('#newCountTransaction'))
                $("#" + grandParentId + " .Data-pagination").html($(data).filter('#infoPagination'));
                let totalPage = $("#total-page").val();
                $("#" + grandParentId + " #input-next").val(totalPage);
                $("#" + grandParentId + " #input-preview").val($("#moin").val());
                let from = 1
                if ($("#" + grandParentId + " #lastRow").val() > 1000) {
                    from = $("#" + grandParentId + " #moin").val() * 1000;
                }
                let to = $("#" + grandParentId + " #plus").val() * 1000 < $("#" + grandParentId + " #nbResult").val() ? $("#" + grandParentId + " #plus").val() * 1000 : $("#" + grandParentId + " #nbResult").val();
                $("#" + grandParentId + " #input-nb-page").val(from + ' to ' + to + ' of ' + $("#" + grandParentId + " #nbResult").val());
            },
            complete: function () {
                $('.ajax-loader').css("visibility", "hidden");
                if ($("#" + grandParentId + " #input-next").val() == $("#" + grandParentId + " #input-preview").val()) {
                    $("#" + grandParentId + " #displayRecord").text("Completed");
                } else {
                    $("#" + grandParentId + " #displayRecord").text("Displaying record(s)");
                }

                //fixHeader('newTable-pax');
            },
            error: function () {
                $("#" + grandParentId + " #errorModal").modal('show');
            }
        });
    }

}

function fixHeader(id) { //newTable-pax
    var tabWhidth = [];
    $('table#' + id + ' tbody tr:first td').each(function () {

        var width = this.getBoundingClientRect().width;
        tabWhidth.push(width);
    });
    let i = 0;
    $('table#' + id + ' thead tr th').each(function () {
        $(this).attr("id", 'setwhidth' + i);
        i++;
    });
    let j = 0;
    $('table#' + id + ' thead tr th').each(function () {
        document.getElementById('setwhidth' + j).style.minWidth = tabWhidth[j] + 'px';
        document.getElementById('setwhidth' + j).style.maxWidth = tabWhidth[j] + 'px';
        j++;
    });

}
function previewValue(event) {

    var $form2 = $(event).closest('markelement');
    let grandParentId = $form2.attr('id');
    let type = "";
    if ($("#" + grandParentId + " div#infoPagination #type").val() == "") {
        type = "PAX TKTs";
    } else {
        type = $("#" + grandParentId + " div #infoPagination #type").val();
    }
    if ($("#" + grandParentId + " #input-preview").val() > 1) {
        postRequeteCriter(grandParentId, 'preview', type, false);
    }

}

function nextValue(event) {
    var $form2 = $(event).closest('markelement');
    let grandParentId = $form2.attr('id');
    let type = "";

    if ($("#" + grandParentId + " div#infoPagination #type").val() == "") {
        type = "PAX TKTs";

    } else {
        type = $("#" + grandParentId + " #infoPagination #type").val();

    }
    console.log(type);
    if ($("#" + grandParentId + " #input-next").val() > $("#" + grandParentId + " #input-preview").val()) {

        postRequeteCriter(grandParentId, 'next', type, false);
    }


}

function fistValue(event) {
    var $form2 = $(event).closest('markelement');
    let grandParentId = $form2.attr('id');
    let type = "";
    if ($("#" + grandParentId + " div#infoPagination #type").val() == "") {
        type = "PAX TKTs";
    } else {
        type = $("#" + grandParentId + " div #infoPagination #type").val();
    }
    if ($("#" + grandParentId + " #input-preview").val() > 1) {
        postRequeteCriter(grandParentId, 'first', type, false);
    }

}
function lastValue(event) {

    var $form2 = $(event).closest('markelement');
    let grandParentId = $form2.attr('id');
    let type = "";
    if ($("#" + grandParentId + " div#infoPagination #type").val() == "") {
        type = "PAX TKTs";
    } else {
        type = $("#" + grandParentId + " div #infoPagination #type").val();
    }
    if ($("#" + grandParentId + " #input-next").val() > $("#" + grandParentId + " #input-preview").val()) {
        postRequeteCriter(grandParentId, 'last', type, false);
    }

}

function clickDocType(event, type) {
    var $form2 = $(event).closest('markelement');
    let grandParentId = $form2.attr('id');
    if (type == "TKTT") {
        type = "PAX TKTs";
    }
    postRequeteCriter(grandParentId, 'none', type, true);
}

function clickLigneTransaction(event, docNumber, transactionCode, domInt) {
    var $form = $(event).closest('markelement');
    let parentId = $form.attr('id')
    let url = symphony + "/Sales/Transaction";
    if (event == null) {
        parentId = 'GrandParent_PAXTKTs';
    }
    $.ajax({
        type: 'POST',
        data: { docNumber: docNumber, transactionCode: transactionCode },
        url: url,
        //async: false,
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            //$('#tab-transaction').trigger('click');
            $("li").removeClass("active");
            $("#" + parentId + " #search").removeClass("active");
            $("#" + parentId + " #testtrans").addClass("active");
            $("#" + parentId + " #transaction").addClass("active in");
            $("#" + parentId + " .data-transaction").html(data);
            $("#" + parentId + " .data-transaction").html(data);
            if (domInt == "I") {
                domInt = "INTERNATIONAL";
            }
            if (domInt == "D") {
                domInt = "DOMESTIC";
            }
            $("#" + parentId + " #domInt").val(domInt);
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");

        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}



function clickLigneTransaction2(event, docNumber, transactionCode) {
    var $form = $(event).closest('markelement');
    let parentId = $form.attr('id')
    let url = symphony + "/Sales/Transaction";
    if (event == null) {
        parentId = 'GrandParent_PAXTKTs';
    }
    $.ajax({
        type: 'POST',
        data: { docNumber: docNumber, transactionCode: transactionCode },
        url: url,
        success: function (data) {

           /* $("li").removeClass("active");
            $("#" + parentId + " #search").removeClass("active");
            $("#" + parentId + " #testtrans").addClass("active");
            $("#" + parentId + " #transaction").addClass("active in");
            $("#" + parentId + " .data-transaction").html(data);
            $("#" + parentId + " .data-transaction").html(data);*/

            $("#search").html(data);
            $("#transaction").html(data);

        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}




function cherche(myvar, parentId) {
    let url = symphony + "/Sales/CherchByCode";
    $.ajax({
        type: "POST",
        url: url,
        data: { value: myvar },
        // dataType: "text",
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (msg) {
            $("#" + parentId + " .autocomplete").html(msg);
        },
        error: function (msg) {
            $('#errorModal').modal('show');
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
        },
    });

}

$(".autocomplete").on('click', ".table-result-code-name tbody tr", function () {

    let code = $(this).find('td:eq(0)').text();
    let name = $(this).find('td:eq(1)').text();
    let city = $(this).find('td:eq(2)').text();
    $('#agent-name-label').html(name)
    $('#agent-code-label').html(code)
    $('#agent-city-label').html(code)

});

/*$(function () {   datedtpIssueDateFrom   datedtpIssueDateTo
    $('.datetimepicker').datepicker();
});*/

/// click sur payement et recuperer les valeur du requette sql

function showModalAncillary(event) {
    let $form2 = $(event).closest('markelement');
    let parentId = $form2.attr('id');
    let url = symphony + "/Sales/FrmOfPayementTfcComm";
    let docNumber = $("#" + parentId + " #firstDocNumber").val() + $("#" + parentId + " #secondDocNumber").val();
    console.log(parentId, docNumber)
    $("#" + parentId + " #documentNumber").val(docNumber);
    $("#" + parentId + " #Ancillary").modal('show');
    $.ajax({
        type: 'POST',
        data: { docNumber: docNumber },
        url: url,
        success: function (data) {
            $("#" + parentId + " #newFrmPayement").html(data);
            showTF(docNumber);
        },
        error: function () {
        }
    });

}
$(".ticket-payment").on('click', '#actionAncillary', function () {

    let url = symphony + "/Sales/FrmOfPayementTfcComm";
    $.ajax({
        type: 'POST',
        data: { nameController: nameController },
        url: url,
        success: function (data) {
            $("#newFrmPayement").html(data);
        },
        error: function () {
        }
    });

})
/***********Charger valeur et affiché ******************/
function showTF(docNumber) {
    let url = symphony + "/Sales/GetTf";
    $.ajax({
        type: 'POST',
        data: { docNumber: docNumber },
        url: url,
        success: function (data) {
            $("#newTFCs").html(data);
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
};
/*****More Details *****/
function showMoreDetails(event) {
    let $form2 = $(event).closest('markelement');
    let parentId = $form2.attr('id');
    let url = symphony + "/Sales/ViewsMoreDetails";
    let docNumber = $("#" + parentId + " #firstDocNumber").val() + $("#" + parentId + " #secondDocNumber").val();
    $("#" + parentId + " #moreDetails").modal('show');
    $.ajax({
        type: 'POST',
        data: { docNumber: docNumber },
        url: url,
        success: function (data) {
            $("#" + parentId + " #newMoreDetails").html(data);
            $("#" + parentId + " #myModalLabel").text("More Details on Document No. : " + $("#" + parentId + " #firstDocNumber").val() + $("#" + parentId + " #secondDocNumber").val())
        },
        error: function () {
            $("#" + parentId + " # errorModal").modal('show');
        }
    });
}

function showTicketHistory(event) {
    let $form2 = $(event).closest('markelement');
    let parentId = $form2.attr('id');

    $("#" + parentId + " #tickethistory").modal('show');
    let url = symphony + "/Sales/ticketHistoryPayment";

    let today = new Date().toISOString().slice(0, 10);
    let todayLess = new Date(today);

    let transactionCode = $("#" + parentId + " #transactionCode").val();
    let docNumber = $("#" + parentId + " #firstDocNumber").val() + $("#" + parentId + " #secondDocNumber").val();
    if ($("#" + parentId + " #testLoadTicket").val() != $("#" + parentId + " #firstDocNumber").val() + $("#" + parentId + " #secondDocNumber").val()) {
        $("#" + parentId + " #testLoadTicket").val($("#" + parentId + " #firstDocNumber").val() + $("#" + parentId + " #secondDocNumber").val());
        $.ajax({
            type: 'POST',
            data: { docNumber: docNumber, transactionCode: transactionCode },
            url: url,
            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {
                $("#" + parentId + " #table-formPayment").html(data);
                $("#" + parentId + " #dateNow").text(today);
            },
            complete: function () {
                $('.ajax-loader').css("visibility", "hidden");
            },
            error: function () {
                $("#" + parentId + " #errorModal").modal('show');
            }
        });

    }

}

function clickDocNumberHistory(transCode) {
    let url = symphony + "/Sales/ticketHistoryPayment";
    let docNumber = transCode.substring(0, 13);
    let transactionCode = transCode.substring(transCode.length - 4);
    let conjonction = "valide";
    $.ajax({
        type: 'POST',
        data: { docNumber: docNumber, transactionCode: transactionCode, conjonction: conjonction },
        url: url,
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $('.table-formPayment2').html(data);
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


$("#list-docNumb").on('click', ".glyphicon-plus", function () {
    $(this).addClass("show").find("li").slideDown();
});

function closeTransaction() {
    $('[href="#search"]').tab('show');
}

function clickSplitFCA(event) {
    let $form2 = $(event).closest('markelement');
    let parentId = $form2.attr('id');

    $("#" + parentId + " .textarea-editor").summernote({
        height: 300, // set editor height
        minHeight: null, // set minimum height of editor
        maxHeight: null, // set maximum height of editor
        focus: true // set focus to editable area after initializing summernote
    });
    if ($("#" + parentId + " #firstDocNumber").val() + $("#" + parentId + " #secondDocNumber").val() != $("#" + parentId + " #testLoadFca").val()) {

        let NumericCode = $("#" + parentId + " #firstDocNumber").val() + $("#" + parentId + " #secondDocNumber").val();
        $("#" + parentId + " #testLoadFca").val(NumericCode);
        let url = symphony + "/Sales/SPLITFCA";
        let farerfca = $("#" + parentId + " #fareCalculation").val();
        $("#" + parentId + " #sputfca").modal('show');
        $.ajax({
            type: "POST",
            data: { docNumber: NumericCode, fare: farerfca },
            url: url,
            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },

            success: function (data) {
                $("#" + parentId + " .splifac-body").html(data);
            },
            complete: function () {
                $('.ajax-loader').css("visibility", "hidden");
            },
            error: function () {
                $('#errorModal').modal('show');
            }

        });
    } else {
        $("#" + parentId + " #sputfca").modal('show');
    }
}

function userObservation() {
    $('#sputfca').modal('hide');
    $("#Observation").modal("show");
    $("#docNumber").val($("#firstDocNumber").val() + $("#secondDocNumber").val());
    $("#hdrguid").val($("#getHdrguid").text());
    $("#dateObs").val("");
    $(".textarea-editor").val("");
    $("#editSubject").val("");
}

$('.textarea-editor').summernote({
    height: 300, // set editor height
    minHeight: null, // set minimum height of editor
    maxHeight: null, // set maximum height of editor
    focus: true // set focus to editable area after initializing summernote
});

function openModalObservation(date, subject, obs, hdrguid, recId) {
    userObservation();
    $("#editSubject").val(subject);
    $("#docNumber").val($("#firstDocNumber").val() + $("#secondDocNumber").val());
    $("#recId").val(recId);
    $("#dateObs").val(date);
    if (hdrguid == "null") {
        hdrguid = $("#getHdrguid").text();
    }
    let getRelatidHdrguid = $("#getRelatidHdrguid").text();
    $("#hdrguid").val(hdrguid)

    $(".note-editable").html(obs)
}

function saveObs() {

    let subject = $("#editSubject").val();
    let docNumber = $("#firstDocNumber").val() + $("#secondDocNumber").val();
    let dateObs = $("#dateObs").val();
    let obsValue = textobservation;
    let hdrguid = $("#hdrguid").val();
    let recId = $("#recId").val();
    let url = symphony + "/Sales/saveObservation";
    /*var tab = [];
    tab.push($(".textarea-editor").val());*/
    $.ajax({
        type: 'POST',
        data: { subject: subject, docNumber: docNumber, dateObs: dateObs, obsValue: obsValue, hdrguid: hdrguid, recId: recId },
        url: url,
        //async: false,
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#dropDownTransactionType").html(data);
            $(".refreshObs").html(data);
            //apidirina eto ilay envoyena 
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}

/** code by fano transaction type **/

let valueDateFirst = "";
let valueDateSeconde = "";
$(document).ready(function () {
    let today = new Date().toISOString().slice(0, 10);
    let todayLess = new Date(today);
    todayLess.setMonth(todayLess.getMonth() - 1);
    let dateLess = todayLess.toISOString().slice(0, 10).split("-");
    let dateMore = today.split("-");
    valueDateFirst = dateLess[1] + "-" + dateLess[2] + "-" + dateLess[0];
    valueDateSeconde = dateMore[1] + "-" + dateMore[2] + "-" + dateMore[0];
    $("#transTypedateFrom").val(valueDateFirst);
    $("#transTypedateTo").val(valueDateSeconde);
    //----
    $("#dateFrom").val(valueDateFirst);
    $("#dateTo").val(valueDateSeconde);
});

function clickDropDownTrans0() {
    let dateFrom = $("#transTypedateFrom").val();
    let dateTo = $("#transTypedateTo").val();
    let url = symphony + "/Sales/LoadTransactionCode";
    if ($("#testTransType").val() != dateFrom || $("#testTrans1Type").val() != dateTo) {
        $("#testTransType").val(dateFrom);
        $("#testTrans1Type").val(dateTo);
        $.ajax({
            type: 'POST',
            data: {
                dateFrom: dateFrom, dateTo: dateTo
            },
            url: url,
            //async: false,
            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {
                $("#dropDownTransactionType").html(data);
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

/*function dropDownTransactionType0() {
    if (valueDateFirst != $("#transTypedateFrom").val() || valueDateSeconde != $("#transTypedateTo").val()) {
        valueDateFirst = $("#transTypedateFrom").val();
        valueDateSeconde = $("#transTypedateTo").val();
        clickDropDownTrans();
    }
}*/

function setCriteriaTransactionType() {
    let dateFrom = $("#transTypedateFrom").val();
    let dateTo = $("#transTypedateTo").val();
    let url = symphony + "/Sales/LoadTransactionType";
    let transactionCode = $("#dropDownTransactionType").val();
    if (dateFrom != "" && dateTo != "") {
        $.ajax({
            type: 'POST',
            data: {
                dateFrom: dateFrom, dateTo: dateTo, transactionCode: transactionCode
            },
            url: url,
            //async: false,
            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {
                $("#loadTrans-summary").html(data);
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
/** end **/

/** coed by fano cancelleation **/

function selectNameCode() {
    $("#agentCodeSet").val($('#agent-code-label').text());
    let today = new Date().toISOString().slice(0, 10);
    let todayLess = new Date(today);
    todayLess.setMonth(todayLess.getMonth() - 1);
    let dateLess = todayLess.toISOString().slice(0, 10).split("-");
    let dateMore = today.split("-");
    $("#cancelDateFrom").val(dateLess[2] + "-" + dateLess[1] + "-" + dateLess[0]);
    $("#cancelDateTo").val(dateMore[2] + "-" + dateMore[1] + "-" + dateMore[0]);
}

function clearCancellation() {
    $("#cancelDateFrom").val('');
    $("#cancelDateTo").val('');
    $("#agentCodeSet").val('');
    $("#newtotalCanx").val('');
    $("#newtotalCann").val('');
    $("#newtotalCanr").val('');
    $("#newtotalOther").val('');
    $("#newtotalDoc").val('');
    $(".table-record-display").html('');
}

function setCriteriaCancellation() {
    let rbcancellation = $("#rbcancellation input:radio[name=rbcancellation]:checked").val();
    let dateFrom = $("#cancelDateFrom").val();
    let dateTo = $("#cancelDateTo").val();
    let agentCodeSet = $("#agentCodeSet").val();
    let url = symphony + '/Sales/LoadCancellations';
    $.ajax({
        type: 'POST',
        data: {
            rbcancellation: rbcancellation, dateFrom: dateFrom, dateTo: dateTo, agentCodeSet: agentCodeSet
        },
        url: url,
        //async: false,
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $(".table-record-display").html(data);
            $("#newtotalCanx").val($("#totalCanx").val());
            $("#newtotalCann").val($("#totalCann").val());
            $("#newtotalCanr").val($("#totalCanr").val());
            $("#newtotalOther").val($("#totalOther").val());
            $("#newtotalDoc").val($("#totalDoc").val());

        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}

function CherchByCodeCancellation(myvar) {
    let url = symphony + "/Sales/CherchByCodeCancellation";
    $.ajax({
        type: "POST",
        url: url,
        data: { value: myvar },
        // dataType: "text",
        success: function (msg) {
            $('.autocomplete').html(msg);
        }, error: function (msg) {
            $('#errorModal').modal('show');
        }
    });

}
function CherchByCodeCancellationFop(myvar, id1, id2) {
    let url = symphony + "/Sales/CherchByCodeCancellationFop";
    $.ajax({
        type: "POST",
        url: url,
        data: { value: myvar, id1: id1, id2, id2 },
        // dataType: "text",
        success: function (msg) {
            $('.autocomplete').html(msg);
        }, error: function (msg) {
            $('#errorModal').modal('show');
        }
    });
}
function putCodeAgent(code, name, city) {
    $('#agent-name-label').html(name)
    $('#agent-code-label').html(code)
    $('#agent-city-label').val(city)
}
function putCodeAgentFop(code, name, city, id1, id2) {
    $('#' + id1).html(name)
    $('#' + id2).html(code)
    //$('#agent-city-label').val(city)
}
function showAgentCodeModal() {
    CherchByCodeCancellation("");
    $('#AgentCodeModal').modal('show');
};

function paxmodal(parentId) {
    $("#" + parentId + " #AgentCodeModal").modal('show');
}
function showAgentCodeModalFop(id, id1, id2) {
    CherchByCodeCancellationFop("", id1, id2);
    $('#' + id).modal('show');
};
/** end **/

/** key controlling data **/
function setNameHeader() {
    let nameHeader = $("#headerName").val();
    let url = symphony + '/Sales/getAllData';
    $.ajax({
        type: 'POST',
        data: { nameHeader: nameHeader },
        url: url,
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $(".keyDataBody").html(data);
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}
/** end **/

/** total psr **/

function showDetails(id, tetxId) {
    var etat = document.getElementById(id).style.display;
    if (etat == 'none') {
        document.getElementById(id).style.display = 'block';
        $("#" + tetxId).text('Hide Add-on Details');
    }
    else {
        document.getElementById(id).style.display = 'none';
        $("#" + tetxId).text('Show Add-on Details');
    }
}
function selectPsd() {
    $("#agentNumCode").val($("#agent-code-label").text());
    $("#agentCodeSet").val($("#agent-code-label").text());

    $("#selectagentNumCode").html("<option>" + $("#agent-code-label").text() + "</option>")
    $("#agentName").val($("#agent-name-label").text());
    $("#agentLocation").val($("#agent-city-label").val());
}
function selectPsd1(id, id1, id2, id3, id4) {
    $("#" + id).val($("#" + id2).text());
    $("#agentCodeSet").val($("#" + id2).text());

    $("#selectagentNumCode").html("<option>" + $("#" + id2).text() + "</option>")
    $("#agentName").val($("#" + id1).text());
    $("#agentLocation").val($("#" + id2).val());
}
function getAllAgentNumCode() {
    let dateFrom = $("#dateFrom").val();
    let dateTo = $("#dateTo").val();
    let url = symphony + "/Sales/getAgentNumCode";
    $("#selection").prop("disabled", true);
    if ($("#testDate").val() != dateFrom) {
        $("#testDate").val(dateFrom);
        $.ajax({
            type: 'POST',
            data: { dateFrom: dateFrom, dateTo: dateTo },
            url: url,
            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {
                $("#selectagentNumCode").html(data);
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

function detailsPassenger(name) {
    let url = symphony + "/Sales/PassengerDetail";
    $("#selection").prop("disabled", true);
    $.ajax({
        type: 'POST',
        data: { name: name },
        url: url,
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#setDetails").html(data);
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}

function getPSR() {
    let dateFrom = $("#dateFrom").val();
    let dateTo = $("#dateTo").val();
    let agentNumCode = $("#selectagentNumCode").val();
    let agentName = $("#agentName").val();
    let agentLocation = $("#agentLocation").val();
    let bookingAgent = $("#bookingAgent").val();
    let reportingEntity = $("#reportingEntity").val();
    let url = symphony + "/Sales/LoadTotalAmountPSR";
    $("#selection").prop("disabled", true);
    $('#PSRSummary').trigger('click');
    $('#PSRKey').val("");
    $('#PSRFare').val("");
    $('#PSRPassenger').val("");
    $.ajax({
        type: 'POST',
        data: { dateFrom: dateFrom, dateTo: dateTo, agentNumCode: agentNumCode, agentName: agentName, agentLocation: agentLocation, bookingAgent: bookingAgent, reportingEntity: reportingEntity },
        url: url,
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#PSRSummary").html(data);
            let lastRow = $("#psrcmptTr").find("tr").length;
            $("#noOfrecords").val(lastRow);
            $("#copteur").val("true");
            $("#copteur1").val("false");
            $("#copteur2").val("false");
            $("#copteur3").val("false");
            for (i = 4; i < 16; i++) {
                $(".cacheCol-" + i).hide();
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
let tmp = "";

function getbookingAgentBSP(nameController, nameAction, id) {
    let dateFrom = $("#dateFrom").val();
    let dateTo = $("#dateTo").val();
    let agentNumCode = $("#selectagentNumCode").val();
    let temoin = false;
    $("#selection").prop("disabled", true);
    if (nameAction == "GetBookingAgent" && agentNumCode != "All") {
        let url = symphony + "/Sales/GetBookingAgent";
        if ($("#booking").val() != agentNumCode) {
            $("#booking").val(agentNumCode);
            $.ajax({
                type: 'POST',
                data: { dateFrom: dateFrom, dateTo: dateTo, agentNumCode: agentNumCode },
                url: url,
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
    }
    if (nameAction == "GetReportingEntity") {
        if ($("#reporting").val() != agentNumCode) {
            let url = symphony + "/Sales/GetReportingEntity";
            $("#reporting").val(agentNumCode);
            $.ajax({
                type: 'POST',
                data: { dateFrom: dateFrom, dateTo: dateTo, agentNumCode: agentNumCode },
                url: url,
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
    }
}

function getPsrKeyElement() {
    let dateFrom = $("#dateFrom").val();
    let dateTo = $("#dateTo").val();
    let agentNumCode = $("#selectagentNumCode").val();
    let agentName = $("#agentName").val();
    let agentLocation = $("#agentLocation").val();
    let bookingAgent = $("#bookingAgent").val();
    let reportingEntity = $("#reportingEntity").val();
    let url = symphony + "/Sales/KeyElement";
    $("#selection").prop("disabled", true);
    if ($("#copteur").val() == "true" && $("#copteur1").val() == "false") {
        $("#copteur1").val("true");
        $.ajax({
            type: 'POST',
            data: { dateFrom: dateFrom, dateTo: dateTo, agentNumCode: agentNumCode, agentName: agentName, agentLocation: agentLocation, bookingAgent: bookingAgent, reportingEntity: reportingEntity },
            url: url,
            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {
                $("#PSRKey").html(data);
                let lastRow = $("#psrcmptTr1").find("tr").length;
                $("#noOfrecords1").val(lastRow);
                for (i = 3; i < 35; i++) {
                    $(".keyelement-" + i).hide();
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

}
function showpsr(id, classe) {
    let idCkeck = "";
    if ($("#" + id).is(':checked')) {
        if (id == "allsumm" || id == "allfaire" || id == "allkey" || id == "allpasse") {
            $('ul.list-group li').each(function (idx, li) {
                idCkeck = $(li).find('input:checkbox').attr('id');
                $("#" + idCkeck).prop("checked", true);

            });
            for (i = 0; i < 33; i++) {
                $('.' + classe + '-' + i).show();
            }
        } else {
            $('.' + classe).show();
        }

    } else {
        if (id == "allsumm" || id == "allfaire" || id == "allkey" || id == "allpasse") {
            $('ul.list-group li').each(function (idx, li) {
                idCkeck = $(li).find('input:checkbox').attr('id');
                $("#" + idCkeck).prop("checked", false);

            });
            for (i = 0; i < 33; i++) {
                $('.' + classe + '-' + i).hide();
            }
        } else {
            $('.' + classe).hide();
        }

    }


    //$('ul.list-group li.list-group-item').find('"input:checkbox').checked()
}
function FareBasisAnalytics() {
    let dateFrom = $("#dateFrom").val();
    let dateTo = $("#dateTo").val();
    let agentNumCode = $("#selectagentNumCode").val();
    let agentName = $("#agentName").val();
    let agentLocation = $("#agentLocation").val();
    let bookingAgent = $("#bookingAgent").val();
    let reportingEntity = $("#reportingEntity").val();
    let selection = $("#selection").val();
    let url = symphony + "/Sales/FareBasisAnalytics";
    $("#selection").prop("disabled", false);
    if (selection == "") {
        $('#alertModal').modal('show');
        $("#msgalert").html("Choose in the selection");
    } else {
        /*if ($("#copteur").val() == "true" && $("#copteur2").val() == "false") {
            $("#copteur2").val("true");*/
        $.ajax({
            type: 'POST',
            data: { dateFrom: dateFrom, dateTo: dateTo, agentNumCode: agentNumCode, agentName: agentName, agentLocation: agentLocation, bookingAgent: bookingAgent, reportingEntity: reportingEntity, selection: selection },
            url: url,
            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {
                $("#PSRFare").html(data);
                let lastRow = $("#psrcmptTr2").find("tr").length;
                $("#noOfrecords2").val(lastRow);
                for (i = 4; i < 29; i++) {
                    $(".farebass-" + i).hide();
                }
            },
            complete: function () {
                $('.ajax-loader').css("visibility", "hidden");
            },
            error: function () {
                $('#errorModal').modal('show');
            }
        });
        //}
    }
}
function ChooseSelection() {

    if ($("#tmpSelection").val() != $("#selection").val()) {
        $("#tmpSelection").val($("#selection").val())
        FareBasisAnalytics();
    }
}
function FarePassenger() {
    let dateFrom = $("#dateFrom").val();
    let dateTo = $("#dateTo").val();
    let agentNumCode = $("#selectagentNumCode").val();
    let url = symphony + "/Sales/PsrPassenger";
    $("#selection").prop("disabled", true);
    if ($("#copteur").val() == "true" && $("#copteur3").val() == "false") {
        $("#copteur3").val("true");
        $.ajax({
            type: 'POST',
            data: { dateFrom: dateFrom, dateTo: dateTo, agentNumCode: agentNumCode },
            url: url,
            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {
                $("#PSRPassenger").html(data);
                let lastRow = $("#psrcmptTr3").find("tr").length;
                $("#noOfrecords3").val(lastRow);
                for (i = 3; i < 18; i++) {
                    $(".passe-" + i).hide();
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

}
/** end **/

/** unused document cpn **/
function setCriteriaUnusedDoc() {
    let dateFrom = $("#dateFromUnsed").val();
    let dateTo = $("#dateToUnsed").val();
    let url = symphony + "/Sales/LoadUnusedDocsCpns";
    if (dateFrom == '' && dateTo == "") {
        $('#alertModal').modal('show');
        $('#msgalert').text('Please select a date range');
    }
    else {
        $.ajax({
            type: 'POST',
            data: { dateFrom: dateFrom, dateTo: dateTo },
            url: url,
            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {
                $(".contentUnused").html(data);
                let lastRow = $("#summUnusedDoc").find("tr").length;
                $("#newtotalCanx").val(lastRow);
                let copteurn = 0;
                $('#summUnusedDoc tr').each(function () {
                    var customerId = $(this).find("td").eq(8).html();
                    copteurn = copteurn + parseFloat(customerId);
                });
                $("#newtotalCann").val(copteurn.toFixed(2));
                $("#newtotalCanr").val(copteurn.toFixed(2));

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
function clearTransactionType() {
    let url = symphony + "/Sales/TransactionType";
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

function clearUnsed() {
    let url = symphony + "/Sales/UnusedDocsCpns";
    $.ajax({
        type: "POST",
        url: "/" + nameController + "/" + nameAction,
        success: function (msg) {
            $('.tab-cancellation').html(msg);
        }, error: function (msg) {
            $('#errorModal').modal('show');
        }
    });
}

/** end **/

/** fop type **/
function setCriteriaFopeType(id, id1, date1, date2, agC, total) {
    let dateFrom = $("#" + date1).val();
    let dateTo = $("#" + date2).val();
    let agentNumCode = $("#" + agC).val();
    let fop = $("#" + id).val();
    let url = symphony + "/Sales/LoadFormOfPayementType";
    $.ajax({
        type: 'POST',
        data: { dateFrom: dateFrom, dateTo: dateTo, agentNumCode: agentNumCode, fop: fop },
        url: url,
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#" + id1).html(data);
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
            let copteurn = 0;
            $('#bodyFormPayment tr').each(function () {

                copteurn++;
            });
            $("#" + total).val(copteurn);
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}
function clearFormFopType() {
    let url = symphony + "/Sales/FormOfPayementType";
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
function clearFOPOther() {
    let url = symphony + "/Sales/FOPOthers";
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
var tmpdate = "";
let tmpCode = "";
function dropDownTransactionType0() {
    let dateFrom = $("#transTypedateFrom").val();
    //console.log(dateFrom);
    let dateTo = $("#transTypedateTo").val();

    let url = symphony + "/Sales/FopTypeTransaction0";
    if (tmpdate != dateFrom) {

        tmpdate = dateFrom;

        $.ajax({
            type: 'POST',
            data: { dateFrom: dateFrom, dateTo: dateTo },
            url: url,
            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {
                $("#dropDownTransactionType").html(data);
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
function dropDownTransactionType(date1, date2, id, agC) {
    //alert(date1);
    let tmpdate = $("#mytemptype").val();
    let tmpCode = $("#mycodetype").val();
    let dateFrom = $("#" + date1).val();
    //alert(datefrom);
    let dateTo = $("#" + date2).val();
    let numCode = $("#" + agC).val();
    let fop = $("#" + id).val();
    let url = symphony + "/Sales/FopTypeTransaction";
    if (tmpdate != dateFrom || tmpCode != numCode) {

        tmpdate = dateFrom;
        tmpCode = numCode;
        $.ajax({
            type: 'POST',
            data: { dateFrom: dateFrom, dateTo: dateTo, agentNumCode: numCode, fop: fop },
            url: url,
            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
                $("#mytemptype").val(tmpdate);
                $("#mycodetype").val(tmpCode);
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

}
function dropDownTransactionTypeOther(date1, date2, id, agC) {
    //alert(date1);
    let dateFrom = $("#" + date1).val();
    //alert(datefrom);
    let dateTo = $("#" + date2).val();
    let numCode = $("#" + agC).val();
    let fop = $("#" + id).val();
    let url = symphony + "/Sales/FopTypeTransactionOther";
    if (tmpdate != dateFrom || tmpCode != numCode) {

        tmpdate = dateFrom;
        tmpCode = numCode;
        $.ajax({
            type: 'POST',
            data: { dateFrom: dateFrom, dateTo: dateTo, agentNumCode: numCode, fop: fop },
            url: url,
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
}
/** end **/

/** exchange **/

function clearExchange() {
    $("#passengerNameSet").val('');
    $("#docNum").val('');
    $("#agentCodeSet").val('');
    $("#DateFrom").val('');
    $("#DateTo").val('');
}
function setCriteriaExchange(event, refine) {
    var $form = $(event).closest('form');
    var $form2 = $(event).closest('markelement');
    let parentId = $form.attr('id');
    let grandParentId = $form2.attr('id');

    let name = $("#" + parentId + " #passengerNameSet").val();
    let docNum = $("#" + parentId + " #docNum").val();
    let agentCode = $("#" + parentId + " #agentCodeSet").val();

    let checkDateTo = $("#" + parentId + " input[name=dtpDateTo]:checkbox:checked").val();
    let checkDateFrom = $("#" + parentId + " input[name=dtpDateFrom]:checkbox:checked").val();

    let dateFrom = $("#" + parentId + " [name=datedtpIssueDateFrom]").val();
    let dateTo = $("#" + parentId + " [name=datedtpIssueDateTo]").val();
    let document = $("#" + parentId + " input:radio[name=document]:checked").val();
    let starting = $("#" + parentId + " input:radio[name=starting]:checked").val();
    let voluntary = $("#" + parentId + " input[name=Voluntary]:checkbox:checked").val();
    let involuntary = $("#" + parentId + " input[name=Involuntary]:checkbox:checked").val();
    let mco = $("#" + parentId + " input[name=MCO]:checkbox:checked").val();
    let emd = $("#" + parentId + " input[name=EDM]:checkbox:checked").val();
    let ebt = $("#" + parentId + " input[name=EBT]:checkbox:checked").val();
    let mpd = $("#" + parentId + " input[name=MPD]:checkbox:checked").val();
    let et = $("#" + parentId + " input[name=ET]:checkbox:checked").val();
    let url = symphony + "/Sales/LoadExchanges";

    let condition = false;
    if (refine != '') {
        if ($("#" + parentId + " #voluntary").is(':checked') || $("#" + parentId + " #involuntary").is(':checked')) {
            if ($("#refine input[name=MCO]:checkbox").is(':checked') || $("#refine input[name=EDM]:checkbox").is(':checked') || $("#refine input[name=EBT]:checkbox").is(':checked') || $("#refine input[name=MPD]:checkbox").is(':checked') || $("#refine input[name=ET]:checkbox").is(':checked')) {
                condition = true;
            } else {
                $('#alertModal').modal('show');
                $("#msgalert").html("Please choose any combination from the Exchange Document Type Box");
            }
        } else {
            $('#alertModal').modal('show');
            $("#msgalert").html("Please choose any combination from the Exchange Document Type Box");
        }

    } else {
        condition = true;
    }
    if (condition == true) {
        $.ajax({
            type: 'POST',
            data: {
                refine: refine, checkDateTo: checkDateTo, checkDateFrom: checkDateFrom, dateFrom: dateFrom, dateTo: dateTo, document: document, name: name, starting: starting, docNum: docNum, agentCode: agentCode,
                voluntary: voluntary, involuntary: involuntary, mco: mco, emd: emd, ebt: ebt, mpd: mpd, et: et
            },
            url: url,
            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {
                $("#" + grandParentId + " .refreshExchange").html(data);
            },
            complete: function () {
                $('.ajax-loader').css("visibility", "hidden");
                let copteurn = 0;
                $("#" + grandParentId + " #compteurRows tr").each(function () {
                    copteurn++;
                });
                console.log(copteurn);
                $("#" + grandParentId + " #comptRow").text('No. Of Records Display: ' + copteurn);
            },
            error: function () {
                $('#errorModal').modal('show');
            }
        });
    }
}

function showMoreDetailsEx(docNum) {
    $("#tab-moredetails").trigger('click');
    let url = symphony + "/Sales/MoreDetailsExchange";
    Ajaxs(docNum, '', 'moreDetails', url);
    $("#orgDocNum").text(docNum);
}
function setExchangeTicket() {
    let datas = $('#ExchangeTicket').attr('class');
    let value = datas.split("-");
    let original = value[0];
    let news = value[1];
    ExchangeTicketDetails(original, news);
}
function setExchangeDetails() {
    let datas = $('#ExchangeDetails').attr('class');
    let value = datas.split("-");
    let original = value[0];
    let news = value[1];
    let transacode = value[2];
    ajout(this, 'PAXTKTs', 'Sales', 'PAX TKTs');
    clickLigneTransaction(null, original, transacode, 'I');
    //clickLigneTransaction(event, docNumber, transactionCode, domInt)
}

function setMenuExchange(id, original, news, transcode) {

    $("#ExchangeTicket").removeClass();
    $("#ExchangeDetails").removeClass();
    $("#ExchangeTicket").addClass(original + '-' + news + '-' + transcode);
    $("#ExchangeDetails").addClass(original + '-' + news + '-' + transcode);
    let tops = $('#' + id).offset().top - 55;
    $("div.menu-dispo").css({ left: $('#' + id).offset().left + 'px', top: tops + 'px' });
    $("#selectDispo").show();

}
window.onclick = function (event) {
    if (event.target.className !== '') {
        $("#selectDispo").hide();
    }
}

function positionnerMenu(cible) {
    $("div.menu-dispo").css({ top: (-1 * (cible.offsetParent.offsetHeight - (cible.offsetTop + cible.offsetHeight))) + 'px' });
    let left = cible.offsetLeft;
    if (cible.offsetParent.offsetWidth < cible.offsetLeft + $("#select-menu-dispo").width()) {
        left -= ((cible.offsetLeft + $("#select-menu-dispo").width()) - cible.offsetParent.offsetWidth) + 14;
    }
    $("div.menu-dispo").css({ left: left + 'px' });
    $(".menu-dispo").toggle();
}

function ExchangeTicketDetails(original, news) {
    let copteurn = 0;
    let copteurn1 = 0;
    let url = symphony + "/Sales/ExchangeTicketDetails";
    $.ajax({
        type: 'POST',
        data: {
            preDocNum: news, docNum: original
        },
        url: url,
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $('#tab-details').trigger('click');
            $("#details").html(data);
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
            $('#originalTicket tr').each(function () {
                if ($(this).find("td").eq(1).html() == "E") {
                    console.log($(this).find("td").eq(6).html().split(',').join('.'));
                    copteurn = copteurn + parseFloat($(this).find("td").eq(6).html().split(',').join('.'));
                }
            });
            if (copteurn == 0) {
                $('#originalTicket tr').each(function () {
                    if ($(this).find("td").eq(1).html() == "S") {
                        copteurn = copteurn + parseFloat($(this).find("td").eq(6).html().split(',').join('.'));
                    }
                });
            }
            $('#newTicket tr').each(function () {
                copteurn1 = copteurn1 + parseFloat($(this).find("td").eq(6).html().split(',').join('.'));
            });


            let cmpt = copteurn - copteurn1;
            if (cmpt > 0) {
                $("#valueAmountCruded").val(cmpt.toFixed(2));
                $("#amountCruded").text("Amount To Be Acruded:");
            }
            else
                if (cmpt < 0) {
                    $("#valueAmountCruded").val(cmpt.toFixed(2));
                    $("#amountCruded").text("Amount To Be Absorbed:");
                }
                else {
                    $("#valueAmountCruded").val(cmpt.toFixed(2));
                    $("#amountCruded").text("Amount To Be Acruded/Absorbed:");
                }

            $("#oldProrateValue").val(copteurn.toFixed(2));
            $("#newProrateValue").val(copteurn1.toFixed(2));
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}

function closeTab(id) {
    $("#" + id).trigger('click');
}


/***************************************surcharges******************************************/
function setCriteriaSurcharges() {
    let dateFrom = $("#dateFromSurcharge").val();
    let dateTo = $("#dateToSurcharge").val();

    let url = symphony + "/Sales/LoadSurcharges";
    let surcharges = $("#dropDownSurcharges").val();
    let dropDownSelctType = $("#dropDownSelctType").val();
    let dropDownSelctDoc = $("#dropDownSelctDoc").val();

    $.ajax({
        type: 'POST',
        data: {
            dateFrom: dateFrom, dateTo: dateTo, surcharges: surcharges, dropDownSelctType: dropDownSelctType, dropDownSelctDoc: dropDownSelctDoc
        },
        url: url,
        //async: false,
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#loadDataSurcharges").html(data);
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}
$(document).ready(function () {
    let today = new Date().toISOString().slice(0, 10);
    let todayLess = new Date(today);
    todayLess.setMonth(todayLess.getMonth() - 1);
    let dateLess = todayLess.toISOString().slice(0, 10).split("-");
    let dateMore = today.split("-");
    $("#dateFrom").val(dateLess[1] + "-" + dateLess[2] + "-" + dateLess[0]);
    $("#dateTo").val(dateMore[1] + "-" + dateMore[2] + "-" + dateMore[0]);
});

function clickDropDownTrans() {
    let dateFrom = $("#dateFrom").val();
    let dateTo = $("#dateTo").val();

    let url = symphony + "/Sales/LoadTransactionCode";

    $.ajax({
        type: 'POST',
        data: {
            dateFrom: dateFrom, dateTo: dateTo
        },
        url: url,
        //async: false,
        beforeSend: function () {
        },
        success: function (data) {
            $("#somethingelse").html(data);
        },
        complete: function () {
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}


$('#dropDownTransactionType').on('click', function () {
    //console.log(this.options[this.selectedIndex].text)
    /*if (tmp == 1) {
        clickDropDownTrans();
        tmp++;
    }*/

});

$('#dropDownSurcharges').on('change', function (e) {
    e.stopImmediatePropagation();
    //console.log(this.options[this.selectedIndex].text)
    /*if (tmp == 1) {
        clickDropDownTrans();
        tmp++;
    }*/

});

function clearTransType() {
    $("#transTypedateFrom").val('');
    $("#transTypedateTo").val('');
    $("#bodySummaryTransType").val('');
    $("#bodyTransType").val('');
    $("#totalTrans").val('');
}
$("#dateFrom").on('change', function () {
    let dateFrom = $("#dateFrom").val();
    let dateTo = $("#dateTo").val();

    let url = symphony + "/Sales/LoadAgDoc";
    $.ajax({
        type: 'POST',
        data: { dateFrom: dateFrom, dateTo: dateTo },
        url: url,
        success: function (data) {
            $('#dropDownSelctDoc').html(data);
        }

    });

});

function showSurchargesCodeModal() {

    CherchSurcharges("");
    $('#SurchargesCodeModal').modal('show');
}


function CherchSurcharges(myvar) {
    let url = symphony + "/Sales/AgentCherch";
    let dateFrom = $("#dateFrom").val();
    let dateTo = $("#dateTo").val();
    $.ajax({
        type: "POST",
        url: url,
        data: { value: myvar, dateFrom: dateFrom, dateTo: dateTo },
        // dataType: "text",
        success: function (msg) {
            $('.agentContent').html(msg);
        }, error: function (msg) {
            $('#errorModal').modal('show');
        }
    });

}

function Select(myVar, myvar1) {
    var value = myVar;
    var value1 = myvar1;
    $('#agent-name-label').html(value1);
    $('#agent-code-label').html(value);
}

function setNumeric() {
    var a = $('#agent-code-label').text();
    $("#dropDownSelctDoc").val($('#agent-code-label').text());
}

function ClearSurcharges() {

    let url = symphony + "/Sales/Surcharges";
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
function ClearVATS() {

    let url = symphony + "/Sales/Vat";
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
/** end **/

/* refund    List Of Refund By Agents  Update Joseph*/
function setCriteriaListRefund() {

    let dateFrom = $("#dateFromListRefund").val();
    let dateTo = $("#dateToListRefund").val();
    let dropdownType = $("#dropdownType").val();
    let dropdownDoc = $("#dropdownDocs").val();

    let url = symphony + "/Sales/LoadListRefund";

    if (dateFrom == "" || dateTo == "") {
        $('#alertModal').modal('show');
        $('#msgalert').text('Please Select a Period Range');

    }else if (($("#dropdownType").val() == "Document Number") && ($("#dropdownDocs").val() == "")) {
        $('#alertModal').modal('show');
        $('#msgalert').text('Please Enter a Document Number');
    }
    else if (($("#dropdownType").val() == "Passenger Name") && ($("#dropdownDocs").val() == "")) {

        $('#alertModal').modal('show');
        $('#msgalert').text('Please Enter a Passenger Name');
    }
    else if (($("#dropdownType").val() == "Agent Numeric Code") && ($("#dropdownDocs").val() == "")) {

        $('#alertModal').modal('show');
        $('#msgalert').text('Please Enter Agent Numeric Code');
    }
    else {
        $.ajax({
            type: "POST",
            url: url,
            data: { dropdownType: dropdownType, dropdownDoc: dropdownDoc, dateFrom: dateFrom, dateTo: dateTo },

            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {
                $("#loadListRefund").html(data);

            },
            complete: function () {
                $('.ajax-loader').css("visibility", "hidden");

                let nbRefun = $("#valListR").val();

                if (nbRefun == 0) {
                    $('#alertModal').modal('show');
                    $('#msgalert').text('No Data.');
                }
            },
        });
    }
}


function boutonvu() {
    if ($("#dropdownType").val() == "Agent Numeric Code") {
        $("#showListRefundCodeModal").show();
    }
    else {
        $("#showListRefundCodeModal").hide();
    }
}

function chercherefn(myvar) {

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
function selectag(myvar, myvar1) {
    $('#agent-name-label').text(myvar1);
    $('#agent-code-label').text(myvar);
}

function setNumericode() {
    let a = $('#agent-code-label').text();
    $("#dropdownDocs").val(a);
}


/* Detail List Of Refund By Agents    update Joseph */

function details(RfnDoc, OrDtIss, OrDcType, DocNum, RfnDt, name, AgtCode, Penalty) {
    $('li').removeClass("active");
    $('#list').removeClass("active");
    $('#tabdetail').addClass("active");
    $('#detail').addClass("active in");
    let url = symphony + "/Sales/DetailsRfn";
    $.ajax({
        type: "POST",
        url: url,
        data: { RfnDoc: RfnDoc, OrDtIss: OrDtIss, OrDcType: OrDcType, DocNum: DocNum, RfnDt: RfnDt, name: name, AgtCode: AgtCode, Penalty: Penalty },
        success: function (msg) {
            $(".tab-rfnddetail").html(msg);
        },
        complete: function () {
            let Original = $("#OriRouting").val();
            if (Original == "") {
                $('#alertModal').modal('show');
                $('#msgalert').text('Cannot find Original Ticket Info.');
            }
        },
        
        error: function (msg) {
            $('#errorModal').modal('show')
        }
    });

}

function checkCpns(myvar) {

    let url = symphony + "/Sales/RfndCoupons";
    $('#RefundCpns').modal('show');
    $.ajax({
        type: "POST",
        url: url,
        data: { DocNum: myvar },
        success: function (msg) {
            $(".content-rfndcpns").html(msg);
        },
        error: function (msg) {
            $('#errorModal').modal('show');
        }
    });
}
function ClearListRefund() {
    let url = symphony + "/Sales/ListofRefunds";
    $.ajax({
        type: "POST",
        url: url,
        success: function (msg) {
            $('.refund-content').html(msg);
        }, error: function (msg) {
            $('#errorModal').modal('show');
        }
    });
}
/*end*/
/**vat**/
$(".div-showModal").on('click', "#showVatCodeModal", function () {
    CherchAgentVat("");
    $('#VatCodeModal').modal('show');
});

function setCriteriaVat() {
    if (($("#dateVatFrom").val() == "") || ($("#dateVatTo").val() == "")) {
        $('#alertModal').modal('show');
        $('#msgalert').text('Please Select a Period Range');
        // alert("Please Select a Period Range");
    }
    else {
        let dateFrom = $("#dateVatFrom").val();
        let dateTo = $("#dateVatTo").val();

        let url = symphony + "/Sales/LoadVat";
        let AgentCode = $('#AgentCode').val();
        $.ajax({
            type: 'POST',
            data: {
                dateFrom: dateFrom, dateTo: dateTo, AgentCode: AgentCode,
            },
            url: url,
            //async: false,
            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {
                $("#loadDataVat").html(data);
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

function ClearVat() {

    let url = symphony + "/Sales/Vat";
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
/*end*/
/*tfcs*/
/*$(".div-showModal").on('click', "#showTfcsCodeModal", function () {
    CherchTfcs("");
    $('#TfcsCodeModal').modal('show');
    console.log('eee');
});*/

function showTfcsCodeModal() {
    CherchTfcs("");
    $('#TfcsCodeModal').modal('show');
}

function CherchTfcs(myvar) {

    let url = symphony + "/Sales/AgentCherchTfcs";
    let dateFrom = $("#dateFromTfcs").val();
    let dateTo = $("#dateToTfcs      ").val();
    $.ajax({
        type: "POST",
        url: url,
        data: { value: myvar, dateFrom: dateFrom, dateTo: dateTo },
        // dataType: "text",
        success: function (msg) {
            $('.agentContent').html(msg);
        }, error: function (msg) {
            $('#errorModal').modal('show');
        }
    });

}
function Select(myVar, myvar1) {
    var value = myVar;
    var value1 = myvar1;
    $('#agent-name-label').html(value1);
    $('#agent-code-label').html(value);
}

function setNumeric() {
    var a = $('#agent-code-label').text();
    $("#dropDownSelctDoc").val($('#agent-code-label').text());
}

function setCriteriaTfcs() {
    let dateFrom = $("#dateFromTfcs").val();
    let dateTo = $("#dateToTfcs").val();

    let url = symphony + "/Sales/LoadTfcs";
    let docType = $("#dropDownSelctTypeTfcs").val();
    let selctDoc = $("#dropDownSelctDoc").val();
    $.ajax({
        type: "POST",
        url: url,
        data: { docType: docType, selectDoc: selctDoc, dateFrom: dateFrom, dateTo: dateTo },
        // dataType: "text",
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#loadDataTfcs").html(data);

        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");

        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });

}

function ClearTfcs() {
    let url = symphony + "/Sales/Tfcs";
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

function loadDocNum(event, myvar, myvar1) {

    ajout(this, 'PAXTKTs', 'Sales', 'PAX TKTs');
    clickLigneTransaction(null, myvar, myvar1);
}
/*end*/

/*******************OAL Refunded***************************/
function setCriteriaOalRefund() {
    let dateFrom = $("#dateFromOalRefund").val();
    let dateTo = $("#dateToOalRefund").val();
    let url = symphony + "/Sales/LoadOalRefund";
    $.ajax({
        type: "POST",
        url: url,
        data: { dateFrom: dateFrom, dateTo: dateTo },
        // dataType: "text",
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#loadOalRefunded").html(data);
            if ($("#checkCompt").val() == 0) {
                $('#alertModal').modal('show');
                $('#msgalert').text('No data for this selected period');
                console.log($("#checkCompt").val());
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

function ClearOalRefund() {
    let url = symphony + "/Sales/OALRefundedCouponsBilling";
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
/********************end**********************************/
/****************OwnRefund******************************/
function setCriteriaOwnRefund() {
    let dateFrom = $("#dateFromRefund").val();
    let dateTo = $("#dateToRefund").val();
    let url = symphony + "/Sales/LoadOwnRefund";
    $.ajax({
        type: "POST",
        url: url,
        data: { dateFrom: dateFrom, dateTo: dateTo },
        // dataType: "text",
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#loadOwnRefunded").html(data);

        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}
/*******************end********************************/



/*****************Refund Engine    Update Joseph ***********************/


/*  Refund Search by Passenger Name (param Starting With or Contains)   or    Document Number Update Joseph*/
function setCriteriaRefundEngine() {
    let pasengerName = $('#valrad input[type=text][name=passenger]').val();
    let docNum = $("#document").val();
    let radContains = $('#valrad input[type=radio][name=radContains]:checked').attr('value');

    if (pasengerName == "" && docNum == "") {
        $('#alertModal').modal('show');
        $('#msgalert').text('Please Enter an Passenger Name or Document Number');

    } else {
        let url = symphony + "/Sales/LoadRefundEngine";
        $.ajax({
            type: "POST",
            url: url,
            data: { pasengerName: pasengerName, docNum: docNum, radContains: radContains },
            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {
                $(".rfnEngDisp").html(data);

            },
            complete: function () {
                $('.ajax-loader').css("visibility", "hidden");

                let nb = $("#valRefun").val();
                if (nb == 0) {
                    $('#alertModal').modal('show');
                    $('#msgalert').text('No Data');
                }
            },
        });
    }
}

function clearRefundEngine() {
    let url = symphony + "/Sales/FrmRefundEngine";
    $.ajax({
        type: "POST",
        url: url,
        success: function (msg) {
            $('.container-refund').html(msg);
        }, error: function (msg) {
            $('#errorModal').modal('show');
        }
    });
}

function loadList() {
    ajout('ListofRefunds', 'Sales', 'ListofRefunds');
}


/*  List Of Manual Refund   Update Joseph */
function manualRefund() {
    $('#manualRefund').modal('show');
    let url = symphony + "/Sales/manualList";
    $.ajax({
        type: "POST",
        url: url,
        success: function (msg) {
            $("#manualRefund").html(msg)
        },
        error: function (msg) {
            $('#errorModal').modal('show');
        }
    });
}

function setCriteriaManualRefund() {
    let dateFrom = $("#dateFrom").val();
    let dateTo = $("#dateTo").val();
    let url = symphony + "/Sales/LoadManualRefund";
    $.ajax({
        type: "POST",
        url: url,
        data: { dateFrom: dateFrom, dateTo: dateTo },

        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $(".manualRefund-content").html(data);

        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");

            let nbManual = $("#valManRef").val();
            if (nbManual == 0) {
                $('#alertModal').modal('show');
                $('#msgalert').text('No Data');
            }
        },
    });
}

/*  End List Of Manual Refund   Update Joseph */

/*  Detail Refund Engine (param document Number)   Update Joseph */

function clickLigneRefundEngine(docNumber) {
    $('#tab-refund').trigger('click');
    console.log(docNumber);
    let url = symphony + "/Sales/RfnEngine";
    let docNum = docNumber;
    $.ajax({
        type: 'POST',
        url: url,
        data: { docNum: docNum },
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#RefundEngine").html(data);

        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}
function shoReasonRfn() {

    if ($("#dropdownRefundType").val() == "VOLUNTARY") {
        $("#voluntaryReson").show();
        $("#involuntaryReson").hide();
        $("#defaultReson").hide();
        $("#voluntarychoiceRfn").show();
        $("#involuntarychoiceRfn").hide();
        $("#defaultchoiceRfn").hide();


    }
    else if ($("#dropdownRefundType").val() == "INVOLUNTARY") {
        $("#involuntaryReson").show();
        $("#voluntaryReson").hide();
        $("#defaultReson").hide();
        $("#voluntarychoiceRfn").hide();
        $("#involuntarychoiceRfn").show();
        $("#defaultchoiceRfn").hide();
    }
    else {
        $("#involuntaryReson").hide();
        $("#voluntaryReson").hide();
        $("#defaultReson").show();
        $("#voluntarychoiceRfn").hide();
        $("#involuntarychoiceRfn").hide();
        $("#defaultchoiceRfn").show();
    }

}

function showfarcurr(cur1, cur2) {
    var a = cur1;
    var b = cur2;
    if ($("#dropdownAmtType").val() == "Fare") {
        $(".curfare").val(a);
        $("#curRfn").val(a);
    }
    else if ($("#dropdownAmtType").val() == "Equivalent Fare") {
        $(".curfare").val(b);
        $("#curRfn").val(b);
    }
    else {
        $(".curfare").val(a);
        $("#curRfn").val(a);
    }
}

function calculRefund(myId, mycheck) {
    var i = 0;
    var totA = parseFloat($("#totA").val().replace(",", "."));
    var temp = $("#reftot").val().replace(",", ".");
    var rfnd = parseFloat(temp);
    var taxe;
    var taxe1;
    if ($("#taxe").val() == "") {
        taxe = 0;
    } else {
        taxe = $("#taxe").val();
    }
    if ($("#totalD").val() == "") {
        taxe1 = 0;
    } else {
        taxe1 = $("#totalD").val();
    }
    console.log(rfnd);
    var temp1 = $("#taxremain").val().replace(",", ".");
    var remain = parseFloat(temp1);
    if ($('#' + mycheck).is(':checked')) {
        //var test1 = $(this).find(myId).eq(3).html().replace(",", ".");
        var test1 = $('#' + myId).text().replace(",", ".");
        var test = parseFloat(test1);
        console.log(test);
        rfnd += test;
        remain -= test;

        console.log(test1);
    }
    else if (!($('#' + mycheck).is(':checked'))) {
        var test = parseFloat($('#' + myId).text().replace(",", "."));
        console.log(test);
        rfnd -= test;
        remain += test;

    }

    var looktot = $("#totfare").val();
    console.log(looktot);
    $("#reftot").val(Math.round(rfnd * 100) / 100);
    $("#taxremain").val(Math.round(remain * 100) / 100);
    var interm = $("#totfare").val();
    console.log(interm);
    // var temp =0
    if ($("#totfare").val() == "") {
        interm = 0;
        console.log(interm);
    }
    var totalB = parseFloat(interm) + parseFloat($("#taxremain").val());
    $("#totalB1").val(totalB);
    $("#totalB").val(totalB);
    //calcultaxe();
    var totBE = parseFloat($("#totalB").val().replace(",", ".")) + parseFloat($("#taxe").val().replace(",", "."));
    //rbt = totA - totBE;
    var difAB = totA - totalB;
    $("#rbt").val(Math.round(rbt * 100) / 100);
    $("#difAB").val(Math.round(difAB * 100) / 100);
    $("#totalC").val(totalB);
    $("#totBE").val(Math.round(totBE * 100) / 100);
    var rbt = totA - totBE;
    $("#rbt").val(Math.round(rbt * 100) / 100);
    var comp1 = parseFloat($("#totalD").val().replace(",", ".")) + parseFloat($("#totalC").val().replace(",", "."));
    var comp2 = parseFloat($("#totalD").val().replace(",", ".")) + parseFloat($("#difAB").val().replace(",", "."));
    console.log(comp1); console.log(comp2);
    if (comp1 > comp2) {
        $("#toteleve").val(Math.round(comp1 * 100) / 100);
    }
    else {
        $("#toteleve").val(Math.round(comp2 * 100) / 100);
    }
    if ($("#dropdownRefundType").val() == "INVOLUNTARY") {
        var totAmt = $("#toteleve").val() * -1;
        $("#totRfnAmt").val(totAmt);
    }
    else if ($("#dropdownRefundType").val() == "VOLUNTARY") {
        var rbtTot = $("#rbt").val() * -1;
        $("#totRfnAmt").val(rbtTot);
    }

}

function checkfare(myID) {
    var type = $("#dropdownAmtType").val();
    var taxe;
    var taxe1;
    if ($("#taxe").val() == "") {
        taxe = 0;
    } else {
        taxe = $("#taxe").val();
    }
    if ($("#totalD").val() == "") {
        taxe1 = 0;
    } else {
        taxe1 = $("#totalD").val();
    }
    var totA = parseFloat($("#totA").val().replace(",", "."));
    $("#taxe").val(taxe);
    if (type == "Fare") {
        if ($("#textFare").val() == "") {
            var textfare = 0;

            var total = parseFloat($("#fare").val().replace(",", ".")) - parseFloat(textfare);
            $("#totfare").val(Math.round(total * 100) / 100);
            var totalB = parseFloat($("#taxremain").val()) + parseFloat($("#totfare").val());
            $("#totalB").val(Math.round(totalB * 100) / 100);
            $("#totalB1").val(Math.round(totalB * 100) / 100);
            var totBE = totalB + taxe;
            $("#difAB").val(Math.round((totA - totalB) * 100) / 100);
            $("#totBE").val(Math.round(totBE * 100) / 100);
            $("#difAB").val(Math.round((totA - totalB) * 100) / 100);
            $("#rbt").val(totA - totBE);
            $("#totalC").val(Math.round(totalB * 100) / 100);
            console.log('taxe1' + taxe1);
            var comp1 = parseFloat($("#totalD").val().replace(",", ".")) + parseFloat($("#totalC").val().replace(",", "."));
            var comp2 = parseFloat($("#totalD").val().replace(",", ".")) + parseFloat($("#difAB").val().replace(",", "."));
            console.log(comp1); console.log(comp2);
            if (comp1 > comp2) {
                $("#toteleve").val(Math.round(comp1 * 100) / 100);
            }
            else {
                $("#toteleve").val(Math.round(comp2 * 100) / 100);
            }
            if ($("#dropdownRefundType").val() == "INVOLUNTARY") {
                var totAmt = $("#toteleve").val() * -1;
                $("#totRfnAmt").val(totAmt);
            }
            else if ($("#dropdownRefundType").val() == "VOLUNTARY") {
                var rbtTot = $("#rbt").val() * -1;
                $("#totRfnAmt").val(rbtTot);
            }

        }
        else {
            var total = parseFloat($("#fare").val().replace(",", ".")) - parseFloat($("#textFare").val());
            $("#totfare").val(total);
            var totalB = parseFloat($("#taxremain").val()) + parseFloat(total);
            $("#totalB").val(Math.round(totalB * 100) / 100);
            $("#totalB1").val(Math.round(totalB * 100) / 100);
            var totBE = totalB + parseFloat(taxe);
            $("#totBE").val(Math.round(totBE * 100) / 100);
            var rbt = totA - totBE;
            $("#difAB").val(Math.round((totA - totalB) * 100) / 100);
            $("#rbt").val(Math.round(rbt * 100) / 100);
            $("#totalC").val(Math.round(totalB * 100) / 100);
            var comp1 = parseFloat($("#totalD").val().replace(",", ".")) + parseFloat($("#totalC").val().replace(",", "."));
            var comp2 = parseFloat($("#totalD").val().replace(",", ".")) + parseFloat($("#difAB").val().replace(",", "."));
            console.log(comp1); console.log(comp2);
            if (comp1 > comp2) {
                $("#toteleve").val(Math.round(comp1 * 100) / 100);
            }
            else {
                $("#toteleve").val(Math.round(comp2 * 100) / 100);
            }
            if ($("#dropdownRefundType").val() == "INVOLUNTARY") {
                var totAmt = $("#toteleve").val() * -1;
                $("#totRfnAmt").val(totAmt);
            }
            else if ($("#dropdownRefundType").val() == "VOLUNTARY") {
                var rbtTot = $("#rbt").val() * -1;
                $("#totRfnAmt").val(rbtTot);
            }
        }
    }


    else if (type == "Equivalent Fare") {
        if ($("#textFare").val() == "") {
            var textfare = 0;
            var total = parseFloat($("#fare").val().replace(",", ".")) - parseFloat(textfare);
            $("#totfare").val(total);
            var totalB = parseFloat($("#taxremain").val()) + parseFloat($("#textFare").val());
            $("#totalB").val(Math.round(totalB * 100) / 100);
            $("#totalB1").val(Math.round(totalB * 100) / 100);
            var totBE = totalB + taxe;
            $("#totBE").val(Math.round(totBE * 100) / 100);
            var rbt = totA - totBE;
            $("#difAB").val(Math.round((totA - totalB) * 100) / 100);
            $("#rbt").val(Math.round(rbt * 100) / 100);
            var comp1 = parseFloat($("#totalD").val().replace(",", ".")) + parseFloat($("#totalC").val().replace(",", "."));
            var comp2 = parseFloat($("#totalD").val().replace(",", ".")) + parseFloat($("#difAB").val().replace(",", "."));
            console.log(comp1); console.log(comp2);
            if (comp1 > comp2) {
                $("#toteleve").val(Math.round(comp1 * 100) / 100);
            }
            else {
                $("#toteleve").val(Math.round(comp2 * 100) / 100);
            }
            if ($("#dropdownRefundType").val() == "INVOLUNTARY") {
                var totAmt = $("#toteleve").val() * -1;
                $("#totRfnAmt").val(totAmt);
            }
            else if ($("#dropdownRefundType").val() == "VOLUNTARY") {
                var rbtTot = $("#rbt").val() * -1;
                $("#totRfnAmt").val(rbtTot);
            }
        }
        else {
            var total = parseFloat($("#fare").val().replace(",", ".")) - parseFloat($("#textFare").val());
            $("#totfare").val(total);
            var totalB = parseFloat($("#taxremain").val()) + parseFloat($("#textFare").val());
            $("#totalB").val(Math.round(totalB * 100) / 100);
            $("#totalB1").val(Math.round(totalB * 100) / 100);
            var totBE = totalB + taxe;
            $("#totBE").val(Math.round(totBE * 100) / 100);
            var rbt = totA - totBE;
            $("#difAB").val(Math.round((totA - totalB) * 100) / 100);
            $("#rbt").val(Math.round(rbt * 100) / 100);
            var comp1 = parseFloat($("#totalD").val().replace(",", ".")) + parseFloat($("#totalC").val().replace(",", "."));
            var comp2 = parseFloat($("#totalD").val().replace(",", ".")) + parseFloat($("#difAB").val().replace(",", "."));
            console.log(comp1); console.log(comp2);
            if (comp1 > comp2) {
                $("#toteleve").val(Math.round(comp1 * 100) / 100);
            }
            else {
                $("#toteleve").val(Math.round(comp2 * 100) / 100);
            }
            if ($("#dropdownRefundType").val() == "INVOLUNTARY") {
                var totAmt = $("#toteleve").val() * -1;
                $("#totRfnAmt").val(totAmt);
            }
            else if ($("#dropdownRefundType").val() == "VOLUNTARY") {
                var rbtTot = $("#rbt").val() * -1;
                $("#totRfnAmt").val(rbtTot);
            }
        }

    }
    else { }
}

function calcultaxe() {
    var comp1 = parseFloat($("#totalD").val().replace(",", ".")) + parseFloat($("#totalC").val().replace(",", "."));
    var comp2 = parseFloat($("#totalD").val().replace(",", ".")) + parseFloat($("#difAB").val().replace(",", "."));
    console.log(comp1); console.log(comp2);
    if (comp1 > comp2) {
        $("#toteleve").val(Math.round(comp1 * 100) / 100);
    }
    else {
        $("#toteleve").val(Math.round(comp2 * 100) / 100);
    }
    var totAmt = $("#toteleve").val() * -1;
    $("#totRfnAmt").val(totAmt);
}

function saveRfn(docNumber, hdrGuidTxt, check, fareCur, eqfareCur, fareAmt, eqfareAmt, dateRfn) {
    let url = symphony + "/Sales/saveRfn";
    let docNum = docNumber;
    let hdrGuid = hdrGuidTxt;
    let checkDigit = check;
    let fareType = $("#dropdownAmtType").val();
    let fareCurr = $(".curfare").val();
    let fareAmount = $("#textFare").val();
    let rfnType = $("#dropdownRefundType").val();
    let totRfnAmt = $("#totRfnAmt").val();
    let curRfn = $("#curRfn").val();
    let fareTypeCur = fareCur;
    let esFareTypeCur = eqfareCur;
    let fareTypeAmt = fareAmt;
    let eqFareTypeAmt = eqfareAmt;
    let fareRemain = $("#totfare").val();
    let totalRfn = $("#totalTax").val();
    let totalTaxe = $("#reftot").val();
    let taxeRemain = $("#taxremain").val();
    let observation = $("#observation").text();
    let volReason = $("#voluntaryReson").val();
    let InvReason = $("#involuntaryReson").val();
    let refundBy = $("#refundBy").val();
    let totalD = $("#totalD").val();
    let taxe = $("#taxe").val();
    let dateOfRfn = dateRfn;

    var tabother = [];
    var i = 0
    $("#otherTax tr").each(function () {
        if ($('#check_' + i).is(':checked')) {
            var element = $(this).find('td').eq(0).text() + ',' + $(this).find('td').eq(1).text() + ',' + $(this).find('td').eq(3).text() + ',' + $(this).find('td').eq(5).text() + ',' + $(this).find('td').eq(6).text();
            tabother.push(element);

        }
    });
    console.log(tabother);
    console.log(rfnType)
    $.ajax({
        type: 'POST',
        url: url,
        data: { docNum: docNum, hdrGuid: hdrGuid, checkDigit: checkDigit, fareType: fareType, fareCurr: fareCurr, fareAmount: fareAmount, rfnType: rfnType, curRfn: curRfn, totRfnAmt: totRfnAmt, fareTypeCur: fareTypeCur, esFareTypeCur: esFareTypeCur, fareTypeAmt: fareTypeAmt, eqFareTypeAmt: eqFareTypeAmt, fareRemain: fareRemain, totalTaxe: totalTaxe, totalRfn: totalRfn, taxeRemain: taxeRemain, observation: observation, volReason: volReason, InvReason: InvReason, refundBy: refundBy, totalD: totalD, taxe: taxe, tabother: tabother, dateOfRfn: dateOfRfn },
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $('#alertModal').modal('show');
            $('#msgalert').text(data);

        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
        },
        error: function () {
            $('#errorModal').modal('show');
            $('#errormsg').text('Error While saving Record');
        }
    });
}
/**********************end****************************/



/*  fim  */
function showFlightsLookup() {
    $("#flightsLookup").modal('hide');
    $('#showFlightsLookup').modal('show');
}

function showFimDetail(docNum) {
    let url = symphony + "/Sales/LoadFimDetailData";
    Ajaxs(docNum, '', 'loadFim', url);
}

function chercheFlightLookup(flight) {
    let url = symphony + "/Sales/FlightLookup";
    Ajaxs(flight, '', 'dataFlight', url);
}
function chercheAirportCode(code) {
    let url = symphony + "/Sales/AiroportCode";
    Ajaxs(code, '', 'dataAirportCode', url);
}
function selectairportCode() {
    let airportCode = $("#airoportCode").val();
    let nb = $("#nb").val();
    if (nb == '1') {
        $("#placeInterruption").val(airportCode);
        $("#fromInterruption").val(airportCode);
        $("#routed1").val(airportCode);

    }
    if (nb == '2') {
        $("#toInterruption").val(airportCode);
        $("#routed2").val(airportCode);
    }
    if (nb == '3') {
        $("#routed3").val(airportCode);
        $("#fromInterruption3").val($("#routed2").val());
        $("#toInterruption3").val(airportCode);
    }
}
function setAirportCode(airportCode) {
    $("#airoportCode").val(airportCode);
    $("#valueAirportCode").text(airportCode);
}

function setFlightLookup(airline, flightNumb, flightDate, from, to) {
    $("#valueFlightLook").text(airline + ' | Flight Date: ' + flightDate + ' | Flight Number : ' + flightNumb);
    $("#flightNumb").val(flightNumb);
    $("#flightDate").val(flightDate);
    $("#flightfrom").val(from);
    $("#flightto").val(to);
}

function setPassengerLookup(passName, cn, cpn, docNum, checkDigit, DocType, fareBasis, date, flight, airline) {

    $("#valuePassengerLookup").val(passName + ',' + cn + ',' + cpn + ',' + docNum + ',' + checkDigit + ',' + DocType + ',' + fareBasis + ',' + date + ',' + flight + ',' + airline);
    $("#valuelabelpassenger").text(docNum + ' | Cpn : ' + cn + ' | Passenger Name : ' + passName);
}

function selectPassengerLookup() {
    let tab = $("#valuePassengerLookup").val().split(',');
    let tr = '<tr>' +
                '<td>1</td>' +
                '<td>' + tab[0] + '</td>' +
                '<td>' + tab[1] + '</td>' +
                '<td>' + tab[2] + '</td>' +
                '<td>' + tab[3].substring(3) + '</td>' +
                '<td>' + tab[4] + '</td>' +
                '<td>' + tab[5] + '</td>' +
                '<td>' + tab[6] + '</td>' +
                '<td></td>' +
                '<td></td>' +
                '<td></td>' +
                '<td></td>' +
                '<td><input type="checkbox" name="difference[]" value="1"/></td></td>' +
                '</tr>';
    $("#airline1").val(tab[9]);
    $("#airline").val(tab[9]);
    $("#flight").val(tab[8]);
    $("#dateFlight").val(tab[7]);
    $("#loadFimHeader").html(tr);
    $("#flightsLookup").modal('hide');
    $("#totalNumberPassenger").val("1");
    clearFim();
}

function selectAllPassengerLookup() {
    var tab = [];
    $("#loadBodyPassLookup tr").each(function () {
        tab.push($(this).find("td").eq(2).html() + ',' + $(this).find("td").eq(1).html() + ',' + $(this).find("td").eq(13).html() + ',' + $(this).find("td").eq(0).html().substring(3) + ',' + $(this).find("td").eq(12).html() + ',' + $(this).find("td").eq(17).html() + ',' + $(this).find("td").eq(16).html() + ',' + $(this).find("td").eq(5).html() + ',' + $(this).find("td").eq(6).html() + ',' + $(this).find("td").eq(15).html());
    });

    let tr = "";
    let sonTab = [];
    let i = 1;
    let cntRow = 0;
    tab.forEach(function (element) {
        sonTab = element.split(',');
        cntRow = i;
        tr += '<tr>' +
                '<td>' + i + '</td>' +
                '<td>' + sonTab[0] + '</td>' +
                '<td>' + sonTab[1] + '</td>' +
                '<td>' + sonTab[2] + '</td>' +
                '<td>' + sonTab[3].substring(3) + '</td>' +
                '<td>' + sonTab[4] + '</td>' +
                '<td>' + sonTab[5] + '</td>' +
                '<td>' + sonTab[6] + '</td>' +
                '<td></td>' +
                '<td></td>' +
                '<td></td>' +
                '<td></td>' +
                '<td><input type="checkbox" name="difference[]" value="1"/></td>' +
                '</tr>';
        $("#airline").val(sonTab[9]);
        $("#airline1").val(sonTab[9]);
        $("#flight").val(sonTab[8]);
        $("#dateFlight").val(sonTab[7]);
        i++;

    });
    $("#totalNumberPassenger").val(cntRow);
    $("#loadFimHeader").html(tr);
    $("#flightsLookup").modal('hide');
    clearFim();
}
function clearFim() {
    $("#routed1").val('');
    $("#1airline").val('');
    $("#1flight").val('');
    $("#1dateFlight").val('');
    $("#routed2").val('');
    $("#2airline").val('');
    $("#2flight").val('');
    $("#2dateFlight").val('');
    $("#routed3").val('');
    $("#placeInterruption").val('');
    $("#fromInterruption").val('');
    $("#toInterruption").val('');
    $("#fromInterruption3").val('');
    $("#toInterruption3").val('');
    $("#validatorName").val('');
    $("#sector").val('');
    $("#fimNumber").val('');
}

function selectflightLook() {

    let flightNo = $("#flightNumb").val();
    let flightDate = $("#flightDate").val();
    let flightFrom = $("#flightfrom").val();
    let flightTo = $("#flightto").val();
    var tab = [flightNo, flightDate, flightFrom, flightTo];
    let url = symphony + "/Sales/PassengerLookup";
    Ajaxs(tab, '', 'dataPassengerLookup', url);
    $('#showFlightsLookup').modal('hide');
    $("#flightsLookup").modal('show');
    $("#valueFlightNo").text(flightNo);
    $("#valueFlightDate").text(flightDate);
    $("#valueFlightFrom").text(flightFrom);
    $("#valueFlightTo").text(flightTo);

    $("#valueFlightNo1").val(flightNo);
    $("#valueFlightDate1").val(flightDate);
    $("#Flightsector").val(flightFrom + flightTo);
}

function showModalAirportCode(nb) {
    $("#nb").val(nb);
    $("#showAirportCode").modal('show');
}

function changeVlaue(value, rang) {
    if (rang == "1") {
        $("#placeInterruption").val(value);
        $("#fromInterruption").val(value);
    }
    if (rang == '2') {
        $("#toInterruption").val(value);
    }
    if (rang == '3') {
        console.log(value);
        if (value == "" || $("#routed2").val() == "") {
            $("#fromInterruption3").val("");
        } else {
            $("#fromInterruption3").val($("#routed2").val());
            $("#toInterruption3").val(value);
        }
    }
}

function SaveFIMHeaderData() {
    var tab = [];
    var tab1 = [];
    if ($("#fimNumber").val() == "") {
        $('#alertModal').modal('show');
        $("#msgalert").html("Enter the FIM Number");
    } else {
        let ownOal = $("#ownOal input:radio[name=ownOal]:checked").val();
        let fimNumber = $("#fimNumber").val();

        let RoutedFrom = $("#routed1").val();
        let AirlineCode = $("#airline").val();
        let RoutedFromFlight = $("#flight").val();
        let DateRoutedFrom = $("#dateFlight").val();
        let RoutedFromPlaceOfInterruption = $("#fromInterruption").val();

        let ReRoutedTo1 = $("#routed1").val();
        let ReroutedAirline1 = $("#1airline").val();
        let ReRoute1Flight = $("#1flight").val();
        let DateRoutedTo1 = $("#1dateFlight").val();
        let ReRoute1From = $("#fromInterruption").val();
        let ReRoute1To = $("#toInterruption").val();

        let ReRoutedTo2 = ReRoute1To;
        let ReroutedAirline2 = $("#2airline").val();
        let ReRoute2Flight = $("#2flight").val();
        let DateRoutedto2 = $("#2dateFlight").val();
        let ReRoute2From = $("#fromInterruption3").val();
        let ReRoute2To = $("#toInterruption3").val();
        let reasonIssuance = $("#reasonIssuance input:radio[name=reasonIssuance]:checked").val();
        let DiversionCarrCode = $("#airline1").val();
        let TotalNoOfPax = $("#totalNumberPassenger").val();
        let ValidatorName = $("#validatorName").val();
        let OriginalSector = $("#sector").val();
        tab = [ownOal, fimNumber, RoutedFrom, AirlineCode, RoutedFromFlight, DateRoutedFrom, RoutedFromPlaceOfInterruption, ReRoutedTo1, ReroutedAirline1, ReRoute1Flight, DateRoutedTo1, ReRoute1From, ReRoute1To, ReRoutedTo2, ReroutedAirline2,
            ReRoute2Flight, DateRoutedto2, ReRoute2From, ReRoute2To, reasonIssuance, DiversionCarrCode, TotalNoOfPax, ValidatorName, OriginalSector];
        let url = symphony + "/Sales/SaveFIMHeaderData";

        let i = 0
        let douz = 0;
        $("#loadFimHeader tr").each(function () {
            if ($(this).find("td:eq(12) input").is(':checked') == true) {
                douz = 1;
            } else {
                douz = 0;
            }
            tab1.push($(this).find("td").eq(0).html() + ',' + $(this).find("td").eq(1).html() + ',' + $(this).find("td").eq(2).html() + ',' + $(this).find("td").eq(3).html() + ',' +
                $(this).find("td").eq(4).html() + ',' + $(this).find("td").eq(5).html() + ',' + $(this).find("td").eq(6).html() + ',' + $(this).find("td").eq(7).html() + ',' +
                $(this).find("td").eq(8).html() + ',' + $(this).find("td").eq(9).html() + ',' + $(this).find("td").eq(10).html() + ',' + $(this).find("td").eq(11).html() + ',' + douz
            );
        })
        Ajaxs(tab, tab1, 'success', url);
    }
}

function closeFimManager() {
    $("#flightsLookup").modal('show');
}
/* passenger agency details */
function getPassengerAgencyDetail() {
    let name = $("#passengerNameSet").val();
    let starting = $("#passName input:radio[name=radContains]:checked").val();
    let agentCodeSet = $("#agentCodeSet").val();
    var tab = [name, starting, agentCodeSet];
    let url = symphony + "/Sales/PassengerMan";
    Ajaxs(tab, '', 'body-passengerAgency', url);
}

function getPassDetailAgency(numCode) {
    let url = symphony + "/Sales/LoadPassengerMan";
    Ajaxs(numCode, '', 'load-PassengerAgency', url);

    let url1 = symphony + "/Sales/addEdditDetails";
    Ajaxs(numCode, '', 'load-PassengerAgency1', url1);
}

function refreshRenumerate() {
    let numCode = $("#agentCode").val();
    let url1 = symphony + "/Sales/addEdditDetails";
    Ajaxs(numCode, '', 'load-PassengerAgency1', url1);
}

function clearPassengerDetails() {
    $("#remark").val('');
    $("#dateOpp").val('');
    $("#location").val('');
    $("#bsps").val('');
    $("#address").val('');
    $("#name").val('');
    $("#agentCode").val('');
    $("#status").val('');
    $("#category").val('');
}

function addEdditDetails() {
    let agentCodeSet = $("#agentCode").val();
    if (agentCodeSet != "") {
        $("#agenNumericCode").text(agentCodeSet);
        let url = symphony + "/Sales/addEdditDetails";
        Ajaxs(agentCodeSet, '', 'load-body-newRenum', url);
        $('#renumeration').modal('show');
    }

}
function newAgencyDetails() {
    let dateFrom = $("#dateFrom").val();
    let dateTo = $("#dateTo").val();
    var tab = [dateFrom, dateTo];
    let url = symphony + "/Sales/ListNewAgents";
    Ajaxs(tab, '', 'body-newagency', url);
}

function accepte() {
    let typeRemuneration = $("#typeRemuneration").val();
    let rateLevels = $("#rateLevels").val();
    let rateLevel = $("#rateLevel").val();
    let applicability = $("#applicability").val();
    let dateFrom = $("#dateFrom1").val();
    let dateTo = $("#dateTo1").val();
    tmp = false;

    if (typeRemuneration == "") {
        $('#alertModal').modal('show');
        $("#msgalert").html("Types of Renumeration is compulsory");
    }
    else if (rateLevels == "") {
        $('#alertModal').modal('show');
        $("#msgalert").html("Percentage / Amount is compulsory");
    }
    else if (rateLevel == "") {
        $('#alertModal').modal('show');
        $("#msgalert").html("Rate / Level is compulsory");
    }
    else if (applicability == "") {
        $('#alertModal').modal('show');
        $("#msgalert").html("Applicability is compulsory");
    }
    else if (dateFrom == "") {
        $('#alertModal').modal('show');
        $("#msgalert").html("Period From is compulsory");
    }
    else if (dateTo == "") {
        $('#alertModal').modal('show');
        $("#msgalert").html("Period To is compulsory");
    }

    $(".load-body-newRenum tr").each(function () {
        if (typeRemuneration == $(this).find("td").eq(0).html() && rateLevels == $(this).find("td").eq(1).html() && rateLevel == $(this).find("td").eq(2).html() && applicability == $(this).find("td").eq(3).html() && dateFrom == $(this).find("td").eq(4).html() && dateTo == $(this).find("td").eq(5).html()) {
            tmp = true;
        }
    })
    if (tmp) {
        $('#alertModal').modal('show');
        $("#msgalert").html("Duplicate Entry Cant be inserted into Database");
    } else {
        if (typeRemuneration != "" || rateLevels != "" || rateLevel != "" || applicability != "" || dateFrom != "" || dateTo != "") {
            $(".load-body-newRenum").append('<tr>' +
                '<td>' + typeRemuneration + '</td>' +
                '<td>' + rateLevels + '</td>' +
                '<td>' + rateLevel + '</td>' +
                '<td>' + applicability + '</td>' +
                '<td>' + dateFrom + '</td>' +
                '<td>' + dateTo + '</td>' +
                '</tr>');
        }
    }

}

function clearRemuneration() {
    $("#typeRemuneration").val('');
    $("#rateLevels").val('');
    $("#rateLevel").val('');
    $("#applicability").val('');
    $("#dateFrom1").val('');
    $("#dateTo1").val('');
}

function SavePassengerAgency() {
    if ($("#agentCode").val() != "") {
        tab = [$("#remark").val(), $("#dateOpp").val(), $("#location").val(), $("#bsps").val(), $("#address").val(), $("#name").val(), $("#agentCode").val(), $("#status").val(), $("#category").val()];
        let url = symphony + "/Sales/SaveNewPassenger";
        Ajaxs(tab, '', 'success', url);
    }
}

function DeletesingleClick() {
    let agentCode = $("#agentCode").val();
    if (agentCode != "") {
        $('#alertModal').modal('show');
        $('div.modal-confirmation').html('<button type="button" class="btn btn-default" style="position: relative;top: 12px;" onclick="confimDelete()">OK</button>' +
            '<button type="button" class="Close btn btn-secondary btn-default" style="background-color:#e8ae72;" data-dismiss="modal">Close</button>');
        $("#msgalert").html("Are you sure you want to delete the following Agent:" + agentCode);
    }
}

function confimDelete() {
    $('#alertModal').modal('hide');
    let tab = $("#agentCode").val();
    let url = symphony + "/Sales/DeletesingleClick";
    Ajaxs(tab, '', 'success', url);
    refreshRenumerate();
    clearPassengerDetails()
}

function SaveAllRenumerate(method) {

    let url = symphony + "/Sales/" + method;
    let tab = [];
    let tab1 = $("#agentCode").val();
    $("#load-body-newRenum tr").each(function () {
        tab.push($(this).find("td").eq(0).html() + '$' + $(this).find("td").eq(1).html() + '$' + $(this).find("td").eq(2).html() + '$' + $(this).find("td").eq(3).html() + '$' + $(this).find("td").eq(4).html() + '$' + $(this).find("td").eq(5).html());
    });
    Ajaxs(tab, tab1, 'success', url);
}

function SelectSingleRenumerate(un, deux, trois, quatre, cinq, six) {
    $("#typeRemuneration").val(un);
    if (deux == 0) {
        $("#rateLevels").val('Amount');
    } else {
        $("#rateLevels").val('Percentage');
    }
    $("#rateLevel").val(trois);
    $("#applicability").val(quatre);
    $("#dateFrom1").val(cinq);
    $("#dateTo1").val(six);
}

function DeleteSingleRenumerate() {
    let tmp = false;
    let tab = [];
    $("#load-body-newRenum tr").each(function () {
        if ($(this).find("td").eq(0).html() == $("#typeRemuneration").val() && $(this).find("td").eq(1).html() == $("#rateLevels").val() && $(this).find("td").eq(4).html() == $("#dateFrom1").val() && $(this).find("td").eq(5).html() == $("#dateTo1").val()) {
            tmp = true;
            let dateFrom;
            let dateTo;
            let typeRemuneration;
        }
    });
    /***alert ***/
    if (tmp) {
        $('#alertModal').modal('show');
        $('div.modal-confirmation').html('<button type="button" class="btn btn-default" style="position: relative;top: 12px;" data-dismiss="modal" onclick="confimSingleDelete()">OK</button>' +
                '<button type="button" class="Close btn btn-secondary btn-default" style="background-color:#e8ae72;" data-dismiss="modal">Close</button>');
        $("#msgalert").html("Are you sure you want to delete this record:");
    }

}

function confimSingleDelete() {
    tab = [$("#agentCode").val(), $("#typeRemuneration").val(), $("#dateFrom1").val(), $("#dateTo1").val()];
    let url = symphony + "/Sales/Deletesingle";
    Ajaxs(tab, '', 'success', url);
    addEdditDetails();
    getPassDetailAgency($("#agentCode").val());

}
function getDetailNewAgency(docNum) {
    $("#agentNumCode").text(docNum)
    $("#dateOfIssTo").text($("#dateTo").val())
    $("#dateOfIssFrom").text($("#dateFrom").val())
    tab = [docNum, $("#dateFrom").val(), $("#dateTo").val()];
    let url = symphony + "/Sales/ClickListNewAgent";
    $("#ListOfNewAgent").modal('show');
    Ajaxs(tab, '', 'load-lisOfNewAgent', url);
}

function setRenumerateDetails(data) {
    $("#ListOfNewAgent").modal('hide');
    let value = data.split("-");
    let original = value[0];
    let transacode = value[1];
    ajout('PAXTKTs', 'Sales', 'PAX TKTs');
    clickLigneTransaction(original, 'TKTT');

}

/**************industry*******************/
function setNew() {
    $("#bonusAc, #headFinancial, #dateFrom, #dateTo, #bonusBase, #bonusPayable, #bonusCur, #plbrange, #plbper").prop("readonly", false);
    $("#add").removeClass("btn2");
    $("#txtadd").removeClass("search-span1");
    $("#save").removeClass("btn2");
    $("#txtsave").removeClass("search-span1");
    $("#edit1").removeClass("btn1");
    $("#delete1").removeClass("btn1");
    $("#txtedit1").removeClass("search-span");
    $("#txtdelete1").removeClass("search-span");
    $("#edit").removeClass("btn1");
    $("#delete").removeClass("btn1");
    $("#txtedit").removeClass("search-span");
    $("#txtdelete").removeClass("search-span");
    $("#add").addClass("btn1");
    $("#save").addClass("btn1");
    $("#txtsave").addClass("search-span");
    $("#txtadd").addClass("search-span");
    $("#edit1").addClass("btn2");
    $("#delete1").addClass("btn2");
    $("#txtedit1").addClass("search-span1");
    $("#txtdelete1").addClass("search-span1");
    $("#edit").addClass("btn2");
    $("#delete").addClass("btn2");
    $("#txtedit").addClass("search-span1");
    $("#txtdelete").addClass("search-span1");

}

function setEdit() {
    if ($("#edit").hasClass("btn1")) {
        $("#bonusAc, #headFinancial, #dateFrom, #dateTo, #bonusBase, #bonusPayable, #bonusCur").prop("readonly", false);
        $("#save").text("Update");
        $("#save").removeClass("btn2");
        $("#save").addClass("btn1");
    }
    else { }
}

function setEditNew() {
    $("#plbrange, #plbper").prop("readonly", false);
    if ($("#edit1").hasClass("btn1")) {

        $("#add1").text("Update");
        $("#add1").removeClass("btn2");
        $("#add1").addClass("btn1");
    }
    else { }
}

function setCriteriaCom() {
    let agCode = $("#agentCode").val();
    let dateFrom = $("#dateFromIndustryCom").val();
    let dateTo = $("#dateToIndustryCom").val();
    let url = symphony + "/Sales/LoadCom";
    $.ajax({
        type: "POST",
        url: url,
        data: { dateFrom: dateFrom, dateTo: dateTo, agCode: agCode },

        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#loadCom").html(data);
            if ($("#Document").val() == 0) {
                $('#alertModal').modal('show');
                $('#msgalert').text('No data for this selected period');
            }
            console.log(data);
            console.log($("#Document").val());
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}

function setAgCode() {
    let dateFrom = $("#dateFromIndustryCom").val();
    let dateTo = $("#dateToIndustryCom").val();
    let url = symphony + "/Sales/getAgCode";
    $("#selection").prop("disabled", true);
    if ($("#testDate").val() != dateFrom) {
        $("#testDate").val(dateFrom);
        $.ajax({
            type: 'POST',
            data: { dateFrom: dateFrom, dateTo: dateTo },
            url: url,
            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {
                $("#agentCode").html(data);
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

function setFinP() {
    let agCode = $("#BonusAgent").val();
    let url = symphony + "/Sales/getFinP";
    if ($("#testfp").val() != agCode) {
        $("#testfp").val(agCode);
        $.ajax({
            type: 'POST',
            url: url,
            data: { agCode: agCode },
            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {
                $("#FinancialPeriod").html(data);
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
function setCriteriaBonus() {
    let agCode = $("#BonusAgent").val()
    let finYear = $("#FinancialPeriod").val();
    let url = symphony + "/Sales/loadBonus";
    $.ajax({
        type: 'POST',
        url: url,
        data: { ag: agCode, yr: finYear },
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#displayBonus").html(data);
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

function setAdd() {
    if ($("#add").hasClass("btn1")) {
        if ($("#bonusAc").val() == "") {
            $('#alertModal').modal('show');
            $('#msgalert').text('Agent Numeric Code is compulsory');
        }
        else if ($("#headFinancial").val() == "") {
            $('#alertModal').modal('show');
            $('#msgalert').text('Financial Year is compulsory');
        }
        else if ($("#dateFrom").val() == "") {
            $('#alertModal').modal('show');
            $('#msgalert').text('Start of Financial Year is compulsory');
        }
        else if ($("#dateTo").val() == "") {
            $('#alertModal').modal('show');
            $('#msgalert').text('Financial Year is compulsory');
        }
        else if ($("#bonusBase").val() == "") {
            $('#alertModal').modal('show');
            $('#msgalert').text('Financial Year is compulsory');
        }
        else if ($("#bonusPayable").val() == "") {
            $('#alertModal').modal('show');
            $('#msgalert').text('Financial Year is compulsory');
        }
        else if ($("#bonusCur").val() == "") {
            $('#alertModal').modal('show');
            $('#msgalert').text('Financial Year is compulsory');
        }
        else if ($("#bonusPayable").val() == "") {
            $('#alertModal').modal('show');
            $('#msgalert').text('Financial Year is compulsory');
        }

        if ($("#bonusAc").val() != "") {
            let agCode = $("#bonusAc").val();
            let finYear = $("#headFinancial").val();
            let dateFrom = $("#dateFrom1").val();
            let dateTo = $("#dateTo1").val();
            let bonusBase = $("#bonusPayable").val();
            let bonusCur = $("#bonusCur").val();
            let bonusPayable = $("#bonusPayable").val();
            let url = symphony + "/Sales/addBonus";
            let BonusAgent = $("#BonusAgent").val();
            let FinancialPeriod = $("#FinancialPeriod").val();
            $.ajax({
                type: 'POST',
                url: url,
                data: { agCode: agCode, finYear: finYear, dateFrom: dateFrom, dateTo: dateTo, bonusBase: bonusBase, bonusCur: bonusCur, bonusPayable: bonusPayable, ag: BonusAgent, yr: FinancialPeriod },
                beforeSend: function () {
                    $('.ajax-loader').css("visibility", "visible");
                },
                success: function (data) {
                    $("#headBonus").html(data);
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
}

function saveHeader() {
    let action = $("#save").text();
    let agCode = $("#bonusAc").val();
    let finYear = $("#headFinancial").val();
    let dateFrom = $("#dateFrom1").val();
    let dateTo = $("#dateTo1").val();
    let bonusBase = $("#bonusPayable").val();
    let bonusCur = $("#bonusCur").val();
    let bonusPayable = $("#bonusPayable").val();
    let BonusAgent = $("#BonusAgent").val();
    let FinancialPeriod = $("#FinancialPeriod").val();
    let url = symphony + "/Sales/saveOperation";
    $.ajax({
        type: 'POST',
        url: url,
        data: { agCode: agCode, finYear: finYear, dateFrom: dateFrom, dateTo: dateTo, bonusBase: bonusBase, bonusCur: bonusCur, bonusPayable: bonusPayable, action: action, ag: BonusAgent, yr: FinancialPeriod },
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#displayBonus").html(data);
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

function addNew() {
    if ($("#add").hasClass("btn1")) {
        if ($("#plbrange").val() == "") {
            $('#alertModal').modal('show');
            $('#msgalert').text('PLBRange is compulsory');
        }
        else if ($("#plbper").val() == "") {
            $('#alertModal').modal('show');
            $('#msgalert').text('PLBPercentage is compulsory');
        }
        if ($("#plbrange").val() != "") {
            let agCode = $("#bonusAc").val();
            let finYear = $("#headFinancial").val();
            let plbrange = $("#plbrange").val();
            let plbper = $("#plbper").val();
            let url = symphony + "/Sales/saveOperation2";
            let action = $("#add1").text();
            $.ajax({
                type: 'POST',
                url: url,
                data: { agCode: agCode, finYear: finYear, plbrange: plbrange, plbper: plbper, action: action },
                beforeSend: function () {
                    $('.ajax-loader').css("visibility", "visible");
                },
                success: function (data) {
                    $("#details").html(data);

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
    else {
        $('#alertModal').modal('show');
        $('#msgalert').text('Record didin\'t save succesfully');
    }

}

function showDetail(myId) {
    $("#" + myId).css("backgroundColor", "rgb(0, 120,215)");
    var agcode;
    var finYr;
    var datefrom;
    var dateto;
    var payable;
    var cur;
    var amount;
    $("#" + myId).each(function () {
        agcode = $(this).find('td').eq(0).text();
        finYr = $(this).find('td').eq(1).text();
        datefrom = $(this).find('td').eq(2).text();
        dateto = $(this).find('td').eq(3).text();
        payable = $(this).find('td').eq(4).text();
        amount = $(this).find('td').eq(7).text();
        cur = $(this).find('td').eq(6).text();
    });
    $("#dateFrom1").val(datefrom);
    $("#bonusAc").val(agcode);
    $("#headFinancial").val(finYr);
    $("#dateTo1").val(dateto);
    $("#bonusBase").val(amount);
    $("#bonusPayable").val(payable);
    $("#bonusCur").val(cur);
    let url = symphony + "/Sales/showDetail";
    $.ajax({
        type: 'POST',
        url: url,
        data: { agcode: agcode },
        success: function (data) {
            $("#details").html(data);
            console.log(data);
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
    console.log(agcode + ' , ' + cur);
}

function selectDetail(myId) {
    $("#" + myId).css("backgroundColor", "rgb(0, 120,215)");
    $("#" + myId).each(function () {
        range = $(this).find('td').eq(0).text();
        percent = $(this).find('td').eq(1).text();
        $("#plbrange").val(range);
        $("#plbper").val(percent);
    });
}

function deleteDetail() {
    var agcode = $("#bonusAc").val();
    var finYr = $("#headFinancial").val();
    var range = $("#plbrange").val();
    var percent = $("#plbper").val();
    let url = symphony + "/Sales/deleteDetail";
    $.ajax({
        type: 'POST',
        url: url,
        data: { agcode: agcode, finYr: finYr, range: range, percent: percent },
        success: function (data) {
            $("#details").html(data);
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
function deleteHead() {
    var ag = $("#BonusAgent").val();
    var yr = $("#FinancialPeriod").val();
    var agcode = $("#bonusAc").val();
    var finYr = $("#headFinancial").val();
    let url = symphony + "/Sales/deleteHead";
    $.ajax({
        type: 'POST',
        url: url,
        data: { ag: ag, yr: yr, agcode: agcode, finYr: finYr, },
        success: function (data) {
            $("#headBonus").html(data);
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}
function showMydet(myId) {
    $("#" + myId).css("backgroundColor", "rgb(0, 120,215)");
    $("#" + myId).each(function () {
        agcode = $(this).find('td').eq(0).text();
        finYear = $(this).find('td').eq(1).text();
        seq = $(this).find('td').eq(2).text();
        pblrange = $(this).find('td').eq(3).text();
        pblpercent = $(this).find('td').eq(4).text();
        split = $(this).find('td').eq(5).text();
        plbAmt = $(this).find('td').eq(6).text();
    });
    $("#plbrange").val(pblrange);
    $("#plbper").val(pblpercent);
}
function launch() {

    let ag = $("#BonusAgent").val();
    let yr = $("#FinancialPeriod").val();
    let agcode = $("#bonusAc").val();
    let finYr = $("#headFinancial").val();
    let datefrom = $("#dateFrom1").val();
    let dateto = $("#dateTo1").val();
    let amt = $("#bonusBase").val();
    let payable = $("#bonusPayable").val();
    let cur = $("#bonusCur").val();
    let url = symphony + "/Sales/launchRefresh";
    $.ajax({
        type: 'POST',
        url: url,
        data: { ag: ag, yr: yr, agcode: agcode, finYr: finYr, datefrom: datefrom, dateto: dateto, amt: amt, payable: payable, cur: cur },
        /*beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },*/
        success: function (data) {
            $("#displayBonus").html(data);
            console.log(data);

            // alert('ok');
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}

function ClearCom() {
    let url = symphony + "/Sales/clearcom";
    $.ajax({
        type: 'POST',
        url: url,
        success: function (msg) {
            $("#commission").html(msg);

        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}
function clearbonus() {
    let url = symphony + "/Sales/clearbonus";
    $.ajax({
        type: 'POST',
        url: url,
        success: function (msg) {
            $("#bonus").html(msg);

        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}
/******************end********************************/
/****************ISC RECLAIM*************************/
function setCriteriaIscReclaim() {
    let dateFrom = $("#dateFromIsReclaim").val();
    let dateTo = $("#dateToIsReclaim").val();
    let url = symphony + "/Sales/loadIscReclaim";
    $.ajax({
        type: 'POST',
        url: url,
        data: { dateFrom: dateFrom, dateTo: dateTo },
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#loadISC").html(data);
            if ($("#compteur").val() == 0) {
                $('#alertModal').modal('show');
                $('#msgalert').text('No data for this selected period');
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
/*******************end******************************/
/**********************facsimile**********************/
function showFacsimile(event) {
    let $form2 = $(event).closest('markelement');
    let parentId = $form2.attr('id');

    $("#" + parentId + " #facsimile").modal('show');
    var tab = [];
    var tab1 = [];
    var tab2 = [];
    var tab3 = [];

    let j = 0;

    $("#" + parentId + " #getFacsValue tr").each(function () {

        $(this).find("td").each(function (cell) {

            if (j == 0) {
                tab.push($(this).text());
            }
            if (j == 1) {
                tab1.push($(this).text());
            }
            if (j == 2) {
                tab2.push($(this).text());
            }
            if (j == 3) {
                tab3.push($(this).text());
            }

        });
        j++;
    });
    /**********************GetGFPA****************************/
    var tgfp3 = [];
    $("#" + parentId + " #getFacsValue tr").each(function () {

        if ($(this).find(".gfpa").html() != undefined) {
            tgfp3.push($(this).find(".gfpa").html());
        }
    });

    var tgfp = tgfp3;

    var tgfp2 = new Array(tgfp.length + 1);
    var tmp = tgfp[0].split('-');
    tgfp2[0] = tmp[0];
    tgfp2[1] = tmp[1];
    for (var i = 1; i < tgfp.length; i++) {
        var tmp = tgfp[i].split('-');
        tgfp2[i] = tmp[0];
        tgfp2[i + 1] = tmp[1];
    }
    var tgfpa = [];
    for (var i = 0; i < tgfp2.length; i++) {
        tgfpa.push(tgfp2[i] + " ");
    }

    let url = symphony + "/Sales/City";
    $.ajax({
        type: "POST",
        url: url,

        dataType: 'json',
        traditional: true,

        data: { tgfpa: tgfpa },
        success: function (data) {

            var datatgp = data.teste.split('/');
            for (var g = 0; g < datatgp.length; g++) {

                $("#" + parentId + " #gfp-" + g).html(datatgp[g].replace(" ", '&nbsp &nbsp &nbsp &nbsp &nbsp&nbsp &nbsp &nbsp &nbsp &nbsp&nbsp &nbsp &nbsp &nbsp &nbsp&nbsp &nbsp &nbsp &nbsp &nbsp&nbsp &nbsp &nbsp &nbsp &nbsp&nbsp &nbsp &nbsp &nbsp &nbsp'));
            }
        }

    });
    /**********************end****************************/
    for (l = 3; l < 13; l++) {

        $("#" + parentId + " #carrer1-" + l).text(tab[l]);
        $("#" + parentId + " #carrer2-" + l).text(tab1[l]);
        $("#" + parentId + " #carrer3-" + l).text(tab2[l]);
        $("#" + parentId + " #carrer4-" + l).text(tab3[l]);

    }
    var farecur = $("#" + parentId + " #FareCurrency").val();
    var fare = $("#" + parentId + " #EquivalentFare").val();
    $("#" + parentId + " #fac-FareCurrency").text(farecur + " " + fare);

    var newtotal = $("#" + parentId + " #EquivalentFare").val();
    $("#" + parentId + " #newtotal").text(newtotal);

    var taxe1 = $("#" + parentId + " #tax1").val();
    $("#" + parentId + " #newtax1fee").text(taxe1);

    var taxe2 = $("#" + parentId + " #tax2").val();
    $("#" + parentId + " #newtax2fee").text(taxe2);

    var taxe3 = $("#" + parentId + " #tax3").val();
    $("#" + parentId + " #newtax3fee").text(taxe3);

    var fareCalc = $("#" + parentId + " #fareCalculation").val();
    $("#" + parentId + " #newFareCalc").text(fareCalc);

    var total = $("#" + parentId + " #TotalAmount").val();
    $("#" + parentId + " #newtotalfee").text("EUR" + total);

    var firstdoc = $("#" + parentId + " #firstDocNumber").val();
    $("#" + parentId + " #newfirstDocNumber").text(firstdoc);

    var secdoc = $("#" + parentId + " #secondDocNumber").val();
    $("#" + parentId + " #newsecondDocNumber").text(secdoc);

    var ck = $("#" + parentId + " #CheckDigit").val();
    $("#" + parentId + " #newtreeDocNumber").text(ck);

    var dateissue = $("#" + parentId + " #DateofIssue").val();
    var agentcode = $("#" + parentId + " #AgentNumericCode").val();
    $("#" + parentId + " #newAgent").text(dateissue + " " + agentcode);

    var paxname = $("#" + parentId + " #paxName").val();
    $("#" + parentId + " #newpassengerID").text(paxname);

    var restrictionEndo = $("#" + parentId + " #EndosRestriction").val();
    $("#" + parentId + " #newrestrictionEndo").text(restrictionEndo);

    var docnum = $("#" + parentId + " #firstDocNumber").val();
    var docnum1 = $("#" + parentId + " #secondDocNumber").val();
    $("#" + parentId + " #docNumber").val(docnum + "" + docnum1);

}
/**********************endFacsmile****************************/

/*******************PrintdIV********************************/


/***** fait par christian**************************/
function saveCanvas() {

    /*  html2canvas(document.querySelector("#capture")).then(canvas => {
          var image = canvas.toDataURL("image/png").replace("image.png", "image/octet-stream;base64");
          var win = window.open("", "new div", "height=850,width=1350");
          win.document.write('<iframe src="' + image + '" frameborder="0" style="border:0; top:50px; left:50px; bottom:50px; right:50px; width:100%; height:100%;" allowfullscreen></iframe>')
  
      });*/


    //fait par NOMENTSOA Christian
    let docNumber = $("#docNumber1").val();

    let url = "/Process/Facsimile";

    $.ajax({
        async: true,
        type: "POST",
        url: url,
        data: { docNumber: docNumber },

        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {

            $(".data-transaction").html(data);
            $(".data-transaction").html(data);
            if (domInt == "I") {
                domInt = "INTERNATIONAL";
            }
            if (domInt == "D") {
                domInt = "DOMESTIC";
            }
            $("#domInt").val(domInt);


            $("#processFacsmile").modal('show');
            var tab = [];
            var tab1 = [];
            var tab2 = [];
            var tab3 = [];

            let j = 0;

            $("#getFacsValue tr").each(function () {
                $(this).find("td").each(function (cell) {

                    if (j == 0) {
                        tab.push($(this).text());
                    }
                    if (j == 1) {
                        tab1.push($(this).text());
                    }
                    if (j == 2) {
                        tab2.push($(this).text());
                    }
                    if (j == 3) {
                        tab3.push($(this).text());
                    }

                });
                j++;
            });

            /**********************GetGFPA****************************/
            var tgfp3 = [];
            $("#getFacsValue tr").each(function () {

                if ($(this).find(".gfpa").html() != undefined) {
                    tgfp3.push($(this).find(".gfpa").html());
                }
            });

            var tgfp = tgfp3;

            var tgfp2 = new Array(tgfp.length + 1);
            var tmp = tgfp[0].split('-');
            tgfp2[0] = tmp[0];
            tgfp2[1] = tmp[1];
            for (var i = 1; i < tgfp.length; i++) {
                var tmp = tgfp[i].split('-');
                tgfp2[i] = tmp[0];
                tgfp2[i + 1] = tmp[1];
            }
            var tgfpa = [];
            for (var i = 0; i < tgfp2.length; i++) {
                tgfpa.push(tgfp2[i] + " ");
            }

            let url = symphony + "/Process/City";
            $.ajax({
                type: "POST",
                url: url,

                dataType: 'json',
                traditional: true,

                data: { tgfpa: tgfpa },
                success: function (data) {

                    var datatgp = data.teste.split('/');
                    for (var g = 0; g < datatgp.length; g++) {

                        $("#gfp-" + g).html(datatgp[g].replace(" ", '&nbsp &nbsp &nbsp &nbsp &nbsp&nbsp &nbsp &nbsp &nbsp &nbsp&nbsp &nbsp &nbsp &nbsp &nbsp&nbsp &nbsp &nbsp &nbsp &nbsp&nbsp &nbsp &nbsp &nbsp &nbsp&nbsp &nbsp &nbsp &nbsp &nbsp'));
                    }
                }

            });
            /**********************end****************************/

            for (l = 3; l < 13; l++) {

                $("#carrer1-" + l).text(tab[l]);
                $("#carrer2-" + l).text(tab1[l]);
                $("#carrer3-" + l).text(tab2[l]);
                $("#carrer4-" + l).text(tab3[l]);

            }
            var farecur = $("#FareCurrency").val();
            var fare = $("#EquivalentFare").val();
            $("#fac-FareCurrency").text(farecur + " " + fare);

            var newtotal = $("#EquivalentFare").val();
            $("#newtotal").text(newtotal);

            var taxe1 = $("#tax1").val();
            $("#newtax1fee").text(taxe1);

            var taxe2 = $("#tax2").val();
            $("#newtax2fee").text(taxe2);

            var taxe3 = $("#tax3").val();
            $("#newtax3fee").text(taxe3);

            var fareCalc = $("#fareCalculation").val();
            $("#newFareCalc").text(fareCalc);

            var total = $("#TotalAmount").val();
            $("#newtotalfee").text("EUR" + total);

            var firstdoc = $("#firstDocNumber").val();
            $("#newfirstDocNumber").text(firstdoc);

            var secdoc = $("#secondDocNumber").val();
            $("#newsecondDocNumber").text(secdoc);

            /*var ck = $("#CheckDigit").val();
            $("#newtreeDocNumber").text(ck);*/

            var ck = $("#ChkCode").val();
            $("#newtreeDocNumber").text(ck);


            var dateissue = $("#DateofIssue").val();
            var agentcode = $("#AgentNumericCode").val();
            $("#newAgent").text(dateissue + " " + agentcode);

            var paxname = $("#paxName").val();
            $("#newpassengerID").text(paxname);

            var restrictionEndo = $("#EndosRestriction").val();
            $("#newrestrictionEndo").text(restrictionEndo);

            var docnum = $("#firstDocNumber").val();
            var docnum1 = $("#secondDocNumber").val();
            $("#docNumber").val(docnum + "" + docnum1);
            // $("#code").val(docnum + "" + docnum1);
            $("#code").text(docnum + "" + docnum1);
            /**********************endFacsmile****************************/


            //  $(".facsimile-body").html($(data).filter('#newfacsimilebody'));

        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
            html2canvas(document.querySelector("#capture")).then(canvas => {
                var image = canvas.toDataURL("image/png").replace("image.png", "image/octet-stream;base64");
                var win = window.open("", "new div", "height=850,width=1350");
                win.document.write('<iframe src="' + image + '" frameborder="0" style="border:0; top:50px; left:50px; bottom:50px; right:50px; width:100%; height:100%;" allowfullscreen></iframe>')

            });
            //  $("#txtnum").val("tsetqsdfqs qsdf qsdfqsdf");

        }
    });

    // fin fait par NOMENTSOA Christian
}
/*******************fien fait par christian****************/
/*******************end********************************/
/****************CommissionReclaim******************************/
function setCriteriaCommiReclaim() {
    let dateFrom = $("#dateFromCommission").val();
    let dateTo = $("#dateToCommission").val();
    let url = symphony + "/Sales/LoadCommissionReclaim";
    $.ajax({
        type: "POST",
        url: url,
        data: { dateFrom: dateFrom, dateTo: dateTo },

        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#loadCommiReclaim").html(data);
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
function clearCommition() {
    let url = symphony + "/Sales/CommissionReclaim";
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
/****************INAD-Oal******************************/
function setCriteriaINADoal() {
    let dateFrom = $("#dateFromInadOal").val();
    let dateTo = $("#dateToInadOal").val();
    let url = symphony + "/Sales/LoadINADOalCostSharing";
    $.ajax({
        type: "POST",
        url: url,
        data: { dateFrom: dateFrom, dateTo: dateTo },

        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#loadINADoal").html(data);

        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
            var cmpt = $('#compt').val();
            $('#alertModal').modal('show');
            $('#msgalert').text(cmpt);

        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}
function clearIndOal() {
    let url = symphony + "/Sales/INADOalCostSharing";
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
/****************INAD******************************/
function setCriteriaINAD() {
    let pasengerName = $('#valrad input[type=text][name=passenger]').val();
    let docNum = $('#valrad input[type=text][name=document]').val();
    let radContains = $('#valrad input[type=radio][name=radContains]:checked').attr('value');
    let url = symphony + "/Sales/LoadINAD";
    $.ajax({
        type: "POST",
        url: url,
        data: { pasengerName: pasengerName, docNum: docNum, radContains: radContains },
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $(".loadINAD").html(data);
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}
function clickLigneINAD(docNumber, pasengerName, dateissue, fareCalc) {
    $('#tab-INAD').trigger('click');

    $('#docnum0').val(docNumber);
    $('#passName').text(pasengerName);
    $('#dateissue').text(dateissue);
    $('#fareCalc').val(fareCalc);
    $('#dateissues').text(dateissue);
    $('#fareCalcu').val(fareCalc);
    var Valinput = 0;
    var Valinput0 = 0;

    var unc5 = $('#unc9').val();

    $('#unc1').keyup(function () {
        let testbilling = $('#billing').val();
        if (testbilling != "") {
            $('#billing').val(testbilling);
        } else {
            $('#alertModal').modal('show');
            $('#msgalert').text('Please Enter the Billing Period First');
        }
    });
    $('#unc2').keyup(function () {
        let testunc1 = $('#unc1').val();

        if (testunc1 != "") {
            $('#unc1').val(testunc1);
            Valinput = $(this).val();
            var valeur = parseFloat(Valinput);
            $('#unc4').val(valeur.toFixed(2));

            $('#unc12').val(valeur.toFixed(2));
            $('#unc10').val(valeur.toFixed(2) * unc5 / 100);
        }
        else {
            $('#alertModal').modal('show');
            $('#msgalert').text('Please Enter The Currency First');
        }
    });
    $('#unc6').keyup(function () {
        let testunc5 = $('#unc5').val();
        if (testunc5 != "") {
            $('#unc5').val(testunc5);
            Valinput0 = $(this).val();
            var valeur = parseFloat(Valinput);
            var valeur1 = parseFloat(Valinput0);
            $('#unc8').val(valeur1.toFixed(2));
            var som = valeur1 + valeur;

            $('#unc12').val(som.toFixed(2));
        }
        else {
            $('#alertModal').modal('show');
            $('#msgalert').text('Please Enter The Currency First');
        }
    });

    let fca = fareCalc;
    let url = symphony + "/Sales/RetINAD";

    $.ajax({
        type: 'POST',
        url: url,
        data: { FCA: fca },
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#fca").html(data);
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");

        },
        error: function () {
            $('#errorModal').modal('show');
        }
    })
}
function loadPop(row, cityCode, cityname) {

    $('#cityname').text(cityname);
    $('#citycode').text(cityCode);
    $('#inadpoint').val(cityCode);
    var airId = "";
    $('#airlinepop tr').each(function () {
        airId += $(this).find("td").eq(1).html() + "/";

    });

    var datatgp = airId.split('/');
    for (var g = 0; g < datatgp.length; g++) {
        if (row.rowIndex == 1) {
            $('#inbound').val(datatgp[2]);
            $('#inbound1').val(datatgp[1]);
            $('#inbound2').val(datatgp[0]);
            $('#inbound3').val(datatgp[3]);
            $('#inbound4').val(datatgp[4]);
        }
        else if (row.rowIndex == 2) {

            $('#inbound').val(datatgp[0]);
            $('#inbound1').val(datatgp[1]);
            $('#inbound2').val(datatgp[3]);
            $('#inbound3').val(datatgp[3]);
            $('#inbound4').val(datatgp[4]);
        }
        else if (row.rowIndex == 3) {

            $('#inbound').val(datatgp[0]);
            $('#inbound1').val(datatgp[1]);
            $('#inbound2').val(datatgp[2]);
            $('#inbound3').val(datatgp[3]);
            $('#inbound4').val(datatgp[4]);
        }
    }
    let fca1 = $('#fareCalc').val();
    let CityCode = cityCode;
    let url = symphony + "/Sales/PopINAD";
    $.ajax({
        type: 'POST',
        url: url,
        data: { FCA: fca1, CityCode: CityCode },
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#popInad").html(data);

        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");

        },
        error: function () {
            $('#errorModal').modal('show');
        }
    })
}
function loadPopu(aircode, airname) {

    $('#airname').text(airname);
    $('#aircode').text(aircode);
    $('#finalbound').val(aircode);

}
function Execute() {

    let fca1 = $('#fareCalc').val();
    let inadpoint = $('#inadpoint').val();
    let finalbound = $('#finalbound').val();
    let billing = $('#billing').val();
    let inbound = $('#inbound').val();
    let inbound1 = $('#inbound1').val();
    let inbound2 = $('#inbound2').val();
    let txtUncollectedFareAmt = $('#unc2').val();
    let txtUncollectedNonTransFareAmt = $('#unc4').val();

    if (inadpoint == "") {
        $('#alertModal').modal('show');
        $('#msgalert').text("No INAD Point is defined");
    }
    else if (finalbound == "") {
        $('#alertModal').modal('show');
        $('#msgalert').text("No Final Inbound Carrier is defined");
    }
    else if (billing == "") {
        $('#alertModal').modal('show');
        $('#msgalert').text("Please Enter the Billing Period first");
    }
    else {
        let url = symphony + "/Sales/ExecuteINAD";
        $.ajax({
            type: 'POST',
            url: url,
            data: {
                FCA: fca1, inadpoint: inadpoint, finalbound: finalbound, issuedate: billing, inbound: inbound, inbound1: inbound1, inbound2: inbound2,
                txtUncollectedFareAmt: txtUncollectedFareAmt, txtUncollectedNonTransFareAmt: txtUncollectedNonTransFareAmt
            },
            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {
                $("#execInad").html(data);
            },
            complete: function () {
                $('.ajax-loader').css("visibility", "hidden");
            },
            error: function () {
                $('#errorModal').modal('show');
            }
        })
    }
}
function SaveInad() {

    let txtNewDoc = $('#docnum0').val();
    let lblNewIssueDate = $('#dateissues').text();
    var lblPaxName = $('#passName').text();
    let txtNewFCA = $('#fareCalc').val();

    let txtOrgDoc = $('#txtOrgDoc').val();
    let txtOrgFCA = $('#fareCalc').val();
    let txtINADpoint = $('#inadpoint').val();
    let txtFinalInboundCarri = $('#finalbound').val();
    let txtInboundCarriage1 = $('#inbound').val();
    let txtInboundCarriage2 = $('#inbound1').val();
    let txtInboundCarriage3 = $('#inbound2').val();
    let txtInboundCarriage4 = $('#inbound3').val();
    let txtInboundCarriage5 = $('#inbound4').val();
    let txtBillPer = $('#billing').val();
    let txtUncollFareCurrency = $('#unc1').val();
    let txtUncollFareAmt = $('#unc4').val();
    let txtUncollectedFareAmt = $('#unc4').val();
    let txtUncolNTCurrency = $('#unc3').val();
    let txtUncollNTAmt = $('#unc8').val();
    let txtUncollectedNonTransFareAmt = $('#unc8').val();
    let txtIscPer = "9.00";
    let txtOutboundCarriage = $('#out').val();
    let txtOutboundCarriage1 = $('#out1').val();
    let txtOutboundCarriage2 = $('#out2').val();
    let txtOutboundCarriage3 = $('#out3').val();
    let txtOutboundCarriage4 = $('#out4').val();
    let txtOutboundCarriage5 = $('#out5').val();
    let txtHandlingAirline = $('#out6').val();
    let Au = "0";
    //let Au = $("#voluntary input:radio[name=Voluntary]:checked").val();

    var tInad = [];
    $('#tInad tr').each(function () {
        if ($(this).find("td").html() != undefined) {
            tInad.push($(this).find("td").eq(0).html() + "," + $(this).find("td").eq(1).html() + "," + $(this).find("td").eq(2).html() + "," + $(this).find("td").eq(3).html() + "," + $(this).find("td").eq(4).html()
                + "," + $(this).find("td").eq(5).html() + "," + $(this).find("td").eq(6).html() + "," + $(this).find("td").eq(7).html());
        }
    });
    var dgProrateShare = tInad;
    if (dgProrateShare == "") {
        $('#alertModal').modal('show');
        $('#msgalert').text("Click on tne Execute Button first to Populate the Grid");

    }
    else {
        let url = symphony + "/Sales/SaveINAD";
        $.ajax({
            type: 'POST',
            url: url,
            data: {
                txtNewDoc: txtNewDoc, lblPaxName: lblPaxName, lblNewIssueDate: lblNewIssueDate,
                txtNewFCA: txtNewFCA, txtOrgDoc: txtOrgDoc,
                txtOrgFCA: txtOrgFCA, txtINADpoint: txtINADpoint,
                txtFinalInboundCarri: txtFinalInboundCarri, txtInboundCarriage1: txtInboundCarriage1,
                txtInboundCarriage2: txtInboundCarriage2, txtInboundCarriage3: txtInboundCarriage3,
                txtInboundCarriage4: txtInboundCarriage4, txtInboundCarriage5: txtInboundCarriage5,
                txtBillPer: txtBillPer, txtUncollFareCurrency: txtUncollFareCurrency,
                txtUncollFareAmt: txtUncollFareAmt, txtUncollectedFareAmt: txtUncollectedFareAmt,
                txtUncolNTCurrency: txtUncolNTCurrency, txtUncollNTAmt: txtUncollNTAmt,
                txtUncollectedNonTransFareAmt: txtUncollectedNonTransFareAmt, txtIscPer: txtIscPer,
                txtOutboundCarriage: txtOutboundCarriage, txtOutboundCarriage1: txtOutboundCarriage1,
                txtOutboundCarriage2: txtOutboundCarriage2, txtOutboundCarriage3: txtOutboundCarriage3,
                txtOutboundCarriage4: txtOutboundCarriage4, txtOutboundCarriage5: txtOutboundCarriage5,
                txtHandlingAirline: txtHandlingAirline, Au: Au, dgProrateShare: dgProrateShare
            },
            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {

                if (data == "update") {
                    $('#alertModal').modal('show');
                    $('div.modal-confirmation').html('<button type="button" class="btn btn-default" style="position: relative;top: 12px;" id="btnconf" onclick="confirmDeleteInad()">OK</button>' +
                        '<button type="button" class="Close btn btn-secondary btn-default" style="background-color:#e8ae72;" data-dismiss="modal">Close</button>');
                    $("#msgalert").html("This Record already Exist!Do you want to update it?");
                }
                else {
                    $('#alertModal').modal('show');
                    $('#msgalert').text(data);
                }

            },
            complete: function () {
                $('.ajax-loader').css("visibility", "hidden");
            },
            error: function () {
                $('#errorModal').modal('show');
            }
        })
    }

}
function confirmDeleteInad() {
    //$('#alertModal').modal('hide');
    let txtNewDoc = $('#docnum0').val();
    let lblNewIssueDate = $('#dateissues').text();
    var lblPaxName = $('#passName').text();
    let txtNewFCA = $('#fareCalc').val();

    let txtOrgDoc = $('#txtOrgDoc').val();
    let txtOrgFCA = $('#fareCalc').val();
    let txtINADpoint = $('#inadpoint').val();
    let txtFinalInboundCarri = $('#finalbound').val();
    let txtInboundCarriage1 = $('#inbound').val();
    let txtInboundCarriage2 = $('#inbound1').val();
    let txtInboundCarriage3 = $('#inbound2').val();
    let txtInboundCarriage4 = $('#inbound3').val();
    let txtInboundCarriage5 = $('#inbound4').val();
    let txtBillPer = $('#billing').val();
    let txtUncollFareCurrency = $('#unc1').val();
    let txtUncollFareAmt = $('#unc4').val();
    let txtUncollectedFareAmt = $('#unc4').val();
    let txtUncolNTCurrency = $('#unc3').val();
    let txtUncollNTAmt = $('#unc8').val();
    let txtUncollectedNonTransFareAmt = $('#unc8').val();
    let txtIscPer = "9.00";
    let txtOutboundCarriage = $('#out').val();
    let txtOutboundCarriage1 = $('#out1').val();
    let txtOutboundCarriage2 = $('#out2').val();
    let txtOutboundCarriage3 = $('#out3').val();
    let txtOutboundCarriage4 = $('#out4').val();
    let txtOutboundCarriage5 = $('#out5').val();
    let txtHandlingAirline = $('#out6').val();
    let Au = "0";

    var tInad = [];
    $('#tInad tr').each(function () {
        if ($(this).find("td").html() != undefined) {
            tInad.push($(this).find("td").eq(0).html() + "," + $(this).find("td").eq(1).html() + "," + $(this).find("td").eq(2).html() + "," + $(this).find("td").eq(3).html() + "," + $(this).find("td").eq(4).html()
                + "," + $(this).find("td").eq(5).html() + "," + $(this).find("td").eq(6).html() + "," + $(this).find("td").eq(7).html());
        }
    });
    var dgProrateShare = tInad;

    let url = symphony + "/Sales/deleteInad";
    $.ajax({
        type: 'POST',
        url: url,
        data: {
            txtNewDoc: txtNewDoc, lblPaxName: lblPaxName, lblNewIssueDate: lblNewIssueDate,
            txtNewFCA: txtNewFCA, txtOrgDoc: txtOrgDoc,
            txtOrgFCA: txtOrgFCA, txtINADpoint: txtINADpoint,
            txtFinalInboundCarri: txtFinalInboundCarri, txtInboundCarriage1: txtInboundCarriage1,
            txtInboundCarriage2: txtInboundCarriage2, txtInboundCarriage3: txtInboundCarriage3,
            txtInboundCarriage4: txtInboundCarriage4, txtInboundCarriage5: txtInboundCarriage5,
            txtBillPer: txtBillPer, txtUncollFareCurrency: txtUncollFareCurrency,
            txtUncollFareAmt: txtUncollFareAmt, txtUncollectedFareAmt: txtUncollectedFareAmt,
            txtUncolNTCurrency: txtUncolNTCurrency, txtUncollNTAmt: txtUncollNTAmt,
            txtUncollectedNonTransFareAmt: txtUncollectedNonTransFareAmt, txtIscPer: txtIscPer,
            txtOutboundCarriage: txtOutboundCarriage, txtOutboundCarriage1: txtOutboundCarriage1,
            txtOutboundCarriage2: txtOutboundCarriage2, txtOutboundCarriage3: txtOutboundCarriage3,
            txtOutboundCarriage4: txtOutboundCarriage4, txtOutboundCarriage5: txtOutboundCarriage5,
            txtHandlingAirline: txtHandlingAirline, Au: Au, dgProrateShare: dgProrateShare
        },
        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {

            $('#alertModal').modal('show');
            $('div.modal-confirmation').html(
                '<button type="button" class="Close btn btn-secondary btn-default" style="background-color:#e8ae72;" data-dismiss="modal">Close</button>');
            $("#msgalert").html(data);
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    })


}
function clearInad() {

    $('#finalbound').val('');
    $('#inbound').val('');
    $('#inbound1').val('');
    $('#inbound2').val('');
    $('#inbound3').val('');
    $('#inbound4').val('');
    $('#billing').val('');
    $('#unc1').val('');
    $('#unc2').val('');
    $('#unc4').val('');
    $('#unc5').val('');
    $('#unc6').val('');
    $('#unc8').val('');
    $('#unc12').val('');
    $('#out').val('');
    $('#out1').val('');
    $('#out2').val('');
    $('#out3').val('');
    $('#out4').val('');
    $('#out5').val('');
    $('#out6').val('');

    $("#tInad").html('');
}
function clearInad0() {

    $('#valrad input[type=text][name=passenger]').val('');
    $('#valrad input[type=text][name=document]').val('');

    $("#inadLoad").html('');
}
function closeTabInad() {
    $('[href="#search"]').tab('show');
}
/*******************end********************************/
/****************FOP-UATP Billings to OALS******************************/
function setCriteriaUATPbilling() {
    let dateFrom = $("#dateFromFop").val();
    let dateTo = $("#dateToFop").val();
    let url = symphony + "/Sales/LoadFOPUATPBillingsToOALs";
    $.ajax({
        type: "POST",
        url: url,
        data: { dateFrom: dateFrom, dateTo: dateTo },

        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#loadUATPbilling").html(data);

        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
            var cmpt = $('#compt').val();
            $('#alertModal').modal('show');
            $('#msgalert').text(cmpt);
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}
function clearUatpBilling() {
    let url = symphony + "/Sales/FOPUATPBillingsToOALs";
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
/****************FOP-UATP Billings to OALS******************************/
function setCriteriaUATPreclaim() {
    let dateFrom = $("#dateFromDiscount").val();
    let dateTo = $("#dateToDiscount").val();
    let url = symphony + "/Sales/LoadUATPDiscountReclaim";
    $.ajax({
        type: "POST",
        url: url,
        data: { dateFrom: dateFrom, dateTo: dateTo },

        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#loadUATPreclaim").html(data);

        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
            var cmpt = $('#compt').val();
            $('#alertModal').modal('show');
            $('#msgalert').text(cmpt);
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}
function clearUatpReclaim() {
    let url = symphony + "/Sales/UATPDiscountReclaim";

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
/*******************end********************************/
function setCriteriaExpectedEIR_FOR() {
    let dateFrom = $("#dateFromExpected").val();
    let dateTo = $("#dateToExpected").val();
    let dropdownType = $("#dropdownType").val();

    let url = symphony + "/Sales/LoadExpectedEIROFR";
    if (dropdownType == "") {
        $('#alertModal').modal('show');
        $('#msgalert').text('Please Select an Option from the Selection DropBox');
    }
    else {
        $.ajax({
            type: "POST",
            url: url,
            data: { dropdownType: dropdownType, dateFrom: dateFrom, dateTo: dateTo },

            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {
                $("#loadExpectedEIR_FOR").html(data);
            },
            complete: function () {
                $('.ajax-loader').css("visibility", "hidden");
                var cmptu = $('#comptu').val();
                if (cmptu != "") {
                    $('#alertModal').modal('show');
                    $('#msgalert').text(cmptu);
                }
            },
            error: function () {
                $('#errorModal').modal('show');
            }
        });
    }
}
function clearEIR() {
    let url = symphony + "/Sales/ExpectedEIROFR";
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
/********************Retroactive***********************/
function setListeRetroactiveAdjustement() {
    var tab = [];
    var tab1 = [];
    if ($("#DocumentNo").val() == "") {
        $('#alertModal').modal('show');
        $("#msgalert").html("Please Enter or Type a Document Number");
    }
    else {
        if ($("#DocumentNo").val() !== "") {
            let passengerNameSet = $("#passengerNameSet").val();
            let DocumentNo = $("#DocumentNo").val();
            tab = [passengerNameSet, DocumentNo];
            let url = symphony + "/Sales/setListeRetroactiveAdjustement";
            $.ajax({
                type: 'POST',
                url: url,
                data: { dataValue: tab },
                beforeSend: function () {
                    $('.ajax-loader').css("visibility", "visible");
                },
                success: function (data) {
                    let dateMore = data.split("$");
                    $("#DateOfIssue").val(dateMore[0]);
                    $("#FareBasis").val(dateMore[1]);
                    $("#OriginalRouting").val(dateMore[2]);
                    $("#ActualFlownRouting").val(dateMore[2]);
                    $("#passengerNameSet").val(dateMore[3]);
                    $("#NotValidAfter").val(dateMore[4]);

                },
                complete: function () {
                    $('.ajax-loader').css("visibility", "hidden");
                },
                error: function () {
                    $('#alertModal').modal('show');
                    $("#msgalert").html("Invalid PassengerName Or Documment Number");
                }
            });
            //Ajaxs(tab, '', 'success', url);
        }
        else if ($("#passengerNameSet").val() !== "") {

            let passengerNameSet = $("#passengerNameSet").val();
            let DocumentNo = $("#DocumentNo").val();
            tab = [passengerNameSet, DocumentNo];
            let url = symphony + "/Sales/setListeRetroactiveAdjustementName";
            $.ajax({
                type: 'POST',
                url: url,
                data: { dataValue: tab },
                beforeSend: function () {
                    $('.ajax-loader').css("visibility", "visible");
                },
                success: function (data) {
                    let dateMore = data.split("$");
                    $("#DateOfIssue").val(dateMore[0]);
                    $("#FareBasis").val(dateMore[1]);
                    $("#OriginalRouting").val(dateMore[2]);
                    $("#DocumentNo").val(dateMore[3]);
                    $("#NotValidAfter").val(dateMore[4]);
                    console.log(dateMore);
                },
                complete: function () {
                    $('.ajax-loader').css("visibility", "hidden");
                },
                error: function () {
                    $('#alertModal').modal('show');
                    $("#msgalert").html("Invalid PassengerName Or Documment Number");
                }
            });
            //Ajaxs(tab, '', 'success', url);
        }
        else {

        }
    }
}
/////////////////////////////////////
function ClearListretroactive() {
    $("#passengerNameSet").val("");
    $("#DocumentNo").val("");
    $("#FareBasis").val("");
    $("#NotValidAfter").val("");
    $("#DateOfIssue").val("");
    $("#OriginalRouting").val("");
    $("#ActualFlownRouting").val("");
}
function BtnRetroactiveAdjustement() {
    var tab = [];
    var tab1 = [];
    if ($("#OriginalRouting").val() == "" && $("#ActiualFlownRouting").val() == "") {
        $('#alertModal').modal('show');
        $("#msgalert").html("Please Enter A Passenger Name or Document Number To Generate An Original Routing First");
    }
    else {
        if ($("#OriginalRouting").val() !== "" && $("#ActiualFlownRouting").val() == "") {
            $('#alertModal').modal('show');
            $("#msgalert").html("Please Enter The Actual Routing Flown");
        }
        else {
            if ($("#DateOfIssue").val() == "") {
                $('#alertModal').modal('show');
                $('#msgalert').html("Please Enter A Passenger Name or Document Number To Generate An Original Routing First");
            }
            else {

            }

        }

    }
}
function closeRetroactiveAdjustement() {
    $('[href="#search"]').tab('show');
}
function btnCalculateDifference() {

}
/**********************No show*******************/
/*******************end******************/

/**
    code by fano Process
*/
function activeNutshell(id) {
    for (i = 2; i < 16; i++) {
        if (document.getElementById('nutshell' + i).style.display == 'block') {
            document.getElementById('nutshell' + i).style.display = ''
        }
    }

    if (document.getElementById(id).style.display == 'none') {
        $("#" + id).show();
        $("#" + id.slice(4, id.length)).show();
    }

    $("ul.liNutshell li.active").removeClass('active');
    $("#" + id).parents().addClass('active');
}

function closeNetshull(id) {
    $("ul.liNutshell li.active").removeClass('active');
    $("#" + id).removeClass('active');
    let index = parseInt(id.slice(-1), 10) + 1;
    let newIndex = "tab-nutshell" + index;
    $("#" + newIndex).parents().addClass('active');
    $("#nutshell" + index).addClass('active in');
    $("#" + id).hide();
    $("#tab-" + id).hide();
}



/* Load Refund By Agent     Joseph*/

function ListOfRefundByA() {
    $('#ListeOfRefund').modal('show');
}

function CloseListOfRefund() {
    $('#ListeOfRefund').removeClass();
}

function tableRefundEngine() {
    $(document).ready(function () {
        $('#TableRefund').DataTable({
            "pageLength": 100,
            dom: 'Bfrtip',
            buttons: [
                 'csv'
            ]
        });
    });
}




/* End  Load Refund By Agent     Joseph*/


