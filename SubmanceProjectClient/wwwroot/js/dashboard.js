// wwwroot/js/dashboard.js
document.addEventListener('DOMContentLoaded', function () {
    console.log("Sistema Submance Conectado a Supabase.");
    loadDashboardData();
    initChart();
});

function escapeHTML(str) {
    if (typeof str !== 'string') return String(str ?? '');
    return str.replace(/[&<>'"]/g, tag => ({
        '&': '&amp;', '<': '&lt;', '>': '&gt;', "'": '&#39;', '"': '&quot;'
    }[tag] || tag));
}

function nav(viewId) {
    document.querySelectorAll('.view-section').forEach(el => el.classList.add('hidden'));
    document.querySelectorAll('.nav-item').forEach(el => el.classList.remove('active'));
    const target = document.getElementById(`view-${viewId}`);
    if (target) { target.classList.remove('hidden'); target.classList.add('animate__fadeIn'); }
    const link = document.getElementById(`link-${viewId}`);
    if (link) link.classList.add('active');
}

function loadDashboardData() {
    fetch('/Admin/GetDashboardData')
        .then(response => {
            if (!response.ok) throw new Error(`HTTP ${response.status}`);
            return response.json();
        })
        .then(data => {
            // Stats
            updateStat("st-total", data.stats.totalDemos);
            updateStat("st-pend", data.stats.pendientes);
            updateStat("st-aprob", data.stats.aprobados);
            updateStat("st-arts", data.stats.artistas);
            document.getElementById('badge-inbox').innerText = data.stats.pendientes;

            const demos = data.demos || [];
            const artistas = data.artistas || [];

            renderInbox(demos);
            renderDemosGrid(demos);
            renderArtists(artistas);
            renderReleases(demos);
            updateChart(data.stats);
        })
        .catch(error => console.error('Error cargando datos:', error));
}

function updateStat(id, value) {
    const el = document.getElementById(id);
    if (el) el.innerText = value ?? 0;
}

