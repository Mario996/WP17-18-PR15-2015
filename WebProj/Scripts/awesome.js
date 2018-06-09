var appUser;

function home() {
	if (appUser != null) {
		if (appUser.IsAdmin) {
			$('#content').load('../Html/DispatcherPanel.html');
		} else if (appUser.IsDriver) {
			$('#content').load('../Html/DriverPage.html');
		} else if (appUser.IsCustomer) {
			$('#content').load('../Html/CustomerPage.html');
		}
	} else {
		location.reload();
	}
}

function Register() {
	var uri = '../api/user/registernewuser';

	var user = $('#user').val();
	var pass = $('#pass').val();
	var firstName = $('#firstName').val();
	var lastName = $('#lastName').val();
	var gender = $('#registerGender').val();
	var jmbg = $('#jmbg').val();
	var phoneNumber = $('#phoneNumber').val();
	var emailAddress = $('#emailAddress').val();
	var role = 'CUSTOMER'
	if (appUser != null && appUser.IsAdmin) {
		role = 'DRIVER'
	}

	var dataObject = {
		'username': user,
		'password': pass,
		'firstName': firstName,
		'lastName': lastName,
		'gender': gender,
		'jmbg': jmbg,
		'phoneNumber': phoneNumber,
		'emailAddress': emailAddress,
		'role': role
	};

	$.ajax({
		url: uri,
		type: 'POST',
		data: JSON.stringify(dataObject),
		contentType: 'application/json',
		success: function (result) {
			login(user, pass);
		},
		error: function (message) {
			alert(message.responseText);
		},
	});
}

function RegisterDriver() {
	var url = '../api/dispatcher/registerdriver';

	var user = $('#user').val();
	var pass = $('#pass').val();
	var firstName = $('#firstName').val();
	var lastName = $('#lastName').val();
	var gender = $('#gender').val();
	var jmbg = $('#jmbg').val();
	var phoneNumber = $('#phoneNumber').val();
	var emailAddress = $('#emailAddress').val();
	var role = 'DRIVER';
	var year = $('#vehYoP').val();
	var plate = $('#vehPlate').val();
	var taxiId = $('#vehIdentifier').val();
	var type = $('#vehType').val();

	var dataObject = {
		'username': user,
		'password': pass,
		'firstName': firstName,
		'lastName': lastName,
		'gender': gender,
		'jmbg': jmbg,
		'phoneNumber': phoneNumber,
		'emailAddress': emailAddress,
		'role': role,
		'year': year,
		'plate': plate,
		'taxiId': taxiId,
		'type': type
	};

	$.ajax({
		url: url,
		type: 'POST',
		data: JSON.stringify(dataObject),
		contentType: 'application/json',
		headers: {
			"Token": appUser.Id
		},
		success: function (result) {

		},
		error: function (message) {
			alert(message.responseText);
		},
	});
}

function updateLocation() {
	var uri = '../api/driver/updatelocation/' + appUser.Id;

	var locX = $('#locX').val();
	var locY = $('#locY').val();
	var addrStreet = $('#addrStreet').val();
	var addrNumber = $('#addrNumber').val();
	var addrCity = $('#addrCity').val();
	var addrPostalCode = $('#addrPostalCode').val();

	var dataObject = {
		'locX': locX,
		'locY': locY,
		'addrStreet': addrStreet,
		'addrNumber': addrNumber,
		'addrCity': addrCity,
		'addrPostalCode': addrPostalCode
	};

	$.ajax({
		url: uri,
		type: 'PUT',
		data: JSON.stringify(dataObject),
		contentType: 'application/json',
		headers: {
			"Token": appUser.Id
		},
		success: function (result) {
			home();
		},
		error: function (message) {
			alert(message.responseText);
		},
	});
}

function update() {
	var uri = '../api/user/updateuser/' + appUser.Id;
	var user = $('#userUp').val();
	var pass = $('#passUp').val();
	var firstName = $('#firstNameUp').val();
	var lastName = $('#lastNameUp').val();
	var gender = $('#genderUp').val();
	var jmbg = $('#jmbgUp').val();
	var phoneNumber = $('#phoneNumberUp').val();
	var emailAddress = $('#emailAddressUp').val();
	var blocked = $('#blocked').val();
	var dataObject = {
		'password': pass,
		'firstName': firstName,
		'lastName': lastName,
		'gender': gender,
		'jmbg': jmbg,
		'phoneNumber': phoneNumber,
		'emailAddress': emailAddress,
	};

	$.ajax({
		url: uri,
		type: 'PUT',
		data: JSON.stringify(dataObject),
		contentType: 'application/json',
		headers: {
			"Token": appUser.Id
		},
		success: function (result) {
			home();
			$("#profileButton").text(format(firstName, lastName))
		},
		error: function (message) {
			alert(message.responseText);
		},
	});
}

