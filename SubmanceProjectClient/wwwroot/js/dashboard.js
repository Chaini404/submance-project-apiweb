// wwwroot/js/dashboard.js
document.addEventListener('DOMContentLoaded', function () {
    console.log("Sistema Submance Conectado a Supabase.");
    loadDashboardData();
    initChart();
});

function escapeHTML(str) {
    if (typeof str !== 'string') return str;
    return str.replace(/[&<>'"]/g, tag => ({
        '&': '&amp;', '<': '&lt;', '>': '&gt;', "'": '&#39;', '"': '&quot;'
    }[tag] || tag));
}

function nav(viewId) {
    document.querySelectorAll('.view-section').forEach(el => el.classList.add('hidden'));
    document.querySelectorAll('.nav-item').forEach(el => el.classList.remove('active'));
    const target = document.getElementById(`view-${viewId}`);
    if (target) {
        target.classList.remove('hidden');
        target.classList.add('animate__fadeIn');
    }
    const link = document.getElementById(`link-${viewId}`);
    if (link) link.classList.add('active');
}

function loadDashboardData() {
    fetch('/Admin/GetDashboardData')
        .then(response => {
            if (!response.ok) throw new Error('Error en red');
            return response.json();
        })
        .then(data => {
            updateStat("st-total", data.stats.totalDemos);
            updateStat("st-pend", data.stats.pendientes);
            updateStat("st-aprob", data.stats.aprobados);
            updateStat("st-arts", data.stats.artistas);
            document.getElementById('badge-inbox').innerText = data.stats.pendientes;

            renderInbox(data.demos || []);
            renderDemosGrid(data.demos || []);
            renderArtists(data.artistas || []);
            renderReleases(data.demos || []);
            updateChart(data.stats);
        })
        .catch(error => console.error('Error cargando datos:', error));
}

function updateStat(id, value) {
    const el = document.getElementById(id);
    if (el) el.innerText = value;
}

function accionDemo(id, nuevoEstado) {
    const texto = nuevoEstado === 'Aprobado' ? 'Aprobar' : 'Rechazar';
    const color = nuevoEstado === 'Aprobado' ? '#00f2ff' : '#dc3545';

    Swal.fire({
        title: `¿${texto} demo?`,
        text: "Se actualizará el estado en la base de datos.",
        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: color,
        confirmButtonText: `Sí, ${texto}`,
        cancelButtonText: 'Cancelar',
        background: '#121212',
        color: '#ffffff'
    }).then((result) => {
        if (result.isConfirmed) {
            fetch(`/Admin/UpdateDemoStatus?id=${id}&status=${nuevoEstado}`, { method: 'POST' })
                .then(res => res.json())
                .then(data => {
                    if (data.success) {
                        Swal.fire({ title: 'Actualizado', text: 'Estado cambiado.', icon: 'success', background: '#121212', color: '#fff' });
                        loadDashboardData();
                    } else {
                        Swal.fire({ title: 'Error', text: 'No se pudo conectar con la BD', icon: 'error', background: '#121212', color: '#fff' });
                    }
                });
        }
    });
}

function openAddArtistModal() {
    new bootstrap.Modal(document.getElementById('modalAddArtist')).show();
}

function saveArtist() {
    const data = {
        nombreArtistico: document.getElementById('newArtName').value,
        nombreReal: document.getElementById('newArtReal').value,
        pais: document.getElementById('newArtCountry').value,
        correo: document.getElementById('newArtEmail').value
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
                Swal.fire({ title: 'Error', text: result.message, icon: 'error', background: '#121212', color: '#fff' });
            }
        });
}