// ─── Buzón ───────────────────────────────────────────────────────────────────
function renderInbox(demos) {
    const tbody = document.getElementById('inboxTableBody');
    if (!tbody) return;

    // Buzón: SOLO pendientes
    const pendientes = demos.filter(d => d.estado === 'Pendiente');

    if (pendientes.length === 0) {
        tbody.innerHTML = `<tr><td colspan="4" class="text-center text-secondary py-4">Sin demos pendientes de revisión.</td></tr>`;
        return;
    }

    tbody.innerHTML = pendientes.map(demo => {
        const id = demo.idDemo;                              // FIX: idDemo correcto
        const tituloSafe = escapeHTML(demo.titulo);
        const artistaSafe = escapeHTML(demo.artistaNombre);
        const tituloJs = (demo.titulo || '').replace(/'/g, "\\'");
        const artistaJs = (demo.artistaNombre || '').replace(/'/g, "\\'");
        const urlSafe = escapeHTML(demo.urlAudio || '');

        return `
        <tr>
            <td class="ps-4">
                <div class="d-flex align-items-center">
                    <div class="text-cyan fs-4 me-3"><i class="bi bi-music-note-beamed"></i></div>
                    <div>
                        <div class="fw-bold">${tituloSafe}</div>
                        <div class="text-secondary small">UID: ${id}</div>
                    </div>
                </div>
            </td>
            <td>${artistaSafe}</td>
            <td class="text-center">
                <button class="btn btn-sm btn-link text-cyan rounded-circle fs-4"
                        id="btn-play-${id}"
                        onclick="playTrack('${urlSafe}', ${id}, '${tituloJs}', '${artistaJs}')">
                    <i class="bi bi-play-circle"></i>
                </button>
            </td>
            <td class="text-end pe-4">
                <button class="btn btn-sm btn-outline-info me-1"   onclick="accionDemo(${id}, 'Aprobado')">APROBAR</button>
                <button class="btn btn-sm btn-outline-danger"       onclick="accionDemo(${id}, 'Rechazado')">RECHAZAR</button>
            </td>
        </tr>`;
    }).join('');
}

// ─── Catálogo ─────────────────────────────────────────────────────────────────
function renderDemosGrid(demos) {
    const grid = document.getElementById('demosGrid');
    if (!grid) return;

    if (demos.length === 0) {
        grid.innerHTML = `<div class="col-12 text-center text-secondary py-5">No hay demos registrados.</div>`;
        return;
    }

    grid.innerHTML = demos.map(demo => {
        const badgeClass = demo.estado === 'Aprobado' ? 'text-bg-info' :
            demo.estado === 'Rechazado' ? 'text-bg-danger' : 'text-bg-warning';
        return `
        <div class="col-md-4 col-lg-3 grid-item">
            <div class="stat-card p-3 h-100">
                <div class="d-flex justify-content-between mb-2">
                    <span class="badge ${badgeClass} status-badge">${escapeHTML(demo.estado)}</span>
                </div>
                <h6 class="fw-bold mb-0 text-truncate title-val">${escapeHTML(demo.titulo)}</h6>
                <p class="text-secondary small mb-3 artist-val">${escapeHTML(demo.artistaNombre)}</p>
            </div>
        </div>`;
    }).join('');
}

// ─── Artistas / Roster ────────────────────────────────────────────────────────
function renderArtists(artists) {
    const grid = document.getElementById('artistsGrid');
    if (!grid) return;

    if (artists.length === 0) {
        grid.innerHTML = `<div class="col-12 text-center text-secondary py-5">No hay artistas registrados.</div>`;
        return;
    }

    grid.innerHTML = artists.map(art => `
        <div class="col-md-4 grid-item">
            <div class="d-flex align-items-center p-3 stat-card">
                <div class="rounded-circle bg-black text-cyan d-flex align-items-center justify-content-center me-3 border border-secondary"
                     style="width:40px; height:40px; font-weight:bold;">
                    ${escapeHTML(art.nombreArtistico.charAt(0).toUpperCase())}
                </div>
                <div>
                    <h6 class="mb-0 fw-bold name-val">${escapeHTML(art.nombreArtistico)}</h6>
                    <small class="text-secondary">${escapeHTML(art.pais || 'N/A')}</small>
                </div>
            </div>
        </div>`
    ).join('');
}

// ─── Lanzamientos ─────────────────────────────────────────────────────────────
function renderReleases(demos) {
    const grid = document.getElementById('releasesGrid');
    if (!grid) return;

    const releases = demos.filter(d => d.estado === 'Aprobado');
    if (releases.length === 0) {
        grid.innerHTML = `<div class="col-12 text-center text-secondary py-5">No hay lanzamientos programados.</div>`;
        return;
    }

    grid.innerHTML = releases.map(r => {
        const date = new Date(r.fechaEnvio);
        date.setDate(date.getDate() + 30);
        const fechaSalida = date.toLocaleDateString('es-PE');
        return `
        <div class="col-md-6 col-lg-4">
            <div class="stat-card h-100 p-3 d-flex align-items-center">
                <div class="rounded p-2 me-3 text-center text-black fw-bold"
                     style="min-width:60px; background: var(--cyan-primary);">
                    <div class="fs-4 mb-0">${date.getDate()}</div>
                    <small class="text-uppercase" style="font-size:0.6rem">Lanzamiento</small>
                </div>
                <div>
                    <h6 class="fw-bold mb-1 text-white">${escapeHTML(r.titulo)}</h6>
                    <p class="text-secondary small mb-0">${escapeHTML(r.artistaNombre)}</p>
                    <span class="badge border border-info text-info mt-2">Programado: ${fechaSalida}</span>
                </div>
            </div>
        </div>`;
    }).join('');
}

// ─── Filtros ──────────────────────────────────────────────────────────────────
function setFilterStatus(status) {
    document.querySelectorAll('#demosGrid .grid-item').forEach(card => {
        const badge = card.querySelector('.status-badge')?.innerText;
        card.style.display = (status === 'Todos' || status === badge) ? 'block' : 'none';
    });
}

function setFilterText(text) {
    const val = text.toLowerCase();
    document.querySelectorAll('#demosGrid .grid-item').forEach(card => {
        const title = card.querySelector('.title-val')?.innerText.toLowerCase() || '';
        const artist = card.querySelector('.artist-val')?.innerText.toLowerCase() || '';
        card.style.display = (title.includes(val) || artist.includes(val)) ? 'block' : 'none';
    });
}

function setArtistFilterText(text) {
    const val = text.toLowerCase();
    document.querySelectorAll('#artistsGrid .grid-item').forEach(card => {
        const name = card.querySelector('.name-val')?.innerText.toLowerCase() || '';
        card.style.display = name.includes(val) ? 'block' : 'none';
    });
}

// ─── Acciones A&R ─────────────────────────────────────────────────────────────
function accionDemo(id, nuevoEstado) {
    const texto = nuevoEstado === 'Aprobado' ? 'Aprobar' : 'Rechazar';
    const color = nuevoEstado === 'Aprobado' ? '#00f2ff' : '#dc3545';

    Swal.fire({
        title: `¿${texto} este demo?`,
        text: "Se actualizará el estado en la base de datos.",
        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: color,
        confirmButtonText: `Sí, ${texto}`,
        cancelButtonText: 'Cancelar',
        background: '#121212', color: '#ffffff'
    }).then(result => {
        if (!result.isConfirmed) return;
        fetch(`/Admin/UpdateDemoStatus?id=${id}&status=${nuevoEstado}`, { method: 'POST' })
            .then(res => res.json())
            .then(data => {
                if (data.success) {
                    Swal.fire({ title: 'Actualizado', icon: 'success', background: '#121212', color: '#fff' });
                    loadDashboardData();
                } else {
                    Swal.fire({ title: 'Error', text: data.message || 'No se pudo actualizar.', icon: 'error', background: '#121212', color: '#fff' });
                }
            });
    });
}

// ─── Chart ────────────────────────────────────────────────────────────────────
let myChart;
function initChart() {
    const ctx = document.getElementById('chartOverview');
    if (!ctx) return;
    myChart = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: ['Pendientes', 'Aprobados', 'Otros'],
            datasets: [{
                data: [0, 0, 0],
                backgroundColor: ['#ffc107', '#00f2ff', '#dc3545'],
                borderWidth: 0, hoverOffset: 4
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: { legend: { labels: { color: '#ffffff' } } }
        }
    });
}

function updateChart(stats) {
    if (!myChart) return;
    const otros = Math.max(0, stats.totalDemos - stats.pendientes - stats.aprobados);
    myChart.data.datasets[0].data = [stats.pendientes, stats.aprobados, otros];
    myChart.update();
}

// ─── Player ───────────────────────────────────────────────────────────────────
let currentAudio = document.getElementById('audio-source');
let currentBtnId = null;
let isPlaying = false;

function playTrack(url, id, title, artist) {
    const playerBar = document.getElementById('global-player');
    const btnId = `btn-play-${id}`;

    if (currentBtnId === btnId) { togglePlay(); return; }

    if (currentBtnId) {
        const prevIcon = document.querySelector(`#${currentBtnId} i`);
        if (prevIcon) prevIcon.className = 'bi bi-play-circle';
    }

    currentAudio.src = url;
    currentBtnId = btnId;
    document.getElementById('player-title').innerText = title;
    document.getElementById('player-artist').innerText = artist;
    playerBar.classList.remove('d-none');

    currentAudio.play().then(() => { isPlaying = true; updateIcons('pause'); })
        .catch(e => console.error('Error de audio:', e));
}

function togglePlay() {
    if (currentAudio.paused) { currentAudio.play(); isPlaying = true; updateIcons('pause'); }
    else { currentAudio.pause(); isPlaying = false; updateIcons('play'); }
}

function updateIcons(state) {
    const rowIcon = document.querySelector(`#${currentBtnId} i`);
    const mainIcon = document.getElementById('player-main-btn');
    if (rowIcon) rowIcon.className = state === 'pause' ? 'bi bi-pause-circle-fill' : 'bi bi-play-circle';
    if (mainIcon) mainIcon.className = state === 'pause' ? 'bi bi-pause-circle-fill' : 'bi bi-play-circle-fill';
}

function closePlayer() {
    currentAudio.pause();
    currentAudio.currentTime = 0;
    document.getElementById('global-player').classList.add('d-none');
    if (currentBtnId) {
        const prevIcon = document.querySelector(`#${currentBtnId} i`);
        if (prevIcon) prevIcon.className = 'bi bi-play-circle';
    }
    currentBtnId = null;
    isPlaying = false;
}

currentAudio.ontimeupdate = function () {
    if (!isNaN(currentAudio.duration)) {
        document.getElementById('player-seek').value =
            (currentAudio.currentTime / currentAudio.duration) * 100 || 0;
    }
};
currentAudio.onended = function () { isPlaying = false; updateIcons('play'); };

window.seekAudio = function () {
    const slider = document.getElementById('player-seek');
    if (!isNaN(currentAudio.duration))
        currentAudio.currentTime = (slider.value / 100) * currentAudio.duration;
};
window.setVolume = function (val) { currentAudio.volume = val / 100; };

// ─── Modales Admin ────────────────────────────────────────────────────────────
window.openAddArtistModal = function () {
    new bootstrap.Modal(document.getElementById('modalAddArtist')).show();
};

window.openAddDemoModal = function () {
    new bootstrap.Modal(document.getElementById('modalAddDemo')).show();
};

window.saveArtist = function () {
    const data = {
        nombreArtistico: document.getElementById('newArtName').value.trim(),
        nombreReal: document.getElementById('newArtReal').value.trim(),
        pais: document.getElementById('newArtCountry').value.trim(),
        correo: document.getElementById('newArtEmail').value.trim()
    };
    fetch('/Admin/CreateArtist', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    })
        .then(res => res.json())
        .then(result => {
            if (result.success) {
                bootstrap.Modal.getInstance(document.getElementById('modalAddArtist')).hide();
                Swal.fire({ title: '¡Creado!', text: 'Artista añadido.', icon: 'success', background: '#121212', color: '#fff' });
                document.getElementById('formAddArtist').reset();
                loadDashboardData();
            } else {
                Swal.fire({ title: 'Error', text: result.message || 'No se pudo crear.', icon: 'error', background: '#121212', color: '#fff' });
            }
        });
};

