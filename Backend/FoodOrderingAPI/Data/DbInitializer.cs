using FoodOrderingAPI.Models;

namespace FoodOrderingAPI.Data
{
    /// <summary>
    /// Database initializer to seed initial data
    /// </summary>
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // Ensure database is created
            context.Database.EnsureCreated();

            // Check if menu items already exist
            if (context.MenuItems.Any())
            {
                return; // Database has been seeded
            }

            // Seed Menu Items
            var menuItems = new MenuItem[]
            {
                // Pizzas
                new MenuItem
                {
                    Name = "Margherita Pizza",
                    Description = "Classic pizza with fresh mozzarella, tomatoes, and basil",
                    Price = 12.99m,
                    Category = "pizza",
                    Image = "https://images.unsplash.com/photo-1574071318508-1cdbab80d002?w=400",
                    Rating = 4.8m,
                    IsPopular = true,
                    IsAvailable = true
                },
                new MenuItem
                {
                    Name = "Pepperoni Pizza",
                    Description = "Loaded with pepperoni slices and melted cheese",
                    Price = 14.99m,
                    Category = "pizza",
                    Image = "https://images.unsplash.com/photo-1628840042765-356cda07504e?w=400",
                    Rating = 4.9m,
                    IsPopular = true,
                    IsAvailable = true
                },
                new MenuItem
                {
                    Name = "BBQ Chicken Pizza",
                    Description = "Grilled chicken, BBQ sauce, red onions, and cilantro",
                    Price = 15.99m,
                    Category = "pizza",
                    Image = "https://images.unsplash.com/photo-1565299624946-b28f40a0ae38?w=400",
                    Rating = 4.7m,
                    IsPopular = false,
                    IsAvailable = true
                },
                new MenuItem
                {
                    Name = "Veggie Supreme Pizza",
                    Description = "Bell peppers, mushrooms, olives, onions, and tomatoes",
                    Price = 13.99m,
                    Category = "pizza",
                    Image = "https://images.unsplash.com/photo-1511689660979-10d2b1aada49?w=400",
                    Rating = 4.5m,
                    IsPopular = false,
                    IsAvailable = true
                },
                // Burgers
                new MenuItem
                {
                    Name = "Classic Cheeseburger",
                    Description = "Juicy beef patty with cheese, lettuce, tomato, and special sauce",
                    Price = 9.99m,
                    Category = "burger",
                    Image = "https://images.unsplash.com/photo-1568901346375-23c9450c58cd?w=400",
                    Rating = 4.8m,
                    IsPopular = true,
                    IsAvailable = true
                },
                new MenuItem
                {
                    Name = "Bacon Deluxe Burger",
                    Description = "Double patty with crispy bacon, cheddar, and caramelized onions",
                    Price = 12.99m,
                    Category = "burger",
                    Image = "https://images.unsplash.com/photo-1553979459-d2229ba7433b?w=400",
                    Rating = 4.9m,
                    IsPopular = true,
                    IsAvailable = true
                },
                new MenuItem
                {
                    Name = "Mushroom Swiss Burger",
                    Description = "Beef patty with sautéed mushrooms and Swiss cheese",
                    Price = 11.49m,
                    Category = "burger",
                    Image = "https://images.unsplash.com/photo-1594212699903-ec8a3eca50f5?w=400",
                    Rating = 4.6m,
                    IsPopular = false,
                    IsAvailable = true
                },
                new MenuItem
                {
                    Name = "Veggie Burger",
                    Description = "Plant-based patty with avocado, lettuce, and tomato",
                    Price = 10.99m,
                    Category = "burger",
                    Image = "https://images.unsplash.com/photo-1520072959219-c595dc870360?w=400",
                    Rating = 4.4m,
                    IsPopular = false,
                    IsAvailable = true
                },
                // Pasta
                new MenuItem
                {
                    Name = "Spaghetti Carbonara",
                    Description = "Creamy pasta with bacon, egg, and parmesan cheese",
                    Price = 13.99m,
                    Category = "pasta",
                    Image = "https://images.unsplash.com/photo-1612874742237-6526221588e3?w=400",
                    Rating = 4.7m,
                    IsPopular = true,
                    IsAvailable = true
                },
                new MenuItem
                {
                    Name = "Penne Alfredo",
                    Description = "Penne pasta in rich and creamy Alfredo sauce",
                    Price = 12.49m,
                    Category = "pasta",
                    Image = "https://images.unsplash.com/photo-1645112411341-6c4fd023714a?w=400",
                    Rating = 4.6m,
                    IsPopular = false,
                    IsAvailable = true
                },
                new MenuItem
                {
                    Name = "Lasagna",
                    Description = "Layered pasta with meat sauce, ricotta, and mozzarella",
                    Price = 14.99m,
                    Category = "pasta",
                    Image = "https://images.unsplash.com/photo-1619895092538-128341789043?w=400",
                    Rating = 4.8m,
                    IsPopular = false,
                    IsAvailable = true
                },
                new MenuItem
                {
                    Name = "Pasta Primavera",
                    Description = "Fresh vegetables in garlic olive oil with linguine",
                    Price = 11.99m,
                    Category = "pasta",
                    Image = "https://images.unsplash.com/photo-1563379926898-05f4575a45d8?w=400",
                    Rating = 4.5m,
                    IsPopular = false,
                    IsAvailable = true
                },
                // Salads
                new MenuItem
                {
                    Name = "Caesar Salad",
                    Description = "Romaine lettuce, croutons, parmesan, and Caesar dressing",
                    Price = 8.99m,
                    Category = "salad",
                    Image = "https://images.unsplash.com/photo-1546793665-c74683f339c1?w=400",
                    Rating = 4.5m,
                    IsPopular = false,
                    IsAvailable = true
                },
                new MenuItem
                {
                    Name = "Greek Salad",
                    Description = "Cucumber, tomatoes, olives, feta cheese, and olive oil",
                    Price = 9.49m,
                    Category = "salad",
                    Image = "https://images.unsplash.com/photo-1540189549336-e6e99c3679fe?w=400",
                    Rating = 4.6m,
                    IsPopular = false,
                    IsAvailable = true
                },
                new MenuItem
                {
                    Name = "Chicken Avocado Salad",
                    Description = "Grilled chicken, avocado, mixed greens, and honey mustard",
                    Price = 11.99m,
                    Category = "salad",
                    Image = "https://images.unsplash.com/photo-1512621776951-a57141f2eefd?w=400",
                    Rating = 4.7m,
                    IsPopular = true,
                    IsAvailable = true
                },
                // Desserts
                new MenuItem
                {
                    Name = "Chocolate Lava Cake",
                    Description = "Warm chocolate cake with molten center, served with ice cream",
                    Price = 7.99m,
                    Category = "dessert",
                    Image = "https://images.unsplash.com/photo-1624353365286-3f8d62daad51?w=400",
                    Rating = 4.9m,
                    IsPopular = true,
                    IsAvailable = true
                },
                new MenuItem
                {
                    Name = "Tiramisu",
                    Description = "Classic Italian dessert with coffee-soaked ladyfingers",
                    Price = 6.99m,
                    Category = "dessert",
                    Image = "https://images.unsplash.com/photo-1571877227200-a0d98ea607e9?w=400",
                    Rating = 4.8m,
                    IsPopular = false,
                    IsAvailable = true
                },
                new MenuItem
                {
                    Name = "Cheesecake",
                    Description = "Creamy New York style cheesecake with berry compote",
                    Price = 6.49m,
                    Category = "dessert",
                    Image = "https://images.unsplash.com/photo-1533134242443-d4fd215305ad?w=400",
                    Rating = 4.7m,
                    IsPopular = false,
                    IsAvailable = true
                },
                new MenuItem
                {
                    Name = "Ice Cream Sundae",
                    Description = "Three scoops of ice cream with chocolate sauce and whipped cream",
                    Price = 5.99m,
                    Category = "dessert",
                    Image = "https://images.unsplash.com/photo-1563805042-7684c019e1cb?w=400",
                    Rating = 4.6m,
                    IsPopular = false,
                    IsAvailable = true
                },
                // Drinks
                new MenuItem
                {
                    Name = "Fresh Lemonade",
                    Description = "Freshly squeezed lemonade with mint",
                    Price = 3.99m,
                    Category = "drinks",
                    Image = "https://images.unsplash.com/photo-1621263764928-df1444c5e859?w=400",
                    Rating = 4.5m,
                    IsPopular = false,
                    IsAvailable = true
                },
                new MenuItem
                {
                    Name = "Mango Smoothie",
                    Description = "Blended fresh mango with yogurt and honey",
                    Price = 4.99m,
                    Category = "drinks",
                    Image = "https://images.unsplash.com/photo-1623065422902-30a2d299bbe4?w=400",
                    Rating = 4.7m,
                    IsPopular = false,
                    IsAvailable = true
                },
                new MenuItem
                {
                    Name = "Iced Coffee",
                    Description = "Cold brew coffee with cream and vanilla syrup",
                    Price = 4.49m,
                    Category = "drinks",
                    Image = "https://images.unsplash.com/photo-1461023058943-07fcbe16d735?w=400",
                    Rating = 4.6m,
                    IsPopular = false,
                    IsAvailable = true
                },
                new MenuItem
                {
                    Name = "Mojito Mocktail",
                    Description = "Refreshing lime and mint drink with soda",
                    Price = 4.99m,
                    Category = "drinks",
                    Image = "https://images.unsplash.com/photo-1551538827-9c037cb4f32a?w=400",
                    Rating = 4.5m,
                    IsPopular = false,
                    IsAvailable = true
                }
            };

            context.MenuItems.AddRange(menuItems);
            context.SaveChanges();
        }
    }
}
