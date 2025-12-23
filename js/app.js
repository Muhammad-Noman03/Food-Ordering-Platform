/* ========================================
   FoodieExpress - Main Application JavaScript
   ======================================== */

// API Base URL - Change this to your ASP.NET backend URL
const API_BASE_URL = 'http://localhost:5000/api';

// Menu Items Data (used when backend is not available)
const menuItemsData = [
    // Pizzas
    {
        id: 1,
        name: "Margherita Pizza",
        description: "Classic pizza with fresh mozzarella, tomatoes, and basil",
        price: 12.99,
        category: "pizza",
        image: "https://images.unsplash.com/photo-1574071318508-1cdbab80d002?w=400",
        rating: 4.8,
        isPopular: true
    },
    {
        id: 2,
        name: "Pepperoni Pizza",
        description: "Loaded with pepperoni slices and melted cheese",
        price: 14.99,
        category: "pizza",
        image: "https://images.unsplash.com/photo-1628840042765-356cda07504e?w=400",
        rating: 4.9,
        isPopular: true
    },
    {
        id: 3,
        name: "BBQ Chicken Pizza",
        description: "Grilled chicken, BBQ sauce, red onions, and cilantro",
        price: 15.99,
        category: "pizza",
        image: "https://images.unsplash.com/photo-1565299624946-b28f40a0ae38?w=400",
        rating: 4.7,
        isPopular: false
    },
    {
        id: 4,
        name: "Veggie Supreme Pizza",
        description: "Bell peppers, mushrooms, olives, onions, and tomatoes",
        price: 13.99,
        category: "pizza",
        image: "https://images.unsplash.com/photo-1511689660979-10d2b1aada49?w=400",
        rating: 4.5,
        isPopular: false
    },
    // Burgers
    {
        id: 5,
        name: "Classic Cheeseburger",
        description: "Juicy beef patty with cheese, lettuce, tomato, and special sauce",
        price: 9.99,
        category: "burger",
        image: "https://images.unsplash.com/photo-1568901346375-23c9450c58cd?w=400",
        rating: 4.8,
        isPopular: true
    },
    {
        id: 6,
        name: "Bacon Deluxe Burger",
        description: "Double patty with crispy bacon, cheddar, and caramelized onions",
        price: 12.99,
        category: "burger",
        image: "https://images.unsplash.com/photo-1553979459-d2229ba7433b?w=400",
        rating: 4.9,
        isPopular: true
    },
    {
        id: 7,
        name: "Mushroom Swiss Burger",
        description: "Beef patty with sautéed mushrooms and Swiss cheese",
        price: 11.49,
        category: "burger",
        image: "https://images.unsplash.com/photo-1594212699903-ec8a3eca50f5?w=400",
        rating: 4.6,
        isPopular: false
    },
    {
        id: 8,
        name: "Veggie Burger",
        description: "Plant-based patty with avocado, lettuce, and tomato",
        price: 10.99,
        category: "burger",
        image: "https://images.unsplash.com/photo-1520072959219-c595dc870360?w=400",
        rating: 4.4,
        isPopular: false
    },
    // Pasta
    {
        id: 9,
        name: "Spaghetti Carbonara",
        description: "Creamy pasta with bacon, egg, and parmesan cheese",
        price: 13.99,
        category: "pasta",
        image: "https://images.unsplash.com/photo-1612874742237-6526221588e3?w=400",
        rating: 4.7,
        isPopular: true
    },
    {
        id: 10,
        name: "Penne Alfredo",
        description: "Penne pasta in rich and creamy Alfredo sauce",
        price: 12.49,
        category: "pasta",
        image: "https://images.unsplash.com/photo-1645112411341-6c4fd023714a?w=400",
        rating: 4.6,
        isPopular: false
    },
    {
        id: 11,
        name: "Lasagna",
        description: "Layered pasta with meat sauce, ricotta, and mozzarella",
        price: 14.99,
        category: "pasta",
        image: "https://images.unsplash.com/photo-1619895092538-128341789043?w=400",
        rating: 4.8,
        isPopular: false
    },
    {
        id: 12,
        name: "Pasta Primavera",
        description: "Fresh vegetables in garlic olive oil with linguine",
        price: 11.99,
        category: "pasta",
        image: "https://images.unsplash.com/photo-1563379926898-05f4575a45d8?w=400",
        rating: 4.5,
        isPopular: false
    },
    // Salads
    {
        id: 13,
        name: "Caesar Salad",
        description: "Romaine lettuce, croutons, parmesan, and Caesar dressing",
        price: 8.99,
        category: "salad",
        image: "https://images.unsplash.com/photo-1546793665-c74683f339c1?w=400",
        rating: 4.5,
        isPopular: false
    },
    {
        id: 14,
        name: "Greek Salad",
        description: "Cucumber, tomatoes, olives, feta cheese, and olive oil",
        price: 9.49,
        category: "salad",
        image: "https://images.unsplash.com/photo-1540189549336-e6e99c3679fe?w=400",
        rating: 4.6,
        isPopular: false
    },
    {
        id: 15,
        name: "Chicken Avocado Salad",
        description: "Grilled chicken, avocado, mixed greens, and honey mustard",
        price: 11.99,
        category: "salad",
        image: "https://images.unsplash.com/photo-1512621776951-a57141f2eefd?w=400",
        rating: 4.7,
        isPopular: true
    },
    // Desserts
    {
        id: 16,
        name: "Chocolate Lava Cake",
        description: "Warm chocolate cake with molten center, served with ice cream",
        price: 7.99,
        category: "dessert",
        image: "https://images.unsplash.com/photo-1624353365286-3f8d62daad51?w=400",
        rating: 4.9,
        isPopular: true
    },
    {
        id: 17,
        name: "Tiramisu",
        description: "Classic Italian dessert with coffee-soaked ladyfingers",
        price: 6.99,
        category: "dessert",
        image: "https://images.unsplash.com/photo-1571877227200-a0d98ea607e9?w=400",
        rating: 4.8,
        isPopular: false
    },
    {
        id: 18,
        name: "Cheesecake",
        description: "Creamy New York style cheesecake with berry compote",
        price: 6.49,
        category: "dessert",
        image: "https://images.unsplash.com/photo-1533134242443-d4fd215305ad?w=400",
        rating: 4.7,
        isPopular: false
    },
    {
        id: 19,
        name: "Ice Cream Sundae",
        description: "Three scoops of ice cream with chocolate sauce and whipped cream",
        price: 5.99,
        category: "dessert",
        image: "https://images.unsplash.com/photo-1563805042-7684c019e1cb?w=400",
        rating: 4.6,
        isPopular: false
    },
    // Drinks
    {
        id: 20,
        name: "Fresh Lemonade",
        description: "Freshly squeezed lemonade with mint",
        price: 3.99,
        category: "drinks",
        image: "https://images.unsplash.com/photo-1621263764928-df1444c5e859?w=400",
        rating: 4.5,
        isPopular: false
    },
    {
        id: 21,
        name: "Mango Smoothie",
        description: "Blended fresh mango with yogurt and honey",
        price: 4.99,
        category: "drinks",
        image: "https://images.unsplash.com/photo-1623065422902-30a2d299bbe4?w=400",
        rating: 4.7,
        isPopular: false
    },
    {
        id: 22,
        name: "Iced Coffee",
        description: "Cold brew coffee with cream and vanilla syrup",
        price: 4.49,
        category: "drinks",
        image: "https://images.unsplash.com/photo-1461023058943-07fcbe16d735?w=400",
        rating: 4.6,
        isPopular: false
    },
    {
        id: 23,
        name: "Mojito Mocktail",
        description: "Refreshing lime and mint drink with soda",
        price: 4.99,
        category: "drinks",
        image: "https://images.unsplash.com/photo-1551538827-9c037cb4f32a?w=400",
        rating: 4.5,
        isPopular: false
    }
];