window.saveDemo = function () {
    const data = {
        Titulo: document.getElementById('newDemoTitle').value.trim(),
        UrlAudio: document.getElementById('newDemoUrl').value.trim(),
        IdArtista: parseInt(document.getElementById('newDemoArtistId').value),
        IdGenero: parseInt(document.getElementById('newDemoGenreId').value)
    };
    fetch('/Admin/CreateDemo', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    })
        .then(res => res.json())
        .then(result => {
            if (result.success) {
                bootstrap.Modal.getInstance(document.getElementById('modalAddDemo')).hide();
                Swal.fire({ title: '¡Subido!', text: 'Demo registrado.', icon: 'success', background: '#121212', color: '#fff' });
                loadDashboardData();
            } else {
                Swal.fire({ title: 'Error', text: 'Verifica que el ID Artista exista.', icon: 'error', background: '#121212', color: '#fff' });
            }
        });
};

// ─── Reportes PDF ─────────────────────────────────────────────────────────────
window.generarReportePDF = function (tipo) {
    const { jsPDF } = window.jspdf;
    const doc = new jsPDF();

    doc.setFontSize(18);
    doc.text(`Reporte: ${tipo.toUpperCase()} — Submance Records`, 14, 22);
    doc.setFontSize(11);
    doc.text(`Generado: ${new Date().toLocaleString('es-PE')}`, 14, 30);

    const cards = document.querySelectorAll('#demosGrid .grid-item');
    const data = [];

    cards.forEach((c, i) => {
        const estado = c.querySelector('.status-badge')?.innerText || '';
        if (tipo === 'todos' ||
            (tipo === 'pendientes' && estado === 'Pendiente') ||
            (tipo === 'lanzamientos' && estado === 'Aprobado')) {
            data.push([
                i + 1,
                c.querySelector('.title-val')?.innerText || '',
                c.querySelector('.artist-val')?.innerText || '',
                estado
            ]);
        }
    });

    doc.autoTable({
        head: [['#', 'Título', 'Artista', 'Estado']],
        body: data,
        startY: 40,
        theme: 'grid',
        styles: { fontSize: 10, cellPadding: 3 }
    });

    doc.save(`submance_${tipo}_${Date.now()}.pdf`);
};

function toggleDarkMode() {
    const html = document.documentElement;
    html.setAttribute('data-theme', html.getAttribute('data-theme') === 'dark' ? 'light' : 'dark');
}