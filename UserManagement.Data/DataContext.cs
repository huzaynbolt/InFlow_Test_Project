﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models;

namespace UserManagement.Data;

public class DataContext : DbContext, IDataContext
{
    public DataContext() => Database.EnsureCreated();

    public DataContext(DbContextOptions<DataContext> options)
       : base(options)
    {
        Database.EnsureCreated();
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("UserManagement.Data.DataContext");
    }

    protected override void OnModelCreating(ModelBuilder model)
    {
        model.Entity<User>().HasMany(c=>c.AuditLogs).WithOne(c => c.User).HasForeignKey(c => c.UserId);

        model.Entity<User>().HasData(new[]
                {
            new User { Id = 1, Forename = "Peter", Surname = "Loew", Email = "ploew@example.com", IsActive = true,
                DateOfBirth = new DateTime(1992, 09, 10) },
            new User { Id = 2, Forename = "Benjamin Franklin", Surname = "Gates", Email = "bfgates@example.com", IsActive = true,
                DateOfBirth = new DateTime(1995, 08, 10) },
            new User { Id = 3, Forename = "Castor", Surname = "Troy", Email = "ctroy@example.com", IsActive = false,
                DateOfBirth = new DateTime(1997, 07, 10) },
            new User { Id = 4, Forename = "Memphis", Surname = "Raines", Email = "mraines@example.com", IsActive = true,
                DateOfBirth = new DateTime(1998, 04, 22) },
            new User { Id = 5, Forename = "Stanley", Surname = "Goodspeed", Email = "sgodspeed@example.com", IsActive = true,
                DateOfBirth = new DateTime(1999, 04, 24) },
            new User { Id = 6, Forename = "H.I.", Surname = "McDunnough", Email = "himcdunnough@example.com", IsActive = true,
                DateOfBirth = new DateTime(1996, 11, 01) },
            new User { Id = 7, Forename = "Cameron", Surname = "Poe", Email = "cpoe@example.com", IsActive = false,
                DateOfBirth = new DateTime(1997, 07, 11) },
            new User { Id = 8, Forename = "Edward", Surname = "Malus", Email = "emalus@example.com", IsActive = false,
                DateOfBirth = new DateTime(1993, 06, 12) },
            new User { Id = 9, Forename = "Damon", Surname = "Macready", Email = "dmacready@example.com", IsActive = false,
                DateOfBirth = new DateTime(1995, 01, 15) },
            new User { Id = 10, Forename = "Johnny", Surname = "Blaze", Email = "jblaze@example.com", IsActive = true,
                DateOfBirth = new DateTime(1999, 02, 03) },
            new User { Id = 11, Forename = "Robin", Surname = "Feld", Email = "rfeld@example.com", IsActive = true,
                DateOfBirth = new DateTime(1999, 11, 10) },
        });
    }
       

    public DbSet<User>? Users { get; set; }

    public DbSet<AuditLogs>? AuditLogs { get; set; }

    public IQueryable<TEntity> GetAll<TEntity>() where TEntity : class
        => base.Set<TEntity>();

    public IQueryable<TEntity> GetAll<TEntity>(Func<TEntity, bool> predicate) where TEntity : class
       => base.Set<TEntity>().Where(predicate).AsQueryable();

    public void Create<TEntity>(TEntity entity) where TEntity : class
    {
        base.Add(entity);
        SaveChanges();
    }

    public new void Update<TEntity>(TEntity entity) where TEntity : class
    {
        base.Update(entity);
        SaveChanges();
    }

    public void Delete<TEntity>(TEntity entity) where TEntity : class
    {
        base.Remove(entity);
        SaveChanges();
    }

    public async Task<TEntity?> Get<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
    {
        return await base.Set<TEntity>().FirstOrDefaultAsync(predicate);
    }

    public async Task<TEntity?> Get<TEntity>(Expression<Func<TEntity, bool>> predicate, string includedEntity) where TEntity : class
    {
        return await base.Set<TEntity>().Include(includedEntity).FirstOrDefaultAsync(predicate);
    }
}
