// AJAX for author Add can go in here!
// This file is connected to the project via Shared/_Layout.cshtml

function AddAuthor() {

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