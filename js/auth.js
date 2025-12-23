// Authentication JavaScript for FoodieExpress
// Handles login, logout, and user session management

const API_URL = 'http://localhost:5000/api';

// Auth state management
const Auth = {
    // Check if user is logged in
    isLoggedIn() {
        const user = localStorage.getItem('foodieexpress_user');
        return user !== null;
    },

    // Get current user
    getUser() {
        const user = localStorage.getItem('foodieexpress_user');
        return user ? JSON.parse(user) : null;
    },

    // Save user to localStorage
    setUser(user) {
        localStorage.setItem('foodieexpress_user', JSON.stringify(user));
    },

    // Remove user from localStorage
    removeUser() {
        localStorage.removeItem('foodieexpress_user');
    },

    // Login or register user
    async login(fullName, email, phone = '', address = '') {
        try {
            const response = await fetch(`${API_URL}/users/login`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    fullName: fullName,
                    email: email,
                    phone: phone,
                    address: address
                })
            });

            const data = await response.json();

            if (data.success && data.user) {
                this.setUser(data.user);
                return { success: true, message: data.message, user: data.user };
            } else {
                return { success: false, message: data.message || 'Login failed' };
            }
        } catch (error) {
            console.error('Login error:', error);
            return { success: false, message: 'Unable to connect to server. Please try again.' };
        }
    },

    // Logout user
    logout() {
        this.removeUser();
        updateNavbarAuth();
        // Redirect to login page if on a protected page
        if (window.location.pathname.includes('login.html')) {
            showLoginForm();
        }
    },

    // Get user's order history
    async getOrderHistory() {
        const user = this.getUser();
        if (!user) return [];

        try {
            const response = await fetch(`${API_URL}/users/${user.id}/orders`);
            if (response.ok) {
                return await response.json();
            }
            return [];
        } catch (error) {
            console.error('Error fetching order history:', error);
            return [];
        }
    }
};

// Update navbar to show login status
function updateNavbarAuth() {
    const loginNavText = document.getElementById('loginNavText');
    const loginNavBtn = document.getElementById('loginNavBtn');
    
    if (loginNavText && loginNavBtn) {
        if (Auth.isLoggedIn()) {
            const user = Auth.getUser();
            const firstName = user.fullName.split(' ')[0];
            loginNavText.innerHTML = `<span class="d-none d-md-inline">Hi, </span>${firstName}`;
            loginNavBtn.classList.add('text-warning');
        } else {
            loginNavText.textContent = 'Login';
            loginNavBtn.classList.remove('text-warning');
        }
    }
}

// Show login form
function showLoginForm() {
    const loginFormCard = document.getElementById('loginFormCard');
    const userProfileCard = document.getElementById('userProfileCard');
    
    if (loginFormCard) loginFormCard.style.display = 'block';
    if (userProfileCard) userProfileCard.style.display = 'none';
}

// Show user profile
async function showUserProfile() {
    const loginFormCard = document.getElementById('loginFormCard');
    const userProfileCard = document.getElementById('userProfileCard');
    
    if (loginFormCard) loginFormCard.style.display = 'none';
    if (userProfileCard) userProfileCard.style.display = 'block';
    
    const user = Auth.getUser();
    if (user) {
        // Update profile info
        document.getElementById('welcomeUserName').textContent = `Welcome, ${user.fullName.split(' ')[0]}!`;
        document.getElementById('userEmail').textContent = user.email;
        
        // Format member since date
        const memberDate = new Date(user.createdAt);
        document.getElementById('memberSince').textContent = memberDate.toLocaleDateString('en-US', { 
            month: 'short', 
            year: 'numeric' 
        });
        
        // Load order history
        await loadOrderHistory();
    }
}

