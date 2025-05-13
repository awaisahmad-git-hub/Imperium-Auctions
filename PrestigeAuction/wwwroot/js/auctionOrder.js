const saveChanges = document.getElementById('save-changes');
if (saveChanges) { 
    saveChanges.addEventListener('click', function () {
        const sdName = document.getElementById('sd-name').value;
        const sdPhoneNumber = document.getElementById('sd-phone-number').value;
        const sdShippingAddress = document.getElementById('sd-shipping-address').value;
        const sdPostalCode = document.getElementById('sd-postal-code').value;


        axios.post('/User/AuctionOrder/SaveChanges', { sdName: sdName, sdPhoneNumber: sdPhoneNumber, sdShippingAddress: sdShippingAddress, sdPostalCode: sdPostalCode })
            .then(response => {
                toastr.options = {
                    "positionClass": "toast-top-center",
                    "timeOut": "1000",
                }
                toastr.success(response.data.message);
            }).catch(error => {
                console.log(error);
                alert('There was an error:', error);
            });
    });
}