// ========================================
// Utility Functions
// ========================================

// Show toast notification
function showToast(message, type = 'success') {
    // Remove existing toasts
    const existingToasts = document.querySelectorAll('.custom-toast');
    existingToasts.forEach(toast => toast.remove());

    const toast = document.createElement('div');
    toast.className = `custom-toast alert alert-${type} position-fixed`;
    toast.style.cssText = 'bottom: 20px; right: 20px; z-index: 9999; min-width: 300px; animation: slideIn 0.3s ease;';
    toast.innerHTML = `
        <div class="d-flex align-items-center">
            <i class="fas ${type === 'success' ? 'fa-check-circle' : type === 'danger' ? 'fa-exclamation-circle' : 'fa-info-circle'} me-2"></i>
            <span>${message}</span>
            <button type="button" class="btn-close ms-auto" onclick="this.parentElement.parentElement.remove()"></button>
        </div>
    `;
    document.body.appendChild(toast);

    // Auto remove after 3 seconds
    setTimeout(() => {
        if (toast.parentElement) {
            toast.style.animation = 'slideOut 0.3s ease';
            setTimeout(() => toast.remove(), 300);
        }
    }, 3000);
}

// Add toast animation styles
const toastStyles = document.createElement('style');
toastStyles.textContent = `
    @keyframes slideIn {
        from { transform: translateX(100%); opacity: 0; }
        to { transform: translateX(0); opacity: 1; }
    }
    @keyframes slideOut {
        from { transform: translateX(0); opacity: 1; }
        to { transform: translateX(100%); opacity: 0; }
    }
`;
document.head.appendChild(toastStyles);

