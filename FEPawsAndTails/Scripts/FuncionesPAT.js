const carouselIndices = {};

/*Carrousel Productos */
function moveCarousel(id, direction) {
    const carousel = document.getElementById(`carousel-${id}`);
    const images = carousel.querySelectorAll('.carousel-image');
    const total = images.length;

    // Init index for this carousel if needed
    if (!(id in carouselIndices)) carouselIndices[id] = 0;

    carouselIndices[id] += direction;

    if (carouselIndices[id] < 0) carouselIndices[id] = total - 1;
    if (carouselIndices[id] >= total) carouselIndices[id] = 0;

    const offset = carouselIndices[id] * images[0].clientWidth;
    carousel.style.transform = `translateX(-${offset}px)`;
}
/*Funcion Añadir al carrito*/
function addToCart(id, name, price, urlimg) {
    const qtyInput = document.getElementById(`qty-${id}`);
    const quantity = parseInt(qtyInput.value);
    const qtyStock = document.getElementById(`stock-${id}`);
    const stock = parseInt(qtyStock.textContent.trim());
    

    if (isNaN(quantity) || quantity < 1) {
        alert("Por favor ingresa una cantidad válida.");
        return;
    }
    if(quantity > stock){
        alert("No se pudo agregar al carrito. Cantidad seleccionada mayor al stock disponible.");
        return;
    }

    const cartItem = {
        idProducto: id,
        nombre: name,
        precio: price,
        cantidad: quantity,
        imagen: urlimg
    };

    // Obtener carrito existente o crear uno nuevo
    let carrito = JSON.parse(localStorage.getItem("carrito")) || [];

    // Verificar si el producto ya existe
    const index = carrito.findIndex(p => p.idProducto === id);
    if (index !== -1) {
        carrito[index].cantidad += quantity;
    } else {
        carrito.push(cartItem);
    }

    localStorage.setItem("carrito", JSON.stringify(carrito));
    alert(`"${name}" agregado al carrito (${quantity}).`);
}
/*Funcion calcular totales mi carrito */
document.addEventListener("DOMContentLoaded", function () {
    const carrito = JSON.parse(localStorage.getItem("carrito")) || [];
    const container = document.getElementById("carrito-container");
    const summary = document.getElementById("carrito-summary");
    const subtotalEl = document.getElementById("subtotal");
    const ivaEl = document.getElementById("iva");
    const totalEl = document.getElementById("total");

    if (carrito.length === 0) {
        container.innerHTML = "<p style='text-align:center;'>Tu carrito está vacío.</p>";
        return;
    }

    let subtotal = 0;

    carrito.forEach((p, index) => {
        const item = document.createElement("div");
        item.className = "carrito-item";

        // Asigna imagen o placeholder
        const imagen = document.createElement("img");
        imagen.src = p.imagen || "/images/default-placeholder.png";
        item.appendChild(imagen);

        const info = document.createElement("div");
        info.className = "item-info";
        info.innerHTML = `<h4>${p.nombre}</h4><small>Cantidad: ${p.cantidad}</small>`;
        item.appendChild(info);

        const precio = document.createElement("div");
        precio.className = "item-precio";
        const totalItem = p.precio * p.cantidad;
        precio.textContent = `$${totalItem.toFixed(2)}`;
        item.appendChild(precio);

        // Botón para eliminar del carrito
        const botonEliminar = document.createElement("button");
        botonEliminar.className = "btn-eliminar";
        botonEliminar.textContent = "❌ Quitar";
        botonEliminar.onclick = () => eliminarDelCarrito(index);
        item.appendChild(botonEliminar);

        container.appendChild(item);
        subtotal += totalItem;
    });

    const iva = subtotal * 0.15;
    const total = subtotal + iva;

    subtotalEl.textContent = `$${subtotal.toFixed(2)}`;
    ivaEl.textContent = `$${iva.toFixed(2)}`;
    totalEl.textContent = `$${total.toFixed(2)}`;
    summary.style.display = "block";
});

function eliminarDelCarrito(index) {
    const carrito = JSON.parse(localStorage.getItem("carrito")) || [];
    carrito.splice(index, 1);
    localStorage.setItem("carrito", JSON.stringify(carrito));
    alert("Producto eliminado");
    window.location.reload(true);
}

/*Funcion para Inicio de Sesion*/
/*Verificar que haya una sesion iniciada */
function validarSesionAntesDePagar() {
    const cuenta = JSON.parse(localStorage.getItem("cuenta"));

    if (!cuenta || !cuenta.id) {
        alert("Debes iniciar sesión para finalizar la compra.");
        window.location.href = "/Login";
        return;
    }
    finalizarCompra();
    
    // Aquí podrías continuar con el proceso de pago real, enviar datos, etc.
}
function finalizarCompra() {
    const carrito = JSON.parse(localStorage.getItem("carrito")) || [];
    const cuenta = JSON.parse(localStorage.getItem("cuenta"));

    if (!cuenta) {
        alert("⚠️ Debes iniciar sesión para realizar la compra.");
        return;
    }

    if (carrito.length === 0) {
        alert("⚠️ Tu carrito está vacío.");
        return;
    }

    const payload = {
        detalles: carrito.map(p => ({
            idProducto: p.idProducto,
            cantidad: p.cantidad
        })),
        direccion: document.getElementById("direccion").value,
        metodoPago: document.getElementById("metodoPago").value,
        idUsuario: cuenta.id
    };

    fetch('/Carrito/CrearFacturaConDetalle', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(payload)
    })
        .then(res => res.json())
        .then(data => {
            if (data.success) {
                alert("✅ Compra realizada con éxito");
                localStorage.removeItem("carrito");
                window.location.href = "/"; // Redirige a inicio o gracias
            } else {
                alert("❌ Error al procesar la compra");
                console.error(data.error);
            }
        })
        .catch(err => {
            console.error("Error en fetch:", err);
            alert("❌ Error de conexión");
        });
}

/*Iniciar Sesion*/
function iniciarSesion(event) {
    event.preventDefault();

    const email = document.getElementById("email").value.trim();
    const password = document.getElementById("password").value;

    if (!email || !password) {
        alert("Por favor completa todos los campos.");
        return false;
    }

    fetch(`/Login/ValidarCliente?correo=${encodeURIComponent(email)}&password=${encodeURIComponent(password)}`)
        .then(res => res.json())
        .then(data => {
            if (data.success) {
                console.log("Bienvenido:", data.usuario.nombre);

                
                localStorage.setItem("cuenta", JSON.stringify(data.usuario));               
                alert("Bienvenido!");
                window.location.href = "/Home";
            } else {
                alert("Combinación de usuario y contraseña inválido. Vuelva a intentar.");
            }
        })
        .catch(err => {
            console.error("Error en la autenticación:", err);
            alert("Ocurrió un error al conectar con el servidor.");
        });


    localStorage.setItem("cuenta", JSON.stringify(cuenta));
    alert("¡Bienvenido, " + cuenta.nombre + "!");
    window.location.href = "/"; // Redirige al home u otra página

    return false;
}