function renderInbox(demos) {
    const tbody = document.getElementById('inboxTableBody');
    if (!tbody) return;
    let htmlContent = '';

    demos.forEach(demo => {
        if (demo.estado === 'Pendiente') {
            const tituloSafe = escapeHTML(demo.titulo);
            const artistaSafe = escapeHTML(demo.artistaNombre);
            const tituloEscapedJs = demo.titulo.replace(/'/g, "\\'");
            const artistaEscapedJs = demo.artistaNombre.replace(/'/g, "\\'");

            // CORRECCIÓN: demo.id -> demo.idDemo | demo.urlAudio
            htmlContent += `
                <tr>
                    <td class="ps-4">
                        <div class="d-flex align-items-center">
                            <div class="text-cyan fs-4 me-3"><i class="bi bi-music-note-beamed"></i></div>
                            <div>
                                <div class="fw-bold">${tituloSafe}</div>
                                <div class="text-secondary small">UID: ${demo.idDemo}</div>
                            </div>
                        </div>
                    </td>
                    <td>${artistaSafe}</td>
                    <td class="text-center">
                        <button class="btn btn-sm btn-link text-cyan rounded-circle fs-4" 
                                id="btn-play-${demo.idDemo}"
                                onclick="playTrack('${escapeHTML(demo.urlAudio)}', ${demo.idDemo}, '${tituloEscapedJs}', '${artistaEscapedJs}')">
                            <i class="bi bi-play-circle"></i>
                        </button>
                    </td>
                    <td class="text-end pe-4">
                        <button class="btn btn-sm btn-outline-info me-1" onclick="accionDemo(${demo.idDemo}, 'Aprobado')">APROBAR</button>
                        <button class="btn btn-sm btn-outline-danger" onclick="accionDemo(${demo.idDemo}, 'Rechazado')">RECHAZAR</button>
                    </td>
                </tr>`;
        }
    });
    tbody.innerHTML = htmlContent;
}

function renderDemosGrid(demos) {
    const grid = document.getElementById('demosGrid');
    if (!grid) return;
    let htmlContent = '';

    demos.forEach(demo => {
        let badgeClass = demo.estado === 'Aprobado' ? 'text-bg-info' : (demo.estado === 'Rechazado' ? 'text-bg-danger' : 'text-bg-warning');
        htmlContent += `
            <div class="col-md-4 col-lg-3 grid-item">
                <div class="stat-card p-3 h-100">
                    <div class="d-flex justify-content-between mb-2">
                        <span class="badge ${badgeClass} status-badge">${escapeHTML(demo.estado)}</span>
                    </div>
                    <h6 class="fw-bold mb-0 text-truncate title-val">${escapeHTML(demo.titulo)}</h6>
                    <p class="text-secondary small mb-3 artist-val">${escapeHTML(demo.artistaNombre)}</p>
                </div>
            </div>`;
    });
    grid.innerHTML = htmlContent;
}

function renderArtists(artists) {
    const grid = document.getElementById('artistsGrid');
    if (!grid) return;
    let htmlContent = '';

    artists.forEach(art => {
        htmlContent += `
            <div class="col-md-4 grid-item">
                <div class="d-flex align-items-center p-3 stat-card">
                    <div class="rounded-circle bg-black text-cyan d-flex align-items-center justify-content-center me-3 border border-secondary" style="width:40px; height:40px;">
                        ${escapeHTML(art.nombreArtistico.charAt(0))}
                    </div>
                    <div>
                        <h6 class="mb-0 fw-bold name-val">${escapeHTML(art.nombreArtistico)}</h6>
                        <small class="text-secondary">${escapeHTML(art.pais || 'N/A')}</small>
                    </div>
                </div>
            </div>`;
    });
    grid.innerHTML = htmlContent;
}

function setFilterStatus(status) {
    const cards = document.querySelectorAll('#demosGrid .grid-item');
    cards.forEach(card => {
        const badge = card.querySelector('.status-badge').innerText;
        const mostrar = (status === 'Todos') || (status === badge);
        card.style.display = mostrar ? 'block' : 'none';
    });
}

function setFilterText(text) {
    const val = text.toLowerCase();
    const cards = document.querySelectorAll('#demosGrid .grid-item');
    cards.forEach(card => {
        const title = card.querySelector('.title-val').innerText.toLowerCase();
        const artist = card.querySelector('.artist-val').innerText.toLowerCase();
        card.style.display = (title.includes(val) || artist.includes(val)) ? 'block' : 'none';
    });
}

function setArtistFilterText(text) {
    const val = text.toLowerCase();
    const cards = document.querySelectorAll('#artistsGrid .grid-item');
    cards.forEach(card => {
        const name = card.querySelector('.name-val').innerText.toLowerCase();
        card.style.display = name.includes(val) ? 'block' : 'none';
    });
}

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
                borderWidth: 0,
                hoverOffset: 4
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
    if (myChart) {
        let otros = stats.totalDemos - (stats.pendientes + stats.aprobados);
        myChart.data.datasets[0].data = [stats.pendientes, stats.aprobados, Math.max(0, otros)];
        myChart.update();
    }
}

function toggleDarkMode() { console.log("Dark mode on."); }

let currentAudio = document.getElementById('audio-source');
let currentBtnId = null;
let isPlaying = false;

function playTrack(url, id, title, artist) {
    const playerBar = document.getElementById('global-player');
    const btnId = `btn-play-${id}`;

    if (currentBtnId === btnId) {
        togglePlay();
        return;
    }

    if (currentBtnId) {
        const prevIcon = document.querySelector(`#${currentBtnId} i`);
        if (prevIcon) prevIcon.className = 'bi bi-play-circle';
    }

    currentAudio.src = url;
    currentBtnId = btnId;
    document.getElementById('player-title').innerText = title;
    document.getElementById('player-artist').innerText = artist;
    playerBar.classList.remove('d-none');

    currentAudio.play().then(() => {
        isPlaying = true;
        updateIcons('pause');
    }).catch(e => console.error(e));
}

function togglePlay() {
    if (currentAudio.paused) {
        currentAudio.play();
        isPlaying = true;
        updateIcons('pause');
    } else {
        currentAudio.pause();
        isPlaying = false;
        updateIcons('play');
    }
}

function updateIcons(state) {
    const rowIcon = document.querySelector(`#${currentBtnId} i`);
    const mainIcon = document.getElementById('player-main-btn');
    const iconClass = state === 'pause' ? 'bi bi-pause-circle-fill' : 'bi bi-play-circle';
    const mainClass = state === 'pause' ? 'bi bi-pause-circle-fill' : 'bi bi-play-circle-fill';

    if (rowIcon) rowIcon.className = iconClass;
    if (mainIcon) mainIcon.className = mainClass;
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
        const percentage = (currentAudio.currentTime / currentAudio.duration) * 100;
        document.getElementById('player-seek').value = percentage || 0;
    }
};

