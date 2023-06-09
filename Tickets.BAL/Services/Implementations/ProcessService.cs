﻿using Microsoft.EntityFrameworkCore;
using Npgsql;
using Tickets.BAL.Dto;
using Tickets.DAL.Contexts;
using Tickets.DAL.Models;
using Tickets.BAL.Services.Interfaces;
using Tickets.BAL.Exceptions;
using Microsoft.Extensions.Options;
using Tickets.BAL.Options.Implementations;
using System.Text.RegularExpressions;
/*
* 
* Сервис для работы с базой данных
* 
*/
namespace Tickets.BAL.Services.Implementations
{
    public class ProcessService : IProcessService
    {
        private readonly ApplicationDbContext _context;
        private readonly IOptions<SqlQueries> _sqlStorage;
        private const string conflictErrorMsg = "Database conflict error.";
        private const string databaseTimeoutErrorMsg = "Database timeout error.";
        private const string setLockTimeoutSqlName = "set_lock_timeout.sql";
        private const string updateRefundSegmentsSqlName = "update_refund_segments.sql";
        public ProcessService(ApplicationDbContext context, IOptions<SqlQueries> sqlStorage)
        {
            _context = context;
            _sqlStorage = sqlStorage;
        }

        public async Task CreateSegmentsAsync(Segments[] segments)
        {
            if (segments == null || segments.Length == 0) throw new ArgumentException(nameof(segments));
            string setLockTimeoutSql = _sqlStorage.Value.GetBy(setLockTimeoutSqlName)!.Data;
            using var transaction = await _context.Database.BeginTransactionAsync();
            await _context.Database.ExecuteSqlRawAsync(setLockTimeoutSql);
            for (int i = 0; i < segments.Length; i++)
            {
                await _context.Segments.AddAsync(segments[i]);
                try
                {
                    await _context.SaveChangesAsync();
                }catch (DbUpdateException ex)
                {
                    var inEx = ex.InnerException;
                    if (inEx == null) throw;
                    if (inEx.InnerException is PostgresException inPgEx && inPgEx.SqlState == PostgresErrorCodes.LockNotAvailable)
                    {
                        throw new RequestTimeoutException(databaseTimeoutErrorMsg, inPgEx);
                    }
                    if (inEx is PostgresException pgex && pgex.SqlState == PostgresErrorCodes.UniqueViolation)
                    {
                        throw new ConflictException(conflictErrorMsg, pgex);
                    }
                    throw;
                }
            }
            await transaction.CommitAsync();
        }

        public async Task<bool> RefundSegmentsAsync(RefundRequestDto refundDto)
        {
            if (refundDto == null) throw new ArgumentNullException(nameof(refundDto));
            bool success = false;
            string updateRefundSegmentsSql = _sqlStorage.Value.GetBy(updateRefundSegmentsSqlName)!.Data;
            NpgsqlParameter operationTimeParam = new NpgsqlParameter("@operation_time", refundDto.OperationTime.UtcDateTime);
            NpgsqlParameter operationTimeTimezoneParam = new NpgsqlParameter("@operation_time_timezone", $"-{refundDto.OperationTime.Offset.Hours}");
            NpgsqlParameter operationPlaceParam = new NpgsqlParameter("@operation_place", refundDto.OperationPlace);
            NpgsqlParameter ticketNumberParam = new NpgsqlParameter("@ticket_number", refundDto.TicketNumber);
            int rowAffectedCount = await _context.Database.ExecuteSqlRawAsync(updateRefundSegmentsSql, 
                operationTimeParam, operationTimeTimezoneParam, operationPlaceParam, ticketNumberParam);
            if (rowAffectedCount > 0)
            {
                try
                {
                    await _context.SaveChangesAsync();
                } catch (DbUpdateException ex) {
                    var inEx = ex.InnerException;
                    if (inEx == null) throw;
                    if (inEx.InnerException is PostgresException inPgEx && inPgEx.SqlState == PostgresErrorCodes.LockNotAvailable)
                    {
                        throw new RequestTimeoutException(databaseTimeoutErrorMsg, inPgEx);
                    }
                    if (inEx is PostgresException pgex && pgex.SqlState == PostgresErrorCodes.UniqueViolation)
                    {
                        throw new ConflictException(conflictErrorMsg, pgex);
                    }
                    throw;
                }
                success = true;
            }
            return success;
        }
    }
}
