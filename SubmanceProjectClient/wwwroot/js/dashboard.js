// ================= CONFIGURACIÓN =================
const API_URL = "https://localhost:7064/api/Dashboard";
let globalDemos = [];
let myChart = null;

// ESTADO DE LOS FILTROS DEMOS
let currentFilter = {
    status: 'Todos',
    text: ''
};

// ESTADO DE LOS FILTROS ARTISTAS
let artistFilter = {
    sort: 'az',
    text: ''
};

// ================= INICIO =================
document.addEventListener("DOMContentLoaded", () => {
    console.log("🚀 Dashboard Submance iniciado...");
    checkDarkMode();
    renderChart({ totalDemos: 0, aprobados: 0, pendientes: 0 });
    refreshAllData();
});

function refreshAllData() {
    loadStats();
    loadDemos();
}

// ================= LÓGICA DE FILTRADO DEMOS =================
function setFilterStatus(status) {
    currentFilter.status = status;
    aplicarFiltros();
}

function setFilterText(text) {
    currentFilter.text = text.trim().toLowerCase();
    aplicarFiltros();
}

function aplicarFiltros() {
    if (!globalDemos) return;
    let resultados = globalDemos;

    // Filtro Estado
    if (currentFilter.status !== 'Todos') {
        resultados = resultados.filter(d => {
            const estado = (d.Estado || d.estado || "Pendiente");
            return estado.toLowerCase() === currentFilter.status.toLowerCase();
        });
    }

    // Filtro Texto
    if (currentFilter.text !== '') {
        resultados = resultados.filter(d => {
            const titulo = (d.TituloDemo || d.tituloDemo || "").toLowerCase();
            const artista = (d.NombreArtistico || d.nombreArtistico || "").toLowerCase();
            return titulo.includes(currentFilter.text) || artista.includes(currentFilter.text);
        });
    }

    renderDemosGrid(resultados);
}

// ================= LÓGICA DE FILTRADO ARTISTAS =================
function setArtistSort(sortType) {
    artistFilter.sort = sortType;
    aplicarFiltrosArtistas();
}

function setArtistFilterText(text) {
    artistFilter.text = text.trim().toLowerCase();
    aplicarFiltrosArtistas();
}

function aplicarFiltrosArtistas() {
    if (!globalDemos) return;

    const artistasMap = new Map();
    globalDemos.forEach(d => {
        const nombre = d.NombreArtistico || "Desconocido";
        if (!artistasMap.has(nombre)) {
            artistasMap.set(nombre, {
                nombre: nombre,
                email: d.Email || "N/A",
                tracks: 0
            });
        }
        artistasMap.get(nombre).tracks++;
    });

    let listaArtistas = Array.from(artistasMap.values());

    if (artistFilter.text !== '') {
        listaArtistas = listaArtistas.filter(a => a.nombre.toLowerCase().includes(artistFilter.text));
    }

    if (artistFilter.sort === 'az') listaArtistas.sort((a, b) => a.nombre.localeCompare(b.nombre));
    if (artistFilter.sort === 'za') listaArtistas.sort((a, b) => b.nombre.localeCompare(a.nombre));
    if (artistFilter.sort === 'tracks') listaArtistas.sort((a, b) => b.tracks - a.tracks);

    renderArtistsGrid(listaArtistas);
}

// ================= NAVEGACIÓN Y UX =================
function nav(view) {
    document.querySelectorAll('.view-section').forEach(el => {
        el.classList.add('hidden');
        el.classList.remove('animate__fadeIn');
    });
    document.querySelectorAll('.nav-item').forEach(el => el.classList.remove('active'));

    const target = document.getElementById('view-' + view);
    if (target) {
        target.classList.remove('hidden');
        void target.offsetWidth;
        target.classList.add('animate__fadeIn');
    }
    const link = document.getElementById('link-' + view);
    if (link) link.classList.add('active');

    if (view === 'home' && myChart) myChart.resize();
}

function toggleDarkMode() {
    document.body.classList.toggle('dark-mode');
    const isDark = document.body.classList.contains('dark-mode');
    localStorage.setItem('darkMode', isDark);
    checkDarkMode();
    if (myChart) {
        myChart.options.plugins.legend.labels.color = isDark ? '#fff' : '#333';
        myChart.update();
    }
}

