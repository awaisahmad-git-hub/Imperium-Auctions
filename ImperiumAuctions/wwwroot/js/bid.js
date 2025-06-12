
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
            axios.post('/User/Bid/PlaceBid', { productId: productId, bidValue: bidValue })
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

    axios.post('/User/Bid/CountDownTargetTime', { startTargetDate: startTargetDate, endTargetDate: endTargetDate, productId: productId })
        .then(response => {

            //location.reload();
        }).catch(error => {
            console.log(error);
            alert('There was an error placing the time:', error);
        });
}
function AuctionEndNotification(productId) {
    if (productId) {
        axios.post('/User/Bid/AuctionEndNotification', { productId: productId })
            .then(response => {
                if (response.status !== 204) {
                    if (response.data.isWinner) {
                        document.getElementById('notifyUser').innerHTML = `
                                                                                   <hr class="text-dark" />
                                                                                   <div class="ps-2">
                                                                                       <a href="/user/auction-order/order-summary/${response.data.bidId}" class="btn bg-gradient-right btn-hover">Proceed to Payment</a>
                                                                                   </div>
                                                                                   <div class="alert alert-success alert-dismissible mt-2 ms-2" role="alert" style="text-align:justify">
                                                                                       <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                                                                                       ${response.data.message}
                                                                                   </div>`;
                    } else {
                        document.getElementById('notifyUser').innerHTML = `
                                                                                   <hr class="text-dark" />
                                                                                   <div class="alert alert-dismissible mt-2 ms-2" style="background-color:#DC143C;text-align:justify" role="alert">
                                                                                       <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                                                                                       ${response.data.message}
                                                                                   </div>`;
                    }
                }
            }).catch(error => {
                console.log(error);
                alert('There was an error:', error);
            });
    }
}
