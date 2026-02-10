// Simulación de envío de Demo
function submitDemoOneForty(e) {
    e.preventDefault(); // Evita recarga real para la demo visual

    const btn = document.getElementById('btnSubmitDemo');
    const originalText = btn.innerHTML;

    // Estado cargando
    btn.innerHTML = "SENDING...";
    btn.style.opacity = "0.7";

    setTimeout(() => {
        // Estado Enviado
        btn.innerHTML = '<i class="bi bi-check-circle-fill"></i> DEMO SENT';
        btn.classList.add('success'); // Se pone verde
        btn.style.opacity = "1";

        // Opcional: Cerrar modal después de 2 segundos
        setTimeout(() => {
            var myModalEl = document.getElementById('demoModal');
            var modal = bootstrap.Modal.getInstance(myModalEl);
            modal.hide();

            // Resetear botón
            btn.innerHTML = originalText;
            btn.classList.remove('success');
            document.getElementById('demoFormTag').reset();
        }, 2000);

    }, 1500);
}

// Scroll suave para las flechas
function scrollPage(id) {
    document.getElementById(id).scrollIntoView({ behavior: 'smooth' });
}
// --- PEGAR AL FINAL DE submance-interact.js ---

function enviarFormulario() {
    // 1. Obtener valores (Asegúrate que los IDs coincidan con tu HTML)
    const artist = document.getElementById('inputArtistName')?.value;
    const real = document.getElementById('inputRealName')?.value;
    const email = document.getElementById('inputEmail')?.value;
    const track = document.getElementById('inputTrackTitle')?.value;
    const link = document.getElementById('inputLink')?.value;

    // 2. Validación simple (Evitar enviar vacíos)
    if (!artist || !email || !track || !link) {
        alert("Por favor completa los campos obligatorios (Artista, Email, Track, Link).");
        return;
    }

    // 3. Preparar datos
    const datos = {
        ArtistName: artist,
        RealName: real || artist, // Si no pone nombre real, usamos el artístico
        Email: email,
        TrackTitle: track,
        Link: link
    };

    // 4. Cambiar texto del botón para feedback visual
    const btn = document.querySelector('button[onclick="enviarFormulario()"]');
    if (btn) btn.innerText = "Enviando...";

    // 5. Enviar al API (Asegúrate que el proyecto API esté corriendo)
    fetch('https://localhost:7064/api/Dashboard/RecibirDemoPublico', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(datos)
    })
        .then(response => {
            if (response.ok) {
                alert("¡Demo enviado exitosamente a Submance Records! Lo revisaremos pronto.");
                // Opcional: Limpiar el formulario
                document.getElementById('inputArtistName').value = '';
                document.getElementById('inputTrackTitle').value = '';
                document.getElementById('inputLink').value = '';
            } else {
                console.error("Error del servidor:", response.statusText);
                alert("Hubo un error al enviar. Intenta nuevamente.");
            }
        })
        .catch(error => {
            console.error("Error de conexión:", error);
            alert("Error de conexión. Asegúrate de que el servidor esté encendido.");
        })
        .finally(() => {
            if (btn) btn.innerText = "SUBMIT DEMO"; // Regresar botón a normal
        });
}