// Format price
function formatPrice(price) {
    return parseFloat(price).toFixed(2);
}

// Generate star rating HTML
function generateStarRating(rating) {
    const fullStars = Math.floor(rating);
    const hasHalfStar = rating % 1 >= 0.5;
    let starsHTML = '';

    for (let i = 0; i < fullStars; i++) {
        starsHTML += '<i class="fas fa-star"></i>';
    }
    if (hasHalfStar) {
        starsHTML += '<i class="fas fa-star-half-alt"></i>';
    }
    const emptyStars = 5 - fullStars - (hasHalfStar ? 1 : 0);
    for (let i = 0; i < emptyStars; i++) {
        starsHTML += '<i class="far fa-star"></i>';
    }

    return starsHTML;
}

// Get category badge class
function getCategoryBadgeClass(category) {
    const classes = {
        pizza: 'bg-danger',
        burger: 'bg-warning text-dark',
        pasta: 'bg-info',
        salad: 'bg-success',
        dessert: 'bg-primary',
        drinks: 'bg-secondary'
    };
    return classes[category] || 'bg-dark';
}

// Create food card HTML
function createFoodCard(item) {
    return `
        <div class="col-lg-3 col-md-4 col-sm-6 menu-item" data-category="${item.category}">
            <div class="card food-card h-100">
                <div class="card-img-wrapper position-relative">
                    <img src="${item.image}" class="card-img-top" alt="${item.name}" loading="lazy">
                    <span class="price-tag">$${formatPrice(item.price)}</span>
                    <span class="category-badge ${getCategoryBadgeClass(item.category)}">${item.category}</span>
                    <div class="card-img-overlay-custom">
                        <button class="btn btn-light btn-sm view-details-btn" data-id="${item.id}">
                            <i class="fas fa-eye me-1"></i>View Details
                        </button>
                    </div>
                </div>
                <div class="card-body">
                    <h5 class="card-title">${item.name}</h5>
                    <p class="card-text text-muted small">${item.description}</p>
                    <div class="d-flex justify-content-between align-items-center mt-auto">
                        <div class="rating text-warning small">
                            ${generateStarRating(item.rating)}
                            <span class="text-muted ms-1">(${item.rating})</span>
                        </div>
                        <button class="btn btn-warning add-to-cart-btn" data-id="${item.id}" title="Add to Cart">
                            <i class="fas fa-cart-plus"></i>
                        </button>
                    </div>
                </div>
            </div>
        </div>
    `;
}

