/* ========================================
   FoodieExpress - Shopping Cart JavaScript
   ======================================== */

// Cart data stored in localStorage
let cart = JSON.parse(localStorage.getItem('foodieExpressCart')) || [];

// ========================================
// Cart Functions
// ========================================

// Add item to cart
function addToCart(item, quantity = 1, specialInstructions = '') {
    // Check if item already exists in cart
    const existingItemIndex = cart.findIndex(cartItem => 
        cartItem.id === item.id && cartItem.specialInstructions === specialInstructions
    );

    if (existingItemIndex > -1) {
        // Update quantity if item exists
        cart[existingItemIndex].quantity += quantity;
    } else {
        // Add new item to cart
        cart.push({
            id: item.id,
            name: item.name,
            price: item.price,
            image: item.image,
            quantity: quantity,
            specialInstructions: specialInstructions
        });
    }

    saveCart();
    updateCartCount();
    showToast(`${item.name} added to cart!`, 'success');
}

// Remove item from cart
function removeFromCart(index) {
    const itemName = cart[index].name;
    cart.splice(index, 1);
    saveCart();
    updateCartCount();
    renderCartItems();
    showToast(`${itemName} removed from cart`, 'info');
}

// Update item quantity
function updateCartItemQuantity(index, newQuantity) {
    if (newQuantity < 1) {
        removeFromCart(index);
        return;
    }
    if (newQuantity > 10) {
        newQuantity = 10;
        showToast('Maximum 10 items per product', 'warning');
    }
    cart[index].quantity = newQuantity;
    saveCart();
    updateCartCount();
    renderCartItems();
}

// Clear entire cart
function clearCart() {
    cart = [];
    saveCart();
    updateCartCount();
    renderCartItems();
}

// Save cart to localStorage
function saveCart() {
    localStorage.setItem('foodieExpressCart', JSON.stringify(cart));
}

// Calculate cart total
function getCartTotal() {
    return cart.reduce((total, item) => total + (item.price * item.quantity), 0);
}

// Get total items in cart
function getCartItemCount() {
    return cart.reduce((total, item) => total + item.quantity, 0);
}

// Update cart count badge
function updateCartCount() {
    const countBadges = document.querySelectorAll('.cart-count');
    const count = getCartItemCount();
    countBadges.forEach(badge => {
        badge.textContent = count;
        if (count > 0) {
            badge.style.display = 'inline-block';
        } else {
            badge.style.display = 'none';
        }
    });
}

// Render cart items in modal
function renderCartItems() {
    const cartContainer = document.getElementById('cartItems');
    const cartTotalElement = document.getElementById('cartTotal');
    
    if (!cartContainer) return;

    if (cart.length === 0) {
        cartContainer.innerHTML = `
            <div class="empty-cart">
                <i class="fas fa-shopping-cart"></i>
                <h5>Your cart is empty</h5>
                <p class="text-muted">Add some delicious items to get started!</p>
                <a href="menu.html" class="btn btn-warning">
                    <i class="fas fa-utensils me-2"></i>Browse Menu
                </a>
            </div>
        `;
        if (cartTotalElement) cartTotalElement.textContent = '0.00';
        return;
    }

    let cartHTML = '';
    cart.forEach((item, index) => {
        cartHTML += `
            <div class="cart-item">
                <img src="${item.image}" alt="${item.name}" class="cart-item-img">
                <div class="cart-item-details">
                    <h6 class="mb-1">${item.name}</h6>
                    <p class="text-success mb-1">$${formatPrice(item.price)} each</p>
                    ${item.specialInstructions ? `<small class="text-muted"><i class="fas fa-info-circle me-1"></i>${item.specialInstructions}</small>` : ''}
                </div>
                <div class="cart-item-quantity">
                    <div class="input-group input-group-sm" style="width: 120px;">
                        <button class="btn btn-outline-secondary" type="button" 
                                onclick="updateCartItemQuantity(${index}, ${item.quantity - 1})">
                            <i class="fas fa-minus"></i>
                        </button>
                        <input type="number" class="form-control text-center" value="${item.quantity}" 
                               min="1" max="10" readonly>
                        <button class="btn btn-outline-secondary" type="button"
                                onclick="updateCartItemQuantity(${index}, ${item.quantity + 1})">
                            <i class="fas fa-plus"></i>
                        </button>
                    </div>
                </div>
                <div class="cart-item-total text-end ms-3">
                    <strong>$${formatPrice(item.price * item.quantity)}</strong>
                    <br>
                    <button class="btn btn-sm btn-outline-danger mt-1" onclick="removeFromCart(${index})">
                        <i class="fas fa-trash"></i>
                    </button>
                </div>
            </div>
        `;
    });

    // Add clear cart button
    cartHTML += `
        <div class="text-end mt-3 pt-3 border-top">
            <button class="btn btn-outline-secondary btn-sm" onclick="clearCart()">
                <i class="fas fa-trash-alt me-1"></i>Clear Cart
            </button>
        </div>
    `;

    cartContainer.innerHTML = cartHTML;
    
    if (cartTotalElement) {
        cartTotalElement.textContent = formatPrice(getCartTotal());
    }
}

