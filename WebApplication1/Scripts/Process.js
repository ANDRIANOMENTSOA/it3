//var symphony = "/symphony";
var symphony = "";

var date = new Date();
var currentDate = date.toISOString().slice(0, 10);

year = date.getFullYear(), // This method gets the four digit year.
month = date.getMonth() + 1, // This method gets the month and Jan is 0.
day = date.getDate(), // This method gets the day of month as a number.
hour = date.getHours(), // This method gets the hour 
min = date.getMinutes(); // This method gets the minutes
month = (month < 10 ? '0' + month : month);
day = (day < 10 ? '0' + day : day);
hour = (hour < 10 ? '0' + hour : hour); // It adds a 0 to number less than 10 because input[type=time] only accepts 00:00 format. 
min = (min < 10 ? '0' + min : min);

$('#FromManuelTicketScreen').val(day + '/' + month + '/' + year);
$('#datedtpIssueDateManuelticket').val(day + '/' + month + '/' + year);
$('#dateNotValidBefore').val(day + '/' + month + '/' + year);
$('#NotValidAfterdate').val(day + '/' + month + '/' + year);
$('#UsageDateManuel1').val(day + '/' + month + '/' + year);


function selectManuelTicketListe() {

}


var btn = document.querySelector('btninvo');
//btn.addEventListener('click', showInvolontaryReroute);

