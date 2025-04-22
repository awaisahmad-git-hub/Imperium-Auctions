
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
            axios.get('/User/Home/SearchHomeProduct', { params: { searchString: searchString } })
                .then(response => {
                    homeProducts.innerHTML = response.data;
                });
        }
    });
}
