// wwwroot/js/artist-portal.js
// JavaScript para el Portal de Artista de SUBMANCE

(function() {
    'use strict';

    const audio = document.getElementById('audio-main');

    // Play Track Function
    window.playTrack = function(url, title) {
        const titleElement = document.getElementById('p-title');
        const playerBar = document.getElementById('player-bar');
        const playIcon = document.getElementById('play-icon');

        if (titleElement && playerBar && audio) {
            titleElement.innerText = title;
            audio.src = url;
            audio.play();
            playerBar.style.display = 'block';
            
            if (playIcon) {
                playIcon.className = 'bi bi-pause-circle-fill';
            }
        }
    };

    // Toggle Play/Pause
    window.togglePlay = function() {
        const playIcon = document.getElementById('play-icon');
        
        if (audio) {
            if (audio.paused) {
                audio.play();
                if (playIcon) {
                    playIcon.className = 'bi bi-pause-circle-fill';
                }
            } else {
                audio.pause();
                if (playIcon) {
                    playIcon.className = 'bi bi-play-circle-fill';
                }
            }
        }
    };

    // Close Player
    window.closePlayer = function() {
        const playerBar = document.getElementById('player-bar');
        
        if (audio) {
            audio.pause();
        }
        
        if (playerBar) {
            playerBar.style.display = 'none';
        }
    };

    // Save New Demo
    window.save = async function() {
        const titulo = document.getElementById('t');
        const urlAudio = document.getElementById('u');
        
        if (!titulo || !urlAudio) {
            console.error('Form elements not found');
            return;
        }

        const data = {
            Titulo: titulo.value,
            UrlAudio: urlAudio.value,
            IdArtista: window.artistId || 0, // This should be set by the view
            IdGenero: 1,
            Estado: "Pendiente"
        };

        // Validation
        if (!data.Titulo || !data.UrlAudio) {
            Swal.fire({
                title: 'Error',
                text: 'Por favor completa todos los campos',
                icon: 'error',
                background: '#0a0a0a',
                color: '#fff',
                confirmButtonColor: '#ff4444'
            });
            return;
        }

        try {
            const response = await fetch('/ArtistPortal/Upload', {
                method: 'POST',
                headers: {'Content-Type': 'application/json'},
                body: JSON.stringify(data)
            });

            const result = await response.json();
            
            if (result.success) {
                // Close Modal
                const modalElement = document.getElementById('modalUpload');
                const modalInstance = bootstrap.Modal.getInstance(modalElement);
                if (modalInstance) {
                    modalInstance.hide();
                }

                // Success Alert
                Swal.fire({
                    title: '¡Recibido!',
                    text: 'Tu demo ha sido enviado a nuestro equipo A&R.',
                    icon: 'success',
                    background: '#0a0a0a',
                    color: '#fff',
                    confirmButtonColor: '#00f3ff',
                    confirmButtonText: '<span style="color:black; font-weight:bold;">GENIAL</span>'
                }).then(() => {
                    location.reload();
                });
            } else {
                throw new Error(result.message || 'Error al enviar el demo');
            }
        } catch (error) {
            Swal.fire({
                title: 'Error',
                text: error.message || 'Hubo un problema al enviar tu demo',
                icon: 'error',
                background: '#0a0a0a',
                color: '#fff',
                confirmButtonColor: '#ff4444'
            });
        }
    };
})();
