// wwwroot/js/artist-portal.js
(function () {
    'use strict';

    const audio = document.getElementById('audio-main');

    // ─── Utilidades ──────────────────────────────────────────────────────────

    function isValidUrl(url) {
        try {
            const p = new URL(url);
            return ['http:', 'https:'].includes(p.protocol);
        } catch { return false; }
    }

    const STREAMING_DOMAINS = [
        'soundcloud.com', 'youtube.com', 'youtu.be',
        'spotify.com', 'music.apple.com', 'deezer.com'
    ];

    function isStreaming(url) {
        try {
            const host = new URL(url).hostname.replace('www.', '');
            return STREAMING_DOMAINS.some(d => host.includes(d));
        } catch { return false; }
    }

    // ─── Player ──────────────────────────────────────────────────────────────

    window.playTrack = function (url, title) {
        const titleEl = document.getElementById('p-title');
        const playerBar = document.getElementById('player-bar');
        if (!audio || !titleEl || !playerBar) return;

        if (isStreaming(url)) {
            Swal.fire({
                title: 'Preview no disponible',
                html: `URL de plataforma de streaming detectada.<br>
                       <a href="${url}" target="_blank" rel="noopener" class="text-info">Abrir link ↗</a>`,
                icon: 'info',
                background: '#0a0a0a',
                color: '#fff',
                confirmButtonColor: '#00e5ff'
            });
            return;
        }

        titleEl.innerText = title;
        audio.src = url;
        audio.play().catch(() => {
            Swal.fire({
                title: 'No se pudo reproducir',
                text: 'La URL debe apuntar directamente a un archivo .mp3 o .wav.',
                icon: 'error', background: '#0a0a0a', color: '#fff'
            });
        });
        playerBar.style.display = 'block';
    };

    window.closePlayer = function () {
        if (audio) audio.pause();
        const playerBar = document.getElementById('player-bar');
        if (playerBar) playerBar.style.display = 'none';
    };

    // ─── Preview del modal ───────────────────────────────────────────────────
    // CLAVE: escuchar 'shown.bs.modal' de Bootstrap.
    // El modal está en el DOM pero oculto — los inputs pueden no responder
    // a eventos hasta que Bootstrap lo muestre completamente.

    const modalEl = document.getElementById('modalUpload');

    if (modalEl) {
        let previewInitialized = false;

        modalEl.addEventListener('shown.bs.modal', function () {
            // Solo inicializar una vez para no duplicar listeners
            if (previewInitialized) return;
            previewInitialized = true;

            const urlInput = modalEl.querySelector('input[name="UrlAudio"]');
            const tituloInput = modalEl.querySelector('input[name="Titulo"]');
            const previewBtn = modalEl.querySelector('#btn-preview-upload');
            const urlHint = modalEl.querySelector('#url-hint');

            if (!urlInput || !previewBtn) {
                console.warn('[ArtistPortal] No se encontraron los campos del modal.');
                return;
            }

            urlInput.addEventListener('input', () => {
                const url = urlInput.value.trim();
                const valid = isValidUrl(url);
                const streaming = isStreaming(url);

                previewBtn.style.display = valid ? 'inline-flex' : 'none';

                if (!urlHint) return;
                if (!url) {
                    urlHint.textContent = '';
                    urlHint.className = 'small mt-1';
                } else if (streaming) {
                    urlHint.textContent = '⚠️ Link de streaming — se guardará pero sin preview de audio.';
                    urlHint.className = 'text-warning small mt-1';
                } else if (valid) {
                    urlHint.textContent = '✅ URL directa — preview disponible.';
                    urlHint.className = 'text-success small mt-1';
                } else {
                    urlHint.textContent = '❌ URL inválida.';
                    urlHint.className = 'text-danger small mt-1';
                }
            });

            previewBtn.addEventListener('click', () => {
                const url = urlInput.value.trim();
                const titulo = tituloInput?.value.trim() || 'Sin título';
                if (isValidUrl(url)) playTrack(url, 'Preview — ' + titulo);
            });
        });

        // Limpiar UI al cerrar
        modalEl.addEventListener('hidden.bs.modal', function () {
            const urlHint = modalEl.querySelector('#url-hint');
            const previewBtn = modalEl.querySelector('#btn-preview-upload');
            const urlInput = modalEl.querySelector('input[name="UrlAudio"]');
            if (urlHint) { urlHint.textContent = ''; urlHint.className = 'small mt-1'; }
            if (previewBtn) { previewBtn.style.display = 'none'; }
            if (urlInput) { urlInput.value = ''; }
        });
    }

    // ─── Submit ──────────────────────────────────────────────────────────────

    window.save = async function () {
        const form = document.getElementById('formUpload');
        const titulo = form?.querySelector('input[name="Titulo"]')?.value.trim();
        const urlAudio = form?.querySelector('input[name="UrlAudio"]')?.value.trim();

        if (!titulo) {
            return Swal.fire({
                title: 'Falta el título', icon: 'warning',
                background: '#0a0a0a', color: '#fff'
            });
        }
        if (!isValidUrl(urlAudio)) {
            return Swal.fire({
                title: 'URL inválida',
                text: 'Ingresa una URL válida (http:// o https://)',
                icon: 'warning', background: '#0a0a0a', color: '#fff'
            });
        }

        if (isStreaming(urlAudio)) {
            const { isConfirmed } = await Swal.fire({
                title: '¿Guardar link de streaming?',
                html: 'Es una URL de SoundCloud u otra plataforma.<br>No habrá preview en el portal, pero el link quedará guardado.',
                icon: 'question',
                showCancelButton: true,
                confirmButtonText: 'Sí, guardar',
                cancelButtonText: 'Cancelar',
                background: '#0a0a0a', color: '#fff',
                confirmButtonColor: '#00e5ff'
            });
            if (!isConfirmed) return;
        }

        try {
            const response = await fetch('/ArtistPortal/Upload', {
                method: 'POST',
                body: new FormData(form)
            });
            const result = await response.json();

            if (result.success) {
                Swal.fire({
                    title: '¡Enviado!',
                    text: 'Tu demo fue recibido por A&R.',
                    icon: 'success', background: '#0a0a0a', color: '#fff',
                    confirmButtonColor: '#00e5ff'
                }).then(() => location.reload());
            } else {
                throw new Error(result.message || 'Error desconocido del servidor.');
            }
        } catch (error) {
            Swal.fire({
                title: 'Error al enviar',
                text: error.message,
                icon: 'error', background: '#0a0a0a', color: '#fff'
            });
        }
    };

})();