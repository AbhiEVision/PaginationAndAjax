$(document).ready(function () {

	// fetch next records in mention pages
	$("#after").on("click", () => {
		const pageNo = $("#PageNo").val();
		const pageSize = $("#PageSize").val();
		$.ajax({
			url: '/Employee/GiveMeData',
			type: 'GET',
			data: {
				pageNumber: pageNo,
				pageSize: pageSize
			},
			//dataType: 'json',
			success: function (data) {
				console.log(data)
				if (Object.keys(data).length === 0) {
					$("#insert").html("<tr class='bg-danger text-white text-center'><td colspan='5'> we have noting to Show!<td></tr>")
				}
				else {
					$("#insert").text("")
					$.each(data, function (index, item) {
						
						$('#insert').append('<tr><td class="col">' + item.Id +
							'</td><td class="col">' + item.Name +
							'</td><td class="col">' + item.email +
							'</td><td class="col">' + item.UserName +
							'</td><td class="col">' + item.DepId +
							'</td><td class="col">' + item.Department_Id +
							'</td></tr>');

					})
				}
				const page = parseInt($("#PageNo").val());
				$("#PageNo").val(page + 1);
			},
			error: function (jqXHR, textStatus, errorThrown) {
				console.error('Error:', textStatus, errorThrown);
				console.log("test")
			}
		})
	});

	// fetch records in mention pages
	$("#before").on("click", () => {
		const pageNo = $("#PageNo").val();
		const pageSize = $("#PageSize").val();
		$.ajax({
			url: '/Employee/GiveMeData',
			type: 'GET',
			data: {
				pageNumber: pageNo,
				pageSize: pageSize
			},
			//dataType: 'json',
			success: function (data) {

				if (Object.keys(data).length === 0) {
					$("#insert").html("<tr class='bg-danger text-white text-center'><td colspan='5'> we have noting to Show!<td></tr>")
				}
				else {
					$("#insert").text("")
					$.each(data, function (index, item) {

						$('#insert').append('<tr><td class="col">' + item.Id +
							'</td><td class="col">' + item.Name +
							'</td><td class="col">' + item.email +
							'</td><td class="col">' + item.UserName +
							'</td><td class="col">' + item.DepId +
							'</td><td class="col">' + item.Department_Id +
							'</td></tr>'
						);

					});
				};
				const page = parseInt($("#PageNo").val());
				$("#PageNo").val(page - 1);
			},
			error: function (jqXHR, textStatus, errorThrown) {
				console.error('Error:', textStatus, errorThrown);
			}
		});
	});
});