function checkDarkMode() {
    const isDark = localStorage.getItem('darkMode') === 'true';
    if (isDark) document.body.classList.add('dark-mode');
    const icon = document.getElementById('theme-icon');
    if (icon) icon.className = isDark ? 'bi bi-sun-fill me-2 ms-2' : 'bi bi-moon-fill me-2 ms-2';
}

// ================= CONEXIÓN API =================
function loadStats() {
    fetch(`${API_URL}/GetStats`)
        .then(r => r.json())
        .then(data => {
            const total = data.totalDemos ?? 0;
            const pend = data.pendientes ?? 0;
            const aprob = data.aprobados ?? 0;
            const arts = data.artistas ?? 0;

            if (document.getElementById('st-total')) document.getElementById('st-total').innerText = total;
            if (document.getElementById('st-pend')) document.getElementById('st-pend').innerText = pend;
            if (document.getElementById('st-aprob')) document.getElementById('st-aprob').innerText = aprob;
            if (document.getElementById('st-arts')) document.getElementById('st-arts').innerText = arts;

            const badge = document.getElementById('badge-inbox');
            if (badge) {
                badge.innerText = pend;
                badge.style.display = pend > 0 ? 'inline-block' : 'none';
            }
            renderChart({ totalDemos: total, aprobados: aprob, pendientes: pend });
        })
        .catch(err => console.error("❌ Error en Stats:", err));
}

function loadDemos() {
    fetch(`${API_URL}/GetDemos`)
        .then(r => r.json())
        .then(data => {
            console.log("📥 Datos recibidos:", data);
            globalDemos = data;

            aplicarFiltros();

            const pendientes = data.filter(d => (d.Estado || "Pendiente").toLowerCase() === 'pendiente');
            renderInbox(pendientes);

            aplicarFiltrosArtistas();

            // Renderizamos lanzamientos
            renderReleasesGrid(data);
        })
        .catch(err => console.error("❌ Error al cargar demos:", err));
}

// ================= RENDERIZADO VISUAL =================

function renderChart(data) {
    const ctx = document.getElementById('chartOverview');
    if (!ctx || typeof Chart === 'undefined') return;
    if (myChart) myChart.destroy();

    const pend = data.pendientes ?? 0;
    const aprob = data.aprobados ?? 0;
    const total = data.totalDemos ?? 0;
    const rech = Math.max(0, total - pend - aprob);

    myChart = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: ['Pendientes', 'Aprobados', 'Rechazados'],
            datasets: [{
                data: total === 0 ? [1, 1, 1] : [pend, aprob, rech],
                backgroundColor: ['#ffc107', '#0d6efd', '#dc3545'],
                borderWidth: 0,
                hoverOffset: 15
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            cutout: '75%',
            plugins: {
                legend: {
                    position: 'bottom',
                    labels: { padding: 20, usePointStyle: true, color: document.body.classList.contains('dark-mode') ? '#fff' : '#333' }
                }
            }
        }
    });
}

function renderInbox(data) {
    const tbody = document.getElementById('inboxTableBody');
    if (!tbody) return;
    tbody.innerHTML = '';

    if (!data || data.length === 0) {
        tbody.innerHTML = '<tr><td colspan="4" class="text-center text-muted py-4">🎉 Buzón al día.</td></tr>';
        return;
    }

    data.forEach(d => {
        const id = d.IdDemo ?? d.idDemo;
        tbody.innerHTML += `
        <tr class="align-middle animate__animated animate__fadeIn">
            <td>
                <div class="fw-bold text-dark">${d.TituloDemo ?? d.tituloDemo ?? 'Sin Título'}</div>
                <div class="small text-muted">${d.Email ?? d.email ?? 'N/A'}</div>
            </td>
            <td>${d.NombreArtistico ?? d.nombreArtistico ?? 'Desconocido'}</td>
            <td><a href="${d.LinkDemo ?? d.linkDemo ?? '#'}" target="_blank" class="btn btn-sm btn-outline-primary"><i class="bi bi-play-fill"></i> Oír</a></td>
            <td>
                <div class="btn-group">
                    <button class="btn btn-sm btn-success" onclick="procesarDemo(${id}, 'Aprobada')"><i class="bi bi-check-lg"></i></button>
                    <button class="btn btn-sm btn-danger" onclick="procesarDemo(${id}, 'Rechazada')"><i class="bi bi-x-lg"></i></button>
                </div>
            </td>
        </tr>`;
    });
}

