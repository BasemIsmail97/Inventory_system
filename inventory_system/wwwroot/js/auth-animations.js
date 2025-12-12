/* ========================================
   AUTH ANIMATIONS & EFFECTS
   Handles page animations, input effects, and toast notifications
   ======================================== */

(function () {
    'use strict';

    // Initialize all animations when DOM is ready
    document.addEventListener('DOMContentLoaded', function () {
        initAnimations();
        initInputEffects();
        initToastNotifications();
    });

    /* ========================================
       ANIMATE ELEMENTS ON PAGE LOAD
       Adds staggered fade-in animation to form elements
       ======================================== */
    function initAnimations() {
        const elements = document.querySelectorAll('.form-group, .btn, .auth-link');

        elements.forEach((el, index) => {
            // Initially hide elements
            el.style.opacity = '0';
            el.style.transform = 'translateY(20px)';

            // Animate in with progressive delay
            setTimeout(() => {
                el.style.transition = 'all 0.5s ease';
                el.style.opacity = '1';
                el.style.transform = 'translateY(0)';
            }, index * 100); // 100ms delay between each element
        });
    }

    /* ========================================
       INPUT FOCUS EFFECTS
       Adds visual feedback when inputs are focused
       ======================================== */
    function initInputEffects() {
        const inputs = document.querySelectorAll('.form-control');

        inputs.forEach(input => {
            // Add focused class on focus
            input.addEventListener('focus', function () {
                this.parentElement.classList.add('focused');
            });

            // Remove focused class on blur if empty
            input.addEventListener('blur', function () {
                if (!this.value) {
                    this.parentElement.classList.remove('focused');
                }
            });

            // Detect browser autofill
            input.addEventListener('animationstart', function (e) {
                if (e.animationName === 'onAutoFillStart') {
                    this.parentElement.classList.add('focused');
                }
            });
        });
    }

    /* ========================================
       TOAST NOTIFICATIONS
       Check for TempData messages and display toasts
       ======================================== */
    function initToastNotifications() {
        // Look for success message from server
        const successMsg = document.querySelector('[data-success-message]');
        const errorMsg = document.querySelector('[data-error-message]');

        if (successMsg) {
            showToast(successMsg.dataset.successMessage, 'success');
        }

        if (errorMsg) {
            showToast(errorMsg.dataset.errorMessage, 'error');
        }
    }

    /* ========================================
       SHOW TOAST NOTIFICATION
       @param {string} message - Message to display
       @param {string} type - Type: 'success', 'error', 'warning', 'info'
       ======================================== */
    window.showToast = function (message, type = 'info') {
        const container = document.getElementById('toast-container');
        if (!container) return;

        // Create toast element
        const toast = document.createElement('div');
        toast.className = `toast toast-${type}`;

        // Get appropriate icon for toast type
        const icon = getToastIcon(type);
        toast.innerHTML = `
            <i class="fas ${icon}"></i>
            <span>${message}</span>
        `;

        // Add to container
        container.appendChild(toast);

        // Auto remove after 3 seconds
        setTimeout(() => {
            toast.style.animation = 'slideUp 0.3s ease reverse';
            setTimeout(() => toast.remove(), 300);
        }, 3000);
    };

    /* ========================================
       GET TOAST ICON
       Returns Font Awesome icon class based on toast type
       ======================================== */
    function getToastIcon(type) {
        const icons = {
            success: 'fa-check-circle',
            error: 'fa-times-circle',
            warning: 'fa-exclamation-triangle',
            info: 'fa-info-circle'
        };
        return icons[type] || icons.info;
    }

    /* ========================================
       SET BUTTON LOADING STATE
       Shows loading spinner on button
       @param {HTMLElement} button - Button element
       @param {boolean} loading - Loading state
       ======================================== */
    window.setButtonLoading = function (button, loading) {
        if (loading) {
            button.classList.add('loading');
            button.disabled = true;
            button.dataset.originalText = button.textContent;
            button.textContent = 'Loading...';
        } else {
            button.classList.remove('loading');
            button.disabled = false;
            button.textContent = button.dataset.originalText;
        }
    };

    /* ========================================
       FORM SUBMIT WITH LOADING STATE
       Automatically show loading state on form submission
       ======================================== */
    document.querySelectorAll('form').forEach(form => {
        form.addEventListener('submit', function (e) {
            const submitBtn = this.querySelector('button[type="submit"]');
            // Only show loading if form is valid
            if (submitBtn && this.checkValidity()) {
                setButtonLoading(submitBtn, true);
            }
        });
    });

})();