function updateUser(id) {
	var uri = '../api/user/updateuser/' + id;
	var pass = $('#passUp').val();
	var firstName = $('#firstNameUp').val();
	var lastName = $('#lastNameUp').val();
	var gender = $('#genderUp').val();
	var jmbg = $('#jmbgUp').val();
	var phoneNumber = $('#phoneNumberUp').val();
	var emailAddress = $('#emailAddressUp').val();
	var blocked = false;

	if ($('#blocked').is(":checked")) {
		blocked = true;
	}

	var dataObject = {
		'password': pass,
		'firstName': firstName,
		'lastName': lastName,
		'gender': gender,
		'jmbg': jmbg,
		'phoneNumber': phoneNumber,
		'emailAddress': emailAddress,
		'blocked': blocked
	};

	$.ajax({
		url: uri,
		type: 'PUT',
		data: JSON.stringify(dataObject),
		contentType: 'application/json',
		headers: {
			"Token": appUser.Id
		},
		success: function (result) {

		},
		error: function (message) {
			alert(message.responseText);
		},
	});
}

function getUserData() {
	var url = '../api/user/getuserdata/' + appUser.Id;
	$.ajax({
		url: url,
		type: 'POST',
		dataType: "json",
		contentType: 'application/json',
		headers: {
			"Token": appUser.Id
		},
		success: function (data) {
			let parsed = JSON.parse(data);
			$('#userUp').val(parsed.User.UserName);
			$('#passUp').val(parsed.User.Password);
			$('#firstNameUp').val(parsed.User.FirstName);
			$('#lastNameUp').val(parsed.User.LastName);
			$('#genderUp').val(parsed.User.GenderString);
			$('#jmbgUp').val(parsed.User.Idnumber);
			$('#phoneNumberUp').val(parsed.User.PhoneNumber);
			$('#emailAddressUp').val(parsed.User.EmailAddress);
			if (appUser != null && appUser.IsDriver) {
				$('#location').css('visibility', 'visible');
				$('#locX').val(parsed.Location.X);
				$('#locY').val(parsed.Location.X);

				$('#addrStreet').val(parsed.Address.Street);
				$('#addrNumber').val(parsed.Address.Number);
				$('#addrCity').val(parsed.Address.City);
				$('#addrPostalCode').val(parsed.Address.PostalCode);

			} else {
				$('#location').css('visibility', 'collapse');
			}
		},
	});
}

function loginUser() {
	var user = $('#user').val();
	var pass = $('#pass').val();
	login(user, pass);
}

function login(user, pass) {
	var uri = '../api/user/login';
	var dataObject = {
		'username': user,
		'password': pass,
	};

	var geturl;
	geturl = $.ajax({
		url: uri,
		type: 'POST',
		data: JSON.stringify(dataObject),
		contentType: 'application/json',
		success: function () {
			let parsed = JSON.parse(geturl.getResponseHeader("loggedUser"));
			appUser = parsed;
			$("#profileButton").css('visibility', 'visible');
			$("#profileButton").text(format(appUser.FirstName, appUser.LastName))
			$("#logoutButton").css('visibility', 'visible');
			$("#loginButton").css('visibility', 'collapse');
			$("#dispatcherButton").css('visibility', 'collapse');
			if (appUser.IsAdmin) {
				$('#content').load('../Html/DispatcherPanel.html');
			} else if (appUser.IsDriver) {
				$('#content').load('../Html/DriverPage.html');
			} else if (appUser.IsCustomer) {
				$('#content').load('../Html/CustomerPage.html');
			}
			$('#registerButton').css('visibility', 'collapse');
		},
		error: function (message) {
			alert(message.responseText);
		},
	});
}

function getLogedInUserWelcomeMessage() {
	return "Welcome " + format(appUser.FirstName, appUser.LastName);
}

function registerUser(param) {
	$(param).load('../Html/RegisterUser.html');
}

function logout() {
	if (appUser == null)
		return;
	var uri = '../api/user/logout/' + appUser.Id;

	$.ajax({
		url: uri,
		type: 'POST',
		contentType: 'application/json',
		success: function () {
			appUser = null;
			$("#profileButton").css('visibility', 'collapse');
			$("#dispatcherButton").css('visibility', 'collapse');
			$("#logoutButton").css('visibility', 'collapse');
			$("#loginButton").css('visibility', 'visible');
			$('#registerButton').css('visibility', 'visible');
			$("#content").html("");
			location.reload();
		},
		error: function (message) {
			alert(message.responseText);
		},
	});
}

function format(first, last) {
	return first + " " + last;
}

function initializeVehicles() {
	$('#registerVehicle').css('visibility', 'visible');
	getVehicleTypes('#vehType');
	getVehicleDrivers('#vehDriver')
}