function showInvolontaryReroute(myid) {
	var etat = document.getElementById(myid).style.display;
	if (etat == 'none') {
		document.getElementById(myid).style.display = 'block';
		document.getElementById('btninvo').value = "Hide Add-on Details";
	}
	else {
		document.getElementById(myid).style.display = 'none';
		document.getElementById('btninvo').value = "Show Add-on Details";
	}
}
function showreportsInvolontaryReroute(id, classe) {
	let idCkeck = "";
	if ($("#" + id).is(':checked')) {

		if (id == "allkeyinvolontary") {
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
		if (id == "allkeyinvolontary") {
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


//////////////////// Proccess/////////////////
function selectManuelTicketListe() {
	let dateselect = $("#selectManuel").val();


	if (dateselect = "Date Of Issue") {
		let url = symphony + "/Process/ManualTicketList";
		$.ajax({
			type: 'POST',
			url: url,
			success: function (msg) {
				$('.tab-cancellation').html(msg);
			}, error: function (msg) {
				$('#errorModal').modal('show');
			}
		});
	}
	if (dateselect = "Date Updated") {
		let url = symphony + "/Process/DateUpdated";
		$.ajax({
			type: 'POST',
			url: url,
			success: function (msg) {
				$('.tab-cancellation').html(msg);
			}, error: function (msg) {
				$('#errorModal').modal('show');
			}
		});
	}
	if (dateselect = "Document Number") {
		let url = symphony + "/Process/DocumentNumberM";
		$.ajax({
			type: 'POST',
			url: url,
			success: function (msg) {
				$('.tab-cancellation').html(msg);
			}, error: function (msg) {
				$('#errorModal').modal('show');
			}
		});
	}
}
function baggageShow() {
	var etat = document.getElementById('bagAllow').checked;
	console.log(etat);
	if (etat) {
		document.getElementById('presetUnit').style.display = 'block'
	}
	else {
		document.getElementById('presetUnit').style.display = 'none'
	}
}

function showPreset() {
	if ($("#presetCode").val() == 'C') {
		$("#kilograms").val('30');
		$("#pound").val('60');
		$("#piece").val('1');
	}
	else if ($("#presetCode").val() == 'F') {
		$("#kilograms").val('40');
		$("#pound").val('80');
		$("#piece").val('1');
	}
	else if ($("#presetCode").val() == 'Y') {
		$("#kilograms").val('20');
		$("#pound").val('40');
		$("#piece").val('1');
	}
}

function editPreset() {
	$("#kilograms, #pound, #piece").prop("readonly", false);
}


/*----------------------------Final Share vs Amount collected---------------------------*/
function ClearFinalSharevsAmount(){
 let url = symphony + "/Process/FinalSharevsAmountCollected";
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
function SearchFinalSharevsAmount(){
	let document = $("#DocumentNoFS").val();
	let url = symphony + "/Process/SearchFinalSharevsAmount";
	tab =[document];
	$.ajax({
		type: 'POST',
		url: url,
		data: { dataValue: tab },
		beforeSend: function () {
			$('.ajax-loader').css("visibility", "visible");
		},
		success: function (data) {
			$("#search").html(data);
			console.log(data);
		}, 
		complete: function () {
			$('.ajax-loader').css("visibility", "hidden");
		},
			error: function () {
				$('#errorModal').modal('show');
			}
	});
	ShowTableFinalSharevAmount();
}
function ShowTableFinalSharevAmount(){
	let document = $("#DocumentNoFS").val();
	let url = symphony + "/Process/ShowtableFinalSharevsAmount";
	tab =[document];
	$.ajax({
		type: 'POST',
		url: url,
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
}
/************************File Upload Status***********************************/

function FileUploadStatusSelectFileType(){
  let dateFromFileU = $("#dateFromFileU").val();
	let dateToFileU = $("#dateToFileU").val();
	tab = [dateFromFileU, dateToFileU];
	let url = symphony + "/Process/FileUploadStatusSelectFileType";

	if ($("#testFDateFrom").val() != dateFromFileU || $("#testFDateTo").val() != dateToFileU) {
		$("#testFDateFrom").val(dateFromFileU);
		$("#testFDateTo").val(dateToFileU);
		$.ajax({
			type: 'POST',
			url: url,
			data: { dataValue: tab },
			success: function (data) {
				$("#SelecteFileType").html(data);
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

function FileUploadStatusSelectProcessingStatus(){
 let dateFromFileU = $("#dateFromFileU").val();
	let dateToFileU = $("#dateToFileU").val();
	let SelecteFileType =$("#SelecteFileType").val();
	tab = [dateFromFileU, dateToFileU,SelecteFileType];
	let url = symphony + "/Process/FileUploadStatusSelectProcessingStatus";

	if ($("#testFDateFrom").val() != dateFromFileU || $("#testFDateTo").val() != dateToFileU || $("#testFileType").val() != SelecteFileType ) {
		$("#testFDateFrom").val(dateFromFileU);
		$("#testFDateTo").val(dateToFileU);
		$("#testFileType").val(SelecteFileType);
		$.ajax({
			type: 'POST',
			url: url,
			data: { dataValue: tab },
			success: function (data) {
				$("#SelectProcessingStatus").html(data);
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

function FileUploadStatusSelectFileName(){
	let dateFromFileU = $("#dateFromFileU").val();
	let dateToFileU = $("#dateToFileU").val();
	let SelecteFileType = $("#SelecteFileType").val();
	let SelectProcessingStatus = $("#SelectProcessingStatus").val();

	tab = [dateFromFileU, dateToFileU,SelecteFileType,SelectProcessingStatus];
	let url = symphony + "/Process/FileUploadStatusSelectFileName";

	if ($("#testFDateFrom").val() != dateFromFileU || $("#testFDateTo").val() != dateToFileU || $("#testFileType").val() != SelecteFileType|| $("#testSatus").val() != SelectProcessingStatus  ) {
		$("#testFDateFrom").val(dateFromFileU);
		$("#testFDateTo").val(dateToFileU);
		$("#testFileType").val(SelecteFileType);
		$("#testSatus").val(SelectProcessingStatus);
		
		$.ajax({
			type: 'POST',
			url: url,
			data: { dataValue: tab },
			success: function (data) {
				$("#SelectFileName").html(data);
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

function FileUploadStatussearch(){
	 let dateFromFileU = $("#dateFromFileU").val();
	let dateToFileU = $("#dateToFileU").val();
	let SelecteFileType = $("#SelecteFileType").val();
	let SelectProcessingStatus = $("#SelectProcessingStatus").val();
	let SelectFileName = $("#SelectFileName").val();

	  let radioDetails = $("#sqlCriteria input[name=radio]:radio:checked").val();
	 // let radioSummary = $("#sqlCriteria input[name=radio]:radio:checked").val();

	console.log(radioDetails);
	console.log(radioSummary);

	tab = [dateFromFileU,dateToFileU,SelecteFileType,SelectProcessingStatus,SelectFileName,radioDetails];
	let url = symphony + "/Process/FileUploadStatussearch";

	if ($("#testFDateFrom").val() != dateFromFileU || $("#testFDateTo").val() != dateToFileU || $("#testFileType").val() != SelecteFileType|| $("#testSatus").val() != SelectProcessingStatus|| $("#testFileName").val() != SelectFileName) {
		$("#testFDateFrom").val(dateFromFileU);
		$("#testFDateTo").val(dateToFileU);
		$("#testFileType").val(SelecteFileType);
		$("#testSatus").val(SelectProcessingStatus);
		$("#testFileName").val(SelectFileName);

		$.ajax({
			type: 'POST',
			url: url,
			data: { dataValue: tab },
			success: function (data) {
				$("#tableFileupload").html(data);
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
/*********************Involontary Reroute****************************/

function InvolontaryRerouteSearch(){
	let FromInvolintaryReroute = $("#FromInvolintaryReroute").val();
	let ToInvolintaryReroute = $("#ToInvolintaryReroute").val();

	tab = [FromInvolintaryReroute, ToInvolintaryReroute];
	let url = symphony + "/Process/InvolontaryRerouteSearch";

	if ($("#testFDateFromInvolu").val() != FromInvolintaryReroute || $("#ToInvolintaryReroute").val() != ToInvolintaryReroute ) {
		$("#testFDateFromInvolu").val(FromInvolintaryReroute);
		$("#ToInvolintaryReroute").val(ToInvolintaryReroute);

		$.ajax({
			type: 'POST',
			url: url,
			data: { dataValue: tab },
			success: function (data) {
				$("#tableInvolontaryReroute").html(data);
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

/************************ EMD Transaction Screen**********/
function SearchEMD() {
	let dateFromEMD = $("#datedtpIssueDateEMD").val();
	let dateToEMD = $("#datedtpIssueDateToEMD").val();
	let documentNum = $("#DocumentNumberEMD").val();

	let checkboxFrom = $("#sqlCriteriaEMD input[name=DateFromEMD]:checkbox:checked").val();
	let checkboxTo = $("#sqlCriteriaEMD input[name=DateToEMD]:checkbox:checked").val();

		tab = [dateFromEMD, dateToEMD,checkboxFrom,checkboxTo,documentNum];
		  let url = symphony + "/Process/SearchEMD";

	if ($("#testFrom").val() != dateFromEMD || $("#testTo").val() != dateToEMD ) {
		$("#testFrom").val(dateFromEMD);
		$("#testTo").val(dateToEMD);

		$.ajax({
			type: 'POST',
			url: url,
			data: { dataValue: tab },
			success: function (data) {
				$("#tableEMD").html(data);
			},
			complete: function () {
			   /* $('.ajax-loader').css("visibility", "hidden");
				var mes = $('#message').val();
				var mes1 = $('#message1').val();
				if (mes !== "") {
					$('#alertModal').modal('show');
					$("#msgalert").html(mes);
				}
				else{
					$('#alertModal').modal('show');
					$("#msgalert").html(mes1);
				}*/
			},

			error: function () {
			   // $('#errorModal').modal('show');
			}
		});
	}
}

function ShowSearch(event, myvar) {
	 
	 document.getElementById("EMDTransactionScreenshow").style.display = "block"; 
	 let documentNum = myvar;
	 //$("#DOCUMENTNOEMD").val(documentNum);
	 //console.log(documentNum);
	 tab = [documentNum];
	let url = symphony + "/Process/ShowSearchEMD";
	$.ajax({
			type: 'POST',
			url: url,
			data: { dataValue: tab },
			success: function (data) {
				$("#IdShowSearchEMD").html(data);
			},
			complete: function () {
			 $('.ajax-loader').css("visibility", "hidden");
				var mes = $('#message').val();
				var mes1 = $('#message1').val();
				if (mes !== "") {
					$('#alertModal').modal('show');
					$("#msgalert").html(mes);
				}
			},

			error: function () {
			   // $('#errorModal').modal('show');
			}
		});
}


function radioEMD(){

		let DocumentNum = $("#DOCUMENTNOEMD").val();
		let Related = $("#RELATEDTICKETNOEMD").val();
		let url = symphony + "/Process/EMDShowView";

  $("input:radio[name='groupOfDefaultRadios']").change(function(){
		if($(this).val() == "EMD"){
			 tab = [DocumentNum];
			 $.ajax({
						type: 'POST',
						url: url,
						data: { dataValue: tab },
						success: function (data) {
							$("#IdEMShowview").html(data);
						},
						complete: function () {
						  var ch = $('#checkIt').val();
						  if(ch = "oui"){
							document.getElementById("checkIt").checked = true;
						  }
						  else{
						  document.getElementById("checkIt").checked = false;
						  }
						},
						error: function () {
						   // $('#errorModal').modal('show');
						}
					});
			}   
		else if($(this).val() == "RELATED DOCUMENT") {
			tab = [Related];
			let url1 = symphony + "/Process/RELATEDDocumentShowView";
			$.ajax({
						type: 'POST',
						url: url1,
						data: { dataValue: tab },
						success: function (data) {
						console.log(data);
							$("#IdEMShowview").html(data);
						},
						complete: function () {
						},

						error: function () {
						   // $('#errorModal').modal('show');
						}
					});
			}
	});
}

function clearEMD(){
	 let url = symphony + "/Process/EMDTransactionScreen";
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
function searchATDA(){
	DocNo();
	Fare();
	tax();
	FOP();
	Ancillaries();
	Modifiedtkkt();
}
function DocNo(){

	 let documentNum = $("#ATDNo").val();
	 if( documentNum == "") 
	 {
	 }
	 else{
	 tab = [documentNum];
	 let url = symphony + "/Process/searchATDA";
	$.ajax({
			type: 'POST',
			url: url,
			data: { dataValue: tab },
			success: function (data) {
				$("#ATDAGroupe").html(data);
			},
			complete: function () {
			/* $('.ajax-loader').css("visibility", "hidden");
				var mes = $('#message').val();
				var mes1 = $('#message1').val();
				if (mes !== "") {
					$('#alertModal').modal('show');
					$("#msgalert").html(mes);
				}*/
			},

			error: function () {
			   // $('#errorModal').modal('show');
			}
		});
		}
}
function Fare(){
	 let documentNum = $("#ATDNo").val();
	 tab = [documentNum];
	 let url = symphony + "/Process/FareATDA";
	$.ajax({
			type: 'POST',
			url: url,
			data: { dataValue: tab },
			success: function (data) {
			console.log("ok");
			 $("#IdSearchATDA").html(data);
			},
			complete: function () {
			},

			error: function () {
			   // $('#errorModal').modal('show');
			}
		});
}

 //DISPLAY DATAGRID TAX & SURCHARGE INFO

 function tax(){
	let documentNum = $("#ATDNo").val();
	 tab = [documentNum];
	 let url = symphony + "/Process/tax";
	$.ajax({
			type: 'POST',
			url: url,
			data: { dataValue: tab },
			success: function (data) {
			//console.log(data);
			 $("#idTaxATDA").html(data);
			},
			complete: function () {
			},

			error: function () {
			   // $('#errorModal').modal('show');
			}
		});
 }

  //DISPLAY FORM OF PAYMENT INFO
function FOP(){
	let documentNum = $("#ATDNo").val();
	 tab = [documentNum];
	 let url = symphony + "/Process/ATDAFOP";
	$.ajax({
			type: 'POST',
			url: url,
			data: { dataValue: tab },
			success: function (data) {
			//console.log(data);
			 $("#ATDAGroupe").html(data);
			},
			complete: function () {
			},

			error: function () {
			   // $('#errorModal').modal('show');
			}
		});
 }

	 //DISPLAY DATAGRID ANCILLARIES INFO
	 
	 function Ancillaries(){
		let documentNum = $("#ATDNo").val();
			tab = [documentNum];
			let url = symphony + "/Process/ATDAAncillaries";
			$.ajax({
					type: 'POST',
					url: url,
					data: { dataValue: tab },
					success: function (data) {
					//console.log(data);
					 $("#idAncillaries").html(data);
					},
					complete: function () {
					},

					error: function () {
					   // $('#errorModal').modal('show');
					}
				});
	 }

	 //DISPLAY IF DOCUMENT HAS BEEN MODIFIED
	 function Modifiedtkkt(){
	 let documentNum = $("#ATDNo").val();
			tab = [documentNum];
			let url = symphony + "/Process/ATDAModifiedtkkt";
			$.ajax({
					type: 'POST',
					url: url,
					data: { dataValue: tab },
					success: function (data) {
						//console.log(data);
						// $("#idAncillaries").html(data);
					},
					complete: function () {
					},

					error: function () {
					   // $('#errorModal').modal('show');
					}
				});
	 }
	 function clearATDA() {
	     $("#carr").val('');
	     $("#dn").val('');
	     $("#chdg").val('');
	     $("#orgdest").val('');
	     $("#pnr").val('');
	     $("#vif").val('');
	     $("#sif").val('');
	     $("#cpui").val('');
	     $("#it").val('');
	     $("#tourcod").val('');
	     $("#passnam").val('');
	     $("#reldoc").val('');
	     $("#didoc").val('');
	     $("#angumcode").val('');
	     $("#agname").val('');
	     $("#pos").val('');
	     $("#bagid").val('');
	     $("#agtcom").val('');
	     $("#farcur").val('');
	     $("#tfar").val('');
	     $("#tsurch").val('');
	     $("#surgcur").val('');
	     $("#tefp").val('');
	     $("#efpcur").val('');
	     $("#tcarfur").val('');
	     $("#tcafmat").val('');
	     $("#amtcolcur").val('');
	     $("#tfarcomp").val('');
	     $("#eadc").val('');
	     $("#ahcod").val('');
	     $("#endoarea").val('');
	     $("#inconnwith").val('');
	     $("#blocno").val('');
	     $("#tfcmi").val('');
	     $("#ttragroup").val('');
	     $("#tinvre").val('');
	     $("#tpaxtyp").val('');
	     $("#tspc").val('');
	     $("#tadno").val('');
	     $("#tadminfar").val('');
	     $("#tadmfp").val('');
	     $("#tadmtfc").val('');
	     $("#tfamt").val('');
	     $("#tmodfarcur").val('');
	     $("#tofarcur").val('');
	     $("#torfaamt").val('');
	     $("#torefar").val('');
	     $("#tresequi").val('');
	     $("#torusd").val('');
	     $("#tdiffarcop").val('');
	     $("#tdifcoc").val('');
	     $("#tabCTD").html('');
	     $("#idAncillaries").html('');
	     $("#idTaxATDA").html('');
	     $("#tdCB").html('');
	     $("#fopATDA").html('');
	     $("#tdchekcoupondetail").html('');
	     $("#coupondetails").html('');
	     $("#tddiscep").html('');
	     $("#tdtabshow").html('');
	     $("#tdrbdchek").html('');
	     $("#tddate").html('');
	 }
/*******************TolotraProcess***********************/
/*******************Free And Reduced***********************/

function searchFreeAndReduced() {
	let dateFrom = $("#dateFromFreeReduced").val();
	let dateTo = $("#dateToFreeReduced").val();
	let excChk="";
	if ($("#ExcludedId").is(':checked')) {
		excChk="1"
	}
	else {
		excChk = "0"
	}
	
	let url = symphony + "/Process/LoadFreeAndReduced";

	$.ajax({
		type: "POST",
		url: url,
		data: { dateFrom: dateFrom, dateTo: dateTo, excChk: excChk },

		beforeSend: function () {
			$('.ajax-loader').css("visibility", "visible");
		},
		success: function (data) {
			$("#loadFreeAndReduced").html(data);
		},
		complete: function () {
			$('.ajax-loader').css("visibility", "hidden");
			$("#page1").val(1);
			 var cmpt = $('#comptFree').val();
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

/*******************End Free And Reduced***********************/
/*******************Special Purpose***********************/

function searchSpecialPurpose() {
	let dateFrom = $("#dateFromSpecialPurp").val();
	let dateTo = $("#dateToSpecialPurp").val();
	let agspc = $("#agspc").val();  
	let url = symphony + "/Process/LoadSpecialPurpose";
	
	$.ajax({
		type: "POST",
		url: url,
		data: { dateFrom: dateFrom, dateTo: dateTo, agspc:agspc },

		beforeSend: function () {
			$('.ajax-loader').css("visibility", "visible");
		},
		success: function (data) {
			$("#loadSpecialPurpose").html(data);

		},
		complete: function () {
			$('.ajax-loader').css("visibility", "hidden");
			$("#txtSPCCount").val($("#txtCountSpec").val());
			 
			var cmpt = $('#comptSpec').val();
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
function recupeAgtSpc() {
	let dateFrom = $("#dateFromSpecialPurp").val();
	let dateTo = $("#dateToSpecialPurp").val();
   
	let url = symphony + "/Process/LoadAgtSPC";
	if ($("#testspc").val() != dateFrom || $("#testspc1").val() != dateTo) {
		$("#testspc").val(dateFrom);
		$("#testspc1").val(dateTo);
		$.ajax({
			type: "POST",
			url: url,
			data: { dateFrom: dateFrom, dateTo: dateTo },

			beforeSend: function () {
				$('.ajax-loader').css("visibility", "visible");
			},
			success: function (data) {
				$("#agspc").html(data);

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
function clearSPC() {
	let url = symphony + "/Process/SpecialPurpose";
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
/*******************End Special Purpose***********************/

/**************POS SUmmary*********************/
function recupeNamePOS() {
	let dateFrom = $("#dateFromPOS").val();
	let dateTo = $("#dateToPOS").val();

	let url = symphony + "/Process/LoadPOS";
	if ($("#testpos").val() != dateFrom || $("#testpos1").val() != dateTo) {
		$("#testpos").val(dateFrom);
		$("#testpos1").val(dateTo);
		$.ajax({
			type: "POST",
			url: url,
			data: { dateFrom: dateFrom, dateTo: dateTo },

			beforeSend: function () {
				$('.ajax-loader').css("visibility", "visible");
			},
			success: function (data) {
				$("#namepos").html(data);

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
function recupeFCMI() {
	let dateFrom = $("#dateFromPOS").val();
	let dateTo = $("#dateToPOS").val();

	let url = symphony + "/Process/LoadFCMI";
	if ($("#testfcmi").val() != dateFrom || $("#testfcmi1").val() != dateTo) {
		$("#testfcmi").val(dateFrom);
		$("#testfcmi1").val(dateTo);
		$.ajax({
			type: "POST",
			url: url,
			data: { dateFrom: dateFrom, dateTo: dateTo },

			beforeSend: function () {
				$('.ajax-loader').css("visibility", "visible");
			},
			success: function (data) {
				$("#FCMI").html(data);

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
function recupeFareBasis() {
	let dateFrom = $("#dateFromPOS").val();
	let dateTo = $("#dateToPOS").val();

	let url = symphony + "/Process/LoadFareBasis";
	if ($("#testFareBasis").val() != dateFrom || $("#testFareBasis1").val() != dateTo) {
		$("#testFareBasis").val(dateFrom);
		$("#testFareBasis1").val(dateTo);
		$.ajax({
			type: "POST",
			url: url,
			data: { dateFrom: dateFrom, dateTo: dateTo },

			beforeSend: function () {
				$('.ajax-loader').css("visibility", "visible");
			},
			success: function (data) {
				$("#FareBasis").html(data);

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
function recupeFopPOS() {
	let dateFrom = $("#dateFromPOS").val();
	let dateTo = $("#dateToPOS").val();

	let url = symphony + "/Process/LoadFopPOS";
	if ($("#testFopPOS").val() != dateFrom || $("#testFopPOS1").val() != dateTo) {
		$("#testFopPOS").val(dateFrom);
		$("#testFopPOS1").val(dateTo);
		$.ajax({
			type: "POST",
			url: url,
			data: { dateFrom: dateFrom, dateTo: dateTo },

			beforeSend: function () {
				$('.ajax-loader').css("visibility", "visible");
			},
			success: function (data) {
				$("#FopPOS").html(data);

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
function recupeAgtPOS() {
	let dateFrom = $("#dateFromPOS").val();
	let dateTo = $("#dateToPOS").val();

	let url = symphony + "/Process/LoadAgtPOS";
	if ($("#testAgtPOS").val() != dateFrom || $("#testAgtPOS1").val() != dateTo) {
		$("#testAgtPOS").val(dateFrom);
		$("#testAgtPOS1").val(dateTo);
		$.ajax({
			type: "POST",
			url: url,
			data: { dateFrom: dateFrom, dateTo: dateTo },

			beforeSend: function () {
				$('.ajax-loader').css("visibility", "visible");
			},
			success: function (data) {
				$("#AgtPOS").html(data);

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

function searchPOSSummary() {
	let dateFrom = $("#dateFromPOS").val();
	let dateTo = $("#dateToPOS").val();
	let agpos = $("#AgtPOS").val();
	let namepos = $("#namepos").val();
	let fcmi = $("#FCMI").val();
	let farebas = $("#FareBasis").val();
	let fopP = $("#FopPOS").val();
	let url = symphony + "/Process/LoadPOSSummary";
	$.ajax({
		type: "POST",
		url: url,
		data: { dateFrom: dateFrom, dateTo: dateTo, agpos: agpos,namepos:namepos,fcmi:fcmi,farebas:farebas,fopP:fopP },

		beforeSend: function () {
			$('.ajax-loader').css("visibility", "visible");
		},
		success: function (data) {
			$("#loadPOSSummary").html(data);

		},
		complete: function () {
			$('.ajax-loader').css("visibility", "hidden");
			$("#TotPOS").val($("#txtCountPOS").val());

			var cmpt = $('#comptPOS').val();
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
function searchPOSSummaryDetails(agpos, namepos, purchase,farebas,fcmi,fopP,travelday,lapse,remcur) {
	let dateFrom = $("#dateFromPOS").val();
	let dateTo = $("#dateToPOS").val();

	let url = symphony + "/Process/LoadPOSSummaryDetails";
	$.ajax({
		type: "POST",
		url: url,
		data: { dateFrom: dateFrom, dateTo: dateTo, agpos: agpos, namepos: namepos, purchase: purchase, farebas: farebas, fcmi: fcmi, fopP: fopP,travelday:travelday,lapse:lapse,remcur:remcur },

		beforeSend: function () {
			$('.ajax-loader').css("visibility", "visible");
		},
		success: function (data) {
			$("#loadPOSSummary").html(data);

		},
		complete: function () {
			$('.ajax-loader').css("visibility", "hidden");
			$("#TotPOS").val($("#txtCountPOSDetails").val());

			var cmpt = $('#comptPOSDetails').val();
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

function setNumericodePOS() {

	let a = $('#agent-code-label').text();
	let b = $("#agent-name-label").text();
	//$('#AgtPOS').text(a);
	//$('#' + id + ' option[value="1"]').text(a);
	$("#AgtPOS").html("<option>" + a + "</option>")

	$('#locPOSS').val(b);
   
}
/************** EndPOS SUmmary*********************/

/*******************End TolotraProcess***********************/

//* Joseph ////


/*  Tab Main and prorate factor in transaction in a Nutshell    Joseph */

function searchTransaction() {

	 let number = $("#txtTicketNo").val();
	$.ajax({
		type: "POST",
		url: "/Process/ResultatTransactionInANutshell",
		data: { number: number },

		beforeSend: function () {
			$('.ajax-loader').css("visibility", "visible");
		},
		success: function (data) {
			$("#donne").html(data);

		},
		complete: function () {
			$('.ajax-loader').css("visibility", "hidden");
			$("#txtnum").val($("#valtxtnum").val());
			$("#txtFare").val($("#valfare").val());
			$("#txtEfp").val($("#valEfp").val());
			$("#dtpDateOfSale").val($("#valdtpDateOfSale").val());
			$("#txtDateOfSale").val($("#valtxtDateOfSale").val());
			$("#txtFca").val($("#valFca").val());
			$("#txtOrigIssueDate").val($("#valtxtOrigIssueDate").val());
			$("#txtrel").val($("#valrel").val());
		}
	});

}

function searchTransactionProration() {
	let number = $("#txtTicketNo").val();
	$.ajax({
		type: "POST",
		url: "/Process/Proration",
		data: { number: number },

		beforeSend: function () {
			$('.ajax-loader').css("visibility", "visible");
		},
		success: function (data) {
			$("#donne").html(data);

		},
		complete: function () {
			$('.ajax-loader').css("visibility", "hidden");
			$("#txtnum").val($("#valtxtnum").val());
			$("#txtFare").val($("#valfare").val());
			$("#txtEfp").val($("#valEfp").val());
			$("#dtpDateOfSale").val($("#valdtpDateOfSale").val());
			$("#txtDateOfSale").val($("#valtxtDateOfSale").val());
			$("#txtFca").val($("#valFca").val());
			$("#txtOrigIssueDate").val($("#valtxtOrigIssueDate").val());
			$("#txtrel").val($("#valrel").val());
		}
	});
}


/*  End Tab Main and prorate factor in transaction in a Nutshell*/


/*  ATDs With Endorsements/Restrictions  Joseph  */

function searchEndorsementsRestrictions() {

	let dateFrom = $("#datedtpIssueDateFrom").val();
	let dateTo = $("#dtpIssueDateTo").val();

	let url = symphony + "/Process/EndorsementsRestrictions";
	$.ajax({
		type: 'POST',
		url: url,
		data: { dateFrom: dateFrom, dateTo: dateTo},
		beforeSend: function () {
			$('.ajax-loader').css("visibility", "visible");
		},
		success: function (data) {
			$("#affichage").html(data);
		   
		},
		complete: function () {
			$('.ajax-loader').css("visibility", "hidden");
			tableEndorsements();

			let nbrER = $("#nbEr").val();

			if (nbrER == 0) {
				$('#errorModal').modal('show');
			}
		},
	});
}


function tableEndorsements() {
	$(document).ready(function () {
		$('#TableEndorsements').DataTable({
			"pageLength": 100,
			dom: 'Bfrtip',
			buttons: [
				 'csv'
			]
		});
	});
}


/* End  ATDs With Endorsements/Restrictions  */


/* ATDs With Exchanged Docs  Joseph */

function searchExchangedDocs() {

	let dateFrom = $("#datedtpIssueDateFromED").val();
	let dateTo = $("#datedtpIssueDateToED").val();


	let url = symphony + "/Process/ATDExchangedDocs";
	$.ajax({
		type: 'POST',
		url: url,
		data: { dateFrom: dateFrom, dateTo: dateTo },
		beforeSend: function () {
			$('.ajax-loader').css("visibility", "visible");
		},
		success: function (data) {
			$("#affichageDonne").html(data);

		},
		complete: function () {
			$('.ajax-loader').css("visibility", "hidden");
			tableExchangedDocs();
			
			let nbrED = $("#nbED").val();

			if (nbrED == 0) {
				$('#errorModal').modal('show');
			}
		},
	});

}

function tableExchangedDocs() {
	$(document).ready(function () {
		$('#TableExchanged').DataTable({
			"pageLength": 100,
			dom: 'Bfrtip',
			buttons: [
				 'csv'
			]
		});
	});
}

/* End ATDs With Exchanged Docs*/




/* ATDs With Conjuction Tickets  Joseph */

function searchConjuctionTickets() {

	let dateFrom = $("#datedtpIssueDateFromCT").val();
	let dateTo = $("#datedtpIssueDateToCT").val();

	let url = symphony + "/Process/ATDConjuctionTickets";
	$.ajax({
		type: 'POST',
		url: url,
		data: { dateFrom: dateFrom, dateTo: dateTo },
		beforeSend: function () {
			$('.ajax-loader').css("visibility", "visible");
		},
		success: function (data) {
			$("#affichageCT").html(data);

		},
		complete: function () {
			$('.ajax-loader').css("visibility", "hidden");
			tableConjuctionTickets();
			let nb = $("#nbCT").val();
			if (nb == 0) {
				$('#errorModal').modal('show');
			}
		},
	});

}

function tableConjuctionTickets() {
	$(document).ready(function () {
		$('#TableConjuctionTickets').DataTable({
			"pageLength": 100,
			dom: 'Bfrtip',
			buttons: [
				 'csv'
			]
		});
	});
}


/* ATDs With Conjuction Tickets */


/* ATDs With UATP FOP Joseph  */
function searchUatpFop() {
	let dateFrom = $("#dateFromUatpFop").val();
	let dateTo = $("#dateToUatpFop").val();


	let url = symphony + "/Process/ATDUatpFop";
	$.ajax({
		type: 'POST',
		url: url,
		data: { dateFrom: dateFrom, dateTo: dateTo },
		beforeSend: function () {
			$('.ajax-loader').css("visibility", "visible");
		},
		success: function (data) {
			$("#affichageUatpFop").html(data);

		},
		complete: function () {
			$('.ajax-loader').css("visibility", "hidden");
			tableUatpFop();
			let nb = $("#nbUatpFop").val();
			if (nb == 0) {
				$('#errorModal').modal('show');
			}
		},
	});
}

function tableUatpFop() {
	$(document).ready(function () {
		$('#TabletableUatpFop').DataTable({
			"pageLength": 100,
			dom: 'Bfrtip',
			buttons: [
				 'csv'
			]
		});
	});
}


/* End ATDs With UATP FOP Joseph  */


/* Expired ATDs Partially Unused  Joseph */
function searchPartiallyUnused() {

	let dateFrom = $("#dateFromPU").val();
	let dateTo = $("#dateToPU").val();

	let url = symphony + "/Process/ATDPartiallyUnused";
	$.ajax({
		type: 'POST',
		url: url,
		data: { dateFrom: dateFrom, dateTo: dateTo },
		beforeSend: function () {
			$('.ajax-loader').css("visibility", "visible");
		},
		success: function (data) {
			$("#affichagePU").html(data);
			

		},
		complete: function () {
			$('.ajax-loader').css("visibility", "hidden");
			tablePartiallyUnused();

			let nb = $("#nbPU").val();
			if (nb == 0) {
				$('#errorModal').modal('show');
			}
		},
	});

}

function tablePartiallyUnused() {
	$(document).ready(function () {
		$('#TablePartiallyUnused').DataTable({
			"pageLength": 100,
			dom: 'Bfrtip',
			buttons: [
				 'csv'
			]
		});
	});
}

/* End Expired ATDs Partially Unused  */


/* Expired ATDs Totally Unused   Joseph  */

function searchTotallyUnused() {

	let dateFrom = $("#dateFromTU").val();
	let dateTo = $("#dateToTU").val();

	let url = symphony + "/Process/ATDTotallyUnused";
	$.ajax({
		type: 'POST',
		url: url,
		data: { dateFrom: dateFrom, dateTo: dateTo },
		beforeSend: function () {
			$('.ajax-loader').css("visibility", "visible");
		},
		success: function (data) {
			$("#affichageTU").html(data);

		},
		complete: function () {
			$('.ajax-loader').css("visibility", "hidden");
			let nb = $("#nbTU").val();
			tableTotallyUnused()();

			if (nb == 0) {
				$('#errorModal').modal('show');
			}
		},
	});

}

function tableTotallyUnused() {
	$(document).ready(function () {
		$('#TableTotallyUnused').DataTable({
			"pageLength": 100,
			dom: 'Bfrtip',
			buttons: [
				 'csv'
			]
		});
	});
}

/* End Expired ATDs Totally Unused  */


/* Tickets With Transit Points  Joseph */
function searchTicketsTransitP() {

	let dateFrom = $("#dateFromTP").val();
	let dateTo = $("#dateToTP").val();

	let url = symphony + "/Process/TicketsTransitPoints";
	$.ajax({
		type: 'POST',
		url: url,
		data: { dateFrom: dateFrom, dateTo: dateTo },
		beforeSend: function () {
			$('.ajax-loader').css("visibility", "visible");
		},
		success: function (data) {
			$("#affichageTP").html(data);

		},
		complete: function () {
			$('.ajax-loader').css("visibility", "hidden");
			let nb = $("#nbTP").val();
			tableTicketsTransitP();
			if (nb == 0) {
				$('#errorModal').modal('show');
			}
		},
	});

}

function tableTicketsTransitP() {
	$(document).ready(function () {
		$('#TableTicketsTransitP').DataTable({
			"pageLength": 100,
			dom: 'Bfrtip',
			buttons: [
				 'csv'
			]
		});
	});
}

/* End Tickets With Transit Points  */


/* Sales Transaction By Type      Joseph   */
function searchSalesTransactionBT() {

	let dateFrom = $("#dateFromSalesTbt").val();
	let dateTo = $("#dateToSalesTbt").val();
	let transactionType = $("#item").val();
	var tr = "";

	if (transactionType == "-ALL-") {
		tr = "%";
	} else {
		tr = $("#item").val();
	}

	let url = symphony + "/Process/SalesTransactionBT?param="+tr;
	$.ajax({
		type: 'POST',
		url: url,
		data: { dateFrom: dateFrom, dateTo: dateTo, transactionType: transactionType },
		beforeSend: function () {
			$('.ajax-loader').css("visibility", "visible");
		},
		success: function (data) {
			$("#affichageSBT").html(data);
			document.getElementById("item").value = "";

		},
		complete: function () {
			$('.ajax-loader').css("visibility", "hidden");
			let nb = $("#nbSBT").val();
			tableSalesTransactionBT();

			if (nb == 0) {
				$('#errorModal').modal('show');
			}
		},
	});
}

function tableSalesTransactionBT() {
	$(document).ready(function () {
		$('#TableSalesTransactionBT').DataTable({
			"pageLength": 100,
			dom: 'Bfrtip',
			buttons: [
				 'csv'
			]
		});
	});
}



/* function getTransaction code   Joseph*/
function getTransactionCode() {

	let dateFrom = $("#dateFromSalesTbt").val();
	let dateTo = $("#dateToSalesTbt").val();

	let url = symphony + "/Process/GetTransationCode";

	$.ajax({
		type: 'POST',
		url: url,
		data: { dateFrom: dateFrom, dateTo: dateTo },

		beforeSend: function () {
			$('.ajax-loader').css("visibility", "visible");
		},
		success: function (data) {
			$("#affichageSBT").html(data);

		},
		complete: function () {
			$('.ajax-loader').css("visibility", "hidden");
		},
	});

}

	function changeDate() {
		$("#dateToSalesTbt").val("");
	}


/*   End Sales Transaction By Type   */

/* Flown Transaction By Type  Joseph */
	function searchFlownTransaction() {

		let dateFrom = $("#dateFromFlown").val();
		let dateTo = $("#dateToFlown").val();
		let ttype = $("#itemflown").val();

		if (ttype == "-ALL-") {
			param = "%";
		} else {
			param = $("#itemflown").val();
		}


		let url = symphony + "/Process/FlownTransactionBT?flwntr="+param;
		$.ajax({
			type: 'POST',
			url: url,
			data: { dateFrom: dateFrom, dateTo: dateTo },
			beforeSend: function () {
				$('.ajax-loader').css("visibility", "visible");
			},
			success: function (data) {
				$("#afficheFlown").html(data);

			},
			complete: function () {
				$('.ajax-loader').css("visibility", "hidden");
				let nb = $("#nbFlown").val();
				tableFlownTransaction();

				if (nb == 0) {
					$('#errorModal').modal('show');
				}
			},
		});
	}

	function tableFlownTransaction() {
		$(document).ready(function () {
			$('#TableFlownTransaction').DataTable({
				"pageLength": 100,
				dom: 'Bfrtip',
				buttons: [
					 'csv'
				]
			});
		});
	}


/* End Flown Transaction By Type */


/* Interline Transaction By Type  Joseph */
	function searchInterlineT() {

		let dateFrom = $("#dateFromInterline").val();
		let dateTo = $("#dateToInterline").val();
		let ttype = $("#itemInter").val();

		if (ttype == "-ALL-") {
			paramInter = "%";
		} else {
			paramInter = $("#itemInter").val();
		}

		let url = symphony + "/Process/InterlineTransactionBT?Intertr=" +paramInter;
		$.ajax({
			type: 'POST',
			url: url,
			data: { dateFrom: dateFrom, dateTo: dateTo },
			beforeSend: function () {
				$('.ajax-loader').css("visibility", "visible");
			},
			success: function (data) {
				$("#affiIterline").html(data);

			},
			complete: function () {
				$('.ajax-loader').css("visibility", "hidden");
				let nb = $("#nbInter").val();
				tableInterlineT();

				if (nb == 0) {
					$('#errorModal').modal('show');
				}
			},
		});

	}

	function tableInterlineT() {
		$(document).ready(function () {
			$('#TableInterlineT').DataTable({
				"pageLength": 100,
				dom: 'Bfrtip',
				buttons: [
					 'csv'
				]
			});
		});
	}
/* End Interline Transaction By Type */


/* Five Days Rate By Currency / Country   Joseph */
    function searchDaysRateByCurrency() {
        let periode = $("#valPeriode").val();
        let country = $("#valCountry").val();
        let currency = $("#valCurrency").val();

        // Parametre periode
                if (periode == "-ALL-") {
                    paramPeriode = "%";
                } else {
                    paramPeriode = periode;
                }

        // Parametre country
                if (country == "-ALL-") {
                    paramCountry = "%";
                } else {
                    paramCountry = country;
                }

        // Parametre currency
                if (currency == "-ALL-") {
                    paramCurrency = "%";
                } else {
                    paramCurrency = currency;
                }

        let url = symphony + "/Process/DaysRateByCurrency";

        $.ajax({
            type: 'POST',
            url: url,
            data: { paramPeriode: paramPeriode, paramCountry: paramCountry, paramCurrency: paramCurrency },

            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {
                $("#affichageDaysRate").html(data);

            },
            complete: function () {
                $('.ajax-loader').css("visibility", "hidden");

                // get Param Periode
                if (paramPeriode == "%") {
                    $("#valPeriode").val("-ALL-");
                } else {
                    $("#valPeriode").val($("#period").val());
                }

                // get Param Country
                if (paramCountry == "%") {
                    $("#valCountry").val("-ALL-");
                }else{
                    $("#valCountry").val($("#country").val());
                }

                // get Param Currency
                if (paramCurrency == "%") {
                    $("#valCurrency").val("-ALL-");
                } else {
                    $("#valCurrency").val($("#currency").val());
                }

                let nb = $("#nbDR").val();
                tableDaysRate();

                if (nb == 0) {
                    $('#errorModal').modal('show');
                }
            },
        });
    }

    function tableDaysRate() {
        $(document).ready(function () {
            $('#TableDaysRate').DataTable({
                "pageLength": 100,
                dom: 'Bfrtip',
                buttons: [
                     'csv'
                ]
            });
        });
    }
    
/* function getItem Currency with Country        Joseph */

	function searchDaysRateByCurrency() {

		let periode = $("#valPeriode").val();
		let country = $("#valCountry").val();
		let currency = $("#valCurrency").val();

		// Parametre periode
				if (periode == "-ALL-") {
					paramPeriode = "%";
				} else {
					paramPeriode = periode;
				}

		// Parametre country
				if (country == "-ALL-") {
					paramCountry = "%";
				} else {
					paramCountry = country;
				}

		// Parametre currency
				if (currency == "-ALL-") {
					paramCurrency = "%";
				} else {
					paramCurrency = currency;
				}

		let url = symphony + "/Process/DaysRateByCurrency";

		$.ajax({
			type: 'POST',
			url: url,
			data: { paramPeriode: paramPeriode, paramCountry: paramCountry, paramCurrency: paramCurrency },

			beforeSend: function () {
				$('.ajax-loader').css("visibility", "visible");
			},
			success: function (data) {
				$("#affichageDaysRate").html(data);

			},
			complete: function () {
				$('.ajax-loader').css("visibility", "hidden");

				// get Param Periode
				if (paramPeriode == "%") {
					$("#valPeriode").val("-ALL-");
				} else {
					$("#valPeriode").val($("#period").val());
				}

				// get Param Country
				if (paramCountry == "%") {
					$("#valCountry").val("-ALL-");
				}else{
					$("#valCountry").val($("#country").val());
				}

				// get Param Currency
				if (paramCurrency == "%") {
					$("#valCurrency").val("-ALL-");
				} else {
					$("#valCurrency").val($("#currency").val());
				}

				let nb = $("#nbDR").val();
				tableDaysRate();

				if (nb == 0) {
					$('#errorModal').modal('show');
				}
			},
		});
	}

	function tableDaysRate() {
		$(document).ready(function () {
			$('#TableDaysRate').DataTable({
				"pageLength": 100,
				dom: 'Bfrtip',
				buttons: [
					 'csv'
				]
			});
		});
	}
	
/* function getItem Currency with Country        Joseph */

	function getItemCurrency() {

		let periode = $("#valPeriode").val();
		let country = $("#valCountry").val();
		let currency = $("#valCurrency").val();

		// Parametre periode
		if (periode == "-ALL-") {
			paramPeriode = "%";
		} else {
			paramPeriode = periode;
		}

		// Parametre country
		if (country == "-ALL-") {
			paramCountry = "";
		} else {
			paramCountry = country;
		}

		// Parametre currency
		if (currency == "-ALL-") {
			paramCurrency = "%";
		} else {
			paramCurrency = currency;
		}

		let url = symphony + "/Process/recupItemCurrency";
		$.ajax({
			type: 'POST',
			url: url,
			data: { paramCountry: paramCountry, paramPeriode: paramPeriode },
			beforeSend: function () {
			   // $('.ajax-loader').css("visibility", "visible");
			},
			success: function (data) {
				$("#affichageDaysRate").html(data);
			 
			},
			complete: function () {
			  //  $('.ajax-loader').css("visibility", "hidden");

				// get Param Periode
				if (paramPeriode == "%") {
					$("#valPeriode").val("-ALL-");
				} else {
					$("#valPeriode").val($("#period").val());
				}

				// get Param Country
				if (paramCountry == "") {
					$("#valCountry").val("-ALL-");
				} else {
					$("#valCountry").val($("#country").val());
				}

				// get Param Currency
				if (paramCurrency == "%") {
					$("#valCurrency").val("-ALL-");
				} else {
					$("#valCurrency").val($("#currency").val());
				}
			},
		});

	}

/* End Five Days Rate By Currency / Country */





/* City / Airport     Joseph */

	/*function loadSearchBy  Joseph*/

	function searchByCityAirport(champs) {

		let paramCA = $("#CityAirport").val();

		$.ajax({
			type: "POST",
			url: "/Process/SearchByCityAirport",
			data: { paramCA: paramCA },

			beforeSend: function () {
			   // $('.ajax-loader').css("visibility", "visible");
			},
			success: function (data) {
				$("#afficheCityAirport").html(data);
			},
			complete: function () {
			   // $('.ajax-loader').css("visibility", "hidden");

				let getValCA = $("#valCA").val()

				if (getValCA == "-ALL") {
					$("#CityAirport").val("-ALL-")
				} else {
					$("#CityAirport").val($("#valCA").val());
				}

				// function visible
				var Obj = document.getElementById(champs);

				if (paramCA == "-ALL-" && Obj.style.visibility == 'visible') {
					Obj.style.visibility = 'hidden';
					document.getElementById('text').innerHTML = '';
				} else {
					document.getElementById('text').innerHTML = paramCA;
				}

			}
		});

	}

	// function resultatCityAirport

	function resultatCityAirport(champs) {

		let paramCA = $("#CityAirport").val();
		let paramByCA = $("#paramSearch").val();

		$.ajax({
			type: "POST",
			url: "/Process/CityAirport",
			data: { paramCA: paramCA, paramByCA: paramByCA },

			beforeSend: function () {
				 $('.ajax-loader').css("visibility", "visible");
			},
			success: function (data) {
				$("#afficheCityAirport").html(data);
			},
			complete: function () {
				 $('.ajax-loader').css("visibility", "hidden");

				let getValCA = $("#valCA").val()

				// get Val CityAirport  and function visible
				if (getValCA == "-ALL") {
					$("#CityAirport").val("-ALL-")
				} else {
					$("#CityAirport").val($("#valCA").val());
				}

				var Obj = document.getElementById(champs);

				if (paramCA == "-ALL-" && Obj.style.visibility == 'visible') {
					Obj.style.visibility = 'hidden';
					document.getElementById('text').innerHTML = '';
				} else {
					document.getElementById('text').innerHTML = paramCA;
				}

				// get val ByCityAirport
				$("#paramSearch").val($("#valparamS").val());
				tableCity();
			}
		});

	}

	function tableCity() {
		$(document).ready(function () {
			$('#TableCity').DataTable({
				"pageLength": 100,
				dom: 'Bfrtip',
				buttons: [
					 'csv'
				]
			});
		});
	}

	function tableCityAdd() {
		$(document).ready(function () {
			$('#TableCityAdd').DataTable({
				"pageLength": 100,
				dom: 'Bfrtip',
				buttons: [
					 'csv'
				]
			});
		});
	}

/* End  function resultatCityAirport  */

	/* function Add New Entry  Joseph */
		
	function addNewEntry() {

		let airportCode = $("#valAirCode").val();
		let cityCode = $("#valCityCode").val();
		let ariportName = $("#valAirName").val();
		let cityName = $("#valCitName").val();
		let country = $("#valAddcountry").val();
		let cityIsoCode = $("#valCitIC").val();
		let status = $("#valStatus").val();

		if (airportCode == "") {
			$('#alertModal').modal('show');
			$('#msgalert').text('Please Enter an AirportCode');
		} else if (cityCode == "") {
			$('#alertModal').modal('show');
			$('#msgalert').text('Please Enter a CityCode');
		} else if (ariportName =="") {
			$('#alertModal').modal('show');
			$('#msgalert').text('Please Enter an Airport Name');
		} else if (cityName == "") {
			$('#alertModal').modal('show');
			$('#msgalert').text('Please Enter a City Name');
		} else if (country =="") {
			$('#alertModal').modal('show');
			$('#msgalert').text('Please Enter a Country');
		} else if (cityIsoCode =="") {
			$('#alertModal').modal('show');
			$('#msgalert').text('Please Enter a CityIsoCode');
		} else if (status == "") {
			$('#alertModal').modal('show');
			$('#msgalert').text('Please Enter a Status');
		} else {

			let exist = citycodeExist(cityCode);
			let existAir = airportExist(airportCode);

			if (exist != "exist" && existAir != "exist") {
				$.ajax({
					type: "POST",
					url: "/Process/AddNewEntry",
					data: {
						airportCode: airportCode, cityCode: cityCode, ariportName: ariportName,
						cityName: cityName, country: country, cityIsoCode: cityIsoCode, status: status
					},

					beforeSend: function () {
						$('.ajax-loader').css("visibility", "visible");
					},
					success: function (data) {
						$("#afficheCityAirport").html(data);
						$('#alertModal').modal('show');
						$('#msgalert').text('Record Successfully Saved');

					},
					complete: function () {
						$('.ajax-loader').css("visibility", "hidden");
						tableCityAdd();
					   
					},
					error: function () {
						$('#alertModal').modal('show');
						$('#msgalert').text('Please Check All your Entry. Some fields have wrongly been Inputted');
					 }
					 });
			} else {
				$('#alertModal').modal('show');
				$('#msgalert').text('Record Already Exist');
			}
		}

	}

	/* End function Add New Entry  Joseph */



	/* function exist CodeCity    Joseph*/
	function citycodeExist(inputCity) {
		let get = " " + inputCity.toUpperCase()
		let statusCode = "";

		var select = [];
		$('#cityCodes').find('select').each(function () {
			var propName = $(this).attr('name');
			select[propName] = [];
			$(this).children().each(function () {
				select[propName].push({
					selectName: $(this).text(),
					optionValue: $(this).attr('value') 
				});
			});
		});

		for (var i = 0; i < select["cityCode"].length; i++) {
			var reponse = select["cityCode"][i].selectName;

			if (get === reponse) {
				statusCode = "exist";
				break
			} else {
				statusCode = "Noexist";
			}
		}

		return statusCode
	}
	/* End   function exist CodeCity    Joseph*/

	
/*function exist AirportCode   Joseph*/
	function airportExist(inputAirport) {
		let getAir = " " + inputAirport.toUpperCase()
		let statusAir = "";

		var select = [];
		$('#airportCodes').find('select').each(function () {
			var propName = $(this).attr('name');
			select[propName] = [];
			$(this).children().each(function () {
				select[propName].push({
					selectName: $(this).text(),
					optionValue: $(this).attr('value')
				});
			});
		});

		for (var i = 0; i < select["airportCode"].length; i++) {
			var reponse = select["airportCode"][i].selectName;

			if (getAir === reponse) {
				statusAir = "exist";
				break
			} else {
				statusAir = "Noexist";
			}
		}

		return statusAir
	}
/* End function exist AirportCode    Joseph*/


	/*function dynamique form      Joseph*/

	function afficheForm(param) {
		var Obj = document.getElementById(param);

		if (Obj.style.visibility == 'hidden') {
			Obj.style.visibility = 'visible';
		} else {
			Obj.style.visibility = 'visible';
		}
	}
	
	function exitForm(param) {
		var Obj = document.getElementById(param);

		if (Obj.style.visibility == 'visible') {
			Obj.style.visibility = 'hidden';
		} else {
			Obj.style.visibility = 'hidden';
		}
	}

	function videChanps() {
		 $("#valAirCode").val("");
		 $("#valCityCode").val("");
		 $("#valAirName").val("");
		 $("#valCitName").val("");
		 $("#valAddcountry").val("");
		 $("#valCitIC").val("");
		$("#valStatus").val("");
	}
	/* End function dynamique form      Joseph*/


// function Delete 
	function deleteCity(arCode, cityCode) {

		if (window.confirm("Are you sure you want to Delete the following Selected Record?")) {
			$.ajax({
				type: "POST",
				url: "/Process/DeleteCityAirport",
				data: { arCode: arCode, cityCode: cityCode },

				beforeSend: function () {
					$('.ajax-loader').css("visibility", "visible");
				},
				success: function (data) {
					$("#afficheCityAirport").html(data);

				},
				complete: function () {
					$('.ajax-loader').css("visibility", "hidden");
					alert("Record Deleted Successfully")
					tableCity();
				}
			});
		}
	}

/* End City / Airport   Joseph */


/*  Prorate Factors     Joseph*/

	/* Function Dynamic Combo    Joseph */
	function changeOrigineCountry() {

		let OCountryN = $("#OriginalCN").val();
		// get position 2 requete
		let valOcountry = $('select.form-control option[value="' + $("#OriginalCN").val() + '"]').data('value');

		if (valOcountry == undefined) {
			valOC ="-ALL-"
		} else {
			valOC = valOcountry
		}

		$.ajax({
			type: "POST",
			url: "/Process/ChangeProrateFactors",
			data: { OCountryN: OCountryN, valOC: valOC },

			beforeSend: function () {
			  //  $('.ajax-loader').css("visibility", "visible");
			},
			success: function (data) {
				$("#affichProrateF").html(data);

			},
			complete: function () {
			  //  $('.ajax-loader').css("visibility", "hidden");
				$("#OriginalCN").val($("#valCN").val());

			}
		});

	}


    function changeDestinationCountry() {
        // get champs Origin country Name
        let OCountryN = $("#OriginalCN").val();

		let valOcountry = $('select.form-control option[value="' + $("#OriginalCN").val() + '"]').data('value');
		if (valOcountry == undefined) {
			valOC = "-ALL-"
		} else {
			valOC = valOcountry
		}

		// get champs Origin City Name
		let OCityName = $("#OriginCityN").val();

		let valOCityName = $('select.form-control option[value="' + $("#OriginCityN").val() + '"]').data('value');
		if (valOCityName == undefined) {
			valOCN = "-ALL-"
		} else {
			valOCN = valOCityName
		}

		// get champs Destination Country Name
		let DCountryN = $("#DestinationCounN").val();

		let valDcountry = $('select.form-control option[value="' + $("#DestinationCounN").val() + '"]').data('value');
		if (valDcountry == undefined) {
			valDC = "-ALL-"
		} else {
			valDC = valDcountry
		}

		$.ajax({
			type: "POST",
			url: "/Process/ChangeDestination",
			data: { OCountryN: OCountryN, valOC: valOC, DCountryN: DCountryN, valDC: valDC, OCityName: OCityName, valOCN: valOCN },

			beforeSend: function () {
				//  $('.ajax-loader').css("visibility", "visible");
			},
			success: function (data) {
				$("#affichProrateF").html(data);

			},
			complete: function () {
				//  $('.ajax-loader').css("visibility", "hidden");
				$("#DestinationCounN").val($("#valDCN").val());

				$("#OriginalCN").val($("#valCN").val());
				$("#OriginCityN").val($("#valCityName").val());
			}
		});
	}

	// function search ProrateFactors   Joseph
	function searchProrateFactors() {

		let valOCountryN = $("#OriginalCN").val();
		let valOCityName = $("#OriginCityN").val();
		let valDCountryN = $("#DestinationCounN").val();
		let valDCityName = $("#DestinationCityN").val();
		let datefrom = $("#dtpIssueDateFrom").val();

		//changement Origin CityName
		let valOcountry = $('select.form-control option[value="' + $("#OriginalCN").val() + '"]').data('value');
		if (valOcountry == undefined) {
			valOC = "-ALL-"
		} else {
			valOC = valOcountry
		}

		// changement Dest CityName
		let valDcountry = $('select.form-control option[value="' + $("#DestinationCounN").val() + '"]').data('value');
		if (valDcountry == undefined) {
			valDC = "-ALL-"
		} else {
			valDC = valDcountry
		}

		$.ajax({
			type: "POST",
			url: "/Process/SearchProrationFactor",
			data: { valOCountryN: valOCountryN, valOCityName: valOCityName, valDCountryN: valDCountryN, valDCityName: valDCityName, datefrom: datefrom, valOC: valOC, valDC: valDC },

			beforeSend: function () {
				 $('.ajax-loader').css("visibility", "visible");
			},
			success: function (data) {
				$("#affichProrateF").html(data);

			},
			complete: function () {
				  $('.ajax-loader').css("visibility", "hidden");
				$("#DestinationCounN").val($("#valDCN").val());
				$("#OriginalCN").val($("#valCN").val());
				$("#OriginCityN").val($("#valCityName").val());
				$("#DestinationCityN").val($("#valDCity").val());

				let nb = $("#valnb").val();
				tableProrate();

				if (nb == 0) {
					$('#errorModal').modal('show');
				}
			}
		});
	}


	function tableProrate() {
		$(document).ready(function () {
			$('#TableProrate').DataTable({
				"pageLength": 100,
				dom: 'Bfrtip',
				buttons: [
					 'csv'
				]
			});
		});
	}


/* End Prorate Factors     Joseph*/

/*******************SML samuel  Process***********************/
function searchSVC() {
	let dateFrom = $("#dateFromsectorValue").val();
	let dateTo = $("#dateTosectorValue").val();
	let url = symphony + "/Process/LoadSectorValueControl";
	$.ajax({
		type: "POST",
		url: url,
		data: { dateFrom: dateFrom, dateTo: dateTo },

		beforeSend: function () {
			$('.ajax-loader').css("visibility", "visible");
		},
		success: function (data) {
			$("#LoadSecvalu").html(data);
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
function GetTabSVC(numCode) {  
   var table = document.getElementById('tabSecValControl');
	var textRange; var i = 0;
	for ( i = 1; i < table.rows.length; i++) {
		table.rows[i].onclick = function (numCode) {
			document.getElementById("origincity").value = this.cells[0].innerHTML;
			document.getElementById("destinationcity").value = this.cells[1].innerHTML;
			document.getElementById("primecode").value = this.cells[2].innerHTML;
			document.getElementById("rbd").value = this.cells[3].innerHTML;
			document.getElementById("maxsharevalue").value = this.cells[4].innerHTML;
			document.getElementById("minsharevalue").value = this.cells[5].innerHTML;
		};
	}
}
function GetTabSVCsave() {
	var table = document.getElementById('tabSecValControl');
	var textRange; var i = 0;
	for (i = 1; i < table.rows.length; i++) {
		table.rows[i].onclick = function (numCode) {
			document.getElementById("origincity").value = this.cells[0].innerHTML;
			document.getElementById("destinationcity").value = this.cells[1].innerHTML;
			document.getElementById("primecode").value = this.cells[2].innerHTML;
			document.getElementById("rbd").value = this.cells[3].innerHTML;
			document.getElementById("maxsharevalue").value = this.cells[4].innerHTML;
			document.getElementById("minsharevalue").value = this.cells[5].innerHTML;
		};
	}
}
function clearSVC() {
	$("#origincity").val('');
	$("#destinationcity").val('');
	$("#primecode").val('');
	$("#rbd").val('');
	$("#maxsharevalue").val('');
	$("#minsharevalue").val('');
	$("#tabSecValControlTD").html('');
}
function AddNewSVC() {
	let url = symphony + "/Process/viewSelectedSectorValueControl";
	$("#origincity").val('');
	$("#destinationcity").val('');
	$("#primecode").val('');
	$("#rbd").val('');
	$("#maxsharevalue").val('');
	$("#minsharevalue").val('');
	$("#tabSecValControlTD").html('');
	$.ajax({
		type: "POST",
		url: url,
		success: function (data) {
			$("#formedit").html(data);
		},
		error: function () {
			$('#errorModal').modal('show');
		}
	});
}
function SaveSVC() {
	let origincity = $("#origincity").val();
	let destinationcity = $("#destinationcity").val();
	let primecode = $("#primecode").val();
	let rbd = $("#rbd").val();
	let maxsharevalue = $("#minsharevalue").val();
	let minsharevalue = $("#maxsharevalue").val();
	let url = symphony + "/Process/SaveNewSectorValueControl";
	let validation = $("#validation").val();

	if ($('input[name=super]').is(':checked')) {
	   
		validation = 1;
	
	} else {
		validation = 0;
	}

	if (origincity == "")
		{
			alert("Origin City Code is compulsory");
				  
		}
		else if (destinationcity == "")
		{
			alert("Destination City Code is compulsory");
				  
		}
		else if (primecode == "")
		{
			alert ("Prime Code is compulsory");
					
		}
		else if (rbd == "")
		{
			alert("RBD is compulsory");
				   
		}

		else if (origincity.Length < 3)

		{
			alert("Origin City Code length can not be less than 3 characters");
				   
		}

		else if (destinationcity.Length < 3)
		{
			alert("Destination City Code length can not be less than 3 characters");
				   
		}
		else if (minsharevalue == "")
		{
			alert("Minimum share value can't be null");
		}
		else if (maxsharevalue == "")
		{
			alert("Maximum share value can't be null");      
		}
	else {
			$.ajax({
				type: "POST",
				url: url,
				data: { origincity: origincity, destinationcity: destinationcity, primecode: primecode, rbd: rbd, minsharevalue: minsharevalue, maxsharevalue: maxsharevalue, validation: validation },
				success: function (data) {
					$("#tabSecValControl").html(data);
					alert("Record Save Sucessfully");
				},  
				error: function () {
					$('#errorModal').modal('show');
				}
			});
		}
	}
function editSVCTD() {
	let url = symphony + "/Process/viewSelectedSectorValueControl";
	$("#origincity").val('');
	$("#destinationcity").val('');
	$("#primecode").val('');
	$("#rbd").val('');
	$("#maxsharevalue").val('');
	$("#minsharevalue").val('');
	$.ajax({
		type: "POST",
		url: url,
		success: function (data) {
			$("#formedit").html(data);
		},
		error: function () {
			$('#errorModal').modal('show');
		}
	});
}

function setCriteriaSectorDiscrepency() {
	let dateFrom = $("#dateFromsectorValue").val();
	let dateTo = $("#dateTosectorValue").val();
	let url = symphony + "/Process/LoadSectorValueDiscrepency";
	$.ajax({
		type: "POST",
		url: url,
		data: { dateFrom: dateFrom, dateTo: dateTo },

		beforeSend: function () {
			$('.ajax-loader').css("visibility", "visible");
		},
		success: function (data) {
			$("#LoadSecDiscrep").html(data);
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
function ExportCSV() {
			 var tab_text = "<table border='2px'><tr bgcolor='#87AFC6'>";
			 var textRange; var j = 0;
			 tab = document.getElementById('tabSVC');

			 for (j = 0 ; j < tab.rows.length ; j++) {
				 tab_text = tab_text + tab.rows[j].innerHTML + "</tr>";
	}
			 tab_text = tab_text + "</table>";
			 tab_text = tab_text.replace(/<A[^>]*>|<\/A>/g, "");
			 tab_text = tab_text.replace(/<img[^>]*>/gi, ""); 
			 tab_text = tab_text.replace(/<input[^>]*>|<\/input>/gi, ""); 

			 var ua = window.navigator.userAgent;
			 var msie = ua.indexOf("MSIE ");

			 if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./))
	{
				 txtArea1.document.open("txt/html", "replace");
				 txtArea1.document.write(tab_text);
				 txtArea1.document.close();
				 txtArea1.focus();
				 sa = txtArea1.document.execCommand("SaveAs", true, "Say Thanks to Sumit.xls");
	}
	else   
				 sa = window.open('data:application/vnd.ms-excel,' + encodeURIComponent(tab_text));

			 return (sa);
	}
	/*******************end SML samuel  Process***********************/


/* Fare Query Screen   Joseph */

function ViewFareBySector() {

	let from = $("#sectorFrom").val();
	let to = $("#sectorTo").val();

	let url = symphony + "/Process/ViewFareBySector";

	$.ajax({
		type: "POST",
		url: url,
		data: { from: from, to: to },

		beforeSend: function () {
			$('.ajax-loader').css("visibility", "visible");
		},
		success: function (data) {
			$("#FareQuery").html(data);

		  
		},
		complete: function () {
			$('.ajax-loader').css("visibility", "hidden");

			$("#sectorFrom").val($("#valFrom").val());
			$("#sectorTo").val($("#valTo").val());

		},
		error: function () {
			$('#errorModal').modal('show');
		}
	});
}

function FareByComponent() {

	let from = $("#sectorFrom").val();
	let to = $("#sectorTo").val();
	let idStatusView = $("#IdViewFare").val();

	let url = symphony + "/Process/FareByComponent";

	$.ajax({
		type: "POST",
		url: url,
		data: { from: from, to: to, idStatusView: idStatusView },

		beforeSend: function () {
			$('.ajax-loader').css("visibility", "visible");
		},
		success: function (data) {
			$("#FareQuery").html(data);


		},
		complete: function () {
			$('.ajax-loader').css("visibility", "hidden");

			$("#sectorFrom").val($("#valFrom").val());
			$("#sectorTo").val($("#valTo").val());

		},
		error: function () {
			$('#errorModal').modal('show');
		}
	});
}

function ViewFareEntries() {

	let from = $("#sectorFrom").val();
	let to = $("#sectorTo").val();
	let idStatus = $("#IdFareComp").val();


	let url = symphony + "/Process/ViewFareEntries";

	$.ajax({
		type: "POST",
		url: url,
		data: { from: from, to: to, idStatus: idStatus },

		beforeSend: function () {
			$('.ajax-loader').css("visibility", "visible");
		},
		success: function (data) {
			$("#FareQuery").html(data);


		},
		complete: function () {
			$('.ajax-loader').css("visibility", "hidden");

			$("#sectorFrom").val($("#valFrom").val());
			$("#sectorTo").val($("#valTo").val());

		},
		error: function () {
			$('#errorModal').modal('show');
		}
	});
}

function ViewFareGet() {
	$('#alertModal').modal('show');
	$('#msgalert').text('The ConnectionString property has not been initialized');
}

function FareByComponentGet() {
	$('#alertModal').modal('show');
	$('#msgalert').text('The ConnectionString property has not been initialized');
}

function ClearViewFare() {
	let from = $("#sectorFrom").val();
	let to = $("#sectorTo").val();
	let idStatus = $("#IdFareComp").val();


	let url = symphony + "/Process/ClearViewFare";

	$.ajax({
		type: "POST",
		url: url,
		data: { from: from, to: to, idStatus: idStatus },

		beforeSend: function () {
			$('.ajax-loader').css("visibility", "visible");
		},
		success: function (data) {
			$("#FareQuery").html(data);
		},
		complete: function () {
			$('.ajax-loader').css("visibility", "hidden");
			$("#sectorFrom").val($("#valFrom").val());
			$("#sectorTo").val($("#valTo").val());

		},
		error: function () {
			$('#errorModal').modal('show');
		}
	});
}

function ClearFareCompent() {
	let from = $("#sectorFrom").val();
	let to = $("#sectorTo").val();
	let idStatus = $("#IdViewFare").val();


	let url = symphony + "/Process/ClearFareComponent";

	$.ajax({
		type: "POST",
		url: url,
		data: { from: from, to: to, idStatus: idStatus },

		beforeSend: function () {
			$('.ajax-loader').css("visibility", "visible");
		},
		success: function (data) {
			$("#FareQuery").html(data);
		},
		complete: function () {
			$('.ajax-loader').css("visibility", "hidden");
			$("#sectorFrom").val($("#valFrom").val());
			$("#sectorTo").val($("#valTo").val());

		},
		error: function () {
			$('#errorModal').modal('show');
		}
	});
}
/* End Fare Query Screen   Joseph */



/* Fare Inpute Screen */

function saveInputeScreen() {

	let valfareIsn = $("#fareIsn").val();
	let valrule = $("#rule").val();
	let valprimeCode = $("#primeCode").val();
	let valcarrierCode = $("#carrierCode").val();
	let valmpm = $("#mpm").val();
	let valseasonal = $("#seasonal").val();
	let valsectorFrom = $("#sectorFromInput").val();
	let valgi = $("#gi").val();
	let valpartofWeek = $("#partofWeek").val();
	let valsectorTo = $("#sectorToInput").val();
	let valrbd = $("#rbd ").val();
	let valpartOfDayCode = $("#partOfDayCode").val();
	let valfareBasis = $("#fareBasis").val();
	let valsalesValidityFrom = $("#salesValidityFrom").val();
	let valfptc = $("#fptc").val();
	let valjourneyType = $("#journeyType").val();
	let valsalesValidityTo = $("#salesValidityTo").val();
	let valfareLevelId = $("#fareLevelId").val();
	let valcurrencyCode = $("#currencyCode").val();
	let vallocalCurrencyFare = $("#localCurrencyFare").val();
	let valnuc = $("#nuc").val();
	let valflownValidityFrom = $("#flownValidityFrom").val();
	let  valflownValidityTo = $("#flownValidityTo").val();

	if (valfareIsn == "") {
		$('#alertModal').modal('show');
		$('#msgalert').text('Please enter fare isn!');
	} else if (valrule == "") {
		$('#alertModal').modal('show');
		$('#msgalert').text('Please enter  Rul');
	} else if (valprimeCode == "") {
		$('#alertModal').modal('show');
		$('#msgalert').text('Please enter Prime Code');
	} else if (valcarrierCode == "") {
		$('#alertModal').modal('show');
		$('#msgalert').text('Please enter Carrier Code');
	} else if (valmpm == "") {
		$('#alertModal').modal('show');
		$('#msgalert').text('Please enter MPM');
	} else if (valsectorFrom == "") {
		$('#alertModal').modal('show');
		$('#msgalert').text('Please enter Sector from');
	} else if (valgi == "") {
		$('#alertModal').modal('show');
		$('#msgalert').text('Please enter GI');
	} else if (valpartofWeek == "") {
		$('#alertModal').modal('show');
		$('#msgalert').text('Please enter Part of Week Code!');
	} else if (valsectorTo == "") {
		$('#alertModal').modal('show');
		$('#msgalert').text('Please enter sector To');
	} else if (valrbd == "") {
		$('#alertModal').modal('show');
		$('#msgalert').text('Please enter RBD');
	} else if (valpartOfDayCode == "") {
		$('#alertModal').modal('show');
		$('#msgalert').text('Please enter part of Day Code');
	} else if (valfareBasis == "") {
		$('#alertModal').modal('show');
		$('#msgalert').text('Please enter fare Basis');
	} else if (valsalesValidityFrom == "") {
		$('#alertModal').modal('show');
		$('#msgalert').text('Please enter sales validity from');
	} else if (valfptc == "") {
		$('#alertModal').modal('show');
		$('#msgalert').text('Please enter FPTC');
	} else if (valjourneyType == "") {
		$('#alertModal').modal('show');
		$('#msgalert').text('Please select Journey Type between OW/RT!');
	} else if (valsalesValidityTo == "") {
		$('#alertModal').modal('show');
		$('#msgalert').text('Please enter sales validity To');
	} else if (valfareLevelId == "") {
		$('#alertModal').modal('show');
		$('#msgalert').text('Please enter Fare Level Identifier!');
	} else if (valcurrencyCode == "") {
		$('#alertModal').modal('show');
		$('#msgalert').text('Please enter currency code');
	} else if (vallocalCurrencyFare == "") {
		$('#alertModal').modal('show');
		$('#msgalert').text('Please enter local currency fare');
	} else if (valnuc == "") {
		$('#alertModal').modal('show');
		$('#msgalert').text('Please enter NUC');
	} else if (valflownValidityFrom == "") {
		$('#alertModal').modal('show');
		$('#msgalert').text('Please enter flown Validity from');
	} else if (valflownValidityTo == "") {
		$('#alertModal').modal('show');
		$('#msgalert').text('Please enter flown Validity to');

	} else {

		let url = symphony + "/Process/saveInputeScreen";

		$.ajax({
			type: "POST",
			url: url,
			data: {
				valfareIsn: valfareIsn,
				valrule: valrule,
				valprimeCode: valprimeCode,
				valcarrierCode: valcarrierCode,
				valmpm: valmpm,
				valseasonal: valseasonal,
				valsectorFrom: valsectorFrom,
				valgi: valgi,
				valpartofWeek: valpartofWeek,
				valsectorTo: valsectorTo,
				valrbd: valrbd,
				valpartOfDayCode: valpartOfDayCode,
				valfareBasis: valfareBasis,
				valsalesValidityFrom: valsalesValidityFrom,
				valfptc: valfptc,
				valjourneyType: valjourneyType,
				valsalesValidityTo: valsalesValidityTo,
				valfareLevelId: valfareLevelId,
				valcurrencyCode: valcurrencyCode,
				vallocalCurrencyFare: vallocalCurrencyFare,
				valnuc: valnuc,
				valflownValidityFrom: valflownValidityFrom,
				valflownValidityTo: valflownValidityTo
			},

			beforeSend: function () {
				$('.ajax-loader').css("visibility", "visible");
			},
			success: function (data) {
				$("#FareInput").html(data);
			},
			complete: function () {
				$('.ajax-loader').css("visibility", "hidden");
				$('#alertModal').modal('show');
				$('#msgalert').text('Record Successfully Saved');
			},
			error: function () {
				$('#errorModal').modal('show');
			}
		});

	}


}

function clearSaveInpute() {
	 $("#fareIsn").val("");
	 $("#rule").val("");
	 $("#primeCode").val("");
	 $("#carrierCode").val("");
	 $("#mpm").val("");
	 $("#seasonal").val("");
	 $("#sectorFromInput").val("");
	 $("#gi").val("");
	 $("#partofWeek").val("");
	 $("#sectorToInput").val("");
	 $("#rbd ").val("");
	 $("#partOfDayCode").val("");
	 $("#fareBasis").val("");
	 $("#salesValidityFrom").val("");
	 $("#fptc").val("");
	 $("#journeyType").val("");
	 $("#salesValidityTo").val("");
	 $("#fareLevelId").val("");
	 $("#currencyCode").val("");
	 $("#localCurrencyFare").val("");
	 $("#nuc").val("");
	 $("#flownValidityFrom").val("");
	 $("#flownValidityTo").val("");
}
/* End Fare Inpute Screen */

//Harentsoa
// Append FAC Menu on click
function AppendFacMenu() {
	var FAC = '<li><a data-toggle="tab" href="#FCADOC" class="nav-pax" id="FacActive">Fare Construction Steps</a></li>';
	
    //Append the new menu
	$("#FareAuditTabMenu").append(FAC);

	//remove all active class
    $("#FareAuditTabMenu li").first().removeClass("active");

	//Add active class to the new menu
    $("#FareAuditTabMenu li").last().addClass("active");

    //Show active Pane
    // Select last tab
   // $("#FareAuditTabMenu li a:last").tab('show');
    var activeTab = $(this).find("a").attr("href");
    $(activeTab).show();

    //on click only one time
     
    $(this).off();
}

/* function LiftDataManagement */

function LiftData() {

    let valfrom = $("#from").val();
    let valto = $("#to").val();
    var checked = $("input[name='inlineDefaultRadiosExample']:checked").val();

    let paramSe = "";

    if (checked == "Inline1") {
        paramSe = 1
    } else if (checked == "Inline2") {
        paramSe = 2
    } else if (checked == "Inline3") {
        paramSe = 3
    }
    let url = symphony + "/Process/LiftData";
    $.ajax({
        type: "POST",
        url: url,
        data: { valfrom: valfrom, valto: valto, paramSe: paramSe },

        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#LIFTDSM").html(data);
        },
        complete: function () {
            
            $('.ajax-loader').css("visibility", "hidden");

            let nb = $("#valnb").val();

            if (nb == 0) {
                $('#alertModal').modal('show');
                $('#msgalert').text('No data available for the selected criteria.');
            }
            tableDataLift();
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}

function tableDataLift() {
    $(document).ready(function () {

        $('#TableLift').DataTable({
            "pageLength": 100,
           // dom: 'Bfrtip',
        });
    });
}

/* End function LiftDataManagement */

//christian
/*
function Transactionchris(docNumber, transactionCode) {

    let url = symphony + "/Sales/Transaction";
    $.ajax({
        type: 'POST',
        data: { docNumber: docNumber, transactionCode: transactionCode },
        url: url,
        success: function (data) {
            $("#testtrans").addClass("active");
            $("#transaction").addClass("active in");
            $(".data-transaction").html(data);
            $(".data-transaction").html(data);
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}
*/
//endchristian



/* function SalesDataManagement */

function SalesData() {

    let valfrom = $("#from").val();
    let valto = $("#to").val();
    var checked = $("input[name='inlineDefaultRadiosExample']:checked").val();
    let selectinterline = $("#selectinterline").val();

    let paramSe = "";

    if (checked == "Inline1") {
        paramSe = 1
    } else if (checked == "Inline2") {
        paramSe = 2
    } else if (checked == "Inline3") {
        paramSe = 3
    }

    let url = symphony + "/Process/SalesData";

    $.ajax({
        type: "POST",
        url: url,
        data: { valfrom: valfrom, valto: valto, paramSe: paramSe, selectinterline: selectinterline },

        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#SALESDSM").html(data);
        },
        complete: function () {

            $('.ajax-loader').css("visibility", "hidden");

            let nb = $("#valnb").val();

            if (nb == 0) {
                $('#alertModal').modal('show');
                $('#msgalert').text('No data available for the selected criteria.');
            }
            tableDataSales();


        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}

function tableDataSales() {
    $(document).ready(function () {

        $('#TableSales').DataTable({
            "pageLength": 100,

            // dom: 'Bfrtip',
        });
    });
}

/* End function SalesDataManagement */



/* function Controle Click */
function ControlLift() {
    $("#control").val("Lift");
}

function ControlSales() {
    $("#control").val("Sales");
}


function SerachDataSplit() {

    let param = $("#control").val();

    if (param == "Lift") {

        // function Joseph
        LiftData();

    } else {

        // function Christian
        SalesData();

    }
}
/* End function Controle Click */


/* Data Splite Management Joseph*/
function clickLigneTransactionData(event, docNumber, transactionCode, domInt) {

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
            $("#search").html(data);
           // $("#transaction").html(data);
          
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
            $("#transaction").html(data);
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}
/* End  Data Splite Management Joseph */

/* End function Controle Click */


function masquedivmasque() {
    $('#divmasque').css("visibility", "hidden");
}

function showdivmasque() {
    $('#divmasque').css("visibility", "visible");
}


/* Function Excess Proration    Joseph*/
function ExcessProration() {

    let valTicketNo = $("#ticketNo").val();
    let valFare = $("#txtFare").val();
    let valEfp = $("#efp").val();
    let valdateExcess = $("#dateExcessProration").val();
    let valMonth = $("#Month").val();
    let valfca = $("#fca").val();

    if (valFare == "") {
        $('#alertModal').modal('show');
        $('#msgalert').text('Please Enter a Fare Currency and Amount');

    } else if (valMonth == "") {
        $('#alertModal').modal('show');
        $('#msgalert').text('Please Enter Month Of Clearance');

    } else if (valfca == "") {
        $('#alertModal').modal('show');
        $('#msgalert').text('Please Enter Fare Calculation Area');

    } else {
        let url = symphony + "/Process/ActionBaggageProration";
        $.ajax({
            type: "POST",
            url: url,
            data: { valTicketNo: valTicketNo, valFare: valFare, valEfp: valEfp, valdateExcess: valdateExcess, valMonth: valMonth, valfca, valfca },

            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {
                $("#search").html(data);
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

/*End Function Excess Proration Joseph*/



/*Free Baggage AllowanceAudit    Joseph */

function ActionFreeBaggage() {

    let code = $("#presetCode").val();

    let url = symphony + "/Process/ActionFreeBaggage";
    $.ajax({
        type: "POST",
        url: url,
        data: { code: code, },

        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#donne").html(data);
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");

            let getVal = $("#valPresetCode").val()

            if (getVal == "-ALL") {
                $("#presetCode").val("-ALL-")
            } else {
                $("#presetCode").val($("#valPresetCode").val());
            }
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });



}


function ActionSaveFreeBaggage() {

    let code = $("#presetCode").val();

    let valK = $("#kilograms").val();
    let valLB = $("#pound").val();
    let valPC = $("#piece").val();

    let url = symphony + "/Process/ActionSave";
    $.ajax({
        type: "POST",
        url: url,
        data: { code: code, valK: valK, valLB: valLB, valPC: valPC },

        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#donne").html(data);
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");

            let getVal = $("#valPresetCode").val()

            if (getVal == "-ALL") {
                $("#presetCode").val("-ALL-")
            } else {
                $("#presetCode").val($("#valPresetCode").val());
            }
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}


// function changement valeur K et LB  Joseph
function changementK() {
    let valK = $("#kilograms").val();

    $("#pound").val( valK * 2);
}

function changementLB() {

    let valLB = $("#pound").val();

    $("#kilograms").val(valLB / 2);

}

// function btn Cancel
function ActionCancel() {
    let code = $("#presetCode").val();

    let valK = $("#kilograms").val();
    let valLB = $("#pound").val();
    let valPC = $("#piece").val();

    let url = symphony + "/Process/ActionCancel";
    $.ajax({
        type: "POST",
        url: url,
        data: { code: code},

        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#donne").html(data);
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");

            let getVal = $("#valPresetCode").val()

            if (getVal == "-ALL-") {
                $("#presetCode").val("-ALL-")
            } else {
                $("#presetCode").val($("#valPresetCode").val());
            }
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}

/*End Free Baggage AllowanceAudit    Joseph */


/* Audit Commission Audit   Joseph  */


// Get Angent Numeric Code by Date From & To Joseph

function GetAgentCode() {

    let from = $("#dateFromcommission").val();
    let to = $("#dateTocommission").val();

    let url = symphony + "/Process/GetAgentNum";
    $.ajax({
        type: "POST",
        url: url,
        data: { from: from , to: to},

        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#donne").html(data);
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}

// Recherche by Angent Numeric Code and Date from Joseph
function RechercheCommission() {

    let from = $("#dateFromcommission").val();
    let to = $("#dateTocommission").val();

    let code = $("#numericCode").val();

    let url = symphony + "/Process/SearchCommission";
    $.ajax({
        type: "POST",
        url: url,
        data: { from: from, to: to, code:code },

        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#donne").html(data);
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");

            let getValAgent = $("#valAgentCode").val()

            if (getValAgent == "-ALL-") {
                $("#numericCode").val("-ALL-")
            } else {
                $("#numericCode").val($("#valAgentCode").val());
            }
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });

}

// function Clear Commission     Joseph
function ClearCommission() {
    let from = $("#dateFromcommission").val();
    let to = $("#dateTocommission").val();
    let code = $("#numericCode").val();

    let url = symphony + "/Process/ClearCommi";
    $.ajax({
        type: "POST",
        url: url,
        data: { from: from, to: to, code: code },

        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#donne").html(data);
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}

/* Audit Commission Audit   Joseph  */


/* SPA Management    Jospeh  */


function activeNutshellSPA(id) {
    for (i = 2; i < 10; i++) {
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

function closeNetshullSPA(id) {
    $("ul.liNutshell li.active").removeClass('active');
    $("#" + id).removeClass('active');
    let index = parseInt(id.slice(-1), 10) + 1;
    let newIndex = "tab-nutshell" + index;
    $("#" + newIndex).parents().addClass('active');
    $("#nutshell" + index).addClass('active in');
    $("#" + id).hide();
    $("#tab-" + id).hide();
}


/* SPA_SPAMain  Joseph*/

function searchSPAMain() {

    this.item = "";
    let spaNumber = $("#spaMain").val();
    
    let url = symphony + "/Process/SPA_SPAMain";
    $.ajax({
        type: "POST",
        url: url,
        data: { spaNumber: spaNumber },

        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#affichageSPAMain").html(data);
        },
        complete: function () {
           
            $('.ajax-loader').css("visibility", "hidden");
            tableSPAMain();
            $("#spaMain").val($("#valSPANumber").val());
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });

}

function tableSPAMain() {

    $(document).ready(function () {
        var table = $('#TableSpaMain').DataTable({
            "pageLength": 100,
            dom: 'Bfrtip',
            buttons: [
           //	 'csv'
            ]
        });
        $('#TableSpaMain tbody').on('click', 'tr', function () {
            if ($(this).hasClass('selected')) {
                $(this).removeClass('selected');
            }
            else {
                table.$('tr.selected').removeClass('selected');
                $(this).addClass('selected');
            }
        });
        $('#button').click(function () {
            table.row('.selected').remove().draw(false);
        });
    });

}

function cancelSPAMain() {
    searchSPAMain();
}

/* END  SPA_SPAMain  Joseph*/



/*  IATA SEASON  Joseph*/
function searchIATASeason() {

    let iataSe = $("#iataSeason").val();

    let url = symphony + "/Process/SPA_IATASeason";
    $.ajax({
        type: "POST",
        url: url,
        data: { iataSe: iataSe },

        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#affichageIATA").html(data);
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
            tableIataSeason();

            this.itemIATASension = "";
            this.itemFrom = "";
            this.itemTo = "";

            $("#iataSeason").val($("#valIataSeason").val());
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });

}

function tableIataSeason() {
    $(document).ready(function () {
        var table = $('#TableIATA').DataTable({
            "pageLength": 100,
            dom: 'Bfrtip',
            buttons: [
           //	 'csv'
            ]
        });
        $('#TableIATA tbody').on('click', 'tr', function () {
            if ($(this).hasClass('selected')) {
                $(this).removeClass('selected');
            }
            else {
                table.$('tr.selected').removeClass('selected');
                $(this).addClass('selected');
            }
        });
        $('#button').click(function () {
            table.row('.selected').remove().draw(false);
        });
    });
}

function cancelIATA() {
    searchIATASeason();
}

/* END  IATA SEASON  Joseph*/


/*   ASV  Joseph   */
function searchASV() {

    let spa = $("#spaNumber").val();
    let asv = $("#asvNumber").val();

    let url = symphony + "/Process/SPA_ASV";
    $.ajax({
        type: "POST",
        url: url,
        data: { spa: spa, asv: asv },

        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#affichagASV").html(data);
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
            tableASV();

            this.itemspaASV = "";
            this.itemASV = "";

            $("#spaNumber").val($("#valSpaNumber").val());
            $("#asvNumber").val($("#valAsvNumber").val());
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });

}

function tableASV() {
    $(document).ready(function () {
        var table = $('#TableAsv').DataTable({
            "pageLength": 100,
            dom: 'Bfrtip',
            buttons: [
           //	 'csv'
            ]
        });
        $('#TableAsv tbody').on('click', 'tr', function () {
            if ($(this).hasClass('selected')) {
                $(this).removeClass('selected');
            }
            else {
                table.$('tr.selected').removeClass('selected');
                $(this).addClass('selected');
            }
        });
        $('#button').click(function () {
            table.row('.selected').remove().draw(false);
        });
    });
}

function cancelASV() {
    searchASV();
}

function searchASVNumber() {

    let spa = $("#spaNumber").val();
    let asv = $("#asvNumber").val();

    let url = symphony + "/Process/ASVNumber";
    $.ajax({
        type: "POST",
        url: url,
        data: { spa: spa, asv: asv },

        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#affichagASV").html(data);
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
           // tableASV();

            $("#spaNumber").val($("#valSpaNumber").val());
            $("#asvNumber").val($("#valAsvNumber").val());
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });

}


/*  END  ASV  Joseph   */


/* DISCOUNT   Jospeh  */
function searchDiscount() {

    let spa = $("#spaNumberDiscount").val();

    let url = symphony + "/Process/SPA_Discount";
    $.ajax({
        type: "POST",
        url: url,
        data: { spa: spa, },

        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#affichagDiscount").html(data);
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
            tableDiscount();

            this.itemspaDiscount = "";

            $("#spaNumberDiscount").val($("#valSpaNumberD").val());
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });

}

function tableDiscount() {
    $(document).ready(function () {
        var table = $('#TableDiscount').DataTable({
            "pageLength": 100,
            dom: 'Bfrtip',
            buttons: [
           //	 'csv'
            ]
        });
        $('#TableDiscount tbody').on('click', 'tr', function () {
            if ($(this).hasClass('selected')) {
                $(this).removeClass('selected');
            }
            else {
                table.$('tr.selected').removeClass('selected');
                $(this).addClass('selected');
            }
        });
        $('#button').click(function () {
            table.row('.selected').remove().draw(false);
        });
    });
}

function cancelDiscount() {
    searchDiscount();
}

/* END DISCOUNT   Jospeh  */


/* FLIGHT RESTRICTION   Joseph*/
function searchFlight() {

    let spa = $("#spaNumberFlight").val();

    let url = symphony + "/Process/SPA_FlightRestriction";
    $.ajax({
        type: "POST",
        url: url,
        data: { spa: spa, },

        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#affichagFlight").html(data);
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
            tableFlight();

            $("#spaNumberFlight").val($("#valspaFlight").val());
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });

}

function tableFlight() {
    $(document).ready(function () {
        var table = $('#TableFlight').DataTable({
            "pageLength": 100,
            dom: 'Bfrtip',
            buttons: [
           //	 'csv'
            ]
        });
        $('#TableFlight tbody').on('click', 'tr', function () {
            if ($(this).hasClass('selected')) {
                $(this).removeClass('selected');
            }
            else {
                table.$('tr.selected').removeClass('selected');
                $(this).addClass('selected');
            }
        });
        $('#button').click(function () {
            table.row('.selected').remove().draw(false);
        });
    });
}

function cancelFlight() {
    searchFlight();
}

/* END  FLIGHT RESTRICTION   Joseph*/



/* SPA Exception  Joseph*/
function searchSpaException() {

    let spa = $("#spaNumberExcep").val();

    let url = symphony + "/Process/SPA_Exception";
    $.ajax({
        type: "POST",
        url: url,
        data: { spa: spa, },

        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#affichageExcep").html(data);
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
            tableSpaExcept();

            this.itemsSpaExcep = "";

            $("#spaNumberExcep").val($("#valspaException").val());
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });

}

function tableSpaExcept() {
    $(document).ready(function () {
        var table = $('#TableSpaException').DataTable({
            "pageLength": 100,
            dom: 'Bfrtip',
            buttons: [
           //	 'csv'
            ]
        });
        $('#TableSpaException tbody').on('click', 'tr', function () {
            if ($(this).hasClass('selected')) {
                $(this).removeClass('selected');
            }
            else {
                table.$('tr.selected').removeClass('selected');
                $(this).addClass('selected');
            }
        });
        $('#button').click(function () {
            table.row('.selected').remove().draw(false);
        });
    });

}

function cancelSpaExcept() {
    searchSpaException();
}

/* End SPA Exception  Joseph*/


/* ASV Exception  Joseph*/
function searchAsvException() {

    let spa = $("#spaNumberAsvExcep").val();

    let url = symphony + "/Process/SPA_AsvException";
    $.ajax({
        type: "POST",
        url: url,
        data: { spa: spa, },

        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#affichageAsvExcep").html(data);
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
            tableAsvExcept();
            this.itemsAsvExcept = "";

            $("#spaNumberAsvExcep").val($("#valAsvException").val());
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });

}

function tableAsvExcept() {
    $(document).ready(function () {
        var table = $('#TableAsvException').DataTable({
            "pageLength": 100,
            dom: 'Bfrtip',
            buttons: [
           //	 'csv'
            ]
        });
        $('#TableAsvException tbody').on('click', 'tr', function () {
            if ($(this).hasClass('selected')) {
                $(this).removeClass('selected');
            }
            else {
                table.$('tr.selected').removeClass('selected');
                $(this).addClass('selected');
            }
        });
        $('#button').click(function () {
            table.row('.selected').remove().draw(false);
        });
    });
}

function cancelAsvExcept() {
    searchAsvException();
}

/* End ASV Exception  Joseph*/


/* Signatories  Joseph*/
function searchSignatories() {

    let spa = $("#spaSignatorie").val();

    let url = symphony + "/Process/SPA_Signatories";
    $.ajax({
        type: "POST",
        url: url,
        data: { spa: spa, },

        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#affichageSignatorie").html(data);
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
            tableSignatories();
            this.itemsSignator = "";

         
            $("#spaSignatorie").val($("#valSignatories").val());
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });

}

function tableSignatories() {
    $(document).ready(function () {
        var table = $('#TableSignatories').DataTable({
            "pageLength": 100,
            dom: 'Bfrtip',
            buttons: [
           //	 'csv'
            ]
        });
        $('#TableSignatories tbody').on('click', 'tr', function () {
            if ($(this).hasClass('selected')) {
                $(this).removeClass('selected');
            }
            else {
                table.$('tr.selected').removeClass('selected');
                $(this).addClass('selected');
            }
        });
        $('#button').click(function () {
            table.row('.selected').remove().draw(false);
        });
    });


}

function cancelSignatories() {
    searchSignatories();
}

/* End Signatories Joseph*/



// All function Delete SPA


// function Delete SPAMAIN
function deleteSPAMain() {
    let itemselected = this.item;

    let spaNumber = $("#spaMain").val();

    if (itemselected != "") {

        if (window.confirm("Are you sure you want to delete the SPA NUMBER: " + itemselected + " ?")) {
            $.ajax({
                type: "POST",
                url: "/Process/DeleteSPAMain",

                data: { itemselected: itemselected, spaNumber: spaNumber },

                beforeSend: function () {
                    $('.ajax-loader').css("visibility", "visible");
                },
                success: function (data) {
                    $("#affichageSPAMain").html(data);

                },
                complete: function () {
                    $('.ajax-loader').css("visibility", "hidden");

                    alert("Record Deleted Successfully")
                    tableSPAMain();

                    this.item = "";
                }
            });
        }
    } else {
        
    }

}

// funtion get item tableaux
let item = "";

function getItem(spanumber) {
    this.item = spanumber;
}


/*  Delete IATASEASON    Joseph*/

// use delete IATASension
let itemIATASension = "";
let itemFrom = "";
let itemTo = "";

function getDeleteIATASeason(iatasension, from, to) {
    this.itemIATASension = iatasension;
    this.itemFrom = from;
    this.itemTo = to;
}

function deleteIATASeason() {

    let itemselected = this.itemIATASension;
    let paramFrom = this.itemFrom;
    let paramTo = this.itemTo;

    let iataSension = $("#iataSeason").val();

    if (itemselected != "") {

        if (window.confirm("Are you sure you want to delete the IATASeason: " + itemselected + " and From: " + paramFrom + " and To: " + paramTo + "   ?")) {

            $.ajax({
                type: "POST",
                url: "/Process/DeleteSPAMain",

                data: { itemselected: itemselected, iataSension: iataSension, paramFrom: paramFrom, paramTo: paramTo },

                beforeSend: function () {
                    $('.ajax-loader').css("visibility", "visible");
                },
                success: function (data) {
                    $("#affichageIATA").html(data);

                },
                complete: function () {
                    $('.ajax-loader').css("visibility", "hidden");

                    alert("Record Deleted Successfully")
                    tableIataSeason();

                    this.itemIATASension = "";
                    this.itemFrom = "";
                    this.itemTo = "";
                }
            });
        }
    } else {

    }
}

/* END  Delete IATASEASON    Joseph*/


/* Delete  ASV    Joseph*/
let itemspaASV = "";
let itemASV = "";

function getDeleteASV(spaAsv, asv) {
    this.itemspaASV = spaAsv;
    this.itemASV = asv;

}

function deleteASV() {

    let spa = this.itemspaASV;
    let asv = this.itemASV;

    let spaNumber = $("#spaNumber").val();
    let asvNumber = $("#asvNumber").val();


    if (spa != "") {

        if (window.confirm("Are you sure you want to delete the SPA NUMBER: " + spa + " and ASV NUMBER: " + asv + "  ?")) {

            $.ajax({
                type: "POST",
                url: "/Process/DeleteASV",

                data: { spa: spa, asv: asv, spaNumber: spaNumber, asvNumber: asvNumber },

                beforeSend: function () {
                    $('.ajax-loader').css("visibility", "visible");
                },
                success: function (data) {
                    $("#affichagASV").html(data);

                },
                complete: function () {
                    $('.ajax-loader').css("visibility", "hidden");

                    alert("Record Deleted Successfully")
                    tableASV();

                    let itemspaASV = "";
                    let itemASV = "";
                }
            });
        }
    } else {

    }
}

/* End  Delete  ASV    Joseph*/


/* Delete  Discount   Joseph*/

let itemspaDiscount = ""

function getDeleteDiscount(spaNumber) {
    this.itemspaDiscount = spaNumber;
}

function deleteDiscount() {

    let spa = this.itemspaDiscount;

    let spaNumber = $("#spaNumberDiscount").val();
   

    if (spa != "") {

        if (window.confirm("Are you sure you want to delete the SPA NUMBER: " + spa + " ?")) {

            $.ajax({
                type: "POST",
                url: "/Process/DeleteDiscount",

                data: { spa: spa,  spaNumber: spaNumber},

                beforeSend: function () {
                    $('.ajax-loader').css("visibility", "visible");
                },
                success: function (data) {
                    $("#affichagDiscount").html(data);

                },
                complete: function () {
                    $('.ajax-loader').css("visibility", "hidden");

                    alert("Record Deleted Successfully")
                    tableDiscount();

                    this.itemspaDiscount = ""
                }
            });
        }
    } else {

    }
}
/* End  Delete  Discount   Joseph*/


/* Delete  SPA_FlightRestriction  Joseph*/
let itemsFlight = ""

function getDeleteFlight(spaNumber) {
    this.itemsFlight = spaNumber;
}

function deleteFlight() {

    let spa = this.itemsFlight;

    let spaNumber = $("#spaNumberFlight").val();


    if (spa != "") {

        if (window.confirm("Are you sure you want to delete the SPA NUMBER: " + spa + " ?")) {

            $.ajax({
                type: "POST",
                url: "/Process/DeleteFlightRestriction",

                data: { spa: spa, spaNumber: spaNumber },

                beforeSend: function () {
                    $('.ajax-loader').css("visibility", "visible");
                },
                success: function (data) {
                    $("#affichagFlight").html(data);

                },
                complete: function () {
                    $('.ajax-loader').css("visibility", "hidden");

                    alert("Record Deleted Successfully")
                    tableFlight();

                    this.itemsFlight = ""
                }
            });
        }
    } else {

    }
}

/* END  Delete  SPA_FlightRestriction  Joseph*/


/* Delete   SPA Exception   Joseph*/
let itemsSpaExcep = "";

function getDeleteSPAExcep(spaNumber) {
    this.itemsSpaExcep = spaNumber;
}

function deleteSPAException() {

    let spa = this.itemsSpaExcep;

    let spaNumber = $("#spaNumberExcep").val();

    if (spa != "") {

        if (window.confirm("Are you sure you want to delete the SPA NUMBER: " + spa + " ?")) {

            $.ajax({
                type: "POST",
                url: "/Process/DeleteSPA_Exception",

                data: { spa: spa, spaNumber: spaNumber },

                beforeSend: function () {
                    $('.ajax-loader').css("visibility", "visible");
                },
                success: function (data) {
                    $("#affichageExcep").html(data);

                },
                complete: function () {
                    $('.ajax-loader').css("visibility", "hidden");

                    alert("Record Deleted Successfully")
                    tableSpaExcept();

                    this.itemsSpaExcep = ""
                }
            });
        }
    } else {

    }
}

/* End Delete   SPA Exception   Joseph*/


/* Delete  ASV Exception  Joseph*/
let itemsAsvExcept = "";

function getDeleteASVExcept(spaNumber) {
    this.itemsAsvExcept = spaNumber;
}

function deleteASVException() {

    let spa = this.itemsAsvExcept;

    let spaNumber = $("#spaNumberAsvExcep").val();

    if (spa != "") {

        if (window.confirm("Are you sure you want to delete the SPA NUMBER: " + spa + " ?")) {

            $.ajax({
                type: "POST",
                url: "/Process/DeleteSPA_AsvException",

                data: { spa: spa, spaNumber: spaNumber },

                beforeSend: function () {
                    $('.ajax-loader').css("visibility", "visible");
                },
                success: function (data) {
                    $("#affichageAsvExcep").html(data);

                },
                complete: function () {
                    $('.ajax-loader').css("visibility", "hidden");

                    alert("Record Deleted Successfully")
                    tableAsvExcept();

                    this.itemsAsvExcept = ""
                }
            });
        }
    } else {

    }
}
/* End Delete  ASV Exception  Joseph*/


/* Delete SPA_Signatories Joseph*/
let itemsSignator = "";

function getDeleteSignator(spaNumber) {
    this.itemsSignator = spaNumber;
}

function deleteSignatories() {

    let spa = this.itemsSignator;

    let spaNumber = $("#spaSignatorie").val();

    if (spa != "") {

        if (window.confirm("Are you sure you want to delete the SPA NUMBER: " + spa + " ?")) {

            $.ajax({
                type: "POST",
                url: "/Process/DeleteSPA_Signatories",

                data: { spa: spa, spaNumber: spaNumber },

                beforeSend: function () {
                    $('.ajax-loader').css("visibility", "visible");
                },
                success: function (data) {
                    $("#affichageSignatorie").html(data);

                },
                complete: function () {
                    $('.ajax-loader').css("visibility", "hidden");

                    alert("Record Deleted Successfully")
                    tableSignatories();

                    this.itemsSignator = "";
                }
            });
        }
    } else {

    }
}

/* End  Delete SPA_Signatories Joseph*/


/* End SPA Management    Joseph*/


/*  Usage Date vs Booking   Joseph*/
function SearchUsageDateBooking() {

    let dateFrom = $("#FromUsageBooking").val();
    let dateTo = $("#ToUsageBooking").val();
    let url = symphony + "/Process/SearchUsageDateBooking";

    $.ajax({
        type: 'POST',
        url: url,

        data: { dateFrom: dateFrom, dateTo: dateTo },

        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#search").html(data);
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
            tableUsageDateBooking();
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}


function tableUsageDateBooking() {
    $(document).ready(function () {
        $('#TableUsageData').DataTable({
            "pageLength": 100,
            dom: 'Bfrtip',
            buttons: [
				 'csv'
            ]
        });
    });
}

function ClearBookingDate() {

    let dateFrom = $("#FromUsageBooking").val();
    let dateTo = $("#ToUsageBooking").val();
    let url = symphony + "/Process/UsageDateBookingDate";

    $.ajax({
        type: 'POST',
        url: url,

        data: { dateFrom: dateFrom, dateTo: dateTo },

        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#search").html(data);
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
           
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}

/* End Usage Date vs Booking   Joseph*/


/*  Outward Billing Manual(Engines)-General   Joseph*/
function ChangeAirlenCode() {

    let valAirlineCode = $("#airlineCode").val();
    let valRecords = $("#records").val();
    let url = symphony + "/Process/GetChangeAirlineCode";

    $.ajax({
        type: 'POST',
        url: url,

        data: { valAirlineCode: valAirlineCode, valRecords: valRecords },

        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#OutwardBillingManual").html(data);
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
            $("#airlineCode").val(valAirlineCode);

            $('#label').html('Read Only');

            tableBillingManual();
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });

}

function ChangeBillingPeriod() {

    let valAirlineCode = $("#airlineCode").val();
    let valBillingPeriod = $("#billingPeriod").val();
    let valRecords = $("#records").val();
  
    let url = symphony + "/Process/GetChangeBillingPeriod";

    $.ajax({
        type: 'POST',
        url: url,

        data: { valAirlineCode: valAirlineCode, valBillingPeriod: valBillingPeriod, valRecords: valRecords },

        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#OutwardBillingManual").html(data);
        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");

            $("#airlineCode").val(valAirlineCode);
            $("#billingPeriod").val(valBillingPeriod);
            $("#records").val(valRecords);

            tableBillingManual();

            $('#label').html('Read Only');
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}

function changeRecord() {
    ChangeBillingPeriod();
}


function tableBillingManual() {
    $(document).ready(function () {
        var table = $('#TableBilling').DataTable({
            "pageLength": 100,
            dom: 'Bfrtip',
            buttons: [
           //	 'csv'
            ]
        });
        $('#TableBilling tbody').on('click', 'tr', function () {
            if ($(this).hasClass('selected')) {
                $(this).removeClass('selected');
            }
            else {
                table.$('tr.selected').removeClass('selected');
                $(this).addClass('selected');
            }
        });
        $('#button').click(function () {
            table.row('.selected').remove().draw(false);
        });
    });
}


function getDataEntry(
    param1, param2, param3,param4,param5, param6, param7,
    param8, param9, param10, param11, param12, param13,
    param14,param15, param16,param17,param18
    ) {

        let valAirlineCode = $("#airlineCode").val();
        let valBillingPeriod = $("#billingPeriod").val();
        let valRecords = $("#records").val();

    let url = symphony + "/Process/GetDataEntry";

        $.ajax({
            type: 'POST',
            url: url,

            data: { param1: param1, valBillingPeriod: valBillingPeriod, valRecords: valRecords, param7: param7, param8: param8, param13: param13, param17: param17, param2: param2, param3: param3, param4: param4 },

            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {
                $("#OutwardBillingManual").html(data);
            },
            complete: function () {
                $('.ajax-loader').css("visibility", "hidden");

                $("#BillingAirline").val(param1);
                $("#AirlineCode").val(param2);
                $("#DocumentNumber").val(param3);
                $("#CouponNumber").val(param4);
                $("#CheckDigit").val(param5);
                $("#SourceCode").val(param6);
                $("#SectorFrom").val(param7);
                $("#SectorTo").val(param8);
                $("#Amount").val(param9);
                $("#ISC").val(param10);
                $("#ISCCommissionAmount").val(param11);
                $("#FlightNumber").val(param12);
                $("#FlightDate").val(param13);
                $("#Currency").val(param14);
                $("#ETKTSAC").val(param15);
                $("#InvoiceNumber").val(param16);
                $("#BillingPeriod").val(param17);
                $("#DataSource").val(param18);

                $("#airlineCode").val(valAirlineCode);
                $("#billingPeriod").val(valBillingPeriod);
                $("#records").val(valRecords);

               

                $("#Year").val($("#valYear").val());
                $("#Month").val($("#valMonth").val());
                $("#Period").val($("#valPeriod").val());

                $('#label').html('Read Only');

                // tableBillingManual();
                tableCouponDetail();
                tableCouponUsage()
                // Dynamique Coupon Detail
                FlownCoupon();

            }
           /* error: function () {
                $('#errorModal').modal('show');
            }*/
        });
}


function editBillingManual() {
    $('#label').html('Edit Record');
    $("#BillingAirline, #AirlineCode,#DocumentNumber,#CouponNumber,#CheckDigit,#SourceCode,#SectorFrom,#SectorTo,#Amount,#ISC,#ISCCommissionAmount,#FlightNumber,#FlightDate,#Currency,#ETKTSAC,#InvoiceNumber,#BillingPeriod,#DataSource")
        .prop("readonly", false);


}

function undoBillingManual() {
    $('#label').html('Read Only');
    $("#BillingAirline, #AirlineCode,#DocumentNumber,#CouponNumber,#CheckDigit,#SourceCode,#SectorFrom,#SectorTo,#Amount,#ISC,#ISCCommissionAmount,#FlightNumber,#FlightDate,#Currency,#ETKTSAC,#InvoiceNumber,#BillingPeriod,#DataSource")
        .prop("readonly", true);
}

/*   Delet Billing Manual  Joseph*/
function deletBillingManual() {

    let valAirlineCode = $("#airlineCode").val();
    let valBillingPeriod = $("#billingPeriod").val();
    let valRecords = $("#records").val();

    if (valAirlineCode == "-ALL-") {
        $('#alertModal').modal('show');
        $('#msgalert').text('Invalid Airline Selection');
    } else {

        if (window.confirm("Are you sure to Delete the Outward Billing Entries For airline " + valAirlineCode + "  and for the Period of " + valBillingPeriod + " ")) {

            $.ajax({
                type: "POST",
                url: "/Process/DeleteOutWardBilling",

                data: { valAirlineCode: valAirlineCode, valBillingPeriod: valBillingPeriod, valRecords:valRecords },

                beforeSend: function () {
                    $('.ajax-loader').css("visibility", "visible");
                },
                success: function (data) {
                    $("#OutwardBillingManual").html(data);

                },
                complete: function () {
                    $('.ajax-loader').css("visibility", "hidden");
                    tableBillingManual();

                    alert("Record Deleted Successfully")
                   
                }
            });
        }
    }

}


/*Save Billing Manual  Joseph*/

function SaveBillingManual() {

    let valAirlineCode = $("#airlineCode").val();
    let valBillingPeriod = $("#billingPeriod").val();
    let valRecords = $("#records").val();

    let param1 =  $("#BillingAirline").val();
    let param2 = $("#AirlineCode").val();
    let param3 = $("#DocumentNumber").val();
    let param4 =$("#CouponNumber").val();
    let param5 =$("#CheckDigit").val();
    let param6 = $("#SourceCode").val();
    let param7 = $("#SectorFrom").val();
    let param8 = $("#SectorTo").val();
    let param9 = $("#Amount").val();
    let param10 = $("#ISC").val();
    let param11 = $("#ISCCommissionAmount").val();
    let param12 = $("#FlightNumber").val();
    let param13 =$("#FlightDate").val();
    let param14 =$("#Currency").val();
    let param15 = $("#ETKTSAC").val();
    let param16 = $("#InvoiceNumber").val();
    let param17 = $("#BillingPeriod").val();
    let param18 = $("#DataSource").val();

    if (param3 == "") {
        $('#alertModal').modal('show');
        $('#msgalert').text('Please Enter Document Number  ');
    } else if (param4 == "") {
        $('#alertModal').modal('show');
        $('#msgalert').text('Please Enter Coupon Number ');
    } else {

        let url = symphony + "/Process/SaveOutwardBilling";

        $.ajax({
            type: 'POST',
            url: url,

            data: {
                param1: param1, param2: param2, param3: param3, param4: param4, param5: param5, param6: param6, param7: param7, param8: param8, param9: param9,
                param10: param10, param11: param11, param12: param12, param13: param13, param14: param14, param15: param15, param16: param16, param17: param16, param18: param18,
                valAirlineCode: valAirlineCode, valBillingPeriod: valBillingPeriod, valRecords: valRecords
            },

            beforeSend: function () {
                $('.ajax-loader').css("visibility", "visible");
            },
            success: function (data) {
                $("#OutwardBillingManual").html(data);
            },
            complete: function () {
                $('.ajax-loader').css("visibility", "hidden");

                $("#BillingAirline").val(param1);
                $("#AirlineCode").val(param2);
                $("#DocumentNumber").val(param3);
                $("#CouponNumber").val(param4);
                $("#CheckDigit").val(param5);
                $("#SourceCode").val(param6);
                $("#SectorFrom").val(param7);
                $("#SectorTo").val(param8);
                $("#Amount").val(param9);
                $("#ISC").val(param10);
                $("#ISCCommissionAmount").val(param11);
                $("#FlightNumber").val(param12);
                $("#FlightDate").val(param13);
                $("#Currency").val(param14);
                $("#ETKTSAC").val(param15);
                $("#InvoiceNumber").val(param16);
                $("#BillingPeriod").val(param17);
                $("#DataSource").val(param18);

                $("#airlineCode").val(valAirlineCode);
                $("#billingPeriod").val(valBillingPeriod);
                $("#records").val(valRecords);

                $('#label').html('Read Only');

                tableBillingManual();
                undoBillingManual();

                alert("Record UpDate Successfully")
            },
            error: function () {
                $('#errorModal').modal('show');
            }
        });

    }
}

function showreportsCreditmanagement(id, classe) {
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
function NewBillingManual() {

    console.log('new click')

    $('#label').html('New Record');

    $("#BillingAirline, #AirlineCode,#DocumentNumber,#CouponNumber,#CheckDigit,#SourceCode,#SectorFrom,#SectorTo,#Amount,#ISC,#ISCCommissionAmount,#FlightNumber,#FlightDate,#Currency,#ETKTSAC,#InvoiceNumber,#BillingPeriod,#DataSource")
     .prop("readonly", false);

    $("#BillingAirline").val("");
    $("#AirlineCode").val(""); 
    $("#DocumentNumber").val("");
    $("#CouponNumber").val("");
    $("#CheckDigit").val("");
    $("#SourceCode").val("");
    $("#SectorFrom").val("");
    $("#SectorTo").val("");
    $("#Amount").val("");
    $("#ISC").val("");
    $("#ISCCommissionAmount").val("");
    $("#FlightNumber").val("");
    $("#FlightDate").val("");
    $("#Currency").val("");
    $("#ETKTSAC").val("");
    $("#InvoiceNumber").val("");
    $("#BillingPeriod").val("");
    $("#DataSource").val("");
}


function ChangeXmlBilling() {

    let valXmlBilling = $("#xmlBilling").val();

    let url = symphony + "/Process/XMLBillingChange";

    $.ajax({
        type: 'POST',
        url: url,

        data: { valXmlBilling: valXmlBilling },

        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#OutwardBillingManual").html(data);
        },
        complete: function () {
            $("#airlineCode").val($("#getAirline").val());
            $("#billingPeriod").val(valXmlBilling);
            $("#xmlBilling").val(valXmlBilling);
            $('.ajax-loader').css("visibility", "hidden");
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}


/* Detail Coupon  Joseph*/
function tableCouponDetail() {
    $(document).ready(function () {
        $('#TableCouponDetail').DataTable({
            "pageLength": 100,
            dom: 'Bfrtip',
            buttons: [
				 'csv'
            ]
        });
    });
}

function tableCouponUsage() {
    $(document).ready(function () {
        $('#TableCouponUsage').DataTable({
            "pageLength": 100,
            dom: 'Bfrtip',
            buttons: [
				 'csv'
            ]
        });
    });
}


function FlownCoupon() {
    $("#FlownCoupon").prop("checked", true);

    $("#CouponUsage").hide();
    $("#CouponDetail").show();
}

function FlownUsage() {
    $("#FlownUsage").prop("checked", true);

    $("#CouponDetail").hide();
    $("#CouponUsage").show();
}
/* End Detail Coupon  Joseph*/


function XmlexportBilling() {

    let url = symphony + "/Process/GenerateSIS";

    $.ajax({
        type: 'POST',
        url: url,

        data: {},

        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#OutwardBillingManual").html(data);
        },
        complete: function () {
            $('#alertModal').modal('show');
            $('#msgalert').text('Process Completed  Outward Billing: SIS IS-XML File Generator');
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}

/* END  Outward Billing Manual(Engines)-General   Joseph*/




/*  Interline   Joseph*/

function ChangeInvoiceNumberold() {

    let valInvoiceNumber = $("#invoiceNumber").val(); 
    let valbillingPeriod = $("#billingPeriod").val();
    let valchargeCode = $("#chargeCode").val();
    let valprocesStatus = $("#procesStatus").val();

    let param12 = 3;

    let url = symphony + "/Interline/InterlineInvoice";

    $.ajax({
        type: 'POST',
        url: url,

        data: { valInvoiceNumber: valInvoiceNumber, valbillingPeriod: valbillingPeriod, valchargeCode: valchargeCode, valprocesStatus: valprocesStatus },

        beforeSend: function () {
           // $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#invoice").html(data);
        },
        complete: function () {

            $("#invoiceNumber").val(valInvoiceNumber)
            $("#billingPeriod").val(valbillingPeriod)
            $("#chargeCode").val(valchargeCode)
            $("#procesStatus").val(valprocesStatus)

            if (param12 == 1 || param12 == 2) {
                $("#BTprocessInvoice").hide();
                $("#BtAlreadyProcess").show();
            }else if (param12 == 3) {
                $("#BTprocessInvoice").hide();
                $("#BtAlreadyProcess").hide();
            }else{
                $("#BTprocessInvoice").show();
                $("#BtAlreadyProcess").hide();
            }
           // $('.ajax-loader').css("visibility", "hidden");
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}

function ChangeInvoiceNumber() {
    ListeInterlineInvoice();
}

function ChangeBillingPeriodInterline() {
    ListeInterlineInvoice()
}

function ChangeChargeCode() {
    ListeInterlineInvoice()
}

function ChangeProcesStatus() {
    ListeInterlineInvoice()
}

// Liste general Interlin Invoice

function ListeInterlineInvoice() {

    let valInvoiceNumber = $("#invoiceNumber").val();
    let valbillingPeriod = $("#billingPeriod").val();
    let valchargeCode = $("#chargeCode").val();
    let valprocesStatus = $("#procesStatus").val();

    let url = symphony + "/Interline/ChangeInterlineInvoice";

    $.ajax({
        type: 'POST',
        url: url,

        data: { valInvoiceNumber: valInvoiceNumber, valbillingPeriod: valbillingPeriod, valchargeCode: valchargeCode, valprocesStatus: valprocesStatus },

        beforeSend: function () {
            // $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#ListeInterlineResult").html($(data).filter('#NewListeInterlineResult'));
        },
        complete: function () {

            $("#invoiceNumber").val(valInvoiceNumber)
            $("#billingPeriod").val(valbillingPeriod)
            $("#chargeCode").val(valchargeCode)
            $("#procesStatus").val(valprocesStatus)

            // $('.ajax-loader').css("visibility", "hidden");
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });

}

// Interline Invoice bt Query

function InvoiceQuery() {
    let valInvoiceNumber = $("#invoiceNumber").val();
    let valbillingPeriod = $("#billingPeriod").val();
    let valchargeCode = $("#chargeCode").val();
    let valprocesStatus = $("#procesStatus").val();
    let param12 = 3;

    let url = symphony + "/Interline/ChangeInterlineInvoice";

    $.ajax({
        type: 'POST',
        url: url,

        data: { valInvoiceNumber: valInvoiceNumber, valbillingPeriod: valbillingPeriod, valchargeCode: valchargeCode, valprocesStatus: valprocesStatus },

        beforeSend: function () {
             $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            // $("#invoice").html(data);
            $("#ListeInterlineResult").html($(data).filter('#NewListeInterlineResult'));
        },
        complete: function () {

            $("#invoiceNumber").val(valInvoiceNumber)
            $("#billingPeriod").val(valbillingPeriod)
            $("#chargeCode").val(valchargeCode)
            $("#procesStatus").val(valprocesStatus)

            if (param12 == 1 || param12 == 2) {
                $("#BTprocessInvoice").hide();
                $("#BtAlreadyProcess").show();
            } else if (param12 == 3) {
                $("#BTprocessInvoice").hide();
                $("#BtAlreadyProcess").hide();
            } else {
                $("#BTprocessInvoice").show();
                $("#BtAlreadyProcess").hide();
            }

             $('.ajax-loader').css("visibility", "hidden");
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}




function loadInterline(
    param0,param1, param2, param3,param4,param5, param6, param7,
    param8, param9, param10, param11, param12, param13,
    param14,param15, param16,param17,param18,param19,param20,param21
    ) {
        let valInvoiceNumber = $("#invoiceNumber").val();
        let valbillingPeriod = $("#billingPeriod").val();
        let valchargeCode = $("#chargeCode").val();
        let valprocesStatus = $("#procesStatus").val();
        
        let url = symphony + "/Interline/ChangeInterlineInvoice";

    $.ajax({
        type: 'POST',
        url: url,

        data: {
            param0: param0,param1: param1, param2: param2, param3: param3, param4: param4, param5: param5, param6: param6, param7: param7, param8: param8, param9: param9,
            param10: param10, param11: param11, param12: param12, param13: param13, param14: param14, param15: param15, param16: param16, param17: param16, param18: param18,
            param19: param19, param20: param20, param21: param21, valInvoiceNumber: valInvoiceNumber, valbillingPeriod: valbillingPeriod, valchargeCode: valchargeCode,
            valprocesStatus: valprocesStatus
        },

        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            // $("#LoadInvoiceResult").html(data);
            $("#currencyHead").html($(data).filter('#NewcurrencyHead'));
            $("#LoadInvoiceResult").html($(data).filter('#NewLoadInvoiceResult'));
            $("#descriptionInvoice").html($(data).filter('#NewdescriptionInvoice'));

            $("#buttonProcessInvoice").html($(data).filter('#NewbuttonProcessInvoice'));
            $("#contenue").html($(data).filter('#Newcontenue'));

            $("#TotalMached").html($(data).filter('#NewTotalMached'));


        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");

            if (param12 == 1 || param12 == 2) {
                $("#BTprocessInvoice").hide();
                $("#BtAlreadyProcess").show();
            } else if (param12 == 3) {
                $("#BTprocessInvoice").hide();
                $("#BtAlreadyProcess").hide();
            
            } else {
                $("#BTprocessInvoice").show();
                $("#BtAlreadyProcess").hide();
            }

           


            $("#invoiceNumber").val(valInvoiceNumber)
            $("#billingPeriod").val(valbillingPeriod)
            $("#chargeCode").val(valchargeCode)
            $("#procesStatus").val(valprocesStatus)

            /* Index Recuperation DatagridView2*/
            $("#indexParam0").val(param0)
            $("#indexParam1").val(param1)
            $("#indexParam2").val(param2)
            $("#indexParam3").val(param3)
            $("#indexParam4").val(param4)
            $("#indexParam5").val(param5)
            $("#indexParam6").val(param6)
            $("#indexParam7").val(param7)
            $("#indexParam8").val(param8)
            $("#indexParam9").val(param9)
            $("#indexParam10").val(param10)
            $("#indexParam11").val(param11)
            $("#indexParam12").val(param12)
            $("#indexParam13").val(param13)
            $("#indexParam14").val(param14)
            $("#indexParam15").val(param15)
            $("#indexParam16").val(param16)
            $("#indexParam17").val(param17)
            $("#indexParam18").val(param18)
            $("#indexParam19").val(param19)
            $("#indexParam20").val(param20)
            $("#indexParam21").val(param21)
            /* End Index Recuperation DatagridView2*/              
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}



// function click Load Interline

/*
    index represente grid 

*/

function clickLoadInterline(selection,
    index0, index1, index2, index3, index4, index5, index6, index7,
    index8, index9, index10, index11, index12, index13,
    index14, index15, index16, index17, index18, index19, index20, index21, index22, index23, index24, index25, index26
    ) {

    console.log("index selection",selection)


    let valInvoiceNumber = $("#invoiceNumber").val();
    let valbillingPeriod = $("#billingPeriod").val();
    let valchargeCode = $("#chargeCode").val();
    let valprocesStatus = $("#procesStatus").val();
    let param12 = 3;

    /*get Index DatagridView12*/
    let grid0 =  $("#indexParam0").val();
    let grid1 =  $("#indexParam1").val();
    let grid2 = $("#indexParam2").val();
    let grid3 = $("#indexParam3").val();
    let grid4 =  $("#indexParam4").val();
    let grid5 =  $("#indexParam5").val();
    let grid6 =  $("#indexParam6").val();
    let grid7 =  $("#indexParam7").val();
    let grid8 =  $("#indexParam8").val();
    let grid9 =  $("#indexParam9").val();
    let grid10 =  $("#indexParam10").val();
    let grid11 = $("#indexParam11").val();
    let grid12 = $("#indexParam12").val();
    let grid13 = $("#indexParam13").val();
    let grid14 = $("#indexParam14").val();
    let grid15 = $("#indexParam15").val();
    let grid16 = $("#indexParam16").val();
    let grid17 =  $("#indexParam17").val();
    let grid18 = $("#indexParam18").val();
    let grid19 =  $("#indexParam19").val();
    let grid20 =  $("#indexParam20").val();
    let grid21 = $("#indexParam21").val();
    /*end get Index DatagridView12*/


    let url = symphony + "/Interline/ChargementAllInterlin";

    $.ajax({
        type: 'POST',
        url: url,

        data: {
            selection:selection, index0: index0, index1: index1, index2: index2, index3: index3, index4: index4, index5: index5, index6: index6, index7: index7, index8: index8, index9: index9,
            index10: index10, index11: index11, index12: index12, index13: index13, index14: index14, index15: index15, index16: index16, index17: index16, index18: index18,
            index19: index19, index20: index20, index21: index21, index22: index22, index23: index23, index24: index24, index25: index25, index26: index26, valInvoiceNumber: valInvoiceNumber, valbillingPeriod: valbillingPeriod, valchargeCode: valchargeCode,
            valprocesStatus: valprocesStatus, grid1: grid1,grid2:grid2,grid5:grid5
        },

        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {

            // $("#invoice").html($(data).filter('#invoice'));
            $("#documentNo_cpn").html($(data).filter('#NewdocumentNo_cpn'));

            $("#ListAddOnchange").html($(data).filter('#NewListAddOnchange'));
            
        },
        complete: function () {

            $("#invoiceNumber").val(valInvoiceNumber)
            $("#billingPeriod").val(valbillingPeriod)
            $("#chargeCode").val(valchargeCode)
            $("#procesStatus").val(valprocesStatus)

            if (grid12 == 1 || grid12 == 2) {
                $("#BTprocessInvoice").hide();
                $("#BtAlreadyProcess").show();
            } else if (grid12 == 3) {
                $("#BTprocessInvoice").hide();
                $("#BtAlreadyProcess").hide();
            } else {
                $("#BTprocessInvoice").show();
                $("#BtAlreadyProcess").hide();
            }

            $('.ajax-loader').css("visibility", "hidden");

            /*Affectation gridView2*/
            $("#indexParam0").val(grid0)
            $("#indexParam1").val(grid1)
            $("#indexParam2").val(grid2)
            $("#indexParam3").val(grid3)
            $("#indexParam4").val(grid4)
            $("#indexParam5").val(grid5)
            $("#indexParam6").val(grid6)
            $("#indexParam7").val(grid7)
            $("#indexParam8").val(grid8)
            $("#indexParam9").val(grid9)
            $("#indexParam10").val(grid10)
            $("#indexParam11").val(grid11)
            $("#indexParam12").val(grid12)
            $("#indexParam13").val(grid13)
            $("#indexParam14").val(grid14)
            $("#indexParam15").val(grid15)
            $("#indexParam16").val(grid16)
            $("#indexParam17").val(grid17)
            $("#indexParam18").val(grid18)
            $("#indexParam19").val(grid19)
            $("#indexParam20").val(grid20)
            $("#indexParam21").val(grid21)
            /* End Affectation gridView2 */


            /* Affectation gridView3 */
            $("#valindex0").val(index0)
            $("#valindex1").val(index1)
            $("#valindex2").val(index2)
            $("#valindex3").val(index3)
            $("#valindex4").val(index4)
            $("#valindex5").val(index5)
            $("#valindex6").val(index6)
            $("#valindex7").val(index7)
            $("#valindex8").val(index8)
            $("#valindex9").val(index9)
            $("#valindex10").val(index10)
            $("#valindex11").val(index11)
            $("#valindex12").val(index12)
            $("#valindex13").val(index13)
            $("#valindex14").val(index14)
            $("#valindex15").val(index15)
            $("#valindex16").val(index16)
            $("#valindex17").val(index17)
            $("#valindex18").val(index18)
            $("#valindex19").val(index19)
            $("#valindex20").val(index20)
            $("#valindex21").val(index21)
            $("#valindex22").val(index22)
            $("#valindex23").val(index23)
            $("#valindex24").val(index24)
            $("#valindex25").val(index25)
            $("#valindex26").val(index26)
            /* End Affectation gridView3 */

        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });

}
/* End  Interline   Joseph*/



/* RevenueAnalysis  Joseph */
function changeDateRevenue() {

    let valdateFrom = $("#FromRevenueAnalytics").val();
    let valdateTo = $("#ToRevenueAnalytics").val();
    let valFligth = $("#Fight").val();
    
    let url = symphony + "/Process/changeDateRevenue";

    $.ajax({
        type: 'POST',
        url: url,

        data: { valdateFrom: valdateFrom, valdateTo: valdateTo },

        beforeSend: function () {
           // $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#revenuAnalysis").html(data);
        },
        complete: function () {
            $("#FromRevenueAnalytics").val(valdateFrom);
            $("#ToRevenueAnalytics").val(valdateTo);


        },
        error: function () {
          //  $('#errorModal').modal('show');
        }
    });
}

function searchRevenuAnalysis() {
    let valdateFrom = $("#FromRevenueAnalytics").val();
    let valdateTo = $("#ToRevenueAnalytics").val();
    let valFligth = $("#Fight").val();

    let url = symphony + "/Process/ListeRevenuAnalysis";

    $.ajax({
        type: 'POST',
        url: url,

        data: { valdateFrom: valdateFrom, valdateTo: valdateTo, valFligth: valFligth },

        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#revenuAnalysis").html(data);
        },
        complete: function () {

            $("#FromRevenueAnalytics").val(valdateFrom);
            $("#ToRevenueAnalytics").val(valdateTo);
            $("#Fight").val(valFligth);

            let nb = $("#valnb").val();

            if (nb == 0) {
                $('#alertModal').modal('show');
                $('#msgalert').text('No data available for the selected criteria.');
            }

        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}

function clearRevenu() {
    let valdateFrom = $("#FromRevenueAnalytics").val();
    let valdateTo = $("#ToRevenueAnalytics").val();
    let valFligth = $("#Fight").val();

    let url = symphony + "/Process/RevenueAnalysis";

    $.ajax({
        type: 'POST',
        url: url,

        data: { valdateFrom: valdateFrom, valdateTo: valdateTo, valFligth: valFligth },

        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#revenuAnalysis").html(data);
        },
        complete: function () {

        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}
/*End RevenueAnalysis  Joseph */


/*Interline Code Shares  Joseph*/
function listCodeShares() {
    let url = symphony + "/Interline/ListeCodeshares";
    $.ajax({
        type: 'POST',
        url: url,
        data: {},

        beforeSend: function () {
            $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#codeShares").html(data);
        },
        complete: function () {

            let nb = $("#valnbCodeShare").val();

            if (nb == 0) {
                $('#alertModal').modal('show');
                $('#msgalert').text('No data available');
            }
            tableCodeShare();
            $('.ajax-loader').css("visibility", "hidden");

        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });

}

function tableCodeShare() {
    $(document).ready(function () {
        $('#TableCodeShares').DataTable({
            "pageLength": 100,
            dom: 'Bfrtip',
            buttons: [
				 'csv'
            ]
        });
    });
}


/* End Interline Code Shares  Joseph*/


/* Discount Uncollected   Joseph */
function changeDateUncollected() {

    let valdateFrom = $("#dateFromDiscUnc").val();
    let valdateTo = $("#dateToDiscUnc").val();

    let url = symphony + "/Process/ChangeDateUncollected";

    $.ajax({
        type: 'POST',
        url: url,

        data: { valdateFrom: valdateFrom, valdateTo: valdateTo },

        beforeSend: function () {
            // $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#discountUncollected").html(data);
        },
        complete: function () {

            $("#dateFromDiscUnc").val(valdateFrom);
            $("#dateToDiscUnc").val(valdateTo);
        },

        error: function () {
            //  $('#errorModal').modal('show');
        }
    });

}

function searchUncollected() {

    let valdateFrom = $("#dateFromDiscUnc").val();
    let valdateTo = $("#dateToDiscUnc").val();
    let valAgentNumCode = $("#AgentNumCodeU").val();


    let url = symphony + "/Process/RecherchAgentNumCode";

    $.ajax({
        type: 'POST',
        url: url,

        data: { valdateFrom: valdateFrom, valdateTo: valdateTo, valAgentNumCode: valAgentNumCode },

        beforeSend: function () {
             $('.ajax-loader').css("visibility", "visible");
        },
        success: function (data) {
            $("#discountUncollected").html(data);
        },
        complete: function () {
            $("#dateFromDiscUnc").val(valdateFrom);
            $("#dateToDiscUnc").val(valdateTo);
            $("#AgentNumCodeU").val(valAgentNumCode);

            let nb = $("#valDiscount").val();

            if (nb == 0) {
                $('#alertModal').modal('show');
                $('#msgalert').text('No data available for the selected criteria');
            }
            tableDiscountUnco();

            $('.ajax-loader').css("visibility", "hidden");
        },

        error: function () {
              $('#errorModal').modal('show');
        }
    });

}

function tableDiscountUnco() {
    $(document).ready(function () {
        $('#TableDiscountUn').DataTable({
            "pageLength": 100,
            dom: 'Bfrtip',
            buttons: [
				 'csv'
            ]
        });
    });
}

/* End Discount Uncollected   Joseph */



//sml discrepency analitique /////
function searchanalitics() {
    let dateFromanal = $("#dateFromanal").val();
    let dateToanal = $("#dateToanal").val();
    let ag = $("#AgentNumericCode").val();
    let url = symphony + "/Process/SearchDiscrepencyAnalitics";
    $.ajax({
        type: "POST",
        url: url,
        data: { dateFromanal: dateFromanal, dateToanal: dateToanal, ag: ag },
        success: function (data) {
            $("#tabNofare").html(data);
        },
        complete: function () {
            tableNofare();
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}
function searchrbd() {
    let dateFromRBD = $("#dateFromRBD").val();
    let dateToRBD = $("#dateToRBD").val();
    let ag = $("#AgentNumericCodeRBD").val();
    let url = symphony + "/Process/SearchRBDDiscrepency";
    $.ajax({
        type: "POST",
        url: url,
        data: { dateFromRBD: dateFromRBD, dateToRBD: dateToRBD, ag: ag },
        success: function (data) {
            $("#searchRBDDiscrep").html(data);
        },
        complete: function () {
            tableRBD();
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}
function searchdate() {
    let dateFromDate = $("#dateFromDate").val();
    let dateToDate = $("#dateToDate").val();
    let ag = $("#AgentNumericCodedate").val();
    let url = symphony + "/Process/SearchDateDiscrepency";
    $.ajax({
        type: "POST",
        url: url,
        data: { dateFromDate: dateFromDate, dateToDate: dateToDate, ag: ag },
        success: function (data) {
            $("#searchDateDiscrep").html(data);
        },
        complete: function () {
            tableDate();
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}
function changeDateanal() {
    $("#dateToanal").val("");
}
function changegetanal() {
    let dateFromanal = $("#dateFromanal").val();
    let dateToanal = $("#dateToanal").val();
    let url = symphony + "/Process/GetAgentNumanal";
    $.ajax({
        type: 'POST',
        url: url,
        data: { dateFromanal: dateFromanal, dateToanal: dateToanal },
        success: function (data) {
            $("#AgentNumericCode").html(data);
            console.log(data);
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });

}
function changeDateRBD() {
    $("#dateToRBD").val("");
}
function changegetRBD() {
    let dateFromRBD = $("#dateFromRBD").val();
    let dateToRBD = $("#dateToRBD").val();
    let url = symphony + "/Process/GetAgentRBD";
    $.ajax({
        type: 'POST',
        url: url,
        data: { dateFromRBD: dateFromRBD, dateToRBD: dateToRBD },
        success: function (data) {
            $("#AgentNumericCodeRBD").html(data);
            console.log(data);
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });

}
function changeDateDate() {
    $("#dateToDate").val("");
}
function changegetDate() {
    let dateFromDate = $("#dateFromDate").val();
    let dateToDate = $("#dateToDate").val();
    let url = symphony + "/Process/GetAgentDate";
    $.ajax({
        type: 'POST',
        url: url,
        data: { dateFromDate: dateFromDate, dateToDate: dateToDate },
        success: function (data) {
            $("#AgentNumericCodedate").html(data);
            console.log(data);
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });

}
function exportNofare() {
    var tab_text = "<table border='2px'><tr bgcolor='#87AFC6'>";
    var textRange; var j = 0;
    tab = document.getElementById('searchanal');

    for (j = 0 ; j < tab.rows.length ; j++) {
        tab_text = tab_text + tab.rows[j].innerHTML + "</tr>";
    }
    tab_text = tab_text + "</table>";
    tab_text = tab_text.replace(/<A[^>]*>|<\/A>/g, "");
    tab_text = tab_text.replace(/<img[^>]*>/gi, "");
    tab_text = tab_text.replace(/<input[^>]*>|<\/input>/gi, "");

    var ua = window.navigator.userAgent;
    var msie = ua.indexOf("MSIE ");

    if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) {
        txtArea1.document.open("txt/html", "replace");
        txtArea1.document.write(tab_text);
        txtArea1.document.close();
        txtArea1.focus();
        sa = txtArea1.document.execCommand("SaveAs", true, "Say Thanks to Sumit.xls");
    }
    else
        sa = window.open('data:application/vnd.ms-excel,' + encodeURIComponent(tab_text));

    return (sa);
}
function exportRBD() {
    var tab_text = "<table border='2px'><tr bgcolor='#87AFC6'>";
    var textRange; var j = 0;
    tab = document.getElementById('searchRBDDiscrep');

    for (j = 0 ; j < tab.rows.length ; j++) {
        tab_text = tab_text + tab.rows[j].innerHTML + "</tr>";
    }
    tab_text = tab_text + "</table>";
    tab_text = tab_text.replace(/<A[^>]*>|<\/A>/g, "");
    tab_text = tab_text.replace(/<img[^>]*>/gi, "");
    tab_text = tab_text.replace(/<input[^>]*>|<\/input>/gi, "");

    var ua = window.navigator.userAgent;
    var msie = ua.indexOf("MSIE ");

    if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) {
        txtArea1.document.open("txt/html", "replace");
        txtArea1.document.write(tab_text);
        txtArea1.document.close();
        txtArea1.focus();
        sa = txtArea1.document.execCommand("SaveAs", true, "Say Thanks to Sumit.xls");
    }
    else
        sa = window.open('data:application/vnd.ms-excel,' + encodeURIComponent(tab_text));

    return (sa);
}
function exportDate() {
    var tab_text = "<table border='2px'><tr bgcolor='#87AFC6'>";
    var textRange; var j = 0;
    tab = document.getElementById('searchDateDiscrep');

    for (j = 0 ; j < tab.rows.length ; j++) {
        tab_text = tab_text + tab.rows[j].innerHTML + "</tr>";
    }
    tab_text = tab_text + "</table>";
    tab_text = tab_text.replace(/<A[^>]*>|<\/A>/g, "");
    tab_text = tab_text.replace(/<img[^>]*>/gi, "");
    tab_text = tab_text.replace(/<input[^>]*>|<\/input>/gi, "");

    var ua = window.navigator.userAgent;
    var msie = ua.indexOf("MSIE ");

    if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) {
        txtArea1.document.open("txt/html", "replace");
        txtArea1.document.write(tab_text);
        txtArea1.document.close();
        txtArea1.focus();
        sa = txtArea1.document.execCommand("SaveAs", true, "Say Thanks to Sumit.xls");
    }
    else
        sa = window.open('data:application/vnd.ms-excel,' + encodeURIComponent(tab_text));

    return (sa);
}
function tableNofare() {
    $(document).ready(function () {
        $('#tabNofare').DataTable({
            "pageLength": 100,
        });
    });
}
function tableRBD() {
    $(document).ready(function () {
        $('#tabRBD').DataTable({
            "pageLength": 100,
        });
    });
}
function tableDate() {
    $(document).ready(function () {
        $('#tabDate').DataTable({
            "pageLength": 100,
        });
    });
}
function clearNofare() {
    let url = symphony + "/Process/clearFare";
    $.ajax({
        async: true,
        type: "POST",
        url: url,
        data: {},
        success: function (data) {
            $("#AgentNumericCode").html($(data).filter('#newAgentNumericCode'));
            $("#tabNofarea").html($(data).filter('#newtabNofarea'));
        },
    });
}
function clearRBD() {
    let url = symphony + "/Process/clearRBD";
    $.ajax({
        async: true,
        type: "POST",
        url: url,
        data: {},
        success: function (data) {
            $("#AgentNumericCodeRBD").html($(data).filter('#newAgentNumericCodeRBD'));
            $("#tabRBDa").html($(data).filter('#newtabRBDa'));
        },
    });
}
function clearDate() {
    let url = symphony + "/Process/clearDate";
    $.ajax({
        async: true,
        type: "POST",
        url: url,
        data: {},
        success: function (data) {
            $("#AgentNumericCodedate").html($(data).filter('#newAgentNumericCodeDate'));
            $("#tabDatea").html($(data).filter('#newtabDatea'));
        },
    });
}
function clickLigneTransactionNofare(event, docNumber, transactionCode, domInt) {

    var $form = $(event).closest('markelement');
    let parentId = $form.attr('id')
    let url = symphony + "/Sales/Transaction2";
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
            $("#search").html(data);
            // $("#transaction").html(data);

        },
        complete: function () {
            $('.ajax-loader').css("visibility", "hidden");
            $("#transaction").html(data);
        },
        error: function () {
            $('#errorModal').modal('show');
        }
    });
}

//sml  end discrepency analitique /////

