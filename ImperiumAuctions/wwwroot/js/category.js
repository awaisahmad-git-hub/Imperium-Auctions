/* delete category */
const categoryTbody = document.getElementById('categoryTbody');
if (categoryTbody) {
    categoryTbody.addEventListener('click', function (e) {
        if (e.target.closest('.deleteCategory')) {
            const id = parseInt(e.target.dataset.categoryId);
            const categoryTbody = document.getElementById('categoryTbody');
            Swal.fire({
                title: "Are you sure?",
                text: "This will also delete related products, and you won't be able to revert this!",
                icon: "warning",
                background: "#fff",
                color: "dark",
                showCancelButton: true,
                confirmButtonColor: "#DC143C",
                cancelButtonColor: "#2aa198",
                confirmButtonText: "Yes, delete it!",
                
            }).then((result) => {
                if (result.isConfirmed) {
                    axios.delete(`/Admin/Category/DeleteCategory/${id}`) 
                        .then(response => {                     
                            categoryTbody.innerHTML = response.data;
                            toastr.options = {
                                "positionClass": "toast-top-center",
                                "timeOut": "1000",
                            }
                            toastr.success('Deleted Successfully');
                        }).catch(error => {
                            alert('There was an error in deleting the category:', error);
                        });
                }
            });
        }
    });
}
/* search category by category name */
const categorySearchInput = document.getElementById('categorySearchInput');
if (categorySearchInput) {
    categorySearchInput.addEventListener('input', function () {
        const categoryTbody = document.getElementById('categoryTbody');
        const searchString = this.value;
        axios.get('/Admin/Category/SearchCategory', { params: { searchString: searchString } })
            .then(response => {
                categoryTbody.innerHTML = response.data;
            }).catch(error => {
                alert('There was an error in searching the product:', error);
            });
    });
}