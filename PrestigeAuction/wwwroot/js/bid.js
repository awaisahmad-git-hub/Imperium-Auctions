
function PlaceBid(productId, bidValue) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        background: "#fff",
        color: "dark",
        showCancelButton: true,
        confirmButtonColor: "#DC143C",
        cancelButtonColor: "#2aa198",
        confirmButtonText: "Yes, do it!"
    }).then((result) => {
        if (result.isConfirmed) {
            axios.get('/User/Bid/PlaceBid', { params: { productId: productId, bidValue: bidValue } })
                .then(response => {
                    toastr.options = {
                        "positionClass": "toast-top-center",
                        "timeOut": "1000",
                    }
                    toastr.success(response.data.message);
                    document.getElementById('bidInput').value = "";
                }).catch(error => {
                    console.log(error);
                    document.getElementById('validationMessage').textContent = error.response.data.message;
                });
        }
    });
}
function CountDownTarget(startTargetDate, endTargetDate, productId) {

    axios.get('/User/Bid/CountDownTargetTime', { params: { startTargetDate: startTargetDate, endTargetDate: endTargetDate, productId: productId } })
        .then(response => {

            //location.reload();
        }).catch(error => {
            console.log(error);
            alert('There was an error placing the time:', error);
        });
}
function AuctionEndNotification(productId, userId) {
    if (productId && userId) {
        axios.post('/User/Bid/Notification', { productId: productId, userId: userId })
            .then(response => {

                //location.reload();
            }).catch(error => {
                console.log(error);
                alert('There was an error placing the time:', error);
            });
    }
}
