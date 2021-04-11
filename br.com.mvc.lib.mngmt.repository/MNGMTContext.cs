using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BCrypt.Net;
using br.com.mvc.lib.mngmt.model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace br.com.mvc.lib.mngmt.repository
{
    public partial class MNGMTContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlite("Data Source=C:\\TEMP\\br.com.mvc.lib.mngmt.db");

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new List<User>()
            {
                new()
                {
                    Username = "admin",
                    Roles = "ADMIN",
                    Id = Guid.NewGuid(),
                    Name = "Admin",
                    Password = BCrypt.Net.BCrypt.HashPassword("admin"),
                }
            });
            //foreach (var entityType in modelBuilder.Model.GetEntityTypes().Where(x => !x.ClrType.Name.Contains("Dictionary`")))
            //{
            //    var parameter = Expression.Parameter(entityType.ClrType);
            //    var propertyMethodInfo = typeof(EF).GetMethod("Property")?.MakeGenericMethod(typeof(bool));
            //    var isDeletedProperty = Expression.Call(propertyMethodInfo, parameter, Expression.Constant("IsDeleted"));
            //    var compareExpression = Expression.MakeBinary(ExpressionType.Equal, isDeletedProperty, Expression.Constant(false));
            //    var lambda = Expression.Lambda(compareExpression, parameter);

            //    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            //}

            base.OnModelCreating(modelBuilder);
        }
    }
}

