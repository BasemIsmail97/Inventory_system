/**
 * Site-wide JavaScript Utilities
 * Inventory Management System
 * 
 * This file contains common JavaScript functions used across all pages
 */

// Immediately Invoked Function Expression (IIFE) to avoid global namespace pollution
(function ($) {
    'use strict';

    /**
     * Initialize all components when document is ready
     */
    $(document).ready(function () {
        // Initialize all data tables
        initializeDataTables();

        // Initialize all tooltips
        initializeTooltips();

        // Initialize all popovers
        initializePopovers();

        // Auto-hide alerts after 5 seconds
        autoHideAlerts();

        // Initialize form validation
        initializeFormValidation();

        // Initialize delete confirmation modals
        initializeDeleteConfirmation();

        // Initialize search functionality
        initializeSearch();
    });

    /**
     * Initialize DataTables on all tables with class 'data-table'
     */
    function initializeDataTables() {
        if ($.fn.DataTable) {
            $('.data-table').each(function () {
                const $table = $(this);

                // Check if DataTable is already initialized
                if (!$.fn.DataTable.isDataTable($table)) {
                    $table.DataTable({
                        responsive: true,
                        pageLength: 10,
                        lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
                        language: {
                            search: "_INPUT_",
                            searchPlaceholder: "Search...",
                            lengthMenu: "Show _MENU_ entries",
                            info: "Showing _START_ to _END_ of _TOTAL_ entries",
                            infoEmpty: "No entries available",
                            infoFiltered: "(filtered from _MAX_ total entries)",
                            zeroRecords: "No matching records found",
                            emptyTable: "No data available in table"
                        },
                        dom: '<"row"<"col-sm-12 col-md-6"l><"col-sm-12 col-md-6"f>>' +
                            '<"row"<"col-sm-12"tr>>' +
                            '<"row"<"col-sm-12 col-md-5"i><"col-sm-12 col-md-7"p>>',
                        order: [[0, 'asc']] // Default sorting by first column
                    });
                }
            });
        }
    }

    /**
     * Initialize Bootstrap tooltips
     */
    function initializeTooltips() {
        const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
        tooltipTriggerList.map(function (tooltipTriggerEl) {
            return new bootstrap.Tooltip(tooltipTriggerEl);
        });
    }

    /**
     * Initialize Bootstrap popovers
     */
    function initializePopovers() {
        const popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'));
        popoverTriggerList.map(function (popoverTriggerEl) {
            return new bootstrap.Popover(popoverTriggerEl);
        });
    }

    /**
     * Auto-hide alert messages after specified duration
     * @param {number} duration - Duration in milliseconds (default: 5000)
     */
    function autoHideAlerts(duration = 5000) {
        $('.alert:not(.alert-permanent)').each(function () {
            const $alert = $(this);
            setTimeout(function () {
                $alert.fadeOut(400, function () {
                    $(this).remove();
                });
            }, duration);
        });
    }

    /**
     * Initialize client-side form validation
     */
    function initializeFormValidation() {
        // Add custom validation styles to all forms with .needs-validation class
        $('form.needs-validation').on('submit', function (e) {
            const form = this;

            if (!form.checkValidity()) {
                e.preventDefault();
                e.stopPropagation();
            }

            $(form).addClass('was-validated');
        });

        // Real-time validation for email fields
        $('input[type="email"]').on('blur', function () {
            validateEmail($(this));
        });

        // Real-time validation for phone fields
        $('input[type="tel"], input[data-type="phone"]').on('blur', function () {
            validatePhone($(this));
        });
    }

    /**
     * Validate email format
     * @param {jQuery} $input - Email input element
     * @returns {boolean} - True if valid, false otherwise
     */
    function validateEmail($input) {
        const email = $input.val();
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        const isValid = emailRegex.test(email);

        if (!isValid && email.length > 0) {
            $input.addClass('is-invalid');
            showValidationMessage($input, 'Please enter a valid email address.');
            return false;
        } else {
            $input.removeClass('is-invalid');
            return true;
        }
    }

    /**
     * Validate phone format
     * @param {jQuery} $input - Phone input element
     * @returns {boolean} - True if valid, false otherwise
     */
    function validatePhone($input) {
        const phone = $input.val();
        const phoneRegex = /^[\d\s\-\+\(\)]+$/;
        const isValid = phoneRegex.test(phone);

        if (!isValid && phone.length > 0) {
            $input.addClass('is-invalid');
            showValidationMessage($input, 'Please enter a valid phone number.');
            return false;
        } else {
            $input.removeClass('is-invalid');
            return true;
        }
    }

    /**
     * Show validation message for an input field
     * @param {jQuery} $input - Input element
     * @param {string} message - Validation message
     */
    function showValidationMessage($input, message) {
        let $feedback = $input.siblings('.invalid-feedback');

        if ($feedback.length === 0) {
            $feedback = $('<div class="invalid-feedback"></div>');
            $input.after($feedback);
        }

        $feedback.text(message);
    }

    /**
     * Initialize delete confirmation functionality
     */
    function initializeDeleteConfirmation() {
        // Handle delete button clicks
        $(document).on('click', '[data-delete-confirm]', function (e) {
            e.preventDefault();
            const $btn = $(this);
            const itemName = $btn.data('item-name') || 'this item';
            const deleteUrl = $btn.attr('href') || $btn.data('url');

            showDeleteConfirmationModal(itemName, deleteUrl);
        });
    }

    /**
     * Show delete confirmation modal
     * @param {string} itemName - Name of item to delete
     * @param {string} deleteUrl - URL for delete action
     */
    function showDeleteConfirmationModal(itemName, deleteUrl) {
        const modalHtml = `
            <div class="modal fade" id="deleteConfirmModal" tabindex="-1">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header bg-danger text-white">
                            <h5 class="modal-title">
                                <i class="fas fa-exclamation-triangle me-2"></i>Confirm Delete
                            </h5>
                            <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"></button>
                        </div>
                        <div class="modal-body">
                            <p>Are you sure you want to delete <strong>${itemName}</strong>?</p>
                            <p class="text-muted mb-0">This action cannot be undone.</p>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">
                                <i class="fas fa-times me-1"></i>Cancel
                            </button>
                            <form method="post" action="${deleteUrl}" style="display: inline;">
                                <input type="hidden" name="__RequestVerificationToken" value="${getAntiForgeryToken()}" />
                                <button type="submit" class="btn btn-danger">
                                    <i class="fas fa-trash me-1"></i>Delete
                                </button>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        `;

        // Remove existing modal if present
        $('#deleteConfirmModal').remove();

        // Add modal to body and show
        $('body').append(modalHtml);
        const modal = new bootstrap.Modal(document.getElementById('deleteConfirmModal'));
        modal.show();

        // Remove modal from DOM after it's hidden
        $('#deleteConfirmModal').on('hidden.bs.modal', function () {
            $(this).remove();
        });
    }

    /**
     * Get anti-forgery token from page
     * @returns {string} - Anti-forgery token value
     */
    function getAntiForgeryToken() {
        return $('input[name="__RequestVerificationToken"]').val() || '';
    }

    /**
     * Initialize search functionality
     */
    function initializeSearch() {
        // Debounce search input
        let searchTimeout;
        $('.search-input').on('keyup', function () {
            clearTimeout(searchTimeout);
            const $input = $(this);
            const searchTerm = $input.val();

            searchTimeout = setTimeout(function () {
                performSearch(searchTerm);
            }, 300); // Wait 300ms after user stops typing
        });
    }

    /**
     * Perform search operation
     * @param {string} searchTerm - Search term entered by user
     */
    function performSearch(searchTerm) {
        // If DataTable is initialized, use its search functionality
        const $table = $('.data-table').DataTable();
        if ($table) {
            $table.search(searchTerm).draw();
        }
    }

    /**
     * Show loading spinner
     * @param {string} message - Loading message (optional)
     */
    window.showLoading = function (message = 'Loading...') {
        const loadingHtml = `
            <div id="loadingOverlay" style="position: fixed; top: 0; left: 0; width: 100%; height: 100%; 
                 background: rgba(0,0,0,0.5); z-index: 9999; display: flex; align-items: center; 
                 justify-content: center;">
                <div class="text-center text-white">
                    <div class="spinner-border mb-3" role="status" style="width: 3rem; height: 3rem;">
                        <span class="visually-hidden">Loading...</span>
                    </div>
                    <div>${message}</div>
                </div>
            </div>
        `;

        $('#loadingOverlay').remove();
        $('body').append(loadingHtml);
    };

    /**
     * Hide loading spinner
     */
    window.hideLoading = function () {
        $('#loadingOverlay').fadeOut(300, function () {
            $(this).remove();
        });
    };

    /**
     * Show toast notification
     * @param {string} message - Toast message
     * @param {string} type - Toast type (success, error, warning, info)
     */
    window.showToast = function (message, type = 'info') {
        const iconMap = {
            success: 'fa-check-circle',
            error: 'fa-exclamation-circle',
            warning: 'fa-exclamation-triangle',
            info: 'fa-info-circle'
        };

        const bgMap = {
            success: 'bg-success',
            error: 'bg-danger',
            warning: 'bg-warning',
            info: 'bg-info'
        };

        const toastHtml = `
            <div class="toast align-items-center text-white ${bgMap[type]} border-0" role="alert" 
                 style="position: fixed; top: 20px; right: 20px; z-index: 9999;">
                <div class="d-flex">
                    <div class="toast-body">
                        <i class="fas ${iconMap[type]} me-2"></i>${message}
                    </div>
                    <button type="button" class="btn-close btn-close-white me-2 m-auto" 
                            data-bs-dismiss="toast"></button>
                </div>
            </div>
        `;

        const $toast = $(toastHtml);
        $('body').append($toast);

        const toast = new bootstrap.Toast($toast[0], { delay: 3000 });
        toast.show();

        $toast.on('hidden.bs.toast', function () {
            $(this).remove();
        });
    };

    /**
     * Format currency value
     * @param {number} value - Numeric value to format
     * @param {string} currency - Currency symbol (default: $)
     * @returns {string} - Formatted currency string
     */
    window.formatCurrency = function (value, currency = '$') {
        return currency + parseFloat(value).toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
    };

    /**
     * Format date to locale string
     * @param {string|Date} date - Date to format
     * @returns {string} - Formatted date string
     */
    window.formatDate = function (date) {
        const d = new Date(date);
        return d.toLocaleDateString() + ' ' + d.toLocaleTimeString();
    };

    /**
     * Confirm action with custom message
     * @param {string} message - Confirmation message
     * @param {function} callback - Function to execute on confirmation
     */
    window.confirmAction = function (message, callback) {
        if (confirm(message)) {
            callback();
        }
    };

})(jQuery);