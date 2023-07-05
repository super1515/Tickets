﻿using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Text.RegularExpressions;
using Tickets.Application.Dto;
using Tickets.Infrastructure;
using Tickets.Infrastructure.Contexts;
using Tickets.Infrastructure.Models;
using Tickets.Infrastructure.Services.Interfaces;
using Tickets.Application.Services.Interfaces;
using System.Net;
using Tickets.Infrastructure.Exceptions;
/*
* 
* Сервис для работы с базой данных
* 
*/
namespace Tickets.Infrastructure.Services.Implementations
{
    public class ProcessService : IProcessService
    {
        private readonly ApplicationDbContext _context;
        private readonly ISqlStorageService _sqlStorage;
        private const string conflictSqlState = "23505";
        private const string timeoutSqlState = "55P03";
        private const string conflictErrorMsg = "Database conflict error.";
        private const string databaseTimeoutErrorMsg = "Database timeout error.";
        private const string ticketNumberPattern = @"^\d{13}$";
        private const string setLockTimeoutSqlName = "set_lock_timeout.sql";
        private const string updateRefundSegmentsSqlName = "update_refund_segments.sql";
        public ProcessService(ApplicationDbContext context, ISqlStorageService sqlStorage)
        {
            _context = context;
            _sqlStorage = sqlStorage;
        }

        public async Task CreateSegmentsAsync(Segments[] segments)
        {
            if (segments == null || segments.Length == 0) throw new ArgumentException(nameof(segments));
            string setLockTimeoutSql = _sqlStorage.Queries.FirstOrDefault(t =>
                t.Key.Contains(setLockTimeoutSqlName, StringComparison.CurrentCultureIgnoreCase)).Value;
            using var transaction = await _context.Database.BeginTransactionAsync();
            await _context.Database.ExecuteSqlRawAsync(setLockTimeoutSql);
            for (int i = 0; i < segments.Length; i++)
            {
                if (!Regex.IsMatch(segments[i].TicketNumber, ticketNumberPattern)) throw new ArgumentException(nameof(segments));
                await _context.Segments.AddAsync(segments[i]);
                try
                {
                    await _context.SaveChangesAsync();
                }catch (DbUpdateException ex)
                {
                    PostgresException pge = ex.InnerException as PostgresException;
                    if (pge == null) throw;
                    if (pge.SqlState == timeoutSqlState)
                    {
                        throw new RequestTimeoutException(databaseTimeoutErrorMsg, pge);
                    }
                    if (pge.SqlState == conflictSqlState)
                    {
                        throw new ConflictException(conflictErrorMsg, pge);
                    }
                    throw;
                }
            }
            await transaction.CommitAsync();
        }

        public async Task<bool> RefundSegmentsAsync(RefundRequestDto refundDto)
        {
            if (refundDto == null) throw new ArgumentNullException(nameof(refundDto));
            if (!Regex.IsMatch(refundDto.TicketNumber, ticketNumberPattern)) throw new ArgumentException(nameof(refundDto));
            bool success = false;
            string updateRefundSegmentsSql = _sqlStorage.Queries.FirstOrDefault(t =>
                t.Key.Contains(updateRefundSegmentsSqlName, StringComparison.CurrentCultureIgnoreCase)).Value;
            NpgsqlParameter operationTimeParam = new NpgsqlParameter("@operation_time", refundDto.OperationTime.UtcDateTime);
            NpgsqlParameter operationTimeTimezoneParam = new NpgsqlParameter("@operation_time_timezone", $"-{refundDto.OperationTime.Offset.Hours}");
            NpgsqlParameter operationPlaceParam = new NpgsqlParameter("@operation_place", refundDto.OperationPlace);
            NpgsqlParameter ticketNumberParam = new NpgsqlParameter("@ticket_number", refundDto.TicketNumber);
            using var transaction = await _context.Database.BeginTransactionAsync();
            int rowAffectedCount = await _context.Database.ExecuteSqlRawAsync(updateRefundSegmentsSql, 
                operationTimeParam, operationTimeTimezoneParam, operationPlaceParam, ticketNumberParam);
            if (rowAffectedCount > 0)
            {
                try
                {
                    await _context.SaveChangesAsync();
                } catch (DbUpdateException ex) {
                    PostgresException pge = ex.InnerException as PostgresException;
                    if (pge == null) throw;
                    if (pge.SqlState == timeoutSqlState)
                    {
                        throw new RequestTimeoutException(databaseTimeoutErrorMsg, pge);
                    }
                    if (pge.SqlState == conflictSqlState)
                    {
                        throw new ConflictException(conflictErrorMsg, pge);
                    }
                    throw;
                }
                await transaction.CommitAsync();
                success = true;
            }
            return success;
        }
    }
}
