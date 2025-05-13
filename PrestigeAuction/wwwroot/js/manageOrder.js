const manageOrderTbody = document.getElementById('manageOrderTbody');
if (manageOrderTbody) { 
    manageOrderTbody.addEventListener('click', function (e) {
        if (e.target.classList.contains('statusShipped')) {
            const orderId = e.target.dataset.orderId;
            const manageOrderTbody = document.getElementById('manageOrderTbody');
            axios.post('/Admin/ManageOrder/StatusShipped', { orderId: orderId })
                .then(response => {
                    manageOrderTbody.innerHTML = response.data;  
                    toastr.options = {
                        "positionClass": "toast-top-center",
                        "timeOut": "1000",
                    }
                    toastr.success('Status Changed Successfully');
                }).catch(error => {
                    console.log(error);
                    alert('There was an error:', error);
                });
        }
        else if (e.target.classList.contains('statusDelivered')) {
                const orderId = e.target.dataset.orderId;
                axios.post('/Admin/ManageOrder/StatusDelivered', { orderId: orderId })
                    .then(response => {
                        manageOrderTbody.innerHTML = response.data;   
                        toastr.options = {
                            "positionClass": "toast-top-center",
                            "timeOut": "1000",
                        }
                        toastr.success('Status Changed Successfully');
                    }).catch(error => {
                        console.log(error);
                        alert('There was an error:', error);
                    });
            }
    });
}