function formatVehicle(reg, type, taxiId) {
	return reg + " " + type + " " + taxiId;
}

function formatFare(customer, startLocation, date) {
	return customer + " " + startLocation + " " + date;
}

function getUsers() {
	var url = '../api/dispatcher/getusers';
	$('#panelContent').load('../Html/AllUsersContent.html')
	$.ajax({
		url: url,
		type: 'GET',
		dataType: "json",
		contentType: 'application/json',
		headers: {
			"Token": appUser.Id
		},
		success: function (data) {
			$.each(data, function (key, item) {
				$('<tr>', { id: item.Id }).addClass('data').appendTo($('#usersBody'));
				$('#' + item.Id).bind('click', { id: item.Id }, function (event) {
					getUserDetails(event.data);
				});

				$('<td>', { text: item.FirstName }).addClass('data').appendTo($('#' + item.Id));
				$('<td>', { text: item.LastName }).addClass('data').appendTo($('#' + item.Id));
				$('<td>', { text: item.UserRole }).addClass('data').appendTo($('#' + item.Id));
			});
		},
		error: function (message) {
			alert(message.responseText);
		},
	});
}

function getVehicles() {
	var url = '../api/dispatcher/getvehicles';
	$('#panelContent').load('../Html/AllVehiclesContent.html')
	$.ajax({
		url: url,
		type: 'GET',
		dataType: "json",
		contentType: 'application/json',
		headers: {
			"Token": appUser.Id
		},
		success: function (data) {
			$.each(data, function (key, item) {

				$('<tr>', { id: item.Id }).addClass('data').appendTo($('#allVehiclesBody'));
				$('<td>', { text: item.Driver }).addClass('data').appendTo($('#' + item.Id));
				$('<td>', { text: item.YearOfProduction }).addClass('data').appendTo($('#' + item.Id));
				$('<td>', { text: item.Licence }).addClass('data').appendTo($('#' + item.Id));
				$('<td>', { text: item.TaxiId }).addClass('data').appendTo($('#' + item.Id));
				$('<td>', { text: item.VehicleTypeString }).addClass('data').appendTo($('#' + item.Id));
			});
		},
		error: function (message) {
			alert(message.responseText);
		},
	});
}

function getFares() {
	var url = '../api/dispatcher/getfares';
	$('#panelContent').load('../Html/AllFaresPage.html')
	$.ajax({
		url: url,
		type: 'GET',
		dataType: "json",
		contentType: 'application/json',
		headers: {
			"Token": appUser.Id
		},
		success: function (data) {
			$.each(data, function (key, item) {
				$('<tr>', { id: item.Id }).addClass('data').appendTo($('#faresBody'));
				$('#' + item.Id).bind('click', { control: '#fareDetail', id: item.Id, role: 'dispatcher' }, function (event) {
					getFareDetail(event.data);
				});
				$('<td>', { text: item.Customer }).addClass('data').appendTo($('#' + item.Id));
				$('<td>', { text: item.StartLocation }).addClass('data').appendTo($('#' + item.Id));
				$('<td>', { text: item.DateTimeString }).addClass('data').appendTo($('#' + item.Id));
				$('<td>', { text: item.Dispatcher }).addClass('data').appendTo($('#' + item.Id));
			});
			$('#allFares').css('visibility', 'visible');
		},
		error: function (message) {
			alert(message.responseText);
		},
	});
}

function getDispatcherFares() {
	var url = '../api/dispatcher/getdispatcherfares/' + appUser.Id;
	$('#dfaresBody').empty();
	$.ajax({
		url: url,
		type: 'GET',
		dataType: "json",
		contentType: 'application/json',
		headers: {
			"Token": appUser.Id
		},
		success: function (data) {
			// TODO ovde postoji problem za tabele za dispatchera
			// ne mogu da izvalim zasto
			$.each(data, function (key, item) {
				$('<tr>', { id: item.Id }).addClass('data').appendTo($('#dfaresBody'));
				$('#' + item.Id).bind('click', { control: '#fareDetail', id: item.Id, role: 'dispatcher' }, function (event) {
					getFareDetail(event.data);
				});
				$('<td>', { text: item.Customer }).addClass('data').appendTo($('#' + item.Id));
				$('<td>', { text: item.StartLocation }).addClass('data').appendTo($('#' + item.Id));
				$('<td>', { text: item.DateTimeString }).addClass('data').appendTo($('#' + item.Id));
				$('<td>', { text: item.Dispatcher }).addClass('data').appendTo($('#' + item.Id));
			});
		},
		error: function (message) {
			alert(message.responseText);
		},
	});
}

