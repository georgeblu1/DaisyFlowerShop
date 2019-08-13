using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Daisy.Data;

namespace Daisy.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            { // Look for any movies. 
                if (context.Flower.Any())
                {
                    return; // DB has been seeded 
                }
                context.Flower.AddRange(
                    new Flower
                    {
                        FlowerName = "Baby Shine Shine",
                        FlowerProducedDate = DateTime.Parse("2019-8-8"),
                        Type = "Hand Bouquet",
                        Price = 108,
                        Quantity = 100,
                        Image = "https://50gram.com.my/wp-content/uploads/2019/08/Shine-Shine-Product-v2.jpg"
                    },
                    new Flower
                    {
                        FlowerName = "Delia",
                        FlowerProducedDate = DateTime.Parse("2019-8-8"),
                        Type = "Hand Bouquet",
                        Price = 78,
                        Quantity = 80,
                        Image = "https://50gram.com.my/wp-content/uploads/2019/07/Hera-Product-Regular-v2.jpg"

                    },
                    new Flower
                    {
                        FlowerName = "FairyTale",
                        FlowerProducedDate = DateTime.Parse("2019-8-8"),
                        Type = "Hand Bouquet",
                        Price = 69,
                        Quantity = 80,
                        Image = "https://50gram.com.my/wp-content/uploads/2019/05/Fairytale-Product-Regular.jpg"
                    },
                    new Flower
                    {
                        FlowerName = "Lady Boss Pink Box",
                        FlowerProducedDate = DateTime.Parse("2019-8-8"),
                        Type = "Box Set",
                        Price = 148,
                        Quantity = 80,
                        Image = "https://50gram.com.my/wp-content/uploads/2019/05/Square-pink-MD.jpg"
                    },
                    new Flower
                    {
                        FlowerName = "Memories Bundle",
                        FlowerProducedDate = DateTime.Parse("2019-8-8"),
                        Type = "Box Set",
                        Price = 299,
                        Quantity = 80,
                        Image = "https://50gram.com.my/wp-content/uploads/2019/06/Memories1.jpg"
                    },
                    new Flower
                    {
                        FlowerName = "Sandra",
                        FlowerProducedDate = DateTime.Parse("2019-8-8"),
                        Type = "Box Set",
                        Price = 158,
                        Quantity = 80,
                        Image = "https://50gram.com.my/wp-content/uploads/2019/04/Round-Flower-Box1.jpg"
                    },
                    new Flower
                    {
                        FlowerName = "Blessed Adventure",
                        FlowerProducedDate = DateTime.Parse("2019-8-8"),
                        Type = "Graduation",
                        Price = 210,
                        Quantity = 80,
                        Image = "https://50gram.com.my/wp-content/uploads/2019/04/1979383713-e1507618065115.jpg"
                    },
                    new Flower
                    {
                        FlowerName = "Warmest Congratulation",
                        FlowerProducedDate = DateTime.Parse("2019-8-8"),
                        Type = "Graduation",
                        Price = 165,
                        Quantity = 80,
                        Image = "https://50gram.com.my/wp-content/uploads/2019/04/1474923861-e1507618023307.jpg"
                    }

                    );
                context.SaveChanges();
            }
        }
    }
}
