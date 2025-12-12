/* ========================================
   PASSWORD STRENGTH METER
   Calculates and displays password strength in real-time
   ======================================== */

(function () {
    'use strict';

    // Initialize on DOM ready
    document.addEventListener('DOMContentLoaded', function () {
        initPasswordStrength();
    });

    /* ========================================
       INITIALIZE PASSWORD STRENGTH METER
       Adds strength meter to password input fields
       ======================================== */
    function initPasswordStrength() {
        // Find password inputs (excluding ConfirmPassword)
        const passwordInputs = document.querySelectorAll('input[name="Password"], input[name="NewPassword"]');

        passwordInputs.forEach(input => {
            // Create strength meter element
            const meter = document.createElement('div');
            meter.className = 'password-strength';
            meter.innerHTML = `
                <div class="strength-bar"></div>
                <span class="strength-text"></span>
            `;

            // Insert meter after input's parent form-group
            input.parentElement.parentElement.appendChild(meter);

            // Check strength on every input change
            input.addEventListener('input', function () {
                const strength = calculatePasswordStrength(this.value);
                updateStrengthMeter(meter, strength);
            });
        });
    }

    /* ========================================
       CALCULATE PASSWORD STRENGTH
       Evaluates password based on multiple criteria
       @param {string} password - Password to evaluate
       @returns {Object} - Contains level, text, and strength percentage
       ======================================== */
    function calculatePasswordStrength(password) {
        // Return empty state if no password
        if (!password) return { level: 0, text: '' };

        let strength = 0;

        // Length checks (50 points total)
        if (password.length >= 8) strength += 25;  // Minimum acceptable length
        if (password.length >= 12) strength += 25; // Good length

        // Character variety checks (50 points total)
        if (/[a-z]/.test(password)) strength += 15; // Lowercase letters
        if (/[A-Z]/.test(password)) strength += 15; // Uppercase letters
        if (/[0-9]/.test(password)) strength += 10; // Numbers
        if (/[^a-zA-Z0-9]/.test(password)) strength += 10; // Special characters

        // Determine strength level and text
        let level, text;
        if (strength < 40) {
            level = 'weak';
            text = 'Weak';
        } else if (strength < 70) {
            level = 'medium';
            text = 'Medium';
        } else {
            level = 'strong';
            text = 'Strong';
        }

        return { level, text, strength };
    }

    /* ========================================
       UPDATE STRENGTH METER DISPLAY
       Updates visual representation of password strength
       @param {HTMLElement} meter - Meter container element
       @param {Object} strength - Strength data from calculation
       ======================================== */
    function updateStrengthMeter(meter, { level, text, strength }) {
        const bar = meter.querySelector('.strength-bar');
        const textEl = meter.querySelector('.strength-text');

        // Reset all strength classes
        meter.className = 'password-strength';

        if (level) {
            // Add appropriate strength class
            meter.classList.add(`strength-${level}`);

            // Update bar width
            bar.style.width = `${strength}%`;

            // Update text
            textEl.textContent = `Password Strength: ${text}`;
        } else {
            // Empty state
            bar.style.width = '0';
            textEl.textContent = '';
        }
    }

})();