// Load order history
async function loadOrderHistory() {
    const orderHistoryList = document.getElementById('orderHistoryList');
    if (!orderHistoryList) return;

    const orders = await Auth.getOrderHistory();
    document.getElementById('totalOrders').textContent = orders.length;

    if (orders.length === 0) {
        orderHistoryList.innerHTML = `
            <div class="text-center py-5">
                <div style="width: 80px; height: 80px; background: linear-gradient(135deg, #fff5f0 0%, #fff8e8 100%); border-radius: 50%; display: flex; align-items: center; justify-content: center; margin: 0 auto 20px; border: 2px dashed #ff6b35;">
                    <i class="fas fa-shopping-bag fa-2x" style="color: #ff6b35;"></i>
                </div>
                <h5 style="color: #1a1a2e; font-weight: 700;">No Orders Yet</h5>
                <p class="text-muted mb-4">Start your culinary journey with us!</p>
                <a href="menu.html" class="btn btn-login text-white px-4">
                    <i class="fas fa-utensils me-2"></i>Explore Menu
                </a>
            </div>
        `;
        return;
    }

    orderHistoryList.innerHTML = orders.map(order => {
        const orderDate = new Date(order.orderDate);
        const statusClass = getStatusClass(order.status);
        const itemCount = order.items ? order.items.length : 0;
        
        // Get food item names
        const foodItems = order.items ? order.items.map(item => 
            `<span class="food-name"><i class="fas fa-utensils"></i>${item.itemName} x${item.quantity}</span>`
        ).join('') : '';
        
        return `
            <div class="order-item">
                <div class="d-flex justify-content-between align-items-center">
                    <div>
                        <span class="order-number">${order.orderNumber}</span>
                        <span class="badge ${statusClass} ms-2">${order.status}</span>
                    </div>
                    <span class="order-total">$${order.totalAmount.toFixed(2)}</span>
                </div>
                <div class="text-muted small mt-2">
                    <i class="fas fa-calendar-alt me-1"></i>${orderDate.toLocaleDateString()}
                    <span class="ms-3"><i class="fas fa-box me-1"></i>${itemCount} item${itemCount !== 1 ? 's' : ''}</span>
                </div>
                ${foodItems ? `<div class="order-items-list">${foodItems}</div>` : ''}
            </div>
        `;
    }).join('');
}

// Get status badge class
function getStatusClass(status) {
    const statusClasses = {
        'Pending': 'bg-warning text-dark',
        'Confirmed': 'bg-info',
        'Preparing': 'bg-primary',
        'OutForDelivery': 'bg-info',
        'Delivered': 'bg-success',
        'Cancelled': 'bg-danger'
    };
    return statusClasses[status] || 'bg-secondary';
}

// Initialize login page
function initLoginPage() {
    const loginForm = document.getElementById('loginForm');
    const logoutBtn = document.getElementById('logoutBtn');

    // Check if already logged in
    if (Auth.isLoggedIn()) {
        showUserProfile();
    } else {
        showLoginForm();
    }

    // Handle login form submission
    if (loginForm) {
        loginForm.addEventListener('submit', async (e) => {
            e.preventDefault();
            
            const fullName = document.getElementById('fullName').value.trim();
            const email = document.getElementById('email').value.trim();
            const phone = document.getElementById('phone').value.trim();
            const address = document.getElementById('address').value.trim();
            
            // Validate
            let isValid = true;
            
            if (!fullName) {
                document.getElementById('fullName').classList.add('is-invalid');
                isValid = false;
            } else {
                document.getElementById('fullName').classList.remove('is-invalid');
            }
            
            if (!email || !isValidEmail(email)) {
                document.getElementById('email').classList.add('is-invalid');
                isValid = false;
            } else {
                document.getElementById('email').classList.remove('is-invalid');
            }
            
            if (!isValid) return;
            
            // Show loading state
            const loginBtn = document.getElementById('loginBtn');
            const originalText = loginBtn.innerHTML;
            loginBtn.innerHTML = '<i class="fas fa-spinner fa-spin me-2"></i>Please wait...';
            loginBtn.disabled = true;
            
            // Attempt login
            const result = await Auth.login(fullName, email, phone, address);
            
            // Reset button
            loginBtn.innerHTML = originalText;
            loginBtn.disabled = false;
            
            // Show message
            const loginMessage = document.getElementById('loginMessage');
            if (result.success) {
                loginMessage.className = 'alert alert-success mt-3';
                loginMessage.innerHTML = `<i class="fas fa-check-circle me-2"></i>${result.message}`;
                loginMessage.style.display = 'block';
                
                // Update navbar and show profile
                updateNavbarAuth();
                
                // Redirect after short delay
                setTimeout(() => {
                    // Check if there's a redirect URL
                    const redirect = sessionStorage.getItem('loginRedirect');
                    if (redirect) {
                        sessionStorage.removeItem('loginRedirect');
                        window.location.href = redirect;
                    } else {
                        showUserProfile();
                    }
                }, 1500);
            } else {
                loginMessage.className = 'alert alert-danger mt-3';
                loginMessage.innerHTML = `<i class="fas fa-exclamation-circle me-2"></i>${result.message}`;
                loginMessage.style.display = 'block';
            }
        });
    }

    // Handle logout
    if (logoutBtn) {
        logoutBtn.addEventListener('click', () => {
            Auth.logout();
            showLoginForm();
            
            // Clear form
            document.getElementById('loginForm').reset();
            document.getElementById('loginMessage').style.display = 'none';
        });
    }
}

// Email validation
function isValidEmail(email) {
    return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
}

// Initialize on page load
document.addEventListener('DOMContentLoaded', () => {
    updateNavbarAuth();
    
    // If on login page, initialize login functionality
    if (window.location.pathname.includes('login.html')) {
        initLoginPage();
    }
});

// Export for use in other files
window.Auth = Auth;
window.updateNavbarAuth = updateNavbarAuth;