// ========================================
// API Functions (for ASP.NET backend)
// ========================================

async function fetchMenuItems() {
    try {
        const response = await fetch(`${API_BASE_URL}/menuitems`);
        if (!response.ok) throw new Error('Failed to fetch menu items');
        return await response.json();
    } catch (error) {
        console.log('Using local data:', error.message);
        return menuItemsData;
    }
}

async function fetchPopularItems() {
    try {
        const response = await fetch(`${API_BASE_URL}/menuitems/popular`);
        if (!response.ok) throw new Error('Failed to fetch popular items');
        return await response.json();
    } catch (error) {
        console.log('Using local data for popular items');
        return menuItemsData.filter(item => item.isPopular);
    }
}

async function createOrder(orderData) {
    try {
        console.log('Creating order at:', `${API_BASE_URL}/orders`);
        console.log('Order payload:', JSON.stringify(orderData, null, 2));
        
        const response = await fetch(`${API_BASE_URL}/orders`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(orderData)
        });
        
        console.log('Response status:', response.status);
        
        if (!response.ok) {
            const errorText = await response.text();
            console.error('Order creation failed:', errorText);
            throw new Error('Failed to create order: ' + errorText);
        }
        
        const result = await response.json();
        console.log('Order created successfully:', result);
        return result;
    } catch (error) {
        console.error('Order creation error:', error.message);
        // Return mock order response for fallback
        return {
            id: 'ORD-' + Date.now(),
            status: 'Confirmed',
            message: 'Order placed successfully!'
        };
    }
}

async function submitContactForm(contactData) {
    try {
        const response = await fetch(`${API_BASE_URL}/contacts`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(contactData)
        });
        if (!response.ok) throw new Error('Failed to submit contact form');
        return await response.json();
    } catch (error) {
        console.log('Contact form submission simulated:', error.message);
        // Return mock response
        return {
            success: true,
            message: 'Message received successfully!'
        };
    }
}

// ========================================
// Load Popular Dishes on Home Page
// ========================================

async function loadPopularDishes() {
    const container = document.getElementById('popularDishes');
    if (!container) return;

    try {
        const popularItems = await fetchPopularItems();
        container.innerHTML = popularItems.slice(0, 4).map(item => createFoodCard(item)).join('');
        
        // Add event listeners for the cards
        attachCardEventListeners();
    } catch (error) {
        console.error('Error loading popular dishes:', error);
        container.innerHTML = '<div class="col-12 text-center"><p class="text-muted">Failed to load dishes. Please try again later.</p></div>';
    }
}

// Attach event listeners to food cards
function attachCardEventListeners() {
    // Add to cart buttons
    document.querySelectorAll('.add-to-cart-btn').forEach(btn => {
        btn.addEventListener('click', function(e) {
            e.stopPropagation();
            const itemId = parseInt(this.dataset.id);
            const item = menuItemsData.find(i => i.id === itemId);
            if (item) {
                addToCart(item);
                // Animate button
                this.classList.add('btn-success');
                this.innerHTML = '<i class="fas fa-check"></i>';
                setTimeout(() => {
                    this.classList.remove('btn-success');
                    this.classList.add('btn-warning');
                    this.innerHTML = '<i class="fas fa-cart-plus"></i>';
                }, 1000);
            }
        });
    });

    // View details buttons
    document.querySelectorAll('.view-details-btn').forEach(btn => {
        btn.addEventListener('click', function(e) {
            e.stopPropagation();
            const itemId = parseInt(this.dataset.id);
            openItemDetailModal(itemId);
        });
    });
}

