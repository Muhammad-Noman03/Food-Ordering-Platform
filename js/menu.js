/* ========================================
   FoodieExpress - Menu Page JavaScript
   ======================================== */

// Current filter state
let currentCategory = 'all';
let currentSearchTerm = '';

// ========================================
// Menu Loading & Filtering
// ========================================

async function loadMenuItems() {
    const container = document.getElementById('menuItems');
    const loadingSpinner = document.getElementById('loadingSpinner');
    const noResults = document.getElementById('noResults');

    if (!container) return;

    // Show loading spinner
    if (loadingSpinner) loadingSpinner.classList.remove('d-none');
    if (noResults) noResults.classList.add('d-none');
    container.innerHTML = '';

    try {
        // Simulate network delay
        await new Promise(resolve => setTimeout(resolve, 500));
        
        const menuItems = await fetchMenuItems();
        
        // Filter items based on category and search
        let filteredItems = menuItems;

        if (currentCategory !== 'all') {
            filteredItems = filteredItems.filter(item => item.category === currentCategory);
        }

        if (currentSearchTerm) {
            const searchLower = currentSearchTerm.toLowerCase();
            filteredItems = filteredItems.filter(item => 
                item.name.toLowerCase().includes(searchLower) ||
                item.description.toLowerCase().includes(searchLower) ||
                item.category.toLowerCase().includes(searchLower)
            );
        }

        // Hide loading spinner
        if (loadingSpinner) loadingSpinner.classList.add('d-none');

        if (filteredItems.length === 0) {
            // Show no results message
            if (noResults) noResults.classList.remove('d-none');
            return;
        }

        // Render menu items
        container.innerHTML = filteredItems.map(item => createFoodCard(item)).join('');
        
        // Attach event listeners
        attachCardEventListeners();

        // Add animation to cards
        animateCards();

    } catch (error) {
        console.error('Error loading menu items:', error);
        if (loadingSpinner) loadingSpinner.classList.add('d-none');
        container.innerHTML = `
            <div class="col-12 text-center">
                <div class="alert alert-danger">
                    <i class="fas fa-exclamation-triangle me-2"></i>
                    Failed to load menu items. Please try again later.
                </div>
            </div>
        `;
    }
}

// Animate cards on load
function animateCards() {
    const cards = document.querySelectorAll('.menu-item');
    cards.forEach((card, index) => {
        card.style.opacity = '0';
        card.style.transform = 'translateY(30px)';
        setTimeout(() => {
            card.style.transition = 'all 0.5s ease';
            card.style.opacity = '1';
            card.style.transform = 'translateY(0)';
        }, index * 50);
    });
}

// ========================================
// Category Filter
// ========================================

function initCategoryFilters() {
    const filterButtons = document.querySelectorAll('.category-btn');
    
    filterButtons.forEach(button => {
        button.addEventListener('click', function() {
            // Update active state
            filterButtons.forEach(btn => {
                btn.classList.remove('active', 'btn-dark');
                btn.classList.add('btn-outline-dark');
            });
            this.classList.add('active', 'btn-dark');
            this.classList.remove('btn-outline-dark');

            // Update current category and reload
            currentCategory = this.dataset.category;
            loadMenuItems();
        });
    });
}

// ========================================
// Search Functionality
// ========================================

function initSearch() {
    const searchInput = document.getElementById('searchInput');
    if (!searchInput) return;

    let searchTimeout;

    searchInput.addEventListener('input', function() {
        clearTimeout(searchTimeout);
        
        // Debounce search for better performance
        searchTimeout = setTimeout(() => {
            currentSearchTerm = this.value.trim();
            loadMenuItems();
        }, 300);
    });

    // Clear search on escape key
    searchInput.addEventListener('keydown', function(e) {
        if (e.key === 'Escape') {
            this.value = '';
            currentSearchTerm = '';
            loadMenuItems();
        }
    });
}

// ========================================
// Quick Add to Cart
// ========================================

function initQuickAddToCart() {
    // This is handled by the attachCardEventListeners function in app.js
}

// ========================================
// Sort Functionality
// ========================================

function sortMenuItems(items, sortBy) {
    const sortedItems = [...items];
    
    switch (sortBy) {
        case 'price-low':
            return sortedItems.sort((a, b) => a.price - b.price);
        case 'price-high':
            return sortedItems.sort((a, b) => b.price - a.price);
        case 'rating':
            return sortedItems.sort((a, b) => b.rating - a.rating);
        case 'name':
            return sortedItems.sort((a, b) => a.name.localeCompare(b.name));
        default:
            return sortedItems;
    }
}

// ========================================
// Lazy Loading Images
// ========================================

function initLazyLoading() {
    if ('IntersectionObserver' in window) {
        const imageObserver = new IntersectionObserver((entries, observer) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    const img = entry.target;
                    img.src = img.dataset.src || img.src;
                    img.classList.remove('lazy');
                    observer.unobserve(img);
                }
            });
        });

        document.querySelectorAll('img.lazy').forEach(img => {
            imageObserver.observe(img);
        });
    }
}

// ========================================
// Initialize Menu Page
// ========================================

document.addEventListener('DOMContentLoaded', function() {
    // Load menu items
    loadMenuItems();
    
    // Initialize category filters
    initCategoryFilters();
    
    // Initialize search
    initSearch();
    
    // Initialize lazy loading
    initLazyLoading();

    console.log('Menu Page Initialized!');
});
