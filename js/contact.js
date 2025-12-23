/* ========================================
   FoodieExpress - Contact Page JavaScript
   ======================================== */

// ========================================
// Form Validation
// ========================================

function validateEmail(email) {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
}

function validatePhone(phone) {
    if (!phone) return true; // Phone is optional
    const phoneRegex = /^[\d\s\-\+\(\)]{10,}$/;
    return phoneRegex.test(phone);
}

function validateForm(formData) {
    const errors = [];

    // First Name validation
    if (!formData.firstName || formData.firstName.trim().length < 2) {
        errors.push({ field: 'firstName', message: 'First name must be at least 2 characters' });
    }

    // Last Name validation
    if (!formData.lastName || formData.lastName.trim().length < 2) {
        errors.push({ field: 'lastName', message: 'Last name must be at least 2 characters' });
    }

    // Email validation
    if (!formData.email || !validateEmail(formData.email)) {
        errors.push({ field: 'email', message: 'Please enter a valid email address' });
    }

    // Phone validation (optional but must be valid if provided)
    if (formData.phone && !validatePhone(formData.phone)) {
        errors.push({ field: 'phone', message: 'Please enter a valid phone number' });
    }

    // Subject validation
    if (!formData.subject) {
        errors.push({ field: 'subject', message: 'Please select a subject' });
    }

    // Message validation
    if (!formData.message || formData.message.trim().length < 10) {
        errors.push({ field: 'message', message: 'Message must be at least 10 characters' });
    }

    return errors;
}

function showFieldError(fieldId, message) {
    const field = document.getElementById(fieldId);
    if (field) {
        field.classList.add('is-invalid');
        const feedback = field.nextElementSibling;
        if (feedback && feedback.classList.contains('invalid-feedback')) {
            feedback.textContent = message;
        }
    }
}

function clearFieldError(fieldId) {
    const field = document.getElementById(fieldId);
    if (field) {
        field.classList.remove('is-invalid');
        field.classList.add('is-valid');
    }
}

function clearAllErrors() {
    const fields = ['firstName', 'lastName', 'email', 'phone', 'subject', 'message'];
    fields.forEach(fieldId => {
        const field = document.getElementById(fieldId);
        if (field) {
            field.classList.remove('is-invalid', 'is-valid');
        }
    });
}

// ========================================
// Form Submission
// ========================================

async function handleContactFormSubmit(event) {
    event.preventDefault();
    
    const form = event.target;
    const submitBtn = form.querySelector('button[type="submit"]');
    
    // Clear previous errors
    clearAllErrors();

    // Gather form data
    const formData = {
        firstName: document.getElementById('firstName').value,
        lastName: document.getElementById('lastName').value,
        email: document.getElementById('email').value,
        phone: document.getElementById('phone').value,
        subject: document.getElementById('subject').value,
        message: document.getElementById('message').value,
        newsletter: document.getElementById('newsletter').checked
    };

    // Validate form
    const errors = validateForm(formData);
    
    if (errors.length > 0) {
        // Show errors
        errors.forEach(error => {
            showFieldError(error.field, error.message);
        });
        
        // Scroll to first error
        const firstErrorField = document.getElementById(errors[0].field);
        if (firstErrorField) {
            firstErrorField.focus();
            firstErrorField.scrollIntoView({ behavior: 'smooth', block: 'center' });
        }
        
        showToast('Please fix the errors in the form', 'danger');
        return;
    }

    // Show loading state
    submitBtn.disabled = true;
    submitBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>Sending...';

    try {
        // Submit to backend
        const response = await submitContactForm(formData);
        
        // Show success modal
        const successModal = new bootstrap.Modal(document.getElementById('contactSuccessModal'));
        successModal.show();
        
        // Reset form
        form.reset();
        clearAllErrors();
        
    } catch (error) {
        console.error('Contact form error:', error);
        showToast('Failed to send message. Please try again.', 'danger');
    } finally {
        // Reset button state
        submitBtn.disabled = false;
        submitBtn.innerHTML = '<i class="fas fa-paper-plane me-2"></i>Send Message';
    }
}

