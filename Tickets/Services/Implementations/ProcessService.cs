using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Npgsql;
using System.Data;
using System.Reflection;
using System.Transactions;
using Tickets.Dto;
using Tickets.Infrastructure;
using Tickets.Models;
using Tickets.Services.Interfaces;

namespace Tickets.Services.Implementations
{
    public class ProcessService : IProcessService
    {
        private readonly ApplicationDbContext _context;
        private readonly ISqlStorageService _sqlStorage;
        private const string setLockTimeoutSqlName = "set_lock_timeout.sql";
        private const string updateRefundSegmentsSqlName = "update_refund_segments.sql";
        public ProcessService(ApplicationDbContext context, ISqlStorageService sqlStorage)
        {
            _context = context;
            _sqlStorage = sqlStorage;
        }

        public async Task CreateSegmentsAsync(Segments[] segments)
        {
            string setLockTimeoutSql = _sqlStorage.Queries.FirstOrDefault(t =>
                t.Key.Contains(setLockTimeoutSqlName, StringComparison.CurrentCultureIgnoreCase)).Value;
            using var transaction = await _context.Database.BeginTransactionAsync();
            await _context.Database.ExecuteSqlRawAsync(setLockTimeoutSql);
            for (int i = 0; i < segments.Length; i++)
            {
                var a = await _context.Segments.AddAsync(segments[i]);
                await _context.SaveChangesAsync();
            }
            await transaction.CommitAsync();
        }

        public async Task<bool> RefundSegmentsAsync(RefundRequestDto request)
        {
            bool success = false;
            string updateRefundSegmentsSql = _sqlStorage.Queries.FirstOrDefault(t =>
                t.Key.Contains(updateRefundSegmentsSqlName, StringComparison.CurrentCultureIgnoreCase)).Value;
            NpgsqlParameter operationTimeParam = new NpgsqlParameter("@operation_time", request.OperationTime.UtcDateTime);
            NpgsqlParameter operationTimeTimezoneParam = new NpgsqlParameter("@operation_time_timezone", $"-{request.OperationTime.Offset.Hours}");
            NpgsqlParameter operationPlaceParam = new NpgsqlParameter("@operation_place", request.OperationPlace);
            NpgsqlParameter ticketNumberParam = new NpgsqlParameter("@ticket_number", request.TicketNumber);
            using var transaction = await _context.Database.BeginTransactionAsync();
            int rowAffectedCount = await _context.Database.ExecuteSqlRawAsync(updateRefundSegmentsSql, 
                operationTimeParam, operationTimeTimezoneParam, operationPlaceParam, ticketNumberParam);
            if (rowAffectedCount > 0)
            {
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                success = true;
            }
            return success;
        }
    }
}