// --- GRID DEMOS (ACTUALIZADO CON BOTÓN PUBLICAR) ---
function renderDemosGrid(data) {
    const grid = document.getElementById('demosGrid');
    if (!grid) return;
    grid.innerHTML = '';

    if (!data || data.length === 0) {
        grid.innerHTML = `<div class="col-12 text-center py-5"><h5 class="text-muted">No hay demos.</h5></div>`;
        return;
    }

    data.forEach(d => {
        const titulo = d.TituloDemo ?? "Sin Título";
        const artista = d.NombreArtistico ?? "Anónimo";
        const email = d.Email ?? "Sin contacto";
        const link = d.LinkDemo ?? "#";
        const estado = (d.Estado ?? "Pendiente");

        let badgeClass = 'bg-warning text-dark bg-opacity-25';
        let bordeColor = '#ffc107';
        let iconStatus = 'bi-hourglass-split';

        // Lógica del botón de Acción
        let actionBtn = '';

        if (estado.toLowerCase() === 'aprobada') {
            badgeClass = 'bg-success text-success bg-opacity-25';
            bordeColor = '#198754';
            iconStatus = 'bi-check-circle-fill';
            // Si ya está aprobada, mostramos indicador visual
            actionBtn = `<small class="text-success fw-bold"><i class="bi bi-check-all me-1"></i> En Lanzamientos</small>`;
        } else if (estado.toLowerCase() === 'rechazada') {
            badgeClass = 'bg-danger text-danger bg-opacity-25';
            bordeColor = '#dc3545';
            iconStatus = 'bi-x-circle-fill';
            // Opción de reconsiderar (volver a publicar)
            actionBtn = `<button class="btn btn-sm btn-outline-success rounded-pill px-3" onclick="publicarDemo(${d.IdDemo})">Reconsiderar</button>`;
        } else {
            // Si está pendiente, botón PUBLICAR
            actionBtn = `<button class="btn btn-sm btn-dark rounded-pill px-3 shadow-sm" onclick="publicarDemo(${d.IdDemo})"><i class="bi bi-rocket-takeoff-fill me-1"></i> Publicar</button>`;
        }

        grid.innerHTML += `
        <div class="col-md-6 col-xl-4 animate__animated animate__fadeInUp">
            <div class="card card-demo h-100 shadow-sm rounded-4 position-relative overflow-hidden">
                <div class="card-body p-4 d-flex flex-column">
                    <div class="d-flex justify-content-between align-items-start mb-3">
                        <div class="track-icon shadow-sm"><i class="bi bi-music-note-beamed"></i></div>
                        <span class="badge ${badgeClass} badge-status"><i class="bi ${iconStatus} me-1"></i> ${estado}</span>
                    </div>
                    <h5 class="fw-bold text-dark mb-1 text-truncate" title="${titulo}">${titulo}</h5>
                    <p class="text-muted mb-3 small"><i class="bi bi-person-fill me-1"></i> ${artista}</p>
                    
                    <div class="mt-auto">
                        <hr class="opacity-25 my-3">
                        <div class="d-flex justify-content-between align-items-center">
                            <a href="${link}" target="_blank" class="text-decoration-none fw-bold text-purple small">
                                Escuchar <i class="bi bi-box-arrow-up-right"></i>
                            </a>
                            ${actionBtn}
                        </div>
                    </div>
                </div>
                <div class="position-absolute bottom-0 start-0 w-100" style="height: 4px; background-color: ${bordeColor}"></div>
            </div>
        </div>`;
    });
}

