
const homeCategoryMenu = document.getElementById('homeCategoryMenu');
if (homeCategoryMenu) {
    homeCategoryMenu.addEventListener('click', function (e) {
        if (e.target.classList.contains('homeCategoryMenuItem')) {
            var items = document.getElementsByClassName("homeCategoryMenuItem");
            for (var i = 0; i < items.length; i++) {
                items[i].classList.remove("active");
            }
            if (e.target.innerText!=="All") {
                e.target.classList.add("active");
            }
            const searchString = e.target.dataset.category;
            const homeProducts = document.getElementById('homeProducts');
            axios.get('/User/Home/SearchHomeProductByCategory', { params: { searchString: searchString } })
                .then(response => {
                    homeProducts.innerHTML = response.data;
                });
        }
    });
}
/* search product by name in home page */
const homeSearchInput = document.getElementById('homeSearchButton');
if (homeSearchInput) {
    homeSearchInput.addEventListener('click', function () {
        const searchString = document.getElementById('homeSearchInput').value;
        axios.get('/User/Home/SearchHomeProductByName', { params: { searchString: searchString } }).then(response => {
            homeProducts.innerHTML = response.data;
        }).catch(error => {
            alert('There was an error in searching the product:', error);
        });
    });
}