/* delete product */
const tbody = document.getElementById('productTbody');
if (tbody) {
    tbody.addEventListener('click', function (e) {
        if (e.target.closest('.deleteProduct')) {
            const id = parseInt(e.target.dataset.id);
            const productTbody = document.getElementById('productTbody');
            Swal.fire({
                title: "Are you sure?",
                text: "You won't be able to revert this!",
                icon: "warning",
                background: "#fdf6e3",
                color: "dark",
                showCancelButton: true,
                confirmButtonColor: "#DC143C",
                cancelButtonColor: "#2aa198",
                confirmButtonText: "Yes, delete it!"
            }).then((result) => {
                if (result.isConfirmed) {
                    axios.delete(`/Admin/Product/Delete/${id}`) // we can also use this, '/Admin/Product/Delete', {params:{id:id}} using this, the url becomes '/Admin/Product/Delete?id=123&another=xyz'
                        .then(response => {                     // we write this `/Admin/Product/Delete/${id}` in backticks (`), JavaScript template literals only work inside backticks(`), not single quotes.
                            productTbody.innerHTML = response.data; /* window.location.href = response.data.redirectUrl; */    // window.location.href = '/Admin/Product/Index';
                            toastr.options = {
                                "positionClass": "toast-top-center",
                                "timeOut": "1000",
                            }
                            toastr.success('Deleted Successfully');
                        }).catch(error => {
                            alert('There was an error deleting the product:', error);
                        });
                }
            });
        }
    });
}

/* delete single image from CreateUpdate page */
function DeleteImage(id) {
    const productImages = document.getElementById('productImages');
    axios.delete('/Admin/Product/DeleteImage', { params: { id: id } })
        .then(response => {
            productImages.innerHTML = response.data;
            toastr.options = {
                "positionClass": "toast-top-center",
                "timeOut": "1000",
            }
            toastr.success('Deleted Successfully');
        }).catch(error => {
            alert('There was an error deleting the image:', error);
        });
}
/* search product by category */
const searchInput = document.getElementById('searchInput');
if (searchInput) {
    searchInput.addEventListener('input', function () {
        const productTbody = document.getElementById('productTbody');
        const searchString = this.value;
        axios.get('/Admin/Product/SearchProduct', { params: { searchString: searchString } })
            .then(response => {
                productTbody.innerHTML = response.data;
            });
    });
}

/* CreateUpdate page's category menu */
const productCategoryMenu = document.getElementById('productCategoryMenu');
if (productCategoryMenu) {
    productCategoryMenu.addEventListener('click', function (e) {
        if (e.target.classList.contains('productCategoryMenuItem')) {
            const _categoryText = e.target.dataset.categoryText;
            const _categoryValue = e.target.dataset.categoryValue;
            document.getElementById('category-button').textContent = _categoryText;
            document.getElementById('hiddenCategory').value = _categoryValue;
        }
    });
}
