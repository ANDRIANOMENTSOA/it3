/*
 * Code 
 * HARENTSOA 
 *
 */

//GLOBAL VARIABLE TO KNOW IF ACTION IS UPDATE OR ADD
var idAction;

function getKKFrom() {
    let fromValue = $("#selectFrom").val();
    console.log(fromValue, "It's typeof is :", typeof(fromValue));

    if (fromValue === "") {
        $('#airlineDetails1').modal({
            show : 'false'
        });
        $.alert({
            title: 'Alert!',
            content: 'Please choose at least one Origin Reference!',
        });
       
    } else {
        $.ajax({
            type: "GET",

            url: "/BillingMemo/AirlineDetailsFrom",
            dataType: "Html",
            data: { fromValue: fromValue },

            success: function (data) {
                $('#airlineDetailContent1').html(data);
                $('#airlineDetails1').modal('show');

            }
        });
    }

}

    function getRefTo() {
        let toValue = $("#selectTo").val();

        if (toValue === "") {
            $('#airlineDetails2').modal({
                show: 'false'
            });
            $.alert({
                title: 'Alert!',
                content: 'Please choose at least one Destination Reference!',
            });

        } else {
            $.ajax({
                type: "GET",
                url: "/BillingMemo/AirlineDetailsTo",
                dataType: "Html",
                data: { toValue: toValue },

                success: function (data) {
                    $('#airlineDetailContent2').html(data);
                    $('#airlineDetails2').modal('show');
                }
            });
        }
        
    }

    function getBilledAirline() {
        let toValueForBilledAirline = $("#selectTo option:selected").text();
        //console.log("Ny value azo", toValueForBilledAirline);

        $.ajax({
            type: "GET",
            url: "/BillingMemo/BilledAirline",
            dataType: "Html",
            data: { toValueForBilledAirline: toValueForBilledAirline },

            success: function(data) {
                //console.log("Je passe par ici", toValueForBilledAirline);
                $('#BilledAirline').html(data);
            }
        });
    }

    function docDetailsSearch() {

        let DocNumbValue = $("#docNumberAren").val();

        console.log(DocNumbValue);

        let url = "/Process/DocDetailsSearch";

        $.ajax({
            type: "GET",
            url: url,
            data: { DocNumbValue: DocNumbValue },

            success: function(data) {
                $("#tabDocumentDetails tbody tr").css('visibility: visible');
                $("#tabDocumentDetails").html(data);
                $('#docDetails').modal('show');
            }
        });

    }


//Hide element details

    function getDocumentDetails(CpnNoAttr) {
        //console.log('TR clicked!!!!', test);

        var CpnNo = CpnNoAttr;
        let url = "/BillingMemo/GetDocDetails";

        $.ajax({
            type: "GET",
            url: url,
            data: { CpnNo: CpnNo },

            success: function(data) {
                //console.log("Madalo avy cliquer-na!!!");
                //$(".docDetailsOnClick").css('visibility : visible');
                $("#docDetailsOnClickAbove").html(data);
            }
        });

    }

// Get all Documents Detais after search
// On click Close
    function DisplayDetails() {
        var doc = $("#docNumberAren").val();
        var cpn = $("#DocDetailsAirlineID").val();

        $.ajax({
            type: "GET",

            url: "/BillingMemo/DisplayDetails",
            dataType: "Html",
            data: {
                doc: doc,
                cpn: cpn
            },

            success: function(data) {
                $('#DocDetailsSearched').html(data);
            },
            complete: function() {
                //console.log("Mandaloe ato aho less!!!!!");
                //Clear all data on modal used to search Document Details
                $("#docNumberAren").val("");
                $(".SearchedDocValueAbove").css('visibility: hidden');
                $("#tabDocumentDetails tbody tr").css('visibility: hidden');
            }
        });

    }

//Clear Details After search Document Details
// OnClick on Bouton close, modal
    function ClearDetails() {
        //Clear All inputs Data
        $("#IssuingAirline").val("");
        $("#CheckDigit").val("");
        $("#TicketingModeIndicator").val("");
        $("#SettlementAuthorizationCode").val("");
        $("#FlightNumber").val("");
        $("#FlightDepartureDate").val("");
        $("#FareBasisTicketDesignator").val("");
        $("#OriginCity").val("");
        $("#DestinationCity").val("");
        $("#Carrier").val("");
        $("#cpnD").val("");
        $("#doc").val("");
    }

//MB Query
//Get first dropdown value
    function getBillingPeriodQuery() {
        let billingPeriodQuery = $("#billingPeriodQuery").val();
        //console.log(billingPeriodQuery);

        $.ajax({
            type: "GET",

            url: "/Process/getBillingPeriodRecordsQuery",
            dataType: "Html",
            data: { billingPeriodQuery: billingPeriodQuery },

            success: function(data) {
                $('#contenu').html(data);
            },
            complete: function() {
                $("#billingPeriodQuery").val(billingPeriodQuery);
            }
        });
    }