function getFareDetail(param) {
	var id = param.id;
	var url = '../api/dispatcher/getfare/' + id;
	$('#fareVehicle').empty();
	$('#fareDriverSelect').empty();
	getVehicleTypes('#fareVehicle');
	getFreeVehicleDrivers('#fareDriverSelect');
	$('#fareDetail').css('visibility', 'visible');

	if (param.role == 'customer') {
		$('#updateFareButton').bind('click', function (event) {
			updateCustomerFare(id);
		});
	}
	if (param.role == 'dispatcher') {
		$('#updateFareButton').bind('click', function (event) {
			updateDispatherFare(id);
		});
	}

	if (param.role == 'driver') {
		$('#updateFareButton').bind('click', function (event) {
			updateDriverFare(id);
		});
	}

	$.ajax({
		url: url,
		type: 'POST',
		dataType: "json",
		contentType: 'application/json',
		headers: {
			"Token": appUser.Id
		},
		success: function (data) {
			let parsed = JSON.parse(data);
			console.log(data);
			$('#fareDetailDate').val(parsed.DateTimeString);
			$('#fareStartLocation').val(parsed.StartLocation);
			$('#fareVehicle').val(parsed.VehicleType);
			$('#fareStatus').val(parsed.StatusString);
			$('#fareFinishLocation').val(parsed.FinishLocation);
			$('#fareCustomer').val(parsed.Customer);
			$('#fareDispatcher').val(parsed.Dispatcher);
			$('#farePrice').val(parsed.Price);
			$('#fareDriver').val(parsed.Driver);
			$('#fareComment').val(parsed.Comment);


			if (parsed.StatusString == 'CANCELED' || parsed.StatusString == 'FAILED' || parsed.StatusString == 'SUCCESFUL') {
				$('#updateFareButton').css('visibility', 'collapse');
				$('#fareStatus').attr('disabled', true);
				$('#fareDriverSelect').attr('disabled', true);
				$('#fareComment').attr('disabled', true);
				
			}
			if (parsed.StatusString == 'ON_HOLD') {
				$('#updateFareButton').css('visibility', 'visible');
				$('#fareStatus').attr('disabled', false);
				$('#fareDriverSelect').attr('disabled', true);
			}


			if (param.role == 'customer') {
				$('#fareStartLocation').attr('readonly', true);
				$('#fareCommentColumn').css('visibility', 'collapse');
				$('#fareRatingColumn').css('visibility', 'collapse');
				if (parsed.StatusString != 'ON_HOLD') {
					$('#fareStatus').attr('disabled', true);
					$('#readOnlyfareStartLocation').css('visibility', 'visible');
					$('#editablefareStartLocation').css('visibility', 'collapse');
					$('#fareDetailDate').attr('readonly', true);
					$('#fareVehicle').attr('disabled', true);
					$('#updateFareButton').attr('disabled', true);
					$('#fareComment').attr('readonly', true);
					$('#fareCommentColumn').css('visibility', 'visible');
				}
				else {
					$('#editablefareStartLocation').css('visibility', 'visible');
					$('#startlocX').val(parsed.StartLocationObject.X);
					$('#startlocY').val(parsed.StartLocationObject.Y);
					$('#startaddrStreet').val(parsed.StartAddressObject.Street);
					$('#startaddrNumber').val(parsed.StartAddressObject.Number);
					$('#startaddrCity').val(parsed.StartAddressObject.City);
					$('#startaddrPostalCode').val(parsed.StartAddressObject.PostalCode);
				}


				if (parsed.StatusString == 'SUCCESFUL') {
					$('#fareComment').attr('readonly', false);
					$('#fareCommentColumn').css('visibility', 'visible');
					$('#fareRatingColumn').css('visibility', 'visible');
					$('#fareStatus').attr('disabled', true);
					$('#readOnlyfareStartLocation').css('visibility', 'visible');
					$('#readOnlyfareFinishLocation').css('visibility', 'visible');
					$('#fareComment').attr('disabled', false);
					$('#updateFareButton').css('visibility', 'visible');
				}

				$('#driverFinishlocation').css('visibility', 'collapse');
				$('#driverFareVehicle').css('visibility', 'collapse');
			}
			if (param.role == 'dispatcher') {
				if (parsed.Customer != null && parsed.Customer != ' ' && parsed.Driver == null || parsed.Driver == ' ') {
					$('#hasDriverColumn').css('visibility', 'collapse');
					$('#choseDriverColumn').css('visibility', 'visible');
				}
				$('#fareDriver').attr('readonly', true);
				$('#fareDispatcher').attr('readonly', true);
			}
			if (param.role == 'driver') {
				$("#fareStatus > option").each(function () {
					str = $(this).text();
					$(this).attr('disabled', true);
					if (str == 'SUCCESFUL' || str == 'FAILED') {
						$(this).attr('disabled', false);
					}
				});
				if (parsed.StatusString == 'SUCCESFUL' || parsed.StatusString == 'FAILED') {
					$('#fareStatus').attr('disabled', true);
					$('#updateFareButton').css('visibility', 'collapse');
					if (parsed.StatusString == 'SUCCESFUL') {
						$('#readOnlyfareFinishLocation').css('visibility', 'visible');
					}
					if (parsed.StatusString == 'FAILED') {
						$('#readOnlyfareFinishLocation').css('visibility', 'collapse');
					}
				}
				$('#fareDriver').attr('readonly', true);
				$('#fareDispatcher').attr('readonly', true);
			}
		},
	});

}

