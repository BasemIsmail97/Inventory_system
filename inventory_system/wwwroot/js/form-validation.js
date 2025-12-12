/* ========================================
   FORM VALIDATION ENHANCEMENT
   Adds real-time validation and custom error messages
   ======================================== */

(function () {
    'use strict';

    // Initialize on DOM ready
    document.addEventListener('DOMContentLoaded', function () {
        enhanceValidation();
    });

    /* ========================================
       ENHANCE VALIDATION
       Adds real-time validation to all forms
       ======================================== */
    function enhanceValidation() {
        const forms = document.querySelectorAll('form');

        forms.forEach(form => {
            // Get all input and select elements
            const inputs = form.querySelectorAll('input, select');

            inputs.forEach(input => {
                // Validate on blur (when user leaves field)
                input.addEventListener('blur', function () {
                    validateField(this);
                });

                // Re-validate on input if field has error
                input.addEventListener('input', function () {
                    if (this.classList.contains('input-validation-error')) {
                        validateField(this);
                    }
                });
            });

            // Special handling for confirm password validation
            const password = form.querySelector('input[name="Password"]');
            const confirmPassword = form.querySelector('input[name="ConfirmPassword"]');

            if (password && confirmPassword) {
                // Check password match on input
                confirmPassword.addEventListener('input', function () {
                    if (this.value !== password.value) {
                        this.setCustomValidity('Passwords do not match');
                    } else {
                        this.setCustomValidity('');
                    }
                });

                // Also check when main password changes
                password.addEventListener('input', function () {
                    if (confirmPassword.value) {
                        confirmPassword.dispatchEvent(new Event('input'));
                    }
                });
            }

            // Special handling for NewPassword confirmation
            const newPassword = form.querySelector('input[name="NewPassword"]');
            const confirmNewPassword = form.querySelector('input[name="ConfirmNewPassword"]');

            if (newPassword && confirmNewPassword) {
                confirmNewPassword.addEventListener('input', function () {
                    if (this.value !== newPassword.value) {
                        this.setCustomValidity('Passwords do not match');
                    } else {
                        this.setCustomValidity('');
                    }
                });

                newPassword.addEventListener('input', function () {
                    if (confirmNewPassword.value) {
                        confirmNewPassword.dispatchEvent(new Event('input'));
                    }
                });
            }
        });
    }

    /* ========================================
       VALIDATE INDIVIDUAL FIELD
       Checks field validity and displays error message
       @param {HTMLElement} field - Input field to validate
       ======================================== */
    function validateField(field) {
        const errorSpan = field.parentElement.parentElement.querySelector('.field-validation-error');

        if (field.validity.valid) {
            // Field is valid - remove error styling
            field.classList.remove('input-validation-error');
            if (errorSpan) errorSpan.textContent = '';
        } else {
            // Field is invalid - show error
            field.classList.add('input-validation-error');
            if (errorSpan) {
                errorSpan.textContent = getErrorMessage(field);
            }
        }
    }

    /* ========================================
       GET ERROR MESSAGE
       Returns appropriate error message based on validation type
       @param {HTMLElement} field - Input field with error
       @returns {string} - Error message
       ======================================== */
    function getErrorMessage(field) {
        // Check different validation states
        if (field.validity.valueMissing) {
            return 'This field is required';
        }
        if (field.validity.typeMismatch) {
            if (field.type === 'email') {
                return 'Please enter a valid email address';
            }
        }
        if (field.validity.tooShort) {
            return `Must be at least ${field.minLength} characters`;
        }
        if (field.validity.tooLong) {
            return `Must be no more than ${field.maxLength} characters`;
        }
        if (field.validity.patternMismatch) {
            return 'Invalid format';
        }
        if (field.validity.customError) {
            return field.validationMessage;
        }

        return 'Invalid value';
    }

    /* ========================================
       EMAIL VALIDATION HELPER
       Additional email format validation
       @param {string} email - Email to validate
       @returns {boolean} - True if valid
       ======================================== */
    function isValidEmail(email) {
        const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return regex.test(email);
    }

})();