currentAudio.onended = function () {
    isPlaying = false;
    updateIcons('play');
};

window.seekAudio = function () {
    const slider = document.getElementById('player-seek');
    if (!isNaN(currentAudio.duration)) {
        currentAudio.currentTime = (slider.value / 100) * currentAudio.duration;
    }
};

window.setVolume = function (val) {
    currentAudio.volume = val / 100;
};

window.openAddDemoModal = function () {
    new bootstrap.Modal(document.getElementById('modalAddDemo')).show();
};

window.saveDemo = function () {
    const data = {
        Titulo: document.getElementById('newDemoTitle').value,
        UrlAudio: document.getElementById('newDemoUrl').value,
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
                Swal.fire({ title: '¡Subido!', text: 'El demo ha sido registrado.', icon: 'success', background: '#121212', color: '#fff' });
                loadDashboardData();
            } else {
                Swal.fire({ title: 'Error', text: 'Verifica que el ID Artista exista.', icon: 'error', background: '#121212', color: '#fff' });
            }
        });
};

function renderReleases(demos) {
    const grid = document.getElementById('releasesGrid');
    if (!grid) return;

    const releases = demos.filter(d => d.estado === 'Aprobado');
    if (releases.length === 0) {
        grid.innerHTML = '<div class="col-12 text-center text-secondary py-5">No hay lanzamientos programados</div>';
        return;
    }

    let htmlContent = '';
    releases.forEach(r => {
        const date = new Date(r.fechaEnvio);
        date.setDate(date.getDate() + 30);
        const fechaSalida = date.toLocaleDateString();

        htmlContent += `
            <div class="col-md-6 col-lg-4">
                <div class="stat-card h-100 p-3 d-flex align-items-center">
                    <div class="bg-cyan text-black rounded p-2 me-3 text-center" style="min-width: 60px; background: var(--cyan-primary);">
                        <div class="fw-bold fs-4 mb-0">${date.getDate()}</div>
                        <small class="text-uppercase" style="font-size:0.6rem">Lanzamiento</small>
                    </div>
                    <div>
                        <h6 class="fw-bold mb-1 text-white">${escapeHTML(r.titulo)}</h6>
                        <p class="text-secondary small mb-0">${escapeHTML(r.artistaNombre)}</p>
                        <span class="badge border border-info text-info mt-2">Programado: ${fechaSalida}</span>
                    </div>
                </div>
            </div>`;
    });
    grid.innerHTML = htmlContent;
}

window.generarReportePDF = function (tipo) {
    const { jsPDF } = window.jspdf;
    const doc = new jsPDF();

    doc.setFontSize(18);
    doc.text(`Reporte de Sistema: ${tipo.toUpperCase()}`, 14, 22);
    doc.setFontSize(11);
    doc.text(`Generado el: ${new Date().toLocaleString()}`, 14, 30);

    let headers = [['ID', 'Titulo', 'Artista', 'Estado']];
    let data = [];

    const cards = document.querySelectorAll('#demosGrid .grid-item');
    cards.forEach((c, index) => {
        const estado = c.querySelector('.status-badge').innerText;
        if (tipo === 'todos' ||
            (tipo === 'pendientes' && estado === 'Pendiente') ||
            (tipo === 'lanzamientos' && estado === 'Aprobado')) {

            data.push([
                index + 1,
                c.querySelector('.title-val').innerText,
                c.querySelector('.artist-val').innerText,
                estado
            ]);
        }
    });

    doc.autoTable({
        head: headers,
        body: data,
        startY: 40,
        theme: 'grid',
        styles: { fontSize: 10, cellPadding: 3 }
    });

    doc.save(`reporte_${tipo}_${Date.now()}.pdf`);
};