function getUnassignedFares() {
	var url = '../api/driver/getunassignedfares/' + appUser.Id;
	$.ajax({
		url: url,
		type: 'GET',
		dataType: "json",
		contentType: 'application/json',
		headers: {
			"Token": appUser.Id
		},
		success: function (data) {
			var isEmpty = true;
			$.each(data, function (key, item) {
				isEmpty = false;
				$('<tr>', { id: item.Id }).addClass('data').appendTo($('#unassignedFaresBody'));
				$('<td>', { text: item.DateOfDrive }).addClass('data').appendTo($('#' + item.Id));
				$('<td>', { text: item.StartLocation }).addClass('data').appendTo($('#' + item.Id));
				$('<td>', { text: item.VehicleType }).addClass('data').appendTo($('#' + item.Id));
				$('<td>', { text: item.Customer }).addClass('data').appendTo($('#' + item.Id));
				$('<td>', { text: item.FinishLocation }).addClass('data').appendTo($('#' + item.Id));
				$('<td>', { text: item.Dispatcher }).addClass('data').appendTo($('#' + item.Id));
				$('<td>', { text: item.Driver }).addClass('data').appendTo($('#' + item.Id));
				$('<td>', { text: item.Price }).addClass('data').appendTo($('#' + item.Id));
				$('<td>', { text: item.StatusString }).addClass('data').appendTo($('#' + item.Id));
				$('<td>', { id: item.Id + 'c' }).addClass('data').appendTo($('#' + item.Id));
				$('<button>', { text: 'Take', id: item.Id + 'b' }).addClass('data').appendTo($('#' + item.Id + 'c'));
				$('#' + item.Id + 'b').bind('click', { id: item.Id }, function (event) {
					takeFare(event.data);
				});
			});

			if (isEmpty)
				$('#unassignedFares').css('visibility', 'collapse');
		},
		error: function (message) {
			alert(message.responseText);
		},
	});
}

function takeFare(data) {
	var url = '../api/driver/takefare/' + appUser.Id;
	var fareId = data.id;
	var dataObject = {
		'fareId': fareId
	};

	$.ajax({
		url: url,
		type: 'POST',
		data: JSON.stringify(dataObject),
		contentType: 'application/json',
		headers: {
			"Token": appUser.Id
		},
		success: function (result) {
			$('#content').load('../Html/DriverPage.html');
		},
		error: function (message) {
			alert(message.responseText);
		},
	});


}

function getDriverFares() {
	var url = '../api/driver/getdriverfares/' + appUser.Id;
	$('#fareContent').load('../Html/DriverFareDetail.html');
	$('#fareTableBody').empty();
	$.ajax({
		url: url,
		type: 'GET',
		dataType: "json",
		contentType: 'application/json',
		headers: {
			"Token": appUser.Id
		},
		success: function (data) {
			$.each(data, function (key, item) {
				populateRow('#driverFareDetail', 'driver', item);
			});

			
		},
		error: function (message) {
			alert(message.responseText);
		},
	});
}

function getCustomerFares() {
	var url = '../api/customer/getcustomerfares/' + appUser.Id;
	$('#fareContent').load('../Html/CustomerFareDetail.html');
	$('#fareTableBody').empty();
	$.ajax({
		url: url,
		type: 'GET',
		dataType: "json",
		contentType: 'application/json',
		headers: {
			"Token": appUser.Id
		},
		success: function (data) {
			$.each(data, function (key, item) {
				populateRow('#customerFareDetail', 'customer', item);
			});
		},
		error: function (message) {
			alert(message.responseText);
		},
	});
}

