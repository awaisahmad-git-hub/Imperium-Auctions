
function PlaceBid(productId, bidValue) {
    //const placeBidForm = document.getElementById('placeBidForm');
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        background: "#002b36",
        color: "white",
        showCancelButton: true,
        confirmButtonColor: "#d33",
        cancelButtonColor: "#2aa198",
        confirmButtonText: "Yes, do it!"
    }).then((result) => {
        if (result.isConfirmed) {
            axios.get('/User/Bid/PlaceBid', { params: { productId: productId, bidValue: bidValue } } )
                .then(response => {
                    toastr.options = {
                        "positionClass": "toast-top-center",
                        "timeOut": "1000",
                    }
                    toastr.success(response.data.message);
                    //currentBid.innerHTML = response.data;
                    //placeBidForm.submit();  
                    //location.reload();                   
                }).catch(error => {
                    console.log(error);
                    alert('There was an error placing the bid:', error);
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