// Open item detail modal
function openItemDetailModal(itemId) {
    const item = menuItemsData.find(i => i.id === itemId);
    if (!item) return;

    const modal = document.getElementById('itemDetailModal');
    if (!modal) return;

    document.getElementById('itemDetailTitle').textContent = item.name;
    document.getElementById('itemDetailImage').src = item.image;
    document.getElementById('itemDetailImage').alt = item.name;
    document.getElementById('itemDetailName').textContent = item.name;
    document.getElementById('itemDetailDescription').textContent = item.description;
    document.getElementById('itemDetailPrice').textContent = `$${formatPrice(item.price)}`;
    document.getElementById('itemQuantity').value = 1;
    document.getElementById('specialInstructions').value = '';

    // Store item id for adding to cart
    modal.dataset.itemId = itemId;

    const bsModal = new bootstrap.Modal(modal);
    bsModal.show();
}

// ========================================
// Newsletter Form
// ========================================

function initNewsletterForm() {
    const form = document.getElementById('newsletterForm');
    if (!form) return;

    form.addEventListener('submit', function(e) {
        e.preventDefault();
        const email = this.querySelector('input[type="email"]').value;
        
        // Simulate subscription
        showToast('Thank you for subscribing!', 'success');
        this.reset();
    });
}

// ========================================
// Scroll to Top Button
// ========================================

function initScrollToTop() {
    // Create scroll to top button
    const scrollBtn = document.createElement('button');
    scrollBtn.className = 'scroll-to-top';
    scrollBtn.innerHTML = '<i class="fas fa-arrow-up"></i>';
    scrollBtn.setAttribute('aria-label', 'Scroll to top');
    document.body.appendChild(scrollBtn);

    // Show/hide based on scroll position
    window.addEventListener('scroll', function() {
        if (window.pageYOffset > 300) {
            scrollBtn.classList.add('show');
        } else {
            scrollBtn.classList.remove('show');
        }
    });

    // Scroll to top on click
    scrollBtn.addEventListener('click', function() {
        window.scrollTo({
            top: 0,
            behavior: 'smooth'
        });
    });
}

// ========================================
// Initialize Application
// ========================================

document.addEventListener('DOMContentLoaded', function() {
    // Load popular dishes on home page
    loadPopularDishes();
    
    // Initialize newsletter form
    initNewsletterForm();
    
    // Initialize scroll to top button
    initScrollToTop();
    
    // Initialize quantity controls in item detail modal
    const decreaseBtn = document.getElementById('decreaseQty');
    const increaseBtn = document.getElementById('increaseQty');
    const quantityInput = document.getElementById('itemQuantity');

    if (decreaseBtn && increaseBtn && quantityInput) {
        decreaseBtn.addEventListener('click', function() {
            const currentValue = parseInt(quantityInput.value);
            if (currentValue > 1) {
                quantityInput.value = currentValue - 1;
            }
        });

        increaseBtn.addEventListener('click', function() {
            const currentValue = parseInt(quantityInput.value);
            if (currentValue < 10) {
                quantityInput.value = currentValue + 1;
            }
        });
    }

    // Add to cart from modal
    const addToCartFromModalBtn = document.getElementById('addToCartFromModal');
    if (addToCartFromModalBtn) {
        addToCartFromModalBtn.addEventListener('click', function() {
            const modal = document.getElementById('itemDetailModal');
            const itemId = parseInt(modal.dataset.itemId);
            const quantity = parseInt(document.getElementById('itemQuantity').value);
            const specialInstructions = document.getElementById('specialInstructions').value;

            const item = menuItemsData.find(i => i.id === itemId);
            if (item) {
                addToCart(item, quantity, specialInstructions);
                bootstrap.Modal.getInstance(modal).hide();
            }
        });
    }

    console.log('FoodieExpress App Initialized!');
});