function populateRow(detailControl, userrole, item) {
	$('<tr>', { id: item.Id }).addClass('data').appendTo($('#fareTableBody'));
	$('#' + item.Id).bind('click', { control: detailControl, id: item.Id, role: userrole }, function (event) {
		getFareDetail(event.data);
	});
	$('<td>', { text: item.DateOfDrive }).addClass('data').appendTo($('#' + item.Id));
	$('<td>', { text: item.StartLocation }).addClass('data').appendTo($('#' + item.Id));
	$('<td>', { text: item.VehicleType }).addClass('data').appendTo($('#' + item.Id));
	$('<td>', { text: item.Customer }).addClass('data').appendTo($('#' + item.Id));
	$('<td>', { text: item.FinishLocation }).addClass('data').appendTo($('#' + item.Id));
	$('<td>', { text: item.Dispatcher }).addClass('data').appendTo($('#' + item.Id));
	$('<td>', { text: item.Driver }).addClass('data').appendTo($('#' + item.Id));
	$('<td>', { text: item.Price }).addClass('data').appendTo($('#' + item.Id));
	$('<td>', { text: item.StatusString }).addClass('data').appendTo($('#' + item.Id));
	$('<td>', { id: item.Id + 'commnent' }).addClass('data').appendTo($('#' + item.Id));
	if (item.CommentObject != null) {
		$('#' + item.Id + 'commnent')
		.append('<div> Date of comment: ' + item.CommentObject.DateOfPublish + '<br>')
		.append('Desctiption: ' + item.CommentObject.Description + '<br>')
		.append('Fare rating: ' + item.CommentObject.Mark + '<br></div>');
	}
}

function getUserDetails(data) {
	var id = data.id;
	var url = '../api/dispatcher/getuserdetails/' + id;

	$('#updateUserButton').bind('click', function (event) {
		updateUser(id);
	});

	getGenders('#genderUp');

	$.ajax({
		url: url,
		type: 'POST',
		dataType: "json",
		contentType: 'application/json',
		headers: {
			"Token": appUser.Id
		},
		success: function (data) {
			$('#userDetail').css('visibility', 'visible');

			let parsed = JSON.parse(data);
			$('#userUp').val(parsed.UserName);
			$('#passUp').val(parsed.Password);
			$('#firstNameUp').val(parsed.FirstName);
			$('#lastNameUp').val(parsed.LastName);
			$('#genderUp').val(parsed.GenderString);
			$('#jmbgUp').val(parsed.Idnumber);
			$('#phoneNumberUp').val(parsed.PhoneNumber);
			$('#emailAddressUp').val(parsed.EmailAddress);
			$('#blocked').prop('checked', parsed.Blocked);
		},
	});
}

function getFareCustomerStatuses(id) {
	var url = '../api/dispatcher/getfarecustomerstatuses';
	console.log(id);
	$.ajax({
		url: url,
		type: 'GET',
		dataType: "json",
		contentType: 'application/json',
		headers: {
			"Token": appUser.Id
		},
		success: function (result) {
			let res = JSON.parse(result);
			res.forEach(function (item) {
				$('<option>', { text: item }).appendTo($(id));
			});

		},
		error: function (message) {
			alert(message.responseText);
		},
	});
}

function fileterCustomerFares() {
	var url = '../api/customer/getfiltercustomerfares/' + appUser.Id;
	var status = $('#filterStatus').val();
	var dataObject = {
		'status': status
	}

	$('#fareContent').load('../Html/CustomerFareDetail.html');
	$('#fareTableBody').empty();
	$.ajax({
		url: url,
		type: 'POST',
		data: JSON.stringify(dataObject),
		contentType: 'application/json',
		headers: {
			"Token": appUser.Id
		},
		success: function (data) {
			$.each(data, function (key, item) {
				populateRow('#customerFareDetail', 'customer', item);
			});
		},
		error: function (message) {
			alert(message.responseText);
		},
	});
}

function fileterDriverFares() {
	var url = '../api/driver/getfilterdriverfares/' + appUser.Id;
	var status = $('#filterStatus').val();
	var dataObject = {
		'status': status
	}

	$('#fareContent').load('../Html/DriverFareDetail.html');
	$('#fareTableBody').empty();
	$.ajax({
		url: url,
		type: 'POST',
		data: JSON.stringify(dataObject),
		contentType: 'application/json',
		headers: {
			"Token": appUser.Id
		},
		success: function (data) {
			$.each(data, function (key, item) {
				populateRow('#driverFareDetail', 'driver', item);
			});
		},
		error: function (message) {
			alert(message.responseText);
		},
	});
}

function getFareStatuses(id, isCustomer) {
	var url = '../api/dispatcher/getfarestatuses';
	$.ajax({
		url: url,
		type: 'GET',
		dataType: "json",
		contentType: 'application/json',
		headers: {
			"Token": appUser.Id
		},
		success: function (result) {
			let res = JSON.parse(result);
			res.forEach(function (item) {
				if (isCustomer) {
					if (item == 'ON_HOLD' || item == 'CANCELED') {
						$('<option>', { text: item, disabled: false }).appendTo($(id));
					} else {
						$('<option>', { text: item, disabled: true }).appendTo($(id));
					}
				} else {
					$('<option>', { text: item }).appendTo($(id));
				}
			});

		},
		error: function (message) {
			alert(message.responseText);
		},
	});
}

