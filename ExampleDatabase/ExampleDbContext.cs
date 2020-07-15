﻿// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT licence. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore;

namespace ExampleDatabase
{
    public class ExampleDbContext : DbContext
    {
        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<TodoItemHybrid> TodoItemHybrids { get; set; }
        public DbSet<Book> Books { get; set; }

        public ExampleDbContext(DbContextOptions<ExampleDbContext> options)
            : base(options) { }
    }
}