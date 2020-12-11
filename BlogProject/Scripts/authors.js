// AJAX for author Add can go in here!
// This file is connected to the project via Shared/_Layout.cshtml

// This function attaches a timer object to the input window.
// When the timer expires (300ms), the search executes.
// Prevents a search on each key up for fast typers.
function _ListAuthors(d) {

	if (d.timer) clearTimeout(d.timer);
	d.timer = setTimeout(function () { ListAuthors(d.value); }, 300);
}

//The actual List Authors Method.
function ListAuthors(SearchKey) {

	var URL = "http://localhost:51326/api/AuthorData/ListAuthors/"+SearchKey;

	var rq = new XMLHttpRequest();
	rq.open("GET", URL, true);
	rq.setRequestHeader("Content-Type", "application/json");
	rq.onreadystatechange = function () {
		//ready state should be 4 AND status should be 200
		if (rq.readyState == 4 && rq.status == 200) {
			//request is successful and the request is finished
			

			var authors = JSON.parse(rq.responseText)
			var listauthors = document.getElementById("listauthors");
			listauthors.innerHTML = "";

			//renders content for each author pulled from the API call
			for (var i = 0; i < authors.length; i++)
			{
				var row = document.createElement("div");
				row.classList = "listitem row";
				var col = document.createElement("col");
				col.classList = "col-md-12";
				var link = document.createElement("a");
				link.href = "/Author/Show/" + authors[i].AuthorId;
				link.innerHTML = authors[i].AuthorFname + " " + authors[i].AuthorLname;

				col.appendChild(link);
				row.appendChild(col);
				listauthors.appendChild(row);

            }
		}

	}
	//POST information sent through the .send() method
	rq.send();
}

// Usually Validation functions for Add and Update are separated.
// You can run into situations where information added is no longer updated, or vice versa
// However, as an example, validation is consolidated into 'ValidateAuthor'
// This is so that both Ajax and Non Ajax techniques can utilize the same client-side validation logic.
function ValidateAuthor() {

	var IsValid = true;
	var ErrorMsg = "";
	var ErrorBox = document.getElementById("ErrorBox");
	var AuthorFname = document.getElementById('AuthorFname').value;
	var AuthorLname = document.getElementById('AuthorLname').value;
	var AuthorEmail = document.getElementById('AuthorEmail').value;
	var AuthorBio = document.getElementById('AuthorBio').value;

	//First Name is two or more characters
	if (AuthorFname.length < 2) {
		IsValid = false;
		ErrorMsg += "First Name Must be 2 or more characters.<br>";
	}
	//Last Name is two or more characters
	if (AuthorLname.length < 2) {
		IsValid = false;
		ErrorMsg += "Last Name Must be 2 or more characters.<br>";
	}
	//Email is valid pattern
	if (!ValidateEmail(AuthorEmail)) {
		IsValid = false;
		ErrorMsg += "Please Enter a valid Email.<br>";
	}

	if (!IsValid) {
		ErrorBox.style.display = "block";
		ErrorBox.innerHTML = ErrorMsg;
	}else {
		ErrorBox.style.display = "none";
		ErrorBox.innerHTML = "";
    }


	return IsValid;
}

function AddAuthor() {

	//check for validation straight away
	var IsValid = ValidateAuthor();
	if (!IsValid) return;

	//goal: send a request which looks like this:
	//POST : http://localhost:51326/api/AuthorData/AddAuthor
	//with POST data of authorname, bio, email, etc.

	var URL = "http://localhost:51326/api/AuthorData/AddAuthor/";

	var rq = new XMLHttpRequest();
	// where is this request sent to?
	// is the method GET or POST?
	// what should we do with the response?

	var AuthorFname = document.getElementById('AuthorFname').value;
	var AuthorLname = document.getElementById('AuthorLname').value;
	var AuthorEmail = document.getElementById('AuthorEmail').value;
	var AuthorBio = document.getElementById('AuthorBio').value;

	var AuthorData = {
		"AuthorFname": AuthorFname,
		"AuthorLname": AuthorLname,
		"AuthorEmail": AuthorEmail,
		"AuthorBio": AuthorBio
	};


	rq.open("POST", URL, true);
	rq.setRequestHeader("Content-Type", "application/json");
	rq.onreadystatechange = function () {
		//ready state should be 4 AND status should be 200
		if (rq.readyState == 4 && rq.status == 200) {
			//request is successful and the request is finished

			//nothing to render, the method returns nothing.
			

		}

	}
	//POST information sent through the .send() method
	rq.send(JSON.stringify(AuthorData));

}



function UpdateAuthor(AuthorId) {

	//check for validation straight away
	var IsValid = ValidateAuthor();
	if (!IsValid) return;

	//goal: send a request which looks like this:
	//POST : http://localhost:51326/api/AuthorData/UpdateAuthor/{id}
	//with POST data of authorname, bio, email, etc.

	var URL = "http://localhost:51326/api/AuthorData/UpdateAuthor/"+AuthorId;

	var rq = new XMLHttpRequest();
	// where is this request sent to?
	// is the method GET or POST?
	// what should we do with the response?

	var AuthorFname = document.getElementById('AuthorFname').value;
	var AuthorLname = document.getElementById('AuthorLname').value;
	var AuthorEmail = document.getElementById('AuthorEmail').value;
	var AuthorBio = document.getElementById('AuthorBio').value;



	var AuthorData = {
		"AuthorFname": AuthorFname,
		"AuthorLname": AuthorLname,
		"AuthorEmail": AuthorEmail,
		"AuthorBio": AuthorBio
	};


	rq.open("POST", URL, true);
	rq.setRequestHeader("Content-Type", "application/json");
	rq.onreadystatechange = function () {
		//ready state should be 4 AND status should be 200
		if (rq.readyState == 4 && rq.status == 200) {
			//request is successful and the request is finished

			//nothing to render, the method returns nothing.
			

		}

	}
	//POST information sent through the .send() method
	rq.send(JSON.stringify(AuthorData));

}


//Helper function from : https://stackoverflow.com/questions/46155/how-to-validate-an-email-address-in-javascript
function ValidateEmail(email) {
	const re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
	return re.test(String(email).toLowerCase());
}