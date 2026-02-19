// wwwroot/js/home-index.js
// JavaScript para la página principal de SUBMANCE

(function() {
    'use strict';

    // Update Year in Footer
    const copyrightElement = document.getElementById('copyright-text');
    if (copyrightElement) {
        copyrightElement.innerHTML = `&copy; ${new Date().getFullYear()} SUBMANCE RECORDS`;
    }

    // Header scroll effect
    const header = document.getElementById('mainHeader');
    if (header) {
        window.addEventListener('scroll', function() {
            if (window.scrollY > 50) {
                header.classList.add('scrolled');
            } else {
                header.classList.remove('scrolled');
            }
        });
    }

    // Smooth Scroll Function
    window.scrollToSection = function(id) {
        const element = document.getElementById(id);
        if (element) {
            element.scrollIntoView({ behavior: 'smooth' });
        }
    };

    // Demo Form Submission
    const demoForm = document.getElementById('demoFormTag');
    if (demoForm) {
        demoForm.addEventListener('submit', function(event) {
            event.preventDefault();

            const btn = document.getElementById('btnSubmitDemo');
            const originalText = btn.innerHTML;

            // Simulate Loading
            btn.innerHTML = 'TRANSMITTING...';
            btn.style.opacity = '0.7';
            btn.disabled = true;

            // Simulate API Call delay
            setTimeout(() => {
                // Close Modal
                const modalElement = document.getElementById('demoModal');
                const modalInstance = bootstrap.Modal.getInstance(modalElement);
                if (modalInstance) {
                    modalInstance.hide();
                }

                // Success Alert
                Swal.fire({
                    title: 'DEMO RECEIVED',
                    text: 'Your track is now in our A&R queue.',
                    icon: 'success',
                    background: '#111',
                    color: '#fff',
                    confirmButtonColor: '#00f3ff',
                    confirmButtonText: '<span style="color:black; font-weight:bold;">AWESOME</span>'
                });

                // Reset Form
                demoForm.reset();
                btn.innerHTML = originalText;
                btn.disabled = false;
                btn.style.opacity = '1';
            }, 1500);
        });
    }
})();
