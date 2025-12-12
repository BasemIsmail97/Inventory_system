/* ========================================
   PASSWORD TOGGLE FUNCTIONALITY
   Adds show/hide password feature to password inputs
   ======================================== */

(function () {
    'use strict';

    // Initialize on DOM ready
    document.addEventListener('DOMContentLoaded', function () {
        initPasswordToggle();
    });

    /* ========================================
       INITIALIZE PASSWORD TOGGLE
       Adds toggle button to all password inputs
       ======================================== */
    function initPasswordToggle() {
        // Find all password input fields
        const passwordInputs = document.querySelectorAll('input[type="password"]');

        passwordInputs.forEach(input => {
            const inputGroup = input.parentElement;

            // Create toggle button with eye icon
            const toggleBtn = document.createElement('span');
            toggleBtn.className = 'password-toggle';
            toggleBtn.innerHTML = '<i class="fas fa-eye"></i>';
            toggleBtn.setAttribute('role', 'button');
            toggleBtn.setAttribute('aria-label', 'Show password');
            toggleBtn.setAttribute('tabindex', '0'); // Make keyboard accessible

            // Insert toggle button after input
            inputGroup.appendChild(toggleBtn);

            // Toggle password visibility on click
            toggleBtn.addEventListener('click', function () {
                togglePasswordVisibility(input, this);
            });

            // Toggle on Enter key press (accessibility)
            toggleBtn.addEventListener('keypress', function (e) {
                if (e.key === 'Enter' || e.key === ' ') {
                    e.preventDefault();
                    togglePasswordVisibility(input, this);
                }
            });
        });
    }

    /* ========================================
       TOGGLE PASSWORD VISIBILITY
       Switches between password and text input types
       @param {HTMLInputElement} input - Password input element
       @param {HTMLElement} toggleBtn - Toggle button element
       ======================================== */
    function togglePasswordVisibility(input, toggleBtn) {
        const icon = toggleBtn.querySelector('i');

        if (input.type === 'password') {
            // Show password
            input.type = 'text';
            icon.classList.remove('fa-eye');
            icon.classList.add('fa-eye-slash');
            toggleBtn.setAttribute('aria-label', 'Hide password');
        } else {
            // Hide password
            input.type = 'password';
            icon.classList.remove('fa-eye-slash');
            icon.classList.add('fa-eye');
            toggleBtn.setAttribute('aria-label', 'Show password');
        }
    }

})();
