document.addEventListener("DOMContentLoaded", () => {
    // 1. Gestión de Tema
    const htmlEl = document.documentElement;
    const themeBtn = document.getElementById('theme-toggle');
    const savedTheme = localStorage.getItem('submance_theme') || 'dark'; // Dark por defecto para Submance

    htmlEl.setAttribute('data-theme', savedTheme);

    themeBtn.addEventListener('click', () => {
        const currentTheme = htmlEl.getAttribute('data-theme');
        const newTheme = currentTheme === 'dark' ? 'light' : 'dark';
        htmlEl.setAttribute('data-theme', newTheme);
        localStorage.setItem('submance_theme', newTheme);
    });

    // 2. Control de Audio
    const audio = document.getElementById('audio-source');
    const playerBar = document.getElementById('global-player');
    const seekSlider = document.getElementById('player-seek');
    const discIcon = document.querySelector('.player-disc');
    let activeTrackId = null;

    window.playTrack = function (fileUrl, title, id) {
        if (activeTrackId === id) {
            window.togglePlay();
            return;
        }

        // Resetear botón anterior
        if (activeTrackId) {
            document.getElementById(`play-btn-${activeTrackId}`).className = 'bi bi-play-circle';
        }

        // Cargar nueva pista
        document.getElementById('player-title').innerText = title;
        audio.src = `/uploads/songs/${fileUrl}`; // Modificar según ruteo estático en ASP.NET
        activeTrackId = id;

        playerBar.classList.remove('d-none');
        audio.play().then(() => updateUIState(true)).catch(e => console.error("Audio block:", e));
    };

    window.togglePlay = function () {
        if (!activeTrackId) return;

        if (audio.paused) {
            audio.play();
            updateUIState(true);
        } else {
            audio.pause();
            updateUIState(false);
        }
    };

    window.closePlayer = function () {
        audio.pause();
        updateUIState(false);
        playerBar.classList.add('d-none');
        if (activeTrackId) {
            document.getElementById(`play-btn-${activeTrackId}`).className = 'bi bi-play-circle';
        }
        activeTrackId = null;
    };

    function updateUIState(isPlaying) {
        if (!activeTrackId) return;

        // Actualizar botón en la tabla
        const rowBtn = document.getElementById(`play-btn-${activeTrackId}`);
        rowBtn.className = isPlaying ? 'bi bi-pause-circle-fill' : 'bi bi-play-circle';

        // Actualizar botón en el reproductor
        const mainBtn = document.getElementById('play-icon-main');
        mainBtn.className = isPlaying ? 'bi bi-pause-fill' : 'bi bi-play-fill';

        // Animar disco
        if (isPlaying) {
            discIcon.classList.add('playing');
        } else {
            discIcon.classList.remove('playing');
        }
    }

    // Actualización de progreso
    audio.addEventListener('timeupdate', () => {
        if (!isNaN(audio.duration)) {
            const percent = (audio.currentTime / audio.duration) * 100;
            seekSlider.value = percent;
        }
    });

    // Seek manual
    seekSlider.addEventListener('input', (e) => {
        if (!isNaN(audio.duration)) {
            const seekTo = audio.duration * (e.target.value / 100);
            audio.currentTime = seekTo;
        }
    });

    // Resetear al terminar la canción
    audio.addEventListener('ended', () => {
        updateUIState(false);
        seekSlider.value = 0;
    });
});