// ========================================
// Checkout Process
// ========================================

async function processCheckout() {
    if (cart.length === 0) {
        showToast('Your cart is empty!', 'warning');
        return;
    }

    // Check if user is logged in
    if (typeof Auth !== 'undefined' && !Auth.isLoggedIn()) {
        showToast('Please login to complete your order', 'warning');
        // Save redirect URL
        sessionStorage.setItem('loginRedirect', window.location.href);
        // Close cart sidebar/modal if open
        const cartSidebar = document.getElementById('cartSidebar');
        const cartOverlay = document.getElementById('cartOverlay');
        if (cartSidebar) cartSidebar.classList.remove('active');
        if (cartOverlay) cartOverlay.classList.remove('active');
        // Close cart modal if open
        const cartModalEl = document.getElementById('cartModal');
        if (cartModalEl) {
            const cartModal = bootstrap.Modal.getInstance(cartModalEl);
            if (cartModal) cartModal.hide();
        }
        // Redirect to login page
        setTimeout(() => {
            window.location.href = 'login.html';
        }, 1000);
        return;
    }

    // Get user info if logged in
    const user = (typeof Auth !== 'undefined') ? Auth.getUser() : null;

    // Prepare order data
    const orderData = {
        userId: user ? user.id : null,
        customerName: user ? user.fullName : 'Guest',
        customerEmail: user ? user.email : '',
        customerPhone: user ? (user.phone || '') : '',
        deliveryAddress: user ? (user.address || '') : '',
        notes: '',
        items: cart.map(item => ({
            menuItemId: item.id,
            itemName: item.name,
            quantity: item.quantity,
            price: item.price,
            specialInstructions: item.specialInstructions || ''
        })),
        totalAmount: getCartTotal(),
        orderDate: new Date().toISOString()
    };

    console.log('Sending order data:', orderData);

    try {
        // Show loading state
        const checkoutBtn = document.getElementById('checkoutBtn');
        if (checkoutBtn) {
            checkoutBtn.disabled = true;
            checkoutBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>Processing...';
        }

        // Submit order to backend
        const response = await createOrder(orderData);

        // Close cart modal
        const cartModal = bootstrap.Modal.getInstance(document.getElementById('cartModal'));
        if (cartModal) cartModal.hide();

        // Clear cart
        clearCart();

        // Show success modal
        const orderIdElement = document.getElementById('orderId');
        if (orderIdElement) {
            orderIdElement.textContent = response.id || 'ORD-' + Date.now();
        }
        
        const successModal = new bootstrap.Modal(document.getElementById('orderSuccessModal'));
        successModal.show();

        // Reset checkout button
        if (checkoutBtn) {
            checkoutBtn.disabled = false;
            checkoutBtn.innerHTML = '<i class="fas fa-credit-card me-2"></i>Checkout';
        }

    } catch (error) {
        console.error('Checkout error:', error);
        showToast('Error processing order. Please try again.', 'danger');
        
        const checkoutBtn = document.getElementById('checkoutBtn');
        if (checkoutBtn) {
            checkoutBtn.disabled = false;
            checkoutBtn.innerHTML = '<i class="fas fa-credit-card me-2"></i>Checkout';
        }
    }
}

// ========================================
// Initialize Cart
// ========================================

document.addEventListener('DOMContentLoaded', function() {
    // Update cart count on page load
    updateCartCount();

    // Cart button click handler
    const cartBtn = document.getElementById('cartBtn');
    if (cartBtn) {
        cartBtn.addEventListener('click', function(e) {
            e.preventDefault();
            renderCartItems();
            const cartModal = new bootstrap.Modal(document.getElementById('cartModal'));
            cartModal.show();
        });
    }

    // Checkout button click handler
    const checkoutBtn = document.getElementById('checkoutBtn');
    if (checkoutBtn) {
        checkoutBtn.addEventListener('click', processCheckout);
    }

    console.log('Cart System Initialized!');
});