// --- GRID LANZAMIENTOS ---
function renderReleasesGrid(data) {
    const grid = document.getElementById('releasesGrid');
    if (!grid) return;
    grid.innerHTML = '';

    const aprobados = data.filter(d => (d.Estado || "").toLowerCase() === 'aprobada');

    if (aprobados.length === 0) {
        grid.innerHTML = `<div class="col-12 text-center py-5 text-muted">No hay tracks aprobados. Ve a Demos y dales "Publicar".</div>`;
        return;
    }

    aprobados.forEach(d => {
        let fechaValue = d.FechaLanzamiento ? new Date(d.FechaLanzamiento).toISOString().split('T')[0] : "";
        grid.innerHTML += `
        <div class="col-xl-6 animate__animated animate__fadeInUp">
            <div class="card border-0 shadow-sm rounded-4 overflow-hidden h-100">
                <div class="card-body p-0 d-flex flex-column flex-md-row">
                    <div class="bg-dark text-white d-flex align-items-center justify-content-center p-4" style="min-width: 150px; background: linear-gradient(45deg, #2c3e50, #000);">
                        <i class="bi bi-disc-fill fs-1"></i>
                    </div>
                    <div class="p-4 flex-grow-1">
                        <div class="d-flex justify-content-between mb-2">
                            <div><h5 class="fw-bold mb-0">${d.TituloDemo}</h5><small>${d.NombreArtistico}</small></div>
                            <span class="badge bg-success bg-opacity-10 text-success rounded-pill px-2">Signed</span>
                        </div>
                        <div class="row align-items-end g-2 mt-3">
                            <div class="col-8">
                                <label class="small fw-bold text-muted">Lanzamiento</label>
                                <input type="date" class="form-control form-control-sm" value="${fechaValue}" onchange="guardarFechaLanzamiento(${d.IdDemo}, this.value)">
                            </div>
                            <div class="col-4"><button class="btn btn-sm btn-outline-dark w-100" onclick="generarReporteFirma('${d.TituloDemo}','${d.NombreArtistico}')"><i class="bi bi-pen"></i></button></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>`;
    });
}

function renderArtistsGrid(listaArtistas) {
    const grid = document.getElementById('artistsGrid');
    if (!grid) return;
    grid.innerHTML = '';
    if (listaArtistas.length === 0) { grid.innerHTML = `<div class="col-12 text-center text-muted py-5">Sin resultados.</div>`; return; }

    listaArtistas.forEach(a => {
        const inicial = a.nombre.charAt(0).toUpperCase();
        const nombreSafe = a.nombre.replace(/'/g, "\\'");
        const emailSafe = a.email.replace(/'/g, "\\'");

        grid.innerHTML += `
        <div class="col-md-6 col-lg-4 col-xl-3 animate__animated animate__fadeInUp">
            <div class="card card-demo h-100 shadow-sm rounded-4 border-0 text-center position-relative overflow-hidden">
                <div class="position-absolute top-0 end-0 p-2 opacity-75 hover-opacity-100">
                    <button class="btn btn-sm btn-light rounded-circle shadow-sm text-primary border" onclick="editarArtista('${nombreSafe}', '${emailSafe}')"><i class="bi bi-pencil-fill small"></i></button>
                     <button class="btn btn-sm btn-light rounded-circle shadow-sm text-danger ms-1 border" onclick="eliminarArtista('${nombreSafe}')"><i class="bi bi-trash-fill small"></i></button>
                </div>
                <div class="card-body p-4 d-flex flex-column align-items-center">
                    <div class="rounded-circle bg-dark text-white d-flex align-items-center justify-content-center mb-3 shadow-sm" style="width: 70px; height: 70px; font-size: 1.5rem; font-weight: 800;">${inicial}</div>
                    <h5 class="fw-bold text-dark mb-1 text-truncate w-100" title="${a.nombre}">${a.nombre}</h5>
                    <div class="badge bg-light text-muted border mb-3 rounded-pill px-3 mt-1">${a.tracks} Tracks</div>
                    <div class="mt-auto w-100"><div class="p-2 bg-light rounded small text-muted text-truncate border">${a.email}</div></div>
                </div>
                <div class="position-absolute bottom-0 start-0 w-100" style="height: 4px; background-color: #6f42c1;"></div>
            </div>
        </div>`;
    });
}

// ================= ACCIONES =================

// NUEVA FUNCIÓN: Publicar Demo (Aprobar directamente)
function publicarDemo(id) {
    Swal.fire({
        title: '¿Publicar Demo?',
        text: "Pasará a estado 'Aprobada' y se enviará a Lanzamientos.",
        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: '#198754',
        confirmButtonText: 'Sí, Publicar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            fetch(`${API_URL}/CambiarEstado`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ IdDemo: id, NuevoEstado: 'Aprobada' })
            }).then(() => {
                refreshAllData();
                const Toast = Swal.mixin({ toast: true, position: 'top-end', showConfirmButton: false, timer: 3000, timerProgressBar: true });
                Toast.fire({ icon: 'success', title: '¡Demo publicado! Revisa Lanzamientos.' });
            });
        }
    });
}

