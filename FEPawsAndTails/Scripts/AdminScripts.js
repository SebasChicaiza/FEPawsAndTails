document.addEventListener("DOMContentLoaded", function () {
    fetch('/Admin/ObtenerProductos') // Este método llama al SOAP internamente
        .then(r => r.json())
        .then(data => {
            const tbody = document.querySelector("#tabla-productos tbody");
            tbody.innerHTML = "";

            data.forEach(p => {
                tbody.innerHTML += `
                        <tr>
                            <td>${p.ID_PRODUCTO}</td>
                            <td>${p.PROD_NOMBRE}</td>
                            <td>$${p.PROD_PRECIO.toFixed(2)}</td>
                            <td>${p.PROD_STOCK}</td>
                            <td>
                                <button onclick="editarProducto(${p.ID_PRODUCTO})">✏️ Editar</button>
                                <button onclick="eliminarProducto(${p.ID_PRODUCTO})">🗑️ Eliminar</button>
                            </td>
                        </tr>`;
            });
        });
});

function editarProducto(id) {
    alert("Funcionalidad de editar para ID " + id + " (a implementar)");
}

function eliminarProducto(id) {
    if (confirm("¿Estás seguro de eliminar este producto?")) {
        fetch(`/Admin/EliminarProducto?id=${id}`, { method: "POST" })
            .then(() => location.reload());
    }
}