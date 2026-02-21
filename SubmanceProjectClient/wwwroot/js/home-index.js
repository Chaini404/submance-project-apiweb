// wwwroot/js/home-index.js
// JavaScript para la página principal de SUBMANCE

(function () {
    'use strict';

    // Update Year in Footer
    const copyrightElement = document.getElementById('copyright-text');
    if (copyrightElement) {
        copyrightElement.innerHTML = `&copy; ${new Date().getFullYear()} SUBMANCE RECORDS`;
    }

    // Header scroll effect
    const header = document.getElementById('mainHeader');
    if (header) {
        window.addEventListener('scroll', function () {
            if (window.scrollY > 50) {
                header.classList.add('scrolled');
            } else {
                header.classList.remove('scrolled');
            }
        });
    }

    // Smooth Scroll Function
    window.scrollToSection = function (id) {
        const element = document.getElementById(id);
        if (element) {
            element.scrollIntoView({ behavior: 'smooth' });
        }
    };

    // ======================================================
    // Demo Form Submission — Conectado al Backend Real
    // ======================================================
    const demoForm = document.getElementById('demoFormTag');
    if (demoForm) {
        demoForm.addEventListener('submit', async function (event) {
            event.preventDefault();

            const btn = document.getElementById('btnSubmitDemo');
            const originalText = btn.innerHTML;

            // UI feedback — desactivar botón mientras se envía
            btn.innerHTML = '<i class="bi bi-hourglass-split me-2"></i> TRANSMITTING...';
            btn.style.opacity = '0.7';
            btn.disabled = true;

            // Recoger valores por ID (más confiable que name)
            const data = {
                ArtistName: document.getElementById('inputArtistName').value.trim(),
                TrackTitle: document.getElementById('inputTrackTitle').value.trim(),
                Email: document.getElementById('inputEmail').value.trim(),
                DemoLink: document.getElementById('inputLink').value.trim()
            };

            try {
                const response = await fetch('/Home/SubmitDemo', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(data)
                });

                const result = await response.json();

                // Cerrar el modal
                const modalElement = document.getElementById('demoModal');
                const modalInstance = bootstrap.Modal.getInstance(modalElement);
                if (modalInstance) modalInstance.hide();

                if (result.success) {
                    Swal.fire({
                        title: 'DEMO RECEIVED',
                        text: result.message || 'Your track is now in our A&R queue.',
                        icon: 'success',
                        background: '#111',
                        color: '#fff',
                        confirmButtonColor: '#00f3ff',
                        confirmButtonText: '<span style="color:black; font-weight:bold;">AWESOME</span>'
                    });
                } else {
                    Swal.fire({
                        title: 'ERROR',
                        text: result.message || 'Something went wrong.',
                        icon: 'error',
                        background: '#111',
                        color: '#fff',
                        confirmButtonColor: '#00f3ff'
                    });
                }
            } catch (err) {
                console.error('Submit error:', err);
                Swal.fire({
                    title: 'CONNECTION ERROR',
                    text: 'Could not reach the server. Please try again.',
                    icon: 'error',
                    background: '#111',
                    color: '#fff',
                    confirmButtonColor: '#00f3ff'
                });
            } finally {
                demoForm.reset();
                btn.innerHTML = originalText;
                btn.disabled = false;
                btn.style.opacity = '1';
            }
        });
    }
})();