// ========================================
// Real-time Validation
// ========================================

function initRealTimeValidation() {
    // First Name
    const firstName = document.getElementById('firstName');
    if (firstName) {
        firstName.addEventListener('blur', function() {
            if (this.value.trim().length >= 2) {
                clearFieldError('firstName');
            } else if (this.value.trim().length > 0) {
                showFieldError('firstName', 'First name must be at least 2 characters');
            }
        });
    }

    // Last Name
    const lastName = document.getElementById('lastName');
    if (lastName) {
        lastName.addEventListener('blur', function() {
            if (this.value.trim().length >= 2) {
                clearFieldError('lastName');
            } else if (this.value.trim().length > 0) {
                showFieldError('lastName', 'Last name must be at least 2 characters');
            }
        });
    }

    // Email
    const email = document.getElementById('email');
    if (email) {
        email.addEventListener('blur', function() {
            if (validateEmail(this.value)) {
                clearFieldError('email');
            } else if (this.value.length > 0) {
                showFieldError('email', 'Please enter a valid email address');
            }
        });
    }

    // Phone
    const phone = document.getElementById('phone');
    if (phone) {
        phone.addEventListener('blur', function() {
            if (validatePhone(this.value)) {
                clearFieldError('phone');
            } else if (this.value.length > 0) {
                showFieldError('phone', 'Please enter a valid phone number');
            }
        });
    }

    // Subject
    const subject = document.getElementById('subject');
    if (subject) {
        subject.addEventListener('change', function() {
            if (this.value) {
                clearFieldError('subject');
            }
        });
    }

    // Message
    const message = document.getElementById('message');
    if (message) {
        message.addEventListener('blur', function() {
            if (this.value.trim().length >= 10) {
                clearFieldError('message');
            } else if (this.value.trim().length > 0) {
                showFieldError('message', 'Message must be at least 10 characters');
            }
        });

        // Character count
        message.addEventListener('input', function() {
            const charCount = this.value.length;
            let countElement = document.getElementById('messageCharCount');
            
            if (!countElement) {
                countElement = document.createElement('small');
                countElement.id = 'messageCharCount';
                countElement.className = 'text-muted';
                this.parentElement.appendChild(countElement);
            }
            
            countElement.textContent = `${charCount}/500 characters`;
            
            if (charCount > 500) {
                countElement.classList.remove('text-muted');
                countElement.classList.add('text-danger');
            } else {
                countElement.classList.remove('text-danger');
                countElement.classList.add('text-muted');
            }
        });
    }
}

// ========================================
// Phone Number Formatting
// ========================================

function initPhoneFormatting() {
    const phone = document.getElementById('phone');
    if (!phone) return;

    phone.addEventListener('input', function(e) {
        // Remove non-digit characters
        let value = this.value.replace(/\D/g, '');
        
        // Format as (XXX) XXX-XXXX
        if (value.length > 0) {
            if (value.length <= 3) {
                value = `(${value}`;
            } else if (value.length <= 6) {
                value = `(${value.slice(0, 3)}) ${value.slice(3)}`;
            } else {
                value = `(${value.slice(0, 3)}) ${value.slice(3, 6)}-${value.slice(6, 10)}`;
            }
        }
        
        this.value = value;
    });
}

// ========================================
// Initialize Contact Page
// ========================================

document.addEventListener('DOMContentLoaded', function() {
    // Initialize contact form
    const contactForm = document.getElementById('contactForm');
    if (contactForm) {
        contactForm.addEventListener('submit', handleContactFormSubmit);
    }

    // Initialize real-time validation
    initRealTimeValidation();

    // Initialize phone formatting
    initPhoneFormatting();

    console.log('Contact Page Initialized!');
});