function getGenders(id) {
	var url = '../api/user/getgenders';
	$.ajax({
		url: url,
		type: 'GET',
		dataType: "json",
		contentType: 'application/json',
		success: function (result) {
			let res = JSON.parse(result);
			res.forEach(function (item) {
				$('<option>', { text: item }).appendTo($(id));
			});

		},
		error: function (message) {
			alert(message.responseText);
		},
	});
}

function getVehicleTypes(id) {
	var url = '../api/dispatcher/getvehicletypes';

	$.ajax({
		url: url,
		type: 'GET',
		dataType: "json",
		contentType: 'application/json',
		headers: {
			"Token": appUser.Id
		},
		success: function (result) {

			let res = JSON.parse(result);
			res.forEach(function (item) {
				$('<option>', { text: item }).appendTo($(id));
			});
		},
		error: function (message) {
			alert(message.responseText);
		},
	});
}

function validateDriverFareInput() {
	var str = "";
	var isError = false;
	$("#fareStatus option:selected").each(function () {
		str = $(this).text();
		if (str == 'SUCCESFUL') {
			var locX = {
				text: $('#locX').val(),
				id: 'X'
			};
			var locY = {
				text: $('#locY').val(),
				id: 'Y'
			};
			var addrStreet = {
				text: $('#addrStreet').val(),
				id: 'Street'
			};
			var addrNumber = {
				text: $('#addrNumber').val(),
				id: 'Street number'
			}
			var addrCity = {
				text: $('#addrCity').val(),
				id: 'City'
			}
			var addrPostalCode = {
				text: $('#addrPostalCode').val(),
				id: 'Postal code'
			}

			var inputVal = new Array(locX, locY, addrStreet, addrNumber, addrCity, addrPostalCode);
			var message = "Field(s)";

			inputVal.forEach(function (item) {
				if (item.text == null || item.text == '') {
					isError = true;
					message += ' ' + item.id + ',';
				}
			});

			message += 'must be set.';

			var farePrice = {
				text: $('#farePrice').val(),
				id: 'Postal code'
			}
			if (farePrice <= 0) {
				message += 'Price must be positive.';
			}
			if (isError)
				alert(message);
		}
		if (str == 'FAILED') {
			var comment = $('#fareCommentColumn').text();
			if (comment == null || comment == '') {
				isError = true;
				alert('Comment cannot be empty');
			}
		}
	});

	return isError;
}

function updateDriverFare(id) {
	var url = '../api/driver/updatedriverfare/' + id;

	if (validateDriverFareInput()) {
		return;
	}

	var locX = $('#locX').val();
	var locY = $('#locY').val();
	var addrStreet = $('#addrStreet').val();
	var addrNumber = $('#addrNumber').val();
	var addrCity = $('#addrCity').val();
	var addrPostalCode = $('#addrPostalCode').val();
	var price = $('#farePrice').val();
	var status = $('#fareStatus').val();
	var comment = $('#fareComment').val();

	var dataObject = {
		'locX': locX,
		'locY': locY,
		'addrStreet': addrStreet,
		'addrNumber': addrNumber,
		'addrCity': addrCity,
		'addrPostalCode': addrPostalCode,
		'price': price,
		'status': status,
	};

	if (status == 'FAILED') {
		dataObject = {
			'status': status,
			'comment': comment
		};
		url = '../api/driver/updatedriverfarefailed/' + id;
	}



	$.ajax({
		url: url,
		type: 'POST',
		data: JSON.stringify(dataObject),
		contentType: 'application/json',
		headers: {
			"Token": appUser.Id
		},
		success: function (result) {
			$('#content').load('../Html/DriverPage.html');
		},
		error: function (message) {
			alert(message.responseText);
		},
	});
}

function updateDispatherFare(fareid) {
	var url = '../api/dispatcher/updatedispatcherfare/' + id;

	var status = $('#fareStatus').val();
	var driver = $('#fareDriverSelect').find(':selected').attr('id')

	var dataObject = {
		'status': status,
		'driver': driver,
		'fareid': fareid
	};

	$.ajax({
		url: url,
		type: 'POST',
		data: JSON.stringify(dataObject),
		contentType: 'application/json',
		headers: {
			"Token": appUser.Id
		},
		success: function (result) {

		},
		error: function (message) {
			alert(message.responseText);
		},
	});
}