//MB Query
//Get second dropdown value
    function getBillingMemoNumberQuery() {
        let billingPeriodQueryGetted = $("#billingPeriodQuery").val();
        let billingMemoNumberQuery = $("#billingMemoNumberQuery").val();

        //console.log(billingPeriodQuery);

        $.ajax({
            type: "GET",

            url: "/Process/getInvoiceNumberQuery",
            dataType: "Html",
            data: {
                billingPeriodQueryGetted: billingPeriodQueryGetted,
                billingMemoNumberQuery: billingMemoNumberQuery
            },

            success: function(data) {
                $('#contenu').html(data);
            },
            complete: function() {
                $("#billingPeriodQuery").val(billingPeriodQueryGetted);
                $("#billingMemoNumberQuery").val(billingMemoNumberQuery);
            }
        });
    }

//MB Query
//Get fird dropdown value
    function BMRecordsDisplayQry() {
        let billingPeriod = $("#billingPeriodQuery").val();
        let billingMemoNumber = $("#billingMemoNumberQuery").val();
        let invoiceNumber = $("#invoiceNumberQuery").val();

        //console.log(billingMemoNumber, billingMemoNumber, invoiceNumber);

        $.ajax({
            type: "GET",

            url: "/BillingMemo/BMRecordsDisplayQry",
            dataType: "Html",
            data: {
                billingPeriod: billingPeriod,
                billingMemoNumber: billingMemoNumber,
                invoiceNumber: invoiceNumber
            },

            success: function(data) {
                $('#bmQueryContent_fromDropdown').html(data);
            },
            complete: function() {
                $("#billingPeriodQuery").val(billingPeriod);
                $("#billingMemoNumberQuery").val(billingMemoNumber);
                $("#invoiceNumberQuery").val(invoiceNumber);
                //console.log("call Populate Billing Memo List Action!!!");
                //PopulateBillingMemoList();
                //BMXML();
            }
        });
    }

    function dgvBMRecQry_CellClick() {
        $.ajax({
            type: "GET",
            url: "/BillingMemo/dgvBMRecQry_CellClick",
            dataType: "Html",
            data: {
                
            },
            success: function (data) {
                
            },
            complete: function () {
               
            }
        });
    }

    function PopulateBillingMemoList() {
        let PVCreditMemoNumber = $("#PVCreditMemoNumber").val();
        let PVInvoiceNumber = $("#PVInvoiceNumber").val();
        let invoiceNumber = $("#cboTo_Text").val();
        let txtCreditMemoNumber = $("#billingMemoNumberQuery").val();
        let txtInvoiceNumber = $("#invoiceNumberQuery").val();

        $.ajax({
            type: "GET",

            url: "/BillingMemo/PopulateBillingMemoList",
            dataType: "Html",
            data: {
                PVCreditMemoNumber: PVCreditMemoNumber,
                PVInvoiceNumber: PVInvoiceNumber,
                invoiceNumber: invoiceNumber,
                txtCreditMemoNumber: txtCreditMemoNumber,
                txtInvoiceNumber: txtInvoiceNumber
            },

            success: function(data) {
                //console.log("fct on succes BM list!!!!");
                $('#BMDisplayList').html(data);
            },

            complete: function() {
                //console.log("fct on COMPLETE BM list!!!!");
            }
        });
    }

    function InvoiceTotals() {

        let ItGrossBilled = $("#ItGrossBilled").val();
        let ItTaxAmount = $("#ItTaxAmount").val();
        let ItISCAmount = $("#ItISCAmount").val();
        let ItOtherCommission = $("#ItOtherCommission").val();
        let ItUATPAmount = $("#ItUATPAmount").val();
        let ItHandingFees = $("#ItHandingFees").val();
        let ItVATAmount = $("#ItVATAmount").val();
        let ItNetAmount = $("#ItNetAmount").val();
        let ItBillingAmount = $("#ItBillingAmount").val();

        let gb = 0;
        let ta = 0;
        let ia = 0;
        let oc = 0;
        let ua = 0;
        let hf = 0;
        let va = 0;
        let gb1 = 0;
        let ta1 = 0;
        let ia1 = 0;
        let oc1 = 0;
        let ua1 = 0;
        let hf1 = 0;
        let va1 = 0;

        let na = 0;
        let na1 = 0;
        let ba = 0;

        gb = Number(ItGrossBilled * 1);
        gb1 = Math.round(gb * 1000) / 1000;
        //console.log("gb1", gb1);
        //let tt = typeof(gb1);
        //console.log(tt);
        ta = ItTaxAmount * 1;
        ta1 = Math.round(ta * 1000) / 1000;
        //console.log("ta", ta);
        //let tta = typeof(ta);
        //console.log(tta);
        ia = Number(ItISCAmount * 1);
        ia1 = Math.round(ia * 1000) / 1000;
        oc = Number(ItOtherCommission * 1);
        oc1 = Math.round(oc * 1000) / 1000;
        ua = Number(ItUATPAmount * 1);
        ua1 = Math.round(ua * 1000) / 1000;
        hf = Number(ItHandingFees * 1);
        hf1 = Math.round(hf * 1000) / 1000;
        va = Number(ItVATAmount * 1);
        va1 = Math.round(va * 1000) / 1000;

        na1 = gb1 + ta1 + ia1 + oc1 + ua1 + hf1 + va1;
        /*if (na1 >= 1) {
            $alert({
            title: 'Alert!',
            content: 'Something\'s wrong, NetAmount is not correct!',
            //});
        };*/
        na = Math.round(na1 * 1000) / 1000;
        ba = na;

        $("#ItNetAmount").val(na);
        $("#ItBillingAmount").val(ba);
    }

    function ChargeAmount() {

        let CaGrossBilled = $("#CaGrossBilled").val();
        let CaIscPer = $("#CaIscPer").val();
        console.log("CaIscPer", CaIscPer);
        let CaIscAmount = $("#CaIscAmount").val();
        let CaOhterCommissionPer = $("#CaOhterCommissionPer").val();
        let CaOtherCommission = $("#CaOtherCommission").val();
        let CaUatpPer = $("#CaUatpPer").val();
        let CaUatp = $("#CaUatp").val();
        let CaHandlingFeesPer = $("#CaHandlingFeesPer").val();
        let CaHandlingFees = $("#CaHandlingFees").val();
        let CaVatAmountPer = $("#CaVatAmountPer").val();
        let CaVatAmount = $("#CaVatAmount").val()
        let CaTax = $("#CaTax");
        let CaNetAmount = $("#CaNetAmount").val();

        let ba1 = 0;
        let ta1 = 0;
        let isc1 = 0;
        let co1 = 0;
        let ua1 = 0;
        let hf1 = 0;
        let va1 = 0;
        let ba = 0;
        let ta = 0;
        let isc = 0;
        let co = 0;
        let ua = 0;
        let hf = 0;
        let va = 0;

        let ne1 = 0;
        let ne = 0;

        let IscPer = 0;
        let OthPer = 0;
        let UatpPer = 0;
        let HfPer = 0;
        let VatPer = 0;

        let IscPer1 = 0;
        let OthPer1 = 0;
        let UatpPer1 = 0;
        let HfPer1 = 0;
        let VatPer1 = 0;

        ba = Number(CaGrossBilled * 1);
        ba1 = Math.round(ba * 1000) / 1000;
        console.log("ba1", ba1);

        isc = Number(CaIscPer * 1);
        co = Number(CaOhterCommissionPer * 1);
        ua = Number(CaUatpPer * 1);
        hf = Number(CaHandlingFeesPer * 1);
        va = Number(CaVatAmountPer * 1);

        IscPer1 = Number(-1 * Math.abs(isc));
        console.log("IscPer1", IscPer1);
        OthPer1 = Number(1 * Math.abs(co));
        UatpPer1 = Number(1 * Math.abs(ua));
        HfPer1 = Number(1 * Math.abs(hf));  
        VatPer1 = Number(1 * Math.abs(va));

        IscPer = (IscPer1 / 100) * ba1;
        console.log("IscPer", IscPer);
        OthPer = (OthPer1 / 100) * ba1;
        UatpPer = (UatpPer1 / 100) * ba1;
        HfPer = (HfPer1 / 100) * ba1;
        VatPer = (VatPer1 / 100) * ba1;

        ta = CaTax * 1;
        ta1 = Math.round(ta * 1000) / 1000;

        isc1 = Math.round(IscPer * 1000) / 1000;
        co1 = Math.round(OthPer * 1000) / 1000;
        ua1 = Math.round(UatpPer * 1000) / 1000;
        hf1 = Math.round(HfPer * 1000) / 1000;
        va1 = Math.round(VatPer * 1000) / 1000;

        $("#CaIscPer").blur(function () {
            console.log("CaIscPer", CaIscPer);
            console.log("IscPer1", IscPer1);
            $("#CaIscPer").val(IscPer1);
            $("#CaIscAmount").val(IscPer);
        });
        $("#CaOhterCommissionPer").blur(function () {
            console.log("OthPer", OthPer);
            console.log("OthPer1", OthPer1);
            $("#CaOhterCommissionPer").val(OthPer1);
            $("#CaOhterCommission").val(OthPer);
        });
        $("#CaUatpPer").blur(function () {
            console.log("UatpPer", UatpPer);
            console.log("UatpPer1", UatpPer1);
            $("#CaUatpPer").val(UatpPer1);
            $("#CaUatp").val(UatpPer);
        });
        $("#CaHandlingFeesPer").blur(function () {
            console.log("HfPer", HfPer);
            console.log("HfPer1", HfPer1);
            $("#CaHandlingFeesPer").val(HfPer1);
            $("#CaHandlingFees").val(HfPer);
        });

        $("#CaVatAmountPer").blur(function () {
            console.log("VatPer", VatPer);
            console.log("VatPer1", VatPer1);
            $("#CaVatAmountPer").val(VatPer1);
            $("#CaVatAmount").val(VatPer);
        });

        ne = Number(ba1 + ta1 + isc1 + co1 + ua1 + hf1 + va1);
        ne1 = Math.round(ne * 1000) / 1000;
        $("#CaNetAmount").val(ne1);
    }

    function saveInforamtion() {
        //var dataToPost = $("#contenu").serialize();
        // get all values
        let txtCreditMemoNumber = $("#txtCreditMemoNumber").val();
        let txtInvoiceNumber = $("#txtInvoiceNumber").val();

        //save BMHeader
        let FROMAirline = $("#selectFrom").val();
        let ToAirline = $("#selectTo").val();
            

        //INSERT SECTION 1
        let txtBillingMonth = $("txtBillingMonth").val();
        let txtBillingPeriod = $("txtBillingPeriod").val();
        let txtOurRefIntUseFrom = $("#txtOurRefIntUseFrom").val();
        let txtYourInvoiceNumber = $("#txtYourInvoiceNumber").val();
        let txtYourBillingMonth = $("#txtYourBillingMonth").val();
        let txtYourBillingPeriod = $("#txtYourBillingPeriod").val();;
        let txtOurRefIntUseTo = $("#txtOurRefIntUseTo").val();
        let txtSourceCode = $("txtSourceCode").val();
        let txtCorrespondanceNumber = $("#txtCorrespondanceNumber").val();
        let txtAttachmentIndicatorOriginal = $("txtAttachmentIndicatorOriginal").val();
        let txtExchangeRate = $("txtExchangeRate").val();
        let txtCurrencyOfBillingMemo = $("txtCurrencyOfBillingMemo").val();
        let txtFIMNumber = $("txtFIMNumber").val();
        let txtFIMCouponNumber = $("txtFIMCouponNumber").val();
        let txtBatchNumber = $("txtBatchNumber").val();
        let txtSequenceNumber = $("txtSequenceNumber").val();
        let txtReasonCode = $("#reasonCodeDropdown").val();
        let txtSettlementMethod = $("txtSettlementMethod").val();


        //INSERT SECTION 2
        //on InvoiceTotals calcul
        let ItGrossBilled = $("#ItGrossBilled").val();
        let ItTaxAmount = $("#ItTaxAmount").val();
        let ItISCAmount = $("#ItISCAmount").val();
        let ItOtherCommission = $("#ItOtherCommission").val();
        let ItUATPAmount = $("#ItUATPAmount").val();
        let ItHandingFees = $("#ItHandingFees").val();
        let ItVATAmount = $("#ItVATAmount").val();
        let ItNetAmount = $("#ItNetAmount").val();
        let ItBillingAmount = $("#ItBillingAmount").val();

        //INSERT SQECXTION 3
        let IssuingAirline = $("#IssuingAirline").val();
        let AirlineFlightDesignator = $("#Carrier").val();
        let cpnD = $("#cpnD").val();
        let FlightNumber = $("#FlightNumber").val();
        let doc = $("#doc").val();
        let FlightDepartureDate = $("#FlightDepartureDate").val();
        let CheckDigit = $("#CheckDigit").val();
        let OriginCity = $("#OriginCity").val();
        let TicketingModeIndicator = $("#TicketingModeIndicator").val();
        let DestinationCity = $("#DestinationCity").val();
        let SettlementAuthorizationCode = $("#SettlementAuthorizationCode").val();
        let FareBasisTicketDesignator = $("#FareBasisTicketDesignator").val();
        let AgreementIndicator = $("#AgreementIndicator").val();
        let ReferenceField1 = $("#ReferenceField1").val();
        let OriginalPMI = $("#OriginalPMI").val();
        let ReferenceField2 = $("#ReferenceField2").val();
        let ValidateMPI = $("#ValidateMPI").val();
        let ReferenceField3 = $("#ReferenceField3").val();
        let ProrateMethodologie = $("#ProrateMethodologie").val();
        let ReferenceField4 = $("#ReferenceField4").val();
        let AirlineOwnUse = $("#ReferenceField4").val();
        let ReferenceField5 = $("#ReferenceField5").val();

        //on Charge Amount calcul
        let CaGrossBilled = $("#CaGrossBilled").val();
        let CaIscPer = $("#CaIscPer").val();
        let CaIscAmount = $("#CaIscAmount").val();
        let CaOhterCommissionPer = $("#CaOhterCommissionPer").val();
        let CaOtherCommission = $("#CaOtherCommission").val();
        let CaUatpPer = $("#CaUatpPer").val();
        let CaUatp = $("#CaUatp").val();
        let CaHandlingFeesPer = $("#CaHandlingFeesPer").val();
        let CaHandlingFees = $("#CaHandlingFees").val();
        let CaVatAmountPer = $("#CaVatAmountPer").val();
        let CaVatAmount = $("#CaVatAmount").val()
        let CaTax = $("#CaTax");
        let CaNetAmount = $("#CaNetAmount").val();

        let CurrencyAdjustmentIndicator = $("#CurrencyAdjustmentIndicator").val();

        $.ajax({
            type: "POST",

            url: "/BillingMemo/SaveInformations",
            dataType: "html",
            data: {
                txtCreditMemoNumber: txtCreditMemoNumber,
                txtInvoiceNumber: txtInvoiceNumber,
                FROMAirline: FROMAirline,
                ToAirline: ToAirline,
                txtBillingMonth: txtBillingMonth,
                txtBillingPeriod: txtBillingPeriod,
                txtOurRefIntUseFrom: txtOurRefIntUseFrom,
                txtYourInvoiceNumber: txtYourInvoiceNumber,
                txtYourBillingMonth: txtYourBillingMonth,
                txtYourBillingPeriod: txtYourBillingPeriod,
                txtOurRefIntUseTo: txtOurRefIntUseTo,
                txtSourceCode: txtSourceCode,
                txtCorrespondanceNumber: txtCorrespondanceNumber,
                txtAttachmentIndicatorOriginal: txtAttachmentIndicatorOriginal,
                txtExchangeRate: txtExchangeRate,
                txtCurrencyOfBillingMemo: txtCurrencyOfBillingMemo,
                txtFIMNumber: txtFIMNumber,
                txtFIMCouponNumber: txtFIMCouponNumber,
                txtBatchNumber: txtBatchNumber,
                txtSequenceNumber: txtSequenceNumber,
                txtReasonCode: txtReasonCode,
                txtSettlementMethod: txtSettlementMethod,
                
                ItGrossBilled: ItGrossBilled,
                ItTaxAmount: ItTaxAmount,
                ItISCAmount: ItISCAmount,
                ItOtherCommission: ItOtherCommission,
                ItUATPAmount: ItUATPAmount,
                ItHandingFees: ItHandingFees,
                ItVATAmount: ItVATAmount,
                ItNetAmount: ItNetAmount,
                ItBillingAmount: ItBillingAmount,

                IssuingAirline: IssuingAirline,
                AirlineFlightDesignator: AirlineFlightDesignator,
                cpnD: cpnD,
                FlightNumber: FlightNumber,
                doc: doc,
                FlightDepartureDate: FlightDepartureDate,
                CheckDigit: CheckDigit,
                OriginCity: OriginCity,
                TicketingModeIndicator: TicketingModeIndicator,
                DestinationCity: DestinationCity,
                SettlementAuthorizationCode: SettlementAuthorizationCode,
                FareBasisTicketDesignator: FareBasisTicketDesignator,
                AgreementIndicator: AgreementIndicator,
                ReferenceField1: ReferenceField1,
                OriginalPMI: OriginalPMI,
                ReferenceField2: ReferenceField2,
                ValidateMPI: ValidateMPI,
                ReferenceField3: ReferenceField3,
                ProrateMethodologie: ProrateMethodologie,
                ReferenceField4: ReferenceField4,
                AirlineOwnUse : AirlineOwnUse,
                ReferenceField5: ReferenceField5, 

                CaGrossBilled: CaGrossBilled,
                CaIscPer: CaIscPer,
                CaIscAmount: CaIscAmount,
                CaOhterCommissionPer: CaOhterCommissionPer,
                CaOtherCommission: CaOtherCommission,
                CaUatpPer: CaUatpPer,
                CaUatp: CaUatp,
                CaHandlingFeesPer: CaHandlingFeesPer,
                CaHandlingFees: CaHandlingFees,
                CaVatAmountPer: CaVatAmountPer,
                CaVatAmount: CaVatAmount,
                CaTax: CaTax,
                CaNetAmount: CaNetAmount
                
            },

            success: function(data) {
               alert("SAVE INFO success!!");
            },

            complete: function() {
                alert("SAVE INFO complete!!");
            },
            error: function() {
                console.log("SAVE INFO error!!!");
            }
        });
    }


    function GetTaxLog() {
        $.ajax({
            type: "GET",

            url: "/Process/Taxlog",
            dataType: "Html",
            data: {
            },
            success: function(data) {
                $('#taxLogDrop').html(data);
            }
        });
    }

    function GetTax() {
        $.ajax({
            type: "GET",

            url: "/BillingMemo/GetTax",
            dataType: "Html",
            data: {
               
            },

            success: function(data) {
                
            },
            complete: function() {

            }
        });
    }

    function GetTaxCode() {
        var sourceCode = $("#sourceCodeDrop").val();
        var taxLog = $("#taxLogDrop").val();

        //var res = sourceCode.split('|');

        //var Param0 = res[0];
        //var Param1 = res[1];

        //console.log("sourceCode: ", sourceCode);
        //console.log("taxLog: ", taxLog);

        $.ajax({
            type: "GET",

            url: "/BillingMemo/GetTaxCode",
            dataType: "Html",
            data: {
                sourceCode: sourceCode,
                taxLog: taxLog
                //Param0: Param0,
                //Param1 : Param1
            },

            success: function (data) {
                //alert("fct success!!");
                $('#taxBreakdownList').html(data);
            },
            complete: function() {
                //alert("fct complete!!!");
                GetTaxRate();
            }
        });
    }

    function GetTaxRate() {
        let lbl5drP = $("#TabRate #lbl5drP").text();
    }

    function ClearTaxLog() {
        var taxLogToDelete = $("#taxLogDrop").val();

        $.confirm({
            title: 'Confirm!',
            content: 'Are you sure to delete this record?',
            buttons: {
                confirm: function() {
                    $.ajax({
                        type: "GET",

                        url: "/Process/ClearTaxLog",
                        dataType: "Html",
                        data: {
                            taxLogToDelete: taxLogToDelete
                        },

                        success: function(data) {
                            $('#taxBreakdownList').html(data);
                        },
                        complete: function() {

                        }
                    });

                    $.alert('Confirmed!');
                },
                cancel: function() {
                    $.alert('Canceled!');
                }
            }
        });
    }

    //Get Tax Reference
    function TaxRefDetails(taxid) {
        var taxId = taxid;

        //console.log("taxRefID : ", taxId);

        $("#taxRefDetails").show();
        $("#taxRefAdd").css("visibility", "hidden");

        let url = "/BillingMemo/GetTaxRef";
        let urlDelete = "/BillingMemo/DeleteTaxRef";

        $.ajax({
            type: "GET",
            url: url,
            data: { taxId: taxId },

            success: function (data) {
                $("#taxRefDetails").html(data);
            },
            compete: function (data) {
                
            }
        });

        $("#RefTaxDelete").on("click",function( event ) {
            $.confirm({
                title: 'Confirm!',
                content: 'Do you really wanna delete this record',
                buttons: {
                    confirm: function () {
                        $.alert('Confirmed!');
                        $.ajax({
                            type: "GET",
                            url: urlDelete,
                            data: { taxId: taxId },

                            success: function (data) {
                                $("#taxRefDetails").html(data);
                            },
                            compete: function (data) {
                                $("#taxRefDetails").hide();
                                $("#taxRefAdd").css("visibility", "visible");
                            }
                        });
                    },
                    cancel: function () {
                        $.alert('Canceled!');
                    }
                }
            });
            $(this).off(event);
        });

    }

    function EdittaxRef() {
        //make idAction as 1 (action == Edit)
        idAction = 1;

        $("#TaxRefFromAirport").removeAttr("disabled");
        $("#taxRefToAirport").removeAttr("disabled");
        $("#taxRefMappedPrimeCode").removeAttr("disabled");
        $("#taxRefPassengerType").removeAttr("disabled");
        $("#taxRefDomInt").removeAttr("disabled");
        $("#taxRefTransitDuration").removeAttr("disabled");
        $("#taxRefValidFrom").removeAttr("disabled");
        $("#taxRefValidTo").removeAttr("disabled");
        $("#taxRefTaxCode").removeAttr("disabled");
        $("#taxRefTaxCurrency").removeAttr("disabled");
        $("#taxRefTaxAmount").removeAttr("disabled");
        $("#taxRefTaxPercentage").removeAttr("disabled");
    }

    function UpdateRefTax() {

        let url = "/BillingMemo/UpdateRefTax";

        let TaxRefID = $("#TaxRefID");
        let TaxRefFromAirport = $("#TaxRefFromAirport").val();
        let taxRefToAirport = $("#taxRefToAirport").val();
        let taxRefMappedPrimeCode = $("#taxRefMappedPrimeCode").val();
        let taxRefPassengerType = $("#taxRefPassengerType").val();
        let taxRefDomInt  = $("#taxRefDomInt").val();
        let taxRefTransitDuration  = $("#taxRefTransitDuration").val();
        let taxRefValidFrom  = $("#taxRefValidFrom").val();
        let taxRefValidTo = $("#taxRefValidTo").val();
        let taxRefTaxCode  = $("#taxRefTaxCode").val();
        let taxRefTaxCurrency  = $("#taxRefTaxCurrency").val();
        let taxRefTaxAmount = $("#taxRefTaxAmount").val();
        let taxRefTaxPercentage = $("#taxRefTaxPercentage").val();

        console.log("TaxRefFromAirport:", TaxRefFromAirport);
        console.log("taxRefToAirport:", taxRefToAirport);
        console.log("taxRefMappedPrimeCode:", taxRefMappedPrimeCode);
        console.log("taxRefPassengerType:", taxRefPassengerType);
        console.log("taxRefDomInt:", taxRefDomInt);
        console.log("taxRefTransitDuration:", taxRefTransitDuration);
        console.log("taxRefValidFrom:", taxRefValidFrom);
        console.log("taxRefValidTo:", taxRefValidTo);
        console.log("taxRefTaxCode:", taxRefTaxCode);
        console.log("taxRefTaxCurrency:", taxRefTaxCurrency);
        console.log("taxRefTaxAmount:", taxRefTaxAmount);
        console.log("taxRefTaxPercentage:", taxRefTaxPercentage);

        $.ajax({
            type: "POST",
            url: url,
            dataType: "json",
            data: {
                TaxRefID: TaxRefID,
                TaxRefFromAirport: TaxRefFromAirport,
                taxRefToAirport: taxRefToAirport,
                taxRefMappedPrimeCode: taxRefMappedPrimeCode,
                taxRefPassengerType: taxRefPassengerType,
                taxRefDomInt: taxRefDomInt,
                taxRefTransitDuration: taxRefTransitDuration,
                taxRefValidFrom: taxRefValidFrom,
                taxRefValidTo: taxRefValidTo,
                taxRefTaxCode: taxRefTaxCode,
                taxRefTaxCurrency: taxRefTaxCurrency,
                taxRefTaxAmount: taxRefTaxAmount,
                taxRefTaxPercentage: taxRefTaxPercentage            
            },
            cache: false,
            processData: false,
            success: function (result) {
                //$("#taxBreakdownList").html();
                $("#taxBreakdownUpdated").html();
            },
            complete: function (result) {
                alert("Lasaaaaaa!");
                /*$.alert({
                    title: 'Alert!',
                    content: 'Data Updated!',
                });*/
            }
        });

    
    }

    function AddRefTax() {
       
        $.confirm({
            title: 'Confirm!',
            content: 'Do you wanna add a new record?',
            buttons: {
                confirm: function () {

                    $("#taxRefDetails").hide();
                    $("#taxRefAdd").css("visibility", "visible");

                    $.alert('Confirmed!');

                    // make idAction as 0 (Add)
                    idAction = 0;

                },
                cancel: function () {
                    $.alert('Canceled!');
                }
            }
        });
    }

    function SaveRefTax() {
        let url = "/BillingMemo/SaveRefTax";

        let TaxRefFromAirportToAdd = $("#TaxRefFromAirportToAdd").val();
        let taxRefToAirportToAdd = $("#taxRefToAirportToAdd").val();
        let taxRefMappedPrimeCodeToAdd = $("#taxRefMappedPrimeCodeToAdd").val();
        let taxRefPassengerTypeToAdd = $("#taxRefPassengerTypeToAdd").val();
        let taxRefDomIntToAdd = $("#taxRefDomIntToAdd").val();
        let taxRefTransitDurationToAdd = $("#taxRefTransitDurationToAdd").val();
        let taxRefValidFromToAdd = $("#taxRefValidFromToAdd").val();
        let taxRefValidToToAdd = $("#taxRefValidToToAdd").val();
        let taxRefTaxCodeToAdd = $("#taxRefTaxCodeToAdd").val();
        let taxRefTaxCurrencyToAdd = $("#taxRefTaxCurrencyToAdd").val();
        let taxRefTaxAmountToAdd = $("#taxRefTaxAmountToAdd").val();
        let taxRefTaxPercentageToAdd = $("#taxRefTaxPercentageToAdd").val();

        console.log("TaxRefFromAirportToAdd:", TaxRefFromAirportToAdd);
        console.log("taxRefToAirportToAdd:", taxRefToAirportToAdd);
        console.log("taxRefMappedPrimeCodeToAdd:", taxRefMappedPrimeCodeToAdd);
        console.log("taxRefPassengerTypeToAdd:", taxRefPassengerTypeToAdd);
        console.log("taxRefDomIntToAdd:", taxRefDomIntToAdd);
        console.log("taxRefTransitDurationToAdd:", taxRefTransitDurationToAdd);
        console.log("taxRefValidFromToAdd:", taxRefValidFromToAdd);
        console.log("taxRefValidToToAdd:", taxRefValidToToAdd);
        console.log("taxRefTaxCodeToAdd:", taxRefTaxCodeToAdd);
        console.log("taxRefTaxCurrencyToAdd:", taxRefTaxCurrencyToAdd);
        console.log("taxRefTaxAmountToAdd:", taxRefTaxAmountToAdd);
        console.log("taxRefTaxPercentageToAdd:", taxRefTaxPercentageToAdd);

        $.ajax({
            type: "POST",
            url: url,
            data: {
                TaxRefFromAirportToAdd: TaxRefFromAirportToAdd,
                taxRefToAirportToAdd: taxRefToAirportToAdd,
                taxRefMappedPrimeCodeToAdd: taxRefMappedPrimeCodeToAdd,
                taxRefPassengerTypeToAdd: taxRefPassengerTypeToAdd,
                taxRefDomIntToAdd: taxRefDomIntToAdd,
                taxRefTransitDurationToAdd: taxRefTransitDurationToAdd,
                taxRefValidFromToAdd: taxRefValidFromToAdd,
                taxRefValidToToAdd: taxRefValidToToAdd,
                taxRefTaxCodeToAdd: taxRefTaxCodeToAdd,
                taxRefTaxCurrencyToAdd: taxRefTaxCurrencyToAdd,
                taxRefTaxAmountToAdd: taxRefTaxAmountToAdd,
                taxRefTaxPercentageToAdd: taxRefTaxPercentageToAdd
            },
            cache: false,
            processData: false,

            success: function (data) {
                $("#taxBreakdownList").html(data);
                $alert({
                    title: 'Alert!',
                    content: 'Simple alert!',
                });                
            }
        });

    }

    function clearTaxdRef() {
        if (idAction == 0) {
            $("#TaxRefFromAirportToAdd").val("");
            $("#taxRefToAirportToAdd").val("");
            $("#taxRefMappedPrimeCodeToAdd").val("");
            $("#taxRefPassengerTypeToAdd").val("");
            $("#taxRefDomIntToAdd").val("");
            $("#taxRefTransitDurationToAdd").val("");
            $("#taxRefValidFromToAdd").val("");
            $("#taxRefValidToToAdd").val("");
            $("#taxRefTaxCodeToAdd").val("");
            $("#taxRefTaxCurrencyToAdd").val("");
            $("#taxRefTaxAmountToAdd").val("");
            $("#taxRefTaxPercentageToAdd").val("");
        }
        if (idAction == 1) {
            $("#TaxRefFromAirport").val("");
            $("#taxRefToAirport").val("");
            $("#taxRefMappedPrimeCode").val("");
            $("#taxRefPassengerType").val("");
            $("#taxRefDomInt").val("");
            $("#taxRefTransitDuration").val("");
            $("#taxRefValidFrom").val("");
            $("#taxRefValidTo").val("");
            $("#taxRefTaxCode").val("");
            $("#taxRefTaxCurrency").val("");
            $("#taxRefTaxAmount").val("");
            $("#taxRefTaxPercentage").val("");
        }
       
    }

    function BMXML() {
        let url = "/BillingMemo/BMXML";

        let txtCreditMemoNumber = $("#txtCreditMemoNumber").val();
        let txtInvoiceNumber = $("#txtInvoiceNumber").val();
        let txtBillingMonth = $("#txtBillingMonth").val();
        let txtBillingPeriod = $("#txtBillingMonth").val();
        let cboBMBillingPeriod = $("#billingPeriodQuery").val();

        console.log("LOAD BMXML!!!!!");

        $.ajax({
            type: "GET",

            url: url,
            dataType: "Html",
            data: {
                txtCreditMemoNumber: txtCreditMemoNumber,
                txtInvoiceNumber: txtInvoiceNumber,
                txtBillingMonth: txtBillingMonth,
                txtBillingPeriod: txtBillingPeriod,
                cboBMBillingPeriod: cboBMBillingPeriod
            },

            success: function (data) {               
                $('#BMXML').html(data);
            },
            complete: function () {
               
            }
        });
    }