function procesarDemo(id, nuevoEstado) {
    Swal.fire({
        title: `¿${nuevoEstado} Demo?`,
        input: 'textarea',
        inputPlaceholder: 'Feedback (opcional)...',
        showCancelButton: true,
        confirmButtonColor: nuevoEstado === 'Aprobada' ? '#198754' : '#dc3545'
    }).then((result) => {
        if (result.isConfirmed) {
            fetch(`${API_URL}/CambiarEstado`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ IdDemo: id, NuevoEstado: nuevoEstado, Comentario: result.value || "" })
            }).then(() => { refreshAllData(); Swal.fire('Éxito', 'Estado actualizado', 'success'); });
        }
    });
}

function guardarFechaLanzamiento(id, fecha) {
    fetch(`${API_URL}/AgendarLanzamiento`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ IdDemo: id, Fecha: fecha })
    })
        .then(r => r.json())
        .then(() => {
            const Toast = Swal.mixin({ toast: true, position: 'top-end', showConfirmButton: false, timer: 2000 });
            Toast.fire({ icon: 'success', title: 'Fecha agendada' });
        });
}

function generarReporteFirma(track, artista) {
    Swal.fire({ title: 'Generando Contrato...', timer: 1500, didOpen: () => { Swal.showLoading() } })
        .then(() => Swal.fire('Listo', `Contrato para ${track} enviado a cola.`, 'success'));
}
// ================= GENERADOR DE REPORTES (PDF REAL) =================

