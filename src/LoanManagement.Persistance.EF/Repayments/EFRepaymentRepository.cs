﻿using LoanManagement.Entities;
using LoanManagement.Services.Repayments.Contracts;
using LoanManagement.Services.Repayments.Contracts.DTOs;
using Microsoft.EntityFrameworkCore;

namespace LoanManagement.Persistance.EF.Repayments
{
    public class EFRepaymentRepository : RepaymentRepository
    {
        private readonly EFDbContext _context;

        public EFRepaymentRepository(EFDbContext context)
        {
            _context = context;
        }

        public async Task<Repayment?> FindById(int id)
        {
            return await _context.Repayments.
                FirstOrDefaultAsync(x => x.Id == id);

        }

        public async Task GenerateRepayments(Repayment repayment)
        {
            await _context.Repayments.AddAsync(repayment);
        }

        public async Task<List<GetAllUnpaidRepaymentsDto?>> GetAllUnpaid(
            int loanId)
        {
            return await _context.Repayments.
                Where(x => x.LoanId == loanId && x.IsRepaid == false).
                Select(x => x == null ? null :
                new GetAllUnpaidRepaymentsDto
                {
                    Id = x.Id,
                    DueDate = x.DueDate,
                }).ToListAsync();

        }

        public async Task<List<GetAllHighRiskCustomersDto>>
          GetAllHighRiskCustomers()
        {
            return await _context.Repayments
            .Where(repayment => repayment.LatePenaltyCount > 0)
            .GroupBy(repayment => new { repayment.Loan.CustomerId })
            .Select(group => new GetAllHighRiskCustomersDto
            {
                CustomerId = group.Key.CustomerId,
                TottalLateCount = group.Max(
                    repayment => repayment.LatePenaltyCount)
            })
        .Where(dto => dto.TottalLateCount > 2)
        .ToListAsync();
        }
    }
}
