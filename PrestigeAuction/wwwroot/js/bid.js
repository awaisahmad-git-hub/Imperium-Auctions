function PlaceBid(productId, bidValue) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        background: "#fdf6e3",
        color: "dark",
        showCancelButton: true,
        confirmButtonColor: "#DC143C",
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
