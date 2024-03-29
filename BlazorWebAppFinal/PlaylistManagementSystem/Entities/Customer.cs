﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlaylistManagementSystem.Entities
{
    [Index("SupportRepId", Name = "IFK_CustomersSupportRepId")]
    internal partial class Customer
    {
        public Customer()
        {
            Invoices = new HashSet<Invoice>();
        }

        [Key]
        public int CustomerId { get; set; }
        [Required]
        [StringLength(40)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(20)]
        public string LastName { get; set; }
        [StringLength(80)]
        public string Company { get; set; }
        [StringLength(70)]
        public string Address { get; set; }
        [StringLength(40)]
        public string City { get; set; }
        [StringLength(40)]
        public string State { get; set; }
        [StringLength(40)]
        public string Country { get; set; }
        [StringLength(10)]
        public string PostalCode { get; set; }
        [StringLength(24)]
        public string Phone { get; set; }
        [StringLength(24)]
        public string Fax { get; set; }
        [Required]
        [StringLength(60)]
        public string Email { get; set; }
        public int? SupportRepId { get; set; }

        [ForeignKey("SupportRepId")]
        [InverseProperty("Customers")]
        public virtual Employee SupportRep { get; set; }
        [InverseProperty("Customer")]
        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}