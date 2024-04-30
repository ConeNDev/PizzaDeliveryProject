let apiURL = "https://forkify-api.herokuapp.com/api/v2/recipes";
let apiKey = "6ea9cc64-7f18-4d42-860a-4b4b3e23b172";

async function getOrderFromMenu(id,showId) {
    let resp = await fetch(`${apiURL}/${id}?key=${apiKey}`);
    let result = await resp.json();
    let recipe = result.data.recipe;
    showOrderedPizzaDetails(recipe, showId);
}
function showOrderedPizzaDetails(orderedPizzaDetails, showId) {
    $.ajax({
        dataType: 'html',
        type: 'POST',
        url: '/Menu/ShowOrder',
        data: orderedPizzaDetails,
        success: function (htmlResult) {
            $('#' + showId).html(htmlResult);
        }
    })
}

//order page

function quantity(option) {
    //qty uzima vrednost onog inputa gde je id=qty (jquery selektor)
    let qty = $('#qty').val();
    let price = parseInt($('#price').val());
    let totalAmount = 0;
    if (option === 'inc') {
        qty = parseInt(qty) + 1;
    }
    else {
        qty = qty == 1 ? qty : qty - 1;
    }
    totalAmount = price * qty;
    //jquery koji sada taj input azurira sa ovu vrednost tj ovu premnljivu qty
    $('#qty').val(qty);
    $('#totalAmount').val(totalAmount);
}


//ajax(klijentska strana) salje zahtev serverui tada server zove kontrolera(server side)
//tojest akciju GetCartList u cart kontroleru
function getCartList(){
    $.ajax({
        url: '/Cart/GetCartList',
        type: 'GET',
        dataType: 'html',
        success: function (result) {
            $('#showCartList').html(result);
        },
        error: function (err) {
            console.log(err);
        }
    })
}

function addToCart() {
    if (!isUserAuthenticated) {
        window.location.href = '/Account/Login'; 
        return;
    }

    // Nastavak funkcije ako je korisnik ulogovan
    var pizzaId = $("input[name='PizzaId']").val();
    var pizzaName = $("input[name='PizzaName']").val();
    var userId = $("input[name='UserId']").val();
    var quantity = $("#qty").val();
    var price = parseInt($("#price").val(), 10);
    var address = $("input[name='Address']").val();
    var totalAmount = parseInt($("#totalAmount").val(), 10);
    var imageUrl = $("#image-url").val();

    $.ajax({
        url: '/Cart/AddToCart',
        type: 'POST',
        data: {
       
            PizzaId: pizzaId,
            PizzaName: pizzaName,
            UserId: userId,
            Quantity: quantity,
            Price: price,
            Address: address,
            TotalAmount: totalAmount,
            ImageUrl: imageUrl
        },
        success: function (response) {
            // U slučaju uspešnog odgovora, preusmeravamo korisnika na stranicu korpe
            window.location.href = '/Cart/Index';
        },
        error: function (error) {
            console.log(error);
            // Obrada grešaka
        }
    });
}
function removeFromCart(orderId) {
    fetch('/Cart/RemoveFromCart', {
        method: 'POST', // ili 'GET', zavisno od toga kako je konfigurisan vaš backend
        headers: {
            'Content-Type': 'application/json',
            // Dodajte dodatne zaglavlja ako je potrebno
        },
        body: JSON.stringify({ orderId: orderId })
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                // Ako je uspešno uklonjeno, ažurirajte prikaz korpe
                // Možete ili ukloniti element iz DOM-a ili ponovo učitati deo stranice gde je korpa
                console.log('Item removed');
                location.reload(); // Ovo će ponovo učitati stranicu, možda želite finije rešenje
            } else {
                console.error('Failed to remove item');
            }
        })
        .catch(error => console.error('Error:', error));

}

function orderAll() {
    $.ajax({
        url: '/Cart/OrderAll',
        type: 'POST',
        success: function (data) {
            alert('Order placed successfully!');
            window.location.reload(); // Ažuriranje stranice nakon uspešnog naručivanja
        },
        error: function () {
            alert('Error placing order.');
        }
    });
}