function updateCustomerFare(id) {
	var url = '../api/customer/updatecustomerfare/' + id;

	var status = $('#fareStatus').val();
	var comment = $('#fareComment').val();
	var locX = $('#startlocX').val();
	var locY = $('#startlocY').val();
	var addrStreet = $('#startaddrStreet').val();
	var addrNumber = $('#startaddrNumber').val();
	var addrCity = $('#startaddrCity').val();
	var addrPostalCode = $('#startaddrPostalCode').val();
	var vehicleType = $('#fareVehicle').val();
	var date = $('#fareDetailDate').val();
	var dataObject = {
		'status': status,
		'comment': comment,
		'locX': locX,
		'locY': locY,
		'addrStreet': addrStreet,
		'addrNumber': addrNumber,
		'addrCity': addrCity,
		'addrPostalCode': addrPostalCode,
		'vehicleType': vehicleType,
		'date': date
	};
	debugger;
	if (status == null) {
		var rate = $('input[name=rating]:checked', '#fareRating').val();
		dataObject = {
			'comment': comment,
			'rate': rate
		}
		var url = '../api/customer/updatecustomerfarerate/' + id;
	}

	$.ajax({
		url: url,
		type: 'POST',
		data: JSON.stringify(dataObject),
		contentType: 'application/json',
		headers: {
			"Token": appUser.Id
		},
		success: function (result) {
			$('#content').load('../Html/CustomerPage.html');
		},
		error: function (message) {
			alert(message.responseText);
		},
	});
}

function getFreeVehicleDrivers(id) {
	var url = '../api/dispatcher/getfreedrivers';
	$.ajax({
		url: url,
		type: 'GET',
		dataType: "json",
		contentType: 'application/json',
		headers: {
			"Token": appUser.Id
		},
		success: function (result) {
			result.forEach(function (item) {
				$('<option>', { text: format(item.FirstName, item.LastName), id: item.Id }).appendTo($(id));
			});

		},
		error: function (message) {
			alert(message.responseText);
		},
	});
}

function addVehicle() {
	var url = '../api/dispatcher/addvehicle';

	var driver = $('#vehDriver').find(':selected').attr('id')
	var year = $('#vehYoP').val();
	var plate = $('#vehPlate').val();
	var taxiId = $('#vehIdentifier').val();
	var type = $('#vehType').val();

	var dataObject = {
		'driver': driver,
		'year': year,
		'plate': plate,
		'taxiId': taxiId,
		'type': type
	};

	$.ajax({
		url: url,
		type: 'POST',
		data: JSON.stringify(dataObject),
		contentType: 'application/json',
		headers: {
			"Token": appUser.Id
		},
		success: function (result) {

		},
		error: function (message) {
			alert(message.responseText);
		},
	});
}

function createDispatcherFare() {
	var uri = '../api/dispatcher/createfare/' + appUser.Id;

	var locX = $('#dlocX').val();
	var locY = $('#dlocY').val();
	var addrStreet = $('#daddrStreet').val();
	var addrNumber = $('#daddrNumber').val();
	var addrCity = $('#daddrCity').val();
	var addrPostalCode = $('#daddrPostalCode').val();
	var vehicleType = $('#fareCreateVehicle').val();
	var driver = $('#fareCreateDriver').find('option:selected').attr('id');
	var date = $('#fareDate').val();
	var dataObject = {
		'locX': locX,
		'locY': locY,
		'addrStreet': addrStreet,
		'addrNumber': addrNumber,
		'addrCity': addrCity,
		'addrPostalCode': addrPostalCode,
		'vehicleType': vehicleType,
		'driver': driver,
		'date': date
	};

	$.ajax({
		url: uri,
		type: 'POST',
		data: JSON.stringify(dataObject),
		contentType: 'application/json',
		headers: {
			"Token": appUser.Id
		},
		success: function (result) {
			$('#panelContent').load('../Html/AllFaresPage.html')
		},
		error: function (message) {
			alert(message.responseText);
		},
	});

}

function createFare() {
	var uri = '../api/customer/createfare/' + appUser.Id;

	var locX = $('#locX').val();
	var locY = $('#locY').val();
	var addrStreet = $('#addrStreet').val();
	var addrNumber = $('#addrNumber').val();
	var addrCity = $('#addrCity').val();
	var addrPostalCode = $('#addrPostalCode').val();
	var vehicleType = $('#customerVehType').val();
	var date = $('#fareDate').val();
	var dataObject = {
		'locX': locX,
		'locY': locY,
		'addrStreet': addrStreet,
		'addrNumber': addrNumber,
		'addrCity': addrCity,
		'addrPostalCode': addrPostalCode,
		'vehicleType': vehicleType,
		'date': date
	};
	$.ajax({
		url: uri,
		type: 'POST',
		data: JSON.stringify(dataObject),
		contentType: 'application/json',
		headers: {
			"Token": appUser.Id
		},
		success: function (result) {

		},
		error: function (message) {
			alert(message.responseText);
		},
	});
}