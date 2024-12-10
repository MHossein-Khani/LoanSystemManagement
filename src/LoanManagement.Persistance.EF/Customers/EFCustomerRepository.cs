﻿using LoanManagement.Entities;
using LoanManagement.Services.Customers.Contracts;
using Microsoft.EntityFrameworkCore;

namespace LoanManagement.Persistance.EF.Customers
{
    public class EFCustomerRepository : CustomerRepository
    {
        private readonly EFDbContext _context;

        public EFCustomerRepository(EFDbContext context)
        {
            _context = context;
        }

        public async Task Add(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
        }

        public async Task<bool> IsNationalCodeExist(string nationalCode)
        {
            return await _context.Customers.AnyAsync(
                x => x.NationalCode == nationalCode);
        }

        public async Task<bool> IsPhoneNumberExist(string phoneNumber)
        {
            return await _context.Customers.AnyAsync(
                x => x.PhoneNumber == phoneNumber);
        }
    }
}