function generarReportePDF(tipo) {
    // Verificar si las librerías cargaron
    if (!window.jspdf) {
        Swal.fire('Error', 'Librería PDF no cargada. Revisa tu conexión.', 'error');
        return;
    }

    const { jsPDF } = window.jspdf;
    const doc = new jsPDF();

    // Configuración inicial
    const fechaHoy = new Date().toLocaleDateString();
    let titulo = "Reporte General - Submance Records";
    let datos = [];
    let columnas = ["Track", "Artista", "Estado", "Email", "Fecha Envio"];

    // 1. FILTRADO DE DATOS SEGÚN EL BOTÓN
    if (tipo === 'pendientes') {
        titulo = "Reporte de Demos Pendientes";
        datos = globalDemos.filter(d => (d.Estado || "").toLowerCase() === 'pendiente');
    }
    else if (tipo === 'lanzamientos') {
        titulo = "Calendario de Lanzamientos";
        columnas = ["Track", "Artista", "Lanzamiento", "Email", "Link"]; // Columnas diferentes
        datos = globalDemos.filter(d => (d.Estado || "").toLowerCase() === 'aprobada');
    }
    else if (tipo === 'artistas') {
        titulo = "Directorio de Artistas";
        columnas = ["Artista", "Tracks", "Email de Contacto"];

        // Lógica especial para agrupar artistas únicos
        const mapa = new Map();
        globalDemos.forEach(d => {
            const nombre = d.NombreArtistico || "Desconocido";
            if (!mapa.has(nombre)) mapa.set(nombre, { nombre: nombre, email: d.Email, tracks: 0 });
            mapa.get(nombre).tracks++;
        });
        datos = Array.from(mapa.values());
    }
    else {
        datos = globalDemos; // Todos
    }

    if (datos.length === 0) {
        Swal.fire('Atención', 'No hay datos para generar este reporte.', 'info');
        return;
    }

    // 2. DISEÑO DEL HEADER DEL PDF
    doc.setFillColor(111, 66, 193); // Morado Submance
    doc.rect(0, 0, 210, 20, 'F'); // Barra superior

    doc.setTextColor(255, 255, 255);
    doc.setFontSize(16);
    doc.setFont("helvetica", "bold");
    doc.text("SUBMANCE RECORDS", 14, 13);

    doc.setTextColor(0, 0, 0);
    doc.setFontSize(14);
    doc.text(titulo, 14, 30);
    doc.setFontSize(10);
    doc.setTextColor(100);
    doc.text(`Generado el: ${fechaHoy}`, 14, 36);

    // 3. GENERACIÓN DE LA TABLA (Mapeo de datos)
    let bodyData = [];

    if (tipo === 'artistas') {
        bodyData = datos.map(a => [a.nombre, a.tracks, a.email]);
    } else if (tipo === 'lanzamientos') {
        bodyData = datos.map(d => [
            d.TituloDemo,
            d.NombreArtistico,
            d.FechaLanzamiento ? new Date(d.FechaLanzamiento).toLocaleDateString() : 'Por definir',
            d.Email,
            d.LinkDemo
        ]);
    } else {
        bodyData = datos.map(d => [
            d.TituloDemo,
            d.NombreArtistico,
            d.Estado,
            d.Email,
            new Date(d.FechaEnvio).toLocaleDateString()
        ]);
    }

    doc.autoTable({
        startY: 45,
        head: [columnas],
        body: bodyData,
        theme: 'grid',
        headStyles: { fillColor: [111, 66, 193], textColor: 255, fontStyle: 'bold' },
        styles: { fontSize: 9, cellPadding: 3 },
        alternateRowStyles: { fillColor: [248, 249, 250] }
    });

    // 4. GUARDAR ARCHIVO
    doc.save(`Submance_${titulo.replace(/\s+/g, '_')}.pdf`);

    // Notificación
    const Toast = Swal.mixin({ toast: true, position: 'top-end', showConfirmButton: false, timer: 3000 });
    Toast.fire({ icon: 'success', title: 'PDF descargado' });
}

function logoutConfirm() { Swal.fire({ title: '¿Cerrar sesión?', icon: 'warning', showCancelButton: true, confirmButtonColor: '#121212' }).then(r => { if (r.isConfirmed) location.href = '/Auth/Login'; }); }
function openAddDemoModal() { Swal.fire({ title: 'Nuevo Demo', html: `<input id="sw-t" class="swal2-input" placeholder="Título"><input id="sw-a" class="swal2-input" placeholder="Artista"><input id="sw-l" class="swal2-input" placeholder="Link">`, preConfirm: () => { return { TrackTitle: document.getElementById('sw-t').value, ArtistName: document.getElementById('sw-a').value, Link: document.getElementById('sw-l').value } } }).then(r => { if (r.isConfirmed) fetch(`${API_URL}/RecibirDemoPublico`, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(r.value) }).then(() => { refreshAllData(); Swal.fire('Guardado', '', 'success') }); }); }
function openAddArtistModal() { openAddDemoModal(); }
function editarArtista(n, e) { Swal.fire({ title: 'Editar', html: `<input id="sw-n" class="swal2-input" value="${n}"><input id="sw-e" class="swal2-input" value="${e}">`, preConfirm: () => { return { NombreActual: n, NuevoNombre: document.getElementById('sw-n').value, NuevoEmail: document.getElementById('sw-e').value } } }).then(r => { if (r.isConfirmed) fetch(`${API_URL}/ActualizarArtista`, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(r.value) }).then(() => { refreshAllData(); Swal.fire('Actualizado', '', 'success') }) }); }
function eliminarArtista(n) { Swal.fire({ title: `Eliminar ${n}?`, text: 'Se borrarán sus tracks.', icon: 'warning', showCancelButton: true, confirmButtonColor: '#dc3545' }).then(r => { if (r.isConfirmed) fetch(`${API_URL}/EliminarArtista`, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify({ NombreArtistico: n }) }).then(() => { refreshAllData(); Swal.fire('Eliminado', '', 'success') }) }); }